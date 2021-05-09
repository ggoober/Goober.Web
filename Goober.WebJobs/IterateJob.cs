using Goober.WebJobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class IterateJob<TIterateJobService> : BaseJob, IIterateJobMetrics
        where TIterateJobService : IIterateJobService
    {
        #region fields

        private long _sumIterationsDurationInMilliseconds;

        #endregion

        #region public properties IIterateJobMetrics

        public int TaskDelayInMilliseconds { get; protected set; }

        public long IteratedCount { get; protected set; }

        public long SuccessIteratedCount { get; protected set; }

        public long ErrorIteratedCount { get; protected set; }

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
        }

        #endregion

        #region BaseJob methods


        protected override void SetWorkerIsStarted()
        {
            base.SetWorkerIsStarted();
        }

        protected override void SetWorkerIsStopped()
        {
            base.SetWorkerIsStopped();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                await ExecuteIterationSafetyAsync(cancellationToken);

                await Task.Delay(millisecondsDelay: TaskDelayInMilliseconds, cancellationToken: cancellationToken);
            }

            SetWorkerIsStopped();
        }

        protected override void LoadJobParametersFromConfiguration(string configSectionKey)
        {
            base.LoadJobParametersFromConfiguration(configSectionKey);

            TaskDelayInMilliseconds = Parameters.IterationDelayInMilliseconds ?? WebJobsGlossary.DefaultIterationDelayInMilliseconds;
        }

        #endregion

        private async Task ExecuteIterationSafetyAsync(CancellationToken cancellationToken)
        {
            IteratedCount++;
            var iterationWatch = new Stopwatch();
            iterationWatch.Start();
            LastIterationStartDateTime = DateTime.Now;

            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<TIterateJobService>() as IIterateJobService;
                    if (service == null)
                        throw new InvalidOperationException($"Can't resolve service {typeof(TIterateJobService).Name} for worker {ClassName} iterate ({IteratedCount})");

                    await service.ExecuteIterationAsync(cancellationToken);
                }

                SuccessIteratedCount++;
            }
            catch(Exception exc)
            {
                ErrorIteratedCount++;

                Logger.LogError(exception: exc, message: $"Fail to execute iteration {IteratedCount} on job {ClassName}");
            }
            finally
            {
                iterationWatch.Stop();
                LastIterationFinishDateTime = DateTime.Now;
                LastIterationDurationInMilliseconds = iterationWatch.ElapsedMilliseconds;
                _sumIterationsDurationInMilliseconds += LastIterationDurationInMilliseconds.Value;
                AvgIterationDurationInMilliseconds = _sumIterationsDurationInMilliseconds / IteratedCount;
            }
        }
    }
}
