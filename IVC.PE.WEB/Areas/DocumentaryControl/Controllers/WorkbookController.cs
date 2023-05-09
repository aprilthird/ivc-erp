using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/libros-de-obra")]
    public class WorkbookController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public WorkbookController(IvcDbContext context,
             IOptions<CloudStorageCredentials> storageCredentials,
           ILogger<WorkbookController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var data = await _context.Workbooks
                .Where(x => x.ProjectId == projectId)
                .Select(x => new WorkbookViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    Name = x.Name,
                    Range = x.Range,
                    Term = x.Term
                }).OrderBy(x => x.Number)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var model = await _context.Workbooks
                .Where(x => x.Id == id)
                .Select(x => new WorkbookViewModel
                {
                    Id = x.Id,
                    Number = x.Number,
                    Name = x.Name,
                    Range = x.Range,
                    Term = x.Term,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkbookViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workbook = new Workbook
            {
                Number = model.Number,
                Name = model.Name,
                Range = model.Range,
                Term = model.Term,
                ProjectId = GetProjectId()
            };


            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                workbook.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                     ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
                     System.IO.Path.GetExtension(model.File.FileName),
                     ConstantHelpers.Storage.Blobs.WORKBOOKS_PDF,
                     $"libro_nro-{workbook.Number}");
            }


            //if (model.WordFile != null)
            //{
            //    var storage = new CloudStorageService(_storageCredentials);
            //    workbook.WordFileUrl = await storage.UploadFile(model.WordFile.OpenReadStream(),
            //         ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
            //         System.IO.Path.GetExtension(model.PdfFile.FileName),
            //         ConstantHelpers.Storage.Blobs.WORKBOOKS_WORD,
            //         $"libro_nro{workbook.Number}");
            //}

            await _context.Workbooks.AddAsync(workbook);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkbookViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workbook = await _context.Workbooks
                .FirstOrDefaultAsync(x => x.Id == id);

            workbook.Number = model.Number;
            workbook.Name = model.Name;
            workbook.Range = model.Range;
            workbook.Term = model.Term;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (workbook.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{workbook.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                workbook.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.WORKBOOKS_PDF,
                   $"libro_nro-{workbook.Number}");
            }
            //if (model.PdfFile != null)
            //{
            //    if (workbook.PdfFileUrl != null)
            //        await storage.TryDelete(workbook.PdfFileUrl, ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            //    workbook.PdfFileUrl = await storage.UploadFile(model.PdfFile.OpenReadStream(),
            //        ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
            //        System.IO.Path.GetExtension(model.PdfFile.FileName),
            //        ConstantHelpers.Storage.Blobs.WORKBOOKS_PDF,
            //        workbook.Number.ToString());
            //}
            //if (model.WordFile != null)
            //{
            //    if (workbook.WordFileUrl != null)
            //        await storage.TryDelete(workbook.WordFileUrl, ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            //    workbook.WordFileUrl = await storage.UploadFile(model.WordFile.OpenReadStream(),
            //        ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
            //        System.IO.Path.GetExtension(model.WordFile.FileName),
            //        ConstantHelpers.Storage.Blobs.WORKBOOKS_WORD,
            //        workbook.Number.ToString());
            //}

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.LegalTechnicalLibrary.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workbook = await _context.Workbooks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (workbook == null)
                return BadRequest($"Libro de Obra con Id '{id}' no se halló.");

            if (workbook.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{workbook.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            }
            //if (workbook.PdfFileUrl != null)
            //{
            //    await storage.TryDelete(workbook.PdfFileUrl, ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            //}
            //if (workbook.WordFileUrl != null)
            //{
            //    await storage.TryDelete(workbook.WordFileUrl, ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            //}

            _context.Workbooks.Remove(workbook);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}