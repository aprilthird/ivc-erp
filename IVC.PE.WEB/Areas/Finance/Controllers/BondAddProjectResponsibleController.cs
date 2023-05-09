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
using IVC.PE.WEB.Areas.Finance.ViewModels.BondAddProjectResponsibleViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using IVC.PE.ENTITIES.UspModels.Finances;

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.FINANCE)]
    [Route("finanzas/responsables")]
    public class BondAddProjectResponsibleController : BaseController
    {
        public BondAddProjectResponsibleController(IvcDbContext context,
                ILogger<BondGuarantorController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Set<UspBondAddProjectResponsible>().FromSqlRaw("execute Finances_uspBondAddProjectResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.BondAddProjectResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var data = new BondAddProjectResponsibleViewModel
            {
                ProjectId = id,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpGet("proyecto")]
        public async Task<IActionResult> GetByProject()
        {
            var projectId = GetProjectId();

            var query = await _context.BondAddProjectResponsibles
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            var data = new BondAddProjectResponsibleViewModel
            {
                ProjectId = projectId,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BondAddProjectResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<BondAddProjectResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new BondAddProjectResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            await _context.BondAddProjectResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(BondAddProjectResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.BondAddProjectResponsibles
                .Where(x => x.ProjectId == model.ProjectId)
                .ToListAsync();

            var responsibles = new List<BondAddProjectResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new BondAddProjectResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            _context.BondAddProjectResponsibles.RemoveRange(responsiblesDb);
            await _context.BondAddProjectResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responsiblesDb = await _context.BondAddProjectResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.BondAddProjectResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
