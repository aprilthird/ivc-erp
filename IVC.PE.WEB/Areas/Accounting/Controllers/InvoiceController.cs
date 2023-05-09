using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Accounting;
using IVC.PE.WEB.Areas.Accounting.ViewModels.InvoiceViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Accounting.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Accounting.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.ACCOUNTING)]
    [Route("contabilidad/facturas")]
    public class InvoiceController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public InvoiceController(IvcDbContext context,
              IOptions<CloudStorageCredentials> storageCredentials,
              ILogger<InvoiceController> logger)
              : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.Invoices
                .Where(x => x.ProjectId == GetProjectId())
                .AsNoTracking()
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Serie.Contains(search));
            }

            var data = await query
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new InvoiceViewModel
                {
                    Id = x.Id,
                    Serie = x.Serie,
                    IssueDate = x.IssueDate.ToDateString(),
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = await query.CountAsync(),
                data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var invoice = await _context.Invoices
                .Where(x => x.Id == id)
                .Select(x => new InvoiceViewModel
                {
                    Id = x.Id,
                    Serie = x.Serie,
                    IssueDate = x.IssueDate.ToDateString(),
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();
            return Ok(invoice);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(InvoiceViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoice = new Invoice
            {
                Serie = model.Serie,
                IssueDate = model.IssueDate.ToDateTime(),
                ProjectId = GetProjectId()
            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                invoice.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.ACCOUNTING, System.IO.Path.GetExtension(model.File.FileName), 
                    ConstantHelpers.Storage.Blobs.ACCOUNTING_INVOICE, model.Serie);
            }

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, InvoiceViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            invoice.Serie = model.Serie;
            invoice.IssueDate = model.IssueDate.ToDateTime();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (invoice.FileUrl != null)
                    await storage.TryDelete(invoice.FileUrl, ConstantHelpers.Storage.Containers.ACCOUNTING);
                invoice.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.ACCOUNTING, System.IO.Path.GetExtension(model.File.FileName), 
                    ConstantHelpers.Storage.Blobs.ACCOUNTING_INVOICE, model.Serie);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id);

            if (invoice == null)
                return BadRequest($"Insumo con Id '{id}' no encontrado.");

            var storage = new CloudStorageService(_storageCredentials);
            if (invoice.FileUrl != null)
                await storage.TryDelete(invoice.FileUrl, ConstantHelpers.Storage.Containers.ACCOUNTING);

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
