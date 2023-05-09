using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.EntibadoVariableViewModels;
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
    [Route("oficina-tecnica/variables-entibado")]
    public class EntibadoVariableController : BaseController
    {
        public EntibadoVariableController(IvcDbContext context,
        ILogger<EntibadoVariableController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.EntibadoVariables
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.BudgetInput)
                .Where(x => x.BudgetInput.ProjectId == GetProjectId())
                .Select(x => new EntibadoVariableViewModel
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
                    Name = x.Name,
                    Dimensions = x.Dimensions,
                    LDimension = x.LDimension.ToString("N", CultureInfo.InvariantCulture),
                    HDimension = x.HDimension.ToString("N", CultureInfo.InvariantCulture),
                    Thickness = x.Thickness.ToString("N", CultureInfo.InvariantCulture),
                    Weight =  x.Weight.ToString("N", CultureInfo.InvariantCulture),
                    FreeDitchTope = x.FreeDitchTope.ToString("N", CultureInfo.InvariantCulture),
                    FreeDitchFondo = x.FreeDitchFondo.ToString("N", CultureInfo.InvariantCulture),
                    MaxDitch = x.MaxDitch.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    UseFactor = x.UseFactor.ToString("N", CultureInfo.InvariantCulture),
                    Quantity = x.Quantity,
                    RestatedPerformance = x.RestatedPerformance.ToString("N", CultureInfo.InvariantCulture)
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.EntibadoVariables
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Select(x => new EntibadoVariableViewModel
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
                    Name = x.Name,
                    Dimensions = x.Dimensions,
                    LDimension = x.LDimension.ToString("N", CultureInfo.InvariantCulture),
                    HDimension = x.HDimension.ToString("N", CultureInfo.InvariantCulture),
                    Thickness = x.Thickness.ToString("N", CultureInfo.InvariantCulture),
                    Weight = x.Weight.ToString("N", CultureInfo.InvariantCulture),
                    FreeDitchTope = x.FreeDitchTope.ToString("N", CultureInfo.InvariantCulture),
                    FreeDitchFondo = x.FreeDitchFondo.ToString("N", CultureInfo.InvariantCulture),
                    MaxDitch = x.MaxDitch.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    UseFactor = x.UseFactor.ToString("N", CultureInfo.InvariantCulture),
                    Quantity = x.Quantity,
                    RestatedPerformance = x.RestatedPerformance.ToString("N", CultureInfo.InvariantCulture)
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EntibadoVariableViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var performance = Math.Round(model.LDimension.ToDoubleString() * model.Quantity, 2);

            var variable = new EntibadoVariable
            {
                BudgetInputId = model.BudgetInputId,
                SupplyId = model.SupplyId,
                Name = model.Name,
                Dimensions = model.Dimensions,
                LDimension = model.LDimension.ToDoubleString(),
                HDimension = model.HDimension.ToDoubleString(),
                Thickness = model.Thickness.ToDoubleString(),
                Weight = model.Weight.ToDoubleString(),
                FreeDitchTope = model.FreeDitchTope.ToDoubleString(),
                FreeDitchFondo = model.FreeDitchFondo.ToDoubleString(),
                MaxDitch = model.MaxDitch.ToDoubleString(),
                UnitPrice = model.UnitPrice.ToDoubleString(),
                UseFactor = model.UseFactor.ToDoubleString(),
                Quantity = model.Quantity,
                RestatedPerformance = performance
            };

            await _context.EntibadoVariables.AddAsync(variable);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EntibadoVariableViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var variable = await _context.EntibadoVariables
                .FirstOrDefaultAsync(x => x.Id == id);

            variable.BudgetInputId = model.BudgetInputId;
            variable.SupplyId = model.SupplyId;
            variable.Name = model.Name;
            variable.Dimensions = model.Dimensions;
            variable.LDimension = model.LDimension.ToDoubleString();
            variable.HDimension = model.HDimension.ToDoubleString();
            variable.Thickness = model.Thickness.ToDoubleString();
            variable.Weight = model.Weight.ToDoubleString();
            variable.FreeDitchTope = model.FreeDitchTope.ToDoubleString();
            variable.FreeDitchFondo = model.FreeDitchFondo.ToDoubleString();
            variable.MaxDitch = model.MaxDitch.ToDoubleString();
            variable.UnitPrice = model.UnitPrice.ToDoubleString();
            variable.UseFactor = model.UseFactor.ToDoubleString();
            variable.Quantity = model.Quantity;

            var performance = Math.Round(variable.LDimension * variable.Quantity, 2);

            variable.RestatedPerformance = performance;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var variable = await _context.EntibadoVariables
               .FirstOrDefaultAsync(x => x.Id == id);

            if (variable == null)
                return BadRequest("No se ha encontrado la variable");

            _context.Remove(variable);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
