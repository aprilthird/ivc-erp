using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Aggregation;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.Production
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/produccion/agregados")]
    public class AggregateController : BaseController
    {
        public AggregateController(IvcDbContext context)
            : base(context)
        {
        }

        //-------------------------------------------
        //Requerimientos > Registro
        //-------------------------------------------
        [HttpPost("requerimientos/crear")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAggregateRequest(
            string reqNum, 
            string project,
            string formula,
            string phase,
            string sewergroup,
            string stockName,
            double stockVolume,
            string deliveryDate,
            string turn)
        {
            var dbProject = await _context.Projects.FirstAsync(x => x.Abbreviation.ToUpper().Equals(project.ToUpper()));
            var dbFormula = await _context.ProjectFormulas.FirstAsync(x => x.Code.ToUpper().Equals(formula.ToUpper()) &&
                                                    x.ProjectId == dbProject.Id);
            var dbPhase = await _context.ProjectPhases.FirstAsync(x => x.Code.ToUpper().Equals(phase.ToUpper()) &&
                                                    x.ProjectId == dbProject.Id);
            var dbSewerGroup = await _context.SewerGroups.FirstAsync(x => x.Code.ToUpper().Equals(sewergroup.ToUpper()) &&
                                                    x.ProjectId == dbProject.Id);
            var dbStock = await _context.AggregationStocks.FirstAsync(x => x.Description.ToUpper().Equals(stockName.ToUpper()));

            var request = new AggregationRequest
            {
                RequestNumber = reqNum,
                ProjectId = dbProject.Id,
                ProjectFormulaId = dbFormula.Id,
                ProjectPhaseId = dbPhase.Id,
                SewerGroupId = dbSewerGroup.Id,
                AggregationStockId = dbStock.Id,
                Volume = stockVolume,
                DeliveryDate = deliveryDate.ToDateTime(),
                Turn = turn,
                Status = true,
                RegistrationDate = DateTime.Today
            };

            await _context.AggregationRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            return Ok(request.Id);
        }
    }
}
