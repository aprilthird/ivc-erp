using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaItemViewModels;
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
    [Route("admin/elementos-de-trabajo")]
    public class WorkAreaItemController : BaseController
    {
        public WorkAreaItemController(IvcDbContext context, ILogger<WorkAreaItemController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _context.WorkAreaItems.Select(
                x => new WorkAreaItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    Action = x.Action,
                    Controller = x.Controller,
                    WorkAreaId = x.WorkAreaId,
                    IsItemGroup = x.IsItemGroup,
                    WorkArea = new ViewModels.WorkAreaViewModels.WorkAreaViewModel
                    {
                        Name = x.WorkArea.Name
                    },
                    ParentId = x.ParentId,
                    Parent = x.ParentId.HasValue ? new WorkAreaItemViewModel
                    {
                        Name = x.Parent.Name
                    } : null,
                    RoleId = x.RoleId
                }).ToListAsync();

            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var area = await _context.WorkAreaItems
                .Select(x => new WorkAreaItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName,
                    IsItemGroup = x.IsItemGroup,
                    Action = x.Action,
                    Controller = x.Controller,
                    WorkAreaId = x.WorkAreaId,
                    ParentId = x.ParentId,
                    RoleId = x.RoleId
                }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(area);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkAreaItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = new WorkAreaItem
            {
                Name = model.Name,
                NormalizedName = model.Name.RemoveAccentMarks().ToUpper(),
                IsItemGroup = model.IsItemGroup,
                Action = model.Action,
                Controller = model.Controller,
                WorkAreaId = model.WorkAreaId,
                ParentId = model.ParentId,
                RoleId = model.RoleId
            };

            await _context.WorkAreaItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkAreaItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.WorkAreaItems.FirstOrDefaultAsync(x => x.Id == id);

            item.Name = model.Name;
            item.NormalizedName = model.Name.RemoveAccentMarks().ToUpper();
            item.IsItemGroup = model.IsItemGroup;
            item.Action = model.Action;
            item.Controller = model.Controller;
            item.WorkAreaId = model.WorkAreaId;
            item.ParentId = model.ParentId;
            item.RoleId = model.RoleId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.WorkAreaItems.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return BadRequest("No se ha encontrado el elemento de trabajo");

            _context.WorkAreaItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
