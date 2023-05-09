using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.InstructionalProcedureViewModels;
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
    [Route("calidad/procedimientos-instructivos")]
    public class InstructionalProcedureController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public InstructionalProcedureController(IvcDbContext context,
    IConfiguration configuration,
    ILogger<InstructionalProcedureController> logger,
    IOptions<CloudStorageCredentials> storageCredentials)
    : base(context, configuration, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.InstructionalProcedures
                .AsQueryable();



            var data = await query
                .Select(x => new InstructionalProcedureViewModel
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
            var model = await _context.InstructionalProcedures
                .Where(x => x.Id == id)
                .Select(x => new InstructionalProcedureViewModel
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
        public async Task<IActionResult> Create(InstructionalProcedureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructionalProcedure = new InstructionalProcedure
            {
                Code = model.Code,
                Title = model.Title,
                PublicationDate = model.PublicationDate.ToUtcDateTime(),
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                instructionalProcedure.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.INSTRUCTIONAL_PROCEDURES, instructionalProcedure.Title);
            }

            await _context.InstructionalProcedures.AddAsync(instructionalProcedure);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, InstructionalProcedureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructionalProcedure = await _context.InstructionalProcedures
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            instructionalProcedure.Code = model.Code;
            instructionalProcedure.Title = model.Title;
            instructionalProcedure.PublicationDate = model.PublicationDate.ToUtcDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (instructionalProcedure.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.INSTRUCTIONAL_PROCEDURES}/{instructionalProcedure.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                instructionalProcedure.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.INSTRUCTIONAL_PROCEDURES,
                    instructionalProcedure.Title);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Quality.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var instructionalProcedure = await _context.InstructionalProcedures
                .FirstOrDefaultAsync(x => x.Id == id);

            if (instructionalProcedure == null)
            {
                return BadRequest($"Procedimiento Instructivo con Id '{id}' no se halló.");
            }

            if (instructionalProcedure.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.INSTRUCTIONAL_PROCEDURES}/{instructionalProcedure.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.InstructionalProcedures.Remove(instructionalProcedure);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
