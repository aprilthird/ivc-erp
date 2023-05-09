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
    [Route("equipos/valorizacion-transporte-reporte")]
    public class ReportTransportValueController : BaseController
    {

        public ReportTransportValueController(IvcDbContext context,
        ILogger<ReportTransportValueController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int year, int month, Guid providerId)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@Year", year);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Id", providerId);


            var data = await _context.Set<UspEquipmentTransportVal>().FromSqlRaw("execute EquipmentMachinery_uspTransportVal @Year, @Month, @Id"
                , param1
                , param2
                , param3)
         .IgnoreQueryFilters()
         .ToListAsync();

            var provider = data.Select(x => x.TradeName).FirstOrDefault();
            var qt = data.Count();
            var days = 0;
            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;
                days += item.FoldingNumber;
                SumaIgv += item.Igv;
                SumaTotalAmmount += item.TotalAmmount;
            }

            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);

            var vm = new List<EquipmentMachineryTransportReportViewModel>();

            if (data.Count > 0)
            {
                vm.Add(new EquipmentMachineryTransportReportViewModel
                {
                    provider = provider,
                    str = str,
                    str2 = str2,
                    str3 = str3,
                    days = days,
                    qt = qt
                });
            }


            return Ok(vm);
        }
    }
}
