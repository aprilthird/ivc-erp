using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/bancos")]
    public class BankController : BaseController
    {
        public BankController(IvcDbContext context, 
            ILogger<BankController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Banks
                .Select(x => new BankViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Banks
                .Where(x => x.Id == id)
                .Select(x => new BankViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    SunatCode = x.SunatCode
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BankViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bank = new Bank
            {
                Name = model.Name,
                SunatCode = model.SunatCode
            };
            await _context.Banks.AddAsync(bank);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BankViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bank = await _context.Banks.FindAsync(id);
            bank.Name = model.Name;
            bank.SunatCode = model.SunatCode;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bank = await _context.Banks.FirstOrDefaultAsync(x => x.Id == id);
            if (bank == null)
                return BadRequest($"Banco con Id '{id}' no encontrado.");
            _context.Banks.Remove(bank);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}