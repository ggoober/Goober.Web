using System.Linq;
using Goober.Base.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Goober.Web
{
    public class SwaggerHideInDocsFilter : IDocumentFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SwaggerHideInDocsFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach(var apiDescription in context.ApiDescriptions)
            {
                if(apiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
                    continue;

                var swaggerHideAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(SwaggerHideInDocsAttribute), false);
                if(swaggerHideAttribute.Length < 1)
                    continue;

                var targetAttribute = swaggerHideAttribute.First() as SwaggerHideInDocsAttribute;
                var cookieName = targetAttribute.CookieName;

                if(string.IsNullOrEmpty(cookieName))
                    continue;

                if(targetAttribute.Password is not null
                    && _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(cookieName, out var password) == true
                    && targetAttribute.Password == password)
                    continue;

                var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                var pathItem = swaggerDoc.Paths[key];
                if(pathItem == null)
                    continue;

                switch(apiDescription.HttpMethod.ToUpper())
                {
                    case "GET":
                    case "POST":
                    case "PUT":
                    case "DELETE":
                        pathItem.Operations
                                .Clear();
                        break;
                }

                if(pathItem.Operations.Count < 1)
                    swaggerDoc.Paths
                              .Remove(key);
            }
        }
    }
}
