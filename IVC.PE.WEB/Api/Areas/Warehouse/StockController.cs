using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Api.Areas.Warehouse
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/almacenes/existencias")]
    public class StockController : BaseController
    {
        public StockController(IvcDbContext context,
           ILogger<StockController> logger) : base(context, logger)
        {
        }
        /*
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks
                .Select(x => new StockResourceModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    Description = x.Description
                }).ToListAsync();

            return Ok(stocks);
        }

        [HttpGet("vales-salida")]
        public async Task<IActionResult> GetForOutputVoucher(Guid stockId, Guid sewerGroupId, Guid projectPhaseId)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.Id == stockId);

            var stockRoof = await _context.StockRoofs
                .FirstOrDefaultAsync(x => x.StockId == stockId &&
                                          x.SewerGroupId == sewerGroupId &&
                                          x.ProjectPhaseId == projectPhaseId);

            var stockResource = new StockResourceModel
            {
                Id = stock.Id,
                Code = stock.Code,
                Description = stock.Description,
                Quantity = stock.Quantity,
                QuantityRoof = stockRoof != null ? stockRoof.RoofQuantity : stock.Quantity
            };

            return Ok(stockResource);
        }
        */
    }
}
