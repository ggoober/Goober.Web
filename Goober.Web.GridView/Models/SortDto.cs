using Indusoft.Web.GridView.Enums;

namespace Indusoft.Web.GridView.Models
{
    public class SortDto
    {
        /// <summary>
        /// Название свойства.
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Тип сортировки.
        /// </summary>
        public SortTypeEnum SortType { get; set; }
    }
}
