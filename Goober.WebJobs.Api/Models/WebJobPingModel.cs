using System;
using Goober.WebJobs.Api.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Goober.WebJobs.Api.Models
{
    public class WebJobPingModel
    {
        public string Name { get; set; }

        public bool IsRunning { get; set; }

        public bool IsExecuting { get; set; }

        public JobState State { get; set; }

        public string ReasonForState { get; set; }

        public PriorityCompareType PriorityCompareType { get; set; }

        public int ClusterNodePriority { get; set; }

        public bool IsCluster { get; set; }

        public bool IsEnabled { get; set; }

        public int RetryDelayInMilliseconds { get; set; }

        public bool IsCancellationRequested { get; set; }

        public long ServiceUpTimeInSec { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? StopDateTime { get; set; }

        public SimpleJobPingModel Simple { get; set; }

        public IterateJobPingModel Iterate { get; set; }

        public ListJobPingModel List { get; set; }
    }
}
