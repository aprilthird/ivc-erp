using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/operador-usuario")]
    public class EquipmentCertificateUserOperatorController : BaseController
    {
        public EquipmentCertificateUserOperatorController(IvcDbContext context,
        ILogger<EquipmentCertificateUserOperatorController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.EquipmentCertificateUserOperators
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new EquipmentCertificateUserOperatorViewModel
                {
                    Id = x.Id,
                    Operator = x.Operator,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentCertificateUserOperators
                 .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentCertificateUserOperatorViewModel
                 {
                     Id = x.Id,
                    Operator = x.Operator,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertificateUserOperatorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentCertificateUser = new EquipmentCertificateUserOperator
            {
                ProjectId = GetProjectId(),
                 Operator = model.Operator
            };

            await _context.EquipmentCertificateUserOperators.AddAsync(equipmentCertificateUser);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentCertificateUserOperatorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentCertificateUserOperator = await _context.EquipmentCertificateUserOperators
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentCertificateUserOperator.Operator= model.Operator;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentCertificateUserOperator  = await _context.EquipmentCertificateUserOperators
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentCertificateUserOperator == null)
                return BadRequest($"Propietario con Id '{id}' no encontrado.");
            _context.EquipmentCertificateUserOperators.Remove(equipmentCertificateUserOperator);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
