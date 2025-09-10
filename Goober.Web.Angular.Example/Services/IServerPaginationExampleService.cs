using Indusoft.Web.Angular.Example.Models.Api.ServerPaginationExample;
using Indusoft.Web.GridView.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Services
{
    public interface IServerPaginationExampleService
    {
        Task<List<ServerPaginationExampleDto>> GetData(int pageNumber, int pageSize, FilterModelDto filterModel, SortModelDto sortModel);
        Task<int> GetCount(FilterModelDto filterModel);
    }
}
