using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BimViewModels;
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
    [Route("oficina-tecnica/bim")]
    public class BimController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BimController(IvcDbContext context,
            ILogger<BimController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? workFrontId = null)
        {

            var query = _context.Bims
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Include(x=>x.ProjectFormula)
                .Include(x=>x.WorkFront)
                .Where(x=>x.ProjectId.Value == pId)
                .Select(x => new BimViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code,
                        Name = x.ProjectFormula.Name
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code

                    },
                    FileUrl = x.FileUrl
                })


                .ToListAsync();

            if (projectFormulaId.HasValue)
            {
                data = data.Where(x => x.ProjectFormulaId == projectFormulaId.Value).ToList();
            }

            if (workFrontId.HasValue)
            {
                data = data.Where(x => x.WorkFrontId == workFrontId.Value).ToList();
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Bims

                 .Where(x => x.Id == id)
                 .Select(x => new BimViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     ProjectFormulaId = x.ProjectFormulaId,
                     WorkFrontId = x.WorkFrontId,
                     FileUrl = x.FileUrl
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BimViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bim = new Bim
            {
                Name = model.Name,
                ProjectFormulaId = model.ProjectFormulaId,
                WorkFrontId = model.WorkFrontId,
                ProjectId = GetProjectId()
            };

            await _context.Bims.AddAsync(bim);
            await _context.SaveChangesAsync();
            
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bim.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.BIM,
                    $"bim_{bim.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BimViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bim = await _context.Bims
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            bim.Name = model.Name;
            bim.ProjectFormulaId = model.ProjectFormulaId;
            bim.WorkFrontId = model.WorkFrontId;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bim.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BIM}/{bim.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                bim.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.BIM,
                    $"bim_{bim.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bim = await _context.Bims
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bim == null)
            {
                return BadRequest($"BIM con Id '{id}' no se halló.");
            }

            if (bim.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BIM}/{bim.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.Bims.Remove(bim);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
