using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Area("TechnicalOffice")]
    public class ControlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}