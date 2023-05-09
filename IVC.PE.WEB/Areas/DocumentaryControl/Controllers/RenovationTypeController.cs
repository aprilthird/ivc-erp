using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.RenovationTypeViewModels;
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
    [Route("control-documentario/tipo-de-renovacion")]
    public class RenovationTypeController : BaseController
    {
        public RenovationTypeController(IvcDbContext context,
ILogger<RenovationTypeController> logger)
: base(context, logger)
        {

        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.RenovationTypes
              .AsQueryable();

            //  query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new RenovationTypeViewModel
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
            var data = await _context.RenovationTypes
                .Where(x => x.Id == id)
                .Select(x => new RenovationTypeViewModel
                {
                    Id = x.Id,
                    Description = x.Description

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(RenovationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var renovationType = new RenovationType
            {
                Description = model.Description
            };
            await _context.RenovationTypes.AddAsync(renovationType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, RenovationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var renovationType = await _context.RenovationTypes.FindAsync(id);


            renovationType.Description = model.Description;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovationType = await _context.RenovationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (renovationType == null)
                return BadRequest($"Tipo de renovacion con Id '{id}' no encontrado.");
            _context.RenovationTypes.Remove(renovationType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
