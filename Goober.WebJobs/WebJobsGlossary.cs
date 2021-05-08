using System;

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
        public static int FirstRunDelayInMilliseconds { get; set; } = 5000;

        /// <summary>
        /// By default: 300 000 (5 min)
        /// </summary>
        public static int DefaultIterationDelayInMilliseconds { get; set; } = 300000;

        /// <summary>
        /// By default: 1
        /// </summary>
        public static ushort DefaultListMaxDegreeOfParallelism { get; set; } = 1;

        /// <summary>
        /// By default: 5 sec
        /// </summary>
        public static TimeSpan RetryIntervalOnException { get; set; } = TimeSpan.FromSeconds(5);
    }
}
