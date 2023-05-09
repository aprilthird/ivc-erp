using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.Biddings;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BusinessResponsibleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.ENTITIES.Models.Bidding;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/responsables-de-empresa")]
    public class BusinessResponsibleController : BaseController
    {
        public BusinessResponsibleController(IvcDbContext context,
        ILogger<BusinessResponsibleController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = await _context.Set<UspBusinessResponsible>().FromSqlRaw("execute Biddings_uspBusinessResponsibles")
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.BusinessResponsibles
                .Where(x => x.BusinessId == id)
                .ToListAsync();

            var data = new BusinessResponsibleViewModel
            {
                BusinessId = id,
                Responsibles = query.Select(x => x.UserId).ToList(),
                SendEmail = query.First(x=>x.SendEmail).SendEmail
            };

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BusinessResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Responsibles.Count() == 0)
                return BadRequest("Seleccionar al menos un responsable.");

            var responsibles = new List<BusinessResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new BusinessResponsible
                {
                    BusinessId = model.BusinessId,
                    UserId = responsible,
                    SendEmail = model.SendEmail
                    
                });
            }

            await _context.BusinessResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(BusinessResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responsiblesDb = await _context.BusinessResponsibles
                .Where(x => x.BusinessId == model.BusinessId)
                .ToListAsync();

            var responsibles = new List<BusinessResponsible>();
            foreach (var responsible in model.Responsibles.First().Split(','))
            {
                responsibles.Add(new BusinessResponsible
                {
                    BusinessId = model.BusinessId,
                    UserId = responsible,
                    SendEmail = model.SendEmail
                });
            }

            _context.BusinessResponsibles.RemoveRange(responsiblesDb);
            await _context.BusinessResponsibles.AddRangeAsync(responsibles);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responsiblesDb = await _context.BusinessResponsibles
                .Where(x => x.BusinessId == id)
                .ToListAsync();

            _context.BusinessResponsibles.RemoveRange(responsiblesDb);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
