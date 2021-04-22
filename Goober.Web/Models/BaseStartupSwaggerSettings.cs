using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Goober.Web.Models
{
    public class BaseStartupSwaggerSettings
    {
        public List<string> XmlCommentsFileNameList { get; set; } = new List<string>();

        public OpenApiInfo OpenApiInfo { get; set; }

        public bool UseHideInDocsFilter { get; set; }
    }
}
