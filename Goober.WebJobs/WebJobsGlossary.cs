using System;

namespace Indusoft.WebJobs
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
        /// By default: 5
        /// </summary>
        public static ushort DefaultListItemProcessingRetryCount { get; set; } = 5;

        /// <summary>
        /// By default: 30000 (30 sec)
        /// </summary>
        public static int DefaultListItemProcessingRetryDelayInMilliseconds { get; set; } = 30000;

        /// <summary>
        /// By default: 5 sec
        /// </summary>
        public static TimeSpan RetryIntervalOnException { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// By default: 10000 (10 sec)
        /// </summary>
        public static int DefaultClusterWatchingDelayInMilliseconds { get; set; } = 10000;
        
        /// <summary>
        /// By default: 5000 (5 sec)
        /// </summary>
        public static int DefaultClusterPingTimeoutInMilliseconds { get; set; } = 5000;
        
        /// <summary>
        /// By default: 3000 (3 sec)
        /// </summary>
        public static int DefaultDelayBeforeRepeatPingApiInMilliseconds { get; set; } = 3000;

        /// <summary>
        /// By default: 20000 (20 sec)
        /// </summary>
        public static int DefaultMinimalExecutingFrameInMilliseconds { get; set; } = 20000;

        /// <summary>
        /// By default: false
        /// </summary>
        public static bool DefaultIsStopExecuteIfLowerPriority { get; set; } = false;

        /// <summary>
        /// By default: 5000
        /// </summary>
        public static int DefaultLifetimePingResult { get; set; } = 5000;

        public static int DefaultRetryDelayInMilliseconds { get; set; } = 5000;
    }
}
