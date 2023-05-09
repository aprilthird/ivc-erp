using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessParticipationFoldingViewModels;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/empresas-participacion")]
    public class BusinessParticipationFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public BusinessParticipationFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
    ILogger<BusinessParticipationFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid fId)
        {
            if (fId == Guid.Empty)
                return Ok(new List<BusinessParticipationFoldingViewModel>());

            var renovations = await _context.BusinessParticipationFoldings
                            .Include(x => x.Business)
                            .Where(x => x.BusinessId == fId)
                            .Select(x => new BusinessParticipationFoldingViewModel
                            {
                                Id = x.Id,
                                IvcParticipation = x.IvcParticipation,
                                BusinessId = x.BusinessId,
                                Order = x.Order,
                                TestimonyUrl = x.TestimonyUrl,
                                Name = x.Name
                            })
                            .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.BusinessParticipationFoldings
                .Include(x => x.Business)
                .Where(x => x.Id == id)
                .Select(x => new BusinessParticipationFoldingViewModel
                {
                    Id = x.Id,

                    BusinessId = x.BusinessId,
                    IvcParticipation = x.IvcParticipation,
                    TestimonyUrl = x.TestimonyUrl,
                    Order = x.Order,
                    Name = x.Name

                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BusinessParticipationFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bs = await _context.Businesses
                .Where(x => x.Id == model.BusinessId)
                .FirstOrDefaultAsync();

            bs.NumberofParticipations++;

            var newRenovation = new BusinessParticipationFolding()
            {
                BusinessId = model.BusinessId,

               IvcParticipation = model.IvcParticipation,

                Order = bs.NumberofParticipations,
                Name = model.Name
            };

            if (model.FileTestimony != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.TestimonyUrl = await storage.UploadFile(model.FileTestimony.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    Path.GetExtension(model.FileTestimony.FileName),
                    ConstantHelpers.Storage.Blobs.BUSINESS,
                    $"testimonio-{bs.NumberofParticipations}-{bs.RUC}");
            }




            await _context.BusinessParticipationFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();




            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BusinessParticipationFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bs = await _context.Businesses
                .Where(x => x.Id == model.BusinessId)
                .FirstOrDefaultAsync();

            var folding = await _context.BusinessParticipationFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.IvcParticipation = model.IvcParticipation;
            folding.Name = model.Name;
            if (model.FileTestimony != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.TestimonyUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{folding.TestimonyUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                folding.TestimonyUrl = await storage.UploadFile(model.FileTestimony.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    Path.GetExtension(model.FileTestimony.FileName),
                    ConstantHelpers.Storage.Blobs.BUSINESS,
                    $"testimonio-{bs.NumberofParticipations}-{bs.RUC}");
            }


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.BusinessParticipationFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var bs = await _context.Businesses
    .FirstOrDefaultAsync(x => x.Id == renovation.BusinessId);
            bs.NumberofParticipations--;

            if (renovation.TestimonyUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BUSINESS}/{renovation.TestimonyUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            _context.BusinessParticipationFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
