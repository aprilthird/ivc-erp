using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AuthorizationTypeViewModels;
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
    [Route("control-documentario/tipo-de-autorizacion")]
    public class AuthorizationTypeController : BaseController
    {
        public AuthorizationTypeController(IvcDbContext context,
       ILogger<AuthorizationTypeController> logger)
       : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.AuthorizationTypes
              .AsQueryable();

            //  query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new AuthorizationTypeViewModel
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
            var data = await _context.AuthorizationTypes
                .Where(x => x.Id == id)
                .Select(x => new AuthorizationTypeViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(AuthorizationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorizationType = new AuthorizationType
            {
                Description = model.Description
            };
            await _context.AuthorizationTypes.AddAsync(authorizationType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, AuthorizationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authorizationType = await _context.AuthorizationTypes.FindAsync(id);


            authorizationType.Description = model.Description;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authorizationType = await _context.AuthorizationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (authorizationType == null)
                return BadRequest($"Tipo de autorización con Id '{id}' no encontrado.");
            _context.AuthorizationTypes.Remove(authorizationType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
