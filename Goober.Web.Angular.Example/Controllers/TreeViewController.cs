using Microsoft.AspNetCore.Mvc;

namespace Indusoft.Web.Angular.Example.Controllers
{
    public class TreeViewController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
