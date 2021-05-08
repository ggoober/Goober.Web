using System;

namespace Goober.WebJobs.Abstractions
{
    public interface IListJobMetrics
    {
        ushort MaxDegreeOfParallelism { get; }

        public bool UseSemaphoreParallelism { get; }

        long? LastIterationListItemsCount { get; }

        DateTime? LastIterationListItemExecuteDateTime { get; }

        long LastIterationListItemsSuccessProcessedCount { get; }

        long LastIterationListItemsErrorsCount { get; }

        long LastIterationListItemsProcessedCount { get; }

        long LastIterationListItemsAvgDurationInMilliseconds { get; }

        long LastIterationListItemsLastDurationInMilliseconds { get; }
    }
}
