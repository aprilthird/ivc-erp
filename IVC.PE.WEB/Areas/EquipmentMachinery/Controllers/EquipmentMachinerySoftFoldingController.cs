using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftViewModels;
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
    [Route("equipos/equipo-liviano-folding")]
    public class EquipmentMachinerySoftFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public EquipmentMachinerySoftFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachinerySoftFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<EquipmentMachinerySoftViewModel>());

            var renovations = await _context.EquipmentMachinerySoftFoldings
                .Include(x => x.EquipmentMachinerySoft)
                .Where(x => x.EquipmentMachinerySoftId == equipId)
                .Select(x => new EquipmentMachinerySoftFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachinerySoftId = x.EquipmentMachinerySoftId,
                    
                    FreeText = x.FreeText,
                    FreeDate = x.FreeDate.Date.ToDateString() 
                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachinerySoftFoldings
                .Include(x => x.EquipmentMachinerySoft)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachinerySoftFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachinerySoftId = x.EquipmentMachinerySoftId,
                    FreeText = x.FreeText,
                    FreeDate = x.FreeDate.Date.ToDateString()
                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachinerySoftFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachinerySofts
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachinerySoftId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.FoldingNumber++;

            var newRenovation = new EquipmentMachinerySoftFolding()
            {

                EquipmentMachinerySoftId = model.EquipmentMachinerySoftId,
                FreeText = model.FreeText,
                FreeDate = model.FreeDate.ToDateTime()

            };


          

            await _context.EquipmentMachinerySoftFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachinerySoftFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTransport = await _context.EquipmentMachinerySofts
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachinerySoftId)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachinerySoftFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.FreeDate = model.FreeDate.ToDateTime();
            folding.FreeText = model.FreeText;
            

            
           



 



            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachinerySoftFoldings.FirstOrDefaultAsync(x => x.Id == id);

           

            var equipment = await _context.EquipmentMachinerySofts
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachinerySoftId);
            equipment.FoldingNumber--;

            _context.EquipmentMachinerySoftFoldings.Remove(renovation);

            await _context.SaveChangesAsync();


            return Ok();
        }
    }
}
