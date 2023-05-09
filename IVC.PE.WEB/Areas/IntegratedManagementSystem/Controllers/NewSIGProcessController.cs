using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.NewSIGProcessViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.IntegratedManagementSystem.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.INTEGRATED_MANAGEMENT_SYSTEM)]
    [Route("sistema-de-manejo-integrado/procesos")]
    public class NewSIGProcessController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public NewSIGProcessController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<NewSIGProcessController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {
            var pId = GetProjectId();
            var processes = await _context.NewSIGProcesses
                .Where(x => x.ProjectId == pId)
                .Select(x => new NewSIGProcessViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    ProcessName = x.ProcessName,
                    Code = x.Code
                }).ToListAsync();

            return Ok(processes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var process = await _context.NewSIGProcesses
                .Where(x => x.Id == id)
                .Select(x => new NewSIGProcessViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    ProcessName = x.ProcessName,
                    Code = x.Code
                }).FirstOrDefaultAsync();

            return Ok(process);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(NewSIGProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var process = new NewSIGProcess
            {
                ProjectId = pId,
                UserId = model.UserId,
                UserName = users.FullName,
                ProcessName = model.ProcessName,
                Code = model.Code
            };

            await _context.NewSIGProcesses.AddAsync(process);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, NewSIGProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var process = await _context.NewSIGProcesses
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            process.ProcessName = model.ProcessName;
            process.Code = model.Code;
            process.UserId = model.UserId;
            process.UserName = users.FullName;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var process = await _context.NewSIGProcesses
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (process == null)
                return BadRequest($"Proceso con Id '{id}' no se halló");

            _context.NewSIGProcesses.Remove(process);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
