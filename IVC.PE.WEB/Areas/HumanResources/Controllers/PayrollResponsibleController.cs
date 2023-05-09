using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
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
    [Route("recursos-humanos/resposanbles")]
    public class PayrollResponsibleController : BaseController
    {
        public PayrollResponsibleController(IvcDbContext context)
            : base(context)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            var projects = await _context.Projects.ToListAsync();

            var model = await _context.ProjectPayrollResponsibles
                .Select(x => new PayrollResponsibleViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Responsible1Id = x.Responsible1Id,
                    Responsible2Id = x.Responsible2Id,
                    Responsible3Id = x.Responsible3Id
                }).ToListAsync();

            if (model.Count == 0)
                return Ok(model);

            foreach (var payroll in model)
            {
                payroll.ProjectFullName = projects.FirstOrDefault(x => x.Id == payroll.ProjectId.Value).Abbreviation;
                payroll.Responsible1FullName = users.FirstOrDefault(x => x.Id == payroll.Responsible1Id).FullName;
                payroll.Responsible2FullName = users.FirstOrDefault(x => x.Id == payroll.Responsible2Id).FullName;
                payroll.Responsible3FullName = users.FirstOrDefault(x => x.Id == payroll.Responsible3Id).FullName;
            }

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var auths = await _context.ProjectPayrollResponsibles
                .Where(x => x.Id == id)
                .Select(x => new PayrollResponsibleViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Responsible1Id = x.Responsible1Id,
                    Responsible2Id = x.Responsible2Id,
                    Responsible3Id = x.Responsible3Id
                }).FirstOrDefaultAsync();

            return Ok(auths);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PayrollResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authDb = await _context.ProjectPayrollResponsibles.FirstOrDefaultAsync(x => x.ProjectId == model.ProjectId);

            if (authDb != null)
                return Ok("Ya exite una autorización registrada.");

            var payrollResponsibles = new ProjectPayrollResponsible
            {
                ProjectId = model.ProjectId.Value,
                Responsible1Id = model.Responsible1Id,
                Responsible2Id = model.Responsible2Id,
                Responsible3Id = model.Responsible3Id
            };

            await _context.ProjectPayrollResponsibles.AddAsync(payrollResponsibles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PayrollResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var auths = await _context.ProjectPayrollResponsibles
                .FirstOrDefaultAsync(x => x.Id == id);

            auths.ProjectId = model.ProjectId.Value;
            auths.Responsible1Id = model.Responsible1Id;
            auths.Responsible2Id = model.Responsible2Id;
            auths.Responsible3Id = model.Responsible3Id;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var auths = await _context.ProjectPayrollResponsibles
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.ProjectPayrollResponsibles.Remove(auths);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
