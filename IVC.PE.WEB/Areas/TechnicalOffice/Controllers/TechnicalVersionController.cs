using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
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
    [Route("oficina-tecnica/version")]
    public class TechnicalVersionController : BaseController
    {
        public TechnicalVersionController(IvcDbContext context,
    ILogger<TechnicalVersionController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.TechnicalVersions
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Where(x=>x.ProjectId.Value == pId)
                .Select(x => new TechnicalVersionViewModel
                {
                    Id = x.Id,
                    Description = x.Description,

                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.TechnicalVersions

                 .Where(x => x.Id == id)
                 .Select(x => new TechnicalVersionViewModel
                 {
                     Id = x.Id,
                     Description = x.Description,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TechnicalVersionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryType = new TechnicalVersion
            {

                Description = model.Description,
                ProjectId = GetProjectId()
            };

            await _context.TechnicalVersions.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TechnicalVersionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTypes = await _context.TechnicalVersions
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentMachineryTypes.Description = model.Description;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.TechnicalVersions
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Especialidad con Id '{id}' no encontrado.");
            _context.TechnicalVersions.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
