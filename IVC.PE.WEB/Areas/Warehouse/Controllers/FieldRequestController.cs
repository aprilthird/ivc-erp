using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Tables;
using Spire.Pdf.Grid;
using System.Net;
using System.Drawing.Imaging;
using IVC.PE.ENTITIES.UspModels.WareHouse;
using Microsoft.Data.SqlClient;
using IVC.PE.WEB.Services;
using IVC.PE.WEB.Options;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/pedidos-campo")]
    public class FieldRequestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public FieldRequestController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<FieldRequestController> logger)
            : base(context, userManager, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? budgetTitleId = null, Guid? projectFormulaId = null,
            Guid? workFrontId = null, Guid? sewerGroupId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var foldings = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .ToListAsync();

            var query = _context.FieldRequests
                .Include(x => x.BudgetTitle)
                //.Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                //.Include(x => x.Warehouse)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.SewerGroup)
                .Where(x => x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.PRE_ISSUED
                && x.WorkFront.ProjectId == GetProjectId());

            var formulas = await _context.FieldRequestProjectFormulas
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);

            //if (projectFormulaId.HasValue)
            //  query = query.Where(x => x.ProjectFormulaId == projectFormulaId);

            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId);

            //if (supplyFamilyId.HasValue)
              //  query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);

            var data = new List<FieldRequestViewModel>();

            foreach (var item in query)
            {
                var folding = foldings.Where(x => x.FieldRequestId == item.Id).ToList();
                var grupo = folding.Where(x => x.GoalBudgetInput.Supply.SupplyGroupId == supplyGroupId);

                if (supplyGroupId.HasValue && grupo == null)
                    continue;

                data.Add(new FieldRequestViewModel
                {
                    Id = item.Id,
                    DocumentNumber = item.DocumentNumber.ToString("D4"),
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = item.BudgetTitle.Name
                    },
                    DeliveryDate = item.DeliveryDate.ToDateString(),
                    //ProjectFormula = new ProjectFormulaViewModel
                    //{
                    //    Name = item.ProjectFormula.Code + " - " + item.ProjectFormula.Name
                    //},
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = item.WorkFront.Code
                    },
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = item.SewerGroup.Code
                    },
                    //Warehouse = new WarehouseViewModel
                    //{
                    //    Address = item.Warehouse.Address
                    //},
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = item.SupplyFamily.Name
                    //},
                    Groups = string.Join("-",
                   folding.GroupBy(x => x.GoalBudgetInput.Supply.SupplyGroupId)
                    .Select(y => y.FirstOrDefault().GoalBudgetInput.Supply.SupplyGroup.Name)),
                    Formulas = string.Join("/", formulas.Where(x => x.FieldRequestId == item.Id)
                    .Select(x => x.ProjectFormula.Code).ToList()),
                    UserName = users.FirstOrDefault(x => x.Id == item.IssuedUserId).FullName
                });
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var field = await _context.FieldRequests
                .Include(x => x.SewerGroup)
                .Include(x => x.WorkFront)
                .FirstOrDefaultAsync(x => x.Id == id);
            var formulas = await _context.FieldRequestProjectFormulas
                .Where(x => x.FieldRequestId == id)
                .Select(x => x.ProjectFormula)
                .ToListAsync();

            var data = await _context.FieldRequests
               .Select(x => new FieldRequestViewModel
               {
                   Id = x.Id,
                   DeliveryDate = x.DeliveryDate.ToDateString(),
                   //ProjectFormulaId = x.ProjectFormulaId,
                   BudgetTitleId = x.BudgetTitleId,
                   WorkFrontId = x.WorkFrontId,
                   SewerGroupId = x.SewerGroupId,
                   //WarehouseId = x.WarehouseId,
                   //SupplyFamilyId = x.SupplyFamilyId,
                   Status = x.Status,
                   Observation = x.Observation,
                   ProjectFormulaIds = formulas.Select(x => x.Id).ToList(),
                   WorkOrder = x.WorkOrder,
                   WorkFrontStr = field.WorkFront.Code,
                   SewerGroupStr = field.SewerGroup.Code,
                   Formulas = string.Join("/", formulas.Select(x => x.Code))
               }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FieldRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var count = 0;

            var list = new List<FieldRequestProjectFormula>();

            var a = GetUserId();

            if (_context.FieldRequests.Count() != 0)
                count = _context.FieldRequests.OrderBy(x => x.DocumentNumber).Last().DocumentNumber;

            var request = new FieldRequest
            {
                DocumentNumber = count + 1,
                BudgetTitleId = model.BudgetTitleId,
                DeliveryDate = model.DeliveryDate.ToDateTime(),
                //ProjectFormulaId = model.ProjectFormulaId,
                WorkFrontId = model.WorkFrontId,
                SewerGroupId = model.SewerGroupId,
                //WarehouseId = model.WarehouseId,
                //SupplyFamilyId = model.SupplyFamilyId,
                Status = ConstantHelpers.Warehouse.FieldRequest.Status.PRE_ISSUED,
                Observation = model.Observation,
                IssuedUserId = a,
                WorkOrder = model.WorkOrder
            };

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
            await _context.FieldRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return Ok(request.Id);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FieldRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.FieldRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            var list = new List<FieldRequestProjectFormula>();

            var formulas = await _context.FieldRequestProjectFormulas
                .ToListAsync();

            _context.FieldRequestProjectFormulas.RemoveRange(formulas);

            request.BudgetTitleId = model.BudgetTitleId;
            request.DeliveryDate = model.DeliveryDate.ToDateTime();
            //request.ProjectFormulaId = model.ProjectFormulaId;
            request.WorkFrontId = model.WorkFrontId;
            request.SewerGroupId = model.SewerGroupId;
            //request.WarehouseId = model.WarehouseId;
            //request.SupplyFamilyId = model.SupplyFamilyId;
            request.Observation = model.Observation;

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.FieldRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            var folding = await _context.FieldRequestFoldings
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            var formulas = await _context.FieldRequestProjectFormulas.
                Where(x => x.FieldRequestId == id)
                .ToListAsync();

            var storage = new CloudStorageService(_storageCredentials);
            if (request.FileUrl != null)
                await storage.TryDelete(request.FileUrl, ConstantHelpers.Storage.Containers.WAREHOUSE);

            if (request == null)
                return BadRequest("No se ha encontrado el pedido de campo");

            _context.FieldRequestProjectFormulas.RemoveRange(formulas);
            _context.FieldRequestFoldings.RemoveRange(folding);
            _context.FieldRequests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> UpdateStatusIssued(Guid id)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            var folding = await _context.FieldRequestFoldings
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            if (folding.Count == 0)
                return BadRequest("El pedido no contiene Items");

            var pId = GetProjectId();

            var meta = await _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId
                && folding.Select(y => y.GoalBudgetInputId).Contains(x.Id))
                .ToListAsync();

            var stock = await _context.Stocks
                .Include(x => x.Supply)
                .Where(x=>x.ProjectId == pId)
                .ToListAsync();

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.ISSUED;
            request.IssueDate = DateTime.Today;

            foreach (var item in folding)
            {
                item.ValidatedQuantity = item.Quantity;
            }

            foreach(var item in folding.GroupBy(x => x.GoalBudgetInputId))
            {
                var auxMeta = meta.FirstOrDefault(x => x.Id == item.FirstOrDefault().GoalBudgetInputId);
                if (auxMeta == null)
                    return BadRequest("No se ha encontrado el insumo meta");
                var sum = item.Sum(x => x.Quantity);
                if (auxMeta.WarehouseCurrentMetered < sum)
                    return BadRequest($"Se ha superado el techo de {auxMeta.Supply.Description}" +
                        $" por {Math.Round(sum - auxMeta.WarehouseCurrentMetered, 2)}");
            }

            foreach(var item in folding.GroupBy(x => x.GoalBudgetInput.SupplyId))
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
        /*
        [HttpPut("validar/{id}")]
        public async Task<IActionResult> UpdateStatusApproved(Guid id)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return BadRequest("No se ha encontrado el requerimiento de campo");

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.VALIDATED;

            await _context.SaveChangesAsync();
            return Ok();
        }
        */
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
                } else
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


        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var pId = GetProjectId();

            var presupuestos = await _context.BudgetTitles
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var formulas = await _context.ProjectFormulas
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var frentes = await _context.WorkFronts
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var cuadrillas = await _context.SewerGroups
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var insumos = await _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .ToListAsync();

            var fases = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Include(x => x.WorkFront)
                .Where(x => x.ProjectPhase.ProjectId == pId)
                .ToListAsync();

            var requests = new List<FieldRequest>();
            var items = new List<FieldRequestFolding>();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;

                    var workFront = frentes.FirstOrDefault(x => x.Code == workSheet.Cell($"E{counter}").GetString());

                    var document = 0;
                    var aux = document;
                    var list = new List<FieldRequestProjectFormula>();
                    while (!workSheet.Cell($"E{counter}").IsEmpty())
                    {
                        document = Int32.Parse(workSheet.Cell($"H{counter}").GetString());
                        var projectFormulas = workSheet.Cell($"D{counter}").GetString().Split(",");
                        var projectF = _context.ProjectFormulas.Where(x => projectFormulas.Contains(x.Code) && x.ProjectId == GetProjectId()).Select(x => x.Id);
                        if (aux != document)
                        {
                            list = new List<FieldRequestProjectFormula>();
                            aux = document;
                            
                            var req = new FieldRequest();

                            var budgetTitle = presupuestos.FirstOrDefault(x => x.Name == workSheet.Cell($"B{counter}").GetString());

                            if (budgetTitle == null)
                                return BadRequest("No se ha encontrado el presupuesto de la fila " + counter);

                            req.BudgetTitleId = budgetTitle.Id;
                            req.DeliveryDate = workSheet.Cell($"C{counter}").GetString().ToDateTime();

                            workFront = frentes.FirstOrDefault(x => x.Code == workSheet.Cell($"E{counter}").GetString());

                            if (workFront == null)
                                return BadRequest("No se ha encontrado el frente de la fila " + counter);

                            req.WorkFrontId = workFront.Id;

                            var sewerGroup = cuadrillas.FirstOrDefault(x => x.Code == workSheet.Cell($"F{counter}").GetString());

                            if (sewerGroup == null)
                                return BadRequest("No se ha encontrado la cuadrilla de la fila " + counter);
                            
                            req.SewerGroupId = sewerGroup.Id;

                            req.Observation = workSheet.Cell($"G{counter}").GetString();
                            req.DocumentNumber = document;
                            req.Status = ConstantHelpers.Warehouse.FieldRequest.Status.PRE_ISSUED;
                            req.IssuedUserId = GetUserId();
                            req.WorkOrder = " ";
                            requests.Add(req);
                            foreach(var form in projectF)
                            {
                                list.Add(new FieldRequestProjectFormula
                                {
                                    Id = Guid.NewGuid(),
                                    FieldRequest = req,
                                    ProjectFormulaId = form
                                });
                            }
                        }

                        var item = new FieldRequestFolding();

                        var goalBudgetInput = insumos.FirstOrDefault(x => x.Supply.Description == workSheet.Cell($"I{counter}").GetString()
                            && projectF.Contains(x.ProjectFormulaId) && x.WorkFrontId == workFront.Id);

                        if (goalBudgetInput == null)
                            return BadRequest("No se ha encontrado el insumo de la fila " + counter);

                        item.GoalBudgetInputId = goalBudgetInput.Id;

                        var projectPhase = fases.FirstOrDefault(x => x.ProjectPhase.Description == workSheet.Cell($"J{counter}").GetString() && x.ProjectPhase.ProjectFormulaId != null
                            && projectF.Contains((Guid)x.ProjectPhase.ProjectFormulaId) && x.WorkFrontId == workFront.Id);

                        if (projectPhase == null)
                            return BadRequest("No se ha encontrado la fase de la fila " + counter);

                        item.ProjectPhaseId = projectPhase.ProjectPhaseId;

                        var measure = workSheet.Cell($"K{counter}").GetString().ToDoubleString();

                        item.Quantity = measure;

                        items.Add(item);

                        ++counter;
                    }

                    await _context.FieldRequests.AddRangeAsync(requests);
                    await _context.FieldRequestFoldings.AddRangeAsync(items);
                    await _context.FieldRequestProjectFormulas.AddRangeAsync(list);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("excel-modelo")]
        public FileResult GetExcelSample()
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Pedidos de Campo");

                workSheet.Cell($"B1").Value = "Presupuesto Contractual";
                workSheet.Cell($"C1").Value = "10/10/2022";
                workSheet.Cell($"D1").Value = "F1,F2,F4";
                workSheet.Cell($"E1").Value = "FT-03";
                workSheet.Cell($"F1").Value = "F1-MT-RAE-01";
                workSheet.Cell($"G1").Value = "Observación";
                workSheet.Cell($"H1").Value = "123";
                workSheet.Cell($"I1").Value = "CEMENTO PORTLAND TIPO I (42.5 KG.)";
                workSheet.Cell($"J1").Value = "CASETA VALVULAS RRP-01";
                workSheet.Cell($"K1").Value = "50";
                workSheet.Range("B1:K1").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B2").Value = "Presupuesto";
                workSheet.Cell($"C2").Value = "Fecha de Entrega";
                workSheet.Cell($"D2").Value = "Fórmula";
                workSheet.Cell($"E2").Value = "Frente";
                workSheet.Cell($"F2").Value = "Cuadrilla";
                workSheet.Cell($"G2").Value = "Observaciones";
                workSheet.Cell($"H2").Value = "N° Doc";
                workSheet.Cell($"I2").Value = "Insumo";
                workSheet.Cell($"J2").Value = "Fase";
                workSheet.Cell($"K2").Value = "Cantidad";

                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 25;
                workSheet.Column(3).Width = 16;
                workSheet.Column(4).Width = 16;
                workSheet.Column(5).Width = 16;
                workSheet.Column(6).Width = 16;
                workSheet.Column(7).Width = 50;
                workSheet.Column(8).Width = 8;
                workSheet.Column(9).Width = 20;
                workSheet.Column(10).Width = 15;
                workSheet.Column(11).Width = 10;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:K2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:K2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var aux2 = _context.BudgetTitles.Where(x => x.ProjectId == GetProjectId()).ToList();

                DataTable dtBudgetTitles = new DataTable();
                dtBudgetTitles.TableName = "Presupuestos";
                dtBudgetTitles.Columns.Add("Nombre", typeof(string));
                foreach (var item in aux2)
                    dtBudgetTitles.Rows.Add(item.Name);
                dtBudgetTitles.AcceptChanges();

                var workSheetBudgetTitle = wb.Worksheets.Add(dtBudgetTitles);

                workSheetBudgetTitle.Column(1).Width = 90;

                var aux3 = _context.ProjectFormulas.Where(x => x.ProjectId == GetProjectId()).ToList();

                DataTable dtFormulas = new DataTable();
                dtFormulas.TableName = "Fórmulas";
                dtFormulas.Columns.Add("Código", typeof(string));
                dtFormulas.Columns.Add("Nombre", typeof(string));
                foreach (var item in aux3)
                    dtFormulas.Rows.Add(item.Code, item.Name);
                dtFormulas.AcceptChanges();

                var workSheetFormulas = wb.Worksheets.Add(dtFormulas);

                workSheetFormulas.Column(1).Width = 10;
                workSheetFormulas.Column(2).Width = 90;

                var aux4 = _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectPhase.ProjectFormula)
                    .Where(x => x.WorkFront.ProjectId == GetProjectId())
                    .Distinct()
                    .ToList();

                DataTable dtWorkFronts = new DataTable();
                dtWorkFronts.TableName = "Frentes";
                dtWorkFronts.Columns.Add("Fórmula", typeof(string));
                dtWorkFronts.Columns.Add("Nombre", typeof(string));
                foreach (var item in aux4)
                    dtWorkFronts.Rows.Add(item.ProjectPhase.ProjectFormula.Code
                        , item.WorkFront.Code);
                dtWorkFronts.AcceptChanges();

                var workSheetWorkFronts = wb.Worksheets.Add(dtWorkFronts);

                workSheetWorkFronts.Column(1).Width = 10;
                workSheetWorkFronts.Column(2).Width = 90;

                var aux5 = _context.WorkFrontSewerGroups
                    .Include(x => x.WorkFront)
                    .Include(x => x.SewerGroupPeriod.SewerGroup)
                    .Where(x => x.WorkFront.ProjectId == GetProjectId())
                    .Distinct()
                    .ToList();

                DataTable dtSewerGroups = new DataTable();
                dtSewerGroups.TableName = "Cuadrillas";
                dtSewerGroups.Columns.Add("Frente", typeof(string));
                dtSewerGroups.Columns.Add("Cuadrilla", typeof(string));
                foreach (var item in aux5)
                    dtSewerGroups.Rows.Add(item.WorkFront.Code
                        , item.SewerGroupPeriod.SewerGroup.Code);
                dtSewerGroups.AcceptChanges();

                var workSheetSewerGroups = wb.Worksheets.Add(dtSewerGroups);

                workSheetSewerGroups.Column(1).Width = 10;
                workSheetSewerGroups.Column(2).Width = 90;

                var aux7 = _context.GoalBudgetInputs
                    .Include(x => x.Supply)
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectFormula)
                    .ToList();

                DataTable dtGoalBudgetInputs = new DataTable();
                dtGoalBudgetInputs.TableName = "Insumos";
                dtGoalBudgetInputs.Columns.Add("Fórmula", typeof(string));
                dtGoalBudgetInputs.Columns.Add("Frente", typeof(string));
                dtGoalBudgetInputs.Columns.Add("Insumo", typeof(string));
                foreach (var item in aux7)
                    dtGoalBudgetInputs.Rows.Add(item.ProjectFormula.Code,
                        item.WorkFront.Code, item.Supply.Description);
                dtGoalBudgetInputs.AcceptChanges();

                var workSheetGoalBudgetInputs = wb.Worksheets.Add(dtGoalBudgetInputs);

                workSheetGoalBudgetInputs.Column(1).Width = 10;
                workSheetGoalBudgetInputs.Column(2).Width = 50;
                workSheetGoalBudgetInputs.Column(3).Width = 50;

                var aux8 = _context.WorkFrontProjectPhases
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.ProjectPhase.ProjectFormula)
                    .ToList();

                DataTable dtProjectPhases = new DataTable();
                dtProjectPhases.TableName = "Fases";
                dtProjectPhases.Columns.Add("Fórmula", typeof(string));
                dtProjectPhases.Columns.Add("Frente", typeof(string));
                dtProjectPhases.Columns.Add("Fase", typeof(string));
                foreach (var item in aux8)
                    dtProjectPhases.Rows.Add(item.ProjectPhase.ProjectFormula.Code,
                        item.WorkFront.Code,
                        item.ProjectPhase.Description);
                dtProjectPhases.AcceptChanges();

                var workSheetProjectPhases = wb.Worksheets.Add(dtProjectPhases);

                workSheetProjectPhases.Column(1).Width = 10;
                workSheetProjectPhases.Column(2).Width = 10;
                workSheetProjectPhases.Column(3).Width = 50;

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PedidosDeCampo.xlsx");
                }
            }
        }


        //[HttpGet("reporte/{id}")]
        //public async Task<IActionResult> Report(Guid id)
        //{

        //    var pId = GetProjectId();

        //    //sp afuera
        //    var data = await _context.Set<UspFieldRequestReport>().FromSqlRaw("execute WareHouse_uspFieldRequestReport")
        //                .IgnoreQueryFilters()
        //                .ToListAsync();

        //    data = data.Where(x => x.ProjectId == pId && x.Id == id).ToList();

        //    //sp folding

        //    SqlParameter param1 = new SqlParameter("@Id", id);

        //    var datafolding = await _context.Set<UspFieldRequestFoldingReport>().FromSqlRaw("execute WareHouse_uspFieldRequestFoldingReport @Id",param1)
        //                .IgnoreQueryFilters()
        //                .ToListAsync();



        //    var first = data.FirstOrDefault();

        //    var project = _context.Projects.Where(x => x.Id == pId).FirstOrDefault();

        //    var enlace = project.LogoUrl.ToString();

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        WebClient client = new WebClient();
        //        Stream img = client.OpenRead(enlace);
        //        Bitmap bitmap; bitmap = new Bitmap(img);

        //        Image image = (Image)bitmap;

        //        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    }

        //    using (MemoryStream ms = new MemoryStream())
        //    {

        //        WebClient client = new WebClient();
        //        Stream img = client.OpenRead(enlace);
        //        Bitmap bitmap; bitmap = new Bitmap(img);

        //        Image image = (Image)bitmap;

        //        //bitmap.Save(msaux, System.Drawing.Imaging.ImageFormat.Png);
                
        //        ///

        //        PdfDocument doc = new PdfDocument();
        //        PdfPageBase page = doc.Pages.Add();

        //        PdfGrid grid = new PdfGrid();
        //        grid.Columns.Add(5);
        //        float width = page.Canvas.ClientSize.Width - (grid.Columns.Count + 1);

        //        grid.Columns[0].Width = 130;
        //        grid.Columns[1].Width = 115;
        //        grid.Columns[2].Width = 115;
        //        grid.Columns[3].Width = 50;
        //        grid.Columns[4].Width = 100;

        //        //float width = page.Canvas.ClientSize.Width - (grid.Columns.Count + 1);
        //        //for (int j = 0; j < grid.Columns.Count; j++)
        //        //{
        //        //    grid.Columns[j].Width = 100;
        //        //    //grid.Columns[j].Width = 100;
        //        //}

        //        PdfGridRow row0 = grid.Rows.Add();
        //        PdfGridRow row1 = grid.Rows.Add();
        //        PdfGridRow row2 = grid.Rows.Add();
        //        PdfGridRow row3 = grid.Rows.Add();
        //        float height = 20.0f;
        //        for (int i = 0; i < grid.Rows.Count; i++)
        //        {
        //            grid.Rows[i].Height = height;
        //        }

        //        PdfGridCellContentList lst = new PdfGridCellContentList();
        //        PdfGridCellContent textAndStyle = new PdfGridCellContent();
        //        textAndStyle.Image = PdfImage.FromStream(ToStream(image));
        //        textAndStyle.ImageSize = new SizeF(130, 80);

        //        lst.List.Add(textAndStyle);

        //        //grid.Draw(page, new PointF(0, 50));

        //        row0.Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);
        //        row1.Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);
        //        row2.Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);
        //        row3.Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);

        //        row0.Cells[0].Value = lst;
        //        row0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        row0.Cells[0].RowSpan = 4;
        //        row0.Cells[0].ColumnSpan = 1;

        //        row0.Cells[1].Value = "";
        //        row0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        row0.Cells[1].ColumnSpan = 2;
        //        row0.Cells[1].RowSpan = 4;

        //        row0.Cells[3].Value = "Código";
        //        row0.Cells[3].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        row1.Cells[3].Value = "Versión";
        //        row1.Cells[3].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row1.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        row2.Cells[3].Value = "Fecha";
        //        row2.Cells[3].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row2.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        row3.Cells[3].Value = "Página";
        //        row3.Cells[3].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row3.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                
                
        //        if (project.CostCenter == "050-2018")
        //            row0.Cells[4].Value = "JIC/GAL-For-01";
        //        if (project.CostCenter == "001")
        //            row0.Cells[4].Value = "CSH/GAL-For-01";
        //        row0.Cells[4].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        row1.Cells[4].Value = "1";
        //        row1.Cells[4].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row1.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        row2.Cells[4].Value = "28/09/2021";
        //        row2.Cells[4].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row2.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        row3.Cells[4].Value = "1 de 1";
        //        row3.Cells[4].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        row3.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                

        //        grid.Draw(page, new PointF(0, 50));

        //        ////////////////////////////////////////////////

        //        PdfGrid grid2 = new PdfGrid();
        //        grid2.Columns.Add(6);
        //        float width2 = page.Canvas.ClientSize.Width - (grid2.Columns.Count + 1);
        //        //for (int j = 0; j < grid2.Columns.Count; j++)
        //        //{
        //        //    grid2.Columns[j].Width = 20;
        //        //}

        //        grid2.Columns[0].Width = 50;
        //        grid2.Columns[1].Width = 155;
        //        grid2.Columns[2].Width = 155;
        //        grid2.Columns[3].Width = 50;
        //        grid2.Columns[4].Width = 50;
        //        grid2.Columns[5].Width = 50;



        //        PdfGridRow gridrow0 = grid2.Rows.Add();
        //        PdfGridRow gridrow1 = grid2.Rows.Add();
        //        PdfGridRow gridrow2 = grid2.Rows.Add();
        //        PdfGridRow gridrow3 = grid2.Rows.Add();
        //        PdfGridRow gridrow4 = grid2.Rows.Add();
        //        PdfGridRow gridrow5 = grid2.Rows.Add();
        //        PdfGridRow gridrow6 = grid2.Rows.Add();
        //        PdfGridRow gridrow7 = grid2.Rows.Add();

        //        gridrow0.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow1.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow2.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow3.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow4.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow5.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow6.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow7.Style.Font  = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);

        //        float height2 = 20.0f;
        //        for (int i = 0; i < grid2.Rows.Count; i++)
        //        {
        //            grid2.Rows[i].Height = height2;
        //        }

        //        gridrow0.Cells[0].Value = "Obra";
        //        gridrow0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow0.Cells[0].RowSpan = 4;
        //        gridrow0.Cells[0].ColumnSpan = 1;

        //        gridrow0.Cells[1].Value = project.Name;
        //        //gridrow0.Cells[1].Style.Font = new PdfTrueTypeFont("Helvetica", 6f, PdfFontStyle.Bold, true);
        //        gridrow0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow0.Cells[1].ColumnSpan = 2;
        //        gridrow0.Cells[1].RowSpan = 4;

        //        gridrow0.Cells[3].Value = "N°";
        //        //gridrow0.Cells[3].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow0.Cells[3].RowSpan = 4;
        //        gridrow0.Cells[3].ColumnSpan = 1;


        //        gridrow0.Cells[4].Value = first.DocumentNumber.ToString("d4");
        //        //gridrow0.Cells[4].Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);
        //        gridrow0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow0.Cells[4].ColumnSpan = 2;
        //        gridrow0.Cells[4].RowSpan = 4;

        //        gridrow4.Cells[0].Value = "O.T.N°:";
        //        gridrow4.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow0.Cells[0].ColumnSpan = 1;
        //        //gridrow4.Cells[0].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);


        //        gridrow4.Cells[1].Value = first.Observation;
        //        gridrow4.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow4.Cells[1].ColumnSpan = 2;
        //        //gridrow4.Cells[1].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);

                


        //        gridrow4.Cells[3].Value = "DIA";
        //        gridrow4.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow4.Cells[3].RowSpan = 2;

        //        gridrow4.Cells[4].Value = "MES";
        //        gridrow4.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow4.Cells[4].RowSpan = 2;

        //        gridrow4.Cells[5].Value = "AÑO";
        //        gridrow4.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow4.Cells[5].RowSpan = 2;

        //        gridrow5.Cells[0].Value = "FORMULA:";
        //        gridrow5.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        //gridrow5.Cells[0].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);


        //        gridrow5.Cells[1].Value = first.FormulaCodes;
        //        gridrow5.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow5.Cells[1].ColumnSpan = 2;
        //        //gridrow5.Cells[1].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);

                

        //        gridrow6.Cells[0].Value = "FRENTE:";
        //        gridrow6.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        //gridrow6.Cells[0].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);

        //        gridrow6.Cells[1].Value = first.WorkFrontCode;
        //        gridrow6.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow6.Cells[1].ColumnSpan = 2;
        //        //gridrow6.Cells[1].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);



        //        gridrow6.Cells[3].Value = first.Day.ToString("d2");
        //        gridrow6.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow6.Cells[3].RowSpan = 2;

        //        gridrow6.Cells[4].Value = first.Month.ToString("d2");
        //        gridrow6.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow6.Cells[4].RowSpan = 2;

        //        gridrow6.Cells[5].Value = first.Year.ToString();
        //        gridrow6.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow6.Cells[5].RowSpan = 2;

        //        gridrow7.Cells[0].Value = "CUADRILLA: ";
        //        gridrow7.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        //gridrow7.Cells[0].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);

        //        gridrow7.Cells[1].Value = first.SewerGroupCode;
        //        gridrow7.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        gridrow7.Cells[1].ColumnSpan = 2;
        //        //gridrow7.Cells[1].Style.Font = new PdfTrueTypeFont("Helvetica", 7f, PdfFontStyle.Bold, true);

                

        //        grid2.Draw(page, new PointF(0, 150));

        //        ////////////////////////////////////////////////

        //        PdfGrid grid3 = new PdfGrid();
        //        grid3.Columns.Add(6);
        //        float width3 = page.Canvas.ClientSize.Width - (grid3.Columns.Count + 1);
        //        for (int j = 0; j < grid3.Columns.Count; j++)
        //        {
        //            grid3.Columns[j].Width = width3 * 0.16f;
        //        }

        //        PdfGridRow grid3row0 = grid3.Rows.Add();



        //        grid3row0.Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Bold, true);

        //        float height3 = 20.0f;
        //        for (int i = 0; i < grid3.Rows.Count; i++)
        //        {
        //            grid3.Rows[i].Height = height3;
        //        }

        //        grid3row0.Cells[0].Value = "FASE";
        //        grid3row0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid3row0.Cells[0].ColumnSpan = 1;

        //        grid3row0.Cells[1].Value = "CODIGO";
        //        grid3row0.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid3row0.Cells[1].ColumnSpan = 1;

        //        grid3row0.Cells[2].Value = "CANTIDAD SOLICITADA";
        //        grid3row0.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid3row0.Cells[2].ColumnSpan = 1;

        //        grid3row0.Cells[3].Value = "CANTIDAD ENTREGADA";
        //        grid3row0.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid3row0.Cells[3].ColumnSpan = 1;

        //        grid3row0.Cells[4].Value = "UND.";
        //        grid3row0.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid3row0.Cells[4].ColumnSpan = 1;

        //        grid3row0.Cells[5].Value = "DESCRIPCIÓN DEL PRODUCTO";
        //        grid3row0.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid3row0.Cells[5].ColumnSpan = 1;

        //        var count = 0;
        //        if (datafolding.Count != 0)
        //            foreach (var item in datafolding)
        //            {
        //                count++;
        //                PdfGridRow row = grid3.Rows.Add();
        //                row.Style.Font = new PdfTrueTypeFont("Helvetica", 10f, PdfFontStyle.Regular, true);
        //                row.Cells[0].Value = item.Phase;
        //                row.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //                row.Cells[0].ColumnSpan = 1;

        //                row.Cells[1].Value = item.FullCode;
        //                row.Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //                row.Cells[1].ColumnSpan = 1;

        //                row.Cells[2].Value = item.Quantity;
        //                row.Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //                row.Cells[2].ColumnSpan = 1;

        //                row.Cells[3].Value = item.DeliveredQuantity;
        //                row.Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //                row.Cells[3].ColumnSpan = 1;

        //                row.Cells[4].Value = item.UnitAbbreviation;
        //                row.Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //                row.Cells[4].ColumnSpan = 1;

        //                row.Cells[5].Value = item.Description;
        //                row.Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //                row.Cells[5].ColumnSpan = 1;
        //            }
        //        else
        //        {
        //            PdfGridRow grid3row1 = grid3.Rows.Add();
        //        }

        //        grid3.Draw(page, new PointF(0, 350));

        //        ///////////////////////////////////////////
        //        ///

        //        PdfGrid grid4 = new PdfGrid();
        //        grid4.Columns.Add(6);
        //        float width4 = page.Canvas.ClientSize.Width - (grid4.Columns.Count + 1);
        //        for (int j = 0; j < grid4.Columns.Count; j++)
        //        {
        //            grid4.Columns[j].Width = width4 * 0.16f;
        //        }

        //        PdfGridRow grid4row0 = grid4.Rows.Add();
        //        PdfGridRow grid4row1 = grid4.Rows.Add();
        //        PdfGridRow grid4row2 = grid4.Rows.Add();
        //        PdfGridRow grid4row3 = grid4.Rows.Add();
        //        PdfGridRow grid4row4 = grid4.Rows.Add();
        //        PdfGridRow grid4row5 = grid4.Rows.Add();
        //        PdfGridRow grid4row6 = grid4.Rows.Add();
        //        PdfGridRow grid4row7 = grid4.Rows.Add();


        //        float height4 = 20.0f;
        //        for (int i = 0; i < grid4.Rows.Count; i++)
        //        {
        //            grid4.Rows[i].Height = height4;
        //        }

        //        grid4row0.Cells[0].Value = "Observaciones";
        //        grid4row0.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid4row0.Cells[0].ColumnSpan = 6;
        //        grid4row0.Cells[0].RowSpan = 3;

        //        grid4row3.Cells[0].Value = "V°B° AUTORIZANTE";
        //        grid4row3.Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
        //        grid4row3.Cells[0].ColumnSpan = 3;
        //        grid4row3.Cells[0].RowSpan = 3;

        //        grid4row3.Cells[3].Value = "PERSONA QUE RECOGE \n \nApellidos y Nombre  \nDNI:  \nFIRMA:";
        //        grid4row3.Cells[3].ColumnSpan = 3;
        //        grid4row3.Cells[3].RowSpan = 3;

        //        grid4row6.Cells[0].Value = "Firma1";
        //        grid4row6.Cells[0].ColumnSpan = 3;
        //        grid4row6.Cells[0].RowSpan = 2;

        //        grid4row6.Cells[3].Value = "Firma2";
        //        grid4row6.Cells[3].ColumnSpan = 3;
        //        grid4row6.Cells[3].RowSpan = 2;

        //        grid4.Draw(page, new PointF(0, 550));

        //        doc.SaveToStream(ms);
        //        doc.Close();

        //        return File(ms.ToArray(), "application/pdf", "prueba.pdf");
        //    }
        //}

        //private Stream ToStream(Image image)
        //{
        //    var stream = new MemoryStream();

        //    image.Save(stream, image.RawFormat);
        //    stream.Position = 0;

        //    return stream;
        //}

       
    }
}
