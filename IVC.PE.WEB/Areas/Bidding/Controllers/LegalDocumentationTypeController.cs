using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/tipo-de-documento-legal")]
    public class LegalDocumentationTypeController : BaseController
    {
        public LegalDocumentationTypeController(IvcDbContext context,
        ILogger<LegalDocumentationTypeController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.LegalDocumentationTypes
              .AsQueryable();

            //  query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new LegalDocumentationTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.LegalDocumentationTypes
                .Where(x => x.Id == id)
                .Select(x => new LegalDocumentationTypeViewModel
                {
                    Id = x.Id,
                    Name = x.Name

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(LegalDocumentationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var legalDocumentationType = new LegalDocumentationType
            {
                // ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId")),



                Name = model.Name
            };
            await _context.LegalDocumentationTypes.AddAsync(legalDocumentationType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, LegalDocumentationTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var legalDocumentationType = await _context.LegalDocumentationTypes.FindAsync(id);


            legalDocumentationType.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var legalDocumentationType = await _context.LegalDocumentationTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (legalDocumentationType == null)
                return BadRequest($"Tipo de documento con Id '{id}' no encontrado.");
            _context.LegalDocumentationTypes.Remove(legalDocumentationType);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}