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
    [Route("libreria-tecnica/procedimientos-constructivos-generales")]
    public class GeneralConstructionProcedureController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public GeneralConstructionProcedureController(IvcDbContext context,
            ILogger<GeneralConstructionProcedureController> logger,
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

            query = query.Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.GENERAL_CONSTRUCTION_PROCEDURES));

            var data = await query
                .OrderBy(x => x.Title)
                .Select(x => new GeneralConstructionProceduresViewModel
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
        public async Task<IActionResult> GetGeneralConstructionProcedure(Guid id)
        {
            var model = await _context.TechnicalLibraryFiles
                .Where(x => x.Id.Equals(id))
                .Select(x => new GeneralConstructionProceduresViewModel
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
        public async Task<IActionResult> Create(GeneralConstructionProceduresViewModel model)
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
                    ConstantHelpers.Storage.Blobs.GENERAL_CONSTRUCTION_PROCEDURES, technicalLibraryFile.Title);
            }

            await _context.TechnicalLibraryFiles.AddAsync(technicalLibraryFile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, GeneralConstructionProceduresViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var generalConstructionProcedure = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType == ConstantHelpers.TechnicalLibrary.FileType.GENERAL_CONSTRUCTION_PROCEDURES)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            generalConstructionProcedure.Code = model.Code;
            generalConstructionProcedure.Title = model.Title;
            generalConstructionProcedure.EffectiveDate = model.EffectiveDate.ToUtcDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (generalConstructionProcedure.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.SEDAPAL_TECHNICAL_SPECIFICATION}/{generalConstructionProcedure.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                generalConstructionProcedure.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.GENERAL_CONSTRUCTION_PROCEDURES,
                    generalConstructionProcedure.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var generalConstructionProcedure = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.GENERAL_CONSTRUCTION_PROCEDURES))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (generalConstructionProcedure == null)
            {
                return BadRequest($"Archivo técnico con Id '{id}' no se halló.");
            }

            if (generalConstructionProcedure.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.GENERAL_CONSTRUCTION_PROCEDURES}/{generalConstructionProcedure.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
            }

            _context.TechnicalLibraryFiles.Remove(generalConstructionProcedure);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}