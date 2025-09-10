using Microsoft.AspNetCore.Mvc;

namespace Indusoft.Web.Angular.Example.Controllers
{
    public class FormController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
