using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVC.PE.CORE.Helpers.ConstantHelpers;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitaciones/control")]
    public class TrainingControlController : BaseController
    {
        public TrainingControlController(IvcDbContext context,
            ILogger<TrainingControlController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string status = null, int? category = null, int? origin = null, int? workgroup = null)
        {
            var projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var query = await _context.Set<UspWorker>().FromSqlRaw("execute HumanResources_uspWorkers @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (status != "todos")
                if (status == "activo")
                    query = query.Where(x => x.IsActive).ToList();
                else if (status == "cesado")
                    query = query.Where(x => !x.IsActive).ToList();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value).ToList();
            if (origin.HasValue)
                query = query.Where(x => x.Origin == origin.Value).ToList();
            if (workgroup.HasValue)
                query = query.Where(x => x.Workgroup == workgroup.Value).ToList();

            return Ok(query);
        }

        [HttpGet("{id}/sesiones/listar")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var data = await _context.TrainingSessionWorkerEmployees
                .Where(x => x.WorkerId == id || x.EmployeeId == id)
                .Select(x => new
                {
                    TrainingCategory = x.TrainingSession.TrainingTopic.TrainingCategory.Name,
                    TrainingTopic = x.TrainingSession.TrainingTopic.Name,
                    SessionDate = x.TrainingSession.SessionDate.ToLocalDateFormat(),
                    User = x.TrainingSession.User.FullName,
                    WorkFront = x.TrainingSession.WorkFront.Code,
                    EmployeeId = x.EmployeeId,
                    WorkerId = x.WorkerId,
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
    }
}
