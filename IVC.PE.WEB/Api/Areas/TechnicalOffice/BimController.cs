using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
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
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.TechnicalOffice
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/oficina-tecnica/bim")]
    public class BimController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BimController(IvcDbContext context,
            ILogger<BimController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string str,Guid? projectId, Guid? workFrontId = null, Guid? formulaId = null)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");

            var bps = await _context.Bims
                
                .Include(x => x.ProjectFormula)
                .Include(x => x.WorkFront)
                .Where(X => X.ProjectId == projectId.Value)
                .Select(x => new BimResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId.Value,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectFormulaCode = x.ProjectFormula.Code,
                    WorkFrontId = x.WorkFrontId,
                    WorkFrontCode = x.WorkFront.Code,
                    FileUrl = x.FileUrl,
                    Name = x.Name
                }).ToListAsync();

           
            if (workFrontId.HasValue)
                bps = bps.Where(x => x.WorkFrontId == workFrontId.Value).ToList();
            if (formulaId.HasValue)
                bps = bps.Where(x => x.ProjectFormulaId == formulaId.Value).ToList();
            if(str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();
            return Ok(bps);
        }
    
    }
}
