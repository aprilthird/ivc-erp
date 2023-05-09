using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SteelVariableViewModels;
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
    [Route("oficina-tecnica/variables-acero")]
    public class SteelVariableController : BaseController
    {
        public SteelVariableController(IvcDbContext context, 
            ILogger<SteelVariableController> logger): base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.SteelVariables
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.BudgetInput)
                .Where(x => x.BudgetInput.ProjectId == GetProjectId())
                .Select(x => new SteelVariableViewModel
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
                    RodDiameterInch = x.RodDiameterInch + '"',
                    RodDiameterMilimeters = x.RodDiameterMilimeters.ToString(),
                    Section = x.Section,
                    Perimeter = x.Perimeter.ToString(),
                    NominalWeight = x.NominalWeight.ToString("F3", CultureInfo.InvariantCulture),
                    PricePerRod = x.PricePerRod.ToString("N", CultureInfo.InvariantCulture)
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.SteelVariables
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Select(x => new SteelVariableViewModel
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
                    RodDiameterInch = x.RodDiameterInch,
                    RodDiameterMilimeters = x.RodDiameterMilimeters.ToString(),
                    Section = x.Section,
                    Perimeter = x.Perimeter.ToString(),
                    NominalWeight = x.NominalWeight.ToString(),
                    PricePerRod = x.PricePerRod.ToString()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SteelVariableViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var insumo = await _context.BudgetInputs
                .Include(x => x.SupplyGroup)
                .Where(x => x.ProjectId == GetProjectId())
                .FirstOrDefaultAsync(x => x.SupplyGroup.Name == "ACERO");

            var pricePerRod = Math.Round(model.NominalWeight.ToDoubleString() * ConstantHelpers.Steel.LENGTH * insumo.SaleUnitPrice, 2);

            var variable = new SteelVariable
            {
                SupplyId = model.SupplyId,
                BudgetInputId = model.BudgetInputId,
                RodDiameterInch = model.RodDiameterInch,
                RodDiameterMilimeters = model.RodDiameterMilimeters.ToDoubleString(),
                Section = model.Section,
                Perimeter = model.Perimeter.ToDoubleString(),
                NominalWeight = model.NominalWeight.ToDoubleString(),
                PricePerRod = pricePerRod
            };

            await _context.SteelVariables.AddAsync(variable);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SteelVariableViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var insumo = await _context.BudgetInputs.Include(x => x.SupplyGroup)
                .Where(x => x.ProjectId == GetProjectId()).FirstOrDefaultAsync(x => x.SupplyGroup.Name == "ACERO");

            var pricePerRod = Math.Round(model.NominalWeight.ToDoubleString() * ConstantHelpers.Steel.LENGTH * insumo.SaleUnitPrice, 2);


            var variable = await _context.SteelVariables
                .FirstOrDefaultAsync(x=>x.Id == id);

            variable.SupplyId = model.SupplyId;
            variable.BudgetInputId = model.BudgetInputId;
            variable.RodDiameterInch = model.RodDiameterInch;
            variable.RodDiameterMilimeters = model.RodDiameterMilimeters.ToDoubleString();
            variable.Section = model.Section;
            variable.Perimeter = model.Perimeter.ToDoubleString();
            variable.NominalWeight = model.NominalWeight.ToDoubleString();
            variable.PricePerRod = pricePerRod;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var variable = await _context.SteelVariables
               .FirstOrDefaultAsync(x => x.Id == id);

            if (variable == null)
                return BadRequest("No se ha encontrado la variable");

            _context.Remove(variable);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> UpdatePricePerRod()
        {
            var steels = await _context.SteelVariables.ToListAsync();

            var insumo = await _context.BudgetInputs.Include(x => x.SupplyGroup).FirstOrDefaultAsync(x => x.SupplyGroup.Name == "ACERO");

            foreach (var item in steels)
                item.PricePerRod = Math.Round(item.NominalWeight * ConstantHelpers.Steel.LENGTH * insumo.SaleUnitPrice, 2);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
