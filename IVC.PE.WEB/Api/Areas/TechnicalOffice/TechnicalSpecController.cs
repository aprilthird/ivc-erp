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
    [Route("api/oficina-tecnica/especificaciones-tecnicas")]
    public class TechnicalSpecController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public TechnicalSpecController(IvcDbContext context,
            ILogger<TechnicalSpecController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string str,Guid? projectId, Guid? familyId = null, Guid? groupId = null, Guid? specId = null)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");

            SqlParameter param1 = new SqlParameter("@SpecialityId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)specId ?? DBNull.Value;

            var data = await _context.Set<UspTechnicalSpec>().FromSqlRaw("execute TechnicalOffice_uspTechnicalSpec @SpecialityId"
    , param1)
.IgnoreQueryFilters()
.ToListAsync();

            var bps = data
                .Where(X => X.ProjectId == projectId.Value)
                .Select(x => new TechnicalSpecResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    SupplyFamilyId = x.SupplyFamilyId,
                    SupplyGroupId = x.SupplyGroupId,
                    //SpecialityId = x.SpecialityId,
                    FamilyCode = x.SupplyFamilyCode+"-"+x.SupplyFamilyName,
                    GroupCode = x.SupplyGroupCode + "-" + x.SupplyGroupName,
                    SpecDescription = x.Specialities,
                    //SpecDescription = x.SpecialityId.HasValue?x.Speciality.Description:null,
                    FileUrl = x.FileUrl,
                    Name = x.Name
                }).ToList();


            if (familyId.HasValue)
                bps = bps.Where(x => x.SupplyFamilyId == familyId.Value).ToList();
            if (groupId.HasValue)
                bps = bps.Where(x => x.SupplyGroupId == groupId.Value).ToList();
            if (str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();

            return Ok(bps);
        }

    }
}
