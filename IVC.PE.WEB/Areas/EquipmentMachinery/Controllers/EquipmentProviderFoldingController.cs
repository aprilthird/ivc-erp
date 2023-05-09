using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
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
    [Route("equipos/proveedores-de-equipos-folding")]
    public class EquipmentProviderFoldingController : BaseController
    {
        public EquipmentProviderFoldingController(IvcDbContext context,
ILogger<EquipmentProviderFoldingController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentProviderId = null,Guid? classEquip = null, Guid? providerId = null, Guid? equipmentType = null)
        {
            if (!equipmentProviderId.HasValue)
                return Ok(new List<EquipmentProviderFoldingViewModel>());

            var softs = _context.EquipmentMachineryTypeSofts
.AsQueryable();

            var types = _context.EquipmentMachineryTypeTypes
                .AsQueryable();

            var transports = _context.EquipmentMachineryTypeTransports
                                .AsQueryable();

            var folding = await _context.EquipmentProviderFoldings
                .Include(x => x.EquipmentProvider)
                .Include(x => x.EquipmentMachineryTypeType)
                .Include(x => x.EquipmentMachineryTypeSoft)
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Include(x => x.EquipmentMachineryType)
                .Where(x => x.EquipmentProviderId == equipmentProviderId)
                .Select(x => new EquipmentProviderFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentMachineryTypeId = x.EquipmentMachineryTypeId,
                    EquipmentMachineryType = new EquipmentMachineryTypeViewModel
                    {
                        Description = x.EquipmentMachineryType.Description
                    },
                    EquipmentMachineryTypeSoftId = x.EquipmentMachineryTypeSoftId,
                    EquipmentMachineryTypeSoft = new EquipmentMachineryTypeSoftViewModel
                    {
                        Description = x.EquipmentMachineryTypeSoft.Description
                    },
                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId,
                    EquipmentMachineryTypeType = new EquipmentMachineryTypeTypeViewModel
                    {
                        Description = x.EquipmentMachineryTypeType.Description
                    },
                    EquipmentMachineryTypeTransportId = x.EquipmentMachineryTypeTransportId,
                    EquipmentMachineryTypeTransport = new EquipmentMachineryTypeTransportViewModel
                    {
                        Description = x.EquipmentMachineryTypeTransport.Description
                    },
                }).ToListAsync();

            if (providerId.HasValue)
                folding = folding.Where(x => x.EquipmentProviderId == equipmentProviderId.Value).ToList();

            if (classEquip.HasValue)
                folding = folding.Where(x => x.EquipmentMachineryTypeId == classEquip.Value).ToList();

            if (equipmentType.HasValue)
            {

                if (types.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    folding = folding.Where(x => x.EquipmentMachineryTypeTypeId == equipmentType.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
                }
                else if (softs.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    folding = folding.Where(x => x.EquipmentMachineryTypeSoftId == equipmentType.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeTypeId == null).ToList();
                }
                else if (transports.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    folding = folding.Where(x => x.EquipmentMachineryTypeTransportId == equipmentType.Value && x.EquipmentMachineryTypeTypeId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
                }
            }


            return Ok(folding);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var equip = await _context.EquipmentProviderFoldings
                .Where(x => x.Id == id)
                .Select(x => new EquipmentProviderFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentMachineryTypeId = x.EquipmentMachineryTypeId,
                    EquipmentMachineryTypeSoftId = x.EquipmentMachineryTypeSoftId,
                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId,
                    EquipmentMachineryTypeTransportId = x.EquipmentMachineryTypeTransportId,
                }).FirstOrDefaultAsync();

            return Ok(equip);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentProviderFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipTypeSoft = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Equipos Menores");

            var equipTypeType = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Maquinaria");

            var equipTransport = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Transporte");

            var equip = new EquipmentProviderFolding
            {
                EquipmentProviderId = model.EquipmentProviderId,
                EquipmentMachineryTypeId = model.EquipmentMachineryTypeId,
                EquipmentMachineryTypeSoftId = model.EquipmentMachineryTypeId == equipTypeSoft.Id
                                            ? model.EquipmentMachineryTypeSoftId : null,
                EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeId == equipTypeType.Id
                                            ? model.EquipmentMachineryTypeTypeId : null,
                EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeId == equipTransport.Id
                                            ? model.EquipmentMachineryTypeTransportId : null,


            };

            await _context.EquipmentProviderFoldings.AddAsync(equip);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentProviderFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipTypeSoft = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Equipos Menores");

            var equipTypeType = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Maquinaria");

            var equipTransport = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Transporte");

            var equipment = await _context.EquipmentProviderFoldings
                .FirstOrDefaultAsync(x => x.Id == id);

            equipment.EquipmentMachineryTypeId = model.EquipmentMachineryTypeId;

            equipment.EquipmentMachineryTypeSoftId = model.EquipmentMachineryTypeId == equipTypeSoft.Id
                            ? model.EquipmentMachineryTypeSoftId : null;
            equipment.EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeId == equipTypeType.Id
                                         ? model.EquipmentMachineryTypeTypeId : null;

            equipment.EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeId == equipTransport.Id
                                         ? model.EquipmentMachineryTypeTransportId : null;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.EquipmentProviderFoldings.FirstOrDefaultAsync(x => x.Id == id);

            _context.EquipmentProviderFoldings.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
