using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxFootageViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("api/oficina-tecnica/metrados/buzones")]
    public class SewerBoxFootageController : BaseController
    {
        public SewerBoxFootageController(IvcDbContext context,
            ILogger<SewerBoxFootageController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> Get(int? type, int? range)
        {
            if (type == null || range == null)
                return BadRequest("No Existe Buzón para la Altura específicada.");

            var sewerBox = await _context.SewerBoxFootageItems
                .Include(x => x.SewerBoxFootage)
                .Where(x => x.SewerBoxFootage.Type == type.Value && x.SewerBoxFootage.Range == range.Value)
                .Select(x => new SewerBoxFootageItemViewModel
                {
                    Id = x.Id,
                    Group = x.Group,
                    Type = x.Type,
                    RealFootage = x.RealFootage,
                    TechnicalRecordFootage = x.TechnicalRecordFootage,
                    SewerBoxFootageId = x.SewerBoxFootageId,
                    SewerBoxFootage = new SewerBoxFootageViewModel
                    {
                        Id = x.SewerBoxFootage.Id,
                        Type = x.SewerBoxFootage.Type,
                        Range = x.SewerBoxFootage.Range
                    }
                })
                .OrderBy(x => x.Group).ThenBy(x => x.Type)
                .ToListAsync();

            return Ok(sewerBox);
        }
    }
}
