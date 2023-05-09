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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.LegalTechnicalLibrary.Controllers.TechnicalLibrary
{
    [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LEGAL_TECHNICAL_LIBRARY)]
    [Route("libreria-tecnica/proveedores")]
    public class SupplierController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public SupplierController(IvcDbContext context,
            ILogger<SupplierController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Suppliers
                .AsNoTracking()
                .AsQueryable();

            //query = query.Where(x => x.Id.Equals(Id)).OrderBy(x => x.BusinessName);

            var data = await query
                .Select(x => new SupplierViewModel
                {
                    Id = x.Id,
                    RUC = x.RUC,
                    BusinessName = x.BusinessName,
                    FileCount = x.FileCount
                }).ToListAsync();

            data.OrderBy(x => x.BusinessName);

            return PartialView("_Results", data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var supplier = new Supplier
            {
                RUC = model.RUC,
                BusinessName = model.BusinessName,
                FileCount = 0
            };

            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}