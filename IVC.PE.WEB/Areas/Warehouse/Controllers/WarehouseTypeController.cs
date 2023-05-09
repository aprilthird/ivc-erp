using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseTypeViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/tipos-de-almacen")]
    public class WarehouseTypeController : BaseController
    {
        public WarehouseTypeController(IvcDbContext context,
            ILogger<WarehouseTypeController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var types = await _context.WarehouseTypes
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new WarehouseTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();

            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var type = await _context.WarehouseTypes
                .Select(x=> new WarehouseTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(type);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WarehouseTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var type = new WarehouseType
            {
                Name = model.Name,
                ProjectId = GetProjectId()
            };

            await _context.WarehouseTypes.AddAsync(type);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WarehouseTypeViewModel model)
        {
            var type = await _context.WarehouseTypes.FirstOrDefaultAsync(x => x.Id == id);

            type.Name = model.Name;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var type = await _context.WarehouseTypes.FirstOrDefaultAsync(x => x.Id == id);

            if(type == null)
                return BadRequest("No se encontró el tipo de Almacén");

            _context.WarehouseTypes.Remove(type);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
