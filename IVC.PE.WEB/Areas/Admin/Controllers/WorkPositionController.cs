using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkPositionViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/cargos")]
    public class WorkPositionController : BaseController
    {
        public WorkPositionController(IvcDbContext context,
              ILogger<WorkPositionController> logger)
              : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int type = -1)
        {
            var users = await _context.Users.ToListAsync();

            var query = _context.WorkPositions
                .Where(x => x.Name != null);

            if (type > -1)
                query = query.Where(x => x.Type == type);

            var positions = await query
                .Select(x=> new WorkPositionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type
                })
                .ToListAsync();

            foreach(var item in positions)
            {
                item.Cantidad = users.Where(x => x.WorkPositionId == item.Id).Count();
            }

            return Ok(positions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var position = await _context.WorkPositions
                .Select(x => new WorkPositionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type
                }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(position);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkPositionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var position = new WorkPosition
            {
                Name = model.Name,
                Type = model.Type
            };

            await _context.WorkPositions.AddAsync(position);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkPositionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var position = await _context.WorkPositions.FirstOrDefaultAsync(x => x.Id == id);

            position.Name = model.Name;
            position.Type = model.Type;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var position = await _context.WorkPositions.FirstOrDefaultAsync(x => x.Id == id);

            if (position == null)
                return BadRequest("No se ha encontrado el cargo");

            _context.WorkPositions.Remove(position);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
