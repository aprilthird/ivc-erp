using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.InterestGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/grupos-de-interes")]
    public class InterestGroupController : BaseController
    {
        public InterestGroupController(IvcDbContext context,
            ILogger<InterestGroupController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.InterestGroups
                .Select(x => new InterestGroupViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    }
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var project = await _context.InterestGroups
                .Where(x => x.Id == id)
                .Select(x => new InterestGroupViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ProjectId = x.ProjectId,
                    UserIds = x.UserInterestGroups.Select(y => y.UserId),
                    Users = x.UserInterestGroups.Select(y => new UserViewModel
                    {
                        Name = y.User.Name,
                        PaternalSurname = y.User.PaternalSurname,
                        MaternalSurname = y.User.MaternalSurname
                    })
                }).FirstOrDefaultAsync();
            return Ok(project);
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsersBy(Guid? projectId = null, int? workArea = null)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (projectId.HasValue)
                query = query.Where(x => x.UserProjects.Any(up => up.ProjectId == projectId.Value));
            if (workArea.HasValue)
                query = query.Where(x => x.WorkArea == workArea.Value);

            var data = await query.Select(x => x.Id).ToListAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(InterestGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var interestGroup = new InterestGroup
            {
                Code = model.Code,
                Name = model.Name,
                ProjectId = model.ProjectId
            };

            if (model.UserIds != null)
            {
                await _context.UserInterestGroups.AddRangeAsync(
                        model.UserIds.Select(x => new ApplicationUserInterestGroup
                        {
                            UserId = x,
                            InterestGroup = interestGroup,
                        }).ToList()); 
            }

            await _context.InterestGroups.AddAsync(interestGroup);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, InterestGroupViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var interestGroup = await _context.InterestGroups.FindAsync(id);
            interestGroup.Name = model.Name;
            interestGroup.Code = model.Code;
            interestGroup.ProjectId = model.ProjectId;

            var existingUsersInterestGroup = await _context.UserInterestGroups.Where(x => x.InterestGroupId.Equals(interestGroup.Id)).ToListAsync();
            if (existingUsersInterestGroup.Count > 0)
            {
                _context.UserInterestGroups.RemoveRange(existingUsersInterestGroup);
            }

            await _context.UserInterestGroups.AddRangeAsync(
                model.UserIds.Select(x => new ApplicationUserInterestGroup
                {
                    UserId = x,
                    InterestGroup = interestGroup,
                }).ToList());

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var interestGroup = await _context.InterestGroups
                .Include(x => x.UserInterestGroups)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (interestGroup == null)
                return BadRequest($"Grupo de Interés con Id '{id}' no encontrado.");
            _context.UserInterestGroups.RemoveRange(interestGroup.UserInterestGroups);
            _context.InterestGroups.Remove(interestGroup);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/usuarios/listar")]
        public async Task<IActionResult> GetUsers(Guid interestGroupId)
        {
            var data = await _context.Users
                .Where(x => x.UserInterestGroups.Any(u => u.InterestGroupId == interestGroupId))
                .Select(x => new UserViewModel
                {
                    Name = x.Name,
                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber
                }).ToListAsync();
            return Ok(data);
        }
    }
}