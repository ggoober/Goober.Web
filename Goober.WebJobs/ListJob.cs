using Goober.WebJobs.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Goober.WebJobs
{
    public abstract class ListJob<TItem, TListJobService> : BaseJob, IListJobMetrics, IIterateJobMetrics
        where TListJobService : IListJobService<TItem>
    {
        #region fields

        private long _sumIterationsDurationInMilliseconds;

        private long _lastIterationListItemsSumDurationInMilliseconds;

        private AsyncRetryPolicy _defaultRetryPolicyAsync;

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
        public int TaskDelayInMilliseconds { get; protected set; }

        public long IteratedCount { get; private set; }

        public long SuccessIteratedCount { get; private set; }

        public long ErrorIteratedCount { get; private set; }

        public DateTime? LastIterationStartDateTime { get; private set; }

        public DateTime? LastIterationFinishDateTime { get; private set; }

        public long? LastIterationDurationInMilliseconds { get; private set; }

        public long? AvgIterationDurationInMilliseconds { get; private set; }

        #endregion

        #region public properties IListJobMetrics

        public ushort MaxDegreeOfParallelism { get; protected set; }

        public bool UseSemaphoreParallelism { get; protected set; }

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

        private long _lastIterationListItemsErrorsCount;
        public long LastIterationListItemsErrorsCount => _lastIterationListItemsErrorsCount;

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

        #region BaseJob methods

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                await ExecuteIterationSafetyAsync(cancellationToken);

                await Task.Delay(millisecondsDelay: TaskDelayInMilliseconds);
            }

            SetWorkerIsStopped();
        }

        protected override void LoadJobParametersFromConfiguration(string configSectionKey)
        {
            base.LoadJobParametersFromConfiguration(configSectionKey);

            TaskDelayInMilliseconds = Parameters.IterationDelayInMilliseconds ?? WebJobsGlossary.DefaultIterationDelayInMilliseconds;
            MaxDegreeOfParallelism = Parameters.ListMaxDegreeOfParallelism ?? WebJobsGlossary.DefaultListMaxDegreeOfParallelism;
            UseSemaphoreParallelism = Parameters.UseSemaphoreParallelism ?? false;

            _defaultRetryPolicyAsync = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: WebJobsGlossary.DefaultListItemProcessingRetryCount,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(WebJobsGlossary.DefaultListItemProcessingRetryDelayInMilliseconds));
        }

        #endregion

        private async Task ExecuteIterationSafetyAsync(CancellationToken cancellationToken)
        {
            IteratedCount++;
            LastIterationStartDateTime = DateTime.Now;
            var iterationWatch = new Stopwatch(); ;
            iterationWatch.Start();

            try
            {
                var items = await GetItemsListAsync();

                ResetListMetrics();

                LastIterationListItemsCount = items.Count;

                if (UseSemaphoreParallelism == true)
                {
                    await ProcessListBySemaphoreAsync(items: items, cancellationToken: cancellationToken);
                }
                else
                {
                    ProcessParallel(items: items, cancellationToken: cancellationToken);
                }

                SuccessIteratedCount++;
            }
            catch (Exception exc)
            {
                ErrorIteratedCount++;
                Logger.LogError(exception: exc, message: $"{ClassName} fail to execute iteration");
            }
            finally
            {
                iterationWatch.Stop();
                LastIterationFinishDateTime = DateTime.Now;
                LastIterationDurationInMilliseconds = iterationWatch.ElapsedMilliseconds;
                _sumIterationsDurationInMilliseconds += LastIterationDurationInMilliseconds.Value;
                AvgIterationDurationInMilliseconds = IteratedCount > 0 ? (_sumIterationsDurationInMilliseconds / IteratedCount) : 0;
            }
        }

        private async Task<List<TItem>> GetItemsListAsync()
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TListJobService>() as IListJobService<TItem>;
                if (service == null)
                    throw new InvalidOperationException($"{ClassName} error resolve service: {typeof(TListJobService).Name}");

                return await service.GetItemsAsync();
            }
        }

        private async Task<TItem> ExecuteItemMethodSafety(TItem item, CancellationToken cancellationToken, SemaphoreSlim semaphore = null)
        {
            Interlocked.Increment(ref _lastIterationListItemsProcessedCount);
            Interlocked.Exchange(ref _lastIterationListItemExecuteDateTimeInBinnary, DateTime.Now.ToBinary());

            var itemWatcher = new Stopwatch();
            itemWatcher.Start();

            try
            {
                await _defaultRetryPolicyAsync.ExecuteAsync(
                                () => ExecuteProcessItemWithoutRetryPolicyAsync(item, cancellationToken)
                            );

                Interlocked.Increment(ref _lastIterationListItemsSuccessProcessedCount);
            }
            catch (Exception exc)
            {
                Interlocked.Increment(ref _lastIterationListItemsErrorsCount);
                Logger.LogError(exception: exc, message: $"{ClassName} fail to execute item: {JsonConvert.SerializeObject(item)}");
            }
            finally
            {
                itemWatcher.Stop();
                Interlocked.Exchange(ref _lastIterationListItemsLastDurationInMilliseconds, itemWatcher.ElapsedMilliseconds);
                Interlocked.Add(ref _lastIterationListItemsSumDurationInMilliseconds, _lastIterationListItemsLastDurationInMilliseconds);

                if (semaphore != null)
                {
                    semaphore.Release();
                }
            }

            return item;
        }

        private async Task ExecuteProcessItemWithoutRetryPolicyAsync(TItem item, CancellationToken stoppingToken)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<TListJobService>() as IListJobService<TItem>;
                if (service == null)
                    throw new InvalidOperationException($"{ClassName} fail to get service: {typeof(TListJobService).Name}");

                await service.ProcessItemAsync(item, stoppingToken);
            }
        }

        private void ProcessParallel(List<TItem> items, CancellationToken cancellationToken)
        {
            var query = items
                .AsParallel()
                .WithDegreeOfParallelism(MaxDegreeOfParallelism)
                .WithCancellation(cancellationToken)
                .Select(x => ExecuteItemMethodSafety(item: x,
                                cancellationToken: cancellationToken));

            var res = query.ToList();
        }

        private async Task ProcessListBySemaphoreAsync(List<TItem> items, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            using (var semaphore = new SemaphoreSlim(initialCount: MaxDegreeOfParallelism))
            {
                foreach (var item in items)
                {
                    if (cancellationToken.IsCancellationRequested == true)
                    {
                        break;
                    }

                    await semaphore.WaitAsync();

                    var task = ExecuteItemMethodSafety(
                            item: item,
                            semaphore: semaphore,
                            cancellationToken: cancellationToken);

                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
            }
        }

        protected override void SetWorkerIsStarted()
        {
            base.SetWorkerIsStarted();
        }

        protected override void SetWorkerIsStopped()
        {
            base.SetWorkerIsStopped();
        }

        private void ResetListMetrics()
        {
            LastIterationListItemsCount = 0;
            _lastIterationListItemsErrorsCount = 0;
            _lastIterationListItemsProcessedCount = 0;
            _lastIterationListItemsSuccessProcessedCount = 0;

            _lastIterationListItemsSumDurationInMilliseconds = 0;
            _lastIterationListItemsLastDurationInMilliseconds = 0;
            _lastIterationListItemExecuteDateTimeInBinnary = 0;
        }
    }
}
