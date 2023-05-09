using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.LegalTechnicalLibrary;
using IVC.PE.WEB.Areas.LegalTechnicalLibrary.ViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.LegalTechnicalLibrary.Controllers.TechnicalLibrary
{
    [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LEGAL_TECHNICAL_LIBRARY)]
    [Route("libreria-tecnica/catalogo-proveedores")]
    public class SupplierCatalogController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public SupplierCatalogController(IvcDbContext context,
            ILogger<SupplierCatalogController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar-proveedores")]
        public async Task<IActionResult> GetSupplier()
        {
            var query = _context.Suppliers
                .AsNoTracking()
                .AsQueryable();

            //query = query.Where(x => x.Id.Equals(Id)).OrderBy(x => x.BusinessName);

            var data = await query
                .Select(x => new SupplierViewModel
                {
                    Id = x.Id,
                    RUC = x.RUC,
                    BusinessName = x.BusinessName,
                    FileCount = x.FileCount
                }).ToListAsync();

            data.OrderBy(x => x.BusinessName);

            return PartialView("_Results", data);
        }

        [Route("{id}")]
        public IActionResult GetCatalog(Guid id)
        {
            ViewBag.SupplierGuid = id;
            return View();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear-proveedor")]
        public async Task<IActionResult> CreateSupplier(SupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier = new Supplier
            {
                RUC = model.RUC,
                BusinessName = model.BusinessName,
                FileCount = 0
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string supplierGuid)
        {
            Guid guid = new Guid();

            if (!String.IsNullOrEmpty(supplierGuid) && !Guid.TryParse(supplierGuid, out guid))
            {
                return BadRequest(supplierGuid);
            }

            var query = _context.TechnicalLibraryFiles
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.SUPPLIER_CATALOG) 
                                    && x.SupplierId.Value.Equals(guid));

            var data = await query
                .OrderBy(x => x.Title)
                .Select(x => new SupplierCatalogViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(data);
        }
        
        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear-catalogo")]
        public async Task<IActionResult> CreateCatalog(SupplierCatalogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var technicalLibraryFile = new TechnicalLibraryFile
            {
                Title = model.Title,
                FileType = model.FileType,
                SupplierId = model.SupplierGuid
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                technicalLibraryFile.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.SUPPLIER_CATALOG, technicalLibraryFile.Title);
            }

            await _context.TechnicalLibraryFiles.AddAsync(technicalLibraryFile);
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpGet("catalogo/{id}")]
        public async Task<IActionResult> GetSupplierCatalog(Guid id)
        {
            var model = await _context.TechnicalLibraryFiles
                .Where(x => x.Id.Equals(id))
                .Select(x => new SupplierCatalogViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPut("catalogo-editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SupplierCatalogViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplierCatalog = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType == ConstantHelpers.TechnicalLibrary.FileType.SUPPLIER_CATALOG)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            supplierCatalog.Title = model.Title;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (supplierCatalog.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SUPPLIER_CATALOG}/{supplierCatalog.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                supplierCatalog.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.SUPPLIER_CATALOG,
                    supplierCatalog.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpDelete("catalogo-eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var supplierCatalog = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.SUPPLIER_CATALOG))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (supplierCatalog == null)
            {
                return BadRequest($"Archivo técnico con Id '{id}' no se halló.");
            }

            if (supplierCatalog.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SUPPLIER_CATALOG}/{supplierCatalog.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
            }

            _context.TechnicalLibraryFiles.Remove(supplierCatalog);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}