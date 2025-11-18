using System.Collections.Generic;
using System;

namespace Goober.WebJobs.Parameters
{
    public class SingleJobParameters: JobParametersBase
    {
        /// <summary>
        /// Флаг активности WebJob
        /// </summary>
	    public bool IsEnabled { get; set; } = false;

	    public string ClassName { get; set; }

        /// <summary>
        /// Таймаут пред запуском следующей итерации IterateJob и ListJob
        /// </summary>
        public int? IterationDelayInMilliseconds { get; set; }

        /// <summary>
        /// Таймаут пред запуском следующей итерации IterateJob и ListJob
        /// </summary>
        public TimeSpan? IterationDelayTimespan { get; set; }

        /// <summary>
        /// Количество потоков выполнения для ListJob
        /// </summary>
        public ushort? ListMaxDegreeOfParallelism { get; set; }

        /// <summary>
        /// Использовать семафор для алгоритма параллелизации ListJob
        /// </summary>
        public bool? UseSemaphoreParallelism { get; set; }

        /// <summary>
        /// Количество повторных попыток запуска в случае ошибки в ListJob
        /// </summary>
        public ushort? ListItemProcessingRetryCount { get; set; }

        /// <summary>
        /// Таймаут перед повторым запуском в случае ошибок при запуске
        /// </summary>
        public int? RetryDelayInMilliseconds { get; set; }

        /// <summary>
        /// Таймаут перед повторым запуском в случае ошибок при запуске
        /// </summary>
        public TimeSpan? RetryDelayTimespan { get; set; }

        /// <summary>
        /// Запуск на старте службы
        /// </summary>
        public bool IsExecuteOnStartup { get; set; } = true;

    }
}
