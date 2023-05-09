using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Api.Areas.TechnicalOffice
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/oficina-tecnica/metrados/buzones")]
    public class SewerBoxFootageController : BaseController
    {
        public SewerBoxFootageController(IvcDbContext context,
            ILogger<SewerBoxFootageController> logger) : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int sewerboxtype,int sewerboxrange)
        {
            var footage = await _context.SewerBoxFootages
                .Where(x => x.Type == sewerboxtype && x.Range == sewerboxrange)
                .Select(x => new FootageSewerBoxResourceModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    Range = x.Range
                }).FirstOrDefaultAsync();

            if (footage == null)
                return BadRequest("No existe el buzón para la altura específicada.");

            var footageItems = await _context.SewerBoxFootageItems
                .Where(x => x.SewerBoxFootageId == footage.Id)
                .Select(x => new FootageSewerBoxItemResourceModel
                {
                    Id = x.Id,
                    Group = x.Group,
                    Type = x.Type,
                    TypeStr = x.TypeStr,
                    RealFootage = x.RealFootage,
                    TechnicalRecordFootage = x.TechnicalRecordFootage
                }).ToListAsync();

            footage.SewerBoxFootageItems = footageItems;

            return Ok(footage);
        }
    }
}
