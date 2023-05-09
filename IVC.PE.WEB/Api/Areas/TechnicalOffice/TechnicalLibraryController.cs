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
    [Route("api/oficina-tecnica/biblioteca-tecnica")]
    public class TechnicalLibraryController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public TechnicalLibraryController(IvcDbContext context,
            ILogger<TechnicalLibraryController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(
            string str,
             Guid? specId = null
           )
        {


            var bps = await _context.TechnicalLibrarys
                .Include(x => x.Speciality)
                .Select(x => new TechnicalLibraryResourceModel
                {
                    Id = x.Id,

                    //--Spec
                    SpecialityId = x.SpecialityId,
                    SpecDescription = x.Speciality.Description,
                    

                    //-- SelfFields

                    Name = x.Name,
                    Author = x.Author,
                    TechLibraryDate = x.LibraryDate.ToDateString(),
                    FileUrl = x.FileUrl,

                }).ToListAsync();



            if (specId.HasValue)
                bps = bps.Where(x => x.SpecialityId.Value == specId.Value).ToList();
            if (str != null)
                bps = bps.Where(x => x.Name.Contains(str)).ToList();

            return Ok(bps);
        }
    }
}
