using System;

namespace Goober.WebJobs.Abstractions
{
    public interface IListJobMetrics
    {
        uint MaxDegreeOfParallelism { get; }

        long? LastIterationListItemsCount { get; }

        DateTime? LastIterationListItemExecuteDateTime { get; }

        long LastIterationListItemsSuccessProcessedCount { get; }

        long LastIterationListItemsProcessedCount { get; }

        long LastIterationListItemsAvgDurationInMilliseconds { get; }

        long LastIterationListItemsLastDurationInMilliseconds { get; }
    }
}
