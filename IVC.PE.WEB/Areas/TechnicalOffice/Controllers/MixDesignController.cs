using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.AggregateTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.CementTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConcreteUseViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DesignTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.MixDesignViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
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
    [Route("oficina-tecnica/diseños-de-mezcla")]
    public class MixDesignController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public MixDesignController(IvcDbContext context,
            ILogger<MixDesignController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? designId = null, Guid? typeId = null, Guid? aggId = null, Guid? concreteId = null, bool? add = null, Guid? versionId = null)
        {

            var query = _context.MixDesigns
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Include(x => x.DesignType)
                .Include(x => x.CementType)
                .Include(x => x.AggregateType)
                .Include(x => x.ConcreteUse)
                .Include(x => x.TechnicalVersion)
                .Where(x=>x.ProjectId.Value == pId)
                .Select(x => new MixDesignViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    TechnicalVersionId = x.TechnicalVersionId,
                    TechnicalVersion = new TechnicalVersionViewModel
                    {
                        Description = x.TechnicalVersion.Description
                    },
                    CementTypeId = x.CementTypeId,
                    CementType = new CementTypeViewModel
                    {
                        Description = x.CementType.Description
                    },

                    DesignTypeId = x.DesignTypeId,
                    DesignType = new DesignTypeViewModel
                    {
                        Description = x.DesignType.Description
                    },

                    AggregateTypeId = x.AggregateTypeId,
                    AggregateType = new AggregateTypeViewModel
                    {
                        Description = x.AggregateType.Description
                    },

                    ConcreteUseId = x.ConcreteUseId,
                    ConcreteUse = new ConcreteUseViewModel
                    {
                        Description = x.ConcreteUse.Description
                    },
                    Additive = x.Additive,

                    DesignDateStr = x.DesignDate.Date.ToDateString(),
                    FileUrl = x.FileUrl
                })
                .ToListAsync();

            if (designId.HasValue)
                data = data.Where(x=>x.DesignTypeId == designId.Value).ToList();

            if (typeId.HasValue)
                data = data.Where(x=>x.CementTypeId == typeId.Value).ToList();

            if (aggId.HasValue)
                data = data.Where(x => x.AggregateTypeId == aggId.Value).ToList();

            if (concreteId.HasValue)
                data = data.Where(x => x.ConcreteUseId == concreteId.Value).ToList();

            if (add.HasValue)
                data = data.Where(x => x.Additive == add.Value).ToList();

            if (versionId.HasValue)
                data = data.Where(x => x.TechnicalVersionId == versionId.Value).ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.MixDesigns

                 .Where(x => x.Id == id)
                 .Select(x => new MixDesignViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     TechnicalVersionId = x.TechnicalVersionId,

                     CementTypeId = x.CementTypeId,


                     DesignTypeId = x.DesignTypeId,


                     AggregateTypeId = x.AggregateTypeId,


                     ConcreteUseId = x.ConcreteUseId,

                     Additive = x.Additive,

                     DesignDateStr = x.DesignDate.Date.ToDateString(),
                     FileUrl = x.FileUrl
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(MixDesignViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = new MixDesign
            {
                Name = model.Name,

                TechnicalVersionId = model.TechnicalVersionId,
                DesignTypeId = model.DesignTypeId,
                CementTypeId = model.CementTypeId,
                AggregateTypeId = model.AggregateTypeId,
                ConcreteUseId = model.ConcreteUseId,
                Additive = model.Additive,
                ProjectId = GetProjectId(),
                DesignDate = model.DesignDateStr.ToDateTime(),

                //MixDesignDateStr = x.MixDesignDate.Date.ToDateString(),

            };

            await _context.MixDesigns.AddAsync(bprint);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bprint.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.DESIGN,
                    $"diseño_{bprint.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, MixDesignViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = await _context.MixDesigns
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            bprint.Name = model.Name;

                bprint.TechnicalVersionId = model.TechnicalVersionId;
                bprint.DesignTypeId = model.DesignTypeId;
                bprint.CementTypeId = model.CementTypeId;
                bprint.AggregateTypeId = model.AggregateTypeId;
                bprint.ConcreteUseId = model.ConcreteUseId;
                bprint.Additive = model.Additive;

                bprint.DesignDate = model.DesignDateStr.ToDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bprint.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DESIGN}/{bprint.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                bprint.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.DESIGN,
                    $"diseño_{bprint.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.MixDesigns
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"diseño con Id '{id}' no se halló.");
            }

            if (bprint.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DESIGN}/{bprint.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.MixDesigns.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
