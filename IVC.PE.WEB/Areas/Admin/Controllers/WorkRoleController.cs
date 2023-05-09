using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkRoleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
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
    [Route("admin/roles-de-trabajo")]
    public class WorkRoleController : BaseController
    {
        public WorkRoleController(IvcDbContext context, ILogger<WorkRoleController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.WorkRoles.Select(
                x => new WorkRoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var workRole = await _context.WorkRoles
                .Where(x => x.Id == id)
                .Select(x => new WorkRoleViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(workRole);
        }

        [HttpGet("{id}/permisos")]
        public async Task<IActionResult> GetPermissions(Guid id)
        {
            //var workRoles = await _context.WorkRoles.ToListAsync();
            //var wr1 = workRoles.Where(w => w.Name == "Capacitador General").FirstOrDefault();
            //var wr2 = workRoles.Where(w => w.Name == "Capacitador Asistente").FirstOrDefault();

            //var users = await _context.Users.Where(x => x.Email == "luisefc.97@gmail.com" || x.Email == "jeffreyrm96@gmail.com").ToListAsync();
            //users[0].WorkRoleId = wr1.Id;
            //users[1].WorkRoleId = wr2.Id;
            //await _context.SaveChangesAsync();

            //var workRoleItems = new List<WorkRoleItem>();
            //var items = await _context.WorkAreaItems.ToListAsync();
            //foreach(var item in items)
            //{
            //    workRoleItems.Add(new WorkRoleItem { WorkRoleId = wr1.Id, WorkAreaItemId = item.Id });
            //    if(item.Controller == "TrainingSession" || item.Controller == "TrainingControl")
            //        workRoleItems.Add(new WorkRoleItem { WorkRoleId = wr2.Id, WorkAreaItemId = item.Id });
            //}
            //await _context.WorkRoleItems.AddRangeAsync(workRoleItems);
            //await _context.SaveChangesAsync();

            var model = new MenuSelectorViewModel
            {
                Areas = await _context.WorkAreas
                    .Select(x => new AreaSelectorViewModel
                    {
                        Name = x.Name,
                        IsChecked = x.WorkAreaItems.Any(wa => wa.WorkRoleItems.Any(wr => wr.WorkRoleId == id)),
                        Items = x.WorkAreaItems.Where(i => i.ParentId == null).Select(i => new ItemSelectorViewModel
                        {
                            Id = i.Id,
                            Name = i.Name,
                            IsChecked = i.WorkRoleItems.Any(wr => wr.WorkRoleId == id),
                            PermissionLevel = i.WorkRoleItems.Where(wr => wr.WorkRoleId == id).Select(wr => wr.PermissionLevel).FirstOrDefault(),
                            Items = i.IsItemGroup ? i.WorkAreaItems.Select(i2 => new ItemSelectorViewModel
                            {
                                Id = i2.Id,
                                Name = i2.Name,
                                IsChecked = i2.WorkRoleItems.Any(wr => wr.WorkRoleId == id),
                                PermissionLevel = i2.WorkRoleItems.Where(wr => wr.WorkRoleId == id).Select(wr => wr.PermissionLevel).FirstOrDefault(),
                                Items = i2.IsItemGroup ? i2.WorkAreaItems.Select(i3 => new ItemSelectorViewModel
                                {
                                    Id = i3.Id,
                                    Name = i3.Name,
                                    IsChecked = i3.WorkRoleItems.Any(wr => wr.WorkRoleId == id),
                                    PermissionLevel = i3.WorkRoleItems.Where(wr => wr.WorkRoleId == id).Select(wr => wr.PermissionLevel).FirstOrDefault(),
                                }).ToList() : new List<ItemSelectorViewModel>()
                            }).ToList() : new List<ItemSelectorViewModel>()
                        }).ToList()
                    }).ToListAsync()
            };

            return PartialView("_MenuSelector", model);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workRole = new WorkRole
            {
                Name = model.Name,
            };
            await _context.WorkRoles.AddAsync(workRole);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var workRole = await _context.WorkRoles.FindAsync(id);
            workRole.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{id}/permisos")]
        public async Task<IActionResult> EditPermissions(Guid id, MenuSelectorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workRole = await _context.WorkRoles
                .Include(x => x.WorkRoleItems)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (workRole is null)
                return BadRequest($"No se encontró un rol de trabajo con el Id '{id}'.");

            var newWorkRoleItems = model.Areas
                .SelectMany(x => x.Items.Where(i => i.IsChecked)
                    .Select(i => new WorkRoleItem
                    {
                        WorkRoleId = id,
                        WorkAreaItemId = i.Id,
                        PermissionLevel = i.PermissionLevel
                    }).ToList()
                ).ToList();

            if (workRole.WorkRoleItems.Any())
                _context.RemoveRange(workRole.WorkRoleItems);

            await _context.WorkRoleItems.AddRangeAsync(newWorkRoleItems);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workRole = await _context.WorkRoles.FindAsync(id);
            if (workRole is null)
                return BadRequest($"Rol de Trabajo con Id '{id}' no encontrado.");
            _context.WorkRoles.Remove(workRole);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
