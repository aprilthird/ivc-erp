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
    [Route("libreria-tecnica/normas-tecnicas-peruanas")]
    public class PeruvianTechnicalStandarController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public PeruvianTechnicalStandarController(IvcDbContext context,
            ILogger<PeruvianTechnicalStandarController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.TechnicalLibraryFiles
                .AsNoTracking()
                .AsQueryable();

            query = query.Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.PERUVIAN_TECHNICAL_STANDAR));

            var data = await query
                .OrderBy(x => x.Title)
                .Select(x => new PeruvianTechnicalStandarsViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    EffectiveDate = x.EffectiveDate.HasValue ? x.EffectiveDate.Value.ToLocalDateFormat() : String.Empty,
                    FileType = x.FileType,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPeruvianTechnicalStandar(Guid id)
        {
            var model = await _context.TechnicalLibraryFiles
                .Where(x => x.Id.Equals(id))
                .Select(x => new PeruvianTechnicalStandarsViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    EffectiveDate = x.EffectiveDate.HasValue ? x.EffectiveDate.Value.ToLocalDateFormat() : String.Empty,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(PeruvianTechnicalStandarsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var technicalLibraryFile = new TechnicalLibraryFile
            {
                Code = model.Code,
                Title = model.Title,
                EffectiveDate = model.EffectiveDate.ToUtcDateTime(),
                FileType = model.FileType
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                technicalLibraryFile.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PERUVIAN_TECHNICAL_STANDAR, technicalLibraryFile.Title);
            }

            await _context.TechnicalLibraryFiles.AddAsync(technicalLibraryFile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PeruvianTechnicalStandarsViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var peruvianTechnicalStandar = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType == ConstantHelpers.TechnicalLibrary.FileType.PERUVIAN_TECHNICAL_STANDAR)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            peruvianTechnicalStandar.Code = model.Code;
            peruvianTechnicalStandar.Title = model.Title;
            peruvianTechnicalStandar.EffectiveDate = model.EffectiveDate.ToUtcDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (peruvianTechnicalStandar.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERUVIAN_TECHNICAL_STANDAR}/{peruvianTechnicalStandar.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                peruvianTechnicalStandar.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PERUVIAN_TECHNICAL_STANDAR,
                    peruvianTechnicalStandar.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var peruvianTechnicalStandar = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.PERUVIAN_TECHNICAL_STANDAR))
                .FirstOrDefaultAsync(x => x.Id == id);


            if (peruvianTechnicalStandar == null)
            {
                return BadRequest($"Archivo técnico con Id '{id}' no se halló.");
            }

            if (peruvianTechnicalStandar.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERUVIAN_TECHNICAL_STANDAR}/{peruvianTechnicalStandar.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
            }

            _context.TechnicalLibraryFiles.Remove(peruvianTechnicalStandar);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}