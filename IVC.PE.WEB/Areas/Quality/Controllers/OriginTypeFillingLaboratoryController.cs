using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/procedencia-proctor")]
    public class OriginTypeFillingLaboratoryController : BaseController
    {
        public OriginTypeFillingLaboratoryController(IvcDbContext context,
            ILogger<OriginTypeFillingLaboratoryController> logger): base(context, logger)
        {

        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var pId = GetProjectId();

            var originsType = await _context.OriginTypeFillingLaboratories
                .Where(x => x.ProjectId == pId)
                .Select(x => new OriginTypeFillingLaboratoryViewModel
                {
                    Id = x.Id,
                    OriginTypeFLName = x.OriginTypeFLName,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    }
                }).ToListAsync();

            return Ok(originsType);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var originType = await _context.OriginTypeFillingLaboratories
                .Where(x => x.Id == id)
               .Select(x => new OriginTypeFillingLaboratoryViewModel
               {
                   Id = x.Id,
                   OriginTypeFLName = x.OriginTypeFLName,
                   Project = new ProjectViewModel
                   {
                       Abbreviation = x.Project.Abbreviation
                   }
               }).FirstOrDefaultAsync();

            return Ok(originType);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(OriginTypeFillingLaboratoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var originType = new OriginTypeFillingLaboratory
            {
                ProjectId = GetProjectId(),
                OriginTypeFLName = model.OriginTypeFLName
            };

            await _context.OriginTypeFillingLaboratories.AddAsync(originType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, OriginTypeFillingLaboratory model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var originType = await _context.OriginTypeFillingLaboratories
                .FirstOrDefaultAsync(x => x.Id == id);
            originType.OriginTypeFLName = model.OriginTypeFLName;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var originType = await _context.OriginTypeFillingLaboratories
                .FirstOrDefaultAsync(x => x.Id == id);

            if(originType == null)
                return BadRequest($"Procedencia con Id '{id}' no encontrado.");

            _context.OriginTypeFillingLaboratories.Remove(originType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
