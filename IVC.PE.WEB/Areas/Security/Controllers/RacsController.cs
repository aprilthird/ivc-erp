using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.Security;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Security.ViewModels.RacsViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/racs")]
    public class RacsController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public RacsController(IvcDbContext context,
            ILogger<RacsController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int? year = null, int? month = null)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var racs = await _context.Set<UspRacsAll>().FromSqlRaw("execute Security_uspRacsAll @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (year.HasValue)
                racs = racs.Where(x => x.ReportDate.Year == year.Value).ToList();
            if (month.HasValue)
                racs = racs.Where(x => x.ReportDate.Month == month.Value).ToList();

            return Ok(racs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var racs = await _context.RacsReports.Include(x => x.Project)
                .Where(x => x.Id == id)
                .Select(x => new RacsViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    ProjectId = x.ProjectId,
                    ReportDate = x.ReportDate.ToDateString(),
                    ApplicationUserId = x.ApplicationUserId,
                    SewerGroupId = x.SewerGroupId,
                    IdentifiesSC = x.IdentifiesSC,
                    DescriptionIdentifiesSC = x.DescriptionIdentifiesSC,
                    SCQ01 = x.SCQ01,
                    SCQ02 = x.SCQ02,
                    SCQ03 = x.SCQ03,
                    SCQ04 = x.SCQ04,
                    SCQ05 = x.SCQ05,
                    SCQ06 = x.SCQ06,
                    SCQ07 = x.SCQ07,
                    SCQ08 = x.SCQ08,
                    SCQ09 = x.SCQ09,
                    SCQ10 = x.SCQ10,
                    SCQ11 = x.SCQ11,
                    SCQ12 = x.SCQ12,
                    SCQ13 = x.SCQ13,
                    SCQ14 = x.SCQ14,
                    SCQ15 = x.SCQ15,
                    SCQ16 = x.SCQ16,
                    SCQ17 = x.SCQ17,
                    SCQ18 = x.SCQ18,
                    SCQ19 = x.SCQ19,
                    SCQ20 = x.SCQ20,
                    SCQ21 = x.SCQ21,
                    SCQ22 = x.SCQ22,
                    SCQ23 = x.SCQ23,
                    SCQ24 = x.SCQ24,
                    SCQ25 = x.SCQ25,
                    SCQ26 = x.SCQ26,
                    SCQ27 = x.SCQ27,
                    SCQ28 = x.SCQ28,
                    SpecifyConditions = x.SpecifyConditions,
                    IdentifiesSA = x.IdentifiesSA,
                    DescriptionIdentifiesSA = x.DescriptionIdentifiesSA,
                    SAQ01 = x.SAQ01,
                    SAQ02 = x.SAQ02,
                    SAQ03 = x.SAQ03,
                    SAQ04 = x.SAQ04,
                    SAQ05 = x.SAQ05,
                    SAQ06 = x.SAQ06,
                    SAQ07 = x.SAQ07,
                    SAQ08 = x.SAQ08,
                    SAQ09 = x.SAQ09,
                    SAQ10 = x.SAQ10,
                    SAQ11 = x.SAQ11,
                    SAQ12 = x.SAQ12,
                    SAQ13 = x.SAQ13,
                    SAQ14 = x.SAQ14,
                    SAQ15 = x.SAQ15,
                    SAQ16 = x.SAQ16,
                    SAQ17 = x.SAQ17,
                    SAQ18 = x.SAQ18,
                    SAQ19 = x.SAQ19,
                    SAQ20 = x.SAQ20,
                    SAQ21 = x.SAQ21,
                    SAQ22 = x.SAQ22,
                    SAQ23 = x.SAQ23,
                    SAQ24 = x.SAQ24,
                    SAQ25 = x.SAQ25,
                    SAQ26 = x.SAQ26,
                    SAQ27 = x.SAQ27,
                    SAQ28 = x.SAQ28,
                    SAQ29 = x.SAQ29,
                    SAQ30 = x.SAQ30,
                    SAQ31 = x.SAQ31,
                    SAQ32 = x.SAQ32,
                    SpecifyActs = x.SpecifyActs,
                    ICQ01 = x.ICQ01,
                    ICQ02 = x.ICQ02,
                    ICQ03 = x.ICQ03,
                    ICQ04 = x.ICQ04,
                    ICQ05 = x.ICQ05,
                    SpecifyAppliedCorrections = x.SpecifyAppliedCorrections,
                    SpecifyAnotherAlternative = x.SpecifyAnotherAlternative,
                    LiftingObservations = x.LiftingObservations,
                    Status = x.Status
                }).FirstOrDefaultAsync();

            return Ok(racs);
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var racs = await _context.RacsReports.FirstOrDefaultAsync(x => x.Id == id);

            if (racs.ObservationImageUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.RACS}/{racs.ObservationImageUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.SECURITY);
            }

            if (racs.LocationImageUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.RACS}/{racs.LocationImageUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.SECURITY);
            }

            if (racs.SignatureUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.RACS}/{racs.SignatureUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.SECURITY);
            }

            if (racs.LiftingImageUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.RACS}/{racs.LiftingImageUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.SECURITY);
            }

            _context.RacsReports.Remove(racs);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("descargar-pdf")]
        public async Task<IActionResult> DownloadPdfRacs(string url, string filename)
        {
            Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp");

            if (!String.IsNullOrEmpty(url))
            {
                serviceUrl = new Uri(serviceUrl + $"?url={url}");
            }

            byte[] pdfBytes = await new WebClient().DownloadDataTaskAsync(serviceUrl);
            return File(pdfBytes, "application/pdf", $"{filename}.pdf");
        }

        [HttpGet("generar-pdf/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePdfRacs(Guid id)
        {
            SqlParameter racsParam = new SqlParameter("@RacsId", id);

            var racs = await _context.Set<UspRacsH>().FromSqlRaw("execute Security_uspRacs @RacsId"
                , racsParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return View("TestPdfH", racs.First());
        }
    }
}
