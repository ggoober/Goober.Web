using Goober.WebJobs.Abstractions;
using Goober.WebJobs.Parameters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Goober.WebJobs
{
    public abstract class BaseJob : ISimpleJobMetrics
    {
        #region fields

        private Stopwatch _serviceWatch = new Stopwatch();

        #endregion

        #region protected properties

        protected string ClassName { get; private set; }

        protected CancellationTokenSource StoppingCts;

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

            Logger = logger;
            ServiceProvider = serviceProvider;
            ServiceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
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
        }

        protected virtual void SetWorkerIsStopped()
        {
            if (StopDateTime.HasValue == true)
                return;

            StoppingCts = new CancellationTokenSource();
            _serviceWatch.Stop();

            StartDateTime = null;
            StopDateTime = DateTime.Now;
        }

        protected virtual SingleJobParameters GetOptionModelFromConfiguration(string configSectionKey)
        {
            var section = Configuration.GetSection(configSectionKey);
            var parameters = section.Get<WebJobsParameters>();
            if (parameters == null)
            {
                throw new InvalidOperationException($"Can't find job configuration parameters by sectionKey = {configSectionKey}");
            }

            var ret = parameters.Jobs?.FirstOrDefault(x => x.ClassName == ClassName);
            if (ret == null)
            {
                ret = new SingleJobParameters 
                { 
                    ClassName = ClassName, 
                    IsEnabled = false
                };
            }

            return ret;
        }

        #endregion
    }
}
