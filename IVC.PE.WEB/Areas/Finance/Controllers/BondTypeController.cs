using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.DATA.Migrations;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;

using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondLoadViewModel;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.FINANCE)]
    [Route("finanzas/tipofianzas")]
    public class BondTypeController : BaseController
    {
        public BondTypeController(IvcDbContext context,
                ILogger<BondTypeController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.BondTypes
              .AsQueryable();

            //  query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new BondTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BondTypes
                .Where(x => x.Id == id)
                .Select(x => new BondTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BondTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bondType = new BondType
            {
                // ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId")),

        

                Name = model.Name
            };
            await _context.BondTypes.AddAsync(bondType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BondTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bondType = await _context.BondTypes.FindAsync(id);

  
            bondType.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondType = await _context.BondTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (bondType == null)
                return BadRequest($"Tipo de Fianza con Id '{id}' no encontrado.");
            _context.BondTypes.Remove(bondType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}