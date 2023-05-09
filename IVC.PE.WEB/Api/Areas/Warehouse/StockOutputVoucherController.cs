using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Api.Areas.Warehouse
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/almacenes/vales-salida")]
    public class StockOutputVoucherController : BaseController
    {
        public StockOutputVoucherController(IvcDbContext context,
           ILogger<StockOutputVoucherController> logger) : base(context, logger)
        {
        }
/*
        [HttpPost("crear")]
        public async Task<IActionResult> Create(StockOutputVoucherResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucher = new StockVoucher
            {
                VoucherType = ConstantHelpers.Warehouse.OUTPUT_VOUCHER,
                VoucherDate = model.RequestDate.ToDateTime(),
                SewerGroupId = model.SewerGroupId,
                ProjectPhaseId = model.ProjectPhaseId,
                PickUpResponsible = model.PickUpResponsible,
                Observation = model.Observations
            };

            await _context.StockVoucherDetails
                .AddRangeAsync(model.VoucherDetails
                    .Select(x => new StockVoucherDetail
                    {
                        StockVoucher = voucher,
                        CurrencyType = ConstantHelpers.Currency.NUEVOS_SOLES,
                        Quantity = x.QuantityRequest,
                        StockId = x.StockId
                    }));

            await _context.StockVouchers.AddAsync(voucher);

            await _context.SaveChangesAsync();

            return Ok();
        }
*/
    }
}
