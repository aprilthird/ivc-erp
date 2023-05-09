using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.SigProcessViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    [Route("calidad/procesos-sig")]
    public class SigProcessController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public SigProcessController(IvcDbContext context,
IConfiguration configuration,
ILogger<SigProcessController> logger,
IOptions<CloudStorageCredentials> storageCredentials)
: base(context, configuration, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.SigProcesses
                .AsQueryable();



            var data = await query
                .Select(x => new SigProcessViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    PublicationDate = x.PublicationDate.HasValue
                        ? x.PublicationDate.Value.ToLocalDateFormat() : String.Empty,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = await _context.SigProcesses
                .Where(x => x.Id == id)
                .Select(x => new SigProcessViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    PublicationDate = x.PublicationDate.HasValue
                        ? x.PublicationDate.Value.ToLocalDateFormat() : String.Empty,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(SigProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sigProcess = new SigProcess
            {
                Code = model.Code,
                Title = model.Title,
                PublicationDate = string.IsNullOrEmpty(model.PublicationDate)
                ? (DateTime?)null : model.PublicationDate.ToUtcDateTime(),

                /* model.PublicationDate.ToUtcDateTime(),*/
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                sigProcess.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.SIG_PROCESSES, sigProcess.Title);
            }

            await _context.SigProcesses.AddAsync(sigProcess);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SigProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sigProcess = await _context.SigProcesses
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            sigProcess.Code = model.Code;
            sigProcess.Title = model.Title;
            sigProcess.PublicationDate = string.IsNullOrEmpty(model.PublicationDate)
    ? (DateTime?)null : model.PublicationDate.ToUtcDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (sigProcess.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SIG_PROCESSES}/{sigProcess.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                sigProcess.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.SIG_PROCESSES,
                    sigProcess.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sigProcess = await _context.SigProcesses
                .FirstOrDefaultAsync(x => x.Id == id);

            if (sigProcess == null)
            {
                return BadRequest($"Proceso SIG con Id '{id}' no se halló.");
            }

            if (sigProcess.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SIG_PROCESSES}/{sigProcess.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.SigProcesses.Remove(sigProcess);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
