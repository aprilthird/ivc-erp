using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcessViewModels;
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
    [Route("oficina-tecnica/proceso")]
    public class ProcessController : BaseController
    {
        public ProcessController(IvcDbContext context,
    ILogger<ProcessController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.Processes
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Where(x => x.ProjectId.Value == pId)
                .Select(x => new ProcessViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId.Value,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    ProcessName = x.ProcessName,
                    Code = x.Code


                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Processes

                 .Where(x => x.Id == id)
                 .Select(x => new ProcessViewModel
                 {
                     Id = x.Id,
                     UserId = x.UserId,
                     UserName = x.UserName,
                     ProcessName = x.ProcessName,
                     Code = x.Code
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            var equipmentMachineryType = new Process
            {

               
                ProjectId = GetProjectId(),
                UserId = model.UserId,
                UserName = users.FullName,
                ProcessName = model.ProcessName,
                Code = model.Code

            };

            await _context.Processes.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var process = await _context.Processes
                .FirstOrDefaultAsync(x => x.Id == id);

            process.ProcessName = model.ProcessName;
            process.Code = model.Code;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.Processes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Especialidad con Id '{id}' no encontrado.");
            _context.Processes.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
