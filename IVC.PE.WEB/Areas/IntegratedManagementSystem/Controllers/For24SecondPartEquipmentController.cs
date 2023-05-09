using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.IntegratedManagementSystem.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.INTEGRATED_MANAGEMENT_SYSTEM)]
    [Route("sistema-de-manejo-integrado/equipo-for24")]
    public class For24SecondPartEquipmentController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public For24SecondPartEquipmentController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<For24SecondPartEquipmentController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? SewerManifoldFor24SecondPartId = null)
        {
            if (!SewerManifoldFor24SecondPartId.HasValue)
                return Ok(new List<For24SecondPartEquipmentViewModel>());

            var equipments = await _context.For24SecondPartEquipments
                .Where(x =>x.SewerManifoldFor24SecondPartId == SewerManifoldFor24SecondPartId)
                .Select(x => new For24SecondPartEquipmentViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor24SecondPartId = x.SewerManifoldFor24SecondPartId,
                    EquipmentName = x.EquipmentName,
                    EquipmentQuantity = x.EquipmentQuantity,
                    EquipmentHours = x.EquipmentHours.ToString(),
                    EquipmentTotalHours = x.EquipmentTotalHours.ToString()
                }).ToListAsync();

            return Ok(equipments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var equipment = await _context.For24SecondPartEquipments
                .Where(x => x.Id == id)
                .Select(x => new For24SecondPartEquipmentViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor24SecondPartId = x.SewerManifoldFor24SecondPartId,
                    EquipmentName = x.EquipmentName,
                    EquipmentQuantity = x.EquipmentQuantity,
                    EquipmentHours = x.EquipmentHours.ToString(),
                    EquipmentTotalHours = x.EquipmentTotalHours.ToString()
                }).FirstOrDefaultAsync();

            return Ok(equipment);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(For24SecondPartEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipment = new For24SecondPartEquipment
            {
                SewerManifoldFor24SecondPartId = model.SewerManifoldFor24SecondPartId,
                EquipmentName = model.EquipmentName,
                EquipmentQuantity = model.EquipmentQuantity,
                EquipmentHours = model.EquipmentHours.ToDoubleString(),
                EquipmentTotalHours = model.EquipmentTotalHours.ToDoubleString()
            };

            await _context.For24SecondPartEquipments.AddAsync(equipment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipment = await _context.For24SecondPartEquipments
                .FirstOrDefaultAsync(x => x.Id == id);

            if (equipment == null)
                return BadRequest("Equipo no encontrado");

            _context.For24SecondPartEquipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
