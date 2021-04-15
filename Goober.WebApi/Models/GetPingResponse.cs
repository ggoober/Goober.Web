using System;
using System.Collections.Generic;
using System.Text;

namespace Goober.WebApi.Models
{
    public class GetPingResponse
    {
        public string ApplicationName { get; set; }

        public string MachineName { get; set; }

        public string Environment { get; set; }

        public DateTime CurrentDateTime { get; set; }

        public string AssemblyVersion { get; set; }
    }
}
