using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockInputVoucherViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/vales-ingreso")]
    public class StockInputVoucherController : BaseController
    {
        public StockInputVoucherController(IvcDbContext context,
           ILogger<StockInputVoucherController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();
        /*
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var vouchers = await _context.StockVouchers.Where(x => x.VoucherType == ConstantHelpers.Warehouse.INPUT_VOUCHER)
                .Select(x => new StockInputVoucherViewModel
                {
                    Id = x.Id,
                    VoucherType = x.VoucherType,
                    VoucherDate = x.VoucherDate.ToDateString(),
                    Supplier = x.Supplier,
                    ReferencePurchaseOrder = x.ReferencePurchaseOrder
                }).ToListAsync();

            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var voucher = await _context.StockVouchers.Where(x => x.Id == id)
                .Select(x => new StockInputVoucherViewModel
                {
                    Id = x.Id,
                    VoucherType = x.VoucherType,
                    VoucherDate = x.VoucherDate.ToDateString(),
                    Supplier = x.Supplier,
                    ReferencePurchaseOrder = x.ReferencePurchaseOrder
                }).FirstOrDefaultAsync();

            return Ok(voucher);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(StockInputVoucherViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockVoucher = new StockVoucher
            {
                ReferencePurchaseOrder = model.ReferencePurchaseOrder,
                Supplier = model.Supplier,
                VoucherDate = model.VoucherDate.ToDateTime(),
                VoucherType = ConstantHelpers.Warehouse.INPUT_VOUCHER
            };

            await _context.StockVouchers.AddAsync(stockVoucher);
            await _context.SaveChangesAsync();

            return Ok(stockVoucher);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, StockInputVoucherViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucher = await _context.StockVouchers.FirstOrDefaultAsync(x => x.Id == id);

            voucher.ReferencePurchaseOrder = model.ReferencePurchaseOrder;
            voucher.Supplier = model.Supplier;
            voucher.VoucherDate = model.VoucherDate.ToDateTime();
            voucher.VoucherType = ConstantHelpers.Warehouse.INPUT_VOUCHER;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var voucher = await _context.StockVouchers.FirstOrDefaultAsync(x => x.Id == id);
            var details = await _context.StockVoucherDetails.Where(x => x.StockVoucherId == voucher.Id).ToListAsync();

            //TODO: Actualizar el stock de los materiales


            _context.StockVoucherDetails.RemoveRange(details);
            _context.StockVouchers.Remove(voucher);

            await _context.SaveChangesAsync();

            return Ok();
        }
        */
    }
}
