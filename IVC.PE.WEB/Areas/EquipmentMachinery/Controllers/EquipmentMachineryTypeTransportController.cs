﻿using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
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
    [Route("equipos/tipo-de-equipo-transporte")]

    public class EquipmentMachineryTypeTransportController : BaseController
    {
        public EquipmentMachineryTypeTransportController(IvcDbContext context,
        ILogger<EquipmentMachineryTypeTransportController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.EquipmentMachineryTypeTransports
              .AsQueryable();

            var data = await query
                .Select(x => new EquipmentMachineryTypeTransportViewModel
                {
                    Id = x.Id,
                    Description = x.Description,

                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachineryTypeTransports

                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentMachineryTypeTransportViewModel
                 {
                     Id = x.Id,
                     Description = x.Description
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTypeTransportViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryType = new EquipmentMachineryTypeTransport
            {

                Description = model.Description
            };

            await _context.EquipmentMachineryTypeTransports.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTypeTransportViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTypes = await _context.EquipmentMachineryTypeTransports
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentMachineryTypes.Description = model.Description;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.EquipmentMachineryTypeTransports
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Tipo de certificado con Id '{id}' no encontrado.");
            _context.EquipmentMachineryTypeTransports.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}