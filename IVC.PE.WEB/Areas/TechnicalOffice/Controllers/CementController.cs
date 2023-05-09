using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.CementViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("oficina-tecnica/cementos")]
    public class CementController : BaseController
    {
        public CementController(IvcDbContext context,
          ILogger<CementController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Cements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var cements = await query
                .Include(x => x.WorkFront)
                .OrderBy(x => x.OrderNumber)
                .Select(x => new CementViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    ContractualMeteredTypeOne = x.ContractualMeteredTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMeteredTypeFive = x.ContractualMeteredTypeFive.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeOne = x.ContractualRestatedTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeFive = x.ContractualRestatedTypeFive.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(cements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.Cements
                .Select(x => new CementViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    ContractualMeteredTypeOne = x.ContractualMeteredTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualMeteredTypeFive = x.ContractualMeteredTypeFive.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeOne = x.ContractualRestatedTypeOne.ToString("N", CultureInfo.InvariantCulture),
                    ContractualRestatedTypeFive = x.ContractualRestatedTypeFive.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, CementViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cement = await _context.Cements.FirstOrDefaultAsync(x => x.Id == id);

            cement.ItemNumber = model.ItemNumber;
            cement.Description = model.Description;
            cement.Unit = model.Unit;
            if (model.Unit == null)
                cement.Unit = "";
            cement.ContractualMeteredTypeOne = model.ContractualMeteredTypeOne.ToDoubleString();
            cement.ContractualMeteredTypeFive = model.ContractualMeteredTypeFive.ToDoubleString();
            cement.ContractualRestatedTypeOne = model.ContractualRestatedTypeOne.ToDoubleString();
            cement.ContractualRestatedTypeFive = model.ContractualRestatedTypeFive.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var cement = await _context.Cements.FirstOrDefaultAsync(x => x.Id == id);

            if (cement == null)
                return BadRequest("No se ha encontrado el acero");

            _context.Cements.Remove(cement);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, CementViewModel model)
        {
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ",";
            //provider.NumberGroupSeparator = ".";
            //provider.NumberGroupSizes = new int[] { 3 };

            var pId = GetProjectId();

            var aux = _context.Cements.Count();
            var constante = _context.Cements.Count() + 1;

            var workFronts = await _context.WorkFronts
                  .Where(x => x.ProjectId == pId)
                 .AsNoTracking().ToListAsync();
            var phases = await _context.ProjectPhases
                .Where(x => x.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var workFrontProjectPhases = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhase.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var phase = phases.FirstOrDefault();
            var workFront = workFronts.FirstOrDefault();

            var budgetTitle = await _context.BudgetTitles.FirstOrDefaultAsync(x => x.Id == model.BudgetTitleId);

            var cements = new List<Cement>();

            var workFrontConts = workFront;

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 4;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var cement = new Cement();
                        cement.Id = Guid.NewGuid();
                        cement.OrderNumber = aux;
                        cement.ItemNumber = workSheet.Cell($"C{counter}").GetString();

                        var ItemNumberConst = cement.ItemNumber.Split(".");
                        var ItemNumberFixed = "";
                        foreach (var item in ItemNumberConst)
                            ItemNumberFixed += item;

                        var a = workSheet.Cell($"M{counter}").GetString();

                        var workFrontExcel = workFronts.FirstOrDefault(x => x.Code == workSheet.Cell($"B{counter}").GetString());

                        if (workFrontExcel == null)
                            return BadRequest($"El frente de trabajo en la fila B{counter} no existe");

                        cement.WorkFrontId = workFrontExcel.Id;

                        var phaseAux = phases
                            .Where(x => x.ProjectFormulaId == model.ProjectFormulaId)
                            .FirstOrDefault(x => x.Code == ItemNumberFixed);
                        var workFrontAux = workFront;
                        if (phaseAux != null)
                        {
                            var description = workSheet.Cell($"D{counter}").GetString();
                            var conexion = workFrontProjectPhases
                                .Where(x => x.ProjectPhase.ProjectFormulaId == model.ProjectFormulaId
                                && x.WorkFrontId == workFrontExcel.Id)
                                .FirstOrDefault(x => x.ProjectPhaseId == phaseAux.Id);
                            if (conexion == null)
                                return BadRequest("No se ha encontrado la relación entre la fase y el frente en la fila " + counter);
                            workFrontAux = workFronts.FirstOrDefault(x => x.Id == conexion.WorkFrontId);
                            phase = phaseAux;
                        }
                        if (workFront.Id != workFrontAux.Id)
                        {

                            var baseE = cement.OrderNumber - 1;
                            var element1 = cements.FirstOrDefault(x => x.OrderNumber == baseE);
                            var elementBase = cements.FirstOrDefault(x => x.OrderNumber == cement.OrderNumber);
                            var lengthBase = cement.ItemNumber.Length;
                            int i = 1;
                            if (element1 != null)
                            {
                                while (element1.ItemNumber.Length <= lengthBase)
                                {
                                    //element1.WorkFrontId = workFrontAux.Id;
                                    element1.ProjectPhaseId = phase.Id;
                                    element1 = cements.FirstOrDefault(x => x.OrderNumber == (baseE - i));
                                    elementBase = cements.FirstOrDefault(x => x.OrderNumber == (baseE - i + 1));
                                    lengthBase = elementBase.ItemNumber.Length;
                                    if (element1 == null)
                                        break;
                                    else
                                        i++;
                                }
                            }


                            workFront = workFrontAux;
                        }


                        cement.ProjectFormulaId = model.ProjectFormulaId;

                        var contractualMeteredTypeOneExcel = workSheet.Cell($"F{counter}").GetString();

                        if (!string.IsNullOrEmpty(contractualMeteredTypeOneExcel))
                        {
                            if (Double.TryParse(contractualMeteredTypeOneExcel, out double contractualMeteredTypeOne))
                                cement.ContractualMeteredTypeOne = Math.Round(contractualMeteredTypeOne, 2);
                        }

                        var contractualMeteredTypeFiveExcel = workSheet.Cell($"G{counter}").GetString();

                        if (!string.IsNullOrEmpty(contractualMeteredTypeFiveExcel))
                        {
                            if (Double.TryParse(contractualMeteredTypeFiveExcel, out double contractualMeteredTypeFive))
                                cement.ContractualMeteredTypeFive = Math.Round(contractualMeteredTypeFive, 2);
                        }

                        var contractualRestatedTypeOneExcel = workSheet.Cell($"H{counter}").GetString();

                        if (!string.IsNullOrEmpty(contractualRestatedTypeOneExcel))
                        {
                            if (Double.TryParse(contractualRestatedTypeOneExcel, out double contractualRestatedTypeOne))
                                cement.ContractualRestatedTypeOne = Math.Round(contractualRestatedTypeOne, 2);
                        }

                        var contractualRestatedTypeFiveExcel = workSheet.Cell($"I{counter}").GetString();

                        if (!string.IsNullOrEmpty(contractualRestatedTypeFiveExcel))
                        {
                            if (Double.TryParse(contractualRestatedTypeFiveExcel, out double contractualRestatedTypeFive))
                                cement.ContractualRestatedTypeFive = Math.Round(contractualRestatedTypeFive, 2);
                        }

                        cement.BudgetTitleId = model.BudgetTitleId;
                        cement.Description = workSheet.Cell($"D{counter}").GetString();
                        cement.Unit = workSheet.Cell($"E{counter}").GetString();
                        cement.ProjectPhaseId = phase.Id;

                        cements.Add(cement);
                        counter++;
                        aux++;
                    }

                }
                mem.Close();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(cements);

                transaction.Commit();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "CementoCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "Frente";
                workSheet.Range("B2:B3").Merge();

                workSheet.Cell($"C2").Value = "Item";
                workSheet.Range("C2:C3").Merge();

                workSheet.Cell($"D2").Value = "Descripción";
                workSheet.Range("D2:D3").Merge();

                workSheet.Cell($"E2").Value = "Und.";
                workSheet.Range("E2:E3").Merge();

                workSheet.Cell($"F2").Value = "Metrado Contractual (lbs)";
                workSheet.Range("F2:G2").Merge();

                workSheet.Cell($"F3").Value = "Cemento Portland Tipo I";
                workSheet.Cell($"G3").Value = "Cemento Portland Tipo V";

                workSheet.Cell($"H2").Value = "Metrado Replanteo (lbs)";
                workSheet.Range("H2:I2").Merge();

                workSheet.Cell($"H3").Value = "Cemento Portland Tipo I";
                workSheet.Cell($"I3").Value = "Cemento Portland Tipo V";

                workSheet.Cell($"B4").Value = "Info Aquí";
                workSheet.Cell($"B4").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 10;
                workSheet.Column(3).Width = 16;
                workSheet.Column(4).Width = 75;
                workSheet.Column(5).Width = 8;
                workSheet.Column(6).Width = 25;
                workSheet.Column(7).Width = 25;
                workSheet.Column(8).Width = 25;
                workSheet.Column(9).Width = 25;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var originTypes = _context.OriginTypeFillingLaboratories.AsNoTracking().ToList();

                var workFrontsPhases = _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.ProjectPhase.ProjectFormula)
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectPhase)
                    .AsNoTracking()
                    .Where(x => x.ProjectPhase.ProjectId == GetProjectId())
                    .ToList();

                var workFronts = _context.WorkFronts.Where(x => x.ProjectId == GetProjectId()).AsNoTracking().ToList();

                int count = 0;

                //------------FRENTES FORMULA 1----------

                DataTable dtFormula1 = new DataTable();
                dtFormula1.TableName = "Fórmula 1";
                dtFormula1.Columns.Add("Código", typeof(string));
                var f1 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F1").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f1)
                    dtFormula1.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula1.AcceptChanges();

                var workSheetF1 = wb.Worksheets.Add(dtFormula1);

                workSheetF1.Column(1).Width = 50;

                //------------FRENTES FORMULA 2----------

                DataTable dtFormula2 = new DataTable();
                dtFormula2.TableName = "Fórmula 2";
                dtFormula2.Columns.Add("Código", typeof(string));
                var f2 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F2").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f2)
                    dtFormula2.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula2.AcceptChanges();

                var workSheetF2 = wb.Worksheets.Add(dtFormula2);

                workSheetF2.Column(1).Width = 50;

                //------------FRENTES FORMULA 3----------

                DataTable dtFormula3 = new DataTable();
                dtFormula3.TableName = "Fórmula 3";
                dtFormula3.Columns.Add("Código", typeof(string));
                var f3 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F3").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f3)
                    dtFormula3.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula3.AcceptChanges();

                var workSheetF3 = wb.Worksheets.Add(dtFormula3);

                workSheetF3.Column(1).Width = 50;

                //------------FRENTES FORMULA 4----------

                DataTable dtFormula4 = new DataTable();
                dtFormula4.TableName = "Fórmula 4";
                dtFormula4.Columns.Add("Código", typeof(string));
                var f4 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F4").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f4)
                    dtFormula4.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula4.AcceptChanges();

                var workSheetF4 = wb.Worksheets.Add(dtFormula4);

                workSheetF4.Column(1).Width = 50;

                //------------FRENTES FORMULA 5----------

                DataTable dtFormula5 = new DataTable();
                dtFormula5.TableName = "Fórmula 5";
                dtFormula5.Columns.Add("Código", typeof(string));
                var f5 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F5").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f5)
                    dtFormula5.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula5.AcceptChanges();

                var workSheetF5 = wb.Worksheets.Add(dtFormula5);

                workSheetF5.Column(1).Width = 50;

                //------------FRENTES FORMULA 6----------

                DataTable dtFormula6 = new DataTable();
                dtFormula6.TableName = "Fórmula 6";
                dtFormula6.Columns.Add("Código", typeof(string));
                var f6 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F6").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f6)
                    dtFormula6.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula6.AcceptChanges();

                var workSheetF6 = wb.Worksheets.Add(dtFormula6);

                workSheetF6.Column(1).Width = 50;

                //------------FRENTES FORMULA 7----------

                DataTable dtFormula7 = new DataTable();
                dtFormula7.TableName = "Fórmula 7";
                dtFormula7.Columns.Add("Código", typeof(string));
                var f7 = workFrontsPhases.Where(x => x.ProjectPhase.ProjectFormula.Code == "F7").GroupBy(x => x.WorkFrontId).Where(x => x.Count() > 0);
                foreach (var item in f7)
                    dtFormula7.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtFormula7.AcceptChanges();

                var workSheetF7 = wb.Worksheets.Add(dtFormula7);

                workSheetF7.Column(1).Width = 50;

                workSheet.Range("B2:I4").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:I4").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("metrado-contractual-tipo-uno")]
        public async Task<IActionResult> GetContractualMeteredTypeOneSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Cements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualMeteredTypeOneSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                contractualMeteredTypeOneSuma += item.ContractualMeteredTypeOne;
            }

            return Ok(contractualMeteredTypeOneSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-contractual-tipo-cinco")]
        public async Task<IActionResult> GetContractualMeteredTypeFiveSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Cements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualMeteredTypeFiveSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                contractualMeteredTypeFiveSuma += item.ContractualMeteredTypeFive;
            }

            return Ok(contractualMeteredTypeFiveSuma.ToString("N0", CultureInfo.InvariantCulture));
        }


        [HttpGet("metrado-replanteado-tipo-uno")]
        public async Task<IActionResult> GetContractualRestatedTypeOneSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Cements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualRestatedTypeOneSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                contractualRestatedTypeOneSuma += item.ContractualRestatedTypeOne;
            }

            return Ok(contractualRestatedTypeOneSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-replanteado-tipo-cinco")]
        public async Task<IActionResult> GetContractualRestatedTypeFiveSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Cements
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var contractualRestatedTypeFiveSuma = 0.0;

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {
                contractualRestatedTypeFiveSuma += item.ContractualRestatedTypeFive;
            }

            return Ok(contractualRestatedTypeFiveSuma.ToString("N0", CultureInfo.InvariantCulture));
        }


        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.Cements
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

            _context.Cements.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
