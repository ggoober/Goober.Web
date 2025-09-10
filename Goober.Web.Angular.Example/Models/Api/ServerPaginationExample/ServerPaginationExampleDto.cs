using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Models.Api.ServerPaginationExample
{
    public class ServerPaginationExampleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int Number { get; set; }
        public bool IsValid { get; set; }
    }
}
