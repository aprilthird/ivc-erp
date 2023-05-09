using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Reports.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.REPORTS)]
    [Route("reportes/calidad")]
    public class QualityController : BaseController
    {
        public QualityController(IvcDbContext context, ILogger<QualityController> logger) : base(context, logger)
        {
        }

        [HttpGet("tramos-certificados")]
        public IActionResult CertifiedSewerLines() => View();

        [HttpGet("tramos-certificados/certificados")]
        public async Task<IActionResult> CertifiedSewerLinesChart1(Guid? workFrontId = null, Guid? sewerGroupId = null)
        {
            var query = _context.SewerLines
                .Where(x => x.Stage == ConstantHelpers.Stage.REAL)
                .AsNoTracking().AsQueryable();
            if (workFrontId.HasValue)
                query = query.Where(x => x.SewerGroup.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            var total = await query.CountAsync();
            var hasCertificate = await query.Where(x => x.CompactionDensityCertificate != null).CountAsync();
            var noCertificate = total - hasCertificate;
            return Ok(new {
                hasCertificate,
                noCertificate
            });
        }

        [HttpGet("tramos-certificados/archivos")]
        public async Task<IActionResult> CertifiedSewerLinesChart2(Guid? workFrontId = null, Guid? sewerGroupId = null)
        {
            var query = _context.SewerLines
                .Where(x => x.Stage == ConstantHelpers.Stage.REAL)
                .AsNoTracking().AsQueryable();
            if (workFrontId.HasValue)
                query = query.Where(x => x.SewerGroup.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId.Value);
            var total = await query.CountAsync();
            var hasCertificateAndFile = await query.Where(x => x.CompactionDensityCertificate != null).Where(x => x.CompactionDensityCertificate.FileUrl != null).CountAsync();
            var noCertificateAndFile = total - hasCertificateAndFile;
            return Ok(new
            {
                hasCertificateAndFile,
                noCertificateAndFile
            });
        }


        [HttpGet("tramos-certificados/fecha-de-ejecucion")]
        public async Task<IActionResult> CertifiedSewerLinesChart3(Guid? workFrontId = null, Guid? sewerGroupId = null)
        {
            var query = _context.CompactionDensityCertificates
                .AsNoTracking().AsQueryable();
            if (workFrontId.HasValue)
                query = query.Where(x => x.SewerLine.SewerGroup.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerLine.SewerGroupId == sewerGroupId.Value);
            var data = await query
                .OrderBy(x => x.ExecutionDate)
                .Select(x => new
                {
                    day = x.ExecutionDate.ToDefaultTimeZone().Day,
                    month = x.ExecutionDate.ToDefaultTimeZone().Month,
                    year = x.ExecutionDate.ToDefaultTimeZone().Year
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("tramos-certificados/pruebas-de-laboratorio")]
        public async Task<IActionResult> CertifiedSewerLinesChart4(Guid? workFrontId = null, Guid? sewerGroupId = null, Guid? fillingLaboratoryTestId = null)
        {
            var query = _context.CompactionDensityCertificateDetails
                .AsNoTracking().AsQueryable();
            if (workFrontId.HasValue)
                query = query.Where(x => x.CompactionDensityCertificate.SewerLine.SewerGroup.WorkFrontId == workFrontId.Value);
            if (sewerGroupId.HasValue)
                query = query.Where(x => x.CompactionDensityCertificate.SewerLine.SewerGroupId == sewerGroupId.Value);
            var data = await query
                .GroupBy(x => x.Layer)
                .Select(x => new
                {
                    label = x.Key,
                    total = x.Count()
                }).ToListAsync();
            return Ok();
        }

    }
}