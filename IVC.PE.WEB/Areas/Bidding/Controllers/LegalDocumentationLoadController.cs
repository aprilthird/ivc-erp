using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentLoadViewModel;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/cargardocumentos")]
    public class LegalDocumentationLoadController : BaseController
    {
        public LegalDocumentationLoadController(IvcDbContext context,
        ILogger<LegalDocumentationLoadController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.LegalDocumentationLoads
                 .AsNoTracking()
                  .AsQueryable();

            //query = query.Where(x => x.ProjectId == GetProjectId());

            var data = await query
                    .Select(x => new LegalDocumentationLoadViewModel
                    {
                        Id = x.Id,
                        /*ProjectId = x.ProjectId,
                        Project = new ProjectViewModel
                        {
                            Id = x.Id,
                            Name = x.Project.Name
                        },*/

                        BusinessId = x.BusinessId,
                        Business = new BusinessViewModel
                        {
                            Id = x.Business.Id,
                            BusinessName = x.Business.BusinessName,
                        },

                        LegalDocumentationTypeId = x.LegalDocumentationTypeId,
                        LegalDocumentationType= new LegalDocumentationTypeViewModel
                        {
                            Id = x.Id,
                            Name = x.LegalDocumentationType.Name
                        },

                        LegalDocumentationRenovationId = x.LegalDocumentationRenovationId,
                        CreateDate = x.CreateDate,
                        DaysLimitTerm = x.DaysLimitTerm,


                    }).AsNoTracking()
                    .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.LegalDocumentationLoads
                .Where(x => x.Id == id)
                .Select(x => new LegalDocumentationLoadViewModel
                {
                    Id = x.Id,
                        /* ProjectId = x.ProjectId,
                         Project = new ProjectViewModel
                         {

                             Name = x.Project.Name
                         },*/

                    BusinessId = x.BusinessId,
                    Business = new BusinessViewModel
                    {
                        BusinessName = x.Business.BusinessName
                    },


                    LegalDocumentationTypeId = x.LegalDocumentationTypeId,
                    LegalDocumentationType = new LegalDocumentationTypeViewModel
                    {

                        Name = x.LegalDocumentationType.Name
                    },


                    LegalDocumentationRenovationId = x.LegalDocumentationRenovationId,
                    CreateDate = x.CreateDate,
                    

                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, LegalDocumentationLoadViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var legalDocumentationLoad = await _context.LegalDocumentationLoads.FindAsync(id);

            //     bondLoad.ProjectId = Guid.Parse(HttpContext.Session.GetString("ProjectId"));



            legalDocumentationLoad.BusinessId = model.BusinessId;

            legalDocumentationLoad.LegalDocumentationTypeId = model.LegalDocumentationTypeId;

            legalDocumentationLoad.CreateDate = model.CreateDate;

            legalDocumentationLoad.LegalDocumentationRenovationId = model.LegalDocumentationRenovationId;

            legalDocumentationLoad.DaysLimitTerm = model.DaysLimitTerm;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var legalDocumentationLoad = await _context.LegalDocumentationLoads.FirstOrDefaultAsync(x => x.Id == id);
            if (legalDocumentationLoad == null)
                return BadRequest($"Documento con Id '{id}' no encontrado.");
            _context.LegalDocumentationLoads.Remove(legalDocumentationLoad);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
