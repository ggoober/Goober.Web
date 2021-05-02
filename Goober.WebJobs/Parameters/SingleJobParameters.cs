namespace Goober.WebJobs.Parameters
{
    public class SingleJobParameters
    {
        public bool IsEnabled { get; set; } = false;

        public string ClassName { get; set; }

        public uint? IterationDelayInMilliseconds { get; set; }

        public uint? ListMaxDegreeOfParallelism { get; set; }
    }
}
