using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor29ViewModels;
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
    [Route("calidad/for29-colector-descarga")]
    public class SewerManifoldFor29Controller : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor29Controller(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor29Controller> logger): base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? porjectId = null)
        {
            var pId = GetProjectId();

            var for29s = await _context.SewerManifoldFor29s
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .Include(x => x.SewerManifold.ProductionDailyPart.SewerGroup)
                .Where(x => x.ProjectId == pId)
                .Select(x => new SewerManifoldFor29ViewModel
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
                        Name = x.SewerManifold.Name,
                        LengthOfDigging = x.SewerManifold.LengthOfDigging
                    },
                    For01ProtocolNumber = x.For01ProtocolNumber,
                    AsphaltDate = x.AsphaltDate.ToDateString(),
                    AsphaltType = x.AsphaltType,
                    Thickness = x.Thickness,
                    AsphaltArea = x.AsphaltArea.ToString(),
                    AreaToValue = x.AreaToValue.ToString(),
                    FileUrl = x.FileUrl,
                    Pavement2InReview = x.Pavement2InReview.ToString(),
                    Pavement3InReview = x.Pavement3InReview.ToString(),
                    Pavement3InMixedReview = x.Pavement3InMixedReview.ToString()
                }).ToListAsync();

            return Ok(for29s);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var pId = GetProjectId();

            var for29 = await _context.SewerManifoldFor29s
                .Include(x => x.SewerManifold)
                .Include(x => x.SewerManifold.ProductionDailyPart)
                .Include(x => x.SewerManifold.ProductionDailyPart.SewerGroup)
                .Where(x => x.Id == id)
                .Select(x => new SewerManifoldFor29ViewModel
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
                        Name = x.SewerManifold.Name,
                        LengthOfDigging = x.SewerManifold.LengthOfDigging
                    },
                    For01ProtocolNumber = x.For01ProtocolNumber,
                    AsphaltDate = x.AsphaltDate.ToDateString(),
                    AsphaltType = x.AsphaltType,
                    Thickness = x.Thickness,
                    AsphaltArea = x.AsphaltArea.ToString(),
                    AreaToValue = x.AreaToValue.ToString(),
                    FileUrl = x.FileUrl,
                    Pavement2InReview = x.Pavement2InReview.ToString(),
                    Pavement3InReview = x.Pavement3InReview.ToString(),
                    Pavement3InMixedReview = x.Pavement3InMixedReview.ToString()
                }).FirstOrDefaultAsync();

            return Ok(for29);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor29ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            var for01s = _context.DischargeManifolds.Where(x => x.ProjectId == pId)
                .FirstOrDefault(x => x.SewerManifoldId == model.SewerManifoldId);

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            var manifoldReview = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Name == manifold.Name && x.ProcessType == ConstantHelpers.Sewer.Manifolds.Process.REVIEW);

            manifold.HasFor29 = true;

            var aux = string.Empty;

            if (for01s != null)
                aux = for01s.ProtocolNumber;

            if (model.AsphaltDate == null || model.AsphaltDate == "") return BadRequest("No se ha ingresado la fecha");

            var for29 = new SewerManifoldFor29
            {
                ProjectId = pId,
                For01ProtocolNumber = aux,
                SewerManifoldId = model.SewerManifoldId,
                AsphaltDate = model.AsphaltDate.ToDateTime(),
                Thickness = model.Thickness,
                AsphaltType = model.AsphaltType,
                AsphaltArea = model.AsphaltArea.ToDoubleString(),
                AreaToValue = model.AreaToValue.ToDoubleString(),
                Pavement2InReview = manifoldReview.Pavement2In,
                Pavement3InReview = manifoldReview.Pavement3In,
                Pavement3InMixedReview = manifoldReview.Pavement3InMixed
            };

            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for29.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FOR_29,
                    $"sewer_manifold_for29_tramo{manifold.Name}_{model.Id}");
            }

            await _context.SewerManifoldFor29s.AddAsync(for29);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerManifoldFor29ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.AsphaltDate == null || model.AsphaltDate == "") return BadRequest("No se ha ingresado la fecha");

            var for29 = await _context.SewerManifoldFor29s
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldId);

            for29.AsphaltDate = model.AsphaltDate.ToDateTime();
            for29.Thickness = model.Thickness;
            for29.AsphaltType = model.AsphaltType;
            for29.AsphaltArea = model.AsphaltArea.ToDoubleString();
            for29.AreaToValue = model.AreaToValue.ToDoubleString();

            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (for29.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_29}/{for29.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                for29.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FOR_29,
                    $"sewer_manifold_for29_tramo{manifold.Name}_{model.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for29 = await _context.SewerManifoldFor29s
                .FirstOrDefaultAsync(x => x.Id == id);

            var manifold = await _context.SewerManifolds
                .FirstOrDefaultAsync(x => x.Id == for29.SewerManifoldId);

            manifold.HasFor29 = false;

            if (for29 == null)
                return BadRequest($"Sewer Manifold For29 con Id '{id}' no se halló.");

            if (for29.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_29}/{for29.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.SewerManifoldFor29s.Remove(for29);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
