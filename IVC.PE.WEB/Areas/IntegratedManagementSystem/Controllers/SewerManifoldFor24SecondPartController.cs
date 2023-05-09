using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.IntegratedManagementSystem.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.INTEGRATED_MANAGEMENT_SYSTEM)]
    [Route("sistema-de-manejo-integrado/second-part-for24")]
    public class SewerManifoldFor24SecondPartController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor24SecondPartController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor24SecondPartController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var for24s = await _context.SewerManifoldFor24SecondParts
                .Select(x => new SewerManifoldFor24SecondPartViewModel
                {
                    SewerManifoldFor24FirstPartId = x.SewerManifoldFor24FirstPartId,
                    Decision = x.Decision,
                    Other = x.Other,
                    LaborerQuantity = x.LaborerQuantity,
                    LaborerHoursMan = x.LaborerHoursMan.ToString(),
                    LaborerTotalHoursMan = x.LaborerTotalHoursMan.ToString(),
                    OfficialQuantity = x.OfficialQuantity,
                    OfficialHoursMan = x.OfficialHoursMan.ToString(),
                    OfficialTotalHoursMan = x.OfficialTotalHoursMan.ToString(),
                    OperatorQuantity = x.OperatorQuantity,
                    OperatorHoursMan = x.OperatorHoursMan.ToString(),
                    OperatorTotalHoursMan = x.OperatorTotalHoursMan.ToString(),
                    FileUrl = x.FileUrl,
                    VideoUrl = x.VideoUrl,
                    Gallery = x.for24SecondPartGallery.Select(g => new For24SecondPartGalleryViewModel
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList()
                }).ToListAsync();

            return Ok(for24s);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var for24 = await _context.SewerManifoldFor24SecondParts
                .Where(x => x.Id == id)
                .Select(x => new SewerManifoldFor24SecondPartViewModel
                {
                    SewerManifoldFor24FirstPartId = x.SewerManifoldFor24FirstPartId,
                    Decision = x.Decision,
                    Other = x.Other,
                    LaborerQuantity = x.LaborerQuantity,
                    LaborerHoursMan = x.LaborerHoursMan.ToString(),
                    LaborerTotalHoursMan = x.LaborerTotalHoursMan.ToString(),
                    OfficialQuantity = x.OfficialQuantity,
                    OfficialHoursMan = x.OfficialHoursMan.ToString(),
                    OfficialTotalHoursMan = x.OfficialTotalHoursMan.ToString(),
                    OperatorQuantity = x.OperatorQuantity,
                    OperatorHoursMan = x.OperatorHoursMan.ToString(),
                    OperatorTotalHoursMan = x.OperatorTotalHoursMan.ToString(),
                    FileUrl = x.FileUrl,
                    VideoUrl = x.VideoUrl,
                    Gallery = x.for24SecondPartGallery.Select(g => new For24SecondPartGalleryViewModel
                    {
                        Id = g.Id,
                        Name = g.Name,
                        URL = g.URL
                    }).ToList()
                }).FirstOrDefaultAsync();

            return Ok(for24);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor24SecondPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var for24 = new SewerManifoldFor24SecondPart
            {
                SewerManifoldFor24FirstPartId = model.SewerManifoldFor24FirstPartId,
                Decision = model.Decision,
                Other = model.Other,
                LaborerQuantity = model.LaborerQuantity,
                LaborerHoursMan = model.LaborerHoursMan.ToDoubleString(),
                LaborerTotalHoursMan = model.LaborerTotalHoursMan.ToDoubleString(),
                OfficialQuantity = model.OfficialQuantity,
                OfficialHoursMan = model.OfficialHoursMan.ToDoubleString(),
                OfficialTotalHoursMan = model.OfficialTotalHoursMan.ToDoubleString(),
                OperatorQuantity = model.OperatorQuantity,
                OperatorHoursMan = model.OperatorHoursMan.ToDoubleString(),
                OperatorTotalHoursMan = model.OperatorTotalHoursMan.ToDoubleString()
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for24.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                    System.IO.Path.GetExtension(model.File.Name),
                    ConstantHelpers.Storage.Blobs.FOR_24_SECOND_PART,
                    $"consolidated_for47_segunda_parte_decision_{model.Decision}_{model.File.FileName}");
            }
            if (model.Video != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for24.VideoUrl = await storage.UploadFile(model.Video.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                    System.IO.Path.GetExtension(model.Video.Name),
                    ConstantHelpers.Storage.Blobs.FOR_24_SECOND_PART_VIDEO,
                    $"video_for47_segunda_parte_decision_{model.Decision}_{model.File.FileName}");
            }
            if (model.GalleryFiles != null)
            {
                if (model.GalleryFiles.Count() > 5)
                    return BadRequest("Se ha superado la cantidad de imágenes");
                for24.for24SecondPartGallery = new Collection<For24SecondPartGallery>();
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var file in model.GalleryFiles)
                {
                    var photo = new For24SecondPartGallery()
                    {
                        Name = file.FileName.ToFileNameString(),
                        URL = await storage.UploadFile(file.OpenReadStream(),
                        ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                        System.IO.Path.GetExtension(file.Name),
                        ConstantHelpers.Storage.Blobs.FOR_24_SECOND_PART_GALLERY,
                        $"gallery_for47_segunda_parte_decision_{model.Decision}_{file.FileName}")
                    };
                    for24.for24SecondPartGallery.Add(photo);
                }
            }


            await _context.SewerManifoldFor24SecondParts.AddAsync(for24);

            var for11 = await _context.SewerManifoldFor24s.FirstOrDefaultAsync(x => x.SewerManifoldFor24FirstPartId == model.SewerManifoldFor24FirstPartId);
            for11.SewerManifoldFor24SecondPartId = for24.Id;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for11 = await _context.SewerManifoldFor24s.FirstOrDefaultAsync(x => x.SewerManifoldFor24SecondPartId == id);

            for11.SewerManifoldFor24SecondPartId = null;

            var actions = await _context.For24SecondPartActions.Where(x => x.SewerManifoldFor24SecondPartId == id).ToListAsync();
            var countA = actions.Count;
            for(int i = 0; i< countA; i++)
            {
                _context.For24SecondPartActions.Remove(actions[i]);
            }

            var equipments = await _context.For24SecondPartEquipments.Where(x => x.SewerManifoldFor24SecondPartId == id).ToListAsync();
            var countE = equipments.Count;
            for (int i = 0; i < countE; i++)
            {
                _context.For24SecondPartEquipments.Remove(equipments[i]);
            }


            var for24 = await _context.SewerManifoldFor24SecondParts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (for24 == null)
                return BadRequest($"Sewer Manifold For24 Parte II con Id '{id}' no se halló.");

            if (for24.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_SECOND_PART}/{for24.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
            }

            if (for24.VideoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_SECOND_PART_VIDEO}/{for24.VideoUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
            }

            var gallery = await _context.For24SecondPartGalleries.Where(x => x.SewerManifoldFor24SecondPartId == id).ToListAsync();
            if (gallery != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var photo in gallery)
                {
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_SECOND_PART_GALLERY}/{photo.URL.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
                    _context.For24SecondPartGalleries.Remove(photo);
                }
            }

            _context.SewerManifoldFor24SecondParts.Remove(for24);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
