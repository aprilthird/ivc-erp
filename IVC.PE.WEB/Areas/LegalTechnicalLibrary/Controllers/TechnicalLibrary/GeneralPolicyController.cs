﻿using System;
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
    [Route("biblioteca-tecnica/politicas-generales")]
    public class GeneralPolicyController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public GeneralPolicyController(IvcDbContext context, 
            IConfiguration configuration,
            ILogger<GeneralPolicyController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, configuration, logger)
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

            query = query.Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.GENERAL_POLICY));

            var data = await query
                .OrderBy(x => x.Title)
                .Select(x => new GeneralPolicyViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    PublicationDate = x.EffectiveDate.HasValue
                        ? x.EffectiveDate.Value.ToLocalDateFormat() : String.Empty,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = await _context.TechnicalLibraryFiles
                .Where(x => x.Id == id)
                .Select(x => new GeneralPolicyViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Title = x.Title,
                    PublicationDate = x.EffectiveDate.HasValue 
                        ? x.EffectiveDate.Value.ToLocalDateFormat() : String.Empty,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(GeneralPolicyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var technicalLibraryFile = new TechnicalLibraryFile
            {
                Code = model.Code,
                Title = model.Title,
                EffectiveDate = model.PublicationDate.ToUtcDateTime(),
                FileType = ConstantHelpers.TechnicalLibrary.FileType.GENERAL_POLICY
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                technicalLibraryFile.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.GENERAL_POLICY, technicalLibraryFile.Title);
            }

            await _context.TechnicalLibraryFiles.AddAsync(technicalLibraryFile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, GeneralPolicyViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var generalPolicy = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType == ConstantHelpers.TechnicalLibrary.FileType.GENERAL_POLICY)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            generalPolicy.Code = model.Code;
            generalPolicy.Title = model.Title;
            generalPolicy.EffectiveDate = model.PublicationDate.ToUtcDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (generalPolicy.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.GENERAL_POLICY}/{generalPolicy.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                generalPolicy.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.GENERAL_POLICY,
                    generalPolicy.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var generalPolicy = await _context.TechnicalLibraryFiles
                .Where(x => x.FileType.Equals(ConstantHelpers.TechnicalLibrary.FileType.GENERAL_POLICY))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (generalPolicy == null)
            {
                return BadRequest($"Archivo técnico con Id '{id}' no se halló.");
            }

            if (generalPolicy.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.GENERAL_POLICY}/{generalPolicy.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
            }

            _context.TechnicalLibraryFiles.Remove(generalPolicy);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}