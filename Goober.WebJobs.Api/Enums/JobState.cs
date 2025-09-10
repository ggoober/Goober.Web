namespace Indusoft.WebJobs.Api.Enums
{
	public enum JobState
	{
		/// <summary>
		/// Инициализация
		/// </summary>
		Init = 0,

		/// <summary>
		/// Начало работы службы
		/// </summary>
		Starting = 10,

		/// <summary>
		/// Перепись узлов
		/// </summary>
		Rollcall = 20,

		/// <summary>
		/// Ожидание и наблюдение
		/// </summary>
		Watching = 30,

		/// <summary>
		/// Выполнение полезной нагрузки службы
		/// </summary>
		Executing = 40,

		/// <summary>
		/// Остановка полезной нагрузки перед переходом в ожидание
		/// </summary>
		StoppingExecute = 50,

		/// <summary>
		/// Остановка полезной нагрузки службы перед остановкой службы
		/// </summary>
		Stopping = 60,

		/// <summary>
		/// Служба остановлена
		/// </summary>
		Stopped = 70
	}
}