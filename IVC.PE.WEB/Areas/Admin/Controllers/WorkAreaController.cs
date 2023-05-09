using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserRolViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaViewModels;
using System.Globalization;


namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/area-trabajo")]
    public class WorkAreaController : BaseController
    {
        public WorkAreaController(IvcDbContext context, ILogger<WorkAreaController> logger) 
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _context.WorkAreas.Select(
                x => new WorkAreaViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    IntValue = x.IntValue,
                    Cantidad = x.Users.Count()
                }).ToListAsync();

            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var area = await _context.WorkAreas
                .Select(x => new WorkAreaViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    IntValue=x.IntValue,
                }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(area);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkAreaViewModel model)
        {
            var count = _context.WorkAreas.Count();
        
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var area = new WorkArea
            {
                Name = model.Name,
                NormalizedName = model.Name.RemoveAccentMarks().ToUpper(),
                IntValue = count + 1
            }; 

            await _context.WorkAreas.AddAsync(area);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkAreaViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var area = await _context.WorkAreas.FirstOrDefaultAsync(x => x.Id == id);

            area.Name = model.Name;
            area.NormalizedName = model.Name.RemoveAccentMarks().ToUpper();
            
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var area = await _context.WorkAreas.FirstOrDefaultAsync(x => x.Id == id);

            if (area == null)
                return BadRequest("No se ha encontrado el área de trabajo");

            _context.WorkAreas.Remove(area);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

