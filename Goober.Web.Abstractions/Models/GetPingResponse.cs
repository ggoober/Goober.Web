using System;

namespace Goober.Web.Models
{
    public class GetPingResponse
    {
        public string MachineName { get; set; }

        public DateTime CurrentDateTime { get; set; }

        public string Version { get; set; }
    }
}
