using ClosedXML.Excel;
using EFCore.BulkExtensions;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SteelViewModels;
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
    [Route("oficina-tecnica/aceros")]
    public class SteelController : BaseController
    {
        public SteelController(IvcDbContext context,
            ILogger<SteelController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
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

            var steels = await query
                .Include(x=>x.WorkFront)
                .OrderBy(x=>x.OrderNumber)
                .Select(x => new SteelViewModel
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
                    ContractualMetered = x.ContractualMetered.ToString("N", CultureInfo.InvariantCulture),
                    Rod6mm = x.Rod6mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod8mm = x.Rod8mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x8 = x.Rod3x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod1x2 = x.Rod1x2.ToString("N", CultureInfo.InvariantCulture),
                    Rod5x8 = x.Rod5x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x4 = x.Rod3x4.ToString("N", CultureInfo.InvariantCulture),
                    Rod1 = x.Rod1.ToString("N", CultureInfo.InvariantCulture),
                    ContractualStaked = x.ContractualStaked.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(steels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.Steels
                .Select(x => new SteelViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    ContractualMetered = x.ContractualMetered.ToString("N", CultureInfo.InvariantCulture),
                    Rod6mm = x.Rod6mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod8mm = x.Rod8mm.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x8 = x.Rod3x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod1x2 = x.Rod1x2.ToString("N", CultureInfo.InvariantCulture),
                    Rod5x8 = x.Rod5x8.ToString("N", CultureInfo.InvariantCulture),
                    Rod3x4 = x.Rod3x4.ToString("N", CultureInfo.InvariantCulture),
                    Rod1 = x.Rod1.ToString("N", CultureInfo.InvariantCulture),
                    ContractualStaked = x.ContractualStaked.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SteelViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var steel = await _context.Steels.FirstOrDefaultAsync(x => x.Id == id);

            steel.ItemNumber = model.ItemNumber;
            steel.Description = model.Description;
            if (model.Unit == null)
                steel.Unit = "";
            steel.ContractualMetered = model.ContractualMetered.ToDoubleString();
            steel.Rod6mm = model.Rod6mm.ToDoubleString();
            steel.Rod8mm = model.Rod8mm.ToDoubleString();
            steel.Rod3x8 = model.Rod3x8.ToDoubleString();
            steel.Rod1x2 = model.Rod1x2.ToDoubleString();
            steel.Rod5x8 = model.Rod5x8.ToDoubleString();
            steel.Rod3x4 = model.Rod3x4.ToDoubleString();
            steel.Rod1 = model.Rod1.ToDoubleString();
            steel.ContractualStaked = model.ContractualStaked.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var steel = await _context.Steels.FirstOrDefaultAsync(x => x.Id == id);

            if (steel == null)
                return BadRequest("No se ha encontrado el acero");

            _context.Steels.Remove(steel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file, SteelViewModel model)
        {
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = ",";
            //provider.NumberGroupSeparator = ".";
            //provider.NumberGroupSizes = new int[] { 3 };

            var pId = GetProjectId();

            var aux = _context.Steels.Count();
            var constante = _context.Steels.Count() + 1;

            var workFronts = await _context.WorkFronts
                 .Where(x => x.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var phases = await _context.ProjectPhases
                .Where(x=>x.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var workFrontProjectPhases = await _context.WorkFrontProjectPhases
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhase.ProjectId == pId)
                .AsNoTracking().ToListAsync();
            var phase = phases.FirstOrDefault();
            var workFront = workFronts.FirstOrDefault();

            var budgetTitle = await _context.BudgetTitles.FirstOrDefaultAsync(x => x.Id == model.BudgetTitleId);

            var steelVariables = await _context.SteelVariables.Include(x => x.BudgetInput).Where(x => x.BudgetInput.ProjectId == pId).AsNoTracking().ToListAsync();

            var rod6mmVariable = steelVariables.FirstOrDefault(x => x.RodDiameterMilimeters == 6);
            if (rod6mmVariable == null)
                return BadRequest("No se ha encontrado la variable de 6mm");

            var rod8mmVariable = steelVariables.FirstOrDefault(x => x.RodDiameterMilimeters == 8);
            if (rod8mmVariable == null)
                return BadRequest("No se ha encontrado la variable de 8mm");

            var rod3x8Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "3/8");
            if (rod3x8Variable == null)
                return BadRequest("No se ha encontrado la variable de 3/8\"");

            var rod1x2Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "1/2");
            if (rod1x2Variable == null)
                return BadRequest("No se ha encontrado la variable de 1/2\"");

            var rod5x8Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "5/8");
            if (rod5x8Variable == null)
                return BadRequest("No se ha encontrado la variable de 5/8\"");

            var rod3x4Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "3/4");
            if (rod3x4Variable == null)
                return BadRequest("No se ha encontrado la variable de 3/4\"");

            var rod1Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "1");
            if (rod1Variable == null)
                return BadRequest("No se ha encontrado la variable de 1\"");

            var steels = new List<Steel>();
            
            var rod6mmSuma = 0.0;
            var rod8mmSuma = 0.0;
            var rod3x8Suma = 0.0;
            var rod1x2Suma = 0.0;
            var rod5x8Suma = 0.0;
            var rod3x4Suma = 0.0;
            var rod1Suma = 0.0;
            var contractualSuma = 0.0;
            
            var workFrontConts = workFront;

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 6;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var steel = new Steel();
                        steel.Id = Guid.NewGuid();
                        steel.OrderNumber = aux;
                        steel.ItemNumber = workSheet.Cell($"B{counter}").GetString();

                        var ItemNumberConst = steel.ItemNumber.Split(".");
                        var ItemNumberFixed = "";
                        foreach (var item in ItemNumberConst)
                            ItemNumberFixed += item;

                        var a = workSheet.Cell($"N{counter}").GetString();

                        var workFrontExcel = workFronts.FirstOrDefault(x => x.Code == workSheet.Cell($"N{counter}").GetString());

                        if (workFrontExcel == null)
                            return BadRequest($"El frente de trabajo en la fila N{counter} no existe");

                        steel.WorkFrontId = workFrontExcel.Id;
                        
                        var phaseAux = phases
                            .Where(x=>x.ProjectFormulaId == model.ProjectFormulaId)
                            .FirstOrDefault(x => x.Code == ItemNumberFixed);
                        var workFrontAux = workFront;
                        if (phaseAux != null)
                        {
                            var description = workSheet.Cell($"N{counter}").GetString();
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
                           
                            var baseE = steel.OrderNumber - 1;
                            var element1 = steels.FirstOrDefault(x => x.OrderNumber == baseE);
                            var elementBase = steels.FirstOrDefault(x => x.OrderNumber == steel.OrderNumber);
                            var lengthBase = steel.ItemNumber.Length;
                            int i = 1;
                            if (element1 != null)
                            {
                                while (element1.ItemNumber.Length <= lengthBase)
                                {
                                    //element1.WorkFrontId = workFrontAux.Id;
                                    element1.ProjectPhaseId = phase.Id;
                                    element1 = steels.FirstOrDefault(x => x.OrderNumber == (baseE - i));
                                    elementBase = steels.FirstOrDefault(x => x.OrderNumber == (baseE - i + 1));
                                    lengthBase = elementBase.ItemNumber.Length;
                                    if (element1 == null)
                                        break;
                                    else
                                        i++;
                                }
                            }


                            workFront = workFrontAux;
                        }
                        

                        steel.ProjectFormulaId = model.ProjectFormulaId;
                        
                        steel.BudgetTitleId = model.BudgetTitleId;
                        steel.Description = workSheet.Cell($"C{counter}").GetString();
                        steel.Unit = workSheet.Cell($"D{counter}").GetString();
                        steel.ProjectPhaseId = phase.Id;

                        /*
                        if (budgetTitle.Name == "Contractual Original")
                            steel.WorkFrontId = workFront.Id;
                        else
                            steel.WorkFrontId = Guid.Empty;
                        */

                        var meteredExcel = workSheet.Cell($"E{counter}").GetString();
                        if (!string.IsNullOrEmpty(meteredExcel))
                        {
                            if (Double.TryParse(meteredExcel, out double metered))
                                steel.ContractualMetered = Math.Round(metered, 2);
                        }

                        var rod6mmExcel = workSheet.Cell($"F{counter}").GetString();
                        var rod6mmKg = 0.0;
                        
                        if (!string.IsNullOrEmpty(rod6mmExcel))
                        {
                            if (Double.TryParse(rod6mmExcel, out double rod6mm))
                                steel.Rod6mm = Math.Round(rod6mm, 2);
                            rod6mmKg = steel.Rod6mm * rod6mmVariable.NominalWeight;
                        }

                        var rod8mmExcel = workSheet.Cell($"G{counter}").GetString();
                        var rod8mmKg = 0.0;

                        if (!string.IsNullOrEmpty(rod8mmExcel))
                        {
                            if (Double.TryParse(rod8mmExcel, out double rod8mm))
                                steel.Rod8mm = Math.Round(rod8mm, 2);
                            rod8mmKg = steel.Rod8mm * rod8mmVariable.NominalWeight;
                        }

                        var rod3x8Excel = workSheet.Cell($"H{counter}").GetString();
                        var rod3x8Kg = 0.0;
                        
                        if (!string.IsNullOrEmpty(rod3x8Excel))
                        {
                            if (Double.TryParse(rod3x8Excel, out double rod3x8))
                                steel.Rod3x8 = Math.Round(rod3x8, 2);
                            rod3x8Kg = steel.Rod3x8 * rod3x8Variable.NominalWeight;
                        }

                        var rod1x2Excel = workSheet.Cell($"I{counter}").GetString();
                        var rod1x2Kg = 0.0;
                        
                        if (!string.IsNullOrEmpty(rod1x2Excel))
                        {
                            if (Double.TryParse(rod1x2Excel, out double rod1x2))
                                steel.Rod1x2 = Math.Round(rod1x2, 2);
                            rod1x2Kg = steel.Rod1x2 * rod1x2Variable.NominalWeight;
                        }

                        var rod5x8Excel = workSheet.Cell($"J{counter}").GetString();
                        var rod5x8Kg = 0.0;
                        
                        if (!string.IsNullOrEmpty(rod5x8Excel))
                        {
                            if (Double.TryParse(rod5x8Excel, out double rod5x8))
                                steel.Rod5x8 = Math.Round(rod5x8, 2);
                            rod5x8Kg = steel.Rod5x8 * rod5x8Variable.NominalWeight;
                        }

                        var rod3x4Excel = workSheet.Cell($"K{counter}").GetString();
                        var rod3x4Kg = 0.0;
                        
                        if (!string.IsNullOrEmpty(rod3x4Excel))
                        {
                            if (Double.TryParse(rod3x4Excel, out double rod3x4))
                                steel.Rod3x4 = Math.Round(rod3x4, 2);
                            rod3x4Kg = steel.Rod3x4 * rod3x4Variable.NominalWeight;
                        }

                        var rod1Excel = workSheet.Cell($"L{counter}").GetString();
                        var rod1Kg = 0.0;
                        
                        if (!string.IsNullOrEmpty(rod1Excel))
                        {
                            if (Double.TryParse(rod1Excel, out double rod1))
                                steel.Rod1 = Math.Round(rod1, 2);
                            rod1Kg = steel.Rod1 * rod1Variable.NominalWeight;
                        }


                        var contractual = Math.Round((rod6mmKg + rod8mmKg + rod3x8Kg + rod1x2Kg + rod5x8Kg + rod3x4Kg + rod1Kg)*ConstantHelpers.Steel.LENGTH, 2);
                        steel.ContractualStaked = contractual;

                        if (steel.ContractualMetered > 0)
                        {
                            rod6mmSuma += steel.Rod6mm;
                            rod8mmSuma += steel.Rod8mm;
                            rod3x8Suma += steel.Rod3x8;
                            rod1x2Suma += steel.Rod1x2;
                            rod5x8Suma += steel.Rod5x8;
                            rod3x4Suma += steel.Rod3x4;
                            rod1Suma += steel.Rod1;
                            contractualSuma += steel.ContractualStaked;
                        }
                   
                        steels.Add(steel);
                        counter++;
                        aux++;
                    }

                }
                mem.Close();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var bulkConfig = new BulkConfig { PreserveInsertOrder = true, SetOutputIdentity = true };
                await _context.BulkInsertAsync(steels);

                transaction.Commit();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "AceroCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "Item";
                workSheet.Range("B2:B4").Merge();

                workSheet.Cell($"C2").Value = "Descripción";
                workSheet.Range("C2:C4").Merge();

                workSheet.Cell($"D2").Value = "Und.";
                workSheet.Range("D2:D4").Merge();

                workSheet.Cell($"E2").Value = "Metrado Contractual";
                workSheet.Range("E2:E4").Merge();

                workSheet.Cell($"F2").Value = "REPLANTEOS";
                workSheet.Range("F2:L2").Merge();

                workSheet.Cell($"F3").Value = "VARILLAS (UND)";
                workSheet.Range("F3:L3").Merge();

                workSheet.Cell($"F4").Value = "6mm";
                workSheet.Cell($"G4").Value = "8mm";
                workSheet.Cell($"H4").Value = "3" + " / " + "8\"";
                workSheet.Cell($"I4").Value = "1" + " / " + "2\"";
                workSheet.Cell($"J4").Value = "5" + " / " + "8\"";
                workSheet.Cell($"K4").Value = "3" + " / " + "4\"";
                workSheet.Cell($"L4").Value = "1\"";

                workSheet.Cell($"M3").Value = "Kg";
                workSheet.Range("M3:M4").Merge();

                workSheet.Cell($"N2").Value = "Frente";
                workSheet.Range("N2:N4").Merge();

                workSheet.Cell($"B6").Value = "Info Aquí";
                workSheet.Cell($"B6").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 13;
                workSheet.Column(3).Width = 75;
                workSheet.Column(4).Width = 6;
                workSheet.Column(5).Width = 20;
                workSheet.Column(6).Width = 8;
                workSheet.Column(7).Width = 8;
                workSheet.Column(8).Width = 8;
                workSheet.Column(9).Width = 8;
                workSheet.Column(10).Width = 8;
                workSheet.Column(11).Width = 8;
                workSheet.Column(12).Width = 8;
                workSheet.Column(13).Width = 12;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var originTypes = _context.OriginTypeFillingLaboratories.AsNoTracking().ToList();

                var workFrontsPhases = _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.ProjectPhase.ProjectFormula)
                    .Include(x => x.WorkFront)
                    .Include(x => x.ProjectPhase)
                    .Where(x => x.ProjectPhase.ProjectId == GetProjectId())
                    .AsNoTracking().ToList();

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

                workSheet.Range("B2:N4").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:N4").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("metrado-contractual")]
        public async Task<IActionResult> GetContractualMetered(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var contractualMeteredSuma = 0.0;

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
                contractualMeteredSuma += item.ContractualMetered;
            }

            return Ok(contractualMeteredSuma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod6mm")]
        public async Task<IActionResult> GetRod6mm(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod6mmSuma = 0.0;

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
                rod6mmSuma += item.Rod6mm;
            }

            return Ok(rod6mmSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod8mm")]
        public async Task<IActionResult> GetRod8mm(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod8mmSuma = 0.0;

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
                rod8mmSuma += item.Rod8mm;
            }

            return Ok(rod8mmSuma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod3x8")]
        public async Task<IActionResult> GetRod3x8(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod3x8Suma = 0.0;

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
                rod3x8Suma += item.Rod3x8;
            }

            return Ok(rod3x8Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod1x2")]
        public async Task<IActionResult> GetRod1x2(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod1x2Suma = 0.0;

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
                rod1x2Suma += item.Rod1x2;
            }

            return Ok(rod1x2Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod5x8")]
        public async Task<IActionResult> GetRod5x8(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod5x8Suma = 0.0;

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
                rod5x8Suma += item.Rod5x8;
            }

            return Ok(rod5x8Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod3x4")]
        public async Task<IActionResult> GetRod3x4(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod3x4Suma = 0.0;

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
                rod3x4Suma += item.Rod3x4;
            }

            return Ok(rod3x4Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("rod1")]
        public async Task<IActionResult> GetRod1(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var rod1Suma = 0.0;

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
                rod1Suma += item.Rod1;
            }

            return Ok(rod1Suma.ToString("N0", CultureInfo.InvariantCulture));
        }

        [HttpGet("metrado-replanteado")]
        public async Task<IActionResult> GetcontractualStaked(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.ContractualMetered != 0);

            var contractualStakedSuma = 0.0;

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
                contractualStakedSuma += item.ContractualStaked;
            }

            return Ok(contractualStakedSuma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.Steels
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

            _context.Steels.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
