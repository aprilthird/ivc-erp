using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DocumentTypeViewModels;
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
    [Route("oficina-tecnica/tipo-de-documento")]
    public class DocumentTypeController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public DocumentTypeController(IvcDbContext context,
            ILogger<DocumentTypeController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid bpId)
        {

            var query = _context.DocumentTypes
              .AsQueryable();
            var data = await query
                .Where(x=>x.ProjectId == GetProjectId())
                .Select(x => new DocumentTypeViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Type = x.Type,
                })
                .ToListAsync();


            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.DocumentTypes
                 .Where(x => x.Id == id)
                 .Select(x => new DocumentTypeViewModel
                 {
                     Id = x.Id,
                     Code = x.Code,
                     Type = x.Type,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(DocumentTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);





            var bprint = new DocumentType
            {
                Code = model.Code,
                Type = model.Type,
                ProjectId = GetProjectId()
            };

            await _context.DocumentTypes.AddAsync(bprint);
            await _context.SaveChangesAsync();



            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, DocumentTypeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = await _context.DocumentTypes
                .FirstOrDefaultAsync(x => x.Id.Equals(id));


            bprint.Code = model.Code;
            bprint.Type = model.Type;




            await _context.SaveChangesAsync();




            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.DocumentTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"tipo de documento con Id '{id}' no se halló.");
            }

            _context.DocumentTypes.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
