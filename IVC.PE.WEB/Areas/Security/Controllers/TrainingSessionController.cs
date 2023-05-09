using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingSessionViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitaciones/sesiones")]
    public class TrainingSessionController : BaseController
    {

        public TrainingSessionController(IvcDbContext context,
            ILogger<TrainingSessionController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? trainingCategoryId = null, Guid? trainingTopicId = null, string userId = null, Guid? workFrontId = null)
        {
            var projectId = GetProjectId();

            var query = _context.TrainingSessions
                .Where(x => x.TrainingTopic.ProjectId == projectId)
                .AsQueryable();

            if (trainingCategoryId.HasValue)
                query = query.Where(x => x.TrainingTopic.TrainingCategoryId == trainingCategoryId.Value);

            if (trainingTopicId.HasValue)
                query = query.Where(x => x.TrainingTopicId == trainingTopicId.Value);

            if (string.IsNullOrEmpty(userId))
                query = query.Where(x => x.UserId == userId);

            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);

            var data = await query
                .Select(x => new TrainingSessionViewModel
                {
                    Id = x.Id,
                    SessionDate = x.SessionDate.ToLocalDateFormat(),
                    UserId = x.UserId,
                    User = new Admin.ViewModels.UserViewModels.UserViewModel
                    {
                        Name = x.User.Name,
                        MiddleName = x.User.MiddleName,
                        PaternalSurname = x.User.PaternalSurname,
                        MaternalSurname = x.User.MaternalSurname,
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new Admin.ViewModels.WorkFrontViewModels.WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    TrainingTopicId = x.TrainingTopicId,
                    TrainingTopic = new ViewModels.TrainingTopicViewModels.TrainingTopicViewModel
                    {
                        Name = x.TrainingTopic.Name,
                        TrainingCategory = new ViewModels.TrainingCategoryViewModels.TrainingCategoryViewModel
                        {
                            Name = x.TrainingTopic.TrainingCategory.Name
                        }
                    }
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}/resultados/listar")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var data = await _context.TrainingSessionWorkerEmployees
                .Where(x => x.TrainingSessionId == id)
                .Select(x => new TrainingSessionAssistantViewModel
                {
                    EmployeeId = x.EmployeeId,
                    Employee = x.EmployeeId.HasValue ? new HumanResources.ViewModels.EmployeeViewModels.EmployeeViewModel
                    {
                        DocumentType = x.Employee.DocumentType,
                        Document = x.Employee.Document,
                        Name = x.Employee.Name,
                        MiddleName = x.Employee.MiddleName,
                        PaternalSurname = x.Employee.PaternalSurname,
                        MaternalSurname = x.Employee.MaternalSurname
                    } : null,
                    WorkerId = x.WorkerId,
                    Worker = x.WorkerId.HasValue ? new HumanResources.ViewModels.WorkerViewModels.WorkerViewModel
                    {
                        DocumentType = x.Worker.DocumentType,
                        Document = x.Worker.Document,
                        Name = x.Worker.Name,
                        MiddleName = x.Worker.MiddleName,
                        PaternalSurname = x.Worker.PaternalSurname,
                        MaternalSurname = x.Worker.MaternalSurname
                    } : null,
                    TrainingResultStatusId = x.TrainingResultStatusId,
                    TrainingResultStatus = new ViewModels.TrainingResultStatusViewModels.TrainingResultStatusViewModel
                    {
                        Name = x.TrainingResultStatus.Name,
                        Color = x.TrainingResultStatus.Color
                    },
                    Observation = x.Observation
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var trainingSession = await _context.TrainingSessions
                .Where(x => x.Id == id)
                .Select(x => new TrainingSessionViewModel
                {
                    Id = x.Id,
                    SessionDate = x.SessionDate.ToLocalDateFormat(),
                    UserId = x.UserId,
                    WorkFrontId = x.WorkFrontId,
                    TrainingCategoryId = x.TrainingTopic.TrainingCategoryId,
                    TrainingTopicId = x.TrainingTopicId,
                }).AsNoTracking().FirstOrDefaultAsync();
            return Ok(trainingSession);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TrainingSessionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var trainingSession = new TrainingSession
            {
                SessionDate = model.SessionDate.ToUtcDateTime(),
                UserId = model.UserId,
                TrainingTopicId = model.TrainingTopicId,
                WorkFrontId = model.WorkFrontId,
            };
            await _context.TrainingSessions.AddAsync(trainingSession);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TrainingSessionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var trainingSession = await _context.TrainingSessions.FindAsync(id);
            trainingSession.SessionDate = model.SessionDate.ToUtcDateTime();
            trainingSession.UserId = model.UserId;
            trainingSession.TrainingTopicId = model.TrainingTopicId;
            trainingSession.WorkFrontId = model.WorkFrontId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var trainingSession= await _context.TrainingSessions.FindAsync(id);
            if (trainingSession is null)
                return BadRequest($"Sesión de capacitación con Id '{id}' no encontrado.");
            _context.TrainingSessions.Remove(trainingSession);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
