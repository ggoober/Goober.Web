using System;

namespace Goober.WebJobs.Abstractions
{
    public interface IIterateJobMetrics
    {
        int TaskDelayInMilliseconds { get; }

        long IteratedCount { get; }

        long SuccessIteratedCount { get; }

        long ErrorIteratedCount { get; }

        DateTime? LastIterationStartDateTime { get; }

        DateTime? LastIterationFinishDateTime { get; }

        long? LastIterationDurationInMilliseconds { get; }

        long? AvgIterationDurationInMilliseconds { get; }
    }
}
