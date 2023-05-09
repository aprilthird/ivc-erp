using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.EntibadoViewModels;
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
    [Route("oficina-tecnica/entibados")]
    public class EntibadoController : BaseController
    {
        public EntibadoController(IvcDbContext context,
      ILogger<EntibadoController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
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

            var entibados = await query
                .Include(x => x.WorkFront)
                .OrderBy(x => x.OrderNumber)
                .Select(x => new EntibadoViewModel
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
                    Performance = x.Performance.ToString("N", CultureInfo.InvariantCulture),
                    KS60xMinibox = x.KS60xMinibox.ToString("N5", CultureInfo.InvariantCulture),
                    KS100xKMC100 = x.KS100xKMC100.ToString("N5", CultureInfo.InvariantCulture),
                    RealzaxExtension = x.RealzaxExtension.ToString("N5", CultureInfo.InvariantCulture),
                    Corredera = x.Corredera.ToString("N5", CultureInfo.InvariantCulture),
                    Paralelo = x.Paralelo.ToString("N5", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(entibados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.Entibados
                .Select(x => new EntibadoViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Performance = x.Performance.ToString("N", CultureInfo.InvariantCulture),
                    KS60xMinibox = x.KS60xMinibox.ToString("N5", CultureInfo.InvariantCulture),
                    KS100xKMC100 = x.KS100xKMC100.ToString("N5", CultureInfo.InvariantCulture),
                    RealzaxExtension = x.RealzaxExtension.ToString("N5", CultureInfo.InvariantCulture),
                    Corredera = x.Corredera.ToString("N5", CultureInfo.InvariantCulture),
                    Paralelo = x.Paralelo.ToString("N5", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EntibadoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entibado = await _context.Entibados.FirstOrDefaultAsync(x => x.Id == id);

            entibado.ItemNumber = model.ItemNumber;
            entibado.Description = model.Description;
            entibado.Unit = model.Unit;
            if (model.Unit == null)
                entibado.Unit = "";
            entibado.Performance = model.Performance.ToDoubleString();
            entibado.KS60xMinibox = model.KS60xMinibox.ToDoubleString();
            entibado.KS100xKMC100 = model.KS100xKMC100.ToDoubleString();
            entibado.RealzaxExtension = model.RealzaxExtension.ToDoubleString();
            entibado.Corredera = model.Corredera.ToDoubleString();
            entibado.Paralelo = model.Paralelo.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entibado = await _context.Entibados.FirstOrDefaultAsync(x => x.Id == id);

            if (entibado == null)
                return BadRequest("No se ha encontrado el acero");

            _context.Entibados.Remove(entibado);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, EntibadoViewModel model)
        {
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ",";
            //provider.NumberGroupSeparator = ".";
            //provider.NumberGroupSizes = new int[] { 3 };

            var pId = GetProjectId();

            var aux = _context.Entibados.Count();
            var constante = _context.Entibados.Count() + 1;

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

            var entibados = new List<Entibado>();

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
                        var entibado = new Entibado();
                        entibado.Id = Guid.NewGuid();
                        entibado.OrderNumber = aux;
                        entibado.ItemNumber = workSheet.Cell($"C{counter}").GetString();

                        var ItemNumberConst = entibado.ItemNumber.Split(".");
                        var ItemNumberFixed = "";
                        foreach (var item in ItemNumberConst)
                            ItemNumberFixed += item;

                        var a = workSheet.Cell($"M{counter}").GetString();

                        var workFrontExcel = workFronts.FirstOrDefault(x => x.Code == workSheet.Cell($"B{counter}").GetString());

                        if (workFrontExcel == null)
                            return BadRequest($"El frente de trabajo en la fila B{counter} no existe");

                        entibado.WorkFrontId = workFrontExcel.Id;

                        var phaseAux = phases
                            .Where(x => x.ProjectFormulaId == model.ProjectFormulaId)
                            .FirstOrDefault(x => x.Code == ItemNumberFixed);
                        var workFrontAux = workFront;
                        if (phaseAux != null)
                        {
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

                            var baseE = entibado.OrderNumber - 1;
                            var element1 = entibados.FirstOrDefault(x => x.OrderNumber == baseE);
                            var elementBase = entibados.FirstOrDefault(x => x.OrderNumber == entibado.OrderNumber);
                            var lengthBase = entibado.ItemNumber.Length;
                            int i = 1;
                            if (element1 != null)
                            {
                                while (element1.ItemNumber.Length <= lengthBase)
                                {
                                    //element1.WorkFrontId = workFrontAux.Id;
                                    element1.ProjectPhaseId = phase.Id;
                                    element1 = entibados.FirstOrDefault(x => x.OrderNumber == (baseE - i));
                                    elementBase = entibados.FirstOrDefault(x => x.OrderNumber == (baseE - i + 1));
                                    lengthBase = elementBase.ItemNumber.Length;
                                    if (element1 == null)
                                        break;
                                    else
                                        i++;
                                }
                            }


                            workFront = workFrontAux;
                        }

                        entibado.ProjectFormulaId = model.ProjectFormulaId;

                        var performanceExcel = workSheet.Cell($"F{counter}").GetString();

                        if (!string.IsNullOrEmpty(performanceExcel))
                        {
                            if (Double.TryParse(performanceExcel, out double performance))
                                entibado.Performance = Math.Round(performance, 2);
                        }

                        var k60xMiniboxExcel = workSheet.Cell($"G{counter}").GetString();

                        if (!string.IsNullOrEmpty(k60xMiniboxExcel))
                        {
                            if (Double.TryParse(k60xMiniboxExcel, out double k60xMinibox))
                                entibado.KS60xMinibox = Math.Round(k60xMinibox, 5);
                        }

                        var kS100xKMC100Excel = workSheet.Cell($"H{counter}").GetString();

                        if (!string.IsNullOrEmpty(kS100xKMC100Excel))
                        {
                            if (Double.TryParse(kS100xKMC100Excel, out double kS100xKMC100))
                                entibado.KS100xKMC100 = Math.Round(kS100xKMC100, 5);
                        }

                        var realzaxExtensionExcel = workSheet.Cell($"I{counter}").GetString();

                        if (!string.IsNullOrEmpty(realzaxExtensionExcel))
                        {
                            if (Double.TryParse(realzaxExtensionExcel, out double realzaxExtension))
                                entibado.RealzaxExtension = Math.Round(realzaxExtension, 5);
                        }

                        var correderaExcel = workSheet.Cell($"J{counter}").GetString();

                        if (!string.IsNullOrEmpty(correderaExcel))
                        {
                            if (Double.TryParse(correderaExcel, out double corredera))
                                entibado.Corredera = Math.Round(corredera, 5);
                        }

                        var paraleloExcel = workSheet.Cell($"K{counter}").GetString();

                        if (!string.IsNullOrEmpty(paraleloExcel))
                        {
                            if (Double.TryParse(paraleloExcel, out double paralelo))
                                entibado.Paralelo = Math.Round(paralelo, 5);
                        }

                        entibado.BudgetTitleId = model.BudgetTitleId;
                        entibado.Description = workSheet.Cell($"D{counter}").GetString();
                        entibado.Unit = workSheet.Cell($"E{counter}").GetString();
                        entibado.ProjectPhaseId = phase.Id;

                        entibados.Add(entibado);
                        counter++;
                        aux++;
                    }

                }
                mem.Close();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(entibados);

                transaction.Commit();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "EntibadoCargaMasiva.xlsx";
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

                workSheet.Cell($"F2").Value = "Rendimiento (ml x día)";
                workSheet.Range("F2:F3").Merge();

                workSheet.Cell($"G2").Value = "Metrados (días)";
                workSheet.Range("G2:K2").Merge();

                workSheet.Cell($"G3").Value = "KS60/Minibox";
                workSheet.Cell($"H3").Value = "KS100/KMC100";
                workSheet.Cell($"I3").Value = "Realza/Extensión";
                workSheet.Cell($"J3").Value = "Corredera";
                workSheet.Cell($"K3").Value = "Paralelo";

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
                workSheet.Column(10).Width = 25;
                workSheet.Column(11).Width = 25;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var originTypes = _context.OriginTypeFillingLaboratories.AsNoTracking().ToList();

                var workFrontsPhases = _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.ProjectPhase.ProjectFormula)
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectPhase)
                    .Where(x => x.ProjectPhase.ProjectId == GetProjectId())
                    .AsNoTracking()
                    .ToList();

                var workFronts = _context.WorkFronts
                    .Where(x => x.ProjectId == GetProjectId())
                    .AsNoTracking()
                    .ToList();

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

                workSheet.Range("B2:K4").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:K4").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        [HttpGet("metrado-ks60")]
        public async Task<IActionResult> GetKS60Suma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var ks60Suma = 0.0;

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
                ks60Suma += item.KS60xMinibox;
            }

            return Ok(ks60Suma.ToString("N5", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-ks100")]
        public async Task<IActionResult> GetKS100Suma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var ks100Suma = 0.0;

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
                ks100Suma += item.KS100xKMC100;
            }

            return Ok(ks100Suma.ToString("N5", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-realza-extension")]
        public async Task<IActionResult> GetRealzaExtensionSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var realzaExtensionSuma = 0.0;

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
                realzaExtensionSuma += item.RealzaxExtension;
            }

            return Ok(realzaExtensionSuma.ToString("N5", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-corredera")]
        public async Task<IActionResult> GetCorrederaSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var correderaSuma = 0.0;

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
                correderaSuma += item.Corredera;
            }

            return Ok(correderaSuma.ToString("N5", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-paralelo")]
        public async Task<IActionResult> GetParaleloSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var parareloSuma = 0.0;

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
                parareloSuma += item.Paralelo;
            }

            return Ok(parareloSuma.ToString("N5", CultureInfo.InvariantCulture));
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var totalSuma = 0.0;

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
                totalSuma += item.KS60xMinibox + item.KS100xKMC100 + item.RealzaxExtension + item.Corredera + item.Paralelo;
            }

            return Ok(totalSuma.ToString("N5", CultureInfo.InvariantCulture));
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.Entibados
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

            _context.Entibados.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
