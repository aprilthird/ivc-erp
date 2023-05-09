using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.ENTITIES.UspModels.WareHouse;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.Warehouse
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/almacenes/pedidos-campo")]
    public class FieldRequestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public FieldRequestController(IvcDbContext context,
            ILogger<FieldRequestController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string userId,Guid? projectId = null, Guid? budgetTitleId = null, 
            Guid? workFrontId = null, Guid? supplyGroupId = null)
        {
            SqlParameter param1 = new SqlParameter("@GroupId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)supplyGroupId ?? DBNull.Value;


            var data = await _context.Set<UspFieldRequest>().FromSqlRaw("execute Warehouse_uspFieldRequest @GroupId"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            var bps = data
                .Select(x => new FieldRequestResourceModel
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber.ToString("D4"),
                    DeliveryDate = x.DeliveryDate.ToDateString(),
                    Status = x.Status,
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetTitleName = x.BudgetTitleName,
                    FormulaCodes = x.FormulaCodes,
                    ProjectId = x.ProjectId,
                    WorkFrontId = x.WorkFrontId,
                    WorkFrontCode = x.WorkFrontCode,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroupCode = x.SewerGroupCode,

                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontHeadCode = x.WorkFrontHeadCode,

                    UserName = x.UserName,
                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    IssuedUserId = x.IssuedUserId,
                    Observation = x.Observation,
                    WorkOrder = x.WorkOrder

                }).ToList();
            bps = bps.Where(x => x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.PRE_ISSUED && x.IssuedUserId == userId).ToList();

            bps = bps.Where(x => x.IssuedUserId == userId).ToList();
            if (projectId.HasValue)
                bps = bps.Where(x => x.ProjectId == projectId.Value).ToList();
            if (budgetTitleId.HasValue)
                bps = bps.Where(x => x.BudgetTitleId == budgetTitleId).ToList();



            if (workFrontId.HasValue)
                bps = bps.Where(x => x.WorkFrontId == workFrontId).ToList();

            return Ok(bps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var formulas = await _context.FieldRequestProjectFormulas
    .Where(x => x.FieldRequestId == id)
    .Select(x => x.ProjectFormula)
    .ToListAsync();

            var list = new List<Guid>();

            foreach (var f in formulas)
            {
                list.Add(f.Id);
            }

            return Ok(list);
        }

        [HttpPost("parte-padre/registrar")]
        public async Task<IActionResult> CreatePart([FromBody] FieldRequestFatherRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var count = 0;

            var list = new List<FieldRequestProjectFormula>();

            if (_context.SupplyEntries.Count() != 0)
                count = _context.SupplyEntries.OrderBy(x => x.DocumentNumber).Last().DocumentNumber;

             var newField = new FieldRequest
            {
                DocumentNumber = count + 1,
                Status = ConstantHelpers.Warehouse.FieldRequest.Status.PRE_ISSUED,
                DeliveryDate = model.DeliveryDate,
                IssuedUserId = model.IssuedUserId,
                BudgetTitleId = model.BudgetTitleId,
                WorkFrontId = model.WorkFrontId,
                SewerGroupId = model.SewerGroupId,
                //SupplyFamilyId = model.SupplyFamilyId,
                Observation = model.Observation,
                WorkOrder = model.WorkOrder

            };

            foreach (var item in model.ProjectFormulaIds)
            {
                list.Add(new FieldRequestProjectFormula
                {
                    Id = Guid.NewGuid(),
                    FieldRequest = newField,
                    ProjectFormulaId = item
                });
            }

            await _context.FieldRequestProjectFormulas.AddRangeAsync(list);
            await _context.FieldRequests.AddAsync(newField);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPost("parte-padre-item/registrar")]
        public async Task<IActionResult> CreatePartandItem([FromBody] FieldRequestFatherandItemRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var count = 0;

            if (_context.SupplyEntries.Count() != 0)
                count = _context.SupplyEntries.OrderBy(x => x.DocumentNumber).Last().DocumentNumber;

            var newField = new FieldRequest
            {
                DocumentNumber = count + 1,
                Status = ConstantHelpers.Warehouse.FieldRequest.Status.PRE_ISSUED,
                DeliveryDate = DateTime.Now,
                IssuedUserId = model.IssuedUserId,
                BudgetTitleId = model.BudgetTitleId,
                WorkFrontId = model.WorkFrontId,
                SewerGroupId = model.SewerGroupId,
                //SupplyFamilyId = model.SupplyFamilyId,
                Observation = model.Observation,
                WorkOrder = model.WorkOrder

            };


            await _context.FieldRequests.AddAsync(newField);
            await _context.SaveChangesAsync();

            var formulas = new FieldRequestProjectFormula
            {
                ProjectFormulaId = model.ProjectFormulaId,
                FieldRequestId = newField.Id,
            };

            await _context.FieldRequestProjectFormulas.AddAsync(formulas);
            await _context.SaveChangesAsync();

            if (newField.Id != null)
            { if (model.ProjectPhaseId == Guid.Empty)
                    return BadRequest("Falta Ingresar la Fase");
            }
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
            var requestItem = new FieldRequestFolding
            {
                FieldRequestId = newField.Id,
                GoalBudgetInputId = model.GoalBudgetInputId,
                ProjectPhaseId = model.ProjectPhaseId,
                Quantity = model.Quantity.ToDoubleString()
            };
            await _context.FieldRequestFoldings.AddAsync(requestItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("parte-padre-formula/registrar")]
        public async Task<IActionResult> CreateFormulaPart([FromBody] FieldRequestFatherFormulaRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var formulas = new FieldRequestProjectFormula
            {
                ProjectFormulaId = model.ProjectFormulaId,
                FieldRequestId = model.Id,
            };

            await _context.FieldRequestProjectFormulas.AddAsync(formulas);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] FieldRequestFatherRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var formulasdelete = await _context.FieldRequestProjectFormulas.
            Where(x => x.FieldRequestId == id)
            .ToListAsync();

            _context.FieldRequestProjectFormulas.RemoveRange(formulasdelete);

            var request = await _context.FieldRequests
                .FirstOrDefaultAsync(x => x.Id == id);


            var list = new List<FieldRequestProjectFormula>();

            request.BudgetTitleId = model.BudgetTitleId;
           request.WorkFrontId = model.WorkFrontId;
           request.SewerGroupId = model.SewerGroupId;
            request.DeliveryDate = model.DeliveryDate;
           //request.SupplyFamilyId = model.SupplyFamilyId;
           request.Observation = model.Observation;
           request.WorkOrder = model.WorkOrder;

            foreach (var item in model.ProjectFormulaIds)
            {
                list.Add(new FieldRequestProjectFormula
                {
                    Id = Guid.NewGuid(),
                    FieldRequest = request,
                    ProjectFormulaId = item
                });
            }

            await _context.FieldRequestProjectFormulas.AddRangeAsync(list);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = await _context.FieldRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            var items = await _context.FieldRequestFoldings
                .Where(x => x.FieldRequestId == id).ToListAsync();

            var us = await _context.FieldRequestProjectFormulas
                .Where(x => x.FieldRequestId == id).ToListAsync();

            _context.FieldRequestProjectFormulas.RemoveRange(us);
            _context.FieldRequestFoldings.RemoveRange(items);
            _context.FieldRequests.Remove(request);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> UpdateStatusIssued(Guid id, Guid projectId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            var folding = await _context.FieldRequestFoldings
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            if (folding.Count == 0)
                return BadRequest("El pedido no contiene Items");

            var pId = projectId;

            var meta = await _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId
                && folding.Select(y => y.GoalBudgetInputId).Contains(x.Id))
                .ToListAsync();

            var stock = await _context.Stocks
                .Include(x => x.Supply)
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.ISSUED;
            request.IssueDate = DateTime.Today;

            foreach (var item in folding)
            {
                item.ValidatedQuantity = item.Quantity;
            }

            foreach (var item in folding.GroupBy(x => x.GoalBudgetInputId))
            {
                var auxMeta = meta.FirstOrDefault(x => x.Id == item.FirstOrDefault().GoalBudgetInputId);
                if (auxMeta == null)
                    return BadRequest("No se ha encontrado el insumo meta");
                var sum = item.Sum(x => x.Quantity);
                if (auxMeta.WarehouseCurrentMetered < sum)
                    return BadRequest($"Se ha superado el techo de {auxMeta.Supply.Description}" +
                        $" por {Math.Round(sum - auxMeta.WarehouseCurrentMetered, 2)}");
            }

            foreach (var item in folding.GroupBy(x => x.GoalBudgetInput.SupplyId))
            {
                var auxStock = stock.FirstOrDefault(x => x.SupplyId == item.FirstOrDefault().GoalBudgetInput.SupplyId);
                if (auxStock == null)
                    return BadRequest("Este item no puede ingresado porque no tiene stock");
                var sum = item.Sum(x => x.Quantity);
                if (auxStock.Measure < sum)
                    return BadRequest($"Se ha superado el stock de {auxStock.Supply.Description}" +
                        $" por {Math.Round(sum - auxStock.Measure, 2)}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            var foldings = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.Supply.SupplyFamily)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .Include(x => x.GoalBudgetInput.Supply.MeasurementUnit)
                .Include(x => x.ProjectPhase)
                .Where(x => x.FieldRequestId == id)
                .Select(x => new FieldRequestFoldingResourceModel
                {
                    Id = x.Id,
                    SupplyCorrelativeCode = x.GoalBudgetInput.Supply.CorrelativeCode,
                    SupplyFamilyName = x.GoalBudgetInput.Supply.SupplyFamily.Name,
                    SupplyFamilyCode = x.GoalBudgetInput.Supply.SupplyFamily.Code,
                    SupplyGroupName = x.GoalBudgetInput.Supply.SupplyGroup.Name,
                    SupplyGroupCode = x.GoalBudgetInput.Supply.SupplyGroup.Code,
                    SupplyDescription = x.GoalBudgetInput.Supply.Description,
                    MeasurementUnitAbbreviation = x.GoalBudgetInput.Supply.MeasurementUnit.Abbreviation,
                    ProjectPhaseCode = x.ProjectPhase.Code,
                    Quantity = x.Quantity.ToString(CultureInfo.InvariantCulture),
                })
                .ToListAsync();

            return Ok(foldings);
        }

        [HttpPost("detalle/registrar")]
        public async Task<IActionResult> CreateDetailtPart([FromBody] FieldRequestFoldingRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.ProjectPhaseId == Guid.Empty)
                return BadRequest("Falta Ingresar la Fase");

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
            var requestItem = new FieldRequestFolding
            {
                FieldRequestId = model.FieldRequestId,
                GoalBudgetInputId = model.GoalBudgetInputId,
                ProjectPhaseId = model.ProjectPhaseId,
                Quantity = model.Quantity.ToDoubleString()
            };
            await _context.FieldRequestFoldings.AddAsync(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("detalles/editar/{id}")]
        public async Task<IActionResult> EditDetail(Guid id, [FromBody] FieldRequestFoldingRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (model.ProjectPhaseId == Guid.Empty)
                return BadRequest("Falta Ingresar la Fase");

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

            var requestItem = await _context.FieldRequestFoldings
                .FirstOrDefaultAsync(x => x.Id == id);


            requestItem.FieldRequestId = model.FieldRequestId;
            requestItem.GoalBudgetInputId = model.GoalBudgetInputId;
            requestItem.ProjectPhaseId = model.ProjectPhaseId;
            requestItem.Quantity = model.Quantity.ToDoubleString();


      

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("detalles/eliminar/{id}")]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var requestItem = await _context.FieldRequestFoldings
                .FirstOrDefaultAsync(x => x.Id == id);
            if (requestItem == null)
                return BadRequest($"Elemento con Id '{id}' no encontrado.");
            _context.FieldRequestFoldings.Remove(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("insumo-meta/{id}")]
        public async Task<IActionResult> GetGoalBudgetInputRequired(Guid id)
        {
            var data = await _context.GoalBudgetInputs
                .Include(x => x.MeasurementUnit)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return BadRequest("No se ha encontrado el insumo meta");

            var stock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.SupplyId == data.SupplyId);

            var list = new List<string>();

            if (stock != null)
            {
                if (data.WarehouseCurrentMetered >= stock.Measure)
                {
                    list.Add(stock.Measure.ToString("N2", CultureInfo.InvariantCulture));
                    list.Add(stock.Measure.ToString("N2", CultureInfo.InvariantCulture));
                }
                else
                {
                    list.Add(data.WarehouseCurrentMetered.ToString("N2", CultureInfo.InvariantCulture));
                    list.Add(stock.Measure.ToString("N2", CultureInfo.InvariantCulture));
                }
            }
            else
            {
                list.Add(data.WarehouseCurrentMetered.ToString("N2", CultureInfo.InvariantCulture));

                list.Add("no se encontró");
            }

            list.Add(data.WarehouseCurrentMetered.ToString("N2", CultureInfo.InvariantCulture));
            list.Add(data.MeasurementUnit.Abbreviation);

            return Ok(list);
        }
    }
}
