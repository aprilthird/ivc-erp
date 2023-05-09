using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.General;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BlueprintViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/planos")]
    public class BlueprintController : BaseController
    {

        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BlueprintController(IvcDbContext context,
            ILogger<BlueprintController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? phaseId = null ,Guid? budgetId = null,Guid? projectFormulaId = null, Guid? workFrontId = null, Guid? specId = null, Guid? versionId = null, Guid? typeId = null)
        {


            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@VersionId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)versionId ?? DBNull.Value;

            var data = await _context.Set<UspBlueprint>().FromSqlRaw("execute TechnicalOffice_uspBluePrint @VersionId"
    , param1)
.IgnoreQueryFilters()
.ToListAsync();

            data = data.Where(x => x.ProjectId == pId).ToList();

            if (budgetId.HasValue)
                data = data.Where(x=>x.BudgetTitleId == budgetId.Value).ToList();

            if (projectFormulaId.HasValue)
            {
                data = data.Where(x => x.ProjectFormulaId == projectFormulaId.Value).ToList();
            }

            if (workFrontId.HasValue)
            {
                data = data.Where(x => x.WorkFrontId == workFrontId.Value).ToList();
            }

            if (specId.HasValue)
            {
                data = data.Where(x => x.SpecialityId == specId.Value).ToList();
            }

            if(phaseId.HasValue)
            {
                data = data.Where(x=>x.ProjectPhaseId == phaseId.Value).ToList();
            }

            if (typeId.HasValue)
            {
                data = data.Where(x => x.BlueprintTypeId == typeId.Value).ToList();
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Blueprints
                 .Where(x => x.Id == id)
                 .Select(x => new BlueprintViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     ProjectFormulaId = x.ProjectFormulaId,


                     BudgetTitleId = x.BudgetTitleId,
                     BlueprintTypeId = x.BlueprintTypeId.Value,

                     SpecialityId = x.SpecialityId,

                     ProjectPhaseId = x.ProjectPhaseId,
                     WorkFrontId = x.WorkFrontId,
                     Description = x.Description,
                     Sheet = x.Sheet,
                     //BlueprintDateStr = x.BlueprintDate.Date.ToDateString(),

                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BlueprintViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = new Blueprint
            {
                Name = model.Name,
                ProjectFormulaId = model.ProjectFormulaId,
                BudgetTitleId = model.BudgetTitleId,
                SpecialityId = model.SpecialityId,
                BlueprintTypeId = model.BlueprintTypeId,
                ProjectId = GetProjectId(),
                WorkFrontId = model.WorkFrontId,
                Sheet = model.Sheet,
                Description = model.Description,
                ProjectPhaseId = model.ProjectPhaseId

            };

            await _context.Blueprints.AddAsync(bprint);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BlueprintViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = await _context.Blueprints
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            bprint.Name = model.Name;
            bprint.ProjectFormulaId = model.ProjectFormulaId;
            bprint.BudgetTitleId = model.BudgetTitleId;
            bprint.SpecialityId = model.SpecialityId;
            bprint.WorkFrontId = model.WorkFrontId;
            bprint.BlueprintTypeId = model.BlueprintTypeId;
            bprint.Sheet = model.Sheet;
            bprint.Description = model.Description;
            bprint.ProjectPhaseId = model.ProjectPhaseId;

            await _context.SaveChangesAsync();

            var bprintfolding = await _context.BlueprintFoldings
                .FirstOrDefaultAsync(x => x.BlueprintId.Equals(id));

            if (bprintfolding != null)
                bprintfolding.Code = model.Description + "-" + model.Sheet;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.Blueprints
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"plano con Id '{id}' no se halló.");
            }

            _context.Blueprints.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {


            var pId = GetProjectId();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 4;

                    
                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {
                        var selected = await _context.Blueprints
                           .FirstOrDefaultAsync(x => x.Description == workSheet.Cell($"H{counter}").GetString() && x.Sheet == workSheet.Cell($"I{counter}").GetString());
                        var countselected = await _context.Blueprints
                            .Where(x => x.Description == workSheet.Cell($"H{counter}").GetString() && x.Sheet == workSheet.Cell($"I{counter}").GetString())
                            .ToListAsync();
                        var bplist = new List<Blueprint>();
                        var fbplist = new List<BlueprintFolding>();
                        if (countselected.Count == 0)
                        { 
                        var bprint = new Blueprint();
                        var bfolding = new BlueprintFolding();
                        var budgetselected = await _context.BudgetTitles
                            .FirstOrDefaultAsync(x => x.Name == workSheet.Cell($"C{counter}").GetString() && x.ProjectId == pId);
                        var formulaselected = await _context.ProjectFormulas
                            .FirstOrDefaultAsync(x => x.Name == workSheet.Cell($"D{counter}").GetString() && x.ProjectId == pId);
                        var frontselected = await _context.WorkFronts
                            .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"E{counter}").GetString() && x.ProjectId == pId);
                        var specselected = await _context.Specialities
                            .FirstOrDefaultAsync(x => x.Description == workSheet.Cell($"F{counter}").GetString() && x.ProjectId == pId);
                        var phaseselected = await _context.ProjectPhases
                            .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"M{counter}").GetString() || x.Description == workSheet.Cell($"M{counter}").GetString() && x.ProjectId == pId);

                            var versionselected = await _context.TechnicalVersions
                            .FirstOrDefaultAsync(x => x.Description == workSheet.Cell($"K{counter}").GetString() && x.ProjectId == pId);

                            var typeselected = await _context.BlueprintTypes
                            .FirstOrDefaultAsync(x => x.Description == workSheet.Cell($"N{counter}").GetString() && x.ProjectId == pId);

                            var letterselected = await _context.Letters
                            .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"L{counter}").GetString() && x.ProjectId == pId);
                        //---------------Creación del For05
                         
                        bprint.Id = Guid.NewGuid();
                        bprint.ProjectId = pId;

                        bprint.BudgetTitleId = budgetselected.Id;
                        bprint.ProjectFormulaId = formulaselected.Id;
                        bprint.WorkFrontId = frontselected.Id;
                        bprint.SpecialityId = specselected.Id;
                            bprint.BlueprintTypeId = typeselected.Id;
                        bprint.Name = workSheet.Cell($"G{counter}").GetString();
                        bprint.Description = workSheet.Cell($"H{counter}").GetString();
                        bprint.Sheet = workSheet.Cell($"I{counter}").GetString();
                            if(phaseselected!=null)
                            bprint.ProjectPhaseId = phaseselected.Id;

                       

                        //bprint.TechnicalVersionId = versionselected.Id;
                        //if (letterselected != null)
                        //    bprint.LetterId = letterselected.Id;



                        bplist.Add(bprint);
                        await _context.Blueprints.AddRangeAsync(bplist);
                        await _context.SaveChangesAsync();


                        bfolding.Id = Guid.NewGuid();
                        bfolding.BlueprintId = bprint.Id;
                            bfolding.Code = bprint.Description + "-" + bprint.Sheet;
                        bfolding.TechnicalVersionId = versionselected.Id;
                        if (letterselected != null)
                            bfolding.LetterId = letterselected.Id;
                            if (!workSheet.Cell($"J{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"J{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {

                                        var date = workSheet.Cell($"J{counter}").GetDateTime();
                                        var newdate = new DateTime(date.Year, date.Month, date.Day);
                                        bfolding.BlueprintDate = newdate;
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"J{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        bfolding.BlueprintDate = date.ToShortDateString().ToDateTime();
                                }
                            }
                            fbplist.Add(bfolding);
                        await _context.BlueprintFoldings.AddRangeAsync(bfolding);
                        await _context.SaveChangesAsync();
                        ++counter;
                        }

                        if (countselected.Count > 0)
                        {
                           
                            var bfolding = new BlueprintFolding();
                           
                            var versionselected = await _context.TechnicalVersions
                                .FirstOrDefaultAsync(x => x.Description == workSheet.Cell($"K{counter}").GetString() && x.ProjectId == pId);

                            var letterselected = await _context.Letters
                                .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"L{counter}").GetString() && x.ProjectId == pId);
                            //---------------Creación del For05

                            


                            bfolding.Id = Guid.NewGuid();
                            bfolding.BlueprintId = selected.Id;
                            bfolding.Code = selected.Description + "-" + selected.Sheet;
                            bfolding.TechnicalVersionId = versionselected.Id;
                            if (letterselected != null)
                                bfolding.LetterId = letterselected.Id;

                            if (!workSheet.Cell($"J{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"J{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {

                                        var date = workSheet.Cell($"J{counter}").GetDateTime();
                                        var newdate = new DateTime(date.Year, date.Month, date.Day);
                                        bfolding.BlueprintDate = newdate;
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"J{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        bfolding.BlueprintDate = date.ToShortDateString().ToDateTime();
                                }
                            }
                            fbplist.Add(bfolding);
                            await _context.BlueprintFoldings.AddRangeAsync(bfolding);
                            await _context.SaveChangesAsync();
                            ++counter;
                        }
                    }




                }
                mem.Close();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public async Task<FileResult> ExportExcelMassiveLoadAsync(Guid? formulaId = null)
        {


            var pId = GetProjectId();

            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter formulaParam = new SqlParameter("@FormulaId", SqlDbType.UniqueIdentifier);
            formulaParam.Value = (object)formulaId ?? DBNull.Value;

            var data = await _context.Set<UspWorkFront>().FromSqlRaw("execute Admin_uspWorkFrontsNonFilter @ProjectId, @FormulaId"
            , projectParam, formulaParam)
            .IgnoreQueryFilters()
            .ToListAsync();

            string fileName = "CargaMasivaPlanos.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"C2").Value = "Presupuesto";
                workSheet.Range("C2:C3").Merge();
                workSheet.Range("C2:C3").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"D2").Value = "Fórmula";
                workSheet.Range("D2:D3").Merge();
                workSheet.Range("D2:D3").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"E2").Value = "Frente";
                workSheet.Range("E2:E3").Merge();
                workSheet.Range("E2:E3").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"F2").Value = "Especialidad";
                workSheet.Range("F2:F3").Merge();
                workSheet.Range("F2:F3").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"G2").Value = "Nombre";
                workSheet.Range("G2:G3").Merge();
                workSheet.Range("G2:G3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H2").Value = "Código";
                workSheet.Range("H2:H3").Merge();
                workSheet.Range("H2:H3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"I2").Value = "Lámina";
                workSheet.Range("I2:I3").Merge();
                workSheet.Range("I2:I3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"J2").Value = "Fecha de Aprobación";
                workSheet.Range("J2:J3").Merge();
                workSheet.Range("J2:J3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"K2").Value = "Versión";
                workSheet.Range("K2:K3").Merge();
                workSheet.Range("K2:K3").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);

                workSheet.Cell($"L2").Value = "Carta Aprobada";
                workSheet.Range("L2:L3").Merge();
                workSheet.Range("L2:L3").Style.Fill.SetBackgroundColor(XLColor.AliceBlue);

                workSheet.Cell($"M2").Value = "Fase (Código)";
                workSheet.Range("M2:M3").Merge();
                workSheet.Range("M2:M3").Style.Fill.SetBackgroundColor(XLColor.AliceBlue);

                workSheet.Cell($"N2").Value = "Tipo de Plano";
                workSheet.Range("N2:N3").Merge();
                workSheet.Range("N2:N3").Style.Fill.SetBackgroundColor(XLColor.AliceBlue);

                workSheet.Cell($"C1").Value = "Contractual Original";
                

                workSheet.Cell($"D1").Value = "OBRAS CIVILES";
                

                workSheet.Cell($"E1").Value = "FT-01";
                

                workSheet.Cell($"F1").Value = "MOV. DE TIERRAS";
                

                workSheet.Cell($"G1").Value = "ejemplo";
                

                workSheet.Cell($"H1").Value = "ej-em-pl-oo";
                

                workSheet.Cell($"I1").Value = "01-99";
                

                workSheet.Cell($"J1").Value = "21/07/2021";
                

                workSheet.Cell($"K1").Value = "Contractual";

                workSheet.Cell($"L1").Value = "0125-2021";
                workSheet.Cell($"M1").Value = "0";



                workSheet.Cell($"A2").Value = "Campo obligatorio";
                workSheet.Cell($"A2").Style.Fill.SetBackgroundColor(XLColor.GreenYellow);
                workSheet.Cell($"A3").Value = "Campo para llenar";
                workSheet.Cell($"A3").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                workSheet.Cell($"A4").Value = "Campo opcional";
                workSheet.Cell($"A4").Style.Fill.SetBackgroundColor(XLColor.AliceBlue);


                workSheet.Column(1).Width = 20;

                workSheet.Column(3).Width = 27;
                workSheet.Column(4).Width = 25;
                workSheet.Column(5).Width = 25;
                workSheet.Column(6).Width = 20;
                workSheet.Column(7).Width = 12;
                workSheet.Column(8).Width = 12;
                workSheet.Column(9).Width = 17;
                workSheet.Column(10).Width = 25;
                workSheet.Column(11).Width = 25;
                workSheet.Column(12).Width = 25;
                workSheet.Column(13).Width = 25;


                workSheet.Rows().AdjustToContents();
                workSheet.Range("C2:N3").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("C2:N3").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var budgetList = await _context.BudgetTitles.Where(x => x.ProjectId == pId).ToListAsync();
                var formulaList = await _context.ProjectFormulas.Where(x => x.ProjectId == pId).ToListAsync();
                var frontList = await _context.WorkFronts.Where(x => x.ProjectId == pId).ToListAsync();
                var specList = await _context.Specialities.Where(x => x.ProjectId == pId).ToListAsync();
                var versionList = await _context.TechnicalVersions.Where(x => x.ProjectId == pId).ToListAsync();
                var phasesList = await _context.ProjectPhases.Include(x=>x.ProjectFormula).Where(x => x.ProjectId == pId).ToListAsync();
                var typeList = await _context.BlueprintTypes.Where(x => x.ProjectId == pId).ToListAsync();
                //TITULOS DE PRESUPUESTO

                DataTable dtBT = new DataTable();
                dtBT.TableName = "Tìtulos de Presupuesto";
                dtBT.Columns.Add("Titulos de Presupuesto", typeof(string));
                var budgets = budgetList;
                foreach (var item in budgets)
                    dtBT.Rows.Add(item.Name);
                dtBT.AcceptChanges();

                var worksheetBT = wb.Worksheets.Add(dtBT);
                worksheetBT.Column(1).Width = 50;

                //FORMULAS

                DataTable dtPF = new DataTable();
                dtPF.TableName = "Fórmulas";
                dtPF.Columns.Add("Fórmulas", typeof(string));
                var formulas = formulaList;
                foreach (var item in formulas)
                    dtPF.Rows.Add(item.Name);
                dtPF.AcceptChanges();

                var worksheetPF = wb.Worksheets.Add(dtPF);
                worksheetPF.Column(1).Width = 45;

                //FRENTES

                DataTable dtWF = new DataTable();
                dtWF.TableName = "Frentes";
                dtWF.Columns.Add("Frentes", typeof(string));
                var fronts = frontList;
                foreach (var item in data)
                    dtWF.Rows.Add(item.Code.ToString() +" _ "+item.FormulaCodes.ToString());
                dtWF.AcceptChanges();

                var worksheetWF = wb.Worksheets.Add(dtWF);
                worksheetWF.Column(1).Width = 30;

                //ESPECIALIDADES
                DataTable dtSP = new DataTable();
                dtSP.TableName = "Especialidades";
                dtSP.Columns.Add("Especialidades", typeof(string));
                var specs = specList;
                foreach (var item in specs)
                    dtSP.Rows.Add(item.Description);
                dtSP.AcceptChanges();

                var worksheetSP = wb.Worksheets.Add(dtSP);
                worksheetSP.Column(1).Width = 40;

                //VERSIONES

                DataTable dtTV = new DataTable();
                dtTV.TableName = "Versiones";
                dtTV.Columns.Add("Versiones", typeof(string));
                var vers = versionList;
                foreach (var item in vers)
                    dtTV.Rows.Add(item.Description);
                dtTV.AcceptChanges();

                var worksheetTV = wb.Worksheets.Add(dtTV);
                worksheetTV.Column(1).Width = 30;
                //FASES
                DataTable dtP = new DataTable();
                dtP.TableName = "Fases";
                dtP.Columns.Add("Fases", typeof(string));
                var phases = phasesList;
                foreach (var item in phases)
                {
                    if(item.ProjectFormulaId != null)
                    dtP.Rows.Add(item.Code.ToString() +" _ " +item.ProjectFormula.Code);
                    else
                        dtP.Rows.Add(item.Code.ToString());
                }
                dtP.AcceptChanges();

                var worksheetP = wb.Worksheets.Add(dtP);
                worksheetP.Column(1).Width = 30;


                //TIPOS
                DataTable dtBPT = new DataTable();
                dtBPT.TableName = "Tipos de Planos";
                dtBPT.Columns.Add("Tipos de Planos", typeof(string));
                var types = typeList;
                foreach (var item in types)
                {
                    
                        dtBPT.Rows.Add(item.Description.ToString());
                }
                dtBPT.AcceptChanges();

                var worksheetBPT = wb.Worksheets.Add(dtBPT);
                worksheetBPT.Column(1).Width = 30;

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("importar-archivos")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> ImportFiles(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var archive = new ZipArchive(mem))
                {
                    var entries = archive.Entries.Where(x => !string.IsNullOrEmpty(x.Name)).ToList();
                    foreach (var entry in entries)
                    {
                        var storage = new CloudStorageService(_storageCredentials);
                        var bp = await _context.BlueprintFoldings.Include(x=>x.TechnicalVersion).FirstOrDefaultAsync(x => entry.Name.Contains(x.Code+"_"+x.TechnicalVersion.Description));
                        if (bp != null && bp.FileUrl != null)
                        {
                            await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BLUEPRINT}/{bp.FileUrl.AbsolutePath.Split('/').Last()}",
                                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                            bp.FileUrl = await storage.UploadFile(entry.Open(),
                                 ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                                 System.IO.Path.GetExtension(entry.Name),
                                 ConstantHelpers.Storage.Blobs.BLUEPRINT,
                                 $"plano_{bp.Code}_{bp.TechnicalVersion.Description}");
                            await _context.SaveChangesAsync();
                        }
                        if (bp != null && bp.FileUrl == null)
                        {
                            bp.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                                System.IO.Path.GetExtension(entry.Name),
                                ConstantHelpers.Storage.Blobs.BLUEPRINT,
                                $"plano_{bp.Code}_{bp.TechnicalVersion.Description}");
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            return Ok();
        }

        [HttpGet("reporte-planos")]
        public async Task<IActionResult> ExcelReport(Guid? versionId = null)
        {

            var pId = GetProjectId();




            var data = await _context.Set<UspBluePrintReport>().FromSqlRaw("execute TechnicalOffice_uspBluePrintReport ")
            .IgnoreQueryFilters()
            .ToListAsync();

            data = data.Where(x => x.ProjectId == pId).ToList();



            var project = _context.Projects.Where(x => x.Id == pId).FirstOrDefault();


            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add("Planos");

                var enlace = project.LogoUrl.ToString();

                WebClient client = new WebClient();
                Stream img = client.OpenRead(enlace);
                Bitmap bitmap; bitmap = new Bitmap(img);

                Image image = (Image)bitmap;

                ws.Range($"A1:B3").Merge();

                var aux = ws.AddPicture(bitmap);
                aux.MoveTo(10, 10);
                aux.Height = 45;
                aux.Width = 250;

                ws.Cell($"C1").Value = "CONTROL DE PLANOS";
                ws.Range($"C1:M1").Merge();
                SetRowBorderStyle(ws, 1, "O");
                ws.Cell($"N1").Value = "Código";
                ws.Cell($"N2").Value = "Versión";
                ws.Cell($"N3").Value = "Fecha";

                if (project.CostCenter == "050-2018")
                    ws.Cell($"O1").Value = "JIC/GCV-For-11";
                if (project.CostCenter == "001")
                    ws.Cell($"O1").Value = "CSH/GCV-For-11";

                
                ws.Cell($"O2").Value = "1";
                ws.Cell($"O3").Value = "28/09/2021";

                ws.Cell($"C2").Value = "PROGRAMA DE CALIBRACION Y CONTROL DE EQUIPOS DE MEDICION EXTERNA";
                ws.Range($"C2:M3").Merge();

                ws.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                SetRowBorderStyle(ws, 2, "O");
                SetRowBorderStyle(ws, 3, "O");


                var count = 8;
                //ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"A{count}").Value = "Presupuesto";
                ws.Range($"A{count}:A{count+1}").Merge();

                ws.Cell($"B{count}").Value = "Fórmula";
                ws.Range($"B{count}:B{count+1}").Merge();

                ws.Cell($"C{count}").Value = "Frente";
                ws.Range($"C{count}:C{count+1}").Merge();

                ws.Cell($"D{count}").Value = "Especialidad";
                ws.Range($"D{count}:D{count+1}").Merge();

                ws.Cell($"E{count}").Value = "Tipo de Plano";
                ws.Range($"E{count}:E{count + 1}").Merge();

                ws.Cell($"F{count}").Value = "Nombre";
                ws.Range($"F{count}:F{count+1}").Merge();

                ws.Cell($"G{count}").Value = "Código";
                ws.Range($"G{count}:G{count+1}").Merge();

                ws.Cell($"H{count}").Value = "Lámina";
                ws.Range($"H{count}:H{count+1}").Merge();

                ws.Cell($"I{count}").Value = "Fecha de Aprobación";
                ws.Range($"I{count}:I{count+1}").Merge();

                ws.Cell($"J{count}").Value = "Versión";
                ws.Range($"J{count}:J{count+1}").Merge();

                ws.Cell($"K{count}").Value = "Carta Aprobada";
                ws.Range($"K{count}:K{count+1}").Merge();

                ws.Cell($"L{count}").Value = "Fase (Código)";
                ws.Range($"L{count}:L{count+1}").Merge();

                ws.Cell($"M{count}").Value = "Última Entrega";
                ws.Range($"M{count}:N{count}").Merge();

                ws.Cell($"M{count+1}").Value = "Fecha";

                ws.Cell($"N{count + 1}").Value = "Entregado a";

                ws.Cell($"O{count}").Value = "Cantidad de Entregas";
                ws.Range($"O{count}:O{count+1}").Merge();

                SetRowBorderStyle2(ws, count, "O");
                SetRowBorderStyle2(ws, count + 1, "O");
                ws.Row(count).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(count + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Range($"A{count}:O{count}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));
                ws.Range($"A{count + 1}:O{count + 1}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));


                count = 10;
                //ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(8).Style.NumberFormat.Format = "d-mm-yy";

                foreach (var first in data)
                {
                    ws.Cell($"A{count}").Value = first.BudgetTitleName;
                    ws.Cell($"B{count}").Value = first.ProjectFormulaName;
                    ws.Cell($"C{count}").Value = first.WorkFrontCode;
                    ws.Cell($"D{count}").Value = first.SpecialityDescription;

                    ws.Cell($"E{count}").Value = first.BlueprintTypeDescription;

                    ws.Cell($"F{count}").Value = first.Name;

                    ws.Cell($"G{count}").Value = first.Description;
                    ws.Cell($"H{count}").Value = first.Sheet;
                    if (first.BlueprintDate.HasValue)
                    {
                        DateTime dateTime = first.BlueprintDate.Value;
                        ws.Cell($"I{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"I{count}").Value = string.Empty;
                    }
                    ws.Cell($"J{count}").Value = first.TechnicalVersionDescription;


                    ws.Cell($"K{count}").Value = first.LetterName;
                    ws.Cell($"L{count}").Value = first.ProjectPhaseCode+"-"+first.ProjectPhaseDescription;

                    if (first.DateType.HasValue)
                    {
                        DateTime dateTime = first.DateType.Value;
                        ws.Cell($"M{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"M{count}").Value = string.Empty;
                    }
                    ws.Cell($"N{count}").Value = first.UserName;
                    ws.Cell($"O{count}").Value = first.Quantity;
                    count++;
                    SetRowBorderStyle2(ws, count - 1, "O");

                }


                ws.Column(1).Width = 20;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 27;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 12;
                ws.Column(8).Width = 12;
                ws.Column(9).Width = 17;
                ws.Column(10).Width = 25;
                ws.Column(11).Width = 25;
                ws.Column(12).Width = 40;
                ws.Column(13).Width = 25;
                ws.Column(14).Width = 25;
                ws.Column(15).Width = 25;
                ws.Rows().AdjustToContents();






                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de Planos.xlsx");
                }

            }

        }

        private void SetRowBorderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetRowBorderStyle2(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        [HttpGet("qr/{id}")]
        public async Task<IActionResult> GeneratePhotocheck(Guid id)
        {
            var b = await _context.BlueprintFoldings
                .Include(x => x.Blueprint)
                .Include(x => x.Blueprint.BlueprintType)
                .Include(x => x.Blueprint.Speciality)
                .Include(x => x.TechnicalVersion)
                .FirstOrDefaultAsync(x => x.Id == id);


            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(b.Code + "/" + b.TechnicalVersion.Description, QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);

                PdfDocument doc = new PdfDocument();

                PdfPageBase page = doc.Pages.Add(new SizeF(580, 220), new PdfMargins(0));

                PdfTrueTypeFont font = new PdfTrueTypeFont("Helvetica", 20f, PdfFontStyle.Bold, true);

                //page.Canvas.DrawString(bprint.OrderItem.Supply.Description, new PdfFont(PdfFontFamily.Helvetica, 12f), new PdfSolidBrush(Color.Black), 180, 30);

                //page.Canvas.DrawString(b.Blueprint.Name, font, new PdfSolidBrush(Color.Black), new RectangleF(220, 30, 350, 65));

                page.Canvas.DrawString(b.Blueprint.BlueprintTypeId != null ? b.Blueprint.BlueprintType.Description : "No hay Tipo", font, new PdfSolidBrush(Color.Black), 220, 20);
                //page.Canvas.DrawString(b.Blueprint.Name, font, new PdfSolidBrush(Color.Black), 230, 55);

                page.Canvas.DrawString(b.Blueprint.Name, font, new PdfSolidBrush(Color.Black), new RectangleF(220, 45, 350, 65));

                //page.Canvas.DrawString(b.Blueprint.Name, font, new PdfSolidBrush(Color.Black), new RectangleF(220, 30, 350, 100));
                page.Canvas.DrawString(b.Blueprint.Speciality.Description, font, new PdfSolidBrush(Color.Black), 220, 95);
                page.Canvas.DrawString(b.Blueprint.Description, font, new PdfSolidBrush(Color.Black), 220, 120);
                page.Canvas.DrawString("Lámina " + b.Blueprint.Sheet, font, new PdfSolidBrush(Color.Black), 220, 145);
                page.Canvas.DrawString(b.TechnicalVersion.Description, font, new PdfSolidBrush(Color.Black), 220, 170);
                
                MemoryStream img = new MemoryStream();


                using (Bitmap bitMap = qrCode.GetGraphic(8))
                {
                    Bitmap resized = new Bitmap(bitMap, new Size(290, 290));

                    resized.Save(img, ImageFormat.Png);

                    PdfImage image = PdfImage.FromStream(img);

                    page.Canvas.DrawImage(image, new PointF(0, 0));
                    doc.SaveToStream(ms);
                    doc.Close();
                    return File(ms.ToArray(), "application/pdf", b.Code + "/" + b.TechnicalVersion.Description + ".pdf");
                }

            }
        }
    }
}
