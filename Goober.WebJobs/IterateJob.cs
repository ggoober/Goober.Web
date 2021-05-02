using Goober.WebJobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class IterateJob<TIterateJobService> : BaseJob, IHostedService, IIterateJobMetrics
        where TIterateJobService : IIterateJobService
    {
        #region fields

        private long _sumIterationsDurationInMilliseconds;

        #endregion

        #region public properties IIterateJobMetrics

        public TimeSpan TaskDelay { get; protected set; }

        public long IteratedCount { get; protected set; }

        public long SuccessIteratedCount { get; protected set; }

        public DateTime? LastIterationStartDateTime { get; protected set; }

        public DateTime? LastIterationFinishDateTime { get; protected set; }

        public long? LastIterationDurationInMilliseconds { get; protected set; }

        public long? AvgIterationDurationInMilliseconds { get; protected set; }

        #endregion

        #region ctor

        public IterateJob(ILogger logger, 
            IServiceProvider serviceProvider)
            : base(logger, serviceProvider)
        {
            IsRunning = false;
        }

        #endregion

        #region IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsRunning == true)
                return Task.CompletedTask;

            IsRunning = true;

            StoppingCts = new CancellationTokenSource();

            Action<Task> repeatAction = null;
            repeatAction = _ignored1 =>
            {
                try
                {
                    var parameters = GetOptionModelFromConfiguration(configSectionKey: WebJobsGlossary.ParametersConfigSectionKey);

                    IsEnabled = parameters.IsEnabled;
                    TaskDelay = TimeSpan.FromMilliseconds(parameters.IterationDelayInMilliseconds ?? WebJobsGlossary.DefaultIterationDelayInMilliseconds);

                    if (StoppingCts.IsCancellationRequested == true)
                    {
                        IsEnabled = false;
                    }

                    if (IsEnabled == true)
                    {
                        SetWorkerIsStarted();

                        ExecuteIteration();
                    }
                    else
                    {
                        SetWorkerIsStopped();
                    }
                }
                catch (Exception exc)
                {
                    Logger.LogError(exception: exc,
                        message: $"Error executeIteration for worker {ClassName} iterate ({IteratedCount})");
                }

                Task.Delay(TaskDelay, StoppingCts.Token)
                    .ContinueWith(_ignored2 => repeatAction(_ignored2), StoppingCts.Token);
            };

            Task.Delay((int) WebJobsGlossary.FirstRunDelayInMilliseconds, StoppingCts.Token)
                .ContinueWith(continuationAction: repeatAction, cancellationToken: StoppingCts.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            
            StoppingCts.Cancel();

            return Task.CompletedTask;
        }

        #endregion

        private void ExecuteIteration()
        {
            IteratedCount++;
            var iterationWatch = new Stopwatch();
            iterationWatch.Start();
            LastIterationStartDateTime = DateTime.Now;

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TIterateJobService>() as IIterateJobService;
                if (service == null)
                    throw new InvalidOperationException($"Can't resolve service {typeof(TIterateJobService).Name} for worker {ClassName} iterate ({IteratedCount})");

                var executeTask = Task.Run(() => service.ExecuteIterationAsync(StoppingCts.Token));

                executeTask.Wait();
            }

            iterationWatch.Stop();
            SuccessIteratedCount++;
            LastIterationFinishDateTime = DateTime.Now;
            LastIterationDurationInMilliseconds = iterationWatch.ElapsedMilliseconds;
            _sumIterationsDurationInMilliseconds += LastIterationDurationInMilliseconds.Value;
            AvgIterationDurationInMilliseconds = _sumIterationsDurationInMilliseconds / SuccessIteratedCount;
        }

        protected override void SetWorkerIsStarted()
        {
            base.SetWorkerIsStarted();
        }

        protected override void SetWorkerIsStopped()
        {
            IteratedCount = 0;
            SuccessIteratedCount = 0;

            LastIterationStartDateTime = null;
            LastIterationFinishDateTime = null;

            LastIterationDurationInMilliseconds = 0;
            _sumIterationsDurationInMilliseconds = 0;

            AvgIterationDurationInMilliseconds = 0;
            _sumIterationsDurationInMilliseconds = 0;

            base.SetWorkerIsStopped();
        }
    }
}
