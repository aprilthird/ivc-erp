using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Production.ViewModels.FoldingBudgetWeeklyAdvanceViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/folding-avance-semanal")]
    public class FoldingBudgetWeeklyAdvanceController : BaseController
    {
        public FoldingBudgetWeeklyAdvanceController(IvcDbContext context,
            ILogger<FoldingBudgetWeeklyAdvanceController> logger): base(context, logger)
        {

        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? weeklyAdvanceId = null)
        {
            var query = _context.FoldingBudgetWeeklyAdvances.Where(x => x.WeeklyAdvanceId != null);

            if (weeklyAdvanceId.HasValue)
                query = query.Where(x => x.WeeklyAdvanceId == weeklyAdvanceId);

            var data = await query
                .OrderBy(x=>x.NumberItem)
                .Select(x => new FoldingBudgetWeeklyAdvanceViewModel
                {
                    Id = x.Id,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    Unit = x.Unit,
                    ContractualMO = x.ContractualMO.ToString("N", CultureInfo.InvariantCulture),
                    ContractualEQ = x.ContractualEQ.ToString("N", CultureInfo.InvariantCulture),
                    ContractualSubcontract = x.ContractualSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMaterials = x.ContractualMaterials.ToString("N", CultureInfo.InvariantCulture),
                    ActualAdvance = x.ActualAdvance.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }
        
    }
}
