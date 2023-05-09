using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor37AViewModels;
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
    [Route("calidad/for37A-colector-descarga")]
    public class SewerManifoldFor37AController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor37AController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor37AController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.SewerManifoldFor37As.Where(x => x.ProjectId == pId);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerManifold.ProductionDailyPart.SewerGroupId == sewerGroupId);

            var for37As = await query
               .Select(x => new SewerManifoldFor37AViewModel
               {
                   Id = x.Id,
                   For01ProtocolNumber = x.For01ProtocolNumber,
                   SewerManifold = new SewerManifoldViewModel
                   {
                       ProductionDailyPart = new PdpViewModel
                       {
                           SewerGroup = new SewerGroupViewModel
                           {
                               Code = x.SewerManifold.ProductionDailyPart.SewerGroup.Code,
                           }
                       },
                       Address = x.SewerManifold.Address,
                       Name = x.SewerManifold.Name,
                       LengthOfDigging = x.SewerManifold.LengthOfDigging,
                       LengthOfPipeInstalled = x.SewerManifold.LengthOfPipelineInstalled
                   },
                   HotMeltsNumber = x.HotMeltsNumber.ToString(),
                   ElectrofusionsNumber = x.ElectrofusionsNumber.ToString(),
                   ElectrofusionsPasNumber = x.ElectrofusionsPasNumber.ToString(),
                   StartElectrofusionDate = x.StartElectrofusionDate.ToDateString(),
                   EndElectrofusionDate = x.EndElectrofusionDate.ToDateString(),
                   FirstPipeBatch = x.FirstPipeBatch,
                   SecondPipeBatch = x.SecondPipeBatch,
                   ThridPipeBatch = x.ThridPipeBatch,
                   ForthPipeBatch = x.ForthPipeBatch,
                   FileUrl = x.FileUrl
               }).AsNoTracking().ToListAsync();

            return Ok(for37As);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var for37A = await _context.SewerManifoldFor37As
               .Include(x => x.SewerManifold)
               .Include(x => x.SewerManifold.ProductionDailyPart)
               .Where(x => x.Id == id)
               .Select(x => new SewerManifoldFor37AViewModel
               {
                   Id = x.Id,
                   For01ProtocolNumber = x.For01ProtocolNumber,
                   SewerManifold = new SewerManifoldViewModel
                   {
                       ProductionDailyPart = new PdpViewModel
                       {
                           SewerGroup = new SewerGroupViewModel
                           {
                               Code = x.SewerManifold.ProductionDailyPart.SewerGroup.Code,
                           }
                       },
                       Address = x.SewerManifold.Address,
                       Name = x.SewerManifold.Name,
                       LengthOfDigging = x.SewerManifold.LengthOfDigging,
                       LengthOfPipeInstalled = x.SewerManifold.LengthOfPipelineInstalled
                   },
                   HotMeltsNumber = x.HotMeltsNumber.ToString(),
                   ElectrofusionsNumber = x.ElectrofusionsNumber.ToString(),
                   ElectrofusionsPasNumber = x.ElectrofusionsPasNumber.ToString(),
                   StartElectrofusionDate = x.StartElectrofusionDate.ToDateString(),
                   EndElectrofusionDate = x.EndElectrofusionDate.ToDateString(),
                   FirstPipeBatch = x.FirstPipeBatch,
                   SecondPipeBatch = x.SecondPipeBatch,
                   ThridPipeBatch = x.ThridPipeBatch,
                   ForthPipeBatch = x.ForthPipeBatch,
                   FileUrl = x.FileUrl
               }).FirstOrDefaultAsync();

            return Ok(for37A);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor37AViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pId = GetProjectId();

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var for01s = _context.DischargeManifolds.Where(x => x.ProjectId == pId)
                .FirstOrDefault(x => x.SewerManifoldId == model.SewerManifoldId);

            var for01ProtocolNumber = string.Empty;
            var for01FirstPipeBatch = string.Empty;
            var for01SecondPipeBatch = string.Empty;
            var for01ThridPipeBatch = string.Empty;
            var for01ForthPipeBatch = string.Empty;

            if (for01s != null)
            {
                for01ProtocolNumber = for01s.ProtocolNumber;
                for01FirstPipeBatch = for01s.PipeBatch;
                for01SecondPipeBatch = for01s.SecondPipeBatch;
                for01ThridPipeBatch = for01s.ThridPipeBatch;
                for01ForthPipeBatch = for01s.ForthPipeBatch;
            }
                

            var folding = await _context.FoldingFor37As.ToListAsync();

            var for37A = new SewerManifoldFor37A
            {
                ProjectId = pId,
                SewerManifoldId = model.SewerManifoldId,
                For01ProtocolNumber = for01ProtocolNumber,
                FirstPipeBatch = for01FirstPipeBatch,
                SecondPipeBatch = for01SecondPipeBatch,
                ThridPipeBatch = for01ThridPipeBatch,
                ForthPipeBatch = for01ForthPipeBatch
            };

            manifold.HasFor37A = true;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for37A.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FOR_37A,
                    $"sewer_manifold_for37A_lotes_{for37A.FirstPipeBatch}_{for37A.SecondPipeBatch}_{for37A.ThridPipeBatch}_{for37A.ForthPipeBatch}");
            }

            await _context.SewerManifoldFor37As.AddAsync(for37A);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerManifoldFor37AViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var pId = GetProjectId();

            var for37A = await _context.SewerManifoldFor37As.Where(x => x.ProjectId == pId)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            for37A.FirstPipeBatch = model.FirstPipeBatch;
            for37A.SecondPipeBatch = model.SecondPipeBatch;
            for37A.ThridPipeBatch = model.ThridPipeBatch;
            for37A.ForthPipeBatch = model.ForthPipeBatch;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (for37A.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_37A}/{for37A.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                for37A.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                     ConstantHelpers.Storage.Containers.QUALITY,
                     System.IO.Path.GetExtension(model.File.FileName),
                     ConstantHelpers.Storage.Blobs.FOR_37A,
                     $"sewer_manifold_for37A_lotes_{for37A.FirstPipeBatch}_{for37A.SecondPipeBatch}_{for37A.ThridPipeBatch}_{for37A.ForthPipeBatch}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for37A = await _context.SewerManifoldFor37As
                .FirstOrDefaultAsync(x => x.Id == id);

            if (for37A == null)
            {
                return BadRequest($"Sewer Manifold For47 con Id '{id}' no se halló.");
            }

            var foldings = await _context.FoldingFor37As.
                Where(x => x.SewerManifoldFor37AId == id).ToListAsync();

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == for37A.SewerManifoldId);

            manifold.HasFor37A = false;

            var cant = foldings.Count();

            for (int i = 0; i < cant; i++)
            {
                _context.FoldingFor37As.Remove(foldings[i]);
            }
            await _context.SaveChangesAsync();

            if (for37A.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_37A}/{for37A.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.SewerManifoldFor37As.Remove(for37A);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("total-termofusiones")]
        public async Task<IActionResult> GetTotalTermofusiones(Guid? projectId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.FoldingFor37As.Include(x=>x.SewerManifoldFor37A)
                .Where(x => x.SewerManifoldFor37A.ProjectId == pId && x.WeldingType == 1);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerManifoldFor37A.SewerManifold.ProductionDailyPart.SewerGroupId == sewerGroupId);

            var data = await query.ToListAsync();

            var Suma = data.Count();

            return Ok(Suma);
        }

        [HttpGet("total-electrofusiones-tub")]
        public async Task<IActionResult> GetTotalElectrofusionesTub(Guid? projectId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.FoldingFor37As.Include(x => x.SewerManifoldFor37A)
                .Where(x => x.SewerManifoldFor37A.ProjectId == pId && x.WeldingType == 2);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerManifoldFor37A.SewerManifold.ProductionDailyPart.SewerGroupId == sewerGroupId);

            var data = await query.ToListAsync();

            var Suma = data.Count();

            return Ok(Suma);
        }

        [HttpGet("total-electrofusiones-pas")]
        public async Task<IActionResult> GetTotalElectrofusionesPas(Guid? projectId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var query = _context.FoldingFor37As.Include(x => x.SewerManifoldFor37A)
                .Where(x => x.SewerManifoldFor37A.ProjectId == pId && x.WeldingType == 3);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerManifoldFor37A.SewerManifold.ProductionDailyPart.SewerGroupId == sewerGroupId);

            var data = await query.ToListAsync();

            var Suma = data.Count();

            return Ok(Suma);
        }


    }
}
