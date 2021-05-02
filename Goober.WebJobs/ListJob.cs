using Goober.WebJobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class ListJob<TItem, TListJobService> : BaseJob, IHostedService, IListJobMetrics, IIterateJobMetrics
        where TListJobService : IListJobService<TItem>
    {
        #region fields

        private long _sumIterationsDurationInMilliseconds;

        private long _lastIterationListItemsSumDurationInMilliseconds;

        #endregion

        #region ctor

        public ListJob(
                ILogger<ListJob<TItem, TListJobService>> logger,
                IServiceProvider serviceProvider)
            : base(logger, serviceProvider)
        {

        }


        #endregion

        #region public properties IIterateJobMetrics

        public virtual TimeSpan TaskDelay { get; private set; }

        public long IteratedCount { get; private set; }

        public long SuccessIteratedCount { get; private set; }

        public DateTime? LastIterationStartDateTime { get; private set; }

        public DateTime? LastIterationFinishDateTime { get; private set; }

        public long? LastIterationDurationInMilliseconds { get; private set; }

        public long? AvgIterationDurationInMilliseconds { get; private set; }

        #endregion

        #region public properties IListJobMetrics

        public uint MaxDegreeOfParallelism { get; protected set; }

        public long? LastIterationListItemsCount { get; private set; }

        private long _lastIterationListItemExecuteDateTimeInBinnary;
        public DateTime? LastIterationListItemExecuteDateTime
        {
            get
            {
                if (_lastIterationListItemExecuteDateTimeInBinnary == 0)
                    return null;

                return DateTime.FromBinary(_lastIterationListItemExecuteDateTimeInBinnary);
            }
        }

        private long _lastIterationListItemsSuccessProcessedCount;
        public long LastIterationListItemsSuccessProcessedCount => _lastIterationListItemsSuccessProcessedCount;


        private long _lastIterationListItemsProcessedCount;
        public long LastIterationListItemsProcessedCount => _lastIterationListItemsProcessedCount;

        public long LastIterationListItemsAvgDurationInMilliseconds
        {
            get
            {
                if (_lastIterationListItemsSuccessProcessedCount == 0)
                    return 0;

                return _lastIterationListItemsSumDurationInMilliseconds / _lastIterationListItemsSuccessProcessedCount;
            }
        }

        private long _lastIterationListItemsLastDurationInMilliseconds;


        public long LastIterationListItemsLastDurationInMilliseconds => _lastIterationListItemsLastDurationInMilliseconds;

        #endregion

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
                    var parameters = GetOptionModelFromConfiguration(WebJobsGlossary.ParametersConfigSectionKey);
                    IsEnabled = parameters.IsEnabled;
                    TaskDelay = TimeSpan.FromMilliseconds(parameters.IterationDelayInMilliseconds ?? WebJobsGlossary.DefaultIterationDelayInMilliseconds);
                    MaxDegreeOfParallelism = parameters.ListMaxDegreeOfParallelism ?? WebJobsGlossary.DefaultListMaxDegreeOfParallelism;

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
                    Logger.LogError(message: $"Error ListJob.ExecuteIteration {this.GetType().Name} iteration ({IteratedCount})",
                        exception: exc);
                }

                Task.Delay(TaskDelay, StoppingCts.Token)
                    .ContinueWith(_ignored2 => repeatAction(_ignored2), StoppingCts.Token);
            };

            Task.Delay((int)WebJobsGlossary.FirstRunDelayInMilliseconds, StoppingCts.Token)
                .ContinueWith(continuationAction: repeatAction, cancellationToken: StoppingCts.Token);

            return Task.CompletedTask;
        }

        private void ExecuteIteration()
        {
            IteratedCount++;
            LastIterationStartDateTime = DateTime.Now;

            var iterationWatch = new Stopwatch(); ;
            iterationWatch.Start();

            var items = GetItemsListAsync();
            LastIterationListItemsCount = items.Count;

            ProcessParallel(items);

            iterationWatch.Stop();
            LastIterationFinishDateTime = DateTime.Now;
            SuccessIteratedCount++;
            LastIterationDurationInMilliseconds = iterationWatch.ElapsedMilliseconds;
            _sumIterationsDurationInMilliseconds += LastIterationDurationInMilliseconds.Value;
            AvgIterationDurationInMilliseconds = _sumIterationsDurationInMilliseconds / SuccessIteratedCount;

            ResetListMetrics();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (StoppingCts.IsCancellationRequested == true)
                return Task.CompletedTask;

            IsRunning = false;

            StoppingCts.Cancel();

            return Task.CompletedTask;
        }

        private List<TItem> GetItemsListAsync()
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TListJobService>() as IListJobService<TItem>;
                if (service == null)
                    throw new InvalidOperationException($"Error resolve service {typeof(TListJobService).Name} ListJob.ExecuteAsync {this.GetType().Name} iteration ({IteratedCount})");

                var task = Task.Run(() => service.GetItemsAsync());

                task.Wait();

                return task.Result;
            }
        }

        private TItem ExecuteItemMethodSafety(TItem item, long iterationId, CancellationToken stoppingToken)
        {
            Interlocked.Increment(ref _lastIterationListItemsProcessedCount);
            Interlocked.Exchange(ref _lastIterationListItemExecuteDateTimeInBinnary, DateTime.Now.ToBinary());

            try
            {
                var itemWatcher = new Stopwatch();
                itemWatcher.Start();

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<TListJobService>() as IListJobService<TItem>;
                    if (service == null)
                        throw new InvalidOperationException($"ListJob.ExecuteItemMethodSafety {this.GetType().Name} iteration ({iterationId}) service {typeof(TListJobService).Name}");


                    var processTask = Task.Run(() => service.ProcessItemAsync(item, stoppingToken));
                    processTask.Wait();
                }

                itemWatcher.Stop();

                Interlocked.Increment(ref _lastIterationListItemsSuccessProcessedCount);
                Interlocked.Exchange(ref _lastIterationListItemsLastDurationInMilliseconds, itemWatcher.ElapsedMilliseconds);
                Interlocked.Add(ref _lastIterationListItemsSumDurationInMilliseconds, _lastIterationListItemsLastDurationInMilliseconds);
            }
            catch (Exception exc)
            {
                this.Logger.LogError(exception: exc, message: $"ListJob.ExecuteItemMethodSafety {this.GetType().Name} iteration ({iterationId}) fail, item: {JsonConvert.SerializeObject(item)}");
            }

            return item;
        }

        private void ProcessParallel(List<TItem> items)
        {
            var query = items
                .AsParallel()
                .WithDegreeOfParallelism((int)MaxDegreeOfParallelism)
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithCancellation(StoppingCts.Token)
                .Select(x => ExecuteItemMethodSafety(item: x,
                                iterationId: IteratedCount,
                                stoppingToken: StoppingCts.Token));

            var res = query.ToList();
        }

        protected override void SetWorkerIsStarted()
        {
            ResetListMetrics();

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

            base.SetWorkerIsStopped();
        }

        private void ResetListMetrics()
        {
            LastIterationListItemsCount = 0;
            _lastIterationListItemsProcessedCount = 0;
            _lastIterationListItemsSuccessProcessedCount = 0;

            _lastIterationListItemsSumDurationInMilliseconds = 0;
            _lastIterationListItemsLastDurationInMilliseconds = 0;
            _lastIterationListItemExecuteDateTimeInBinnary = 0;
        }
    }
}
