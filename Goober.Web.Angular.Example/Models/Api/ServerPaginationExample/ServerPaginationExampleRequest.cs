using Indusoft.Web.GridView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Models.Api.ServerPaginationExample
{
    public class ServerPaginationExampleRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public SortModelDto SortModel { get; set; }
        public FilterModelDto FilterModel { get; set; }
    }
}
