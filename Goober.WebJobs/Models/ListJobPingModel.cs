using System;

namespace Goober.WebJobs.Models
{
    public class ListJobPingModel
    {
        public uint MaxDegreeOfParallelism { get; set; }

        public bool UseSemaphoreParallelism { get; set; }

        public long? LastIterationListItemsCount { get; set; }

        public DateTime? LastIterationListItemExecuteDateTime { get; set; }

        public long? LastIterationListItemsSuccessProcessedCount { get; set; }

        public long? LastIterationListItemsProcessedCount { get; set; }

        public long? LastIterationListItemsAvgDurationInMilliseconds { get; set; }

        public long? LastIterationListItemsLastDurationInMilliseconds { get; set; }
    }
}
