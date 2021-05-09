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

        protected IServiceProvider ServiceProvider { get; private set; }

        protected IServiceScopeFactory ServiceScopeFactory { get; private set; }

        protected IConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        #endregion

        #region public properties ISimpleJobMetrics

        public string ClassName { get; private set; }

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
            ClassName = GetType().FullName;
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

            StoppingCts = new CancellationTokenSource();

            Action<Task> executeAction = null;
            executeAction = async _ignored1 =>
            {
                try
                {
                    await StartNotSafetyAsync(StoppingCts.Token);
                }
                catch (Exception exc)
                {
                    Logger.LogError(exception: exc, message: $"Fail to execute {this.GetType().Name}");

                    if (StoppingCts.IsCancellationRequested == false)
                    {
                        await Task.Delay(delay: WebJobsGlossary.RetryIntervalOnException)
                                  .ContinueWith(continuationAction: _ignored2 => executeAction(_ignored2));
                    }
                }

                SetWorkerIsStopped();
            };

            Task.Delay(millisecondsDelay: WebJobsGlossary.FirstRunDelayInMilliseconds)
                .ContinueWith(continuationAction: executeAction);

            return Task.CompletedTask;
        }

        private async Task StartNotSafetyAsync(CancellationToken cancellationToken)
        {
            LoadJobParametersFromConfiguration(configSectionKey: WebJobsGlossary.ParametersConfigSectionKey);

            if (cancellationToken.IsCancellationRequested == true)
            {
                IsEnabled = false;
            }

            if (IsEnabled == true)
            {
                SetWorkerIsStarted();

                await ExecuteAsync(cancellationToken);
            }
            else
            {
                SetWorkerIsStopped();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            StoppingCts.Cancel();

            return Task.CompletedTask;
        }

        #endregion

        #region protected methods

        protected virtual void SetWorkerIsStarted()
        {
            if (IsRunning == true)
                return;

            _serviceWatch.Start();

            StartDateTime = DateTime.Now;
            StopDateTime = null;

            IsRunning = true;
        }

        protected virtual void SetWorkerIsStopped()
        {
            if (IsRunning == false)
                return;

            _serviceWatch.Stop();

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
