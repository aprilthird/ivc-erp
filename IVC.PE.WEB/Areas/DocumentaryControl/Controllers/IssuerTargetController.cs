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
    [Route("control-documentario/emisores-y-receptores")]
    public class IssuerTargetController : BaseController
    {
       
        public IssuerTargetController(IvcDbContext context,
    ILogger<IssuerTargetController> logger)
: base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var projectId = GetProjectId();

            var query = _context.IssuerTargets
                .AsQueryable();

            var data = await query
                .Where(x => x.ProjectId == projectId)
                .Select(x => new IssuerTargetViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.IssuerTargets
                //.Include(x => x.Project)
                .Where(x => x.Id == id)
                .Select(x => new IssuerTargetViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    LetterIds = x.LetterIssuerTargets.Select(y=>y.LetterId),
                    Letters = x.LetterIssuerTargets.Select(y=> new LetterViewModel()
                    {
                        Name = y.Letter.Name,
                    })
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(IssuerTargetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var issuerTarget = new IssuerTarget
            {
                Name = model.Name,
                ProjectId = GetProjectId()
            };

            if (model.LetterIds != null)
            {
                await _context.LetterIssuerTargets.AddRangeAsync(
                    model.LetterIds.Select(x=> new LetterIssuerTarget
                    {
                        LetterId = x,
                        IssuerTarget = issuerTarget
                    }).ToList());
            }
            await _context.IssuerTargets.AddAsync(issuerTarget);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, IssuerTargetViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var issuerTarget = await _context.IssuerTargets.FindAsync(id);
            issuerTarget.Name = model.Name;


            //var existingLetterIssuerTarget = await _context.LetterIssuerTargets.Where(x => x.IssuerTargetId.Equals(issuerTarget.Id)).ToListAsync();
            //if (existingLetterIssuerTarget.Count > 0)
            //{
            //    _context.LetterIssuerTargets.RemoveRange(existingLetterIssuerTarget);
            //}

            //await _context.LetterIssuerTargets.AddRangeAsync(
            //    model.LetterIds.Select(x => new LetterIssuerTarget
            //    {
            //        LetterId = x,
            //        IssuerTarget = issuerTarget,
            //    }).ToList()) ;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var issuerTarget= await _context.IssuerTargets
                .Include(x => x.LetterIssuerTargets)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (issuerTarget == null)
                return BadRequest($"Emisor o receptor con Id '{id}' no encontrado.");
            _context.LetterIssuerTargets.RemoveRange(issuerTarget.LetterIssuerTargets);
            _context.IssuerTargets.Remove(issuerTarget);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/cartas/listar")]
        public async Task<IActionResult> GetLetters(Guid issuerTargetId)
        {
            var data = await _context.Letters
                .Where(x => x.LetterIssuerTargets.Any(u => u.IssuerTargetId == issuerTargetId))
                .Select(x => new UserViewModel
                {
                    Name = x.Name,
                }).ToListAsync();
            return Ok(data);
        }

    }
}
