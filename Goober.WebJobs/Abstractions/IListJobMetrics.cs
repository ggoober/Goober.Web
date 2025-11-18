using System;

namespace Goober.WebJobs.Abstractions
{
    public interface IListJobMetrics
    {
        ushort MaxDegreeOfParallelism { get; }

        bool UseSemaphoreParallelism { get; }

        ushort ListItemProcessingRetryCount { get; set; }

        long? LastIterationListItemsCount { get; }

        DateTime? LastIterationListItemExecuteDateTime { get; }

        long LastIterationListItemsSuccessProcessedCount { get; }

        long LastIterationListItemsErrorsCount { get; }

        long LastIterationListItemsProcessedCount { get; }

        long LastIterationListItemsAvgDurationInMilliseconds { get; }

        long LastIterationListItemsLastDurationInMilliseconds { get; }
    }
}
