using Goober.WebJobs.Abstractions;
using Goober.WebJobs.Api.Enums;
using Goober.WebJobs.Api.Models;
using Goober.WebJobs.Helpers;
using Goober.WebJobs.Parameters;
using Goober.WebJobs.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class BaseJob : IBaseJobMetrics, IHostedService
    {
        #region fields

        private CancellationTokenSource StoppingCts;

        private IPriorityComparer _priorityComparer;

        private Stopwatch _serviceWatch = new Stopwatch();

        #endregion

        #region protected properties

        protected SingleJobParameters Parameters { get; set; }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }

        protected IConfiguration Configuration { get; }

        protected ILogger Logger { get; }

        public bool IsRunning =>
            State == JobState.Watching || State == JobState.Rollcall || State == JobState.Executing || State == JobState.Starting || State == JobState.Stopping || State == JobState.StoppingExecute;

        public bool IsExecuting => State == JobState.Executing || State == JobState.StoppingExecute;
        #endregion

        #region public properties ISimpleJobMetrics

        public string ClassName { get; }

        public JobState State { get; protected set; }

        public string ReasonForState { get; set; }

        public bool IsEnabled { get; protected set; }

        public bool IsExecuteOnStartup { get; protected set; } = true;

        public DateTime? StartDateTime { get; protected set; }

        public DateTime? StopDateTime { get; protected set; }

        public TimeSpan ServiceUpTime => _serviceWatch.Elapsed;

        public bool IsCancellationRequested => StoppingCts?.IsCancellationRequested ?? false;

        public PriorityCompareType PriorityCompareType => Parameters?.ClusterSettings?.PriorityCompareType ?? PriorityCompareType.None;

        public int ClusterNodePriority => Parameters?.ClusterSettings?.NodePriority ?? 0;

        public bool IsCluster => (Parameters?.ClusterSettings?.Nodes.Count ?? 0) > 0;

        public string SelfApiSchemeAndHost => Parameters?.ClusterSettings?.SelfApiSchemeAndHost;

        public int RetryDelayInMilliseconds { get; set; }

        #endregion

        #region ctor

        protected BaseJob(ILogger logger, IServiceProvider serviceProvider)
        {
            ClassName = GetType().FullName;
            StoppingCts = new CancellationTokenSource();
            State = JobState.Init;
            IsEnabled = false;
            Logger = logger;
            ServiceProvider = serviceProvider;
            ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
        }

        #endregion

        #region IHostedService methods

        public async Task StartAsync(CancellationToken externalCancellationToken)
        {
            LoadJobParametersFromConfiguration(configSectionKey: WebJobsGlossary.ParametersConfigSectionKey);
            if (IsExecuteOnStartup)
                await ForceStartAsync(externalCancellationToken);
        }

        /// <summary>
        /// Безусловный запуск 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ForceStartAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAny(FastStartAsync(cancellationToken), Task.Delay(1, cancellationToken));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace($"Job: {ClassName}. Stop signal received.");
            StoppingCts.Cancel();
        }

        #endregion

        public async Task FastStartAsync(CancellationToken externalCancellationToken)
        {
	        if (IsRunning)
		        return;
	        Logger.LogTrace($"Job: {ClassName}. Starting...");

	        StoppingCts = new CancellationTokenSource();
	        bool isException = false;

	        Action<Task> executeAction = null;
	        executeAction = async (_) =>
	        {
		        using (var serviceScope = ServiceScopeFactory.CreateScope())
			        try
			        {
				        ServiceProvider = serviceScope.ServiceProvider;
				        using (var linkCancel = CancellationTokenSource.CreateLinkedTokenSource(StoppingCts.Token, externalCancellationToken))
					        await StartNotSafetyAsync(linkCancel.Token);
				        isException = false;
			        }
			        catch (Exception exc)
			        {
				        isException = true;
				        Logger.LogError(exception: exc, message: $"Fail to execute {this.GetType().Name}");
				        SetWorkerIsStopped("Job has error(s). See logs for details");
			        }
	        };

	        await Task.Delay(millisecondsDelay: WebJobsGlossary.FirstRunDelayInMilliseconds)
		        .ContinueWith(continuationAction: executeAction);

	        for (var i = 0; i < 3 && StoppingCts.IsCancellationRequested == false && isException; i++)
	        {
		        isException = false;
		        Logger.LogTrace($"Job: {ClassName}. Repeat executing, attempt - {i + 1}");
		        await Task.Delay(delay: TimeSpan.FromMilliseconds(RetryDelayInMilliseconds))
			        .ContinueWith(continuationAction: _ => executeAction(_));
	        }
        }

        #region protected methods


        protected virtual void SetWorkerIsStarted()
        {
            if (IsRunning)
                return;

            SetState(JobState.Starting, "Init job");

            _serviceWatch.Start();

            StartDateTime = DateTime.Now;
            StopDateTime = null;
        }

        protected virtual void SetWorkerIsStopped(string reason)
        {
            SetState(JobState.Stopped, reason);

            if (State == JobState.Stopped)
                return;

            _serviceWatch.Stop();

            StopDateTime = DateTime.Now;
        }

        protected virtual void LoadJobParametersFromConfiguration(string configSectionKey)
        {
            Logger.LogTrace($"Job: {ClassName}. Reading config...");
            var section = Configuration.GetSection(configSectionKey);
            var parameters = section.Get<WebJobsParameters>();
            if (parameters == null)
            {
                throw new InvalidOperationException($"Can't find job configuration parameters by sectionKey = {configSectionKey}");
            }

            Parameters = parameters.Jobs?.FirstOrDefault(x => x.ClassName == ClassName);
            if (Parameters == null)
            {
                Logger.LogTrace($"Job: {ClassName}. Not configured. Creating default values.");
                Parameters = new SingleJobParameters
                {
                    ClassName = ClassName,
                    IsEnabled = false
                };
            }
            if (Parameters.ClusterSettings.PriorityCompareType == PriorityCompareType.None)
                Parameters.ClusterSettings.PriorityCompareType = parameters.ClusterSettings.PriorityCompareType;

            if (Parameters.ClusterSettings.PriorityCompareType == PriorityCompareType.None)
                Parameters.ClusterSettings.PriorityCompareType = PriorityCompareType.AddressPort;

            if (string.IsNullOrEmpty(Parameters.ClusterSettings.SelfApiSchemeAndHost))
                Parameters.ClusterSettings.SelfApiSchemeAndHost = parameters.ClusterSettings.SelfApiSchemeAndHost;

            if (Parameters.ClusterSettings.Nodes.Count == 0)
                Parameters.ClusterSettings.Nodes.AddRange(parameters.ClusterSettings.Nodes);

            Parameters.ClusterSettings.Nodes
                .RemoveAll(node => node.ApiSchemeAndHost.Trim().ToLowerInvariant()
                                   == SelfApiSchemeAndHost.Trim().ToLowerInvariant());

            if (Parameters.ClusterSettings.NodePriority == 0)
                Parameters.ClusterSettings.NodePriority = parameters.ClusterSettings.NodePriority;

            if (Parameters.ClusterSettings.WatchingDelayInMilliseconds <= 0)
                Parameters.ClusterSettings.WatchingDelayInMilliseconds = parameters.ClusterSettings.WatchingDelayInMilliseconds <= 0
                    ? WebJobsGlossary.DefaultClusterWatchingDelayInMilliseconds
                    : parameters.ClusterSettings.WatchingDelayInMilliseconds;

            if (Parameters.ClusterSettings.PingTimeoutInMilliseconds <= 0)
                Parameters.ClusterSettings.PingTimeoutInMilliseconds = parameters.ClusterSettings.PingTimeoutInMilliseconds <= 0
                    ? WebJobsGlossary.DefaultClusterPingTimeoutInMilliseconds
                    : parameters.ClusterSettings.PingTimeoutInMilliseconds;

            if (Parameters.ClusterSettings.DelayBeforeRepeatPingApiInMilliseconds <= 0)
                Parameters.ClusterSettings.DelayBeforeRepeatPingApiInMilliseconds = parameters.ClusterSettings.DelayBeforeRepeatPingApiInMilliseconds <= 0
                    ? WebJobsGlossary.DefaultDelayBeforeRepeatPingApiInMilliseconds
                    : parameters.ClusterSettings.DelayBeforeRepeatPingApiInMilliseconds;

            if (Parameters.ClusterSettings.IsStopExecuteIfLowerPriority == null)
                Parameters.ClusterSettings.IsStopExecuteIfLowerPriority =
                    parameters.ClusterSettings.IsStopExecuteIfLowerPriority ?? WebJobsGlossary.DefaultIsStopExecuteIfLowerPriority;

            IsEnabled = Parameters.IsEnabled;

            IsExecuteOnStartup = Parameters.IsExecuteOnStartup;

            RetryDelayInMilliseconds = Parameters.RetryDelayInMilliseconds ?? (int?)Parameters.RetryDelayTimespan?.TotalMilliseconds ?? WebJobsGlossary.DefaultRetryDelayInMilliseconds;
        }

        protected void SetState(JobState state, string reason = "")
        {
            if (State != state)
                Logger.LogTrace($"Job: {ClassName}. Changing state: {State} => {state}. Reason: `{reason}`");
            else
                Logger.LogTrace($"Job: {ClassName}. Set state reason: `{reason}`");
            ReasonForState = reason;
            State = state;
        }

        #endregion

        #region abstract methods

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
        protected abstract Task ExecuteAwake();

        #endregion

        private async Task StartClusterWatching(CancellationToken externalCancellationToken)
        {
            if (externalCancellationToken.IsCancellationRequested)
                return;
            CancellationTokenSource executingTokenSource = null, linkedCts = null;
            var isExecuting = false;
            while (externalCancellationToken.IsCancellationRequested == false && State != JobState.Stopped)
                try
                {
                    if (externalCancellationToken.IsCancellationRequested)
                        State = JobState.Stopping;
                    switch (State)
                    {
                        case JobState.Starting:
                            SetState(JobState.Rollcall, "Job is starting");
                            if (externalCancellationToken.IsCancellationRequested)
                                return;
                            break;
                        case JobState.Rollcall:
                            Logger.LogTrace($"Job: {ClassName}. Starting rollcall...");
                            await StartRollcall();
                            break;
                        case JobState.Watching:
                            await Task.Delay(Parameters.ClusterSettings.WatchingDelayInMilliseconds, externalCancellationToken);
                            if (externalCancellationToken.IsCancellationRequested)
                                return;
                            SetState(JobState.Rollcall, "Rollcall in watching");
                            break;
                        case JobState.Executing:
                            if (isExecuting == false)
                            {
                                isExecuting = true;
                                executingTokenSource = new CancellationTokenSource();
                                linkedCts = CancellationTokenSource.CreateLinkedTokenSource(externalCancellationToken,
                                    executingTokenSource.Token);

                                await ExecuteAwake();
                                await Task.WhenAny(ExecuteAsync(linkedCts.Token)
                                    .ContinueWith(_ =>
                                    {
                                        executingTokenSource?.Dispose();
                                        executingTokenSource = null;
                                        linkedCts?.Dispose();
                                        linkedCts = null;
                                        isExecuting = false;
                                    }), Task.Delay(1));
                            }
                            await Task.Delay(Parameters.ClusterSettings.MinimalExecutingFrameInMilliseconds, externalCancellationToken);
                            if (externalCancellationToken.IsCancellationRequested)
                                return;
                            await StartRollcall();
                            break;
                        case JobState.StoppingExecute:
                            if (isExecuting)
                                executingTokenSource?.Cancel();
                            else
                                State = JobState.Watching;
                            await Task.Delay(100);
                            break;
                        case JobState.Stopping:
                            if (isExecuting)
                            {
                                executingTokenSource?.Cancel();
                                await Task.Delay(50);
                            }
                            else
                                State = JobState.Stopped;
                            break;
                        case JobState.Stopped:
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (TaskCanceledException ex)
                {
                    Logger.LogTrace($"Job: {ClassName}. Expected task canceled.");
                }

            // перекличка нодов
            async Task StartRollcall()
            {
                var clusterInfo = ServiceProvider.GetRequiredService<IClusterInfoVisor>();

                var apiPingTasks = await clusterInfo.GetApiPingResultsAsync(
                    apiSchemeAndHosts: Parameters.ClusterSettings.Nodes.Select(s => s.ApiSchemeAndHost).ToList(),
                    cancellationToken: externalCancellationToken,
                    pingTimeout: Parameters.ClusterSettings.PingTimeoutInMilliseconds);

                if (externalCancellationToken.IsCancellationRequested)
                    return;

                var completedPing = apiPingTasks.Count(s => s.Value != null);

                if (completedPing == 0)
                {
                    Logger.LogTrace($"Job: {ClassName}. Api-ping failed.");

                    var hostPingTasks = await clusterInfo.GetHostPingResultsAsync(
                        Parameters.ClusterSettings.Nodes.Select(s => s.ApiSchemeAndHost).ToList(),
                        externalCancellationToken,
                        Parameters.ClusterSettings.PingTimeoutInMilliseconds);

                    if (externalCancellationToken.IsCancellationRequested)
                        return;

                    completedPing = hostPingTasks.Count(s => s.Value != null
                                                             && s.Value.Status == IPStatus.Success);
                    if (completedPing == 0)
                    // split-brain guard
                    {
                        Logger.LogTrace($"Job: {ClassName}. Host-ping failed.");
                        SetState(JobState.Watching, "Split-brain guard");
                        return;
                    }

                    await Task.Delay(Parameters.ClusterSettings.DelayBeforeRepeatPingApiInMilliseconds, externalCancellationToken);
                    if (externalCancellationToken.IsCancellationRequested)
                        return;

                    Logger.LogTrace($"Job: {ClassName}. Repeat api-ping.");
                    // повторная попытка api-ping
                    apiPingTasks = await clusterInfo.GetApiPingResultsAsync(
                        apiSchemeAndHosts: Parameters.ClusterSettings.Nodes.Select(s => s.ApiSchemeAndHost).ToList(),
                        cancellationToken: externalCancellationToken,
                        pingTimeout: Parameters.ClusterSettings.PingTimeoutInMilliseconds,
                        isForceUpdateResult: true);
                    if (externalCancellationToken.IsCancellationRequested)
                        return;

                    completedPing = apiPingTasks.Count(s => s.Value != null);
                    if (completedPing == 0)
                    {
                        Logger.LogTrace($"Job: {ClassName}. Repeated api-ping failed. In the end, there can be only one!");
                        SetState(JobState.Executing, "Other nodes not started");
                        return;
                    }
                }

                var apiPingResults = apiPingTasks
                    .Where(s => s.Value != null)
                    .ToDictionary(s => s.Key, s => s.Value.Services.FirstOrDefault(service => service.Name == ClassName));

                if (VerifyApiPingResults(apiPingResults) == false || externalCancellationToken.IsCancellationRequested)
                    return;

                Logger.LogTrace($"Job: {ClassName}. Process rollcall result.");
                if (apiPingResults.All(s => s.Value.IsExecuting == false)
                        && _priorityComparer.OwnerHasPriority(apiPingResults))
                    SetState(JobState.Executing, "Has highest priority");
                else if (isExecuting)
                {
                    if (Parameters.ClusterSettings.IsStopExecuteIfLowerPriority ?? true)
                        SetState(JobState.StoppingExecute, "Has lower priority");
                }
                else
                    SetState(JobState.Watching, "Has lower priority");
            }
        }

        private bool VerifyApiPingResults(Dictionary<string, WebJobPingModel> apiPingResults)
        {
            Logger.LogTrace($"Job: {ClassName}. Verifying api-ping result.");
            var result = true;
            foreach (var pingResult in apiPingResults)
            {
                var remoteJobDesc = pingResult.Value;

                if (remoteJobDesc == null)
                {
                    result = false;
                    Logger.LogError($"Узел '{pingResult.Key}' не содержит службы '{ClassName}'. Кластер не настроен. Служба '{ClassName}' остановлена.");
                    continue;
                }

                if (remoteJobDesc.IsRunning == false)
                {
                    Logger.LogDebug($"Служба '{ClassName}' на узле '{pingResult.Key}' остановлена и не будет учитываться при голосовании.");
                    continue;
                }

                if (remoteJobDesc.IsCluster == false)
                {
                    result = false;
                    Logger.LogError($"Служба '{ClassName}' на узле '{pingResult.Key}' вне кластера. Кластер не настроен. Служба '{ClassName}' остановлена.");
                }

                if (remoteJobDesc.PriorityCompareType != PriorityCompareType)
                {
                    result = false;
                    Logger.LogError($"Служба '{ClassName}' на узле '{pingResult.Key}' имеет отличающиеся настройки приоритетов {remoteJobDesc.PriorityCompareType} != {PriorityCompareType}. Кластер не настроен. Служба '{ClassName}' остановлена.");
                }

                if (PriorityCompareType == PriorityCompareType.NodePriority && remoteJobDesc.ClusterNodePriority == ClusterNodePriority)
                {
                    result = false;
                    Logger.LogError($"Служба '{ClassName}' на узле '{pingResult.Key}' имеет тот же приоритет = '{ClusterNodePriority}'. Кластер не настроен. Служба '{ClassName}' остановлена.");
                }
            }

            if (result == false)
                SetState(JobState.Stopped, "Cluster settings has error(s). See logs for details");
            return result;
        }

        private async Task StartNotSafetyAsync(CancellationToken cancellationToken)
        {
            _priorityComparer = Parameters.ClusterSettings.PriorityCompareType == PriorityCompareType.NodePriority
                ? (IPriorityComparer)new ClusterPriorityComparer(this)
                : new HostAndPortPriorityComparer(this);

            if (cancellationToken.IsCancellationRequested)
            {
                IsEnabled = false;
            }

            if (IsEnabled)
            {
                SetWorkerIsStarted();

                if (IsCluster)
                    await StartClusterWatching(cancellationToken);
                else
                {
                    SetState(JobState.Executing, "Non-cluster start");
                    await ExecuteAsync(cancellationToken);
                }

                SetWorkerIsStopped("Job is completed");
            }
            else
            {
                SetWorkerIsStopped("Job is not enabled");
            }
        }
    }
}
