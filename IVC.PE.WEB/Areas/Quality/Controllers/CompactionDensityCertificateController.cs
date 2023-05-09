using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models;
using IVC.PE.ENTITIES.Models.Aggregation;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Aggregation.ViewModels.QuarryViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.CompactionDensityCertificateViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DrainageNetworkSummaryViewModels;
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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/certificado-densidad-compactacion")]
    public class CompactionDensityCertificateController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public CompactionDensityCertificateController(IvcDbContext context,
            ILogger<CompactionDensityCertificateController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid projectId, Guid? workFrontId = null, Guid? sewerGroupId = null, bool? hasCertificate = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.SewerLines
                .Where(x => x.SewerGroup.Type == ConstantHelpers.Sewer.Group.Type.DRAINAGE)
                .Where(x => x.Stage == ConstantHelpers.Stage.REAL)
                .OrderBy(x => x.InitialSewerBox.Code)
                .AsNoTracking()
                .AsQueryable();

            if (workFrontId.HasValue)
                query = query.Where(x => x.SewerGroup.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            if (hasCertificate.HasValue)
                query = query.Where(x => hasCertificate.Value ? x.CompactionDensityCertificate != null : true);

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.Address.Contains(search) ||
                    x.InitialSewerBox.Code.Contains(search) ||
                    x.FinalSewerBox.Code.Contains(search) ||
                    x.DrainageArea.Contains(search));

            var data = await query
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new SewerLineViewModel
                {
                    Id = x.Id,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code,
                        Type = x.SewerGroup.Type,
                        WorkFront = new WorkFrontViewModel
                        {
                            Code = x.SewerGroup.WorkFront.Code
                        }
                    },
                    Address = x.Address,
                    AverageDepthSewerLine = x.AverageDepthSewerLine,
                    NominalDiameter = x.NominalDiameter,
                    InitialSewerBoxCode = x.InitialSewerBox.Code,
                    FinalSewerBoxCode = x.FinalSewerBox.Code,
                    Layers = x.Layers,
                    CompactionDensityCertificate = x.CompactionDensityCertificate != null 
                        ? new CompactionDensityCertificateViewModel
                        {
                            Id = x.CompactionDensityCertificate.Id,
                            SerialNumber = x.CompactionDensityCertificate.SerialNumber,
                            ExecutionDate = x.CompactionDensityCertificate.ExecutionDate.ToLocalDateFormat(),
                            MaterialType = x.CompactionDensityCertificate.MaterialType,
                            Quarry = new QuarryViewModel
                            {
                                Name = x.CompactionDensityCertificate.Quarry.Name
                            },
                            FileUrl = x.CompactionDensityCertificate.FileUrl
                        }
                        : new CompactionDensityCertificateViewModel
                        {
                            SerialNumber = "---",
                            ExecutionDate = "---",
                            MaterialType = 0,
                            Quarry = new QuarryViewModel
                            {
                                Name = "---"
                            }
                        }
                }).ToListAsync();

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var compactionDensityCertificate = await _context.CompactionDensityCertificates.FindAsync(id);
            var model = new FillingLaboratoryTestViewModel
            {
                Id = compactionDensityCertificate.Id,
                SerialNumber = compactionDensityCertificate.SerialNumber,
                FileUrl = compactionDensityCertificate.FileUrl
            };
            return Ok(model);
        }

        [HttpGet("detalle/{id}/listar")]
        public async Task<IActionResult> GetAllDetails(Guid id)
        {
            var data = await _context.CompactionDensityCertificateDetails
                .Where(x => x.CompactionDensityCertificate.SewerLineId == id)
                .OrderBy(x => x.Latest)
                .ThenBy(x => x.Layer)
                .Select(x => new CompactionDensityCertificateDetailViewModel
                {
                    TestDate = x.TestDate.ToLocalDateFormat(),
                    Layer = x.Layer,
                    WetDensity = x.WetDensity,
                    Moisture = x.Moisture,
                    DryDensity = x.DryDensity,
                    FillingLaboratoryTest = new FillingLaboratoryTestViewModel
                    {
                        RecordNumber = x.FillingLaboratoryTest.RecordNumber,
                        MaxDensity = x.FillingLaboratoryTest.MaxDensity,
                        OptimumMoisture = x.FillingLaboratoryTest.OptimumMoisture,
                    },
                    DensityPercentage = x.DensityPercentage,
                    Latest = x.Latest
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> CreateCertificate(CompactionDensityCertificateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var compactionDensityCertificate = new CompactionDensityCertificate
            {
                SerialNumber = model.SerialNumber,
                ExecutionDate = model.ExecutionDate.ToUtcDateTime(),
                MaterialType = model.MaterialType,
                QuarryId = model.QuarryId,
                SewerLineId = model.SewerLineId
            };

            if(model.File == null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                compactionDensityCertificate.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(model.File.FileName), ConstantHelpers.Storage.Blobs.COMPACTION_DENSITY_CERTIFICATE);
            }

            await _context.CompactionDensityCertificates.AddAsync(compactionDensityCertificate);
            await _context.CompactionDensityCertificateDetails
                .AddRangeAsync(await Task.WhenAll(model.Details.Select(async x => {
                    var fillingLaboratoryTest = await _context.FillingLaboratoryTests.FindAsync(x.FillingLaboratoryTestId);
                    var compactionDensityCertificateDetail = new CompactionDensityCertificateDetail
                    {
                        CompactionDensityCertificate = compactionDensityCertificate,
                        TestDate = x.TestDate.ToUtcDateTime(),
                        Layer = x.Layer,
                        FillingLaboratoryTestId = x.FillingLaboratoryTestId,
                        FillingLaboratoryTest = fillingLaboratoryTest,
                        WetDensity = x.WetDensity,
                        Moisture = x.Moisture
                    };
                    compactionDensityCertificateDetail.Calculate();
                    return compactionDensityCertificateDetail;
                })));
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var compactionDensityCertificate = await _context.CompactionDensityCertificates
                .Include(x => x.CompactionDensityCertificateDetails)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (compactionDensityCertificate == null)
                return BadRequest($"Certificado de densidad de compactación con Id '{id}' no encontrado.");
            if (compactionDensityCertificate.CompactionDensityCertificateDetails != null)
                _context.CompactionDensityCertificateDetails.RemoveRange(compactionDensityCertificate.CompactionDensityCertificateDetails);
            _context.CompactionDensityCertificates.Remove(compactionDensityCertificate);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 14;
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var sewerGroups = await _context.SewerGroups.ToListAsync();
                    var compactionDensityCertificateDetails = new List<CompactionDensityCertificateDetail>();
                    var compactionDensityCertificates = new List<CompactionDensityCertificate>();
                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {
                        var compactionDensityCertificateDetail = new CompactionDensityCertificateDetail();
                        var compactionDensityCertificateSerialNumber = workSheet.Cell($"C{counter}").GetString();
                        var compactionDensityCertificate = await _context.CompactionDensityCertificates
                            .Include(x => x.SewerLine)
                            .FirstOrDefaultAsync(x => x.SerialNumber == compactionDensityCertificateSerialNumber)
                            ?? compactionDensityCertificates.FirstOrDefault(x => x.SerialNumber == compactionDensityCertificateSerialNumber);
                        if (!workSheet.Cell($"E{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"E{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    compactionDensityCertificateDetail.TestDate = workSheet.Cell($"E{counter}").GetDateTime().ToUniversalTime();
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"E{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    compactionDensityCertificateDetail.TestDate = date.ToUniversalTime();
                            }
                        }
                        if (compactionDensityCertificate == null)
                        {
                            compactionDensityCertificate = new CompactionDensityCertificate();
                            var quarry = await _context.Quarries.FirstOrDefaultAsync();
                            if(quarry == null)
                            {
                                quarry = new Quarry();
                                quarry.Name = "Prueba";
                                quarry.ProjectId = project.Id;
                                await _context.Quarries.AddAsync(quarry);
                                await _context.SaveChangesAsync();
                            }
                            var sewerLine = await _context.SewerLines
                                .Where(x => x.Stage == ConstantHelpers.Stage.REAL)
                                .FirstOrDefaultAsync(x => x.InitialSewerBox.Code == workSheet.Cell($"G{counter}").GetString() && x.FinalSewerBox.Code == workSheet.Cell($"H{counter}").GetString());
                            if(sewerLine == null)
                            {
                                ++counter;
                                continue;
                            }
                            compactionDensityCertificate.SerialNumber = workSheet.Cell($"C{counter}").GetString();
                            compactionDensityCertificate.ExecutionDate = compactionDensityCertificateDetail.TestDate;
                            compactionDensityCertificate.QuarryId = quarry.Id;
                            compactionDensityCertificate.SewerLineId = sewerLine.Id;
                            compactionDensityCertificate.SewerLine = sewerLine;
                            compactionDensityCertificate.CompactionDensityCertificateDetails = new List<CompactionDensityCertificateDetail>();
                            compactionDensityCertificates.Add(compactionDensityCertificate);
                        }
                        compactionDensityCertificateDetail.CompactionDensityCertificate = compactionDensityCertificate;
                        var layer = workSheet.Cell($"J{counter}").GetString();
                        compactionDensityCertificateDetail.Latest = layer == "Rasante";
                        compactionDensityCertificateDetail.Layer = layer == "BASE" 
                            ? 0 : layer == "Rasante"
                            ? compactionDensityCertificate.SewerLine.Layers : (int)workSheet.Cell($"J{counter}").GetDouble();
                        var fillingLaboratoryTest = await _context.FillingLaboratoryTests.FirstOrDefaultAsync(x => x.RecordNumber == workSheet.Cell($"K{counter}").GetString());
                        if(fillingLaboratoryTest != null)
                        {
                            compactionDensityCertificateDetail.FillingLaboratoryTestId = fillingLaboratoryTest.Id;
                            compactionDensityCertificateDetail.WetDensity = workSheet.Cell($"L{counter}").IsEmpty()
                                ? 0 : workSheet.Cell($"L{counter}").GetDouble();
                            compactionDensityCertificateDetail.Moisture = workSheet.Cell($"M{counter}").IsEmpty()
                                ? 0 : workSheet.Cell($"M{counter}").GetDouble();
                            compactionDensityCertificateDetail.DryDensity = workSheet.Cell($"N{counter}").IsEmpty()
                                ? 0 : workSheet.Cell($"N{counter}").GetDouble();
                            compactionDensityCertificateDetail.DensityPercentage = workSheet.Cell($"R{counter}").IsEmpty()
                                ? 0 : workSheet.Cell($"R{counter}").GetDouble();
                            compactionDensityCertificate.CompactionDensityCertificateDetails.Add(compactionDensityCertificateDetail);
                            compactionDensityCertificateDetails.Add(compactionDensityCertificateDetail);
                        }
                        ++counter;
                    }
                    var dump = compactionDensityCertificates.Where(x => compactionDensityCertificates.Any(y => y.SewerLineId == x.SewerLineId && y.Id != x.Id)).ToList();
                    var aa = dump.Count();
                    await _context.CompactionDensityCertificates.AddRangeAsync(compactionDensityCertificates);
                    await _context.CompactionDensityCertificateDetails.AddRangeAsync(compactionDensityCertificateDetails);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
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
                        var compactionDensityCertificate = await _context.CompactionDensityCertificates.FirstOrDefaultAsync(x => entry.Name.Contains(x.SerialNumber));
                        if (compactionDensityCertificate != null && compactionDensityCertificate.FileUrl == null)
                        {
                            compactionDensityCertificate.FileUrl = await storage.UploadFile(entry.Open(),
                                ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(entry.Name), ConstantHelpers.Storage.Blobs.COMPACTION_DENSITY_CERTIFICATE, compactionDensityCertificate.SerialNumber);
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
    }
}
