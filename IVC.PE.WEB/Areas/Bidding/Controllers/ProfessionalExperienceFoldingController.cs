using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.PositionViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalExperienceFoldingViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalsViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/profesionales-experiencias")]
    public class ProfessionalExperienceFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProfessionalExperienceFoldingController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<ProfessionalExperienceFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId, Guid? positionId = null)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<ProfessionalExperienceFoldingViewModel>());

            var renovations = await _context.ProfessionalExperienceFoldings
                .Include(x => x.Professional)
                .Include(x => x.Business)
                .Include(x => x.BiddingWork)
                .Include(x => x.Position)
                .Where(x => x.ProfessionalId == equipId)
                .Select(x => new ProfessionalExperienceFoldingViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    ProfessionalId = x.ProfessionalId,
                    BusinessId = x.BusinessId,
                    Business = new BusinessViewModel
                    {
                        Tradename = x.Business.Tradename,
                        Type = x.Business.Type
                    },
                    BiddingWorkId = x.BiddingWorkId,
                    BiddingWork = new BiddingWorkViewModel
                    {
                        Name = x.BiddingWork.Name,
                        CurrencyType = x.BiddingWork.CurrencyType
                    },
                    PositionId = x.PositionId,
                    Position = new PositionViewModel
                    {
                        Name = x.Position.Name
                    },
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    Dif = x.Dif,
                    Order = x.Order,
                    Observations = x.Observations,
                    FileUrl = x.FileUrl
                    

                })
                .ToListAsync();

            if(positionId.HasValue)
            {
                renovations = renovations.Where(x => x.PositionId == positionId).ToList();
            }

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.ProfessionalExperienceFoldings
                .Include(x => x.Professional)
                .Include(x => x.Business)
                .Include(x => x.BiddingWork)
                .Include(x => x.Position)
                .Where(x => x.Id == id)
                .Select(x => new ProfessionalExperienceFoldingViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    ProfessionalId = x.ProfessionalId,
                    BusinessId = x.BusinessId,
                    BiddingWorkId = x.BiddingWorkId,
                    PositionId = x.PositionId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    Dif = x.Dif,
                    Order = x.Order,
                    Observations = x.Observations,
                    FileUrl = x.FileUrl

                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProfessionalExperienceFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var last = await _context.BiddingWorks.
    FirstOrDefaultAsync(x=>x.Id == model.BiddingWorkId);

            var equipmentMachineryTransport = await _context.Professionals
                .Where(x => x.Id == model.ProfessionalId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.NumberOfExperiences++;

            var newRenovation = new ProfessionalExperienceFolding()
            {
                ProfessionalId = model.ProfessionalId,
                BusinessId = model.BusinessId,
                BiddingWorkId = model.BiddingWorkId,
                PositionId = model.PositionId,
                StartDate = model.StartDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                Dif = EF.Functions.DateDiffDay(model.StartDate.ToDateTime(), model.EndDate.ToDateTime()) +1,
                Observations = model.Observations,
                Order = equipmentMachineryTransport.NumberOfExperiences

            };

            if (newRenovation.StartDate < last.StartDate)
                return BadRequest("Rango de fechas equivocado.");

            if (newRenovation.EndDate > last.EndDate)
                return BadRequest("Rango de fechas equivocado.");






            await _context.ProfessionalExperienceFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EXPERIENCE,
                    $"experiencia_{equipmentMachineryTransport.Document}_{equipmentMachineryTransport.NumberOfExperiences}");

                await _context.SaveChangesAsync();
            }


            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProfessionalExperienceFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.Professionals
                .Where(x => x.Id == model.ProfessionalId)
                .FirstOrDefaultAsync();

            var last = await _context.BiddingWorks.
FirstOrDefaultAsync(x => x.Id == model.BiddingWorkId);

            var folding = await _context.ProfessionalExperienceFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.BusinessId = model.BusinessId;
            folding.BiddingWorkId = model.BiddingWorkId;
            folding.PositionId = model.PositionId;
            folding.StartDate = model.StartDate.ToDateTime();
            folding.EndDate = model.EndDate.ToDateTime();
            folding.Dif = EF.Functions.DateDiffDay(model.StartDate.ToDateTime(), model.EndDate.ToDateTime()) + 1;
            folding.Observations = model.Observations;

            if (folding.StartDate < last.StartDate)
                return BadRequest("Rango de fechas equivocado.");

            if (folding.EndDate > last.EndDate)
                return BadRequest("Rango de fechas equivocado.");


            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EXPERIENCE}/{folding.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                folding.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EXPERIENCE,
                    $"experiencia_{equipmentMachineryTransport.Document}_{equipmentMachineryTransport.NumberOfExperiences}");
            }



         


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.ProfessionalExperienceFoldings.FirstOrDefaultAsync(x => x.Id == id);


            if (renovation.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EXPERIENCE}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            var equipment = await _context.Professionals
    .FirstOrDefaultAsync(x => x.Id == renovation.ProfessionalId);
            equipment.NumberOfExperiences--;



            _context.ProfessionalExperienceFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("listar-scheduler")]
        public async Task<IActionResult> GetAllScheduler(ShedulerProExpViewModel model)
        {
            var profePositions = _context.ProfessionalExperienceFoldings.
                Include(x => x.Professional)
                .Where(x => x.ProfessionalId == model.ProfessionalId
                && x.PositionId == model.PositionId).Select(x => new ProfessionalExperienceFoldingViewModel
                {
                    ProfessionalId = x.ProfessionalId,
                    BiddingWorkId = x.BiddingWorkId,
                    BiddingWork = new BiddingWorkViewModel
                    {
                        Name = x.BiddingWork.Name,
                        Number = x.BiddingWork.CodeNumber.ToString("D3"),
                        StartDateSche = x.BiddingWork.StartDate,
                        EndDateSche = x.BiddingWork.EndDate,
                        StartDate = x.BiddingWork.StartDate.ToDateString(),
                        EndDate = x.BiddingWork.EndDate.ToDateString(),
                        CurrencyType = x.BiddingWork.CurrencyType

                    },
                    PositionId = x.PositionId,
                    Position = new PositionViewModel
                    {
                        Name = x.Position.Name
                    },
                    StartDateSche = x.StartDate,
                    EndDateSche = x.EndDate,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString()



                }).ToList();



            //    .Include(x => x.ProjectFormula)
            //    .Include(x => x.WorkFrontHead)
            //    .Include(x => x.WorkFrontHead.User)
            //    .Include(x => x.WorkFront)
            //    .Include(x => x.SewerGroup)
            //    .Where(x => x.ProjectId == GetProjectId());


            //var pdps = _context.ProductionDailyParts
            //    .Include(x => x.ProjectFormula)
            //    .Include(x => x.WorkFrontHead)
            //    .Include(x => x.WorkFrontHead.User)
            //    .Include(x => x.WorkFront)
            //    .Include(x => x.SewerGroup)
            //    .Where(x => x.ProjectId == GetProjectId());

            //var pdpsfiltered = new List<ProductionDailyPart>();

            //foreach (var group in model.SewerGroupShedulers)
            //{
            //    pdpsfiltered.AddRange(pdps.Where(x => x.SewerGroupId == group).ToList());
            //}

            //var sewerManifoldRefs = await _context.SewerManifoldReferences
            //    .Include(y => y.SewerManifoldReview)
            //    .Include(y => y.SewerManifoldExecution)
            //    .Include(y => y.SewerManifoldReview.SewerBoxStart)
            //    .Include(y => y.SewerManifoldReview.SewerBoxEnd)
            //    .ToListAsync();

            //var query = new List<F7PdpViewModel>();

            //foreach (var pdp in pdpsfiltered)
            //{
            //    var smReview = sewerManifoldRefs
            //        .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id)
            //        .Select(x => x.SewerManifoldReview)
            //        .FirstOrDefault();

            //    var folding = await _context.FoldingF7s.Where(x => x.ProductionDailyPartId == pdp.Id && x.Date >= model.StartDate.ToDateTime() && x.Date <= model.EndDate.ToDateTime()).ToListAsync();

            //    if (folding.Count != 0)
            //    {
            //        query.Add(new F7PdpViewModel
            //        {
            //            Id = pdp.Id,
            //            ProjectFormula = new ProjectFormulaViewModel
            //            {
            //                Code = pdp.ProjectFormula.Code
            //            },
            //            ReportDate = pdp.ReportDate.ToDateString(),
            //            WorkFrontHead = new WorkFrontHeadViewModel
            //            {
            //                User = new UserViewModel
            //                {
            //                    AuxFullName = pdp.WorkFrontHead.User.FullName
            //                }
            //            },
            //            SewerGroup = new SewerGroupViewModel
            //            {
            //                Code = pdp.SewerGroup.Code
            //            },
            //            SewerManifold = new SewerManifoldViewModel
            //            {
            //                Name = smReview.Name,
            //                LengthOfDigging = smReview.LengthOfDigging,
            //                LengthOfPipeInstalled = smReview.LengthOfPipelineInstalled,
            //                DitchHeight = smReview.DitchHeight,
            //            },
            //            Filling = pdp.Filling,
            //            TheoreticalLayer = pdp.TheoreticalLayer,
            //            FillLength = pdp.FillLength,
            //            ExcavatedLength = pdp.ExcavatedLength,
            //            InstalledLength = pdp.InstalledLength,
            //            RefilledLength = pdp.RefilledLength,
            //            GranularBaseLength = pdp.GranularBaseLength,
            //            Excavation = pdp.Excavation,
            //            Installation = pdp.Installation,
            //            Filled = pdp.Filled,
            //            Status = pdp.Status,
            //            Folding = folding.Select(x => new FoldingF7ViewModel()
            //            {
            //                Id = x.Id,
            //                ExcavatedLength = Math.Round(x.ExcavatedLength, MidpointRounding.AwayFromZero).ToString(),
            //                InstalledLength = Math.Round(x.InstalledLength, MidpointRounding.AwayFromZero).ToString(),
            //                RefilledLength = Math.Round(x.RefilledLength, MidpointRounding.AwayFromZero).ToString(),
            //                GranularBaseLength = Math.Round(x.GranularBaseLength, MidpointRounding.AwayFromZero).ToString(),
            //                CalendarDate = x.Date
            //            }).ToList()
            //        });
            //    }
            //}
            return Ok(profePositions);
        }
    }
}
