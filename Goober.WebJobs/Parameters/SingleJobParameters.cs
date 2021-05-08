namespace Goober.WebJobs.Parameters
{
    public class SingleJobParameters
    {
        public bool IsEnabled { get; set; } = false;

        public string ClassName { get; set; }

        public int? IterationDelayInMilliseconds { get; set; }

        public ushort? ListMaxDegreeOfParallelism { get; set; }

        public bool? UseSemaphoreParallelism { get; set; }
    }
}
