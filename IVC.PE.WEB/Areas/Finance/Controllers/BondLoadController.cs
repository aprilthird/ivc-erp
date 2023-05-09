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
using System.Security.Cryptography.Xml;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
   
        [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
        [Area(ConstantHelpers.Areas.FINANCE)]
        [Route("finanzas/cargafianza")]
        public class BondLoadController : BaseController
        {
            public BondLoadController(IvcDbContext context,
               ILogger<BondLoadController> logger) : base(context, logger)
            {
            }

            public IActionResult Index() => View();

            [HttpGet("listar")]
            public async Task<IActionResult> GetAll()
            {
            var query = _context.BondLoads
                 .AsNoTracking()
                  .AsQueryable();

            //query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                    .Select(x => new BondLoadViewModel
                    {
                        Id = x.Id,
                        /*ProjectId = x.ProjectId,
                        Project = new ProjectViewModel
                        {
                            Id = x.Id,
                            Name = x.Project.Name
                        },*/

                        BondGuarantorId = x.BondGuarantorId,
                        BondGuarantor = new BondGuarantorViewModel
                        {
                            Id = x.Id,
                            Name = x.BondGuarantor.Name
                        },
                       
                       
                        BudgetTitleId = x.BudgetTitleId,
                        BudgetTitle = new BudgetTitleViewModel
                        {
                            Id = x.Id,
                            Name = x.BudgetTitle.Name
                        },

                        BondTypeId = x.BondTypeId,
                        BondType = new BondTypeViewModel
                        {
                            Id = x.Id,
                            Name = x.BondType.Name
                        },
                        BankId = x.BankId,
                        Bank = new BankViewModel
                        {
                            Id = x.Id,
                            Name = x.Bank.Name
                        },

                        BondNumber = x.BondNumber,

                        BondRenovationId = x.BondRenovationId,
                        CreateDate = x.CreateDate,

                       
                        PenAmmount = x.PenAmmount,
                        UsdAmmount = x.UsdAmmount,
                        daysLimitTerm = x.daysLimitTerm,
                        guaranteeDesc = x.guaranteeDesc,

                        EmployeeId = x.EmployeeId,
                        Employee = new EmployeeViewModel
                        {
                            Id = x.Id,
                            Name = x.Employee.Name
                        }


                    }).AsNoTracking()
                    .ToListAsync();
                return Ok(data);
            }


            [HttpGet("{id}")]
            public async Task<IActionResult> Get(Guid id)
            {
                var data = await _context.BondLoads
                    .Where(x => x.Id == id)
                    .Select(x => new BondLoadViewModel
                    {
                        Id = x.Id,
                       /* ProjectId = x.ProjectId,
                        Project = new ProjectViewModel
                        {

                            Name = x.Project.Name
                        },*/
                        BondGuarantorId = x.BondGuarantorId,
                        BondGuarantor = new BondGuarantorViewModel
                        {

                            Name = x.BondGuarantor.Name
                        },

                        BudgetTitleId = x.BudgetTitleId,
                        BudgetTitle = new BudgetTitleViewModel
                        {

                            Name = x.BudgetTitle.Name
                        },

                        BondTypeId = x.BondTypeId,
                        BondType = new BondTypeViewModel
                        {

                            Name = x.BondType.Name
                        },
                        BankId = x.BankId,
                        Bank = new BankViewModel
                        {

                            Name = x.Bank.Name
                        },

                        BondNumber = x.BondNumber,

                        BondRenovationId = x.BondRenovationId,
                        CreateDate = x.CreateDate,
                       EmployeeId = x.EmployeeId,
                        Employee = new EmployeeViewModel
                        {
                            Id = x.Id,
                            Name = x.Employee.Name
                        },


                        UsdAmmount = x.UsdAmmount,
                        daysLimitTerm = x.daysLimitTerm,
                        guaranteeDesc = x.guaranteeDesc
                    }).AsNoTracking()
                    .FirstOrDefaultAsync();
                return Ok(data);
            }

            [HttpPost("crear")]
            public async Task<IActionResult> Create(BondLoadViewModel model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var bondLoad = new BondLoad
                {
                 //   ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId")),
              
                    BondGuarantorId = model.BondGuarantorId,

                    BudgetTitleId = model.BudgetTitleId,
                    

                    BondTypeId = model.BondTypeId,
                   
                    BankId = model.BankId,
                    

                    BondNumber = model.BondNumber,
                    CreateDate = model.CreateDate,
                    BondRenovationId = model.BondRenovationId,
                  
                    PenAmmount  = model.PenAmmount,
                    UsdAmmount = model.UsdAmmount,
                    daysLimitTerm = model.daysLimitTerm,
                    guaranteeDesc = model.guaranteeDesc,
                    EmployeeId = model.EmployeeId

                };
                await _context.BondLoads.AddAsync(bondLoad);
                await _context.SaveChangesAsync();
                return Ok();
            }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BondLoadViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var bondLoad = await _context.BondLoads.FindAsync(id);

       //     bondLoad.ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId"));
                bondLoad.BondGuarantorId = model.BondGuarantorId;

                bondLoad.BudgetTitleId = model.BudgetTitleId;


                bondLoad.BondTypeId = model.BondTypeId;

                bondLoad.BankId = model.BankId;
            bondLoad.CreateDate = model.CreateDate;

                bondLoad.BondNumber = model.BondNumber;

                bondLoad.BondRenovationId = model.BondRenovationId;
        
                bondLoad.UsdAmmount = model.UsdAmmount;
                bondLoad.daysLimitTerm = model.daysLimitTerm;
                bondLoad.guaranteeDesc = model.guaranteeDesc;

            bondLoad.EmployeeId = model.EmployeeId;


            await _context.SaveChangesAsync();
                return Ok();
            }

            [HttpDelete("eliminar/{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var bondLoad = await _context.BondLoads.FirstOrDefaultAsync(x => x.Id == id);
                if (bondLoad == null)
                    return BadRequest($"Fianza con Id '{id}' no encontrado.");
                _context.BondLoads.Remove(bondLoad);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }

