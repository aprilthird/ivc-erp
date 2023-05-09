using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockRoofViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/techos-antes")]
    public class StockRoofController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public StockRoofController(IvcDbContext context,
             IOptions<CloudStorageCredentials> storageCredentials,
           ILogger<StockRoofController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();
        /*
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var roofs = await _context.StockRoofs
                .Include(x => x.SewerGroup)
                .Include(x => x.Stock)
                .Include(x => x.ProjectPhase)
                .Select(x => new StockRoofViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    StockId = x.StockId,
                    Stock = new StockViewModel
                    {
                        Code = x.Stock.Code,
                        Description = x.Stock.Description,
                        Unit = x.Stock.Unit,
                        CurrencyType = x.Stock.CurrencyType,
                        SalePriceUnit = x.Stock.SalePriceUnit
                    },
                    RoofQuantity = x.RoofQuantity
                }).ToListAsync();

            return Ok(roofs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var roof = await _context.StockRoofs
                .Include(x => x.SewerGroup)
                .Include(x => x.Stock)
                .Include(x => x.ProjectPhase)
                .Where(x => x.Id == id)
                .Select(x => new StockRoofViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    },
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    StockId = x.StockId,
                    Stock = new StockViewModel
                    {
                        Code = x.Stock.Code,
                        Description = x.Stock.Description,
                        CurrencyType = x.Stock.CurrencyType,
                        SalePriceUnit = x.Stock.SalePriceUnit
                    },
                    RoofQuantity = x.RoofQuantity
                }).FirstOrDefaultAsync();

            return Ok(roof);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, StockRoofViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roof = await _context.StockRoofs.FirstOrDefaultAsync(x => x.Id == id);

            roof.ProjectPhaseId = model.ProjectPhaseId;
            roof.RoofQuantity = model.RoofQuantity;
            roof.SewerGroupId = model.SewerGroupId;
            roof.StockId = model.StockId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roof = await _context.StockRoofs.FirstOrDefaultAsync(x => x.Id == id);

            _context.StockRoofs.Remove(roof);

            await _context.SaveChangesAsync();

            return Ok();
        }
        */
    }
}
