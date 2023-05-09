using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.IntegratedManagementSystem;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.IntegratedManagementSystem.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.INTEGRATED_MANAGEMENT_SYSTEM)]
    [Route("sistema-de-manejo-integrado/accion-for24")]
    public class For24SecondPartActionController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public For24SecondPartActionController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<For24SecondPartActionController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? SewerManifoldFor24SecondPartId = null)
        {
            if (!SewerManifoldFor24SecondPartId.HasValue)
                return Ok(new List<For24SecondPartActionViewModel>());

            var actions = await _context.For24SecondPartActions
                .Where(x=>x.SewerManifoldFor24SecondPartId == SewerManifoldFor24SecondPartId)
                .Select(x => new For24SecondPartActionViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor24SecondPartId = x.SewerManifoldFor24SecondPartId,
                    ActionName = x.ActionName,
                    Date = x.Date.ToDateString()
                }).ToListAsync();

            return Ok(actions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var action = await _context.For24SecondPartActions
                .Where(x => x.Id == id)
                .Select(x => new For24SecondPartActionViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor24SecondPartId = x.SewerManifoldFor24SecondPartId,
                    ActionName = x.ActionName,
                    Date = x.Date.ToDateString()
                }).FirstOrDefaultAsync();

            return Ok(action);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(For24SecondPartActionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Action = new For24SecondPartAction
            {
                SewerManifoldFor24SecondPartId = model.SewerManifoldFor24SecondPartId,
                ActionName = model.ActionName,
                Date = model.Date.ToDateTime()
            };

            await _context.For24SecondPartActions.AddAsync(Action);
            
            var for24 = await _context.SewerManifoldFor24SecondParts
                .FirstOrDefaultAsync(x => x.Id == model.SewerManifoldFor24SecondPartId);

            var Actions = await _context.For24SecondPartActions
                .Where(x=>x.SewerManifoldFor24SecondPartId == model.SewerManifoldFor24SecondPartId)
                .ToListAsync();

            var count = Actions.Count();
            var max = Actions[0].Date;
            for(int i= 0; i < count; i++)
            {
                if (max < Actions[i].Date)
                    max = Actions[i].Date;
            }

            for24.ProposedDate = max;
           
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var Action = await _context.For24SecondPartActions
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Action == null)
                return BadRequest("Equipo no encontrado");

            var for24 = await _context.SewerManifoldFor24SecondParts
                .FirstOrDefaultAsync(x => x.Id == Action.SewerManifoldFor24SecondPartId);

            var Actions = await _context.For24SecondPartActions
                .Where(x => x.SewerManifoldFor24SecondPartId == Action.SewerManifoldFor24SecondPartId)
                .ToListAsync();

            _context.For24SecondPartActions.Remove(Action);

            var count = Actions.Count();
            var max = Actions[0].Date;
            for (int i = 0; i < count; i++)
            {
                if (max < Actions[i].Date)
                    max = Actions[i].Date;
            }

            if (count != 0)
                for24.ProposedDate = max;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
