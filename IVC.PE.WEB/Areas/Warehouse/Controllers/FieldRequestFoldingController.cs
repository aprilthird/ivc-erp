using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels;
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

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/folding-pedidos-campo")]
    public class FieldRequestFoldingController : BaseController
    {
        public FieldRequestFoldingController(IvcDbContext context,
               ILogger<FieldRequestFoldingController> logger) : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var techos = await _context.Stocks.ToListAsync();

            var foldings = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.Supply.SupplyFamily)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .Include(x => x.GoalBudgetInput.Supply.MeasurementUnit)
                .Include(x => x.ProjectPhase)
                .Where(x => x.FieldRequestId == id)
                .Select(x => new FieldRequestFoldingViewModel
                {
                    Id = x.Id,
                    GoalBudgetInput = new GoalBudgetInputViewModel
                    {
                        SupplyId = x.GoalBudgetInput.SupplyId,
                        Supply = new SupplyViewModel
                        {
                            CorrelativeCode = x.GoalBudgetInput.Supply.CorrelativeCode,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Name = x.GoalBudgetInput.Supply.SupplyFamily.Name,
                                Code = x.GoalBudgetInput.Supply.SupplyFamily.Code
                            },
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Name = x.GoalBudgetInput.Supply.SupplyGroup.Name,
                                Code = x.GoalBudgetInput.Supply.SupplyGroup.Code
                            },
                            Description = x.GoalBudgetInput.Supply.Description,
                            MeasurementUnit = new MeasurementUnitViewModel
                            {
                                Abbreviation = x.GoalBudgetInput.Supply.MeasurementUnit.Abbreviation
                            }
                        }
                    },
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code + " - " + x.ProjectPhase.Description
                    },
                    Quantity = x.Quantity.ToString(CultureInfo.InvariantCulture),
                    DeliveredQuantity = x.DeliveredQuantity.ToString(CultureInfo.InvariantCulture),
                    ValidatedQuantity = x.ValidatedQuantity.ToString(CultureInfo.InvariantCulture),
                    Techo = x.GoalBudgetInput.WarehouseCurrentMetered.ToString("N2", CultureInfo.InvariantCulture)
                })
                .ToListAsync();

            foreach(var item in foldings)
            {
                var aux = techos.FirstOrDefault(x => x.SupplyId == item.GoalBudgetInput.SupplyId);

                if (aux != null)
                    item.Stock = aux.Measure.ToString("N2", CultureInfo.InvariantCulture);
                else
                    item.Stock = "No se encontró";
            }

            return Ok(foldings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var folding = await _context.FieldRequestFoldings
                .Include(x=>x.GoalBudgetInput.Supply)
                .Select(x => new FieldRequestFoldingViewModel
                {
                    Id = x.Id,
                    FieldRequestId = x.FieldRequestId,
                    GoalBudgetInputId = x.GoalBudgetInputId,
                    GoalBudgetInput = new GoalBudgetInputViewModel
                    {
                        Supply = new SupplyViewModel
                        {
                            SupplyGroupId = x.GoalBudgetInput.Supply.SupplyGroupId
                        }
                    },
                    ProjectPhaseId = x.ProjectPhaseId,
                    Quantity = x.Quantity.ToString(CultureInfo.InvariantCulture)
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(folding);
        }
        
        [HttpPost("crear")]
        public async Task<IActionResult> Create(FieldRequestFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var meta = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == model.GoalBudgetInputId);

            if (meta == null)
                return BadRequest("No se ha encontrado el insumo meta");

            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.SupplyId == meta.SupplyId);

            if (stock == null)
                return BadRequest("Este item no puede ingresado porque no tiene stock");

            var limite = 0.0;
            if (meta.WarehouseCurrentMetered >= stock.Measure)
                limite = stock.Measure;
            else
                limite = meta.WarehouseCurrentMetered;

            if (model.Quantity.ToDoubleString() > limite)
                return BadRequest("Se ha superado el límite en la cantidad");

            var folding = new FieldRequestFolding
            {
                FieldRequestId = model.FieldRequestId,
                GoalBudgetInputId = model.GoalBudgetInputId,
                ProjectPhaseId = model.ProjectPhaseId,
                Quantity = model.Quantity.ToDoubleString()
            };

            await _context.FieldRequestFoldings.AddAsync(folding);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FieldRequestFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var meta = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == model.GoalBudgetInputId);

            if (meta == null)
                return BadRequest("No se ha encontrado el insumo meta");

            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.SupplyId == meta.SupplyId);

            if (stock == null)
                return BadRequest("Este item no puede ingresado porque no tiene stock");

            var limite = 0.0;
            if (meta.WarehouseCurrentMetered >= stock.Measure)
                limite = stock.Measure;
            else
                limite = meta.WarehouseCurrentMetered;

            if (model.Quantity.ToDoubleString() > limite)
                return BadRequest("Se ha superado el límite en la cantidad");

            var folding = await _context.FieldRequestFoldings.FirstOrDefaultAsync(x => x.Id == id);

            folding.GoalBudgetInputId = model.GoalBudgetInputId;
            folding.ProjectPhaseId = model.ProjectPhaseId;
            folding.Quantity = model.Quantity.ToDoubleString();

            await _context.SaveChangesAsync();

            return Ok();
        }
         
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var folding = await _context.FieldRequestFoldings.FirstOrDefaultAsync(x => x.Id == id);

            if (folding == null)
                return BadRequest("No se ha encontrado el item");

            _context.FieldRequestFoldings.Remove(folding);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
