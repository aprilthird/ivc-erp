using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollWorkerVariableViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/variables")]
    public class PayrollVariableController : BaseController
    {
        public PayrollVariableController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<PayrollVariableController> logger)
            : base(context, userManager, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var variables = await _context.PayrollVariables
                    .Select(x => new PayrollVariableViewModel
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Description = x.Description,
                        Type = x.Type,
                        Formula = x.Formula
                    }).ToListAsync();

            return Ok(variables);
        }

        
        //Metodos para Editar por Obrero
        [HttpGet("obrero")]
        public async Task<IActionResult> GetAllByWorker(Guid headerId, Guid workerId)
        {
            var workerVariables = await _context.PayrollWorkerVariables
                .Include(x => x.PayrollVariable)
                .Where(x => x.PayrollMovementHeaderId == headerId && x.WorkerId == workerId)
                .Select(x => new PayrollWorkerVariableViewModel
                {
                    Id = x.Id,
                    WorkerId = x.WorkerId,
                    PayrollVariableId = x.PayrollVariableId,
                    PayrollVariable = new PayrollVariableViewModel
                    {
                        Type = x.PayrollVariable.Type,
                        Code = x.PayrollVariable.Code,
                        Description = x.PayrollVariable.Description,
                    },
                    PayrollMovementHeaderId = x.PayrollMovementHeaderId,
                    Value = x.Value
                })
                .OrderBy(x => x.PayrollVariable.Type).ThenBy(x => x.PayrollVariable.Code)
                .ToListAsync();

            return Ok(workerVariables);
        }

        [HttpGet("obrero/{id}")]
        public async Task<IActionResult> GetWorkerVariable(Guid id)
        {
            var workerVariable = await _context.PayrollWorkerVariables
                .Where(x => x.Id == id)
                .Select(x => new PayrollWorkerVariableViewModel
                {
                    Id = x.Id,
                    PayrollVariableId = x.PayrollVariableId,
                    Value = x.Value,
                    WorkerId = x.WorkerId,
                    PayrollMovementHeaderId = x.PayrollMovementHeaderId
                }).FirstOrDefaultAsync();

            return Ok(workerVariable);
        }

        [HttpPut("obrero/editar/{id}")]
        public async Task<IActionResult> UpdateWorkerVariable(Guid id, PayrollWorkerVariable model)
        {
            var workerVariable = await _context.PayrollWorkerVariables
                .FirstOrDefaultAsync(x => x.Id == id);

            workerVariable.Value = model.Value;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}