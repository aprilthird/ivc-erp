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
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.StatusViewModels;
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
    [Route("control-documentario/cartas-estados")]
    public class StatusController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public StatusController(IvcDbContext context,
        ILogger<StatusController> logger,
        IOptions<CloudStorageCredentials> storageCredentials)
        : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        //[HttpGet("listar")]
        //public async Task<IActionResult> GetAll()
        //{
        //    var query = _context.Status
        //      .AsQueryable();

        //    var data = await query
        //        .Select(x => new StatusViewModel
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //        })
        //        .ToListAsync();
        //    return Ok(data);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(Guid id)
        //{
        //    var data = await _context.Status
        //        .Where(x => x.Id == id)
        //        .Select(x => new StatusViewModel
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //        }).AsNoTracking()
        //        .FirstOrDefaultAsync();
        //    return Ok(data);
        //}

        //[HttpPost("crear")]
        //public async Task<IActionResult> Create(StatusViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var status = new StatusViewModel
        //    {
        //        Name = model.Name
        //    };

        //    await _context.Status.AddAsync(status);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[HttpPut("editar/{id}")]
        //public async Task<IActionResult> Edit(Guid id, StatusViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var status = await _context.BondTypes.FindAsync(id);


        //    status.Name = model.Name;
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[HttpDelete("eliminar/{id}")]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var status = await _context.Status.FirstOrDefaultAsync(x => x.Id == id);
        //    if (status == null)
        //        return BadRequest($"Tipo de Estado con Id '{id}' no encontrado.");
        //    _context.Status.Remove(status);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
