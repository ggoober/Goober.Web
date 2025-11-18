using Goober.WebJobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class SimpleJob<TSimpleJobService> : BaseJob, ISimpleJobMetrics
        where TSimpleJobService : ISimpleJobService
    {
        #region fields

        private long _sumExecutedDurationInMilliseconds;

        #endregion

        #region public properties ISimpleJobMetrics

        public long ExecutedCount { get; protected set; }
        public long SuccessExecutedCount { get; protected set; }
        public long ErrorExecutedCount { get; protected set; }
        public DateTime? LastExecutedStartDateTime { get; protected set; }
        public DateTime? LastExecutedFinishDateTime { get; protected set; }
        public long? LastExecutedDurationInMilliseconds { get; protected set; }
        public long? AvgExecutedDurationInMilliseconds { get; protected set; }

        #endregion

        #region ctor

        protected SimpleJob(ILogger logger,
            IServiceProvider serviceProvider)
            : base(logger, serviceProvider)
        {
        }

        #endregion

        #region BaseJob methods

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace($"Job: {ClassName}. Starting iteration");
            await ExecuteSafetyAsync(cancellationToken);
        }

        protected override async Task ExecuteAwake()
        { }

        #endregion

        private async Task ExecuteSafetyAsync(CancellationToken cancellationToken)
        {
            ExecutedCount++;
            var iterationWatch = new Stopwatch();
            iterationWatch.Start();
            LastExecutedStartDateTime = DateTime.Now;

            try
            {
                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<TSimpleJobService>();
                    if (service == null)
                        throw new InvalidOperationException($"Can't resolve service {typeof(TSimpleJobService).Name} for worker {ClassName} run at ({ExecutedCount}) time");

                    await service.ExecuteAsync(cancellationToken);
                }

                SuccessExecutedCount++;
            }
            catch (Exception exc)
            {
                ErrorExecutedCount++;

                Logger.LogError(exception: exc, message: $"Fail to execute iteration {ExecutedCount} on job {ClassName}");
            }
            finally
            {
                iterationWatch.Stop();
                LastExecutedFinishDateTime = DateTime.Now;
                LastExecutedDurationInMilliseconds = iterationWatch.ElapsedMilliseconds;
                _sumExecutedDurationInMilliseconds += LastExecutedDurationInMilliseconds.Value;
                AvgExecutedDurationInMilliseconds = _sumExecutedDurationInMilliseconds / ExecutedCount;
            }
        }
    }
}
