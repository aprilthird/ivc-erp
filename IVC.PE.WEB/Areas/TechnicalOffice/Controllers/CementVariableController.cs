using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.CementVariableViewModels;
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

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/variables-cemento")]
    public class CementVariableController : BaseController
    {
        public CementVariableController(IvcDbContext context,
       ILogger<CementVariableController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.CementVariables
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.BudgetInput)
                .Where(x => x.BudgetInput.ProjectId == GetProjectId())
                .Select(x => new CementVariableViewModel
                {
                    Id = x.Id,
                    BudgetInputId = x.BudgetInputId,
                    BudgetInput = new BudgetInputViewModel
                    {
                        Code = x.BudgetInput.Code,
                        Description = x.BudgetInput.Description,
                        SaleUnitPrice = x.BudgetInput.SaleUnitPrice.ToString("N", CultureInfo.InvariantCulture)
                    },
                    SupplyId = x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description,
                        SupplyFamilyId = x.Supply.SupplyFamilyId,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = x.Supply.SupplyFamily.Code,
                            Name = x.Supply.SupplyFamily.Name
                        },
                        SupplyGroupId = x.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = x.Supply.SupplyGroup.Code,
                            Name = x.Supply.SupplyGroup.Name
                        },
                        CorrelativeCode = x.Supply.CorrelativeCode
                    },
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture)
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.CementVariables
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Select(x => new CementVariableViewModel
                {
                    Id = x.Id,
                    BudgetInputId = x.BudgetInputId,
                    BudgetInput = new BudgetInputViewModel
                    {
                        Code = x.BudgetInput.Code,
                        Description = x.BudgetInput.Description,
                        SaleUnitPrice = x.BudgetInput.SaleUnitPrice.ToString("N", CultureInfo.InvariantCulture)
                    },
                    SupplyId = x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description,
                        SupplyFamilyId = x.Supply.SupplyFamilyId,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = x.Supply.SupplyFamily.Code,
                            Name = x.Supply.SupplyFamily.Name
                        },
                        SupplyGroupId = x.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = x.Supply.SupplyGroup.Code,
                            Name = x.Supply.SupplyGroup.Name
                        },
                        CorrelativeCode = x.Supply.CorrelativeCode
                    },
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(CementVariableViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var variable = new CementVariable
            {
                BudgetInputId = model.BudgetInputId,
                SupplyId = model.SupplyId,
                UnitPrice = model.UnitPrice.ToDoubleString()
            };

            await _context.CementVariables.AddAsync(variable);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, CementVariableViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var variable = await _context.CementVariables
                .FirstOrDefaultAsync(x => x.Id == id);

            variable.BudgetInputId = model.BudgetInputId;
            variable.SupplyId = model.SupplyId;
            variable.UnitPrice = model.UnitPrice.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var variable = await _context.CementVariables
               .FirstOrDefaultAsync(x => x.Id == id);

            if (variable == null)
                return BadRequest("No se ha encontrado la variable");

            _context.Remove(variable);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
