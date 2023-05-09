using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterResponsibleViewModels;
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
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/responsables")]
    public class LetterResponsibleController : BaseController
    {
        public LetterResponsibleController(IvcDbContext context,
        ILogger<LetterResponsibleController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Set<UspLetterResponsibles>().FromSqlRaw("execute DocumentaryControl_uspLetterResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.LetterResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var data = new LetterResponsibleViewModel
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

            var query = await _context.LetterResponsibles
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            var data = new LetterResponsibleViewModel
            {
                ProjectId = projectId,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(LetterResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<LetterResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new LetterResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            await _context.LetterResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();


            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(LetterResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.LetterResponsibles
                .Where(x => x.ProjectId == model.ProjectId)
                .ToListAsync();

            var responsibles = new List<LetterResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new LetterResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            _context.LetterResponsibles.RemoveRange(responsiblesDb);
            await _context.LetterResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();



            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responsiblesDb = await _context.LetterResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.LetterResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
