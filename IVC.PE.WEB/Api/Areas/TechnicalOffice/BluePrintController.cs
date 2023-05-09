using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.TechnicalOffice
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/oficina-tecnica/planos")]
    public class BluePrintController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BluePrintController(IvcDbContext context,
            ILogger<BluePrintController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string str,Guid? projectId, Guid? budgetId = null, Guid? workFrontId = null, Guid? formulaId= null, Guid? specId= null,
            Guid? versionId= null, Guid? typeId = null)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");

            var bps = await _context.BlueprintFoldings
                .Include(x => x.Blueprint)
                .Include(x => x.Blueprint.BudgetTitle)
                .Include(x => x.Blueprint.ProjectFormula)
                .Include(x => x.Blueprint.Speciality)
                .Include(x => x.Blueprint.WorkFront)
                .Include(x => x.Blueprint.BlueprintType)
                .Include(x => x.Letter)
                .Where(x => x.Blueprint.ProjectId == projectId.Value)
                .Select(x => new BluePrintListResourceModel
                {
                    Id = x.Id,
                    Sheet = x.Blueprint.Sheet,
                    ProjectId = x.Blueprint.ProjectId.Value,
                    BudgetTitleId = x.Blueprint.BudgetTitleId,
                    WorkFrontId = x.Blueprint.WorkFrontId,
                    ProjectFormulaId = x.Blueprint.ProjectFormulaId,
                    SpecialityId = x.Blueprint.SpecialityId,
                    TechnicalVersionId = x.TechnicalVersionId,
                    BlueprintDate = x.BlueprintDate.HasValue
                    ? x.BlueprintDate.Value.Date.ToDateString() : String.Empty,
                    Description = x.Blueprint.Description,
                    BudgetName = x.Blueprint.BudgetTitle.Name,
                    ProjectFormulaCode = x.Blueprint.ProjectFormula.Code,
                    SpecialityDescription = x.Blueprint.Speciality.Description,
                    TechnicalVersionDescription = x.TechnicalVersion.Description,
                    WorkFrontCode = x.Blueprint.WorkFront.Code,
                    Name = x.Blueprint.Name,
                    FileUrl = x.FileUrl,
                    LetterId = x.Letter.Id,
                    LetterFileUrl = x.Letter.FileUrl,
                    Code = x.Code,
                    Color = x.TechnicalVersion.Description == "Contractual"?"Green":"Blue",
                    BlueprintTypeId = x.Blueprint.BlueprintTypeId.Value

                    

                }).ToListAsync();

            if (budgetId.HasValue)
                bps = bps.Where(x => x.BudgetTitleId == budgetId.Value).ToList();
            if (workFrontId.HasValue)
                bps = bps.Where(x => x.WorkFrontId == workFrontId.Value).ToList();
            if(formulaId.HasValue)
                bps = bps.Where(x => x.ProjectFormulaId == formulaId.Value).ToList();
            if(specId.HasValue)
                bps = bps.Where(x => x.SpecialityId == specId.Value).ToList();
            if(versionId.HasValue)
                bps = bps.Where(x => x.TechnicalVersionId == versionId.Value).ToList();
            if(str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();
            if (typeId.HasValue)
                bps = bps.Where(x => x.BlueprintTypeId == typeId.Value).ToList();
            return Ok(bps);
        }

        [HttpGet("validar-qr")]
        public async Task<IActionResult> GetAll(string qrString)
        {


            SqlParameter param = new SqlParameter("@QrString", qrString);
            var bps = await _context.Set<UspBluePrintsApp>().FromSqlRaw("execute TechnicalOffice_uspBluePrintsApp @QrString"
                , param)
                .IgnoreQueryFilters()
                .ToListAsync();

            var query = bps
                .Select(x => new BluePrintUspResourceModel
                {

                    Code = x.Code,
                    Description = x.Description,
                    FileUrl = x.FileUrl,
                    LetterUrl = x.LetterUrl

                }).ToList();

            return Ok(query);
        }

    }
}
