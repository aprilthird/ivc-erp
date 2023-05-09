using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
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
    [Route("admin/jefes-de-frente")]
    public class WorkFrontHeadController : BaseController
    {
        public WorkFrontHeadController(IvcDbContext context,
            ILogger<WorkFrontHeadController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.WorkFrontHeads
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new WorkFrontHeadViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Project = new ProjectViewModel
                    {
                        Id = x.ProjectId,
                        Abbreviation = x.Project.Abbreviation
                    },
                    User = !string.IsNullOrEmpty(x.UserId)
                    ? new UserViewModel
                    {
                        Id = x.UserId,
                        Name = x.User.Name,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                        MiddleName = x.User.MiddleName,
                    } : null
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var workFront = await _context.WorkFrontHeads
                .Where(x => x.Id == id)
                .Select(x => new WorkFrontHeadViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    UserId = x.UserId,
                    ProjectId = x.ProjectId
                }).FirstOrDefaultAsync();
            return Ok(workFront);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkFrontHeadViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workFrontHead = new WorkFrontHead
            {
                Code = model.Code,
                UserId = model.UserId,
                ProjectId = model.ProjectId
            };
            await _context.WorkFrontHeads.AddAsync(workFrontHead);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkFrontHeadViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workFrontHead = await _context.WorkFrontHeads.FindAsync(id);
            workFrontHead.Code = model.Code;
            workFrontHead.UserId = model.UserId;
            workFrontHead.ProjectId = model.ProjectId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workFrontHead = await _context.WorkFrontHeads.FirstOrDefaultAsync(x => x.Id == id);
            if (workFrontHead == null)
                return BadRequest($"Jefe de frente con Id '{id}' no encontrado.");
            _context.WorkFrontHeads.Remove(workFrontHead);
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
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Contains("JEFE"));
                    var counter = 4;
                    // TODO: update project filter
                    var project = await _context.Projects.FirstOrDefaultAsync();
                    var workFrontHeads = new List<WorkFrontHead>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var workFrontHead = new WorkFrontHead();
                        var codeStr = workSheet.Cell($"A{counter}").GetString();
                        if (await _context.WorkFronts.AnyAsync(x => x.Code == codeStr))
                        {
                            ++counter;
                            continue;
                        }
                        workFrontHead.Code = codeStr;
                        var userStr = workSheet.Cell($"B{counter}").GetString();
                        if(!string.IsNullOrEmpty(userStr))
                        {
                            var user = await _context.Users
                                .Where(x => x.Name + " " + x.MiddleName + " " + x.PaternalSurname + " " + x.MaternalSurname == userStr)
                                .FirstOrDefaultAsync();
                            workFrontHead.UserId = user?.Id;
                        }
                        workFrontHead.ProjectId = project.Id;
                        workFrontHeads.Add(workFrontHead);
                        ++counter;
                    }

                    await _context.WorkFrontHeads.AddRangeAsync(workFrontHeads);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }
    }
}