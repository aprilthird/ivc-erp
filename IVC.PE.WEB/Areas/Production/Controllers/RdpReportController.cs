using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.RdpItemViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.RdpReportViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/rdp")]
    public class RdpReportController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public RdpReportController(IvcDbContext context,
            ILogger<RdpReportController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? sgId, Guid? ppId, string rptDate)
        {
            if (sgId == Guid.Empty || ppId == Guid.Empty)
                return BadRequest("No existen actividades para el Cuadrilla/Fase seleccionada.");

            var rdpDate = rptDate.ToDateTime();
            var report = await _context.RdpReports
                .Where(x => x.SewerGroupId == sgId.Value && x.ProjectPhaseId == ppId.Value && x.ReportDate.Date == rdpDate.Date)
                .Select(x => new RdpReportViewModel
                {
                    Id = x.Id,
                    ProjectPhaseId = x.ProjectPhaseId,
                    SewerGroupId = x.SewerGroupId,
                    ReportDate = x.ReportDate
                })
                .FirstOrDefaultAsync();

            var items = await _context.RdpItems
                .Where(x => x.ProjectPhaseId == ppId.Value)
                .Select(x => new RdpItemViewModel
                {
                    Id = x.Id,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ItemGroup = x.ItemGroup,
                    ItemPhaseCode = x.ItemPhaseCode,
                    ItemDescription = x.ItemDescription,
                    ItemUnit = x.ItemUnit,
                    ItemContractualAmmount = x.ItemContractualAmmount,
                    ItemStakeOutAmmount = x.ItemStakeOutAmmount
                }).ToListAsync();

            if (report == null)
            {
                report = new RdpReportViewModel
                {
                    ProjectPhaseId = ppId.Value,
                    SewerGroupId = sgId.Value,
                    ReportDate = rdpDate
                };

                var accumulateds = await _context.RdpItemAccumulatedAmmounts
                .Where(x => x.SewerGroupId == sgId.Value)
                .ToListAsync();

                report.RdpItems = items.Select(x => new RdpItemFootageViewModel {
                    RdpItemId = x.Id,
                    RdpItem = x,
                    RdpDate = rptDate,
                    SewerGroupId = sgId.Value,
                    PartialAmmount = 0.00M,
                    StakeOut = x.ItemStakeOutAmmount,
                    AccumulatedAmmount = accumulateds.Count() == 0 ? 0.00M : accumulateds.FirstOrDefault(y => y.RdpItemId == x.Id).AccumulatedAmmount
                }).OrderBy(x => x.RdpItem.ItemGroup).ThenBy(x => x.RdpItem.ItemPhaseCode).ToList();
            } else
            {
                var footages = await _context.RdpItemFootages
                    .Where(x => x.RdpReportId == report.Id)
                    .ToListAsync();

                report.RdpItems = items.Select(x => new RdpItemFootageViewModel
                {
                    RdpItemId = x.Id,
                    RdpItem = x,
                    RdpDate = rptDate,
                    SewerGroupId = sgId.Value,
                    PartialAmmount = footages.FirstOrDefault(y => y.RdpItemId == x.Id).PartialAmmount ?? 0.00M,
                    StakeOut = x.ItemStakeOutAmmount,
                    AccumulatedAmmount = footages.FirstOrDefault(y => y.RdpItemId == x.Id).AccumulatedAmmount ?? 0.00M
                }).OrderBy(x => x.RdpItem.ItemGroup).ThenBy(x => x.RdpItem.ItemPhaseCode).ToList();
            }

            return PartialView("_Fields", report);

            //return Ok(report);
        }
    }
}
