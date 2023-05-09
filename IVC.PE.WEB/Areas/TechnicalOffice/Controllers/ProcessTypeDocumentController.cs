using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcessTypeDocumentViewModels;
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
    [Route("oficina-tecnica/proceso-tipo-de-documentos")]
    public class ProcessTypeDocumentController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProcessTypeDocumentController(IvcDbContext context,
            ILogger<ProcessTypeDocumentController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid bpId)
        {

            var query = _context.ProcessTypeDocuments
              .AsQueryable();
            var data = await query
                .Where(x => x.ProcessId == bpId)
                .Select(x => new ProcessTypeDocumentViewModel
                {
                    Id = x.Id,
                    ProcessId = x.ProcessId,
                    Code = x.Code,
                    DocumentType = x.DocumentType,
                })
                .ToListAsync();

            
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.ProcessTypeDocuments
                 .Where(x => x.Id == id)
                 .Select(x => new ProcessTypeDocumentViewModel
                 {
                     Id = x.Id,
                     ProcessId = x.ProcessId,
                     Code = x.Code,
                     DocumentType = x.DocumentType,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProcessTypeDocumentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
    
            
            
            
            var bprint = new ProcessTypeDocument
            {
                ProcessId = model.ProcessId,
                Code = model.Code,
                DocumentType = model.DocumentType
            };

            await _context.ProcessTypeDocuments.AddAsync(bprint);
            await _context.SaveChangesAsync();



            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProcessTypeDocumentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = await _context.ProcessTypeDocuments
                .FirstOrDefaultAsync(x => x.Id.Equals(id));


            bprint.Code = model.Code;
            bprint.DocumentType = model.DocumentType;




            await _context.SaveChangesAsync();




            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.ProcessTypeDocuments
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"tipo de documento con Id '{id}' no se halló.");
            }

            _context.ProcessTypeDocuments.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }


}
