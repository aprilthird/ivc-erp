using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.WEB.Areas.Admin.ViewModels.CertificateIssuerViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantHelpers.Roles.SUPERADMIN)]
    [Area(ConstantHelpers.Areas.ADMIN)]
    [Route("admin/emisores-de-certificados")]
    public class CertificateIssuerController : BaseController
    {
        public CertificateIssuerController(IvcDbContext context,
            ILogger<CertificateIssuerController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.CertificateIssuers
                .Select(x => new CertificateIssuerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    }
                })
                .AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var project = await _context.CertificateIssuers
                .Where(x => x.Id == id)
                .Select(x => new CertificateIssuerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProjectId = x.ProjectId
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(project);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(CertificateIssuerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var certificateIssuer = new CertificateIssuer
            {
                Name = model.Name,
                ProjectId = model.ProjectId
            };
            await _context.CertificateIssuers.AddAsync(certificateIssuer);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, CertificateIssuerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var certificateIssuer = await _context.CertificateIssuers.FindAsync(id);
            certificateIssuer.Name = model.Name;
            certificateIssuer.ProjectId = model.ProjectId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var certificateIssuer = await _context.CertificateIssuers.FirstOrDefaultAsync(x => x.Id == id);
            if (certificateIssuer == null)
                return BadRequest($"Emisor de Certificados con Id '{id}' no encontrado.");
            _context.CertificateIssuers.Remove(certificateIssuer);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}