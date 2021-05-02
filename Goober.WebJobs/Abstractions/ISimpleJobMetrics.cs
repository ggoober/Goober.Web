using System;

namespace Goober.WebJobs.Abstractions
{
    public interface ISimpleJobMetrics
    {
        DateTime? StartDateTime { get; }

        DateTime? StopDateTime { get; }

        bool IsEnabled { get; }

        TimeSpan ServiceUpTime { get; }

        bool IsCancellationRequested { get; }
    }
}
