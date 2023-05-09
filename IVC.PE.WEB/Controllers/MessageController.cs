using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Controllers
{
    [Route("mensaje")]
    [AllowAnonymous]
    public class MessageController : Controller
    {
        public IActionResult Index(string title, string message, string? icon = null)
        {
            return View(model: (title, message, icon ?? Url.Content("~/media/info.png")));
        }

        [HttpGet("error")]
        public IActionResult Error(string message)
        {
            return View("Index", ("Error", message, Url.Content("~/media/error.png")));
        }

        [HttpGet("pre-requerimiento/observacion")]
        public IActionResult PreRequestObs(string title, string message, string id, string? icon = null)
        {
            return View(model: (title, message, id, icon ?? Url.Content("~/media/file_rejected.png")));
        }

        [HttpGet("observacion-agregada")]
        public IActionResult ObservationSent() => View();

        [HttpGet("orden/observacion")]
        public IActionResult OrderObs(string title, string message, string id, string? icon = null)
        {
            return View(model: (title, message, id, icon ?? Url.Content("~/media/file_rejected.png")));
        }
    }
}
