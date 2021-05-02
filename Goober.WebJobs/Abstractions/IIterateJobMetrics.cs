using System;

namespace Goober.WebJobs.Abstractions
{
    public interface IIterateJobMetrics
    {
        TimeSpan TaskDelay { get; }

        long IteratedCount { get; }

        long SuccessIteratedCount { get; }

        DateTime? LastIterationStartDateTime { get; }

        DateTime? LastIterationFinishDateTime { get; }

        long? LastIterationDurationInMilliseconds { get; }

        long? AvgIterationDurationInMilliseconds { get; }
    }
}
