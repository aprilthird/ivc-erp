using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.RateMachConsumeViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
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

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-combustible-maquinaria-ratio")]
    public class EquipmentMachineryFuelMachRateController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryFuelMachRateController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
    ILogger<EquipmentMachineryFuelMachRateController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid fuelId)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@Id", fuelId);

            var data = await _context.Set<UspRateConsumeMach>().FromSqlRaw("execute EquipmentMachinery_uspRateConsumeMach @Id",
            param1)
         .IgnoreQueryFilters()
         .ToListAsync();

            var rate = 0.00;
            var invrate = 0.00;
            var week = 0;
            var tempmax = 0.00;
            var vm = new List<RateMachConsumeViewModel>();
            foreach (var item in data)
            {
                week = item.WeekNumber;
                if (item.Order == 1)
                {
                    if (item.HorometerMax - item.HorometerMin != 0)
                        rate = item.TotalGallon / (item.HorometerMax - item.HorometerMin);
                    else
                        rate = 0.00;
                    if (item.TotalGallon != 0)
                        invrate = (item.HorometerMax - item.HorometerMin) / item.TotalGallon;
                    else
                        invrate = 0.00;
                    

                    tempmax = item.HorometerMax;
                }
                else
                {
                    if (item.HorometerMax - tempmax != 0)
                        rate = item.TotalGallon / (item.HorometerMax - tempmax);
                    else
                        rate = 0.00;

                    if (item.TotalGallon != 0)
                        invrate = (item.HorometerMax - tempmax) / item.TotalGallon;
                    else
                        invrate = 0.00;

                    tempmax = item.HorometerMax;
                }

                vm.Add(new RateMachConsumeViewModel
                {
                    Week = week,
                    Rate = rate,
                    InvRate = invrate


                });
            }

            return Ok(vm);
        }
    }
}
