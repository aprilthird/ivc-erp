using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.CertificateIssuerViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/pruebas-de-laboratorio-de-relleno")]
    public class FillingLaboratoryTestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public FillingLaboratoryTestController(IvcDbContext context,
            ILogger<CompactionDensityCertificateController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int? materialType = null, Guid? originType = null, bool? hasFile = null)
        {
            var query = _context.FillingLaboratoryTests
                .AsNoTracking()
                .AsQueryable();

            if (materialType.HasValue)
                query = query.Where(x => x.MaterialType == materialType.Value);
            if (originType.HasValue)
                query = query.Where(x => x.OriginTypeFillingLaboratoryId == originType.Value);
            if (hasFile.HasValue)
                query = query.Where(x => hasFile.Value ? x.FileUrl != null : true);

            var data = await query
                .Select(x => new FillingLaboratoryTestViewModel
                {
                    Id = x.Id,
                    MaterialType = x.MaterialType,
                    OriginType = x.OriginType,
                    Ubication = x.Ubication,
                    RecordNumber = x.RecordNumber,
                    SerialNumber = x.SerialNumber,
                    CertificateIssuer = new CertificateIssuerViewModel
                    {
                        Name = x.CertificateIssuer.Name
                    },
                    SamplingDate = x.SamplingDate.ToLocalDateFormat(),
                    TestDate = x.TestDate.ToLocalDateFormat(),
                    MaterialMoisture = x.MaterialMoisture,
                    MaxDensity = x.MaxDensity,
                    OptimumMoisture = x.OptimumMoisture,
                    FileUrl = x.FileUrl,
                    OriginTypeFillingLaboratoryId = x.OriginTypeFillingLaboratoryId,
                    OriginTypeFillingLaboratory = new OriginTypeFillingLaboratoryViewModel
                    {
                        OriginTypeFLName = x.OriginTypeFillingLaboratory.OriginTypeFLName
                    }
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertificate(Guid id)
        {
            var fillingLaboratoryTest = await _context.FillingLaboratoryTests.FindAsync(id);
            var model = new FillingLaboratoryTestViewModel
            {
                Id = fillingLaboratoryTest.Id,
                MaterialType = fillingLaboratoryTest.MaterialType,
                OriginType = fillingLaboratoryTest.OriginType,
                Ubication = fillingLaboratoryTest.Ubication,
                RecordNumber = fillingLaboratoryTest.RecordNumber,
                CertificateIssuerId = fillingLaboratoryTest.CertificateIssuerId,
                SamplingDate = fillingLaboratoryTest.SamplingDate.ToLocalDateFormat(),
                TestDate = fillingLaboratoryTest.TestDate.ToLocalDateFormat(),
                MaterialMoisture = fillingLaboratoryTest.MaterialMoisture,
                MaxDensity = fillingLaboratoryTest.MaxDensity,
                OptimumMoisture = fillingLaboratoryTest.OptimumMoisture,
                FileUrl = fillingLaboratoryTest.FileUrl,
                SerialNumber = fillingLaboratoryTest.SerialNumber,
                OriginTypeFillingLaboratoryId = fillingLaboratoryTest.OriginTypeFillingLaboratoryId
            };
            return Ok(model);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FillingLaboratoryTestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var fillingLaboratoryTest = new FillingLaboratoryTest
            {
                MaterialType = model.MaterialType,
                OriginType = model.OriginType,
                Ubication = model.Ubication,
                RecordNumber = model.RecordNumber,
                SerialNumber = model.SerialNumber,
                CertificateIssuerId = model.CertificateIssuerId,
                SamplingDate = model.SamplingDate.ToUtcDateTime(),
                TestDate = model.TestDate.ToUtcDateTime(),
                MaterialMoisture = model.MaterialMoisture,
                MaxDensity = model.MaxDensity,
                OptimumMoisture = model.OptimumMoisture,
                OriginTypeFillingLaboratoryId = model.OriginTypeFillingLaboratoryId
            };
            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                fillingLaboratoryTest.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(model.File.FileName), ConstantHelpers.Storage.Blobs.FILLING_LABORATORY_TEST);
            }
            await _context.FillingLaboratoryTests.AddAsync(fillingLaboratoryTest);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FillingLaboratoryTestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            if (model.SamplingDate == null || model.SamplingDate == "")
                return BadRequest("No se ha ingresado Fecha de Muestreo");
            if (model.TestDate == null || model.TestDate == "")
                return BadRequest("No se ha ingresado Fecha de Ensayo");

            var fillingLaboratoryTest = await _context.FillingLaboratoryTests
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            fillingLaboratoryTest.MaterialType = model.MaterialType;
            fillingLaboratoryTest.OriginType = model.OriginType;
            fillingLaboratoryTest.Ubication = model.Ubication;
            fillingLaboratoryTest.RecordNumber = model.RecordNumber;
            fillingLaboratoryTest.SerialNumber = model.SerialNumber;
            fillingLaboratoryTest.CertificateIssuerId = model.CertificateIssuerId;
            fillingLaboratoryTest.SamplingDate = model.SamplingDate.ToDateTime();
            fillingLaboratoryTest.TestDate = model.TestDate.ToDateTime();
            fillingLaboratoryTest.MaterialMoisture = model.MaterialMoisture;
            fillingLaboratoryTest.MaxDensity = model.MaxDensity;
            fillingLaboratoryTest.OptimumMoisture = model.OptimumMoisture;
            fillingLaboratoryTest.OriginTypeFillingLaboratoryId = model.OriginTypeFillingLaboratoryId;

            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (fillingLaboratoryTest.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FILLING_LABORATORY_TEST}/{fillingLaboratoryTest.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                fillingLaboratoryTest.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(model.File.FileName), 
                    ConstantHelpers.Storage.Blobs.FILLING_LABORATORY_TEST);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        //[HttpPut("editar/{id}")]
        //public async Task<IActionResult> Edit(Guid id, QuarryViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var quarry = await _context.Quarries.FindAsync(id);
        //    quarry.Name = model.Name;
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[HttpDelete("eliminar/{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var quarry = await _context.Quarries.FirstOrDefaultAsync(x => x.Id == id);
        //    if (quarry == null)
        //        return BadRequest($"Cantera con Id '{id}' no encontrado.");
        //    _context.Quarries.Remove(quarry);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 9;
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var fillingLaboratoryTests = new List<FillingLaboratoryTest>();

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var existe = await _context.FillingLaboratoryTests.FirstOrDefaultAsync(x => x.RecordNumber == workSheet.Cell($"E{counter}").GetString());
                        if (existe == null)
                        {
                            var fillingLaboratoryTest = new FillingLaboratoryTest();
                            fillingLaboratoryTest.MaterialType = ConstantHelpers.Certificate.FillingLaboratory.MaterialType.VALUES
                                .FirstOrDefault(x => x.Value == workSheet.Cell($"B{counter}").GetString()).Key;
                            //fillingLaboratoryTest.OriginType = ConstantHelpers.Certificate.FillingLaboratory.OriginType.OWN_EXCAVATION;
                            var aux = await _context.OriginTypeFillingLaboratories
                                .FirstOrDefaultAsync(x => x.OriginTypeFLName == workSheet.Cell($"C{counter}").GetString());
                            if (aux != null)
                                fillingLaboratoryTest.OriginTypeFillingLaboratoryId = aux.Id;
                            else
                                return BadRequest("El origen de la celda C" + counter + " está mal");
                            fillingLaboratoryTest.Ubication = workSheet.Cell($"D{counter}").GetString();
                            fillingLaboratoryTest.RecordNumber = workSheet.Cell($"E{counter}").GetString();
                            fillingLaboratoryTest.SerialNumber = workSheet.Cell($"F{counter}").GetString();
                            var certificateIssuer = await _context.CertificateIssuers.FirstOrDefaultAsync(x => x.Name == workSheet.Cell($"G{counter}").GetString());
                            fillingLaboratoryTest.CertificateIssuerId = certificateIssuer.Id;
                            if (!workSheet.Cell($"H{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"H{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {
                                        fillingLaboratoryTest.SamplingDate = workSheet.Cell($"H{counter}").GetDateTime().ToUniversalTime();
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"H{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        fillingLaboratoryTest.SamplingDate = date.ToUniversalTime();
                                }
                            }
                            if (!workSheet.Cell($"I{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"I{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {
                                        fillingLaboratoryTest.TestDate = workSheet.Cell($"I{counter}").GetDateTime().ToUniversalTime();
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"I{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        fillingLaboratoryTest.TestDate = date.ToUniversalTime();
                                }
                            }
                            fillingLaboratoryTest.MaterialMoisture = workSheet.Cell($"J{counter}").IsEmpty() ? 0 : workSheet.Cell($"J{counter}").GetDouble();
                            fillingLaboratoryTest.MaxDensity = workSheet.Cell($"K{counter}").IsEmpty() ? 0 : workSheet.Cell($"K{counter}").GetDouble();
                            fillingLaboratoryTest.OptimumMoisture = workSheet.Cell($"L{counter}").IsEmpty() ? 0 : workSheet.Cell($"L{counter}").GetDouble();
                            fillingLaboratoryTests.Add(fillingLaboratoryTest);
                            await _context.FillingLaboratoryTests.AddRangeAsync(fillingLaboratoryTests);
                            await _context.SaveChangesAsync();
                        }
                        ++counter;
                    }
                }
                mem.Close();
            }
            return Ok();
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
                    foreach(var entry in entries)
                    {
                        var storage = new CloudStorageService(_storageCredentials);
                        var fillingLaboratoryTest = await _context.FillingLaboratoryTests.FirstOrDefaultAsync(x => entry.Name.Contains(x.RecordNumber));
                        if(fillingLaboratoryTest != null && fillingLaboratoryTest.FileUrl == null)
                        {
                            fillingLaboratoryTest.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(entry.Name), ConstantHelpers.Storage.Blobs.FILLING_LABORATORY_TEST, fillingLaboratoryTest.RecordNumber);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }

            //foreach (var file in files)
            //{
            //    if(file != null)
            //    {
            //        var storage = new CloudStorageService(_storageCredentials);
            //        var fillingLaboratoryTest = await _context.FillingLaboratoryTests.FirstOrDefaultAsync(x => file.FileName.Contains(x.RecordNumber));
            //        fillingLaboratoryTest.FileUrl = await storage.UploadFile(file.OpenReadStream(),
            //            ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(file.FileName), ConstantHelpers.Storage.Blobs.FILLING_LABORATORY_TEST, fillingLaboratoryTest.RecordNumber);
            //        await _context.SaveChangesAsync();  
            //    }
            //}
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var fillingLaboratoryTest = await _context.FillingLaboratoryTests
                .FirstOrDefaultAsync(x => x.Id == id);

            if (fillingLaboratoryTest == null)
                return BadRequest("PROCTOR con Id '{id}' no se halló");

            if(fillingLaboratoryTest.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FILLING_LABORATORY_TEST}/{fillingLaboratoryTest.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.FillingLaboratoryTests.Remove(fillingLaboratoryTest);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "PROCTORCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B8").Value = "Tipo de Material";

                workSheet.Cell($"C8").Value = "Procedencia";

                workSheet.Cell($"D8").Value = "Ubicación";

                workSheet.Cell($"E8").Value = "N° de Muestra";

                workSheet.Cell($"F8").Value = "N° de Certificado";

                workSheet.Cell($"G8").Value = "Certificado por";

                workSheet.Cell($"H8").Value = "Fecha de Muestreo";

                workSheet.Cell($"I8").Value = "Fecha de Ensayo"; ;

                workSheet.Cell($"J8").Value = "Material Humedad";

                workSheet.Cell($"K8").Value = "Máxima Densidad";

                workSheet.Cell($"L8").Value = "Humdad Óptima";

                workSheet.Cell($"B9").Value = "Info Aquí";
                workSheet.Cell($"B9").Style.Fill.SetBackgroundColor(XLColor.Yellow);


                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 19;
                workSheet.Column(3).Width = 19;
                workSheet.Column(4).Width = 19;
                workSheet.Column(5).Width = 19;
                workSheet.Column(6).Width = 19;
                workSheet.Column(7).Width = 19;
                workSheet.Column(8).Width = 19;
                workSheet.Column(9).Width = 19;
                workSheet.Column(10).Width = 19;
                workSheet.Column(11).Width = 19;
                workSheet.Column(12).Width = 19;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                var originTypes = _context.OriginTypeFillingLaboratories.AsNoTracking().ToList();

                DataTable dtOriginType = new DataTable();
                dtOriginType.TableName = "Tipos de Origen";
                dtOriginType.Columns.Add("Nombre", typeof(string));
                foreach (var item in originTypes)
                    dtOriginType.Rows.Add(item.OriginTypeFLName);
                dtOriginType.AcceptChanges();

                var workSheetOrigin = wb.Worksheets.Add(dtOriginType);

                workSheetOrigin.Column(1).Width = 30;

                workSheet.Range("B8:L8").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B8:L8").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}