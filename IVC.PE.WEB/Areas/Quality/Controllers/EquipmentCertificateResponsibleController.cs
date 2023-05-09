using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.ENTITIES.UspModels.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.ENTITIES.Models.Quality;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/responsables")]
    public class EquipmentCertificateResponsibleController : BaseController
    {
        
        public EquipmentCertificateResponsibleController(IvcDbContext context,
                ILogger<EquipmentCertificateResponsibleController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Set<UspEquipmentResponsibles>().FromSqlRaw("execute Qualitiy_uspEquipmentResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.EquipmentResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var data = new EquipmentCertificateResponsibleViewModel
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

            var query = await _context.EquipmentResponsibles
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            var data = new EquipmentCertificateResponsibleViewModel
            {
                ProjectId = projectId,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertificateResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<EquipmentCertificateResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new EquipmentCertificateResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            await _context.EquipmentResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(EquipmentCertificateResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.EquipmentResponsibles
                .Where(x => x.ProjectId == model.ProjectId)
                .ToListAsync();

            var responsibles = new List<EquipmentCertificateResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new EquipmentCertificateResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            _context.EquipmentResponsibles.RemoveRange(responsiblesDb);
            await _context.EquipmentResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responsiblesDb = await _context.EquipmentResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.EquipmentResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
