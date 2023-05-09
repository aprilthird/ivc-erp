using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BluePrintResponsibleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/planos/responsables")]
    public class BluePrintResponsibleController : BaseController
    {       public BluePrintResponsibleController(IvcDbContext context,
                ILogger<BluePrintResponsibleController> logger) : base(context, logger)
            {
            }
    public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = await _context.Set<UspBluePrintResponsibles>().FromSqlRaw("execute TechnicalOffice_uspBluePrintResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();
            query = query.Where(x => x.ProjectId == GetProjectId()).ToList();
            return Ok(query);
        }

        [HttpGet("{id}/{formulaid}")]
        public async Task<IActionResult> Get(Guid id,Guid formulaid)
        {
            var query = await _context.BluePrintResponsibles
                .Where(x => x.ProjectId == id && x.ProjectFormulaId == formulaid)
                .ToListAsync();

            var data = new BluePrintResponsibleViewModel
            {
                ProjectFormulaId = query.FirstOrDefault().ProjectFormulaId,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BluePrintResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<BluePrintResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new BluePrintResponsible
                {
                    ProjectId = GetProjectId(),
                    ProjectFormulaId = model.ProjectFormulaId,
                    UserId = responsible
                });
            }

            await _context.BluePrintResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("editar")]
        public async Task<IActionResult> Edit(BluePrintResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.BluePrintResponsibles
                .Where(x => x.ProjectId == model.ProjectId && x.ProjectFormulaId == model.ProjectFormulaId)
                .ToListAsync();

            var responsibles = new List<BluePrintResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new BluePrintResponsible
                {
                    ProjectId = GetProjectId(),
                    ProjectFormulaId = model.ProjectFormulaId,
                    UserId = responsible
                });
            }

            _context.BluePrintResponsibles.RemoveRange(responsiblesDb);
            await _context.BluePrintResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}/{formulaid}")]
        public async Task<IActionResult> Delete(Guid id,Guid formulaid)
        {
            var responsiblesDb = await _context.BluePrintResponsibles
                .Where(x => x.ProjectId == id && x.ProjectFormulaId ==formulaid)
                .ToListAsync();

            _context.BluePrintResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
