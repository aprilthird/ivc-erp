using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.TechoViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/techos")]
    public class TechoController : BaseController
    {
        public TechoController(IvcDbContext context, 
            ILogger<TechoController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
            Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var query = _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                .Include(x => x.MeasurementUnit)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId());

            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);

            var aux = query
                .AsEnumerable()
                .GroupBy(x => x.SupplyId)
                .ToList();

            var data = new List<TechoViewModel>();

            foreach(var techo in aux)
            {
                data.Add(new TechoViewModel
                {
                    Id = techo.FirstOrDefault().Id,
                    Code = techo.FirstOrDefault().Supply.SupplyFamily.Code
                    + techo.FirstOrDefault().Supply.SupplyGroup.Code
                    + techo.FirstOrDefault().Supply.CorrelativeCode.ToString("D3"),
                    Description = techo.FirstOrDefault().Supply.Description,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = techo.FirstOrDefault().MeasurementUnit.Abbreviation
                    },
                    Metered = techo.Sum(x => x.Metered).ToString("N2", CultureInfo.InvariantCulture),
                    WarehouseCurrentMetered = techo.Sum(x => x.WarehouseCurrentMetered).ToString("N2", CultureInfo.InvariantCulture),
                    WarehouseAccumulatedMetered = techo.Sum(x => x.WarehouseAccumulatedMetered).ToString("N2", CultureInfo.InvariantCulture)
                });
            }
            /*
            var techos = await query
                .Select(x => new TechoViewModel
                {
                    Id = x.Id,
                    Code = x.Supply.SupplyFamily.Code
                    + x.Supply.SupplyGroup.Code
                    + x.Supply.CorrelativeCode.ToString("D3"),
                    Description = x.Supply.Description,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation
                    },
                    Metered = x.Metered.ToString("N2", CultureInfo.InvariantCulture)
                })
                .ToListAsync();
            */
            return Ok(data);
        }
    }
}
