using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.Warehouse
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/almacenes/existencias-techos")]
    public class TechoController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public TechoController(IvcDbContext context,
            ILogger<TechoController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null,Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
    Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var query = _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                .Include(x => x.MeasurementUnit)
                .Where(x => x.ProjectFormula.ProjectId == projectId.Value);

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

            var techos = await query
                .Select(x => new TechoResourceModel
                {
                    Id = x.Id,
                    Code = x.Supply.SupplyFamily.Code
                    + x.Supply.SupplyGroup.Code
                    + x.Supply.CorrelativeCode.ToString("D3"),
                    Description = x.Supply.Description,
                    MeasurementUnitAbbreviation = x.MeasurementUnit.Abbreviation,
                    Metered = x.Metered.ToString("N2", CultureInfo.InvariantCulture)
                })
                .ToListAsync();

            return Ok(techos);
        }
    }
}
