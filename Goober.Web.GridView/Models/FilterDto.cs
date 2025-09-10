using Indusoft.Web.GridView.Enums;

namespace Indusoft.Web.GridView.Models
{
    public class FilterDto
    {
        /// <summary>
        /// Название свойства.
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Тип фильтра
        /// </summary>
        public FilterTypeEnum FilterType { get; set; }
        /// <summary>
        /// Отношение между условиями.
        /// </summary>
        public FilterOperatorEnum? Operator { get; set; }
        /// <summary>
        /// Условия фильтрации.
        /// </summary>
        public FilterConditionDto[] Conditions { get; set; }

    }
}
