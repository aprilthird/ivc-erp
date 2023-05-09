using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros/periodos-laborales")]
    public class WorkerWorkPeriodController : BaseController
    {
        public WorkerWorkPeriodController(IvcDbContext context,
            UserManager<ApplicationUser> userManager)
            : base(context, userManager)
        {
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? workerId = null)
        {
            if (!workerId.HasValue)
                return Ok(new List<WorkerWorkPeriodViewModel>());

            var periods = await _context.WorkerWorkPeriods
                .Include(x => x.Project)
                .Where(x => x.WorkerId == workerId.Value)
                .Select(x => new WorkerWorkPeriodViewModel
                {
                    Id = x.Id,
                    EntryDateDt = x.EntryDate.Value,
                    EntryDate = x.EntryDate.Value.ToDateString(),
                    CeaseDate = x.CeaseDate.HasValue ? x.CeaseDate.Value.ToDateString() : string.Empty,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                    IsActive = x.IsActive
                })
                .OrderByDescending(x => x.EntryDateDt)
                .ToListAsync();

            return Ok(periods);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var period = await _context.WorkerWorkPeriods
                .Where(x => x.Id == id)
                .Select(x => new WorkerWorkPeriodViewModel
                {
                    Id = x.Id,
                    WorkerId = x.WorkerId.Value,
                    EntryDate = x.EntryDate.Value.ToDateString(),
                    ProjectId = x.ProjectId,
                    PensionFundAdministratorId = x.PensionFundAdministratorId.Value,
                    PensionFundUniqueIdentificationCode = x.PensionFundUniqueIdentificationCode,
                    Category = x.Category,
                    Origin = x.Origin,
                    Workgroup = x.Workgroup,
                    WorkerPositionId = x.WorkerPositionId,
                    NumberOfChildren = x.NumberOfChildren,
                    HasUnionFee = x.HasUnionFee,
                    HasSctr = x.HasSctr,
                    SctrHealthType = x.SctrHealthType,
                    SctrPensionType = x.SctrPensionType,
                    JudicialRetentionFixedAmmount = x.JudicialRetentionFixedAmmount,
                    JudicialRetentionPercentRate = x.JudicialRetentionPercentRate,
                    HasWeeklySettlement = x.HasWeeklySettlement,
                    LaborRegimen = x.LaborRegimen,
                    HasEPS = x.HasEPS,
                    HasEsSaludPlusVida = x.HasEsSaludPlusVida,
                    CeaseDate = x.CeaseDate.HasValue ? x.CeaseDate.Value.ToDateString() : string.Empty,
                    IsActive = x.IsActive
                }).FirstAsync();

            return Ok(period);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkerWorkPeriodViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var periods = await _context.WorkerWorkPeriods
                .Where(x => x.WorkerId == model.WorkerId)
                .OrderBy(x => x.EntryDate.Value)
                .ToListAsync();

            if (periods.Any(x => x.IsActive))
                return BadRequest("Existe periodo activo.");

            var newEntryDate = model.EntryDate.ToDateTime();
            if (periods.Any(x => x.EntryDate.Value.Date >= newEntryDate.Date ||
                        (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= newEntryDate.Date : false)))
                return BadRequest("El nuevo periodo se cruza con un periodo anterior.");
            

            var period = new WorkerWorkPeriod
            {
                WorkerId = model.WorkerId,
                EntryDate = model.EntryDate.ToDateTime(),
                //ProjectId = model.ProjectId,
                ProjectId = GetProjectId(),
                Category = model.Category,
                Origin = model.Origin,
                Workgroup = model.Workgroup,
                WorkerPositionId = model.WorkerPositionId,
                PensionFundAdministratorId = model.PensionFundAdministratorId,
                PensionFundUniqueIdentificationCode = model.PensionFundUniqueIdentificationCode,
                NumberOfChildren = model.NumberOfChildren,
                HasUnionFee = true,
                HasSctr = true,
                SctrHealthType = 2,
                SctrPensionType = 2,
                JudicialRetentionFixedAmmount = model.JudicialRetentionFixedAmmount,
                JudicialRetentionPercentRate = model.JudicialRetentionPercentRate,
                HasWeeklySettlement = true,
                LaborRegimen = 27,
                HasEPS = false,
                HasEsSaludPlusVida = true,
                IsActive = true,
            };

            await _context.WorkerWorkPeriods.AddAsync(period);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkerWorkPeriodViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var periods = await _context.WorkerWorkPeriods
                .Where(x => x.WorkerId == model.WorkerId)
                .OrderBy(x => x.EntryDate.Value)
                .ToListAsync();

            var newEntryDate = model.EntryDate.ToDateTime();

            var hasIntersectingPeriods = periods.Where(x => x.Id != id && (x.EntryDate.Value.Date >= newEntryDate.Date ||
                        (x.CeaseDate.HasValue ? x.CeaseDate.Value.Date >= newEntryDate.Date : false)))
                .Count() > 0;
            if (hasIntersectingPeriods)
                return BadRequest("El nuevo periodo se cruza con un periodo anterior.");

            var period = periods.FirstOrDefault(x => x.Id == id);
            if (period.EntryDate.Value.Date < newEntryDate.Date)
            {
                var hasTasks = await _context.WorkerDailyTasks
                    .Where(x => x.Date.Date >= period.EntryDate.Value.Date && x.Date.Date < newEntryDate.Date)
                    .CountAsync() > 0;

                if (hasTasks)
                    return BadRequest("El obrero tiene tareos registrados entre la fecha de ingreso actual y la ingresada.");
            }

            period.EntryDate = newEntryDate;
            period.PensionFundAdministratorId = model.PensionFundAdministratorId;
            period.PensionFundUniqueIdentificationCode = model.PensionFundUniqueIdentificationCode;
            period.Category = model.Category;
            period.Origin = model.Origin;
            period.Workgroup = model.Workgroup;
            period.WorkerPositionId = model.WorkerPositionId;
            period.NumberOfChildren = model.NumberOfChildren;
            period.JudicialRetentionFixedAmmount = model.JudicialRetentionFixedAmmount;
            period.JudicialRetentionPercentRate = model.JudicialRetentionPercentRate;

            if (string.IsNullOrEmpty(model.CeaseDate))
            {
                period.CeaseDate = null;
                period.IsActive = true;
            }
            else
            {
                var cDate = model.CeaseDate.ToDateTime();
                // TODO: Controlar los ceses de los obreros en edición
                var tasks = await _context
                    .WorkerDailyTasks
                    .Where(x => x.WorkerId == period.WorkerId &&
                                x.Date.Date > cDate.Date &&
                                x.ProjectId == period.ProjectId)
                    .CountAsync();

                if(tasks > 0)
                    return BadRequest("El obrero tiene tareos registrados posterior a la fecha de cese.");

                period.CeaseDate = cDate;
                period.IsActive = false;
            }


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("cesar/{id}")]
        public async Task<IActionResult> CeasePeriod(Guid id, WorkerWorkPeriodCeaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var period = await _context.WorkerWorkPeriods.FirstOrDefaultAsync(x => x.Id == id);
            if (period == null)
                return BadRequest("Período no existe.");

            var ceaseDate = model.CeaseDate.ToDateTime();
            var haveDts = await _context.WorkerDailyTasks
                .Where(x => x.Id == id && x.Date.Date > ceaseDate.Date)
                .CountAsync() > 0;

            if (haveDts)
                return BadRequest("Existen tareos posteriores a la fecha de cese, registrados.");

            period.CeaseDate = ceaseDate;
            period.IsActive = false;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var period = await _context.WorkerWorkPeriods.FirstOrDefaultAsync(x => x.Id == id);

            var hasTasks = await _context.WorkerDailyTasks
                .AnyAsync(x => x.Date.Date >= period.EntryDate.Value.Date &&
                        (period.CeaseDate.HasValue ? x.Date.Date <= period.CeaseDate.Value.Date : true));

            _context.WorkerWorkPeriods.Remove(period);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}