using Goober.WebJobs.Abstractions;
using Goober.WebJobs.Parameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class BaseJob : IBaseJobMetrics, IHostedService
    {
        #region fields

        private CancellationTokenSource StoppingCts;

        private Stopwatch _serviceWatch = new Stopwatch();

        #endregion

        #region protected properties

        protected SingleJobParameters Parameters { get; set; }

        protected string ClassName { get; private set; }

        protected IServiceProvider ServiceProvider { get; private set; }

        protected IServiceScopeFactory ServiceScopeFactory { get; private set; }

        protected IConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        #endregion

        #region public properties ISimpleJobMetrics

        public bool IsRunning { get; protected set; }

        public bool IsEnabled { get; protected set; }

        public DateTime? StartDateTime { get; protected set; }

        public DateTime? StopDateTime { get; protected set; }

        public TimeSpan ServiceUpTime => _serviceWatch.Elapsed;

        public bool IsCancellationRequested => StoppingCts?.IsCancellationRequested ?? false;

        #endregion

        #region ctor

        public BaseJob(ILogger logger, IServiceProvider serviceProvider)
        {
            ClassName = this.GetType().Name;
            StoppingCts = new CancellationTokenSource();
            IsRunning = false;
            IsEnabled = false;
            Logger = logger;
            ServiceProvider = serviceProvider;
            ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
        }

        #endregion

        #region IHostedService methods

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsRunning == true)
                return Task.CompletedTask;

            Action<Task> executeAction = null;
            executeAction = async _ignored1 =>
            {
                try
                {
                    await StartNotSafetyAsync();
                }
                catch (Exception exc)
                {
                    Logger.LogError(exception: exc, message: $"Fail to execute {this.GetType().Name}");

                    await Task.Delay(delay: WebJobsGlossary.RetryIntervalOnException,
                                cancellationToken: StoppingCts.Token)
                        .ContinueWith(continuationAction: _ignored2 => executeAction(_ignored2),
                                cancellationToken: StoppingCts.Token);
                }
            };

            Task.Delay(millisecondsDelay: (int)WebJobsGlossary.FirstRunDelayInMilliseconds,
                        cancellationToken: StoppingCts.Token)
                .ContinueWith(continuationAction: executeAction,
                        cancellationToken: StoppingCts.Token);

            return Task.CompletedTask;
        }

        private Task StartNotSafetyAsync()
        {
            LoadJobParametersFromConfiguration(configSectionKey: WebJobsGlossary.ParametersConfigSectionKey);

            if (StoppingCts.IsCancellationRequested == true)
            {
                IsEnabled = false;
            }

            if (IsEnabled == true)
            {
                SetWorkerIsStarted();

                var resTask = ExecuteAsync(StoppingCts.Token);

                return resTask;
            }
            else
            {
                SetWorkerIsStopped();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            SetWorkerIsStopped();

            return Task.CompletedTask;
        }

        #endregion

        #region protected methods

        protected virtual void SetWorkerIsStarted()
        {
            if (StartDateTime.HasValue == true)
                return;

            _serviceWatch.Start();

            StopDateTime = null;
            StartDateTime = DateTime.Now;
            IsRunning = true;
        }

        protected virtual void SetWorkerIsStopped()
        {
            if (StopDateTime.HasValue == true)
                return;

            StoppingCts.Cancel();

            StoppingCts = new CancellationTokenSource();
            _serviceWatch.Stop();

            StartDateTime = null;
            StopDateTime = DateTime.Now;
            IsRunning = false;
        }

        protected virtual void LoadJobParametersFromConfiguration(string configSectionKey)
        {
            var section = Configuration.GetSection(configSectionKey);
            var parameters = section.Get<WebJobsParameters>();
            if (parameters == null)
            {
                throw new InvalidOperationException($"Can't find job configuration parameters by sectionKey = {configSectionKey}");
            }

            Parameters = parameters.Jobs?.FirstOrDefault(x => x.ClassName == ClassName);
            if (Parameters == null)
            {
                Parameters = new SingleJobParameters
                {
                    ClassName = ClassName,
                    IsEnabled = false
                };
            }

            IsEnabled = Parameters.IsEnabled;
        }

        #endregion

        #region abstract methods

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        #endregion
    }
}
