using Microsoft.AspNetCore.Mvc;

namespace Goober.Web.VueJs.Example.Controllers
{
    public class EntityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
