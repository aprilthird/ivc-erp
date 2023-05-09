using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.Admin.ViewModels.InterestGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.IssuerTargetViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterDocumentCharacteristicViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/caracteristicas-del-documento")]
    public class LetterDocumentCharacteristicController : BaseController
    {
        public LetterDocumentCharacteristicController(IvcDbContext context,
        ILogger<LetterDocumentCharacteristicController> logger)
        : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.LetterDocumentCharacteristics
              .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new LetterDocumentCharacteristicViewModel
                {
                    Id = x.Id,
                    Description= x.Description,
                    ProjectId = x.ProjectId,
                    ProjectViewModel = new ProjectViewModel
                    {
                        Abbreviation = x.Project.Abbreviation
                    },
                    DocStyle = x.DocumentStyle
                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.LetterDocumentCharacteristics
                 .Include(x => x.Project)
                 .Where(x => x.Id == id)
                 .Select(x => new LetterDocumentCharacteristicViewModel
                 {
                     Id = x.Id,
                     Description = x.Description,
                     DocStyle = x.DocumentStyle
                 }).FirstOrDefaultAsync();
                
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(LetterDocumentCharacteristicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var letterDocumentCharacteristics = new LetterDocumentCharacteristic
            {
                ProjectId = GetProjectId(),
                Description = model.Description,
                DocumentStyle = model.DocStyle
            };

            await _context.LetterDocumentCharacteristics.AddAsync(letterDocumentCharacteristics);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, LetterDocumentCharacteristicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var letterDocumentCharacteristics = await _context.LetterDocumentCharacteristics
                .FirstOrDefaultAsync(x=>x.Id == id);
            letterDocumentCharacteristics.Description = model.Description;
            letterDocumentCharacteristics.DocumentStyle = model.DocStyle;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var letterDocumentCharacteristics = await _context.LetterDocumentCharacteristics
                .FirstOrDefaultAsync(x=>x.Id == id);
            if (letterDocumentCharacteristics == null)
                return BadRequest($"Caracteristica del documento con Id '{id}' no encontrado.");
            _context.LetterDocumentCharacteristics.Remove(letterDocumentCharacteristics);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
