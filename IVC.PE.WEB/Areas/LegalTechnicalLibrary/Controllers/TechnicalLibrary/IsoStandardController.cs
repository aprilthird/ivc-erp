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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.LegalTechnicalLibrary.Controllers.TechnicalLibrary
{
    [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LEGAL_TECHNICAL_LIBRARY)]
    [Route("biblioteca-tecnica/normas-iso")]
    public class IsoStandardController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public IsoStandardController(IvcDbContext context,
            IConfiguration configuration,
            ILogger<IsoStandardController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, configuration, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.IsoStandards
                .AsQueryable();

            

            var data = await query
                .Select(x => new IsoStandardViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    PublicationDate = x.PublicationDate.HasValue
                        ? x.PublicationDate.Value.Date.ToDateString() : String.Empty,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = await _context.IsoStandards
                .Where(x => x.Id == id)
                .Select(x => new IsoStandardViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    PublicationDate = x.PublicationDate.HasValue
                        ? x.PublicationDate.Value.Date.ToDateString() : String.Empty,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(IsoStandardViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isoStandard = new IsoStandard
            {
                Code = model.Code,
                Title = model.Title,
                PublicationDate = string.IsNullOrEmpty(model.PublicationDate)
                ? (DateTime?)null : model.PublicationDate.ToDateTime(),
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                isoStandard.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.ISO_STANDARD, isoStandard.Title);
            }

            await _context.IsoStandards.AddAsync(isoStandard);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id,IsoStandardViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isoStandard = await _context.IsoStandards
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            isoStandard.Code = model.Code;
            isoStandard.Title = model.Title;
            isoStandard.PublicationDate = string.IsNullOrEmpty(model.PublicationDate)
? (DateTime?)null : model.PublicationDate.ToDateTime();


            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (isoStandard.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.ISO_STANDARD}/{isoStandard.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                isoStandard.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.ISO_STANDARD,
                    isoStandard.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var isoStandard = await _context.IsoStandards
                .FirstOrDefaultAsync(x => x.Id == id);

            if (isoStandard == null)
            {
                return BadRequest($"Norma ISO con Id '{id}' no se halló.");
            }

            if (isoStandard.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.ISO_STANDARD}/{isoStandard.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
            }

            _context.IsoStandards.Remove(isoStandard);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
