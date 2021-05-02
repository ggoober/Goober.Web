namespace Goober.WebJobs
{
    public static class WebJobsGlossary
    {
        /// <summary>
        /// By default: WebJobs
        /// </summary>
        public static string ParametersConfigSectionKey { get; set; } = "WebJobs";

        /// <summary>
        /// By default: 5000 (5 sec)
        /// </summary>
        public static uint FirstRunDelayInMilliseconds { get; set; } = 5000;

        /// <summary>
        /// By default: 300 000 (5 min)
        /// </summary>
        public static uint DefaultIterationDelayInMilliseconds { get; set; } = 300000;

        public static uint DefaultListMaxDegreeOfParallelism { get; set; } = 1;
    }
}
