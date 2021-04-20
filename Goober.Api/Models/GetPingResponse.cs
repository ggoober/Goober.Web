using System;

namespace Goober.Api.Models
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
