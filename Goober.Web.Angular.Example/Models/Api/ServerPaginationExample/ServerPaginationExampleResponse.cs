using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Models.Api.ServerPaginationExample
{
    public class ServerPaginationExampleResponse
    {
        public int Total { get; set; }
        public List<ServerPaginationExampleDto> Dtos { get; set; } = new List<ServerPaginationExampleDto>();
    }
}
