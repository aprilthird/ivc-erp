using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AuthorizingEntityViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/entidad-que-autoriza")]
    public class AuthorizingEntityController : BaseController
    {
        public AuthorizingEntityController(IvcDbContext context,
ILogger<AuthorizingEntityController> logger)
: base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.AuthorizingEntities
              .AsQueryable();

            //  query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new AuthorizingEntityViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.AuthorizingEntities
                .Where(x => x.Id == id)
                .Select(x => new AuthorizingEntityViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(AuthorizingEntityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorizingEntity = new AuthorizingEntity
            {
                Description = model.Description
            };
            await _context.AuthorizingEntities.AddAsync(authorizingEntity);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, AuthorizingEntityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorizingEntity = await _context.AuthorizingEntities.FindAsync(id);


            authorizingEntity.Description = model.Description;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authorizingEntity = await _context.AuthorizingEntities.FirstOrDefaultAsync(x => x.Id == id);
            if (authorizingEntity == null)
                return BadRequest($"Entidad que autoriza con Id '{id}' no encontrado.");
            _context.AuthorizingEntities.Remove(authorizingEntity);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
