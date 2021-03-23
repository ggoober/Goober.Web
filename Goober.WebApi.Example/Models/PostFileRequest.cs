using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goober.WebApi.Example.Models
{
    public class PostFileRequest
    {
        public int Id { get; set; }

        public IFormFile File { get; set; }
    }
}
