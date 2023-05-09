using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
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
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.TechnicalOffice
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/oficina-tecnica/diseños-de-mezcla")]
    public class MixDesignController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public MixDesignController(IvcDbContext context,
            ILogger<MixDesignController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(
            string str
            ,Guid? projectId
            , Guid? cementTypeId = null
            , Guid? aggTypeId = null
            , Guid? concreteUseId = null
            , Guid? designTypeId = null)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");

            var bps = await _context.MixDesigns
                .Include(x => x.DesignType)
                .Include(x => x.CementType)
                .Include(x => x.AggregateType)
                .Include(x => x.ConcreteUse)
                .Include(x => x.TechnicalVersion)
                .Where(X => X.ProjectId == projectId.Value)
                .Select(x => new MixDesignResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId.Value,
                    //--Cement
                    CementTypeId = x.CementTypeId,
                    CementDescription = "Cemento "+ x.CementType.Description,
                    //--Aggregate

                    AggregateTypeId = x.AggregateTypeId,
                    AggDescription = x.AggregateType.Description,

                    //--ConcreteUse

                    ConcreteUseId = x.ConcreteUseId,
                    ConcreteDescription = "Uso: " + x.ConcreteUse.Description,

                    //--
                    DesignTypeId = x.DesignTypeId,
                    DesignDescription = x.DesignType.Description,

                    //-- SelfFields

                    Name = x.Name,
                    Date = x.DesignDate.ToDateString(),
                    FileUrl = x.FileUrl,

                }).ToListAsync();


            
            if (cementTypeId.HasValue)
                bps = bps.Where(x => x.CementTypeId == cementTypeId.Value).ToList();
            if (aggTypeId.HasValue)
                bps = bps.Where(x => x.AggregateTypeId == aggTypeId.Value).ToList();
            if (concreteUseId.HasValue)
                bps = bps.Where(x => x.ConcreteUseId == concreteUseId.Value).ToList();
            if (designTypeId.HasValue)
                bps = bps.Where(x => x.DesignTypeId == designTypeId.Value).ToList();
            if (str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();


            return Ok(bps);
        }

    }
}
