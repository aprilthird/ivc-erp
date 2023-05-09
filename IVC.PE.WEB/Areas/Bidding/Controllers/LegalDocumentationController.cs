using System;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationViewModels;
using IVC.PE.WEB.Services;
using Microsoft.Extensions.Options;
using IVC.PE.WEB.Options;
using IVC.PE.ENTITIES.UspModels.Biddings;
using System.Data;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/documentos-legal")]
    public class LegalDocumentationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public LegalDocumentationController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<LegalDocumentationController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var legalDocumentations = await _context.Set<UspLegalDocumentations>().FromSqlRaw("execute Biddings_uspLegalDocumentations")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(legalDocumentations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var legalDocumentation = await _context.LegalDocumentations
                .Include(x => x.LegalDocumentationType)
                .Where(x => x.Id == id)
                .Select(x => new LegalDocumentationViewModel
                {
                    Id = x.Id,
                    BusinessId = x.BusinessId,
                    Business = new BusinessViewModel
                    {
                        Id = x.BusinessId,
                        BusinessName = x.Business.BusinessName,
                        RUC = x.Business.RUC
                    },
                    LegalDocumentationTypeId = x.LegalDocumentationTypeId,
                    LegalDocumentationType = new LegalDocumentationTypeViewModel
                    {
                        Id = x.LegalDocumentationTypeId,
                        Name = x.LegalDocumentationType.Name
                    },
                    NumberOfRenovations = x.NumberOfRenovations,
                }).FirstOrDefaultAsync();

            var legalDocumentationRenovation = await _context.LegalDocumentationRenovations
                .Where(x => x.LegalDocumentationId == legalDocumentation.Id && x.LegalDocumentationOrder == legalDocumentation.NumberOfRenovations)
                .Select(x => new LegalDocumentationRenovationViewModel
                {
                    Id = x.Id,
                    LegalDocumentationId = x.LegalDocumentationId,
                    LegalDocumentationOrder = x.LegalDocumentationOrder,
                    CreateDate = x.CreateDate.ToDateString(),
                    EndDate = x.EndDate.ToDateString(),
                    FileUrl = x.FileUrl,
                    IsTheLast = x.IsTheLast,
                }).FirstOrDefaultAsync();

            legalDocumentation.LegalDocumentationRenovation = legalDocumentationRenovation;

            return Ok(legalDocumentation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(LegalDocumentationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var legalDocumentation = new LegalDocumentation
            {
                BusinessId = model.BusinessId,
                LegalDocumentationTypeId = model.LegalDocumentationTypeId,
                NumberOfRenovations = 1,
            };

            var legalDocumentationRenovation = new LegalDocumentationRenovation
            {
                LegalDocumentation = legalDocumentation,
                LegalDocumentationOrder = legalDocumentation.NumberOfRenovations,
                CreateDate = model.LegalDocumentationRenovation.CreateDate.ToDateTime(),
                EndDate = model.LegalDocumentationRenovation.EndDate.ToDateTime(),
                DaysLimitTerm = model.LegalDocumentationRenovation.daysLimitTerm,
                Days5 = false,
                IsTheLast = model.LegalDocumentationRenovation.IsTheLast
            };





            await _context.LegalDocumentationRenovations.AddAsync(legalDocumentationRenovation);
            await _context.LegalDocumentations.AddAsync(legalDocumentation);
            await _context.SaveChangesAsync();

            if (model.LegalDocumentationRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                legalDocumentationRenovation.FileUrl = await storage.UploadFile(model.LegalDocumentationRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.LegalDocumentationRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION,
                   $"documento-legal_{legalDocumentation.Id}_nro-{legalDocumentationRenovation.LegalDocumentationOrder}");
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, LegalDocumentationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var legalDocumentation = await _context.LegalDocumentations.FirstOrDefaultAsync(x => x.Id == id);
            legalDocumentation.LegalDocumentationTypeId = model.LegalDocumentationTypeId;
            legalDocumentation.BusinessId = model.BusinessId;

            var legalDocumentationRen = await _context.LegalDocumentationRenovations.FirstOrDefaultAsync(x => x.Id == model.LegalDocumentationRenovation.Id);

            legalDocumentationRen.CreateDate = model.LegalDocumentationRenovation.CreateDate.ToDateTime();
            legalDocumentationRen.EndDate = model.LegalDocumentationRenovation.EndDate.ToDateTime();
            legalDocumentationRen.DaysLimitTerm = model.LegalDocumentationRenovation.daysLimitTerm;
            legalDocumentationRen.IsTheLast = model.LegalDocumentationRenovation.IsTheLast;

            if (model.LegalDocumentationRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (legalDocumentationRen.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION}/{legalDocumentationRen.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                legalDocumentationRen.FileUrl = await storage.UploadFile(model.LegalDocumentationRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.LegalDocumentationRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION,
                    $"documento-legal_{legalDocumentation.Id}_nro-{legalDocumentationRen.LegalDocumentationOrder}");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var legalDocumentation = await _context.LegalDocumentations.FirstOrDefaultAsync(x => x.Id == id);

            var legalDocumentationRenovations = await _context.LegalDocumentationRenovations.Where(x => x.LegalDocumentationId == id).ToListAsync();

            foreach (var renovation in legalDocumentationRenovations)
            {
                if (renovation.FileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LEGAL_DOCUMENTATION}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.BIDDING);
                }
            }

            _context.LegalDocumentationRenovations.RemoveRange(legalDocumentationRenovations);
            _context.LegalDocumentations.Remove(legalDocumentation);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
