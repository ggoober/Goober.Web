using Microsoft.AspNetCore.Mvc;

namespace Indusoft.Web.Angular.Example.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
