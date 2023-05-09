using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProviderCatalogViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
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

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/catalogo-de-proveedores")]
    public class ProviderCatalogController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProviderCatalogController(IvcDbContext context,
            ILogger<ProviderCatalogController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? familyId = null, Guid? groupId = null, Guid? specId = null)
        {

            var query = _context.ProviderCatalogs
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Include(x => x.SupplyGroup)
                .Include(x => x.Speciality)
                .Include(x => x.SupplyFamily)
                .Include(x => x.Provider)
                .Select(x => new ProviderCatalogViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyFamily = new SupplyFamilyViewModel
                    {
                        Code = x.SupplyFamily.Code,
                        Name = x.SupplyFamily.Name

                    },
                    SupplyGroupId = x.SupplyGroupId,
                    SupplyGroup = new SupplyGroupViewModel
                    {
                        Code = x.SupplyGroup.Code,
                        Name = x.SupplyGroup.Name
                    },
                    SpecialityId = x.SpecialityId.Value,
                    Speciality = new SpecialityViewModel
                    {
                        Description = x.Speciality.Description
                    },
                    ProviderId = x.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        Tradename = x.Provider.Tradename
                    },

                    FileUrl = x.FileUrl
                })
                .ToListAsync();

            if (familyId.HasValue)
            {
                data = data.Where(x => x.SupplyFamilyId == familyId.Value).ToList();
            }

            if (groupId.HasValue)
            {
                data = data.Where(x => x.SupplyGroupId == groupId.Value).ToList();
            }

            if (specId.HasValue)
                data = data.Where(x => x.SpecialityId == specId.Value).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.ProviderCatalogs

                 .Where(x => x.Id == id)
                 .Select(x => new ProviderCatalogViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     SupplyFamilyId = x.SupplyFamilyId,
 
                     SupplyGroupId = x.SupplyGroupId,
 
                     SpecialityId = x.SpecialityId.Value,
                     ProviderId = x.ProviderId.Value,
                     FileUrl = x.FileUrl
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProviderCatalogViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spec = new ProviderCatalog
            {
                Name = model.Name,
                SupplyFamilyId = model.SupplyFamilyId,
                SupplyGroupId = model.SupplyGroupId,
                SpecialityId = model.SpecialityId,
                ProviderId = model.ProviderId,
            

                //ProviderCatalogDateStr = x.ProviderCatalogDate.Date.ToDateString(),

            };

            await _context.ProviderCatalogs.AddAsync(spec);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                spec.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.CATALOG_PROVIDER,
                    $"catalogo_{spec.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProviderCatalogViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spec = await _context.ProviderCatalogs
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            spec.Name = model.Name;
            spec.SupplyFamilyId = model.SupplyFamilyId;
            spec.SupplyGroupId = model.SupplyGroupId;
            spec.SpecialityId = model.SpecialityId;
            spec.ProviderId = model.ProviderId;






            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (spec.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CATALOG_PROVIDER}/{spec.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                spec.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.CATALOG_PROVIDER,
                    $"catalogo_{spec.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spec = await _context.ProviderCatalogs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (spec == null)
            {
                return BadRequest($"proveedor con Id '{id}' no se halló.");
            }

            if (spec.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CATALOG_PROVIDER}/{spec.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.ProviderCatalogs.Remove(spec);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
