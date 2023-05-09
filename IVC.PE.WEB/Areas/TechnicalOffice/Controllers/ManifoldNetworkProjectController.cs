using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/proyecto-colectores-descarga")]
    public class ManifoldNetworkProjectController : BaseController
    {
        public ManifoldNetworkProjectController(IvcDbContext context,
            ILogger<ManifoldNetworkProjectController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        //[HttpGet("formato-excel")]
        //public async Task<IActionResult> GetExcelFormat()
        //{

        //}
    }
}
