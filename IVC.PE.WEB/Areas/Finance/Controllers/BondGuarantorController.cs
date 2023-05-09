using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.DATA.Migrations;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;

using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondLoadViewModel;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.FINANCE)]
    [Route("finanzas/garantes")]
    public class BondGuarantorController : BaseController
    {
        public BondGuarantorController(IvcDbContext context,
                ILogger<BondGuarantorController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.BondGuarantors
              .AsQueryable();

       //    query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                .Select(x => new BondGuarantorViewModel
                {
                    Id = x.Id,
                  Name = x.Name,
               

                })
                .ToListAsync();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BondGuarantors
                .Where(x => x.Id == id)
                .Select(x => new BondGuarantorViewModel
                {
                    Id = x.Id,
                     Name = x.Name,


                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BondGuarantorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Guid ident = new Guid(model.Name);
            var business = _context.Businesses.Find(ident);

            var bondGuarantor = new BondGuarantor
            {
                // ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId")),
                
                Name = business.BusinessName,
            
            };
            await _context.BondGuarantors.AddAsync(bondGuarantor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BondGuarantorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Guid ident = new Guid(model.Name);
            var business = _context.Businesses.Find(ident);

            var bondLoad = await _context.BondGuarantors.FindAsync(id);


            bondLoad.Name = business.BusinessName;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondGuarantor = await _context.BondGuarantors.FirstOrDefaultAsync(x => x.Id == id);
            if (bondGuarantor == null)
                return BadRequest($"Garante con Id '{id}' no encontrado.");
            _context.BondGuarantors.Remove(bondGuarantor);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
