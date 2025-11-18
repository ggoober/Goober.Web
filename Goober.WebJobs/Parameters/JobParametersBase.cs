using System.Collections.Generic;
using Goober.WebJobs.Api.Enums;

namespace Goober.WebJobs.Parameters
{
	public abstract class JobParametersBase
	{
		public ClusterSettings ClusterSettings { get; set; } = new ClusterSettings();
	}

	public class ClusterSettings
	{
		public List<ClusterNode> Nodes { get; set; } = new List<ClusterNode>();

		/// <summary>
		/// Метод сравнения приоритета в кластере
		/// </summary>
		public PriorityCompareType PriorityCompareType { get; set; }

		/// <summary>
		/// Собственный протокол, адрес и порт
		/// </summary>
		public string SelfApiSchemeAndHost { get; set; }

		/// <summary>
		/// Приоритет узла в кластере
		/// </summary>
		public int NodePriority { get; set; }

		/// <summary>
		/// Задержка в состоянии ожидания в составе кластера
		/// </summary>
		public int WatchingDelayInMilliseconds { get; set; }

		/// <summary>
		/// Таймаут для пинга узлов в составе кластера
		/// </summary>
		public int PingTimeoutInMilliseconds { get; set; }

		/// <summary>
		/// Задержка перед повторной попыткой пинга узлов в составе кластера
		/// </summary>
		public int DelayBeforeRepeatPingApiInMilliseconds { get; set; }

		/// <summary>
		/// Останавливать выполнение полезной нагрузки, если есть более высокоуровневый узел
		/// </summary>
		public bool? IsStopExecuteIfLowerPriority { get; set; }

		/// <summary>
		/// Окно выполнения без смены состояния
		/// </summary>
		public int MinimalExecutingFrameInMilliseconds { get; set; } =
			WebJobsGlossary.DefaultMinimalExecutingFrameInMilliseconds;
    }
}