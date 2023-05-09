using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
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
    [Route("admin/cuadrillas/periodos")]
    public class SewerGroupPeriodController : BaseController
    {
        public SewerGroupPeriodController(IvcDbContext context,
            ILogger<SewerGroupPeriodController> logger)
            : base(context, logger)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? sewerGroupId = null)
        {
            if (!sewerGroupId.HasValue)
                return Ok(new List<SewerGroupPeriodViewModel>());

            var data = await _context.SewerGroupPeriods
                .Include(x => x.WorkFrontHead)
                .Include(x => x.WorkFrontHead.User)
                .Where(x => x.SewerGroupId == sewerGroupId.Value)
                .Select(x => new SewerGroupPeriodViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    //WorkFrontId = x.WorkFrontId,
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    WorkFrontHead = new WorkFrontHeadViewModel
                    {
                        User = new UserViewModel
                        {
                            PaternalSurname = x.WorkFrontHead.User.PaternalSurname,
                            MaternalSurname = x.WorkFrontHead.User.MaternalSurname,
                            Name = x.WorkFrontHead.User.Name
                        }
                    },
                    Destination = x.Destination,
                    WorkComponent = x.WorkComponent,
                    WorkStructure = x.WorkStructure,
                    ProviderId = x.ProviderId,
                    ProjectCollaboratorId = x.ProjectCollaboratorId,
                    ForemanId = x.ForemanEmployeeId ?? x.ForemanWorkerId,
                    DateStart = x.DateStart.ToDateString(),
                    DateEnd = x.DateEnd.HasValue ? x.DateEnd.Value.ToDateString() : string.Empty
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.SewerGroupPeriods
                .Where(x => x.Id == id)
                .Select(x => new SewerGroupPeriodViewModel
                {
                    Id = x.Id,
                    SewerGroupId = x.SewerGroupId,
                    //WorkFrontId = x.WorkFrontId,
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    Destination = x.Destination,
                    WorkComponent = x.WorkComponent,
                    WorkStructure = x.WorkStructure,
                    ProviderId = x.ProviderId,
                    ProjectCollaboratorId = x.ProjectCollaboratorId,
                    ForemanId = x.ForemanEmployeeId ?? x.ForemanWorkerId,
                    DateStart = x.DateStart.ToDateString(),
                    DateEnd = x.DateEnd.HasValue ? x.DateEnd.Value.ToDateString() : string.Empty
                }).FirstOrDefaultAsync();

            data.WorkFrontIds = await _context.WorkFrontSewerGroups
                .Where(x => x.SewerGroupPeriodId == id)
                .Select(x => x.WorkFrontId)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SewerGroupPeriodViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lastPeriod = await _context.SewerGroupPeriods
                .FirstOrDefaultAsync(x => x.Id == model.Id.Value);

            var period = new SewerGroupPeriod
            {
                SewerGroupId = model.SewerGroupId,
                //WorkFrontId = model.WorkFrontId,
                WorkFrontHeadId = model.WorkFrontHeadId,
                Destination = model.Destination,
                WorkComponent = model.WorkComponent,
                WorkStructure = model.WorkStructure,
                DateStart = model.DateStart.ToDateTime()
            };

            if (lastPeriod.DateEnd.Value.Date >= period.DateStart.Date)
                return BadRequest("Fecha de Inicio de la nueva vigencia, se suporpone con otra vigencia.");

            if (!string.IsNullOrEmpty(model.DateEnd))
                period.DateEnd = model.DateEnd.ToDateTime();
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.COLLABORATOR)
            {
                period.ProviderId = model.ProviderId;
                period.ProjectCollaboratorId = model.ProjectCollaboratorId;
            }                
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.LOCAL)
            {
                if (await _context.Workers.AnyAsync(x => x.Id == model.ForemanId))
                    period.ForemanWorkerId = model.ForemanId;
                else if (await _context.Employees.AnyAsync(x => x.Id == model.ForemanId))
                    period.ForemanEmployeeId = model.ForemanId;
            }

            if (model.WorkFrontIds != null)
                await _context.WorkFrontSewerGroups.AddRangeAsync(
                    model.WorkFrontIds.Select(x => new ENTITIES.Models.General.WorkFrontSewerGroup
                    {
                        SewerGroupPeriod = period,
                        WorkFrontId = x
                    }).ToList()
                );
            await _context.SewerGroupPeriods.AddAsync(period);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SewerGroupPeriodViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var period = await _context.SewerGroupPeriods.FirstOrDefaultAsync(x => x.Id == id);

            //period.WorkFrontId = model.WorkFrontId;
            period.WorkFrontHeadId = model.WorkFrontHeadId;
            period.Destination = model.Destination;
            period.WorkComponent = model.WorkComponent;
            period.WorkStructure = model.WorkStructure;
            period.DateStart = model.DateStart.ToDateTime();

            period.DateEnd = null;
            if (!string.IsNullOrEmpty(model.DateEnd))
                period.DateEnd = model.DateEnd.ToDateTime();

            period.ProviderId = null;
            period.ProjectCollaboratorId = null;
            period.ForemanWorkerId = null;
            period.ForemanEmployeeId = null;
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.COLLABORATOR)
            {
                period.ProviderId = model.ProviderId;
                period.ProjectCollaboratorId = model.ProjectCollaboratorId;
            }
            if (period.Destination == ConstantHelpers.Sewer.Group.Destination.LOCAL)
            {
                if (await _context.Workers.AnyAsync(x => x.Id == model.ForemanId))
                    period.ForemanWorkerId = model.ForemanId;
                else if (await _context.Employees.AnyAsync(x => x.Id == model.ForemanId))
                    period.ForemanEmployeeId = model.ForemanId;
            }

            var workfronts = await _context.WorkFrontSewerGroups
                .Where(x => x.SewerGroupPeriodId == id)
                .ToListAsync();
            _context.WorkFrontSewerGroups.RemoveRange(workfronts);
            if (model.WorkFrontIds != null)
                await _context.WorkFrontSewerGroups.AddRangeAsync(
                    model.WorkFrontIds.Select(x => new ENTITIES.Models.General.WorkFrontSewerGroup
                    {
                        SewerGroupPeriodId = id,
                        WorkFrontId = x
                    }).ToList()
                );

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var period = await _context.SewerGroupPeriods.FindAsync(id);
            var workfronts = await _context.WorkFrontSewerGroups
                .Where(x => x.SewerGroupPeriodId == id)
                .ToListAsync();
            if (workfronts.Count > 0)
                _context.WorkFrontSewerGroups.RemoveRange(workfronts);
            _context.SewerGroupPeriods.Remove(period);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
