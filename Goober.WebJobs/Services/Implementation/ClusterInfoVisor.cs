using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Goober.Web.Models;
using Goober.WebJobs.Api.Models;
using Goober.WebJobs.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Goober.WebJobs.Services.Implementation
{
	class ClusterInfoVisor : IClusterInfoVisor
	{
		private static readonly Regex HostRegex = new Regex(@"(?<= *https? *:(//|\\\\))(?<host>\[[0-9:]+\]|[^:]+)(:\d+)?"
			, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);

		private readonly ILogger<IClusterInfoVisor> _logger;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		private readonly ConcurrentDictionary<string, PingResultDesc<PingApiResponse>> _apiPingResults
			= new ConcurrentDictionary<string, PingResultDesc<PingApiResponse>>(StringComparer
				.InvariantCultureIgnoreCase);

		private readonly ConcurrentDictionary<string, PingResultDesc<PingReply>> _hostPingResults
			= new ConcurrentDictionary<string, PingResultDesc<PingReply>>(StringComparer
				.InvariantCultureIgnoreCase);

		public ClusterInfoVisor(ILogger<IClusterInfoVisor> logger, 
			IServiceScopeFactory serviceScopeFactory)
		{
			_logger = logger;
			_serviceScopeFactory = serviceScopeFactory;
		}

		private int _lifetimeResultInMilliseconds = WebJobsGlossary.DefaultLifetimePingResult;

		public int LifetimeResultInMilliseconds
		{
			get => _lifetimeResultInMilliseconds;
			set
			{
				if (value >= 0)
					_lifetimeResultInMilliseconds = value;
			}
		}

		public async Task<List<KeyValuePair<string, PingApiResponse>>> GetApiPingResultsAsync(List<string> apiSchemeAndHosts, CancellationToken cancellationToken, int? pingTimeout, bool isForceUpdateResult = false)
		{
			using (var scope = _serviceScopeFactory.CreateScope())
			{
				if (apiSchemeAndHosts == null)
					throw new ArgumentNullException(nameof(apiSchemeAndHosts));
				var readyToApiPingList =
					apiSchemeAndHosts
						.Where(s => _apiPingResults.ContainsKey(s) == false
						            || _apiPingResults[s].IsExpired(isForceUpdateResult
							            ? LifetimeResultInMilliseconds / 5
							            : LifetimeResultInMilliseconds))
						.ToList();

				var pingResults = await ApiPingAsync(
					scope.ServiceProvider.GetRequiredService<IWebJobsHttpService>(), 
					readyToApiPingList, 
					cancellationToken,
					pingTimeout ?? WebJobsGlossary.DefaultClusterPingTimeoutInMilliseconds, scope.ServiceProvider);

				foreach (var pingResult in pingResults)
				{
					_apiPingResults.AddOrUpdate(pingResult.ApiSchemeAndHost, pingResult,
						(_, desc) => desc.Timestamp > pingResult.Timestamp ? desc : pingResult);
				}

				return pingResults
					.Select(s =>
						new KeyValuePair<string, PingApiResponse>(
							key: s.ApiSchemeAndHost,
							value: _apiPingResults[s.ApiSchemeAndHost].Result))
					.ToList();
			}
		}

		public async Task<List<KeyValuePair<string, PingReply>>> GetHostPingResultsAsync(List<string> apiSchemeAndHosts, CancellationToken cancellationToken, int? pingTimeout, bool isForceUpdateResult = false)
		{
			if (apiSchemeAndHosts == null)
				throw new ArgumentNullException(nameof(apiSchemeAndHosts));
			var readyToHostPingList =
				apiSchemeAndHosts
					.Where(s => _hostPingResults.ContainsKey(s) == false
					            || _hostPingResults[s].IsExpired(isForceUpdateResult
						            ? LifetimeResultInMilliseconds / 5
						            : LifetimeResultInMilliseconds))
					.ToList();

			var pingResults = await HostPingAsync(readyToHostPingList, cancellationToken,
				pingTimeout ?? WebJobsGlossary.DefaultClusterPingTimeoutInMilliseconds);

			foreach (var pingResult in pingResults)
			{
				_hostPingResults.AddOrUpdate(pingResult.ApiSchemeAndHost, pingResult,
					(_, desc) => desc.Timestamp > pingResult.Timestamp ? desc : pingResult);
			}

			return apiSchemeAndHosts
				.Select(s =>
					new KeyValuePair<string, PingReply>(
						key: s,
						value: _hostPingResults[s].Result))
				.ToList();
		}

		private async Task<List<PingResultDesc<PingReply>>> HostPingAsync(IEnumerable<string> apiSchemeAndHosts,
			CancellationToken cancellationToken, int pingTimeout)
		{
			_logger.LogTrace($"Host-ping init...");

			var hostPingTasks = new Dictionary<string, Task<PingReply>>();
			foreach (var node in apiSchemeAndHosts)
			{
				var host = HostRegex.Match(node).Groups["host"].Value;
				hostPingTasks.Add(node, new Ping().SendPingAsync(host, pingTimeout));
			}

			_logger.LogTrace($"Sending host-ping {hostPingTasks.Count} request(s)...");
			await Task.WhenAny(Task.WhenAll(hostPingTasks.Values), Task.Delay(pingTimeout, cancellationToken));
			var result = new List<PingResultDesc<PingReply>>();
			foreach (var hostPingTask in hostPingTasks)
			{
				if (hostPingTask.Value.Status == TaskStatus.RanToCompletion)
					result.Add(new PingResultDesc<PingReply>
					{
						ApiSchemeAndHost = hostPingTask.Key,
						Result = hostPingTask.Value.Result,
						Timestamp = DateTime.Now
					});
				else
				{
					result.Add(new PingResultDesc<PingReply>
					{
						ApiSchemeAndHost = hostPingTask.Key,
						Result = null,
						Timestamp = DateTime.Now
					});
				}
			}
			_logger.LogTrace($"Received host-ping response(s).");

			return result;
		}

		private async Task<List<PingResultDesc<PingApiResponse>>> ApiPingAsync(IWebJobsHttpService webJobsHttpService,
			IEnumerable<string> apiSchemeAndHosts, CancellationToken cancellationToken, int pingTimeout,
			IServiceProvider serviceProvider)
		{
			_logger.LogTrace($"Api-ping init...");

			var apiPingTasks = new Dictionary<string, Task<PingApiResponse>>();
			foreach (var node in apiSchemeAndHosts)
			{
				apiPingTasks.Add(node, PingNodeWithCookie(node, pingTimeout, serviceProvider, webJobsHttpService));
			}

			_logger.LogTrace($"Sending api-ping {apiPingTasks.Count} request(s)...");
			await Task.WhenAny(Task.WhenAll(apiPingTasks.Values), Task.Delay(pingTimeout, cancellationToken));
			var result = new List<PingResultDesc<PingApiResponse>>();
			foreach (var apiPingTask in apiPingTasks)
			{
				if (apiPingTask.Value.Status == TaskStatus.RanToCompletion)
					result.Add(new PingResultDesc<PingApiResponse>
					{
						ApiSchemeAndHost = apiPingTask.Key,
						Result = apiPingTask.Value.Result,
						Timestamp = DateTime.Now
					});
				else
				{
					result.Add(new PingResultDesc<PingApiResponse>
					{
						ApiSchemeAndHost = apiPingTask.Key,
						Result = null,
						Timestamp = DateTime.Now
					});
				}
			}
			_logger.LogTrace($"Received api-ping response(s).");

			return result;
		}

		private async Task<PingApiResponse> PingNodeWithCookie(string apiSchemeAndHost, int pingTimeout, IServiceProvider serviceProvider, IWebJobsHttpService webJobsHttpService)
		{
			PingApiResponse result = null;
			try
			{
				result = await webJobsHttpService.PingAsync(apiSchemeAndHost, pingTimeout);
			}
			catch (WebException ex)
			{
				var cookieSetter = serviceProvider.GetRequiredService<ICookieHttpService>();
				await cookieSetter.SetAsync(apiSchemeAndHost, new SetCookieRequest
				{
					Name = "indusoft",
					Value = Goober.Web.Filters.BasicAuthAttribute.DefaultPassword
				}, pingTimeout);
				result = await webJobsHttpService.PingAsync(apiSchemeAndHost, pingTimeout);
			}
			return result;
		}

		private class PingResultDesc<T>
		{
			public string ApiSchemeAndHost;
			public DateTime Timestamp;
			public T Result;
			public bool IsExpired(int lifeTimeInMilliseconds) 
				=> (DateTime.Now - Timestamp).TotalMilliseconds >= lifeTimeInMilliseconds;
		}
	}

	public class PingStats
	{
		private ConcurrentQueue<PingStatRecord> items 
			= new ConcurrentQueue<PingStatRecord>();

		/// <summary>
		/// Максимальное количество записей статистики
		/// </summary>
		public int MaxQuantityStorage { get; set; } = 1000;

		/// <summary>
		/// Максимальный срок хранения статистики
		/// </summary>
		public TimeSpan MaxPeriodStorage { get; set; } = TimeSpan.MaxValue;

		public PingStats(string apiSchemeAndHost)
		{
			ApiSchemeAndHost = apiSchemeAndHost;
		}

		public string ApiSchemeAndHost { get; }

		public void AddStat(DateTime timestamp, int durationPingMs, bool isSuccess, string message = null)
		{
			var rec = new PingStatRecord
			{
				Timestamp = timestamp,
				DurationMs = durationPingMs,
				IsSuccess = isSuccess,
				Message = message
			};
		}

		private class PingStatRecord
		{
			public int DurationMs { get; set; }
			public DateTime Timestamp { get; set; }
			public bool IsSuccess { get; set; }
			public string Message { get; set; }
		}
	} 
}
