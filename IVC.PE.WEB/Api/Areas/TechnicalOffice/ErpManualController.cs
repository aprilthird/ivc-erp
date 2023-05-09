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
    [Route("api/oficina-tecnica/manuales-erp")]
    public class ErpManualController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ErpManualController(IvcDbContext context,
    ILogger<ErpManualController> logger,
    IOptions<CloudStorageCredentials> storageCredentials)
    : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(
            string str,
                     Guid? moduleId = null
                   )
        {


            var bps = await _context.ErpManuals
                .Include(x => x.AreaModule)
                .Select(x => new ErpManualResourceModel
                {
                    Id = x.Id,

                    //--Spec
                    AreaModuleId = x.AreaModuleId,
                    AreaModuleDescription = x.AreaModule.Description,


                    //-- SelfFields

                    Name = x.Name,
                    FileUrl = x.FileUrl,

                }).ToListAsync();



            if (moduleId.HasValue)
                bps = bps.Where(x => x.AreaModuleId == moduleId.Value).ToList();
            if (str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();

            return Ok(bps);
        }
    }
}
