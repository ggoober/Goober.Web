using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Indusoft.Web.Angular.Example.Controllers
{
    public class IndexedDbDemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
