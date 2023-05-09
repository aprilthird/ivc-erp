using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionProjectResponsibleViewModels;
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
    [Route("control-documentario/responsables-de-permisos")]
    public class PermissionProjectResponsibleController : BaseController
    {
        public PermissionProjectResponsibleController(IvcDbContext context,
      ILogger<PermissionProjectResponsibleController> logger)
      : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Set<UspPermissionProjectResponsible>().FromSqlRaw("execute DocumentaryControl_uspPermissionProjectResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.PermissionProjectResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var data = new PermissionProjectResponsibleViewModel
            {
                ProjectId = id,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpGet("proyecto")]
        public async Task<IActionResult> GetByProject()
        {
            var projectId = GetProjectId();

            var query = await _context.PermissionProjectResponsibles
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            var data = new PermissionProjectResponsibleViewModel
            {
                ProjectId = projectId,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PermissionProjectResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<PermissionProjectResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new PermissionProjectResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            await _context.PermissionProjectResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(PermissionProjectResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.PermissionProjectResponsibles
                .Where(x => x.ProjectId == model.ProjectId)
                .ToListAsync();

            var responsibles = new List<PermissionProjectResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new PermissionProjectResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            _context.PermissionProjectResponsibles.RemoveRange(responsiblesDb);
            await _context.PermissionProjectResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responsiblesDb = await _context.PermissionProjectResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.PermissionProjectResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
