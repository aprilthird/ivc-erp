using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.MachineryPhaseViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-equipo-maquinaria-folding")]
    public class EquipmentMachPartFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachPartFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachPartFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid softPartId, int? week = null)
        {
            if (softPartId == Guid.Empty)
                return Ok(new List<EquipmentMachPartViewModel>());

            var renovations = await _context.EquipmentMachPartFoldings
                .Include(x => x.EquipmentMachPart)
                .Include(x => x.EquipmentMachineryOperator)
                .Include(x => x.EquipmentMachineryOperator.Worker)
                .Include(x => x.EquipmentMachineryTypeTypeActivity)
                .Include(x => x.SewerGroup)
                .Include(x => x.MachineryPhase)
                .Where(x => x.EquipmentMachPartId == softPartId)
                .Select(x => new EquipmentMachPartFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachPartId = x.EquipmentMachPartId,
                    EquipmentMachPart = new EquipmentMachPartViewModel
                    {
                        EquipmentMachineryTypeTypeId = x.EquipmentMachPart.EquipmentMachineryTypeTypeId,
                        EquipmentProviderId = x.EquipmentMachPart.EquipmentProviderId,
                        EquipmentProvider = new EquipmentProviderViewModel
                        {
                            ProviderId = x.EquipmentMachPart.EquipmentProvider.ProviderId,
                        }

                    },

                    PartDate = x.PartDate.Date.ToDateString(),
                    PartNumber = x.PartNumber,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                    EquipmentMachineryOperator = new EquipmentMachineryOperatorViewModel
                    {
                        ActualName = x.EquipmentMachineryOperator.ActualName,
                        WorkerId = x.EquipmentMachineryOperator.WorkerId,

                        Worker = x.EquipmentMachineryOperator.WorkerId != null ? new WorkerViewModel
                        {

                            Name = x.EquipmentMachineryOperator.Worker.Name,
                            MiddleName = x.EquipmentMachineryOperator.Worker.MiddleName,
                            PaternalSurname = x.EquipmentMachineryOperator.Worker.PaternalSurname,
                            MaternalSurname = x.EquipmentMachineryOperator.Worker.MaternalSurname,
                            PhoneNumber = x.EquipmentMachineryOperator.Worker.PhoneNumber,
                            Document = x.EquipmentMachineryOperator.Worker.Document,
                            DocumentType = x.EquipmentMachineryOperator.Worker.DocumentType

                        } : null,

                    },
                    InitHorometer = x.InitHorometer,
                    EndHorometer = x.EndHorometer,
                    Dif = x.Dif,
                    Specific = x.Specific,

                    UserId = x.UserId,
                    UserName = x.UserName,
                    WorkArea = x.WorkArea.Value,
                    EquipmentMachineryTypeTypeActivityId = x.EquipmentMachineryTypeTypeActivityId,
                    EquipmentMachineryTypeTypeActivity = new EquipmentMachineryTypeTypeActivityViewModel
                    {
                        Description = x.EquipmentMachineryTypeTypeActivity.Description

                    },

                    SewerGroupId = x.SewerGroupId,
                    SewerGroup = new SewerGroupViewModel
                    {

                        Name = x.SewerGroup.Code

                    },
                   Order = x.Order,
                    MachineryPhaseId = x.MachineryPhaseId,
                    MachineryPhase = new MachineryPhaseViewModel
                    {
                        ProjectPhaseId = x.MachineryPhase.ProjectPhaseId,
                        ProjectPhase = new ProjectPhaseViewModel
                        {
                            Code = x.MachineryPhase.ProjectPhase.Code
                        }
                    }
                })
                .OrderBy(x => x.Order)
                .ToListAsync();


           
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            if (week.HasValue)
            {
                renovations = renovations.Where(x => cal.GetWeekOfYear(x.PartDate.ToDateTime(), dfi.CalendarWeekRule, DayOfWeek.Monday) - 1 == week.Value).ToList();
            }
           

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachPartFoldings
                .Include(x => x.EquipmentMachPart)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachPartFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachPartId = x.EquipmentMachPartId,

                    PartDate = x.PartDate.Date.ToDateString(),
                    PartNumber = x.PartNumber,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,

                    InitHorometer = x.InitHorometer,
                    EndHorometer = x.EndHorometer,
                    Dif = x.Dif,
                    Specific = x.Specific,

                    UserId = x.UserId,
                    UserName = x.UserName,
                    WorkArea = x.WorkArea.Value,
                    SewerGroupId = x.SewerGroupId,
                    EquipmentMachineryTypeTypeActivityId = x.EquipmentMachineryTypeTypeActivityId,
                    MachineryPhaseId = x.MachineryPhaseId
                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            var dates = await _context.ProjectCalendarWeeks.Include(x=>x.ProjectCalendar).Where(x=>x.ProjectCalendar.ProjectId == GetProjectId()).FirstOrDefaultAsync(x => x.WeekStart <= model.PartDate.ToDateTime() && x.WeekEnd >= model.PartDate.ToDateTime() || x.WeekEnd == model.PartDate.ToDateTime() || x.WeekStart == model.PartDate.ToDateTime());
            var tempusers = new List<IVC.PE.ENTITIES.Models.General.ApplicationUser>
            {
                new IVC.PE.ENTITIES.Models.General.ApplicationUser
                {
                    Name = "",
                    PaternalSurname = "",
                    MaternalSurname = "",
                    WorkArea = 0

                }
            };

            if (users == null)
                users = tempusers.FirstOrDefault();
            var equipmentMachPart = await _context.EquipmentMachParts.Where(x => x.Id == model.EquipmentMachPartId).FirstOrDefaultAsync();
            equipmentMachPart.FoldingNumber++;
            var last = await _context.EquipmentMachPartFoldings.OrderByDescending(x=>x.Order).FirstOrDefaultAsync();
            var sewers = await _context.SewerGroups.Where(x => x.Id == model.SewerGroupId).FirstOrDefaultAsync();

            var acts = await _context.EquipmentMachineryTypeTypeActivities.Where(x => x.Id == model.EquipmentMachineryTypeTypeActivityId).FirstOrDefaultAsync();

            var phases = await _context.MachineryPhases.Include(x=>x.ProjectPhase).Where(x => x.Id == model.MachineryPhaseId).FirstOrDefaultAsync();

            var newRenovation = new EquipmentMachPartFolding()
            {
                EquipmentMachPartId = model.EquipmentMachPartId,
                PartDate = model.PartDate.ToDateTime(),
                PartNumber = model.PartNumber,
                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                InitHorometer = model.InitHorometer,
                EndHorometer = model.EndHorometer,
                Dif = model.EndHorometer - model.InitHorometer,
                Specific = model.Specific,

                UserId = model.UserId,
                UserName = model.UserId != null ? users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname : null,
                WorkArea = model.UserId != null ? users.WorkArea : 0,

                EquipmentMachineryTypeTypeActivityId = model.EquipmentMachineryTypeTypeActivityId,
                SewerGroupId = model.SewerGroupId,
                MachineryPhaseId = model.MachineryPhaseId.Value,
                Order = equipmentMachPart.FoldingNumber,
                ProjectCalendarWeekId = dates.Id
            };



            await _context.EquipmentMachPartFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            var dates = await _context.ProjectCalendarWeeks.Include(x => x.ProjectCalendar).Where(x=>x.ProjectCalendar.ProjectId == GetProjectId()).FirstOrDefaultAsync(x => x.WeekStart <= model.PartDate.ToDateTime() && x.WeekEnd >= model.PartDate.ToDateTime()  || x.WeekEnd == model.PartDate.ToDateTime() || x.WeekStart == model.PartDate.ToDateTime());
            var tempusers = new List<IVC.PE.ENTITIES.Models.General.ApplicationUser>
            {
                new IVC.PE.ENTITIES.Models.General.ApplicationUser
                {
                    Name = "",
                    PaternalSurname = "",
                    MaternalSurname = "",
                    WorkArea = 0

                }
            };

            if (users == null)
                users = tempusers.FirstOrDefault();
            var last = await _context.EquipmentMachPartFoldings.OrderByDescending(x => x.Order-1).FirstOrDefaultAsync();
            var equipmentMachineryTransportPart = await _context.EquipmentMachParts.Where(x => x.Id == model.EquipmentMachPartId).FirstOrDefaultAsync();

            var sewers = await _context.SewerGroups.Where(x => x.Id == model.SewerGroupId).FirstOrDefaultAsync();

            var acts = await _context.EquipmentMachineryTypeTypeActivities.Where(x => x.Id == model.EquipmentMachineryTypeTypeActivityId).FirstOrDefaultAsync();

            var phases = await _context.MachineryPhases.Include(x => x.ProjectPhase).Where(x => x.Id == model.MachineryPhaseId).FirstOrDefaultAsync();


            var renovation = await _context.EquipmentMachPartFoldings
                .FirstOrDefaultAsync(x => x.Id == id);

            renovation.PartDate = model.PartDate.ToDateTime();
            renovation.PartNumber = model.PartNumber;
            renovation.EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId;
            renovation.InitHorometer = model.InitHorometer;
            renovation.EndHorometer = model.EndHorometer;

            renovation.Dif = model.EndHorometer - model.InitHorometer;

            renovation.Specific = model.Specific;

            renovation.UserId = model.UserId;

            renovation.UserName = model.UserId != null ? users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname : null;
            renovation.WorkArea = model.UserId != null ? users.WorkArea : 0;

            renovation.EquipmentMachineryTypeTypeActivityId = model.EquipmentMachineryTypeTypeActivityId;
            renovation.SewerGroupId = model.SewerGroupId;
            renovation.MachineryPhaseId = model.MachineryPhaseId.Value;
            renovation.ProjectCalendarWeekId = dates.Id;


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.EquipmentMachPartFoldings.FirstOrDefaultAsync(x => x.Id == id);

            var equipment = await _context.EquipmentMachParts
.FirstOrDefaultAsync(x => x.Id == data.EquipmentMachPartId);

            equipment.FoldingNumber--;

            _context.EquipmentMachPartFoldings.Remove(data);

            await _context.SaveChangesAsync();

            


            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
