using Microsoft.AspNetCore.Mvc;

namespace Indusoft.Web.Angular.Example.Controllers
{
    public class BaseButtonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
