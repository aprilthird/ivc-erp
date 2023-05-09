using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
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
    [Route("api/oficina-tecnica/procedimientos")]
    public class ProcedureController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProcedureController(IvcDbContext context,
            ILogger<ProcedureController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(
            string str
            , Guid? processId = null
            , Guid? projectId = null
           , Guid? documentTypeId = null)
        {
            SqlParameter param1 = new SqlParameter("@ProcessId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)processId ?? DBNull.Value;


            var data = await _context.Set<UspProcedureProcess>().FromSqlRaw("execute TechnicalOffice_uspProcedureProcess @ProcessId"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            var bps = data
                //.Include(x => x.Process)
                .Select(x => new ProcedureResourceModel
                {
                    Id = x.Id,
                    Processes = x.Processes,
                    ProjectId = x.ProjectId,
                    DocumentTypeId = x.DocumentTypeId,
                    DocumentType = x.DocumentType,
                    Name = x.Name,
                    FileUrl = x.FileUrl,

                }).ToList();

            if (projectId.HasValue)
                bps = bps.Where(x => x.ProjectId == projectId.Value).ToList();
            if (documentTypeId.HasValue)
                bps = bps.Where(x => x.DocumentTypeId == documentTypeId.Value).ToList();
            if (str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();

            return Ok(bps);
        }
    }
}
