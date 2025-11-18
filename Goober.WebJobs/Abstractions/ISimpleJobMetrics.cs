using System;

namespace Goober.WebJobs.Abstractions
{
    public interface ISimpleJobMetrics
    {
        long ExecutedCount { get; }

        long SuccessExecutedCount { get; }

        long ErrorExecutedCount { get; }

        DateTime? LastExecutedStartDateTime { get; }

        DateTime? LastExecutedFinishDateTime { get; }

        long? LastExecutedDurationInMilliseconds { get; }

        long? AvgExecutedDurationInMilliseconds { get; }
    }
}
