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
    [Route("calidad/tipo-de-certificado-equipo")]
    public class EquipmentCertificateTypeController : BaseController
    {

        public EquipmentCertificateTypeController(IvcDbContext context,
        ILogger<EquipmentCertificateTypeController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.EquipmentCertificateTypes
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new EquipmentCertificateTypeViewModel
                {
                    Id = x.Id,
                    CertificateTypeName = x.CertificateTypeName,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                    Color = x.PillColor
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentCertificateTypes
                 .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentCertificateTypeViewModel
                 {
                     Id = x.Id,
                     CertificateTypeName = x.CertificateTypeName,
                     Color = x.PillColor
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertificateTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentCertificateType = new EquipmentCertificateType
            {
                ProjectId = GetProjectId(),
                CertificateTypeName = model.CertificateTypeName,
                PillColor = model.Color
            };

            await _context.EquipmentCertificateTypes.AddAsync(equipmentCertificateType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentCertificateTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentCertificateTypes = await _context.EquipmentCertificateTypes
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentCertificateTypes.CertificateTypeName = model.CertificateTypeName;
            equipmentCertificateTypes.PillColor = model.Color;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentCertificateTypes = await _context.EquipmentCertificateTypes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentCertificateTypes == null)
                return BadRequest($"Tipo de certificado con Id '{id}' no encontrado.");
            _context.EquipmentCertificateTypes.Remove(equipmentCertificateTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
