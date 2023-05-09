using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor05ViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/for05-colector-descarga")]
    public class SewerManifoldFor05Controller : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor05Controller(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor05Controller> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.SewerManifoldFor05s
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .Where(x => x.ProjectId == pId);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerManifold.ProductionDailyPart.SewerGroupId == sewerGroupId);

            var data = await query
                .Select(x => new SewerManifoldFor05ViewModel
                {
                    Id = x.Id,
                    CertificateNumber = x.CertificateNumber,
                    SewerManifoldId = x.SewerManifoldId,
                    LayerNumber = x.LayerNumber,
                    TestDate = x.TestDate.ToDateString(),
                    Status = x.Status,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        ProductionDailyPart = new PdpViewModel
                        {
                            SewerGroupId = x.SewerManifold.ProductionDailyPart.SewerGroupId,
                            SewerGroup = new SewerGroupViewModel
                            {
                                Code = x.SewerManifold.ProductionDailyPart.SewerGroup.Code,
                                WorkFrontHeadId = x.SewerManifold.ProductionDailyPart.SewerGroup.WorkFrontHeadId
                            }
                        },
                        Address = x.SewerManifold.Address,
                        Name = x.SewerManifold.Name,
                        LengthOfDigging = x.SewerManifold.LengthOfDigging,
                        DitchHeight = x.SewerManifold.DitchHeight
                    },
                    ShippingDate = x.ShippingDate.ToDateString(),
                    Filling = x.Filling,
                    TheoreticalLayer = x.TheoreticalLayer,
                    LayersNumber = x.LayersNumber,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            var for05 = await _context.SewerManifoldFor05s
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .Where(x => x.Id == Id)
                .Select(x => new SewerManifoldFor05ViewModel
                {
                    Id = x.Id,
                    CertificateNumber = x.CertificateNumber,
                    SewerManifoldId = x.SewerManifoldId,
                    LayerNumber = x.LayerNumber,
                    TestDate = x.TestDate.ToDateString(),
                    Status = x.Status,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        ProductionDailyPart = new PdpViewModel
                        {
                            SewerGroup = new SewerGroupViewModel
                            {
                                Code = x.SewerManifold.ProductionDailyPart.SewerGroup.Code,
                                WorkFrontHeadId = x.SewerManifold.ProductionDailyPart.SewerGroup.WorkFrontHeadId
                            }
                        },
                        Address = x.SewerManifold.Address,
                        Name = x.SewerManifold.Name,
                        LengthOfDigging = x.SewerManifold.LengthOfDigging,
                        DitchHeight = x.SewerManifold.DitchHeight
                    },
                    ShippingDate = x.ShippingDate.ToDateString(),
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(for05);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor05ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pId = GetProjectId();

            if (model.ShippingDate == null || model.ShippingDate == "")
                return BadRequest("No se ha ingresado la fecha");

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var FillingFormula = manifold.DitchHeight - (manifold.PipelineDiameter/1000) - 0.30 - 0.20;

            var TheoricalLayerFormular = (int) (FillingFormula / 0.3);

            var for05 = new SewerManifoldFor05
            {
                ProjectId = pId,
                SewerManifoldId = model.SewerManifoldId,
                CertificateNumber = model.CertificateNumber,
                ShippingDate = model.ShippingDate.ToDateTime(),
                Filling = Math.Round(FillingFormula, 2),
                TheoreticalLayer = TheoricalLayerFormular
            };

            manifold.HasFor05 = true;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for05.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FOR_05,
                    $"sewer_manifold_for05_numero_certificado_{for05.CertificateNumber}");
            }

            await _context.SewerManifoldFor05s.AddAsync(for05);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerManifoldFor05ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pId = GetProjectId();

            if (model.ShippingDate == null || model.ShippingDate == "")
                return BadRequest("No se ha ingresado la fecha");

            var for05 = await _context.SewerManifoldFor05s.Where(x => x.ProjectId == pId)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var FillingFormula = manifold.DitchHeight - (manifold.PipelineDiameter / 1000) - 0.30 - 0.20;

            var TheoricalLayerFormular = (int)(FillingFormula / 0.3);

            for05.SewerManifoldId = model.SewerManifoldId;
            for05.CertificateNumber = model.CertificateNumber;
            for05.ShippingDate = model.ShippingDate.ToDateTime();
            for05.Filling = Math.Round(FillingFormula, 2);
            for05.TheoreticalLayer = TheoricalLayerFormular;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (for05.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_05}/{for05.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                for05.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                     ConstantHelpers.Storage.Containers.QUALITY,
                     System.IO.Path.GetExtension(model.File.FileName),
                     ConstantHelpers.Storage.Blobs.FOR_05,
                     $"sewer_manifold_for05_numero_certificado_{for05.CertificateNumber}");
            }

            var TodosFor05 = await _context.SewerManifoldFor05s.ToListAsync();
            foreach(var Ufor05 in TodosFor05)
            {
                var auxManifold = await _context.SewerManifolds.FirstOrDefaultAsync(x => x.Id == Ufor05.SewerManifoldId);
                Ufor05.Filling = Math.Round((auxManifold.DitchHeight - (auxManifold.PipelineDiameter / 1000) - 0.30 - 0.20), 2);
                Ufor05.TheoreticalLayer = (int)(Ufor05.Filling / 0.3);
            }
            var TodosFolding = await _context.FoldingFor05s.ToListAsync();
            foreach(var Ufolding in TodosFolding)
            {
                Ufolding.DryDensity = Math.Round(Ufolding.DryDensity, 3);
                Ufolding.PercentageRealCompaction = Math.Round(Ufolding.PercentageRealCompaction, 1);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for05 = await _context.SewerManifoldFor05s
                .FirstOrDefaultAsync(x => x.Id == id);
 
            if (for05 == null)
            {
                return BadRequest($"Sewer Manifold For47 con Id '{id}' no se halló.");
            }
            
            var foldings = await _context.FoldingFor05s.
                Where(x => x.SewerManifoldFor05Id == id).ToListAsync();

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == for05.SewerManifoldId);

            manifold.HasFor05 = false;

            var cant = foldings.Count();

            for (int i = 0; i < cant; i++)
            {
                _context.FoldingFor05s.Remove(foldings[i]);
            }
            await _context.SaveChangesAsync();

            if (for05.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_05}/{for05.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }
            
            _context.SewerManifoldFor05s.Remove(for05);
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
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var foldings = new List<FoldingFor05>();
                    var for05 = new SewerManifoldFor05();

                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {
                        //---------------Creación del For05
                        var for05selected = await _context.SewerManifoldFor05s
                            .FirstOrDefaultAsync(x => x.SewerManifold.Name == workSheet.Cell($"H{counter}").GetString() + " al " + workSheet.Cell($"I{counter}").GetString());

                        var aux = await _context.SewerManifoldFor05s
                            .FirstOrDefaultAsync(x => x.SewerManifold.Name == workSheet.Cell($"H{counter + 1}").GetString() + " al " + workSheet.Cell($"I{counter + 1}").GetString());

                        var manifold = await _context.SewerManifolds.Where(x => x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.EXECUTION)
                                                    .FirstOrDefaultAsync(x => x.Name == workSheet.Cell($"H{counter}").GetString() + " al " + workSheet.Cell($"I{counter}").GetString());
                        if (for05selected == null)
                        {
                            manifold.HasFor05 = true;
                            for05.Id = Guid.NewGuid();
                            for05.ProjectId = pId;
                            for05.SewerManifoldId = manifold.Id;
                            for05.Filling = Math.Round(manifold.DitchHeight - (manifold.PipelineDiameter / 1000) - 0.30 - 0.20, 2);
                            for05.TheoreticalLayer = (int)(for05.Filling / 0.3);
                            for05.CertificateNumber = workSheet.Cell($"C{counter}").GetString();
                            if (!workSheet.Cell($"V{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"V{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {
                                        for05.ShippingDate = workSheet.Cell($"V{counter}").GetDateTime().ToUniversalTime();
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"V{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        for05.ShippingDate = date.ToUniversalTime();
                                }
                            }

                            await _context.SewerManifoldFor05s.AddAsync(for05);
                            await _context.SaveChangesAsync();
                            for05selected = await _context.SewerManifoldFor05s
                                .FirstOrDefaultAsync(x => x.SewerManifold.Name == workSheet.Cell($"H{counter}").GetString() + " al " + workSheet.Cell($"I{counter}").GetString());
                        }

                        //---------------Creación del Folding For05
                        var folding = new FoldingFor05();
                        folding.LayerNumber = workSheet.Cell($"K{counter}").GetString();
                        //folding.Id = Guid.NewGuid();
                        if (!workSheet.Cell($"D{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"D{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    folding.TestDate = workSheet.Cell($"D{counter}").GetDateTime().ToUniversalTime();
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"D{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    folding.TestDate = date.ToUniversalTime();
                            }
                        }
                        folding.WetDensity = workSheet.Cell($"N{counter}").GetString().ToDoubleString();
                        folding.MoisturePercentage = workSheet.Cell($"O{counter}").GetString().ToDoubleString();
                        var PROCTOR = await _context.FillingLaboratoryTests.FirstOrDefaultAsync(x => x.RecordNumber == workSheet.Cell($"M{counter}").GetString());
                        if (PROCTOR == null)
                            return BadRequest($"El N° de Muestra {workSheet.Cell($"M{counter}").GetString()} no existe");
                        folding.DryDensity = folding.WetDensity / (1 + (folding.MoisturePercentage / 100));
                        folding.PercentageRealCompaction = Math.Round((folding.DryDensity / PROCTOR.MaxDensity) * 100, 1);
                        folding.FillingLaboratoryTestId = PROCTOR.Id;
                        folding.SewerManifoldFor05Id = for05selected.Id;
                        if (folding.LayerNumber == "Base" || folding.LayerNumber == "BASE")
                            folding.PercentageRequiredCompaction = 100;
                        else
                            folding.PercentageRequiredCompaction = 95;
                        if (folding.PercentageRealCompaction < folding.PercentageRequiredCompaction)
                            folding.Status = "No Aprobado";
                        else
                            folding.Status = "Aprobado";
                        foldings.Add(folding);
                        if (for05selected != null && aux == null)
                        {
                            var LNumbers = foldings.Where(x => x.SewerManifoldFor05Id == for05selected.Id).ToList();
                            var cant = LNumbers.Count();
                            var max = 0.0;
                            var lastDate = LNumbers[0].TestDate;
                            var BaseExist = false;
                            var status = "Aprobado";
                            for (int i = 0; i < cant; i++)
                            {
                                if (LNumbers[i].LayerNumber != "Base" && LNumbers[i].LayerNumber != "BASE")
                                {
                                    if (max < LNumbers[i].LayerNumber.ToDoubleString())
                                    {
                                        max = LNumbers[i].LayerNumber.ToDoubleString();
                                    }
                                }
                                else
                                    BaseExist = true;

                                if (LNumbers[i].TestDate > lastDate)
                                    lastDate = LNumbers[i].TestDate;

                                if (LNumbers[i].Status == "No Aprobado")
                                    status = "No Aprobado";
                            }

                            if (BaseExist == true)
                            {
                                if (max != 0.0)
                                    for05selected.LayerNumber = max.ToString() + " + Base";
                                else
                                    for05selected.LayerNumber = "Base";
                            }
                            else
                                for05selected.LayerNumber = max.ToString();

                            for05selected.TestDate = lastDate;
                            for05selected.Status = status;
                            for05selected.LayersNumber = cant;
                            await _context.SaveChangesAsync();
                        }
                        ++counter;
                    }
                    await _context.FoldingFor05s.AddRangeAsync(foldings);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "For05CargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook()){
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"C2").Value = "Nº Certificado";
                workSheet.Range("C2:C3").Merge();
                workSheet.Range("C2:C3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D2").Value = "Fecha de Ensayo";
                workSheet.Range("D2:D3").Merge();
                workSheet.Range("D2:D3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E2").Value = "Ubicación o Progresiva del Ensayo";
                workSheet.Range("E2:E3").Merge();
                workSheet.Range("E2:E3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"F2").Value = "Frente";
                workSheet.Range("F2:F3").Merge();
                workSheet.Range("F2:F3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"G2").Value = "Ubicación";
                workSheet.Range("G2:G3").Merge();
                workSheet.Range("G2:G3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"H2").Value = "Tramo";
                workSheet.Range("H2:I2").Merge();
                workSheet.Range("H2:I2").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                workSheet.Cell($"H3").Value = "Bz. N° 1";
                workSheet.Cell($"H3").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                workSheet.Cell($"I3").Value = "Bz. N° 2";
                workSheet.Cell($"I3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"J2").Value = "Área Liberada";
                workSheet.Range("J2:J3").Merge();
                workSheet.Range("J2:J3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"K2").Value = "Capa";
                workSheet.Range("K2:K3").Merge();
                workSheet.Range("K2:K3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"L2").Value = "Prof. (m)";
                workSheet.Range("L2:L3").Merge();
                workSheet.Range("L2:L3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"M2").Value = "N° Muestra";
                workSheet.Range("M2:M3").Merge();
                workSheet.Range("M2:M3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"N2").Value = "Densidad Húmeda";
                workSheet.Range("N2:N3").Merge();
                workSheet.Range("N2:N3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"O2").Value = "% Humedad";
                workSheet.Range("O2:O3").Merge();
                workSheet.Range("O2:O3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"P2").Value = "Densidad Seca";
                workSheet.Range("P2:P3").Merge();
                workSheet.Range("P2:P3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"Q2").Value = "Máxima Densidad Seca";
                workSheet.Range("Q2:Q3").Merge();
                workSheet.Range("Q2:Q3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"R2").Value = "Óptimo Contenido Humedad";
                workSheet.Range("R2:R3").Merge();
                workSheet.Range("R2:R3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell($"S2").Value = "Resultados Obtenidos";
                workSheet.Range("S2:U2").Merge();
                workSheet.Range("S2:U2").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                workSheet.Cell($"S3").Value = "% Compactación";
                workSheet.Cell($"S3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                workSheet.Cell($"T3").Value = "% Especificación";
                workSheet.Cell($"T3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                workSheet.Cell($"U3").Value = "Aprobado / Desaprobado";
                workSheet.Cell($"U3").Style.Fill.SetBackgroundColor(XLColor.BlueGray);

                workSheet.Cell("V2").Value = "Fecha de Envío";
                workSheet.Range("V2:V3").Merge();
                workSheet.Range("V2:V3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(3).Width = 20;
                workSheet.Column(4).Width = 18;
                workSheet.Column(5).Width = 30;
                workSheet.Column(6).Width = 8;
                workSheet.Column(7).Width = 10;
                workSheet.Column(8).Width = 9;
                workSheet.Column(9).Width = 9;
                workSheet.Column(10).Width = 15;
                workSheet.Column(11).Width = 5;
                workSheet.Column(12).Width = 9;
                workSheet.Column(13).Width = 12;
                workSheet.Column(14).Width = 17;
                workSheet.Column(15).Width = 12;
                workSheet.Column(16).Width = 14;
                workSheet.Column(17).Width = 22;
                workSheet.Column(18).Width = 26;
                workSheet.Column(19).Width = 15;
                workSheet.Column(20).Width = 15;
                workSheet.Column(21).Width = 23;
                workSheet.Column(22).Width = 15;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("C2:V3").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("C2:V3").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


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
                        var for05 = await _context.SewerManifoldFor05s.FirstOrDefaultAsync(x => entry.Name.Contains(x.CertificateNumber));
                        if (for05 != null && for05.FileUrl != null)
                        {
                            await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_05}/{for05.FileUrl.AbsolutePath.Split('/').Last()}",
                                    ConstantHelpers.Storage.Containers.QUALITY);
                            for05.FileUrl = await storage.UploadFile(entry.Open(),
                                 ConstantHelpers.Storage.Containers.QUALITY,
                                 System.IO.Path.GetExtension(entry.Name),
                                 ConstantHelpers.Storage.Blobs.FOR_05,
                                 $"sewer_manifold_for05_numero_certificado_{for05.CertificateNumber}");
                            await _context.SaveChangesAsync();
                        }
                        if (for05 != null  && for05.FileUrl == null)
                        {
                            for05.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.QUALITY,
                                System.IO.Path.GetExtension(entry.Name),
                                ConstantHelpers.Storage.Blobs.FOR_05,
                                $"sewer_manifold_for05_numero_certificado_{for05.CertificateNumber}");
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            return Ok();
        }

        [HttpDelete("todo-eliminar")]
        public async Task<IActionResult> Delete()
        {
            var TodoFor05 = await _context.SewerManifoldFor05s.ToListAsync();

            foreach(var for05 in TodoFor05)
            {
                var TodoFolding = await _context.FoldingFor05s.Where(x => x.SewerManifoldFor05Id == for05.Id).ToListAsync();

                foreach(var folding in TodoFolding)
                {
                    _context.FoldingFor05s.Remove(folding);
                }
                await _context.SaveChangesAsync();

                if (for05.FileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_05}/{for05.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                }

                _context.SewerManifoldFor05s.Remove(for05);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var For05 = await _context.SewerManifoldFor05s.Where(x=>x.Status == "Aprobado").ToListAsync();

            var data = For05.Count();

            return Ok(data);
        }

        [HttpGet("suma-longitud")]
        public async Task<IActionResult> GetSuma()
        {
            var For05Todos = await _context.SewerManifoldFor05s.Include(x=>x.SewerManifold).ToListAsync();

            var Suma = 0.0;

            foreach (var for05 in For05Todos)
                Suma = Suma + for05.SewerManifold.LengthOfDigging;

            //return Ok(Math.Round(Suma,2));
            return Ok(Suma.ToString("N", CultureInfo.InvariantCulture));
        }

        [HttpGet("porcentaje")]
        public async Task<IActionResult> GetPorcentaje()
        {
            var For05Todos = await _context.SewerManifoldFor05s.Include(x => x.SewerManifold).ToListAsync();

            var SumaFor05 = 0.0;
            var SumaManifold = 0.0;

            foreach (var for05 in For05Todos)
                SumaFor05 = SumaFor05 + for05.SewerManifold.LengthOfDigging;

            var manifoldTodos = await _context.SewerManifolds.Where(x=>x.ProcessType==1).ToListAsync();

            foreach (var manifold in manifoldTodos)
                SumaManifold = SumaManifold + manifold.LengthOfDigging;

            var result = Math.Round((SumaFor05 / SumaManifold) * 100,2);

            return Ok(result + "%");
        }

    }
}