using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BlueprintViewModels;
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

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/folding-planos-detalle")]
    public class BluePrintFoldingDetailController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BluePrintFoldingDetailController(IvcDbContext context,
            ILogger<BluePrintFoldingDetailController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid bpfId)
        {

            var query = _context.BluePrintFoldingDetails
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Where(x => x.BlueprintFoldingId == bpfId)
                .Select(x => new BlueprintFoldingDetailViewModel
                {
                    Id = x.Id,
                    BlueprintFoldingId = x.BlueprintFoldingId,
                    DateType = x.DateType.Date.ToDateString(),
                    UserId = x.UserId,
                    UserName = x.UserName,
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BluePrintFoldingDetails
                 .Where(x => x.Id == id)
                 .Select(x => new BlueprintFoldingDetailViewModel
                 {
                     Id = x.Id,
                     BlueprintFoldingId = x.BlueprintFoldingId,
                     DateType = x.DateType.Date.ToDateString(),
                     UserId = x.UserId,
                     UserName = x.UserName,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BlueprintFoldingDetailViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var bprint = new BluePrintFoldingDetail
            {
                BlueprintFoldingId = model.BlueprintFoldingId,
                DateType = model.DateType.ToDateTime(),
                UserId = model.UserId,
                UserName = model.UserId != null ? users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname : null,

            };

            await _context.BluePrintFoldingDetails.AddAsync(bprint);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BlueprintFoldingDetailViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            var bprint = await _context.BluePrintFoldingDetails
                .FirstOrDefaultAsync(x => x.Id.Equals(id));


            bprint.DateType = model.DateType.ToDateTime();
            bprint.UserId = model.UserId;

            bprint.UserName = model.UserId != null ? users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname : null;

            await _context.SaveChangesAsync();




            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.BluePrintFoldingDetails
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"plano con Id '{id}' no se halló.");
            }

            _context.BluePrintFoldingDetails.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
