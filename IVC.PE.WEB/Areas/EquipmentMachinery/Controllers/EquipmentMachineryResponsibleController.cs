using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryResponsibleViewModels;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/responsables")]

    public class EquipmentMachineryResponsibleController : BaseController
    {
        public EquipmentMachineryResponsibleController(IvcDbContext context,
        ILogger<EquipmentMachineryResponsibleController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Set<UspEquipmentMachineryResponsibles>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentMachineryResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.EquipmentMachineryResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var data = new EquipmentMachineryResponsibleViewModel
            {
                ProjectId = id,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpGet("proyecto")]
        public async Task<IActionResult> GetByProject()
        {
            var projectId = GetProjectId();

            var query = await _context.EquipmentMachineryResponsibles
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            var data = new EquipmentMachineryResponsibleViewModel
            {
                ProjectId = projectId,
                Responsibles = query.Select(x => x.UserId).ToList()
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<EquipmentMachineryResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new EquipmentMachineryResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            await _context.EquipmentMachineryResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            var query = await _context.Set<UspEquipmentMachineryResponsibles>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentMachineryResponsibles")
    .IgnoreQueryFilters()
    .ToListAsync();

            var q = query.Where(x => x.ProjectId == GetProjectId()).FirstOrDefault();

            (await _context.EquipmentMachInsuranceFoldingApplicationUsers.ToListAsync()).ForEach(x =>  x.UserId = q.UserIds);
            (await _context.EquipmentMachSOATFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachTechnicalRevisionFoldingApplications.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachineryTransportTechnicalRevisionFoldingApplications.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachinerySoftInsuranceFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(EquipmentMachineryResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.EquipmentMachineryResponsibles
                .Where(x => x.ProjectId == model.ProjectId)
                .ToListAsync();

            var responsibles = new List<EquipmentMachineryResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new EquipmentMachineryResponsible
                {
                    ProjectId = model.ProjectId,
                    UserId = responsible
                });
            }

            _context.EquipmentMachineryResponsibles.RemoveRange(responsiblesDb);
            await _context.EquipmentMachineryResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            var query = await _context.Set<UspEquipmentMachineryResponsibles>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentMachineryResponsibles")
.IgnoreQueryFilters()
.ToListAsync();

            var q = query.Where(x => x.ProjectId == GetProjectId()).FirstOrDefault();

            (await _context.EquipmentMachInsuranceFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachSOATFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachTechnicalRevisionFoldingApplications.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachineryTransportTechnicalRevisionFoldingApplications.ToListAsync()).ForEach(x => x.UserId = q.UserIds);
            (await _context.EquipmentMachinerySoftInsuranceFoldingApplicationUsers.ToListAsync()).ForEach(x => x.UserId = q.UserIds);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responsiblesDb = await _context.EquipmentMachineryResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.EquipmentMachineryResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
