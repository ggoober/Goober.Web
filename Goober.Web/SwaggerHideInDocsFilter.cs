using Goober.Core.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

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
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var controllerActionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor == null)
                    continue;

                var swaggerHideAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(SwaggerHideInDocsAttribute), false);

                if (swaggerHideAttribute.Any() == false)
                    continue;

                var targetAttribute = swaggerHideAttribute.First() as SwaggerHideInDocsAttribute;
                var cookieName = targetAttribute.CookieName;

                if (string.IsNullOrEmpty(cookieName))
                    continue;

                 _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(cookieName, out string password);

                if (targetAttribute.Password == password)
                    continue;

                var key = "/" + apiDescription.RelativePath.TrimEnd('/');
                var pathItem = swaggerDoc.Paths[key];
                if (pathItem == null)
                    continue;

                switch (apiDescription.HttpMethod.ToUpper())
                {
                    case "GET":
                    case "POST":
                    case "PUT":
                    case "DELETE":
                        pathItem.Operations.Clear();
                        break;
                }

                if (pathItem.Operations.Any() == false)
                    swaggerDoc.Paths.Remove(key);
            }
        }
    }
}
