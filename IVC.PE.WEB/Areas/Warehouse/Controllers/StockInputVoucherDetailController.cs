using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockInputVoucherViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/vales-ingreso/detalle")]
    public class StockInputVoucherDetailController : BaseController
    {
        public StockInputVoucherDetailController(IvcDbContext context,
           ILogger<StockInputVoucherDetailController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();
        /*
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid voucherId)
        {
            if (voucherId == Guid.Empty)
                return Ok(new List<StockInputVoucherDetailViewModel>());

            var voucherDetails = await _context.StockVoucherDetails
                .Include(x => x.StockVoucher)
                .Include(x => x.Stock)
                .Where(x => x.StockVoucherId == voucherId)
                .Select(x => new StockInputVoucherDetailViewModel
                {
                    Id = x.Id,
                    StockVoucherId = x.StockVoucherId,
                    StockId = x.StockId,
                    Stock = new StockViewModel
                    {
                        Code = x.Stock.Code,
                        SalePriceUnit = x.Stock.SalePriceUnit,
                        Unit = x.Stock.Unit,
                        CurrencyType = x.Stock.CurrencyType,
                        Description = x.Stock.Description
                    },
                    CurrencyType = x.CurrencyType,
                    Quantity = x.Quantity,
                    SalePriceUnit = x.SalePriceUnit
                }).ToListAsync();

            return Ok(voucherDetails);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var voucherDetail = await _context.StockVoucherDetails
                .Include(x => x.Stock)
                .Where(x => x.Id == id)
                .Select(x => new StockInputVoucherDetailViewModel
                {
                    Id = x.Id,
                    StockVoucherId = x.StockVoucherId,
                    StockId = x.StockId,
                    CurrencyType = x.CurrencyType,
                    Quantity = x.Quantity,
                    SalePriceUnit = x.SalePriceUnit
                }).FirstOrDefaultAsync();

            return Ok(voucherDetail);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(StockInputVoucherDetailViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucherDetail = new StockVoucherDetail
            {
                CurrencyType = ConstantHelpers.Currency.NUEVOS_SOLES,
                Quantity = model.Quantity,
                SalePriceUnit = model.SalePriceUnit,
                StockId = model.StockId,
                StockVoucherId = model.StockVoucherId
            };

            //TODO: Actualizar el stock del material

            await _context.StockVoucherDetails.AddAsync(voucherDetail);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, StockInputVoucherDetailViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucherDetail = await _context.StockVoucherDetails
                .FirstOrDefaultAsync(x => x.Id == id);

            voucherDetail.StockId = model.StockId;
            //voucherDetail.CurrencyType = model.CurrencyType;
            voucherDetail.Quantity = model.Quantity;
            voucherDetail.SalePriceUnit = model.SalePriceUnit;

            //TODO: Actualizar el stock del material

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucherDetail = await _context.StockVoucherDetails
                .FirstOrDefaultAsync(x => x.Id == id);

            //TODO: Actualizar el stock del material

            _context.StockVoucherDetails.Remove(voucherDetail);

            await _context.SaveChangesAsync();

            return Ok();
        }
        */
    }
}
