using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor47ViewModels;
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

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/for47-colector-descarga")]
    public class SewerManifoldFor47Controller : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor47Controller(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor47Controller> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {
            var pId = GetProjectId();

            var for01s = await _context.DischargeManifolds.Where(x => x.ProjectId == pId).ToListAsync();

            var for47s = await _context.SewerManifoldFor47s
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .Include(x => x.SewerManifold.ProductionDailyPart.SewerGroup)
                .Include(x => x.SewerManifold.SewerBoxStart)
                .Include(x => x.SewerManifold.SewerBoxEnd)
                .Where(x => x.ProjectId == pId)
                .Select(x => new SewerManifoldFor47ViewModel
                {
                    Id = x.Id,
                    SewerManifoldId = x.SewerManifoldId,
                    SewerManifold = new SewerManifoldViewModel
                    {
                        ProductionDailyPart = new PdpViewModel
                        {
                            SewerGroup = new SewerGroupViewModel
                            {
                                Code = x.SewerManifold.ProductionDailyPart.SewerGroup.Code
                            }
                        },
                        Address = x.SewerManifold.Address,
                        SewerBoxStart = new SewerBoxViewModel
                        {
                            Code = x.SewerManifold.SewerBoxStart.Code,
                            TerrainType = x.SewerManifold.SewerBoxStart.TerrainType
                        },
                        SewerBoxEnd = new SewerBoxViewModel
                        {
                            Code = x.SewerManifold.SewerBoxEnd.Code,
                            TerrainType = x.SewerManifold.SewerBoxEnd.TerrainType
                        },
                        Name = x.SewerManifold.Name,
                        TerrainType = x.SewerManifold.TerrainType,
                        LengthOfDigging = x.SewerManifold.LengthOfDigging,
                    },
                    LengthOfDiggingN = x.LengthOfDiggingN.ToString(),
                    LengthOfDiggingSR = x.LengthOfDiggingSR.ToString(),
                    LengthOfDiggingR = x.LengthOfDiggingR.ToString(),
                    WorkBookRegistryDate = x.WorkBookRegistryDate.ToDateString(),
                    WorkBookNumber = x.WorkBookNumber.ToString(),
                    WorkBookSeat = x.WorkBookSeat.ToString(),
                    FileUrl = x.FileUrl,
                    For01ProtocolNumber = x.For01ProtocolNumber,
                    BZiRealTerrainType = x.BZiRealTerrainType,
                    BZjRealTerrainType = x.BZjRealTerrainType
                }).ToListAsync();

            

            return Ok(for47s);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var for47 = await _context.SewerManifoldFor47s
                .Include(x => x.Project)
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .Include(x => x.SewerManifold.ProductionDailyPart.SewerGroup)
                .Include(x => x.SewerManifold.SewerBoxStart)
                .Include(x => x.SewerManifold.SewerBoxEnd)
                .Where(x => x.Id == id)
                .Select(x => new SewerManifoldFor47ViewModel
                {
                    Id = x.Id,
                    SewerManifold =  new SewerManifoldViewModel
                    {
                        ProductionDailyPart = new PdpViewModel
                        {
                            SewerGroup = new SewerGroupViewModel
                            {
                                Code = x.SewerManifold.ProductionDailyPart.SewerGroup.Code
                            }
                        },
                        Address = x.SewerManifold.Address,
                        SewerBoxStart = new SewerBoxViewModel
                        {
                            Code = x.SewerManifold.SewerBoxStart.Code,
                            TerrainType = x.SewerManifold.SewerBoxStart.TerrainType
                        },
                        SewerBoxEnd = new SewerBoxViewModel
                        {
                            Code = x.SewerManifold.SewerBoxEnd.Code,
                            TerrainType = x.SewerManifold.SewerBoxEnd.TerrainType 
                        },
                        Name = x.SewerManifold.Name,
                        TerrainType = x.SewerManifold.TerrainType,
                        LengthOfDigging = x.SewerManifold.LengthOfDigging,
                    },
                    LengthOfDiggingN = x.LengthOfDiggingN.ToString(),
                    LengthOfDiggingSR = x.LengthOfDiggingSR.ToString(),
                    LengthOfDiggingR = x.LengthOfDiggingR.ToString(),
                    WorkBookRegistryDate = x.WorkBookRegistryDate.ToDateString(),
                    WorkBookNumber = x.WorkBookNumber.ToString(),
                    WorkBookSeat = x.WorkBookSeat.ToString(),
                    FileUrl = x.FileUrl,
                    For01ProtocolNumber = x.For01ProtocolNumber,
                    BZiRealTerrainType = x.BZiRealTerrainType,
                    BZjRealTerrainType = x.BZjRealTerrainType
                }).FirstOrDefaultAsync();

            return Ok(for47);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor47ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId(); 
            
            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var manifoldReview = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Name == manifold.Name && x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.REVIEW);

            var sbStartReview = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifoldReview.SewerBoxStartId);
            var sbEndReview = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifoldReview.SewerBoxEndId);

            var for01s = _context.DischargeManifolds.Where(x => x.ProjectId == pId)
                .FirstOrDefault(x => x.SewerManifoldId == model.SewerManifoldId);

            var aux = string.Empty;

            if(for01s != null)
                aux = for01s.ProtocolNumber;

            if (model.LengthOfDiggingN == null) { return BadRequest("No se ha ingresado Tramo Normal"); }
            if (model.LengthOfDiggingSR == null) { return BadRequest("No se ha ingresado Tramo SemiRocoso"); }
            if (model.LengthOfDiggingR == null) { return BadRequest("No se ha ingresado Tramo Rocoso"); }
            if (model.WorkBookRegistryDate == null || model.WorkBookRegistryDate == "") { return BadRequest("No se ha ingresado la fecha"); }

            var for47 = new SewerManifoldFor47
            {
                ProjectId = GetProjectId(),
                SewerManifoldId = model.SewerManifoldId,
                LengthOfDiggingN = model.LengthOfDiggingN.ToDoubleString(),
                LengthOfDiggingR = model.LengthOfDiggingR.ToDoubleString(),
                LengthOfDiggingSR = model.LengthOfDiggingSR.ToDoubleString(),
                WorkBookRegistryDate = model.WorkBookRegistryDate.ToDateTime(),
                WorkBookNumber = int.TryParse(model.WorkBookNumber, out int wbn) ? wbn : 0,
                WorkBookSeat = int.TryParse(model.WorkBookSeat, out int wbs) ? wbs : 0,
                For01ProtocolNumber = aux,
                BZiRealTerrainType = sbStartReview.TerrainType,
                BZjRealTerrainType = sbEndReview.TerrainType
            };

            manifold.HasFor47 = true;

            var sbStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);
            sbStart.TerrainType = model.SewerManifold.SewerBoxStart.TerrainType;

            var sbEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);
            sbEnd.TerrainType = model.SewerManifold.SewerBoxEnd.TerrainType;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for47.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FOR_47,
                    $"sewer_manifold_for47_tramo{manifold.Id}-{sbStart.Code}-{sbEnd.Code}"); 
            }

            await _context.SewerManifoldFor47s.AddAsync(for47);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerManifoldFor47ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            var for01s = _context.DischargeManifolds.Where(x => x.ProjectId == pId)
                .FirstOrDefault(x => x.SewerManifoldId == model.SewerManifoldId);

            var aux = "0";

            if (for01s != null)
                aux = for01s.ProtocolNumber;

            if (model.WorkBookRegistryDate == null || model.WorkBookRegistryDate == "" )
            {
                return BadRequest("No se ha ingresado la fecha");
            }

            var for47 = await _context.SewerManifoldFor47s
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            for47.SewerManifoldId = model.SewerManifoldId;
            for47.LengthOfDiggingN = model.LengthOfDiggingN.ToDoubleString();
            for47.LengthOfDiggingR = model.LengthOfDiggingR.ToDoubleString();
            for47.LengthOfDiggingSR = model.LengthOfDiggingSR.ToDoubleString();
            for47.WorkBookNumber = int.TryParse(model.WorkBookNumber, out int wbn) ? wbn : 0;
            for47.WorkBookSeat = int.TryParse(model.WorkBookSeat, out int wbs) ? wbs : 0;
            for47.WorkBookRegistryDate = model.WorkBookRegistryDate.ToDateTime();
            for47.For01ProtocolNumber = aux;

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var sbStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);
            sbStart.TerrainType = model.SewerManifold.SewerBoxStart.TerrainType;

            var sbEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);
            sbEnd.TerrainType = model.SewerManifold.SewerBoxEnd.TerrainType;

            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (for47.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_47}/{for47.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                for47.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FOR_47,
                    $"sewer_manifold_for47_tramo{manifold.Id}-{sbStart.Code}-{sbEnd.Code}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for47 = await _context.SewerManifoldFor47s
                .FirstOrDefaultAsync(x => x.Id == id);

            if(for47 == null)
            {
                return BadRequest($"Sewer Manifold For47 con Id '{id}' no se halló.");
            }

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == for47.SewerManifoldId);
            var sbStart = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxStartId);
            var sbEnd = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifold.SewerBoxEndId);

            manifold.HasFor47 = false;

            var manifoldReview = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Name == manifold.Name && x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.REVIEW);
            var sbStartReview = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifoldReview.SewerBoxStartId);
            var sbEndReview = await _context.SewerBoxes
                .FirstOrDefaultAsync(x => x.Id == manifoldReview.SewerBoxEndId);

            sbStart.TerrainType = sbStartReview.TerrainType;
            sbEnd.TerrainType = sbEndReview.TerrainType;

            if (for47.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_47}/{for47.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.SewerManifoldFor47s.Remove(for47);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
