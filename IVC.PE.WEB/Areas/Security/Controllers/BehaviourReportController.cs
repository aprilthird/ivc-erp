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

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/comportamientos")]
    public class BehaviourReportController : BaseController
    {
        public BehaviourReportController(IvcDbContext context,
            ILogger<BehaviourReportController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();
    }
}
