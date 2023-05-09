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
    [Route("api/oficina-tecnica/catalogo-de-proveedores")]
    public class ProviderCatalogController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProviderCatalogController(IvcDbContext context,
            ILogger<ProviderCatalogController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(
            string str,
            Guid? projectId,
            Guid? familyId = null
            , Guid? groupId = null
            , Guid? specId = null)
        {


            var bps = await _context.ProviderCatalogs
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .Include(x => x.Speciality)
                .Select(x => new ProviderCatalogResourceModel
                {
                    Id = x.Id,

                    //--SupplyFamily
                    SupplyFamilyId = x.SupplyFamilyId,
                    FamilyDescription = x.SupplyFamily.Code,

                    //--SupplyGroup
                    SupplyGroupId = x.SupplyGroupId,
                    GroupDescription = x.SupplyGroup.Code + " - "+ x.SupplyGroup.Name,

                    //--Speciality
                    SpecialityId = x.SpecialityId.Value,
                    SpecDescription = x.Speciality.Description,

                    ProviderId = x.ProviderId,
                    ProviderDescription = x.Provider.Tradename,

                    //-- SelfFields

                    Name = x.Name,

                    FileUrl = x.FileUrl,

                }).ToListAsync();



            if (familyId.HasValue)
                bps = bps.Where(x => x.SupplyFamilyId == familyId.Value).ToList();
            if (groupId.HasValue)
                bps = bps.Where(x => x.SupplyGroupId == groupId.Value).ToList();
            if (specId.HasValue)
                bps = bps.Where(x => x.SpecialityId == specId.Value).ToList();
            if (str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();
            return Ok(bps);
        }
    }
}
