using IVC.PE.BINDINGRESOURCES.Areas.General.Dashboard;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.Dashboard;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.General
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/general/dashboard")]
    public class DashboardController : BaseController
    {
        public DashboardController(IvcDbContext context)
            : base(context)
        {
        }

        [HttpGet("obreros-semana/{id}")]
        public async Task<IActionResult> GetWorkersByWeek(Guid? id = null, int? category = null)
        {
            if (!id.HasValue)
                return Ok(new List<DashboardChartResourceModel>());

            SqlParameter projectParam = new SqlParameter("@ProjectId", id.Value);
            SqlParameter sewerGroupParam = new SqlParameter("@SewerGroupId", System.Data.SqlDbType.UniqueIdentifier);
            SqlParameter categoryParam = new SqlParameter("@Category", System.Data.SqlDbType.Int);
            categoryParam.Value = (object)category ?? DBNull.Value;
            sewerGroupParam.Value = (object)DBNull.Value;

            var workers = await _context.Set<UspWorkersByWeek>().FromSqlRaw("execute Dashboard_uspWorkersByWeek @ProjectId, @SewerGroupId,@Category"
                , projectParam, sewerGroupParam,categoryParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            workers = workers.OrderBy(x => x.YearWeekNumber).ToList();

            var data = workers.Select(x => new DashboardChartResourceModel
            {
                Label = x.YearWeekNumber,
                Value = x.Workers.ToString()
            }).ToList();

            return Ok(data);
        }

        [HttpGet("horas-semana/{id}")]
        public async Task<IActionResult> GetHoursByWeek(Guid? id = null, int? category = null)
        {
            if (!id.HasValue)
                return Ok(new List<DashboardChartResourceModel>());

            SqlParameter projectParam = new SqlParameter("@ProjectId", id.Value);
            SqlParameter sewerGroupParam = new SqlParameter("@SewerGroupId", System.Data.SqlDbType.UniqueIdentifier);
            SqlParameter categoryParam = new SqlParameter("@Category", System.Data.SqlDbType.Int);
            categoryParam.Value = (object)category ?? DBNull.Value;
            //sewerGroupParam.Value = (object)sgId ?? DBNull.Value;
            sewerGroupParam.Value = (object)DBNull.Value;

            var hours = await _context.Set<UspHoursByWeek>().FromSqlRaw("execute Dashboard_uspHoursByWeek @ProjectId, @SewerGroupId, @Category"
                , projectParam, sewerGroupParam,categoryParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            hours = hours.OrderBy(x => x.YearWeekNumber).ToList();

            var data = hours.Select(x => new DashboardChartResourceModel
            {
                Label = x.YearWeekNumber,
                Value = x.Hours.ToString()
            }).ToList();

            return Ok(data);
        }

        [HttpGet("costos-semana/{id}")]
        public async Task<IActionResult> GetCostsByWeek(Guid? id = null, int? category = null)
        {
            if (!id.HasValue)
                return Ok(new List<DashboardChartResourceModel>());

            SqlParameter projectParam = new SqlParameter("@ProjectId", id.Value);
            SqlParameter sewerGroupParam = new SqlParameter("@SewerGroupId", System.Data.SqlDbType.UniqueIdentifier);
            SqlParameter categoryParam = new SqlParameter("@Category", System.Data.SqlDbType.Int);
            categoryParam.Value = (object)category ?? DBNull.Value;
            //sewerGroupParam.Value = (object)sgId ?? DBNull.Value;
            sewerGroupParam.Value = (object)DBNull.Value;

            var costs = await _context.Set<UspCostsByWeek>().FromSqlRaw("execute Dashboard_uspCostsByWeek @ProjectId, @SewerGroupId ,@Category"
                , projectParam, sewerGroupParam, categoryParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            costs = costs.OrderBy(x => x.YearWeekNumber).ToList();

            var data = costs.Select(x => new DashboardChartResourceModel
            {
                Label = x.YearWeekNumber,
                Value = x.TotalCost.ToString()
            }).ToList();

            return Ok(data);
        }
    }
}
