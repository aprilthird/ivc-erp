using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.NewSIGProcessViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.IntegratedManagementSystem.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.INTEGRATED_MANAGEMENT_SYSTEM)]
    [Route("sistema-de-manejo-integrado/for11-no-conformidad")]
    public class SewerManifoldFor24Controller : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor24Controller(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor24Controller> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var for11s = await _context.SewerManifoldFor24s
                .Select(x => new SewerManifoldFor24ViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor24FirstPartId = x.SewerManifoldFor24FirstPartId,
                    SewerManifoldFor24FirstPart = new SewerManifoldFor24FirstPartViewModel
                    {
                        NewSIGProcess = new NewSIGProcessViewModel
                        {
                            ProcessName = x.SewerManifoldFor24FirstPart.NewSIGProcess.ProcessName,
                            Code = x.SewerManifoldFor24FirstPart.NewSIGProcess.Code
                        },
                        Date = x.SewerManifoldFor24FirstPart.Date.ToDateString(),
                        ReportUserName = x.SewerManifoldFor24FirstPart.ReportUserName,
                        NCOrigin = x.SewerManifoldFor24FirstPart.NCOrigin,
                        SewerGroup = new SewerGroupViewModel
                        {
                            Code = x.SewerManifoldFor24FirstPart.SewerGroup.Code
                        },
                        Provider = new ProviderViewModel
                        {
                            BusinessName = x.SewerManifoldFor24FirstPart.Provider.BusinessName
                        },
                        Client = x.SewerManifoldFor24FirstPart.Client,
                        Description = x.SewerManifoldFor24FirstPart.Description,
                        ResponsableUserName = x.SewerManifoldFor24FirstPart.ResponsableUserName,
                        FileUrl = x.SewerManifoldFor24FirstPart.FileUrl,
                        VideoUrl = x.SewerManifoldFor24FirstPart.VideoUrl,
                        Gallery = x.SewerManifoldFor24FirstPart.for24FirstPartGallery.Select(g => new For24FirstPartGalleryViewModel
                        {
                            Id = g.Id,
                            Name = g.Name,
                            URL = g.URL
                        }).ToList()
                    },
                    SewerManifoldFor24SecondPartId = x.SewerManifoldFor24SecondPartId,
                    SewerManifoldFor24SecondPart = new SewerManifoldFor24SecondPartViewModel
                    {
                        Decision = x.SewerManifoldFor24SecondPart.Decision,
                        Other = x.SewerManifoldFor24SecondPart.Other,
                        LaborerQuantity = x.SewerManifoldFor24SecondPart.LaborerQuantity,
                        LaborerHoursMan = x.SewerManifoldFor24SecondPart.LaborerHoursMan.ToString(),
                        LaborerTotalHoursMan = x.SewerManifoldFor24SecondPart.LaborerTotalHoursMan.ToString(),
                        OfficialQuantity = x.SewerManifoldFor24SecondPart.OfficialQuantity,
                        OfficialHoursMan = x.SewerManifoldFor24SecondPart.OfficialHoursMan.ToString(),
                        OfficialTotalHoursMan = x.SewerManifoldFor24SecondPart.OfficialTotalHoursMan.ToString(),
                        OperatorQuantity = x.SewerManifoldFor24SecondPart.OperatorQuantity,
                        OperatorHoursMan = x.SewerManifoldFor24SecondPart.OperatorHoursMan.ToString(),
                        OperatorTotalHoursMan = x.SewerManifoldFor24SecondPart.OperatorTotalHoursMan.ToString(),
                        FileUrl = x.SewerManifoldFor24SecondPart.FileUrl,
                        ProposedDate = x.SewerManifoldFor24SecondPart.ProposedDate.ToDateString(),
                        VideoUrl = x.SewerManifoldFor24SecondPart.VideoUrl,
                        Gallery = x.SewerManifoldFor24SecondPart.for24SecondPartGallery.Select(g => new For24SecondPartGalleryViewModel
                        {
                            Id = g.Id,
                            Name = g.Name,
                            URL = g.URL
                        }).ToList()
                    },
                    SewerManifoldFor24ThirdpartId = x.SewerManifoldFor24ThirdpartId,
                    SewerManifoldFor24ThirdPart = new SewerManifoldFor24ThirdPartViewModel
                    {
                        ActionTaken = x.SewerManifoldFor24ThirdPart.ActionTaken,
                        PreventiveCorrectiveAction = x.SewerManifoldFor24ThirdPart.PreventiveCorrectiveAction,
                        ClosingDate = x.SewerManifoldFor24ThirdPart.ClosingDate.ToDateString(),
                        FileUrl = x.SewerManifoldFor24ThirdPart.FileUrl
                    }
                }).ToListAsync();

            return Ok(for11s);
        }

    }
}
