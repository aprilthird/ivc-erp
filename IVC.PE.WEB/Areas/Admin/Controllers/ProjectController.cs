using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/proyectos")]
    public class ProjectController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ProjectController(IvcDbContext context,
            ILogger<ProjectController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Projects
                .Include(x=>x.Business)
                .Select(x => new ProjectViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CostCenter = x.CostCenter,
                    Abbreviation = x.Abbreviation,
                    BusinessId = x.BusinessId.Value,
                    Business = new BusinessViewModel
                    {
                        Tradename = x.Business.Tradename
                    },
                    LogoUrl = x.LogoUrl,
                    InvoiceSignatureUrl = x.InvoiceSignatureUrl
                }).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var project = await _context.Projects
                .Where(x => x.Id == id)
                .Select(x => new ProjectViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbreviation,
                    BusinessId = x.BusinessId.Value,
                    CostCenter = x.CostCenter,
                    LogoUrl = x.LogoUrl,
                    EstablishmentCode = x.EstablishmentCode,
                    RucCompany = x.RucCompany,
                    InvoiceSignatureUrl = x.InvoiceSignatureUrl
                }).FirstOrDefaultAsync();
            return Ok(project);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var project = new Project
            {
                Name = model.Name,
                Abbreviation = model.Abbreviation,
                CostCenter = model.CostCenter,
                BusinessId = model.BusinessId,
                EstablishmentCode = model.EstablishmentCode,
                RucCompany = model.RucCompany
            };

            if (model.LogoFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                project.LogoUrl = await storage.UploadFile(model.LogoFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.PROJECTS,
                    System.IO.Path.GetExtension(model.LogoFile.FileName),
                    ConstantHelpers.Storage.Blobs.LOGOS,
                    $"logo-{project.Abbreviation}");
            }

            if (model.InvoiceSignatureFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                project.InvoiceSignatureUrl = await storage.UploadFile(model.InvoiceSignatureFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.PROJECTS,
                    System.IO.Path.GetExtension(model.InvoiceSignatureFile.FileName),
                    ConstantHelpers.Storage.Blobs.LOGOS,
                    $"firma-{project.Abbreviation}");
            }

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProjectViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var project = await _context.Projects.FindAsync(id);
            project.Name = model.Name;
            project.Abbreviation = model.Abbreviation;
            project.CostCenter = model.CostCenter;
            project.BusinessId = model.BusinessId;
            project.EstablishmentCode = model.EstablishmentCode;
            project.RucCompany = model.RucCompany;

            if (model.LogoFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (project.LogoUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LOGOS}/{project.LogoUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.PROJECTS);
                project.LogoUrl = await storage.UploadFile(model.LogoFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.PROJECTS,
                    System.IO.Path.GetExtension(model.LogoFile.FileName),
                    ConstantHelpers.Storage.Blobs.LOGOS,
                    $"logo-{project.Abbreviation}");
            }

            if (model.InvoiceSignatureFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (project.InvoiceSignatureUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LOGOS}/{project.InvoiceSignatureUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.PROJECTS);
                project.InvoiceSignatureUrl = await storage.UploadFile(model.InvoiceSignatureFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.PROJECTS,
                    System.IO.Path.GetExtension(model.InvoiceSignatureFile.FileName),
                    ConstantHelpers.Storage.Blobs.LOGOS,
                    $"firma-{project.Abbreviation}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project == null)
                return BadRequest($"Proyecto con Id '{id}' no encontrado.");
            _context.Projects.Remove(project);

            if (project.LogoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LOGOS}/{project.LogoUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.PROJECTS);
            }

            if (project.InvoiceSignatureUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LOGOS}/{project.InvoiceSignatureUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.PROJECTS);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}