using System;

namespace Goober.WebJobs.Models
{
    public class WebJobPingModel
    {
        public string Name { get; set; }

        public bool IsRunning { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsCancellationRequested { get; set; }

        public long ServiceUpTimeInSec { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? StopDateTime { get; set; }

        public IterateJobPingModel Iterate { get; set; }

        public ListJobPingModel List { get; set; }
    }
}
