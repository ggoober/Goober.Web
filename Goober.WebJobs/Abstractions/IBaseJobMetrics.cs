using System;

namespace Goober.WebJobs.Abstractions
{
    public interface IBaseJobMetrics
    {
        DateTime? StartDateTime { get; }

        DateTime? StopDateTime { get; }

        bool IsEnabled { get; }

        TimeSpan ServiceUpTime { get; }

        bool IsCancellationRequested { get; }
    }
}
