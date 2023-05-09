using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/proveedores-de-combustible-folding")]


    public class FuelProviderFoldingController : BaseController
    {
        public FuelProviderFoldingController(IvcDbContext context,
 ILogger<FuelProviderFoldingController> logger) : base(context, logger)
        {

        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentProviderId = null)
        {
            if (!equipmentProviderId.HasValue)
                return Ok(new List<FuelProviderFoldingViewModel>());

            var folding = await _context.FuelProviderFoldings
                .Include(x => x.FuelProvider)
                .Where(x => x.FuelProviderId == equipmentProviderId)
                .Select(x => new FuelProviderFoldingViewModel
                {
                    Id = x.Id,
                    FuelProviderId = x.FuelProviderId,
                    CisternPlate = x.CisternPlate
                }).ToListAsync();

            return Ok(folding);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var equip = await _context.FuelProviderFoldings
                .Where(x => x.Id == id)
                .Select(x => new FuelProviderFoldingViewModel
                {
                    Id = x.Id,
                    FuelProviderId = x.FuelProviderId,
                    CisternPlate = x.CisternPlate
                }).FirstOrDefaultAsync();

            return Ok(equip);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FuelProviderFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equip = new FuelProviderFolding
            {
                FuelProviderId = model.FuelProviderId,
                CisternPlate = model.CisternPlate

            };

            await _context.FuelProviderFoldings.AddAsync(equip);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FuelProviderFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var equipment = await _context.FuelProviderFoldings
                .FirstOrDefaultAsync(x => x.Id == id);

            equipment.CisternPlate = model.CisternPlate;
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.FuelProviderFoldings.FirstOrDefaultAsync(x => x.Id == id);

            _context.FuelProviderFoldings.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
