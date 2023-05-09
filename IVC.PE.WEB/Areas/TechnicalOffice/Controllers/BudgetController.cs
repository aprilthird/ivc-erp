using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/presupuestos")]
    public class BudgetController : BaseController
    {
        public BudgetController(IvcDbContext context,
            ILogger<BudgetController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? budgetTypeId = null, int group = 0)
        {
            var pId = GetProjectId();

            //var result = await _context.Database.ExecuteSqlRawAsync("execute sp_updatestats");

            var query = _context.Budgets
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (budgetTypeId.HasValue)
                query = query.Where(x => x.BudgetTypeId == budgetTypeId);
            if (group != 0)
                query = query.Where(x => x.Group == group);
                 

            var budgets = await query
                .OrderBy(x => x.ProjectFormula.Code)
                .ThenBy(x=>x.OrderNumber)
                .Select(x => new BudgetViewModel
                {
                    Id = x.Id,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    TotalPrice = x.TotalPrice.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMO = x.ContractualMO.ToString("N", CultureInfo.InvariantCulture),
                    ContractualEQ = x.ContractualEQ.ToString("N", CultureInfo.InvariantCulture),
                    ContractualSubcontract = x.ContractualSubcontract.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMaterials = x.ContractualMaterials.ToString("N", CultureInfo.InvariantCulture),
                    CollaboratorMO = x.CollaboratorMO.ToString("N", CultureInfo.InvariantCulture),
                    CollaboratorEQ = x.CollaboratorEQ.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(budgets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var budget = await _context.Budgets
                .Where(x => x.Id == id)
                .Select(x => new BudgetViewModel
                {
                    Id = x.Id,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    Unit = x.Unit,
                    ProjectFormulaId = x.ProjectFormulaId,
                    BudgetTitleId = x.BudgetTitleId,
                    Type = x.Type,
                    BudgetTypeId = x.BudgetTypeId,
                    Group = x.Group,
                    Metered = x.Metered.ToString(),
                    UnitPrice = x.UnitPrice.ToString(),
                    TotalPrice = x.TotalPrice.ToString("F", CultureInfo.InvariantCulture),
                    ContractualMO = x.ContractualMO.ToString(),
                    ContractualEQ = x.ContractualEQ.ToString(),
                    ContractualSubcontract = x.ContractualSubcontract.ToString(),
                    ContractualMaterials = x.ContractualMaterials.ToString(),
                    CollaboratorMO = x.CollaboratorMO.ToString(),
                    CollaboratorEQ = x.CollaboratorEQ.ToString()
                }).AsNoTracking().FirstOrDefaultAsync();

            return Ok(budget);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BudgetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(x => x.Id == id);

            budget.NumberItem = model.NumberItem;
            budget.Description = model.Description;
            budget.Unit = model.Unit ?? "";
            budget.Metered = model.Metered.ToDoubleString();
            budget.UnitPrice = model.UnitPrice.ToDoubleString();
            budget.TotalPrice = model.TotalPrice.ToDoubleString();
            budget.ContractualMO = model.ContractualMO.ToDoubleString();
            budget.ContractualEQ = model.ContractualEQ.ToDoubleString();
            budget.ContractualSubcontract = model.ContractualSubcontract.ToDoubleString();
            budget.ContractualMaterials = model.ContractualMaterials.ToDoubleString();
            budget.CollaboratorMO = model.CollaboratorMO.ToDoubleString();
            budget.CollaboratorEQ = model.CollaboratorEQ.ToDoubleString();
            budget.BudgetTitleId = model.BudgetTitleId;
            budget.BudgetTypeId = model.BudgetTypeId;
            budget.Group = model.Group;
            budget.ProjectFormulaId = model.ProjectFormulaId;

            await _context.SaveChangesAsync();

            return Ok(budget);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.Id == id);

            if (budget == null)
                return BadRequest("No se ha encontrado el budget");

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, BudgetViewModel model)
        {
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ",";
            //provider.NumberGroupSeparator = ".";
            //provider.NumberGroupSizes = new int[] { 3 };

            var aux = _context.Budgets.Count();

            var budgets = new List<Budget>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 11;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var budget = new Budget();
                        budget.Id = Guid.NewGuid();
                        budget.NumberItem = workSheet.Cell($"B{counter}").GetString();

                        budget.ProjectFormulaId = model.ProjectFormulaId;
                        budget.BudgetTitleId = model.BudgetTitleId;
                        budget.OrderNumber = aux;
                        budget.BudgetTypeId = model.BudgetTypeId;
                        budget.Group = model.Group;
                        budget.Description = workSheet.Cell($"C{counter}").GetString();
                        budget.Unit = workSheet.Cell($"E{counter}").GetString();

                        var meteredExcel = workSheet.Cell($"F{counter}").GetString();
                        if (!string.IsNullOrEmpty(meteredExcel))
                        {
                            if (Double.TryParse(meteredExcel, out double metered))
                                budget.Metered = metered;
                        }

                        var unitPriceExcel = workSheet.Cell($"G{counter}").GetString();
                        if (!string.IsNullOrEmpty(unitPriceExcel))
                        {
                            if (Double.TryParse(unitPriceExcel, out double unitPrice))
                                budget.UnitPrice = unitPrice;
                        }

                        var totalPriceExcel = workSheet.Cell($"H{counter}").GetString();
                        if (!string.IsNullOrEmpty(totalPriceExcel))
                        {
                            if (Double.TryParse(totalPriceExcel, out double totalPrice))
                                budget.TotalPrice = totalPrice;
                        }

                        var conMOExcel = workSheet.Cell($"J{counter}").GetString();
                        if (!string.IsNullOrEmpty(conMOExcel))
                        {
                            if (Double.TryParse(conMOExcel, out double conMO))
                                budget.ContractualMO = conMO;
                        }

                        var contEQExcel = workSheet.Cell($"K{counter}").GetString();
                        if (!string.IsNullOrEmpty(contEQExcel))
                        {
                            if (Double.TryParse(contEQExcel, out double contEQ))
                                budget.ContractualEQ = contEQ;
                        }

                        var contSubContractExcel = workSheet.Cell($"L{counter}").GetString();
                        if (!string.IsNullOrEmpty(contSubContractExcel))
                        {
                            if (Double.TryParse(contSubContractExcel, out double contSubContract))
                                budget.ContractualSubcontract = contSubContract;
                        }

                        var contMaterialsExcel = workSheet.Cell($"M{counter}").GetString();
                        if (!string.IsNullOrEmpty(contMaterialsExcel))
                        {
                            if (Double.TryParse(contMaterialsExcel, out double contMaterials))
                                budget.ContractualMaterials = contMaterials;
                        }

                        var colMOExcel = workSheet.Cell($"O{counter}").GetString();
                        if (!string.IsNullOrEmpty(colMOExcel))
                        {
                            if (Double.TryParse(colMOExcel, out double colMO))
                                budget.CollaboratorMO = colMO;
                        }

                        var colEQExcel = workSheet.Cell($"P{counter}").GetString();
                        if (!string.IsNullOrEmpty(colEQExcel))
                        {
                            if (Double.TryParse(colEQExcel, out double colEQ))
                                budget.CollaboratorEQ = colEQ;
                        }

                        budgets.Add(budget);
                        counter++;
                        aux++;
                    }

                }
                mem.Close();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(budgets);

                transaction.Commit();
            }

            return Ok();
        }


        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "PresupuestoCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B9").Value = "Item";

                workSheet.Cell($"C9").Value = "Descripción";
                workSheet.Range("C9:D9").Merge();

                workSheet.Cell($"E9").Value = "Und.";

                workSheet.Cell($"F9").Value = "Metrado";

                workSheet.Cell($"G9").Value = "Precio S/";

                workSheet.Cell($"H9").Value = "Parcial S/";

                workSheet.Cell($"J8").Value = "CONTRACTUAL";
                workSheet.Range("J8:M8").Merge();

                workSheet.Cell($"J9").Value = "MO";

                workSheet.Cell($"K9").Value = "EQ";

                workSheet.Cell($"L9").Value = "SUBCONTRATO";

                workSheet.Cell($"M9").Value = "MATERIALES";

                workSheet.Cell($"O8").Value = "COLABORADOR";
                workSheet.Range("O8:P8").Merge();

                workSheet.Cell($"O9").Value = "MO";

                workSheet.Cell($"P9").Value = "EQ";

                workSheet.Cell($"B11").Value = "Info Aquí";
                workSheet.Cell($"B11").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 13;
                workSheet.Column(3).Width = 6;
                workSheet.Column(4).Width = 67;
                workSheet.Column(5).Width = 6;
                workSheet.Column(6).Width = 13;
                workSheet.Column(7).Width = 13;
                workSheet.Column(8).Width = 13;
                workSheet.Column(9).Width = 1;
                workSheet.Column(10).Width = 13;
                workSheet.Column(11).Width = 13;
                workSheet.Column(12).Width = 13;
                workSheet.Column(13).Width = 13;
                workSheet.Column(14).Width = 1;
                workSheet.Column(15).Width = 13;
                workSheet.Column(16).Width = 13;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B9:P9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B9:P9").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        [HttpDelete("todo-eliminar")]
        public async Task<IActionResult> DeleteAll()
        {
            var TodoBudget = await _context.Budgets.ToListAsync();

            foreach (var budget in TodoBudget)
            {
                _context.Budgets.Remove(budget);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("calcular-netos")]
        public async Task<IActionResult> CreateNeto()
        {
            var budgetType = await _context.BudgetTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Name == "Deductivo");

            var budgets = await _context.Budgets.Where(x => x.BudgetTypeId == budgetType.Id).OrderBy(x => x.OrderNumber).AsNoTracking().ToListAsync();

            var budgetTitle = await _context.BudgetTitles.FirstOrDefaultAsync(x => x.Name == "Contractual Neto");

            var budgetsNetos = await _context.Budgets.Where(x => x.BudgetTitleId == budgetTitle.Id).ToListAsync();

            if (budgetsNetos != null)
            {
                _context.Budgets.RemoveRange(budgetsNetos);
                await _context.SaveChangesAsync();
            }

            var counter = _context.Budgets.Count();

            var netos = new List<Budget>();

            foreach(var item in budgets)
            {
                var neto = new Budget();

                var metered = item.Metered;
                var parcial = item.TotalPrice;

                var contractual = await _context.Budgets.Include(x => x.BudgetTitle).FirstOrDefaultAsync(x => x.BudgetTitle.Name == "Contractual Original" && x.NumberItem == item.NumberItem);
                if (contractual != null)
                {
                    metered += contractual.Metered;
                    parcial += contractual.TotalPrice;
                }

                neto = item;
                neto.Id = Guid.NewGuid();
                neto.BudgetTitleId = budgetTitle.Id;
                neto.OrderNumber = counter;
                neto.Metered = metered;
                neto.TotalPrice = parcial;

                netos.Add(neto);
                counter++;

            }
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(netos);

                transaction.Commit();
            }

            return Ok();
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.Budgets
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            else
                return BadRequest("No se ha escogido el título de presupuesto");

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            else
                return BadRequest("No se ha escogido la fórmula");

            var data = await query.ToListAsync();

            _context.Budgets.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /*

        [HttpGet("venta")]
        public IActionResult Sale() => View();

        [HttpGet("meta")]
        public IActionResult Goal() => View();

        [HttpGet("venta/consolidado")]
        public IActionResult SaleSummary() => View();

        [HttpGet("meta/consolidado")]
        public IActionResult GoalSummary() => View();

        [HttpGet("venta/listar")]
        public async Task<IActionResult> SaleGetAll(int? type = null, int? group = null, Guid? budgetTitleId = null, Guid? budgetFormulaId = null)
        {
            var result = await GetAll(true, type, group, budgetTitleId, budgetFormulaId);
            return Ok(result);
        }

        [HttpGet("meta/listar")]
        public async Task<IActionResult> GoalGetAll(int? type = null, int? group = null, Guid? budgetTitleId = null, Guid? budgetFormulaId = null)
        {
            var result = await GetAll(false, type, group, budgetTitleId, budgetFormulaId);
            return Ok(result);
        }

        [HttpGet("venta/{id}")]
        public async Task<IActionResult> SaleGet(Guid id)
        {
            var data = await Get(id);
            return Ok(data);
        }

        [HttpGet("meta/{id}")]
        public async Task<IActionResult> GoalGet(Guid id)
        {
            var data = await Get(id, false);
            return Ok(data);
        }

        public async Task<PagedListViewModel<BudgetViewModel>> GetAll(bool initialVersion = true, int? type = null, int? group = null, Guid? budgetTitleId = null, Guid? budgetFormulaId = null)
        {
            var paginationParameter = GetPaginationParameters();

            var query = _context.Budgets
                .Where(x => x.BudgetFormula.ProjectId == GetProjectId())
                .AsNoTracking().AsQueryable();

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                query = query.Where(x => x.NumberItem.ToString().Contains(paginationParameter.Search) ||
                        x.Description.Contains(paginationParameter.Search));
            }

            if (type.HasValue)
                query = query.Where(x => x.Type == type.Value);
            if (group.HasValue)
                query = query.Where(x => x.Group == group.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (budgetFormulaId.HasValue)
                query = query.Where(x => x.BudgetFormulaId == budgetFormulaId.Value);

            var data = await query
                .Skip(paginationParameter.CurrentNumber)
                .Take(paginationParameter.RecordsPerPage)
                .Select(x => new BudgetViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    Group = x.Group,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Abbreviation = x.BudgetTitle.Abbreviation,
                        Name = x.BudgetTitle.Name
                    },
                    BudgetFormulaId = x.BudgetFormulaId,
                    BudgetFormula = new BudgetFormulaViewModel
                    {
                        Code = x.BudgetFormula.Code,
                        Name = x.BudgetFormula.Name
                    },
                    MeasurementUnitId = x.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation,
                    },
                    Measure = x.Measure,
                    UnitPrice = initialVersion
                        ? x.SaleUnitPrice : x.GoalUnitPrice,
                    BudgetParent = x.BudgetParentId.HasValue
                        ? new BudgetViewModel
                        {
                            NumberItem = x.BudgetParent.NumberItem,
                            BudgetParentId = x.BudgetParent.BudgetParentId,
                            BudgetParent = x.BudgetParentId.HasValue
                            ? new BudgetViewModel
                            {
                                NumberItem = x.BudgetParent.NumberItem,
                                BudgetParentId = x.BudgetParent.BudgetParentId,
                                BudgetParent = x.BudgetParentId.HasValue
                                ? new BudgetViewModel
                                {
                                    NumberItem = x.BudgetParent.NumberItem,
                                    BudgetParentId = x.BudgetParent.BudgetParentId,
                                    BudgetParent = x.BudgetParentId.HasValue
                                    ? new BudgetViewModel
                                    {
                                        NumberItem = x.BudgetParent.NumberItem,
                                        BudgetParentId = x.BudgetParent.BudgetParentId,

                                    } : null
                                } : null
                            } : null
                        } : null
                }).AsNoTracking().ToListAsync();

            var result = GetPagedList(totalRecords, await query.CountAsync(), data);

            return result;
        }

        public async Task<BudgetViewModel> Get(Guid id, bool initialVersion = true)
        {
            var data = await _context.Budgets
                .Where(x => x.Id == id)
                .Select(x => new BudgetViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    Group = x.Group,
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetFormulaId = x.BudgetFormulaId,
                    MeasurementUnitId = x.MeasurementUnitId,
                    Measure = x.Measure,
                    UnitPrice = initialVersion 
                        ? x.SaleUnitPrice : x.GoalUnitPrice,
                    NumberItem = x.NumberItem,
                    Description = x.Description,
                    BudgetParentId = x.BudgetParentId,
                    BudgetParent = x.BudgetParentId.HasValue
                        ? new BudgetViewModel
                        {
                            NumberItem = x.BudgetParent.NumberItem,
                            BudgetParentId = x.BudgetParent.BudgetParentId,
                            BudgetParent = x.BudgetParentId.HasValue
                            ? new BudgetViewModel
                            {
                                NumberItem = x.BudgetParent.NumberItem,
                                BudgetParentId = x.BudgetParent.BudgetParentId,
                                BudgetParent = x.BudgetParentId.HasValue
                                ? new BudgetViewModel
                                {
                                    NumberItem = x.BudgetParent.NumberItem,
                                    BudgetParentId = x.BudgetParent.BudgetParentId,
                                    BudgetParent = x.BudgetParentId.HasValue
                                    ? new BudgetViewModel
                                    {
                                        NumberItem = x.BudgetParent.NumberItem,
                                        BudgetParentId = x.BudgetParent.BudgetParentId,

                                    } : null
                                } : null
                            } : null
                        } : null
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return data;
        }

        [HttpPost("venta/crear")]
        public async Task<IActionResult> Create(BudgetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var budget = new Budget
            {
                Type = model.Type,
                Group = model.Group,
                Description = model.Description,
                BudgetTitleId = model.BudgetTitleId,
                BudgetFormulaId = model.BudgetFormulaId,
            };
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("venta/editar/{id}")]
        public async Task<IActionResult> SaleEdit(Guid id, BudgetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await Edit(id, model);
            return Ok();
        }

        [HttpPut("meta/editar/{id}")]
        public async Task<IActionResult> GoalEdit(Guid id, BudgetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await Edit(id, model, false);
            return Ok();
        }

        public async Task Edit(Guid id, BudgetViewModel model, bool initialVersion = true)
        {
            var budget = await _context.Budgets.FindAsync(id);
            budget.Type = model.Type;
            budget.Group = model.Group;
            budget.NumberItem = model.NumberItem;
            budget.Description = model.Description;
            budget.MeasurementUnitId = model.MeasurementUnitId;
            budget.BudgetTitleId = model.BudgetTitleId;
            budget.BudgetFormulaId = model.BudgetFormulaId;
            budget.Measure = model.Measure;
            budget.BudgetParentId = model.BudgetParentId;
            if (initialVersion)
                budget.SaleUnitPrice = model.UnitPrice;
            else
                budget.GoalUnitPrice = model.UnitPrice;
            await _context.SaveChangesAsync();
        }

        [HttpDelete("venta/eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var budget = await _context.Budgets.FirstOrDefaultAsync(x => x.Id == id);
            if (budget == null)
                return BadRequest($"Presupuesto con Id '{id}' no encontrado.");
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return Ok();
        }
        */
    }
}
