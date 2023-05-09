using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/detalles-transporte-actividades")]
    public class EquipmentMachineryTransportPartPlusController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryTransportPartPlusController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachineryTransportPartPlusController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid foldingId)
        {
            if (foldingId == Guid.Empty)
                return Ok(new List<EquipmentMachineryTransportPartFoldingViewModel>());

            var renovations = await _context.EquipmentMachineryTransportPartPlusUltra
                .Include(x => x.EquipmentMachineryTypeTransportActivity)
                .Where(x => x.EquipmentMachineryTransportPartFoldingId == foldingId)
                .Select(x => new EquipmentMachineryTransportPartPlusViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTypeTransportActivityId = x.EquipmentMachineryTypeTransportActivityId,
                    EquipmentMachineryTypeTransportActivity = new EquipmentMachineryTypeTransportActivityViewModel
                    {
                        Description = x.EquipmentMachineryTypeTransportActivity.Description
                    },



                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachineryTransportPartPlusUltra
                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentMachineryTransportPartPlusViewModel
                 {
                     Id = x.Id,
                     EquipmentMachineryTypeTransportActivityId = x.EquipmentMachineryTypeTransportActivityId,



                 })
                 .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTransportPartPlusViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var newRenovation = new EquipmentMachineryTransportPartPlus()
            {
                EquipmentMachineryTypeTransportActivityId = model.EquipmentMachineryTypeTransportActivityId,


            };


            await _context.EquipmentMachineryTransportPartPlusUltra.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTransportPartPlusViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renovation = await _context.EquipmentMachineryTransportPartPlusUltra
                .FirstOrDefaultAsync(x => x.Id == id);


            renovation.EquipmentMachineryTypeTransportActivityId = model.EquipmentMachineryTypeTransportActivityId;


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.EquipmentMachineryTransportPartPlusUltra.FirstOrDefaultAsync(x => x.Id == id);

            _context.EquipmentMachineryTransportPartPlusUltra.Remove(data);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
