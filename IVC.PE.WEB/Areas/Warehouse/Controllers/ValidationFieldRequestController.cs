using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/validacion-pedidos-campo")]
    public class ValidationFieldRequestController : BaseController
    {
        public ValidationFieldRequestController(IvcDbContext context,
            ILogger<ValidationFieldRequestController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? budgetTitleId = null, Guid? projectFormulaId = null,
            Guid? workFrontId = null, Guid? sewerGroupId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            var foldings = await _context.FieldRequestFoldings
                .Include(x => x.GoalBudgetInput.Supply)
                .Include(x => x.GoalBudgetInput.Supply.SupplyGroup)
                .ToListAsync();

            var query = _context.FieldRequests
                .Include(x => x.BudgetTitle)
                //.Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                //.Include(x => x.Warehouse)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.SewerGroup)
                .Include(x => x.SewerGroup.WorkFrontHead.User)
                .Where(x => x.WorkFront.ProjectId == GetProjectId())
                .Where(x => x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ISSUED
                || x.Status == ConstantHelpers.Warehouse.FieldRequest.Status.READYTOVALIDATE);

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);

            //if (projectFormulaId.HasValue)
              //  query = query.Where(x => x.ProjectFormulaId == projectFormulaId);

            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);

            if (sewerGroupId.HasValue)
                query = query.Where(x => x.SewerGroupId == sewerGroupId);

            //if (supplyFamilyId.HasValue)
            //    query = query.Where(x => x.SupplyFamilyId == supplyFamilyId);

            var data = await query
                .Select(x => new FieldRequestViewModel
                {
                    Id = x.Id,
                    DocumentNumber = x.DocumentNumber.ToString("D4"),
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    DeliveryDate = x.DeliveryDate.ToDateString(),
                    //ProjectFormula = new ProjectFormulaViewModel
                    //{
                    //    Name = x.ProjectFormula.Code + " - " + x.ProjectFormula.Name
                    //},
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    SewerGroup = new SewerGroupViewModel
                    {
                        Code = x.SewerGroup.Code,
                        WorkFrontHead = new WorkFrontHeadViewModel
                        {
                            User = new UserViewModel
                            {
                                PaternalSurname = x.SewerGroup.WorkFrontHead.User.PaternalSurname,
                                MaternalSurname = x.SewerGroup.WorkFrontHead.User.MaternalSurname,
                                Name = x.SewerGroup.WorkFrontHead.User.Name,
                                MiddleName = x.SewerGroup.WorkFrontHead.User.MiddleName
                            }
                        }
                    },
                    //Warehouse = new WarehouseViewModel
                    //{
                    //  Address = x.Warehouse.Address
                    //},
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = x.SupplyFamily.Name
                    //},
                    Status = x.Status
                }).ToListAsync();

            foreach (var item in data)
            {
                var folding = foldings.Where(x => x.FieldRequestId == item.Id).ToList();
                item.Groups = string.Join("-",
                   folding.GroupBy(x => x.GoalBudgetInput.Supply.SupplyGroupId)
                    .Select(y => y.FirstOrDefault().GoalBudgetInput.Supply.SupplyGroup.Name));
                var grupo = folding.Where(x => x.GoalBudgetInput.Supply.SupplyGroupId == supplyGroupId);

                if (supplyGroupId.HasValue && grupo == null)
                    data.Remove(item);
            }

            return Ok(data);
        }

        [HttpPut("listo/{id}")]
        public async Task<IActionResult> UpdateStatusReadyToValidate(Guid id, FieldRequestViewModel model)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return BadRequest("No se ha encontrado el requerimiento de campo");

            var items = await _context.FieldRequestFoldings
                .Where(x => x.FieldRequestId == id)
                .ToListAsync();

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.READYTOVALIDATE;

            foreach(var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split("|");

                var itemId = stringSplit[0];
                var itemMeasure = stringSplit[1];

                if (itemId == "undefined" || itemMeasure == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                var item = items.FirstOrDefault(x => x.Id == Guid.Parse(itemId));

                if (itemMeasure.ToDoubleString() > item.Quantity)
                    return BadRequest("Se ha superado el metrado solicitado");

                item.ValidatedQuantity = itemMeasure.ToDoubleString();
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("validar/{id}")]
        public async Task<IActionResult> UpdateStatusValidate(Guid id)
        {
            var request = await _context.FieldRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (request == null)
                return BadRequest("No se ha encontrado el requerimiento de campo");

            request.Status = ConstantHelpers.Warehouse.FieldRequest.Status.VALIDATED;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
