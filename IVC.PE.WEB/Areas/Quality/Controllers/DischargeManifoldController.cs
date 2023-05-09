using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.DischargeManifoldViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
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

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/colector-descarga")]

    public class DischargeManifoldController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public DischargeManifoldController(IvcDbContext context,
            ILogger<DischargeManifoldController> logger,
            IOptions<CloudStorageCredentials> storageCredentials) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {

            var pId = GetProjectId();

            var query = _context.DischargeManifolds
              .AsQueryable();

            var data = await query
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.SewerBoxStart)
                .Include(x => x.SewerManifold.SewerBoxEnd)
                .Where(x => x.ProjectId == pId)
                .Select(x => new DischargeManifoldViewModel
                {
                    Id = x.Id,

                    ProtocolNumber = x.ProtocolNumber,

                    SewerManifoldId = x.SewerManifoldId,
                    SewerManifold = new SewerManifoldFor01ViewModel
                    {
                        Id = x.SewerManifoldId,
                        Name = x.SewerManifold.Name,

                        LengthBetweenHAxles = x.SewerManifold.LengthBetweenHAxles,
                        LengthBetweenIAxles = x.SewerManifold.LengthBetweenIAxles,
                        LengthOfPipelineInstalled = x.SewerManifold.LengthOfPipelineInstalled,
                        DitchHeight = x.SewerManifold.DitchHeight,
                        DitchLevelPercent = x.SewerManifold.DitchLevelPercent,
                        SewerBoxStart = new SewerBoxViewModel
                        {
                            Id = x.SewerManifold.SewerBoxStartId.Value,
                            Code = x.SewerManifold.SewerBoxStart.Code,
                            CoverLevel = x.SewerManifold.SewerBoxStart.CoverLevel,
                            BottomLevel = x.SewerManifold.SewerBoxStart.BottomLevel,
                            ArrivalLevel = x.SewerManifold.SewerBoxStart.ArrivalLevel,
                            Height = x.SewerManifold.SewerBoxStart.Height
                        },
                        SewerBoxEnd = new SewerBoxViewModel
                        {
                            Id = x.SewerManifold.SewerBoxStartId.Value,
                            Code = x.SewerManifold.SewerBoxEnd.Code,
                            CoverLevel = x.SewerManifold.SewerBoxEnd.CoverLevel,
                            BottomLevel = x.SewerManifold.SewerBoxEnd.BottomLevel,
                            ArrivalLevel = x.SewerManifold.SewerBoxEnd.ArrivalLevel,
                            Height = x.SewerManifold.SewerBoxEnd.Height
                        }

                    },
                    EquipmentCertificateId = x.EquipmentCertificateId,
                    EquipmentCertificate = new EquipmentCertificateViewModel
                    {
                        Serial = x.EquipmentCertificate.Serial
                    },
                    EquipmentCertificate2Id = x.EquipmentCertificate2Id,
                    EquipmentCertificate2 = new EquipmentCertificateViewModel
                    {
                        Serial = x.EquipmentCertificate2.Serial
                    },
                    //EquipmentCertificteRenewalId = x.EquipmentCertificateRenewalId ?? Guid.Empty,
                    //EquipmentCertificateRenewal = new EquipmentCertificateRenewalViewModel
                    //{


                    //},
                    Producer = x.Producer,
                    PipeBatch = x.PipeBatch,
                    SecondPipeBatch = x.SecondPipeBatch,
                    ThirdPipeBatch = x.ThridPipeBatch,
                    FourthPipeBatch = x.ForthPipeBatch,
                    Leveling = x.Leveling.Date.ToDateString(),
                    OpenZTest = x.OpenZTest.HasValue
                    ? x.OpenZTest.Value.Date.ToDateString() : String.Empty,
                    ClosedZTest = x.ClosedZTest.HasValue
                    ? x.ClosedZTest.Value.Date.ToDateString() : String.Empty,
                    MirrorTest = x.MirrorTest.HasValue
                    ? x.MirrorTest.Value.Date.ToDateString() : String.Empty,
                    BallTest = x.BallTest.HasValue
                    ? x.BallTest.Value.Date.ToDateString() : String.Empty,

                    BookPZO = x.BookPZO,
                    SeatPZC = x.SeatPZC,
                    BookPZF = x.BookPZF,
                    SeatPZF = x.SeatPZF,

                    FileUrl = x.FileUrl,
                    MirrorTestVideoUrl = x.MirrorTestVideoUrl,
                    MonkeyBallTestVideoUrl = x.MonkeyBallTestVideoUrl,
                    ZoomTestVideoUrl = x.ZoomTestVideoUrl
                    //VideoUrl = x.VideoUrl

                    //Book = x.Book,
                    //Seat = x.Seat
                })
                .ToListAsync();


            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.DischargeManifolds
                 .Include(x => x.Project)
                 .Include(x => x.SewerManifold)
                 .Include(x => x.SewerManifold.SewerBoxStart)
                 .Include(x => x.SewerManifold.SewerBoxEnd)
                 .Where(x => x.Id == id)
                 .Select(x => new DischargeManifoldViewModel
                 {
                     Id = x.Id,

                     ProtocolNumber = x.ProtocolNumber,

                     SewerManifoldId = x.SewerManifoldId,
                     SewerManifold = new SewerManifoldFor01ViewModel
                     {
                         Id = x.SewerManifoldId,
                         Name = x.SewerManifold.Name,
                         LengthBetweenHAxles = x.SewerManifold.LengthBetweenHAxles,
                         LengthBetweenIAxles = x.SewerManifold.LengthBetweenIAxles,
                         LengthOfPipelineInstalled = x.SewerManifold.LengthOfPipelineInstalled,
                         DitchHeight = x.SewerManifold.DitchHeight,
                         DitchLevelPercent = x.SewerManifold.DitchLevelPercent,
                         SewerBoxStartId = x.SewerManifold.SewerBoxStartId.Value,
                         SewerBoxStart = new SewerBoxViewModel
                         {
                             Id = x.SewerManifold.SewerBoxStartId.Value,
                             CoverLevel = x.SewerManifold.SewerBoxStart.CoverLevel,
                             BottomLevel = x.SewerManifold.SewerBoxStart.BottomLevel,
                             ArrivalLevel = x.SewerManifold.SewerBoxStart.ArrivalLevel,
                             Height = x.SewerManifold.SewerBoxStart.Height
                         },
                         SewerBoxEndId = x.SewerManifold.SewerBoxEndId.Value,
                         SewerBoxEnd = new SewerBoxViewModel
                         {
                             Id = x.SewerManifold.SewerBoxStartId.Value,
                             CoverLevel = x.SewerManifold.SewerBoxEnd.CoverLevel,
                             BottomLevel = x.SewerManifold.SewerBoxEnd.BottomLevel,
                             ArrivalLevel = x.SewerManifold.SewerBoxEnd.ArrivalLevel,
                             Height = x.SewerManifold.SewerBoxEnd.Height
                         }

                     },

                     EquipmentCertificateId = x.EquipmentCertificateId,
                     EquipmentCertificate = new EquipmentCertificateViewModel
                     {
                         Serial = x.EquipmentCertificate.Serial
                     },
                     EquipmentCertificate2Id = x.EquipmentCertificate2Id,
                     EquipmentCertificate2 = new EquipmentCertificateViewModel
                     {
                         Serial = x.EquipmentCertificate2.Serial
                     },


                     Producer = x.Producer,
                     PipeBatch = x.PipeBatch,
                     SecondPipeBatch = x.SecondPipeBatch,
                     ThirdPipeBatch = x.ThridPipeBatch,
                     FourthPipeBatch = x.ForthPipeBatch,
                     Leveling = x.Leveling.Date.ToDateString(),
                     OpenZTest = x.OpenZTest.HasValue
                    ? x.OpenZTest.Value.Date.ToDateString() : String.Empty,
                     ClosedZTest = x.ClosedZTest.HasValue
                    ? x.ClosedZTest.Value.Date.ToDateString() : String.Empty,
                     MirrorTest = x.MirrorTest.HasValue
                    ? x.MirrorTest.Value.Date.ToDateString() : String.Empty,
                     BallTest = x.BallTest.HasValue
                    ? x.BallTest.Value.Date.ToDateString() : String.Empty,

                     BookPZO = x.BookPZO,
                     SeatPZC = x.SeatPZC,
                     BookPZF = x.BookPZF,
                     SeatPZF = x.SeatPZF,

                     FileUrl = x.FileUrl,
                     MirrorTestVideoUrl = x.MirrorTestVideoUrl,
                     MonkeyBallTestVideoUrl = x.MonkeyBallTestVideoUrl,
                     ZoomTestVideoUrl = x.ZoomTestVideoUrl
                     //VideoUrl = x.VideoUrl

                     //Book = x.Book,
                     //Seat = x.Seat
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpGet("colector/{id}")]
        public async Task<IActionResult> GetSewer(Guid id)
        {
            var manifolReview = await _context.SewerManifoldReferences
                .Include(x => x.SewerManifoldReview)
                .Include(x => x.SewerManifoldReview.SewerBoxStart)
                .Include(x => x.SewerManifoldReview.SewerBoxEnd)
                .Where(x => x.SewerManifoldExecutionId == id)
                .Select(x => new SewerManifoldFor01ViewModel
                {
                    Id = x.Id,
                    SewerBoxStartId = x.SewerManifoldReview.SewerBoxStartId.Value,
                    SewerBoxStart = new SewerBoxViewModel
                    {
                        Id = x.SewerManifoldReview.SewerBoxStartId.Value,
                        CoverLevel = x.SewerManifoldReview.SewerBoxStart.CoverLevel,
                        BottomLevel = x.SewerManifoldReview.SewerBoxStart.BottomLevel,
                        //ArrivalLevel = x.SewerBoxStart.ArrivalLevel
                    },
                    SewerBoxEndId = x.SewerManifoldReview.SewerBoxEndId.Value,
                    SewerBoxEnd = new SewerBoxViewModel
                    {
                        Id = x.SewerManifoldReview.SewerBoxStartId.Value,
                        CoverLevel = x.SewerManifoldReview.SewerBoxEnd.CoverLevel,
                        BottomLevel = x.SewerManifoldReview.SewerBoxEnd.BottomLevel,
                        ArrivalLevel = x.SewerManifoldReview.SewerBoxEnd.ArrivalLevel
                    },
                    LengthBetweenHAxles = x.SewerManifoldReview.LengthBetweenHAxles,
                    LengthBetweenIAxles = x.SewerManifoldReview.LengthBetweenIAxles
                }).FirstOrDefaultAsync();

            //var exe = await _context.SewerManifolds.FirstOrDefaultAsync(x=>x.Id == id);

            //var data = await _context.SewerManifolds
            //     .Where(x => x.Name == exe.Name && x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.REVIEW)
            //     .Select(x => new SewerManifoldFor01ViewModel
            //     {
            //         Id = x.Id,
            //         SewerBoxStartId = x.SewerBoxStartId.Value,
            //         SewerBoxStart = new SewerBoxViewModel
            //         {
            //             Id = x.SewerBoxStartId.Value,
            //             CoverLevel = x.SewerBoxStart.CoverLevel,
            //             BottomLevel = x.SewerBoxStart.BottomLevel,
            //             //ArrivalLevel = x.SewerBoxStart.ArrivalLevel
            //         },
            //         SewerBoxEndId = x.SewerBoxEndId.Value,
            //         SewerBoxEnd = new SewerBoxViewModel
            //         {
            //             Id = x.SewerBoxStartId.Value,
            //             CoverLevel = x.SewerBoxEnd.CoverLevel,
            //             BottomLevel = x.SewerBoxEnd.BottomLevel,
            //             ArrivalLevel = x.SewerBoxEnd.ArrivalLevel
            //         },
            //         LengthBetweenHAxles = x.LengthBetweenHAxles,
            //         LengthBetweenIAxles = x.LengthBetweenIAxles
            //     }).FirstOrDefaultAsync();

            return Ok(manifolReview);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(DischargeManifoldViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //--SWFor47
            var sewerManifoldFor47 = await _context.SewerManifoldFor47s
                .FirstOrDefaultAsync(x => x.SewerManifoldId == model.SewerManifoldId);
            if (sewerManifoldFor47 != null)
                sewerManifoldFor47.For01ProtocolNumber = model.ProtocolNumber;
            //--SWFor29
            var sewerManifoldFor29 = await _context.SewerManifoldFor29s
                .FirstOrDefaultAsync(x => x.SewerManifoldId == model.SewerManifoldId);
            if (sewerManifoldFor29 != null)
                sewerManifoldFor29.For01ProtocolNumber = model.ProtocolNumber;
            //--SWFor37A
            var sewerManifoldFor37A = await _context.SewerManifoldFor37As
                .FirstOrDefaultAsync(x => x.SewerManifoldId == model.SewerManifoldId);
            if (sewerManifoldFor37A != null)
            {
                sewerManifoldFor37A.For01ProtocolNumber = model.ProtocolNumber;
                sewerManifoldFor37A.FirstPipeBatch = model.PipeBatch;
                sewerManifoldFor37A.SecondPipeBatch = model.SecondPipeBatch;
                sewerManifoldFor37A.ThridPipeBatch = model.ThirdPipeBatch;
                sewerManifoldFor37A.ForthPipeBatch = model.FourthPipeBatch;
            }


            var dischargeManifold = new DischargeManifold
            {
                ProjectId = GetProjectId(),
                ProtocolNumber = model.ProtocolNumber,
                SewerManifoldId = model.SewerManifoldId,
                EquipmentCertificateId = model.EquipmentCertificateId,
                EquipmentCertificate2Id = model.EquipmentCertificate2Id,
                Producer = model.Producer,
                PipeBatch = model.PipeBatch,
                SecondPipeBatch = model.SecondPipeBatch,
                ThridPipeBatch = model.ThirdPipeBatch,
                ForthPipeBatch = model.FourthPipeBatch,
                Leveling = model.Leveling.ToDateTime(),
                OpenZTest = string.IsNullOrEmpty(model.OpenZTest)
                ? (DateTime?)null : model.OpenZTest.ToDateTime(),
                ClosedZTest = string.IsNullOrEmpty(model.ClosedZTest)
                ? (DateTime?)null : model.ClosedZTest.ToDateTime(),
                MirrorTest = string.IsNullOrEmpty(model.MirrorTest)
                ? (DateTime?)null : model.MirrorTest.ToDateTime(),
                BallTest = string.IsNullOrEmpty(model.BallTest)
                ? (DateTime?)null : model.BallTest.ToDateTime(),

                BookPZF = model.BookPZF,
                BookPZO = model.BookPZO,
                SeatPZC = model.SeatPZC,
                SeatPZF = model.SeatPZF
                //Book = model.Book,
                //Seat = model.Seat
            };

            //revisar como hacer que la data acepte nulls

            var manifold = await _context.SewerManifolds
                            .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);
            manifold.HasFor01 = true; //-> permite solo 1 registro

            var sewerBoxStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);

            sewerBoxStart.CoverLevel = model.SewerManifold.SewerBoxStart.CoverLevel;
            //sewerBoxStart.ArrivalLevel = model.SewerManifold.SewerBoxStart.ArrivalLevel;
            sewerBoxStart.BottomLevel = model.SewerManifold.SewerBoxStart.BottomLevel;
            sewerBoxStart.Height = sewerBoxStart.CoverLevel - sewerBoxStart.BottomLevel;

            var sewerBoxEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);

            sewerBoxEnd.CoverLevel = model.SewerManifold.SewerBoxEnd.CoverLevel;
            sewerBoxEnd.ArrivalLevel = model.SewerManifold.SewerBoxEnd.ArrivalLevel;
            sewerBoxEnd.BottomLevel = model.SewerManifold.SewerBoxEnd.BottomLevel;
            //sewerBoxEnd.Height = sewerBoxEnd.CoverLevel - sewerBoxEnd.BottomLevel;
            sewerBoxEnd.Height = sewerBoxEnd.CoverLevel - sewerBoxEnd.BottomLevel;

            manifold.LengthBetweenHAxles = model.SewerManifold.LengthBetweenHAxles;
            manifold.LengthBetweenIAxles = Math.Pow((Math.Pow(sewerBoxStart.BottomLevel - sewerBoxEnd.BottomLevel, 2) + Math.Pow(manifold.LengthBetweenHAxles, 2)), 0.5);
            manifold.DitchHeight = Math.Round((sewerBoxStart.CoverLevel - sewerBoxStart.BottomLevel + sewerBoxEnd.CoverLevel - sewerBoxEnd.ArrivalLevel) / 2.0, 2);
            manifold.DitchLevelPercent = Math.Round((sewerBoxStart.BottomLevel - sewerBoxEnd.ArrivalLevel) * 1000.0 / manifold.LengthBetweenHAxles, 2);

            manifold.LengthOfPipelineInstalled = manifold.LengthBetweenIAxles - ((sewerBoxStart.Diameter + sewerBoxEnd.Diameter) / 2.0);
            manifold.LengthOfDigging = manifold.LengthOfPipelineInstalled - ((sewerBoxStart.Thickness + sewerBoxEnd.Thickness) / 2.0);

            await _context.DischargeManifolds.AddAsync(dischargeManifold);
            await _context.SaveChangesAsync();
            //await _context.SewerManifolds.AddAsync(manifold);
            //await _context.SaveChangesAsync();
            //await _context.SewerBoxes.AddAsync(sewerBoxStart);
            //await _context.SaveChangesAsync();
            //await _context.SewerBoxes.AddAsync(sewerBoxEnd);
            //await _context.SaveChangesAsync();
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                dischargeManifold.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_{dischargeManifold.Id}");

                await _context.SaveChangesAsync();
            }
            if (model.VideoMirror != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                dischargeManifold.MirrorTestVideoUrl = await storage2.UploadFile(model.VideoMirror.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.VideoMirror.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_video_espejo_{dischargeManifold.Id}");

                await _context.SaveChangesAsync();

            }
            if (model.VideoMonkeyBall != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                dischargeManifold.MonkeyBallTestVideoUrl = await storage2.UploadFile(model.VideoMonkeyBall.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.VideoMonkeyBall.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_video_bola_{dischargeManifold.Id}");

                await _context.SaveChangesAsync();

            }
            if (model.VideoZoom != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                dischargeManifold.ZoomTestVideoUrl = await storage2.UploadFile(model.VideoZoom.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.VideoZoom.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_video_zoom_{dischargeManifold.Id}");

                await _context.SaveChangesAsync();

            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, DischargeManifoldViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dischargeManifold = await _context.DischargeManifolds
                .FirstOrDefaultAsync(x => x.Id == id);
            //--SWFor47
            var sewerManifoldFor47 = await _context.SewerManifoldFor47s
               .FirstOrDefaultAsync(x => x.SewerManifoldId == model.SewerManifoldId);
            if (sewerManifoldFor47 != null)
                sewerManifoldFor47.For01ProtocolNumber = model.ProtocolNumber;
            //--SWFor29
            var sewerManifoldFor29 = await _context.SewerManifoldFor29s
               .FirstOrDefaultAsync(x => x.SewerManifoldId == model.SewerManifoldId);
            if (sewerManifoldFor29 != null)
                sewerManifoldFor29.For01ProtocolNumber = model.ProtocolNumber;
            //--SWFor37A
            var sewerManifoldFor37A = await _context.SewerManifoldFor37As
                .FirstOrDefaultAsync(x => x.SewerManifoldId == model.SewerManifoldId);
            if (sewerManifoldFor37A != null)
            {
                sewerManifoldFor37A.For01ProtocolNumber = model.ProtocolNumber;
                sewerManifoldFor37A.FirstPipeBatch = model.PipeBatch;
                sewerManifoldFor37A.SecondPipeBatch = model.SecondPipeBatch;
                sewerManifoldFor37A.ThridPipeBatch = model.ThirdPipeBatch;
                sewerManifoldFor37A.ForthPipeBatch = model.FourthPipeBatch;
            }

            dischargeManifold.ProtocolNumber = model.ProtocolNumber;
            dischargeManifold.SewerManifoldId = model.SewerManifoldId;
            dischargeManifold.EquipmentCertificateId = model.EquipmentCertificateId;
            dischargeManifold.EquipmentCertificate2Id = model.EquipmentCertificate2Id;
            dischargeManifold.Producer = model.Producer;
            dischargeManifold.PipeBatch = model.PipeBatch;
            dischargeManifold.SecondPipeBatch = model.SecondPipeBatch;
            dischargeManifold.ThridPipeBatch = model.ThirdPipeBatch;
            dischargeManifold.ForthPipeBatch = model.FourthPipeBatch;
            dischargeManifold.Leveling = model.Leveling.ToDateTime();

            dischargeManifold.OpenZTest = string.IsNullOrEmpty(model.OpenZTest)
  ? (DateTime?)null : model.OpenZTest.ToDateTime();

            dischargeManifold.ClosedZTest = string.IsNullOrEmpty(model.ClosedZTest)
    ? (DateTime?)null : model.ClosedZTest.ToDateTime();

            dischargeManifold.MirrorTest = string.IsNullOrEmpty(model.MirrorTest)
    ? (DateTime?)null : model.MirrorTest.ToDateTime();

            dischargeManifold.BallTest = string.IsNullOrEmpty(model.BallTest)
                ? (DateTime?)null : model.BallTest.ToDateTime();

            dischargeManifold.BookPZO = model.BookPZO;
            dischargeManifold.SeatPZF = model.SeatPZF;

            dischargeManifold.BookPZF = model.BookPZF;
            dischargeManifold.SeatPZC = model.SeatPZC;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (dischargeManifold.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                dischargeManifold.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_{dischargeManifold.Id}");
            }

            if (model.VideoMirror != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (dischargeManifold.MirrorTestVideoUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.MirrorTestVideoUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                dischargeManifold.MirrorTestVideoUrl = await storage.UploadFile(model.VideoMirror.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.VideoMirror.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_video_espejo_{dischargeManifold.Id}");
            }
            if (model.VideoMonkeyBall != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (dischargeManifold.MonkeyBallTestVideoUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.MonkeyBallTestVideoUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                dischargeManifold.MonkeyBallTestVideoUrl = await storage.UploadFile(model.VideoMonkeyBall.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.VideoMonkeyBall.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_video_bola_{dischargeManifold.Id}");
            }
            if (model.VideoZoom != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (dischargeManifold.ZoomTestVideoUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.ZoomTestVideoUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                dischargeManifold.ZoomTestVideoUrl = await storage.UploadFile(model.VideoZoom.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.VideoZoom.FileName),
                    ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD,
                    $"dm_video_zoom_{dischargeManifold.Id}");
            }

            //revisar como hacer que la data acepte nulls (copy paste)

            var manifold = await _context.SewerManifolds
                            .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var sewerBoxStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);

            sewerBoxStart.CoverLevel = model.SewerManifold.SewerBoxStart.CoverLevel;
            //sewerBoxStart.ArrivalLevel = model.SewerManifold.SewerBoxStart.ArrivalLevel;
            sewerBoxStart.BottomLevel = model.SewerManifold.SewerBoxStart.BottomLevel;
            sewerBoxStart.Height = sewerBoxStart.CoverLevel - sewerBoxStart.BottomLevel;

            var sewerBoxEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);

            sewerBoxEnd.CoverLevel = model.SewerManifold.SewerBoxEnd.CoverLevel;
            sewerBoxEnd.ArrivalLevel = model.SewerManifold.SewerBoxEnd.ArrivalLevel;
            sewerBoxEnd.BottomLevel = model.SewerManifold.SewerBoxEnd.BottomLevel;
            sewerBoxEnd.Height = sewerBoxEnd.CoverLevel - sewerBoxEnd.BottomLevel;

            manifold.LengthBetweenHAxles = model.SewerManifold.LengthBetweenHAxles;
            //manifold.LengthBetweenIAxles = Math.Pow(Math.Pow(sewerBoxStart.BottomLevel - sewerBoxEnd.BottomLevel, 2) + Math.Pow(manifold.LengthBetweenHAxles, 2), 0.5);
            manifold.LengthBetweenIAxles = Math.Pow((Math.Pow(sewerBoxStart.BottomLevel - sewerBoxEnd.BottomLevel, 2) + Math.Pow(manifold.LengthBetweenHAxles, 2)), 0.5);
            manifold.DitchHeight = Math.Round((sewerBoxStart.CoverLevel - sewerBoxStart.BottomLevel + sewerBoxEnd.CoverLevel - sewerBoxEnd.ArrivalLevel) / 2.0, 2);
            manifold.DitchLevelPercent = Math.Round((sewerBoxStart.BottomLevel - sewerBoxEnd.ArrivalLevel) * 1000.0 / manifold.LengthBetweenHAxles, 2);

            manifold.LengthOfPipelineInstalled = manifold.LengthBetweenIAxles - ((sewerBoxStart.Diameter + sewerBoxEnd.Diameter) / 2.0);
            manifold.LengthOfDigging = manifold.LengthOfPipelineInstalled - ((sewerBoxStart.Thickness + sewerBoxEnd.Thickness) / 2.0);

            //SWFor05

            var for05 = await _context.SewerManifoldFor05s.FirstOrDefaultAsync(x => x.SewerManifoldId == manifold.Id);
            if (for05 != null)
            {
                for05.Filling = Math.Round(manifold.DitchHeight - (manifold.PipelineDiameter / 1000) - 0.30 - 0.20, 2);
                for05.TheoreticalLayer = (int)(for05.Filling / 0.3);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dischargeManifold = await _context.DischargeManifolds
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dischargeManifold == null)
                return BadRequest($"Colector de descarga con Id '{id}' no encontrado.");
            //--SWFor47
            var sewerManifoldFor47 = await _context.SewerManifoldFor47s
                .FirstOrDefaultAsync(x => x.SewerManifoldId == dischargeManifold.SewerManifoldId);
            if (sewerManifoldFor47 != null)
                sewerManifoldFor47.For01ProtocolNumber = string.Empty;
            //--SWFor29
            var sewerManifoldFor29 = await _context.SewerManifoldFor29s
                .FirstOrDefaultAsync(x => x.SewerManifoldId == dischargeManifold.SewerManifoldId);
            if (sewerManifoldFor29 != null)
                sewerManifoldFor29.For01ProtocolNumber = string.Empty;
            //--SWFor37A
            var sewerManifoldFor37A = await _context.SewerManifoldFor37As
                .FirstOrDefaultAsync(x => x.SewerManifoldId == dischargeManifold.SewerManifoldId);
            if (sewerManifoldFor37A != null)
            {
                sewerManifoldFor37A.For01ProtocolNumber = string.Empty;
                sewerManifoldFor37A.FirstPipeBatch = string.Empty;
                sewerManifoldFor37A.SecondPipeBatch = string.Empty;
                sewerManifoldFor37A.ThridPipeBatch = string.Empty;
                sewerManifoldFor37A.ForthPipeBatch = string.Empty;
            }

            if (dischargeManifold.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            if (dischargeManifold.MirrorTestVideoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.MirrorTestVideoUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }
            if (dischargeManifold.MonkeyBallTestVideoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.MonkeyBallTestVideoUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }
            if (dischargeManifold.ZoomTestVideoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DISCHARGE_MANIFOLD}/{dischargeManifold.ZoomTestVideoUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            var manifoldReview = await _context.SewerManifoldReferences
    .Include(x => x.SewerManifoldReview)
    .Include(x => x.SewerManifoldReview.SewerBoxStart)
    .Include(x => x.SewerManifoldReview.SewerBoxEnd)
    .Where(x => x.SewerManifoldExecutionId == dischargeManifold.SewerManifoldId)
    .FirstOrDefaultAsync();



            var manifold = await _context.SewerManifolds
                            .FirstOrDefaultAsync(x => x.Id == dischargeManifold.SewerManifoldId);
            manifold.HasFor01 = false;
            var sewerBoxStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);

            sewerBoxStart.CoverLevel = manifoldReview.SewerManifoldReview.SewerBoxStart.CoverLevel;
            //sewerBoxStart.ArrivalLevel = 0.00;
            sewerBoxStart.BottomLevel = manifoldReview.SewerManifoldReview.SewerBoxStart.BottomLevel;
            sewerBoxStart.Height = manifoldReview.SewerManifoldReview.SewerBoxStart.Height;

            var sewerBoxEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);

            sewerBoxEnd.CoverLevel = manifoldReview.SewerManifoldReview.SewerBoxEnd.CoverLevel;
            sewerBoxEnd.ArrivalLevel = manifoldReview.SewerManifoldReview.SewerBoxEnd.ArrivalLevel;
            sewerBoxEnd.BottomLevel = manifoldReview.SewerManifoldReview.SewerBoxEnd.BottomLevel;
            sewerBoxEnd.Height = manifoldReview.SewerManifoldReview.SewerBoxEnd.Height;

            manifold.LengthBetweenHAxles = manifoldReview.SewerManifoldReview.LengthBetweenHAxles;
            //manifold.LengthBetweenIAxles = Math.Pow(Math.Pow(sewerBoxStart.BottomLevel - sewerBoxEnd.BottomLevel, 2) + Math.Pow(manifold.LengthBetweenHAxles, 2), 0.5);
            manifold.LengthBetweenIAxles = manifoldReview.SewerManifoldReview.LengthBetweenIAxles;
            manifold.DitchHeight = manifoldReview.SewerManifoldReview.DitchHeight;
            manifold.DitchLevelPercent = manifoldReview.SewerManifoldReview.DitchLevelPercent;

            manifold.LengthOfPipelineInstalled = manifoldReview.SewerManifoldReview.LengthOfPipelineInstalled;
            manifold.LengthOfDigging = manifoldReview.SewerManifoldReview.LengthOfDigging;

            _context.DischargeManifolds.Remove(dischargeManifold);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
