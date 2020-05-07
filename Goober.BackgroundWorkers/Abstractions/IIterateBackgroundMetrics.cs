using System;

namespace Goober.BackgroundWorker.Abstractions
{
    public interface IIterateBackgroundMetrics
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
