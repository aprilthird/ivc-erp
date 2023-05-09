using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Controllers
{
    [Route("mapa")]
    public class MapController : Controller
    {
        public IActionResult Index() => View();
    }
}
