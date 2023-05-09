using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.ConcreteQualityCertificateViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/certificado-calidad-concreto")]
    public class ConcreteQualityCertificateController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ConcreteQualityCertificateController(IvcDbContext context,
            ILogger<ConcreteQualityCertificateController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid projectId, Guid? workFrontId = null, Guid? sewerGroupId = null, int? terrainType = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.SewerBoxes
                //.Where(x => x.SewerGroup.Type == ConstantHelpers.Sewer.Group.Type.DRAINAGE)
                //.Where(x => x.Stage == ConstantHelpers.Stage.REAL)
                .OrderBy(x => x.Code)
                .AsNoTracking()
                .AsQueryable();

            //if (workFrontId.HasValue)
            //    query = query.Where(x => x.SewerGroup.WorkFrontId == workFrontId.Value);
            //if (sewerGroupId.HasValue)
            //    query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            if (terrainType.HasValue)
                query = query.Where(x => x.TerrainType == terrainType.Value);

            var totalRecords = await query.CountAsync();

            //if (!string.IsNullOrEmpty(search))
            //    query = query.Where(x => x.Address.Contains(search) ||
            //        x.Code.Contains(search) ||
            //        x.DrainageArea.Contains(search));

            var data = await query
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new ConcreteQualityCertificateSummaryViewModel
                {
                    SewerBox = new SewerBoxViewModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        //SewerGroup = new SewerGroupViewModel
                        //{
                        //    //Code = x.SewerGroup.Code,
                        //    //WorkFront = new WorkFrontViewModel
                        //    //{
                        //    //    Code = x.SewerGroup.WorkFront.Code
                        //    //}
                        //},
                        //Address = x.Address,
                    },
                    //MaxSlabAge = x.ConcreteQualityCertificateDetails
                    //    .Where(y => y.Segment == ConstantHelpers.Certificate.ConcreteQuality.Segment.SLAB)
                    //    .Max(y => y.ConcreteQualityCertificate.Age),
                    //MaxRoofAge = x.ConcreteQualityCertificateDetails
                    //    .Where(y => y.Segment == ConstantHelpers.Certificate.ConcreteQuality.Segment.ROOF)
                    //    .Max(y => y.ConcreteQualityCertificate.Age),
                    //MaxBodyAge = x.ConcreteQualityCertificateDetails
                    //    .Where(y => y.Segment == ConstantHelpers.Certificate.ConcreteQuality.Segment.BODY)
                    //    .Max(y => y.ConcreteQualityCertificate.Age)
                }).ToListAsync();

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data
            });
        }

        [HttpGet("detalle/{id}/listar")]
        public async Task<IActionResult> GetAllDetails(Guid id)
        {
            var data = await _context.ConcreteQualityCertificateDetails
                .Where(x => x.SewerBoxId == id)
                .Select(x => new ConcreteQualityCertificateDetailViewModel
                {
                    Id = x.Id,
                    ConcreteQualityCertificateId = x.ConcreteQualityCertificateId,
                    ConcreteQualityCertificate = new ConcreteQualityCertificateViewModel
                    {
                        CertificateSerialNumber = x.ConcreteQualityCertificate.CertificateSerialNumber,
                        Age = x.ConcreteQualityCertificate.Age,
                        SamplingDate = x.ConcreteQualityCertificate.SamplingDate.ToLocalDateFormat(),
                        TestDate = x.ConcreteQualityCertificate.TestDate.ToLocalDateFormat(),
                        FirstResult = x.ConcreteQualityCertificate.FirstResult,
                        SecondResult = x.ConcreteQualityCertificate.SecondResult
                    },
                    Segment = x.Segment,
                    SegmentNumber = x.SegmentNumber,
                }).ToListAsync();
            data = data.Concat(
                ConstantHelpers.Certificate.ConcreteQuality.Age.VALUES
                    .SelectMany(x => ConstantHelpers.Certificate.ConcreteQuality.Segment.VALUES
                    .Where(y => !data.Any(d => d.ConcreteQualityCertificate.Age == x.Key && d.Segment == y.Key))
                    .Select(y => new ConcreteQualityCertificateDetailViewModel
                    {
                        ConcreteQualityCertificate = new ConcreteQualityCertificateViewModel
                        {
                            CertificateSerialNumber = "---",
                            For07SerialNumber = "---",
                            Age = x.Key,
                            SamplingDate = "---",
                            TestDate = "---",
                            FirstResult = 0,
                            SecondResult = 0
                        },
                        Segment = y.Key,
                        SegmentNumber = 1,
                    })).ToList())
                .ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertificate(Guid id)
        {
            var concreteQualityCertificate = await _context.ConcreteQualityCertificates.FindAsync(id);
            var model = new ConcreteQualityCertificateViewModel
            {
                CertificateSerialNumber = concreteQualityCertificate.CertificateSerialNumber,
                For07SerialNumber = concreteQualityCertificate.For07SerialNumber,
                CertificateFileUrl = concreteQualityCertificate.CertificateFileUrl,
                For07FileUrl = concreteQualityCertificate.For07FileUrl
            };
            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> CreateCertificate(ConcreteQualityCertificateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var concreteQualityCertificate = new ConcreteQualityCertificate
            {
                CertificateSerialNumber = model.CertificateSerialNumber,
                For07SerialNumber = model.For07SerialNumber,
                Age = model.Age,
                SamplingDate = model.SamplingDate.ToUtcDateTime(),
                TestDate = model.TestDate.ToUtcDateTime(),
                FirstResult = model.FirstResult,
                SecondResult = model.SecondResult
            };

            if (model.CertificateFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                concreteQualityCertificate.CertificateFileUrl = await storage.UploadFile(model.CertificateFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(model.CertificateFile.FileName), ConstantHelpers.Storage.Blobs.CONCRETE_QUALITY_CERTIFICATE);
            }

            if(model.For07File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                concreteQualityCertificate.For07FileUrl = await storage.UploadFile(model.For07File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY, System.IO.Path.GetExtension(model.For07File.FileName), ConstantHelpers.Storage.Blobs.FOR_07);
            }

            await _context.ConcreteQualityCertificates.AddAsync(concreteQualityCertificate);
            await _context.ConcreteQualityCertificateDetails
                .AddRangeAsync(model.Details.Select(x => new ConcreteQualityCertificateDetail
                {
                    ConcreteQualityCertificate = concreteQualityCertificate,
                    Segment = x.Segment,
                    SegmentNumber = x.SegmentNumber,
                    SewerBoxId = x.SewerBoxId
                }));
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}