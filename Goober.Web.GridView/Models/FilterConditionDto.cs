using Indusoft.Web.GridView.Enums;

namespace Indusoft.Web.GridView.Models
{
    public class FilterConditionDto
    {
        /// <summary>
        /// Тип отношения.
        /// </summary>
        public FilterEquationTypeEnum Type { get; set; }
        /// <summary>
        /// Фильтрующее значение (или начало диапазона для EquationTypes.InRange)
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// Конец диапазона для EquationTypes.InRange
        /// </summary>
        public string FilterTo { get; set; }
    }
}
