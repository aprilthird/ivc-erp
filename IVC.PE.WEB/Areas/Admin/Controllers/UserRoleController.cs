using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserRolViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/roles")]
    public class UserRoleController : BaseController
    {
        public UserRoleController(IvcDbContext context,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<UserRoleController> logger)
            : base(context, userManager, roleManager, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.UserRoles.ToListAsync();

            var roles = await _context.Roles
                .Select(x => new UserRoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName
                })
                .ToListAsync();

            foreach(var item in roles)
            {
                item.Cantidad = users.Where(x => x.RoleId == item.Id).Count();
            }

            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _context.Roles
                .Select(x => new UserRoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(role);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(UserRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = new ApplicationRole
            {
                Name = model.Name,
                NormalizedName = model.Name.RemoveAccentMarks(),
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            role.Name = model.Name;
            role.NormalizedName = model.Name.RemoveAccentMarks();

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
