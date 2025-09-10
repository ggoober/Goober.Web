using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Goober.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class BasicAuthAttribute : ActionFilterAttribute, IActionFilter
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public static string DefaultLogin { get; set; } = "indusoft";

        public static string DefaultPassword { get; set; }

        public BasicAuthAttribute(string login = null, string password = null)
        {
            Login = login ?? DefaultLogin;

            Password = password ?? DefaultPassword;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(Password != null && context.HttpContext.Request.Cookies.TryGetValue(Login, out var password) && Password == password)
                return;

            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        }

        public override void OnActionExecuted(ActionExecutedContext context) { }
    }
}
