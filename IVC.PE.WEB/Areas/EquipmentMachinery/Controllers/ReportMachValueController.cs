using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryReportViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/valorizacion-maquinaria-reporte")]
    public class ReportMachValueController : BaseController
    {
        public ReportMachValueController(IvcDbContext context,
        ILogger<ReportMachValueController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int year, Guid providerId, int month, int? week = null)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@WeekofYear", System.Data.SqlDbType.Int);
            param1.Value = (object)week ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Year", year);
            SqlParameter param3 = new SqlParameter("@Id", providerId);
            SqlParameter param4 = new SqlParameter("@Month", month);

            var data = await _context.Set<UspMachValTable>().FromSqlRaw("execute EquipmentMachinery_uspMachValTable @WeekofYear, @Year, @Id, @Month"
                , param1
                , param2
                , param3
                , param4)

                .IgnoreQueryFilters()

         .ToListAsync()
         ;


            var provider = data.Select(x => x.TradeName).FirstOrDefault();
            var qt = data.Count();
            var days = 0.00;
            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;
                days += item.AcumulatedHorometer;
                SumaIgv += item.Igv;
                SumaTotalAmmount += item.TotalAmmount;
            }

            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);

            var vm = new List<EquipmentMachReportViewModel>();

            if (data.Count > 0)
            {
                vm.Add(new EquipmentMachReportViewModel
                {
                    provider = provider,
                    str = str,
                    str2 = str2,
                    str3 = str3,
                    acumulated = days,
                    qt = qt
                });
            }


            return Ok(vm);
        }

    }
}
