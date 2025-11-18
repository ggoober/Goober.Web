using System;

namespace Goober.WebJobs.Api.Models
{
    public class SimpleJobPingModel
    {
        public long? ExecutedCount { get; set; }

        public long? SuccessExecutedCount { get; set; }

        public long? ErrorExecutedCount { get; set; }

        public DateTime? LastExecutedStartDateTime { get; set; }

        public DateTime? LastExecutedFinishDateTime { get; set; }

        public long? LastExecutedDurationInMilliseconds { get; set; }

        public long? AvgExecutedDurationInMilliseconds { get; set; }
    }
}
