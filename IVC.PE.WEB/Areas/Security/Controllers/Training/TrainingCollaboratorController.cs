using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.Security.Training;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.Controllers.Training
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitacion/colaboradores")]
    public class TrainingCollaboratorController : BaseController
    {
        public TrainingCollaboratorController(IvcDbContext context)
            : base(context)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetCollaborators()
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var data = await _context.Set<UspTrainingGetCollaborators>().FromSqlRaw("execute Security_uspTrainingGetCollaborators @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(data);
        }
    }
}
