using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.RateTransportConsumeViewModels;
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
    [Route("equipos/parte-combustible-transporte-ratio")]
    public class EquipmentMachineryFuelTransportRateController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryFuelTransportRateController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
    ILogger<EquipmentMachineryFuelTransportRateController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid fuelId)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@Id", fuelId);

            var data = await _context.Set<UspRateConsumeTransport>().FromSqlRaw("execute EquipmentMachinery_uspRateConsumeTransport @Id",
            param1)
         .IgnoreQueryFilters()
         .ToListAsync();

            var rate = 0.00;
            var invrate = 0.00;
            var week = 0;
            var tempmax = 0.00;
            var vm = new List<RateTransportConsumeViewModel>();
            foreach (var item in data)
            {
                week = item.WeekNumber;
                if (item.Order == 1)
                {
                    if (item.MileageMax - item.MileageMin != 0)
                        rate = item.TotalGallon / (item.MileageMax - item.MileageMin);
                    else
                        rate = 0.00;
                    if (item.TotalGallon != 0)
                        invrate = (item.MileageMax - item.MileageMin) / item.TotalGallon;
                    else
                        invrate = 0.00;

                    tempmax = item.MileageMax;
                }
                else
                {
                    if (item.MileageMax - tempmax != 0)
                        rate = item.TotalGallon / (item.MileageMax - tempmax);
                    else
                        rate = 0.00;
                    if (item.TotalGallon != 0)
                        invrate = (item.MileageMax - tempmax) / item.TotalGallon;
                    else
                        invrate = 0.00;
                    tempmax = item.MileageMax;
                }

                vm.Add(new RateTransportConsumeViewModel
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
