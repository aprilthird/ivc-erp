using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalLibrayViewModels;
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
    [Route("oficina-tecnica/biblioteca-tecnica")]
    public class TechnicalLibraryController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public TechnicalLibraryController(IvcDbContext context,
            ILogger<TechnicalLibraryController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? specId = null)
        {

            var query = _context.TechnicalLibrarys
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Include(x => x.Speciality)
                .Select(x => new TechnicalLibraryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    
                   
                    SpecialityId = x.SpecialityId.Value,
                    Speciality = new SpecialityViewModel
                    {
                        Description = x.Speciality.Description
                    },
                    Author = x.Author,
                    LibraryDateStr= x.LibraryDate.Date.ToDateString(),
                    FileUrl = x.FileUrl
                })
                .ToListAsync();

            if (specId.HasValue)
                data = data.Where(x => x.SpecialityId == specId.Value).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.TechnicalLibrarys

                 .Where(x => x.Id == id)
                 .Select(x => new TechnicalLibraryViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     SpecialityId = x.SpecialityId.Value,
                     Author = x.Author,
                     LibraryDateStr = x.LibraryDate.Date.ToDateString(),
                     FileUrl = x.FileUrl
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TechnicalLibraryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spec = new TechnicalLibrary
            {
                Name = model.Name,
                Author = model.Author,
                LibraryDate = model.LibraryDateStr.ToDateTime(),
                SpecialityId = model.SpecialityId,



                //TechnicalLibraryDateStr = x.TechnicalLibraryDate.Date.ToDateString(),

            };

            await _context.TechnicalLibrarys.AddAsync(spec);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                spec.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.TECHNICAL_LIBRARY,
                    $"biblioteca_{spec.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TechnicalLibraryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spec = await _context.TechnicalLibrarys
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            spec.Name = model.Name;
            spec.Author = model.Author;
            spec.LibraryDate = model.LibraryDateStr.ToDateTime();
            spec.SpecialityId = model.SpecialityId;







            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (spec.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.TECHNICAL_LIBRARY}/{spec.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                spec.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.TECHNICAL_LIBRARY,
                    $"biblioteca_{spec.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spec = await _context.TechnicalLibrarys
                .FirstOrDefaultAsync(x => x.Id == id);

            if (spec == null)
            {
                return BadRequest($"biblioteca con Id '{id}' no se halló.");
            }

            if (spec.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.TECHNICAL_LIBRARY}/{spec.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.TechnicalLibrarys.Remove(spec);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
