using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.NewSIGProcessViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels;
using IVC.PE.WEB.Areas.LegalTechnicalLibrary.ViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("sistema-de-manejo-integrado/first-part-for24")]
    public class SewerManifoldFor24FirstPartController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public SewerManifoldFor24FirstPartController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<SewerManifoldFor24FirstPartController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {
            var pId = GetProjectId();

            var for24s = await _context.SewerManifoldFor24FirstParts
                .Include(x => x.NewSIGProcess)
                .Include(x => x.SewerGroup)
                .Include(x => x.Provider)
                .Where(x => x.ProjectId == pId)
                .Select(x => new SewerManifoldFor24FirstPartViewModel
                {
                    Id = x.Id,
                    NewSIGProcessId = x.NewSIGProcessId,
                    Date = x.Date.ToDateString(),
                    NewSIGProcess = new NewSIGProcessViewModel
                    {
                        ProcessName = x.NewSIGProcess.ProcessName
                    },
                    ReportUserId = x.ReportUserId,
                    ReportUserName = x.ReportUserName,
                    OriginType = x.OriginType,
                    NCOrigin = x.NCOrigin,
                    SewerGroupId =  x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                          Code = x.SewerGroup.Code
                    },
                    ProviderId = x.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    Client = x.Client,
                    Description = x.Description,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity.ToString(),
                    BrandProvider = x.BrandProvider,
                    CodeReference = x.CodeReference,
                    ExpirationDate = x.ExpirationDate.ToDateString(),
                    ResponsableUserId = x.ResponsableUserId,
                    ResponsableUserName = x.ResponsableUserName,
                    FileUrl = x.FileUrl,
                    VideoUrl = x.VideoUrl,
                    Gallery = x.for24FirstPartGallery.Select(g => new For24FirstPartGalleryViewModel()
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
            var for24 = await _context.SewerManifoldFor24FirstParts
                .Include(x => x.NewSIGProcess)
                .Include(x => x.SewerGroup)
                .Include(x => x.Provider)
                .Where(x => x.Id == id)
                .Select(x => new SewerManifoldFor24FirstPartViewModel
                {
                    Id = x.Id,
                    NewSIGProcessId = x.NewSIGProcessId,
                    Date = x.Date.ToDateString(),
                    NewSIGProcess = new NewSIGProcessViewModel
                    {
                        ProcessName = x.NewSIGProcess.ProcessName
                    },
                    ReportUserId = x.ReportUserId,
                    ReportUserName = x.ReportUserName,
                    OriginType = x.OriginType,
                    NCOrigin = x.NCOrigin,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    ProviderId = x.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        RUC = x.Provider.RUC,
                        BusinessName = x.Provider.BusinessName
                    },
                    Client = x.Client,
                    Description = x.Description,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity.ToString(),
                    BrandProvider = x.BrandProvider,
                    CodeReference = x.CodeReference,
                    ExpirationDate = x.ExpirationDate.ToDateString(),
                    ResponsableUserId = x.ResponsableUserId,
                    ResponsableUserName = x.ResponsableUserName,
                    FileUrl = x.FileUrl,
                    VideoUrl = x.VideoUrl,
                    Gallery = x.for24FirstPartGallery.Select(g => new For24FirstPartGalleryViewModel()
                    {
                        Id = g.Id,
                        Name =  g.Name,
                        URL = g.URL
                    }).ToList()
                }).FirstOrDefaultAsync();

            return Ok(for24);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerManifoldFor24FirstPartViewModel model)
        {
            if (model.ReportUserId == null)
                return BadRequest("no se ha ingresado el Reportado");
            if (model.NCOrigin == 2)
                if (_context.Providers.FirstOrDefault(x=>x.Id == model.ProviderId) == null)
                    return BadRequest("No se ha ingresado el Proveedor");
            if (model.NCOrigin == 3)
                if (await _context.SewerGroups.FirstOrDefaultAsync(x=>x.Id == model.SewerGroupId) == null)
                    return BadRequest("No se ha ingresado la Cuadrilla");
            if (model.Date == null || model.Date == "") 
                return BadRequest("No se ha ingresado la Fecha"); 
            if (model.ResponsableUserId == null)
                return BadRequest("No se ha ingresado el Responsable");

            if (model.OriginType == 1)
            {
                if (model.ExpirationDate == null || model.ExpirationDate == "")
                    return BadRequest("No se ha ingresado la Fecha");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            var usuarios = await _context.Users.ToListAsync();
            var sigProcess = await _context.NewSIGProcesses.FirstOrDefaultAsync(x=>x.Id == model.NewSIGProcessId);

            if (sigProcess == null)
                return BadRequest("No se ha ingresado el Proceso Observado");

            var for24 = new SewerManifoldFor24FirstPart
            {
                ProjectId = pId,
                NewSIGProcessId = model.NewSIGProcessId,
                Date = model.Date.ToDateTime(),
                ReportUserId = model.ReportUserId,
                ReportUserName = usuarios.FirstOrDefault(x => x.Id == model.ReportUserId).FullName,
                OriginType = model.OriginType,
                NCOrigin = model.NCOrigin,
                SewerGroupId = model.SewerGroupId,
                ProviderId = model.ProviderId,
                Client = model.Client,
                Description = model.Description,
                ProductName = model.ProductName,
                Quantity = int.TryParse(model.Quantity, out int wbs) ? wbs : 0,
                BrandProvider = model.BrandProvider,
                CodeReference = model.CodeReference,
                ExpirationDate = model.ExpirationDate.ToDateTime(),
                ResponsableUserId = model.ResponsableUserId,
                ResponsableUserName = usuarios.FirstOrDefault(x => x.Id == model.ResponsableUserId).FullName
            };

            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for24.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                    System.IO.Path.GetExtension(model.File.Name),
                    ConstantHelpers.Storage.Blobs.FOR_24_FIRST_PART,
                    $"consolidated_for47_proceso_{sigProcess.ProcessName}_{model.Date}_{model.ExpirationDate}");
            }
            if (model.Video != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                for24.VideoUrl = await storage.UploadFile(model.Video.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                    System.IO.Path.GetExtension(model.Video.Name),
                    ConstantHelpers.Storage.Blobs.FOR_24_FIRST_PART_VIDEO,
                    $"video_for47_proceso_{sigProcess.ProcessName}_{model.Date}_{model.ExpirationDate}");
            }
            if (model.GalleryFiles != null)
            {
                if (model.GalleryFiles.Count() > 5)
                    return BadRequest("Se ha superado la cantidad de imágenes");
                for24.for24FirstPartGallery = new Collection<For24FirstPartGallery>();
                var storage = new CloudStorageService(_storageCredentials);
                foreach(var file in model.GalleryFiles)
                {
                    var photo = new For24FirstPartGallery()
                    {
                        Name = file.FileName.ToFileNameString(),
                        URL = await storage.UploadFile(file.OpenReadStream(),
                        ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM,
                        System.IO.Path.GetExtension(file.Name),
                        ConstantHelpers.Storage.Blobs.FOR_24_FIRST_PART_GALLERY,
                        $"gallery_for47_nombre_{file.FileName}_{model.Date}_{model.ExpirationDate}")
                    };
                    for24.for24FirstPartGallery.Add(photo);
                }
            }


            await _context.SewerManifoldFor24FirstParts.AddAsync(for24);

            var for11 = new SewerManifoldFor24
            {
                SewerManifoldFor24FirstPartId = for24.Id
            };

            await _context.SewerManifoldFor24s.AddAsync(for11);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var for11 = await _context.SewerManifoldFor24s.FirstOrDefaultAsync(x => x.SewerManifoldFor24FirstPartId == id);

            _context.SewerManifoldFor24s.Remove(for11);

            var for24 = await _context.SewerManifoldFor24FirstParts
                .FirstOrDefaultAsync(x => x.Id == id);

            if(for24 == null)
                return BadRequest($"Sewer Manifold For24 Parte I con Id '{id}' no se halló.");

            if(for24.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_FIRST_PART}/{for24.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
            }

            if (for24.VideoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_FIRST_PART_VIDEO}/{for24.VideoUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
            }

            var gallery = await _context.For24FirstPartGalleries.Where(x => x.SewerManifoldFor24FirstPartId == id).ToListAsync();
            if (gallery != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var photo in gallery)
                {
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FOR_24_FIRST_PART_GALLERY}/{photo.URL.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.INTEGRATED_MANAGEMENT_SYSTEM);
                    _context.For24FirstPartGalleries.Remove(photo);
                }
            }

            _context.SewerManifoldFor24FirstParts.Remove(for24);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
