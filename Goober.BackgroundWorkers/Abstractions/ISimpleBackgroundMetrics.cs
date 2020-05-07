using System;

namespace Goober.BackgroundWorker.Abstractions
{
    public interface ISimpleBackgroundMetrics
    {
        DateTime? StartDateTime { get; }

        DateTime? StopDateTime { get; }

        bool IsDisabled { get; set; }

        bool IsRunning { get; }

        TimeSpan ServiceUpTime { get; }

        bool IsCancellationRequested { get; }
    }
}
