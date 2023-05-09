﻿using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.InsuranceEntityViewModels;
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
    [Route("equipos/entidad-aseguradora")]
    public class InsuranceEntityController : BaseController
    {
        public InsuranceEntityController(IvcDbContext context,
    ILogger<InsuranceEntityController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.InsuranceEntity
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Where(x => x.ProjectId == pId)
                .Select(x => new InsuranceEntityViewModel
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
            var data = await _context.InsuranceEntity

                 .Where(x => x.Id == id)
                 .Select(x => new InsuranceEntityViewModel
                 {
                     Id = x.Id,
                     Description = x.Description,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(InsuranceEntityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryType = new InsuranceEntity
            {
                ProjectId = GetProjectId(),
                Description = model.Description,
            };

            await _context.InsuranceEntity.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, InsuranceEntityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTypes = await _context.InsuranceEntity
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentMachineryTypes.Description = model.Description;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.InsuranceEntity
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Entidad con Id '{id}' no encontrado.");
            _context.InsuranceEntity.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
