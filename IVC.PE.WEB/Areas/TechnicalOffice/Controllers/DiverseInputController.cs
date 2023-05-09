using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DiverseInputViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/insumos-diversos")]
    public class DiverseInputController : BaseController
    {
        public DiverseInputController(IvcDbContext context,
           ILogger<DiverseInputController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, 
            Guid? workFrontId = null, Guid? projectPhaseId = null,
            Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);
            var pId = GetProjectId();
            /*
            SqlParameter projectParam = new SqlParameter("@ProjectId", pId);
            SqlParameter projectFormulaParam = new SqlParameter("@ProjectFormulaId", SqlDbType.UniqueIdentifier);
            projectFormulaParam.Value = (object)projectFormulaId ?? DBNull.Value;
            SqlParameter budgetTitleParam = new SqlParameter("@BudgetTitleId", SqlDbType.UniqueIdentifier);
            budgetTitleParam.Value = (object)budgetTitleId ?? DBNull.Value;
            SqlParameter workFrontParam = new SqlParameter("@WorkFrontId", SqlDbType.UniqueIdentifier);
            workFrontParam.Value = (object)workFrontId ?? DBNull.Value;
            SqlParameter projectPhaseParam = new SqlParameter("@ProjectPhaseId", SqlDbType.UniqueIdentifier);
            projectPhaseParam.Value = (object)projectPhaseId ?? DBNull.Value;
            SqlParameter supplyFamilyParam = new SqlParameter("@SupplyFamilyId", SqlDbType.UniqueIdentifier);
            supplyFamilyParam.Value = (object)supplyFamilyId ?? DBNull.Value;
            SqlParameter supplyGroupParam = new SqlParameter("@SupplyGroupId", SqlDbType.UniqueIdentifier);
            supplyGroupParam.Value = (object)supplyGroupId ?? DBNull.Value;
            if (projectFormulaParam.Value != DBNull.Value)
                projectParam.Value = DBNull.Value;
            var query = await _context.Set<UspDiverseInput>()
                .FromSqlRaw("execute TechnicalOffice_uspDiverseInput @ProjectFormulaId, @BudgetTitleId, @WorkFrontId, @ProjectPhaseId, @SupplyFamilyId, @SupplyGroupId, @ProjectId",
                projectFormulaParam, budgetTitleParam, workFrontParam, projectPhaseParam, supplyFamilyParam, supplyGroupParam, projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            */
            //var result = await _context.Database.ExecuteSqlRawAsync("execute sp_updatestats");

            var query = _context.DiverseInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.BudgetInput)
                .Include(x => x.ProjectPhase)
                .Include(x => x.WorkFront)
                .Include(x => x.MeasurementUnit)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .OrderBy(x => x.OrderNumber)
                .AsQueryable();

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId);

            var totalRecords = await query.CountAsync();

            var aux = query.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                aux = aux.Where(x => x.Supply.Description.Contains(search) ||
                    x.Supply.SupplyFamily.Name.Contains(search) ||
                    x.Supply.SupplyGroup.Name.Contains(search) ||
                    x.Supply.FullCode.Contains(search)).ToList();
            }

            var metered = aux.Sum(x => x.Metered);
            var parcial = aux.Sum(x => x.Parcial);

            var data = aux
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new DiverseInputViewModel
                {
                    Id = x.Id,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Description = x.ProjectPhase.Code + "-" + x.ProjectPhase.Description
                    },
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
                    BudgetInput = new BudgetInputViewModel
                    {
                        Code = x.BudgetInput != null ? x.BudgetInput.Code : "",
                        Description = x.BudgetInput != null ? x.BudgetInput.Description : "",
                        SaleUnitPrice = x.BudgetInput != null ? x.BudgetInput.SaleUnitPrice.ToString("N", CultureInfo.InvariantCulture) : ""
                    },
                    MeasurementUnitId = x.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation
                    },
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N", CultureInfo.InvariantCulture)
                })
                .ToList();

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = aux.Count(),
                data,
                metered,
                parcial
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.DiverseInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Include(x => x.BudgetInput)
                .Include(x=>x.MeasurementUnit)
                .Select(x => new DiverseInputViewModel
                {
                    Id = x.Id,
                    ProjectFormulaId = x.ProjectFormulaId,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    MeasurementUnitId = x.MeasurementUnitId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
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
                    BudgetInputId = x.BudgetInputId,
                    BudgetInput = new BudgetInputViewModel
                    {
                        Code = x.BudgetInput.Code,
                        Description = x.BudgetInput.Description,
                        SaleUnitPrice = x.BudgetInput.SaleUnitPrice.ToString("N", CultureInfo.InvariantCulture)
                    },
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(DiverseInputViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diverseInput = new DiverseInput();

            diverseInput.ItemNumber = model.ItemNumber;
            diverseInput.ProjectFormulaId = model.ProjectFormulaId;
            diverseInput.WorkFrontId = model.WorkFrontId;
            diverseInput.ProjectPhaseId = model.ProjectPhaseId;
            diverseInput.BudgetTitleId = model.BudgetTitleId;
            diverseInput.Description = model.Description;
            diverseInput.MeasurementUnitId = model.MeasurementUnitId;
            diverseInput.SupplyId = model.SupplyId;
            diverseInput.Metered = model.Metered.ToDoubleString();
            diverseInput.UnitPrice = model.UnitPrice.ToDoubleString();
            diverseInput.Parcial = Math.Round(diverseInput.Metered * diverseInput.UnitPrice, 2);
            diverseInput.BudgetInputId = model.BudgetInputId;

            await _context.DiverseInputs.AddAsync(diverseInput);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, DiverseInputViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var diverseInput = await _context.DiverseInputs.FirstOrDefaultAsync(x => x.Id == id);

            diverseInput.BudgetTitleId = model.BudgetTitleId;
            diverseInput.ProjectFormulaId = model.ProjectFormulaId;
            diverseInput.WorkFrontId = model.WorkFrontId;
            diverseInput.ProjectPhaseId = model.ProjectPhaseId;
            diverseInput.ItemNumber = model.ItemNumber;
            diverseInput.Description = model.Description;
            diverseInput.MeasurementUnitId = model.MeasurementUnitId;
            diverseInput.SupplyId = model.SupplyId;
            diverseInput.Metered = model.Metered.ToDoubleString();
            diverseInput.UnitPrice = model.UnitPrice.ToDoubleString();
            diverseInput.Parcial = Math.Round(diverseInput.Metered * diverseInput.UnitPrice, 2);
            diverseInput.BudgetInputId = model.BudgetInputId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, DiverseInputViewModel model)
        {
            var pId = GetProjectId();

            var workFronts = await _context.WorkFronts
                .Where(x => x.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var projectPhases = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhase.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var supplies = await _context.Supplies.Include(x => x.SupplyFamily).Include(x => x.SupplyGroup).ToListAsync();
            var budgetInputs = await _context.BudgetInputs.Include(x => x.SupplyFamily).Include(x => x.SupplyGroup).ToListAsync();

            var aux = _context.DiverseInputs.Count();

            var diverseInputs = new List<DiverseInput>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var diverseInput = new DiverseInput();
                        diverseInput.Id = Guid.NewGuid();

                        diverseInput.ItemNumber = workSheet.Cell($"D{counter}").GetString();

                        var workFrontExcel = workFronts.FirstOrDefault(x => x.Code == workSheet.Cell($"B{counter}").GetString());

                        if (workFrontExcel == null)
                            return BadRequest($"El frente de trabajo en la celda B{counter} no existe");

                        diverseInput.WorkFrontId = workFrontExcel.Id;

                        var fase = projectPhases.FirstOrDefault(x => x.WorkFrontId == diverseInput.WorkFrontId 
                        && x.ProjectPhase.Code == workSheet.Cell($"C{counter}").GetString());

                        if (fase == null)
                            return BadRequest("No se ha encontrado la fase de la celda C" + counter);

                        diverseInput.ProjectPhaseId = fase.ProjectPhaseId;

                        /*
                        var ItemNumberConst = diverseInput.ItemNumber.Split(".");
                        var ItemNumberFixed = "";
                        foreach (var item in ItemNumberConst)
                            ItemNumberFixed += item;

                        var a = workSheet.Cell($"M{counter}").GetString();

                        var workFrontExcel = workFronts.FirstOrDefault(x => x.Code == workSheet.Cell($"B{counter}").GetString());

                        if (workFrontExcel == null)
                            return BadRequest($"El frente de trabajo en la celda B{counter} no existe");

                        diverseInput.WorkFrontId = workFrontExcel.Id;

                        var phaseAux = projectPhases.FirstOrDefault(x => x.Code == ItemNumberFixed);
                        var workFrontAux = workFront;
                        if (phaseAux != null)
                        {
                            var description = workSheet.Cell($"B{counter}").GetString();
                            var conexion = workFrontProjectPhases.FirstOrDefault(x => x.ProjectPhaseId == phaseAux.Id);
                            workFrontAux = workFronts.FirstOrDefault(x => x.Id == conexion.WorkFrontId);
                            phase = phaseAux;
                        }
                        if (workFront.Id != workFrontAux.Id)
                        {

                            var baseE = diverseInput.OrderNumber - 1;
                            var element1 = diverseInputs.FirstOrDefault(x => x.OrderNumber == baseE);
                            var elementBase = diverseInputs.FirstOrDefault(x => x.OrderNumber == diverseInput.OrderNumber);
                            var lengthBase = diverseInput.ItemNumber.Length;
                            int i = 1;
                            if (element1 != null)
                            {
                                while (element1.ItemNumber.Length <= lengthBase)
                                {
                                    //element1.WorkFrontId = workFrontAux.Id;
                                    element1.ProjectPhaseId = phase.Id;
                                    element1 = diverseInputs.FirstOrDefault(x => x.OrderNumber == (baseE - i));
                                    elementBase = diverseInputs.FirstOrDefault(x => x.OrderNumber == (baseE - i + 1));
                                    lengthBase = elementBase.ItemNumber.Length;
                                    if (element1 == null)
                                        break;
                                    else
                                        i++;
                                }
                            }
                            workFront = workFrontAux;
                        }
                        */
                        diverseInput.ProjectFormulaId = model.ProjectFormulaId;
                        diverseInput.BudgetTitleId = model.BudgetTitleId;
                        diverseInput.OrderNumber = aux;
                        diverseInput.Description = workSheet.Cell($"E{counter}").GetString();
                        
                        var codeIVC = workSheet.Cell($"F{counter}").GetString();
                        //var descriptionIVC = workSheet.Cell($"F{counter}").GetString();

                        var supply = supplies.FirstOrDefault(x => x.FullCode == codeIVC);
                        if (supply == null)
                            return BadRequest("El insumos del catálogo en celda F" + counter + " no se encuentra");
                       
                        diverseInput.SupplyId = supply.Id;

                        diverseInput.MeasurementUnitId = supply.MeasurementUnitId;

                        var meteredExcel = workSheet.Cell($"H{counter}").GetString();
                        if (!string.IsNullOrEmpty(meteredExcel))
                        {
                            if (Double.TryParse(meteredExcel, out double metered))
                                diverseInput.Metered = metered;
                        }

                        var unitPriceExcel = workSheet.Cell($"I{counter}").GetString();
                        if (!string.IsNullOrEmpty(unitPriceExcel))
                        {
                            if (Double.TryParse(unitPriceExcel, out double unitPrice))
                                diverseInput.UnitPrice = unitPrice;
                        }

                        diverseInput.Parcial = Math.Round(diverseInput.Metered * diverseInput.UnitPrice, 2);

                        var codeS10 = workSheet.Cell($"J{counter}").GetString();

                        var budgetInput = budgetInputs.FirstOrDefault(x => x.Code == codeS10);
                        if (budgetInput != null)
                            diverseInput.BudgetInputId = budgetInput.Id;

                        diverseInputs.Add(diverseInput);
                        counter++;
                        aux++;
                    }

                }
                mem.Close();
            }

            //await _context.DiverseInputs.AddRangeAsync(diverseInputs);
            //await _context.SaveChangesAsync();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(diverseInputs);

                transaction.Commit();
            }
            return Ok();
        }
        
        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "InsumosDiversosCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "Frente";

                workSheet.Cell($"C2").Value = "Fase";

                workSheet.Cell($"D2").Value = "Item";

                workSheet.Cell($"E2").Value = "Descripción";

                workSheet.Cell($"F2").Value = "Código IVC";

                workSheet.Cell($"G2").Value = "Insumo";

                workSheet.Cell($"H2").Value = "Metrado";

                workSheet.Cell($"I2").Value = "P. U. (S/)";

                workSheet.Cell($"J2").Value = "Código S10";

                workSheet.Cell($"B3").Value = "Info Aquí";
                workSheet.Cell($"B3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 11;
                workSheet.Column(3).Width = 11;
                workSheet.Column(4).Width = 14;
                workSheet.Column(5).Width = 75;
                workSheet.Column(6).Width = 15;
                workSheet.Column(7).Width = 40;
                workSheet.Column(8).Width = 15;
                workSheet.Column(9).Width = 15;
                workSheet.Column(10).Width = 15;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:I2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:I2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //-------Catálogo de Insumos

                DataTable dtSupply = new DataTable();
                dtSupply.TableName = "Catálogo de Insumos";
                dtSupply.Columns.Add("Código", typeof(string));
                dtSupply.Columns.Add("Descripción", typeof(string));

                var supplies = _context.Supplies
                    .Include(x => x.SupplyFamily)
                    .Include(x => x.SupplyGroup)
                    .ToList();

                foreach (var item in supplies)
                    dtSupply.Rows.Add(item.FullCode, item.Description);
                dtSupply.AcceptChanges();

                var workSheetSupply = wb.Worksheets.Add(dtSupply);

                workSheetSupply.Column(1).Width = 13;
                workSheetSupply.Column(2).Width = 70;

                //-------FrenteEnFases

                DataTable dtWorkFrontProjectPhase = new DataTable();
                dtWorkFrontProjectPhase.TableName = "Frentes en Fases";
                dtWorkFrontProjectPhase.Columns.Add("Frente", typeof(string));
                dtWorkFrontProjectPhase.Columns.Add("Fases", typeof(string));
                dtWorkFrontProjectPhase.Columns.Add("Descripción", typeof(string));

                var wfphases = _context.WorkFrontProjectPhases
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectPhase)
                    .Where(x => x.WorkFront.ProjectId == GetProjectId())
                    .ToList();

                foreach (var item in wfphases)
                    dtWorkFrontProjectPhase.Rows.Add(item.WorkFront.Code, item.ProjectPhase.Code, item.ProjectPhase.Description);
                dtWorkFrontProjectPhase.AcceptChanges();

                var workSheetWorkFrontProjectPhase = wb.Worksheets.Add(dtWorkFrontProjectPhase);

                workSheetWorkFrontProjectPhase.Column(1).Width = 20;
                workSheetWorkFrontProjectPhase.Column(2).Width = 10;
                workSheetWorkFrontProjectPhase.Column(2).Width = 70;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var diverseInput = await _context.DiverseInputs.FirstOrDefaultAsync(x => x.Id == id);
            if (diverseInput == null)
                return BadRequest($"Insumo Diverso con Id '{id}' no encontrado.");
            _context.DiverseInputs.Remove(diverseInput);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("metrado")]
        public async Task<IActionResult> GetMetered(Guid? supplyFamilyId = null, Guid? supplyGroupId = null, Guid? projectPhaseId = null,
           Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var pId = GetProjectId();

            var query = _context.DiverseInputs
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            var meteredSuma = 0.0;

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (projectPhaseId != null)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                meteredSuma += item.Metered;
            }

            return Ok(meteredSuma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("parcial")]
        public async Task<IActionResult> GetParcial(Guid? supplyFamilyId = null, Guid? supplyGroupId = null, Guid? projectPhaseId = null,
          Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var pId = GetProjectId();

            var query = _context.DiverseInputs
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            var parcialSuma = 0.0;

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (projectPhaseId != null)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                parcialSuma += item.Parcial;
            }

            return Ok(parcialSuma.ToString("N2", CultureInfo.InvariantCulture));
        }
    }
}
