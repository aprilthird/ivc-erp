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

namespace IVC.PE.WEB.Areas.Aggregation.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Aggregation.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.AGGREGATION)]
    [Route("agregados/vale")]
    public class AggregationVoucherController : BaseController
    {
        public AggregationVoucherController(IvcDbContext context,
            ILogger<AggregationVoucherController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();
    }
}
