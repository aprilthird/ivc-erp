using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/colaboradores")]
    public class ProjectCollaboratorController : BaseController
    {
        public ProjectCollaboratorController(IvcDbContext context,
            ILogger<ProjectCollaboratorController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ProjectCollaborators
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new ProjectCollaboratorViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    Provider = new ProviderViewModel
                    { 
                        Id = x.ProviderId,
                        BusinessName = x.Provider.BusinessName,
                        Tradename = x.Provider.Tradename
                    },
                    Project = new ProjectViewModel
                    {
                        Id = x.ProjectId,
                        Abbreviation = x.Project.Abbreviation
                    },
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var workFront = await _context.ProjectCollaborators
                .Where(x => x.Id == id)
                .Select(x => new ProjectCollaboratorViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    ProjectId = x.ProjectId,
                    ProviderId = x.ProviderId
                }).FirstOrDefaultAsync();
            return Ok(workFront);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectCollaboratorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var projectCollaborator = new ProjectCollaborator
            {
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                ProjectId = model.ProjectId,
                ProviderId = model.ProviderId
            };
            await _context.ProjectCollaborators.AddAsync(projectCollaborator);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProjectCollaboratorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var projectCollaborator = await _context.ProjectCollaborators.FindAsync(id);
            projectCollaborator.Name = model.Name;
            projectCollaborator.MaternalSurname = model.MaternalSurname;
            projectCollaborator.PaternalSurname = model.PaternalSurname;
            projectCollaborator.ProjectId = model.ProjectId;
            projectCollaborator.ProviderId = model.ProviderId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var projectCollaborator = await _context.ProjectCollaborators.FirstOrDefaultAsync(x => x.Id == id);
            if (projectCollaborator == null)
                return BadRequest($"Colaborador con Id '{id}' no encontrado.");
            _context.ProjectCollaborators.Remove(projectCollaborator);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "COLABORADORES");
                    var counter = 7;
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var collaborators = new List<ProjectCollaborator>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var projectCollaborator = new ProjectCollaborator();
                        projectCollaborator.PaternalSurname = workSheet.Cell($"C{counter}").GetString();
                        projectCollaborator.MaternalSurname = workSheet.Cell($"D{counter}").GetString();
                        projectCollaborator.Name = workSheet.Cell($"E{counter}").GetString();
                        projectCollaborator.ProjectId = project.Id;
                        var providerStr = workSheet.Cell($"B{counter}").GetString();
                        var provider = await _context.Providers.FirstOrDefaultAsync(x => x.BusinessName == providerStr);
                        projectCollaborator.ProviderId = provider?.Id;
                        collaborators.Add(projectCollaborator);
                        ++counter;
                    }
                    await _context.ProjectCollaborators.AddRangeAsync(collaborators);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }
    }
}