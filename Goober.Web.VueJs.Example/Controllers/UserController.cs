using Microsoft.AspNetCore.Mvc;

namespace Goober.Web.VueJs.Example.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
