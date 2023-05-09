using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/protocolo-soldadura")]

    public class WeldingProtocolController : BaseController
    {
        public WeldingProtocolController(IvcDbContext context,
        ILogger<WeldingProtocolController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();
    }
}
