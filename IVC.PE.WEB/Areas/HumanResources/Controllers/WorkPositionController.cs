using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    public class WorkPositionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
