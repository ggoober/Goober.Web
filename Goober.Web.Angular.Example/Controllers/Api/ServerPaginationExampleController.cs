using Indusoft.Web.Angular.Example.Models.Api.ServerPaginationExample;
using Indusoft.Web.Angular.Example.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Controllers.Api
{
    [ApiController]
    public class ServerPaginationExampleController : Controller
    {
        private readonly IServerPaginationExampleService _service;
        public ServerPaginationExampleController(IServerPaginationExampleService service)
        {
            _service = service;
        }

        [HttpPost("api/server-pagination-example/get")]
        public async Task<ServerPaginationExampleResponse> GetDataAsync([FromBody] ServerPaginationExampleRequest request)
        {
            try
            {
                var count = await _service.GetCount(request.FilterModel);
                if (count == 0)
                    return new ServerPaginationExampleResponse();

                var dtos = await _service.GetData(request.PageNumber,
                                                request.PageSize,
                                                request.FilterModel,
                                                request.SortModel);

                return new ServerPaginationExampleResponse()
                {
                    Dtos = dtos,
                    Total = count
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
