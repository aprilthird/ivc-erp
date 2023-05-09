﻿using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
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
    [Route("equipos/costo-obra-maquinaria")]
    public class MachCostWorkNonDetailController : BaseController
    {
        public MachCostWorkNonDetailController(IvcDbContext context,
ILogger<MachCostWorkNonDetailController> logger) : base(context, logger)
        {

        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int? year, int? month, Guid? providerId, Guid? phaseId,Guid? sgId, int? week = null)
        {




            var data = await _context.Set<UspMachCostWorkNonDetail>().FromSqlRaw("execute EquipmentMachinery_uspMachCostWorkNonDetail")
         .IgnoreQueryFilters()
         .ToListAsync();

            if (month == 1)
                data = data.Where(x => x.Month == 1).ToList();
            if (month == 2)
                data = data.Where(x => x.Month == 2).ToList();
            if (month == 3)
                data = data.Where(x => x.Month == 3).ToList();
            if (month == 4)
                data = data.Where(x => x.Month == 4).ToList();
            if (month == 5)
                data = data.Where(x => x.Month == 5).ToList();
            if (month == 6)
                data = data.Where(x => x.Month == 6).ToList();
            if (month == 7)
                data = data.Where(x => x.Month == 7).ToList();
            if (month == 8)
                data = data.Where(x => x.Month == 8).ToList();
            if (month == 9)
                data = data.Where(x => x.Month == 9).ToList();
            if (month == 10)
                data = data.Where(x => x.Month == 10).ToList();
            if (month == 11)
                data = data.Where(x => x.Month == 11).ToList();
            if (month == 12)
                data = data.Where(x => x.Month == 12).ToList();

            if (year == 2021)
                data = data.Where(x => x.Year == 2021).ToList();

            if (year == 2020)
                data = data.Where(x => x.Year == 2020).ToList();

            if (year == 2019)
                data = data.Where(x => x.Year == 2019).ToList();

            if (year == 2018)
                data = data.Where(x => x.Year == 2018).ToList();

            if (providerId.HasValue)
            {
                data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            }

            if (phaseId.HasValue)
            {
                data = data.Where(x => x.MachineryPhaseId == phaseId.Value).ToList();

            }

            if(sgId.HasValue)
            {
                data = data.Where(x => x.SewerGroupId == sgId.Value).ToList();
            }

            if (week.HasValue)
            {
                data = data.Where(x => x.Wk == week.Value).ToList();
            }

            return Ok(data);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Get()
        {


            var data = await _context.Set<UspMachCostWorkNonDetail>().FromSqlRaw("execute EquipmentMachinery_uspMachCostWorkNonDetail")
         .IgnoreQueryFilters()
         .ToListAsync();



            return Ok(data);
        }

        [HttpGet("monto")]
        public async Task<IActionResult> GetTotalInstalled(int? year, int? month, int? week, Guid? providerId, Guid? phaseId, Guid? sgId)
        {



            var data = await _context.Set<UspMachCostWorkNonDetail>().FromSqlRaw("execute EquipmentMachinery_uspMachCostWorkNonDetail")
         .IgnoreQueryFilters()
         .ToListAsync();

            if (month == 1)
                data = data.Where(x => x.Month == 1).ToList();
            if (month == 2)
                data = data.Where(x => x.Month == 2).ToList();
            if (month == 3)
                data = data.Where(x => x.Month == 3).ToList();
            if (month == 4)
                data = data.Where(x => x.Month == 4).ToList();
            if (month == 5)
                data = data.Where(x => x.Month == 5).ToList();
            if (month == 6)
                data = data.Where(x => x.Month == 6).ToList();
            if (month == 7)
                data = data.Where(x => x.Month == 7).ToList();
            if (month == 8)
                data = data.Where(x => x.Month == 8).ToList();
            if (month == 9)
                data = data.Where(x => x.Month == 9).ToList();
            if (month == 10)
                data = data.Where(x => x.Month == 10).ToList();
            if (month == 11)
                data = data.Where(x => x.Month == 11).ToList();
            if (month == 12)
                data = data.Where(x => x.Month == 12).ToList();

            if (year == 2021)
                data = data.Where(x => x.Year == 2021).ToList();

            if (year == 2020)
                data = data.Where(x => x.Year == 2020).ToList();

            if (year == 2019)
                data = data.Where(x => x.Year == 2019).ToList();

            if (year == 2018)
                data = data.Where(x => x.Year == 2018).ToList();

            if (providerId.HasValue)
            {
                data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            }

            if (phaseId.HasValue)
            {
                data = data.Where(x => x.MachineryPhaseId == phaseId.Value).ToList();

            }

            if (sgId.HasValue)
            {
                data = data.Where(x => x.SewerGroupId== sgId.Value).ToList();

            }

            if (week.Value < 9999)
            {
                data = data.Where(x => x.Wk == week.Value).ToList();
            }


            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;

                SumaIgv += item.Igv;
                SumaTotalAmmount += item.Total;
            }
            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);
            return Ok(str);
        }
        [HttpGet("igv")]
        public async Task<IActionResult> GetTotalInstalled2(int? year, int? month, int? week, Guid? providerId, Guid? phaseId, Guid? sgId)
        {


            var data = await _context.Set<UspMachCostWorkNonDetail>().FromSqlRaw("execute EquipmentMachinery_uspMachCostWorkNonDetail")
        .IgnoreQueryFilters()
        .ToListAsync();

            if (month == 1)
                data = data.Where(x => x.Month == 1).ToList();
            if (month == 2)
                data = data.Where(x => x.Month == 2).ToList();
            if (month == 3)
                data = data.Where(x => x.Month == 3).ToList();
            if (month == 4)
                data = data.Where(x => x.Month == 4).ToList();
            if (month == 5)
                data = data.Where(x => x.Month == 5).ToList();
            if (month == 6)
                data = data.Where(x => x.Month == 6).ToList();
            if (month == 7)
                data = data.Where(x => x.Month == 7).ToList();
            if (month == 8)
                data = data.Where(x => x.Month == 8).ToList();
            if (month == 9)
                data = data.Where(x => x.Month == 9).ToList();
            if (month == 10)
                data = data.Where(x => x.Month == 10).ToList();
            if (month == 11)
                data = data.Where(x => x.Month == 11).ToList();
            if (month == 12)
                data = data.Where(x => x.Month == 12).ToList();

            if (year == 2021)
                data = data.Where(x => x.Year == 2021).ToList();

            if (year == 2020)
                data = data.Where(x => x.Year == 2020).ToList();

            if (year == 2019)
                data = data.Where(x => x.Year == 2019).ToList();

            if (year == 2018)
                data = data.Where(x => x.Year == 2018).ToList();

            if (providerId.HasValue)
            {
                data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            }

            if (phaseId.HasValue)
            {
                data = data.Where(x => x.MachineryPhaseId == phaseId.Value).ToList();

            }

            if (sgId.HasValue)
            {
                data = data.Where(x => x.SewerGroupId == sgId.Value).ToList();

            }

            if (week.Value < 9999)
            {
                data = data.Where(x => x.Wk == week.Value).ToList();
            }



            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;

                SumaIgv += item.Igv;
                SumaTotalAmmount += item.Total;
            }
            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);
            return Ok(str2);
        }
        [HttpGet("monto-total")]
        public async Task<IActionResult> GetTotalInstalled3(int? year, int? month, int? week, Guid? providerId, Guid? phaseId, Guid? sgId)
        {
            var data = await _context.Set<UspMachCostWorkNonDetail>().FromSqlRaw("execute EquipmentMachinery_uspMachCostWorkNonDetail")
        .IgnoreQueryFilters()
        .ToListAsync();

            if (month == 1)
                data = data.Where(x => x.Month == 1).ToList();
            if (month == 2)
                data = data.Where(x => x.Month == 2).ToList();
            if (month == 3)
                data = data.Where(x => x.Month == 3).ToList();
            if (month == 4)
                data = data.Where(x => x.Month == 4).ToList();
            if (month == 5)
                data = data.Where(x => x.Month == 5).ToList();
            if (month == 6)
                data = data.Where(x => x.Month == 6).ToList();
            if (month == 7)
                data = data.Where(x => x.Month == 7).ToList();
            if (month == 8)
                data = data.Where(x => x.Month == 8).ToList();
            if (month == 9)
                data = data.Where(x => x.Month == 9).ToList();
            if (month == 10)
                data = data.Where(x => x.Month == 10).ToList();
            if (month == 11)
                data = data.Where(x => x.Month == 11).ToList();
            if (month == 12)
                data = data.Where(x => x.Month == 12).ToList();

            if (year == 2021)
                data = data.Where(x => x.Year == 2021).ToList();

            if (year == 2020)
                data = data.Where(x => x.Year == 2020).ToList();

            if (year == 2019)
                data = data.Where(x => x.Year == 2019).ToList();

            if (year == 2018)
                data = data.Where(x => x.Year == 2018).ToList();

            if (providerId.HasValue)
            {
                data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            }

            if (phaseId.HasValue)
            {
                data = data.Where(x => x.MachineryPhaseId == phaseId.Value).ToList();

            }

            if (sgId.HasValue)
            {
                data = data.Where(x => x.SewerGroupId == sgId.Value).ToList();

            }

            if (week.Value < 9999)
            {
                data = data.Where(x => x.Wk == week.Value).ToList();
            }


            var SumaAmmount = 0.0;
            var SumaIgv = 0.0;
            var SumaTotalAmmount = 0.0;
            foreach (var item in data)
            {
                SumaAmmount += item.Ammount;

                SumaIgv += item.Igv;
                SumaTotalAmmount += item.Total;
            }
            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaAmmount);
            string str2 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaIgv);
            string str3 = String.Format(new CultureInfo("es-PE"), "{0:C}", SumaTotalAmmount);
            return Ok(str3);

            //returnOk(new {str,str2,str3})
        }

        [HttpGet("detalles-listar")]
        public async Task<IActionResult> GetAllDetails(int year, int month, int week,Guid providerId,Guid sgId, Guid mId)
        {

            SqlParameter param1 = new SqlParameter("@WeekofYear", week);


            var data = await _context.Set<UspMachCostWork>().FromSqlRaw("execute EquipmentMachinery_uspMachCostWork @WeekofYear",
                param1)
         .IgnoreQueryFilters()
         .ToListAsync();

            data = data.Where(x => x.Counts != null && x.Up != null).ToList();

            if (month == 1)
                data = data.Where(x => x.Month == 1).ToList();
            if (month == 2)
                data = data.Where(x => x.Month == 2).ToList();
            if (month == 3)
                data = data.Where(x => x.Month == 3).ToList();
            if (month == 4)
                data = data.Where(x => x.Month == 4).ToList();
            if (month == 5)
                data = data.Where(x => x.Month == 5).ToList();
            if (month == 6)
                data = data.Where(x => x.Month == 6).ToList();
            if (month == 7)
                data = data.Where(x => x.Month == 7).ToList();
            if (month == 8)
                data = data.Where(x => x.Month == 8).ToList();
            if (month == 9)
                data = data.Where(x => x.Month == 9).ToList();
            if (month == 10)
                data = data.Where(x => x.Month == 10).ToList();
            if (month == 11)
                data = data.Where(x => x.Month == 11).ToList();
            if (month == 12)
                data = data.Where(x => x.Month == 12).ToList();

            if (year == 2021)
                data = data.Where(x => x.Year == 2021).ToList();

            if (year == 2020)
                data = data.Where(x => x.Year == 2020).ToList();

            if (year == 2019)
                data = data.Where(x => x.Year == 2019).ToList();

            if (year == 2018)
                data = data.Where(x => x.Year == 2018).ToList();

            data = data.Where(x => x.EquipmentProviderId == providerId).ToList();

            data = data.Where(x => x.SewerGroupId == sgId).ToList();

            data = data.Where(x => x.MachineryPhaseId == mId).ToList();


            return Ok(data);
        }
    }
}
