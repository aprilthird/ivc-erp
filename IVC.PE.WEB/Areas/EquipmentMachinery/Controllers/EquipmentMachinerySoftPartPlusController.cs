using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
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
    [Route("equipos/detalles-actividades")]
    public class EquipmentMachinerySoftPartPlusController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachinerySoftPartPlusController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachinerySoftPartPlusController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid foldingId)
        {
            if (foldingId == Guid.Empty)
                return Ok(new List<EquipmentMachinerySoftPartFoldingViewModel>());

            var renovations = await _context.EquipmentMachinerySoftPartPlusUltra
                .Include(x => x.EquipmentMachineryTypeSoftActivity)
                .Where(x => x.EquipmentMachinerySoftPartFoldingId == foldingId)
                .Select(x => new EquipmentMachinerySoftPartPlusViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTypeSoftActivityId = x.EquipmentMachineryTypeSoftActivityId,
                    EquipmentMachineryTypeSoftActivity = new EquipmentMachineryTypeSoftActivityViewModel
                    {
                        Description = x.EquipmentMachineryTypeSoftActivity.Description
                    },



                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachinerySoftPartPlusUltra
                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentMachinerySoftPartPlusViewModel
                 {
                     Id = x.Id,
                     EquipmentMachineryTypeSoftActivityId = x.EquipmentMachineryTypeSoftActivityId,                     
               


                 })
                 .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachinerySoftPartPlusViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var newRenovation = new EquipmentMachinerySoftPartPlus()
            {
                EquipmentMachineryTypeSoftActivityId = model.EquipmentMachineryTypeSoftActivityId,
           
                
            };


            await _context.EquipmentMachinerySoftPartPlusUltra.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachinerySoftPartPlusViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renovation = await _context.EquipmentMachinerySoftPartPlusUltra
                .FirstOrDefaultAsync(x => x.Id == id);


            renovation.EquipmentMachineryTypeSoftActivityId = model.EquipmentMachineryTypeSoftActivityId;
      

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.EquipmentMachinerySoftPartPlusUltra.FirstOrDefaultAsync(x => x.Id == id);

            _context.EquipmentMachinerySoftPartPlusUltra.Remove(data);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
