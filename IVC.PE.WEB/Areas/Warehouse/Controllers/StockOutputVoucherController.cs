using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockOutputVoucherViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/vales-salida")]
    public class StockOutputVoucherController : BaseController
    {
        public StockOutputVoucherController(IvcDbContext context,
           ILogger<StockOutputVoucherController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();
        /*
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var vouchers = await _context.StockVouchers
                .Include(x => x.ProjectPhase)
                .Include(x => x.SewerGroup)
                .Where(x => x.VoucherType == ConstantHelpers.Warehouse.OUTPUT_VOUCHER)
                .Select(x => new StockOutputVoucherViewModel
                {
                    Id = x.Id,
                    VoucherType = x.VoucherType,
                    VoucherDate = x.VoucherDate.ToDateString(),
                    PickUpResponsible = x.PickUpResponsible,
                    Observation = x.Observation,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WasDelivered = x.WasDelivered,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    }
                }).ToListAsync();

            return Ok(vouchers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var voucher = await _context.StockVouchers.Where(x => x.Id == id)
                .Select(x => new StockOutputVoucherViewModel
                {
                    Id = x.Id,
                    VoucherType = x.VoucherType,
                    VoucherDate = x.VoucherDate.ToDateString(),
                    PickUpResponsible = x.PickUpResponsible,
                    Observation = x.Observation,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WasDelivered = x.WasDelivered,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code
                    }
                }).FirstOrDefaultAsync();

            return Ok(voucher);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(StockOutputVoucherViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockVoucher = new StockVoucher
            {
                VoucherType = ConstantHelpers.Warehouse.OUTPUT_VOUCHER,
                VoucherDate = model.VoucherDate.ToDateTime(),
                PickUpResponsible = model.PickUpResponsible,
                Observation = model.Observation,
                WasDelivered = model.WasDelivered,
                ProjectPhaseId = model.ProjectPhaseId,
                SewerGroupId = model.SewerGroupId
            };

            await _context.StockVouchers.AddAsync(stockVoucher);
            await _context.SaveChangesAsync();

            return Ok(stockVoucher);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, StockOutputVoucherViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucher = await _context.StockVouchers.FirstOrDefaultAsync(x => x.Id == id);

            voucher.VoucherType = ConstantHelpers.Warehouse.OUTPUT_VOUCHER;
            voucher.VoucherDate = model.VoucherDate.ToDateTime();
            voucher.PickUpResponsible = model.PickUpResponsible;
            voucher.Observation = model.Observation;
            voucher.WasDelivered = model.WasDelivered;
            voucher.ProjectPhaseId = model.ProjectPhaseId;
            voucher.SewerGroupId = model.SewerGroupId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("entregado/{id}")]
        public async Task<IActionResult> EditWasDelivered(Guid id)
        {
            var voucher = await _context.StockVouchers.FirstOrDefaultAsync(x => x.Id == id);

            voucher.WasDelivered = true;

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
