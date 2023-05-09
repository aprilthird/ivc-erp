using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.AreaModuleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ErpManualViewModels;
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
    [Route("oficina-tecnica/manuales")]
    public class ErpManualController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ErpManualController(IvcDbContext context,
            ILogger<ErpManualController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? moduleId = null)
        {

            var query = _context.ErpManuals
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Include(x => x.AreaModule)
                .Select(x => new ErpManualViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    
                    AreaModuleId = x.AreaModuleId,
                    AreaModule = new AreaModuleViewModel
                    {
                        Description = x.AreaModule.Description
                    },

                    FileUrl = x.FileUrl,

                })
                .ToListAsync();

            
            if (moduleId.HasValue)
                data = data.Where(x => x.AreaModuleId == moduleId.Value).ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.ErpManuals

                 .Where(x => x.Id == id)
                 .Select(x => new ErpManualViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     AreaModuleId = x.AreaModuleId,
                     FileUrl = x.FileUrl
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ErpManualViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = new ErpManual
            {
                Name = model.Name,
                AreaModuleId = model.AreaModuleId
                //ErpManualDateStr = x.ErpManualDate.Date.ToDateString(),

            };

            await _context.ErpManuals.AddAsync(bprint);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bprint.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.MANUAL,
                    $"plano_{bprint.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ErpManualViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = await _context.ErpManuals
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            bprint.Name = model.Name;
            bprint.AreaModuleId = model.AreaModuleId;
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bprint.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.MANUAL}/{bprint.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                bprint.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.MANUAL,
                    $"plano_{bprint.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.ErpManuals
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"plano con Id '{id}' no se halló.");
            }

            if (bprint.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.MANUAL}/{bprint.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.ErpManuals.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
