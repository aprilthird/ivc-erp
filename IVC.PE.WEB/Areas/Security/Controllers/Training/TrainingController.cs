using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.Controllers.Training
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitacion")]
    public class TrainingController : BaseController
    {
        public TrainingController(IvcDbContext context)
            : base(context)
        {
        }

        public IActionResult Index() => View();
    }
}
