using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.UspModels.Biddings;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessLegalAgentFoldingViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/empresas-representante")]
    public class BusinessLegalAgentFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public BusinessLegalAgentFoldingController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<BusinessLegalAgentFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<BusinessLegalAgentFoldingViewModel>());

            var data = await _context.Set<UspBusinessLegalAgent>().FromSqlRaw("execute Bidding_uspBusinessLegalAgent")
             .IgnoreQueryFilters()
             .ToListAsync();

            data = data.Where(x => x.BusinessId == equipId).ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.BusinessLegalAgentFoldings
                .Include(x => x.Business)
                .Where(x => x.Id == id)
                .Select(x => new BusinessLegalAgentFoldingViewModel
                {
                    Id = x.Id,

                    BusinessId = x.BusinessId,

                    LegalAgent = x.LegalAgent,
                    IsActive = x.IsActive,
                    FromDate = x.FromDate.HasValue
                    ? x.FromDate.Value.Date.ToDateString() : String.Empty,

                    ToDate = x.ToDate.HasValue
                    ? x.ToDate.Value.Date.ToDateString() : String.Empty,

                    Order = x.Order,

                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BusinessLegalAgentFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.Businesses
                .Where(x => x.Id == model.BusinessId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.Number++;

            var newRenovation = new BusinessLegalAgentFolding()
            {
                BusinessId = model.BusinessId,
                
                LegalAgent = model.LegalAgent,

                FromDate = string.IsNullOrEmpty(model.FromDate)
                ? (DateTime?)null : model.FromDate.ToDateTime(),

                ToDate= string.IsNullOrEmpty(model.ToDate)
                ? (DateTime?)null : model.ToDate.ToDateTime(),

                Order = equipmentMachineryTransport.Number,

                IsActive = model.IsActive

            };






            await _context.BusinessLegalAgentFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            


            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BusinessLegalAgentFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.Businesses
                .Where(x => x.Id == model.BusinessId)
                .FirstOrDefaultAsync();

            var folding = await _context.BusinessLegalAgentFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.LegalAgent = model.LegalAgent;

            folding.FromDate = string.IsNullOrEmpty(model.FromDate)
                ? (DateTime?)null : model.FromDate.ToDateTime();

            folding.ToDate = string.IsNullOrEmpty(model.ToDate)
                ? (DateTime?)null : model.ToDate.ToDateTime();

            folding.IsActive = model.IsActive;



            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.BusinessLegalAgentFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var equipment = await _context.Businesses
    .FirstOrDefaultAsync(x => x.Id == renovation.BusinessId);
            equipment.Number--;



            _context.BusinessLegalAgentFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
