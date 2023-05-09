using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelMachPartViewModels;
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
    [Route("equipos/parte-combustible-maquinaria-folding")]

    public class EquipmentMachineryFuelMachPartFoldingController : BaseController
    {

        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryFuelMachPartFoldingController(IvcDbContext context,
IOptions<CloudStorageCredentials> storageCredentials,
ILogger<EquipmentMachineryFuelMachPartFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid softPartId, int? month = null, int? year = null, int? week = null)
        {
            if (softPartId == Guid.Empty)
                return Ok(new List<EquipmentMachineryFuelMachPartViewModel>());


            var data = await _context.Set<UspFuelMachFolding>().FromSqlRaw("execute EquipmentMachinery_uspFuelMachFolding")
.IgnoreQueryFilters()
.ToListAsync();
            data = data.Where(x => x.EquipmentMachineryFuelMachPartId == softPartId).ToList();
            data = data.OrderBy(x => x.Order).ToList();


            if (year.HasValue)
            {
                data = data.Where(x => x.YearPartDate == year.Value).ToList();
            }


            if (month.HasValue)
            {
                data = data.Where(x => x.MonthPartDate == month.Value).ToList();
            }

            if (week.HasValue)
            {
                data = data.Where(x => x.WeekPartDate == week.Value).ToList();
            }
            //if (week == 1)
            //    data = data.Where(x => x.WeekPartDate == 1).ToList();
            //if (week == 2)
            //    data = data.Where(x => x.WeekPartDate == 2).ToList();
            //if (week == 3)
            //    data = data.Where(x => x.WeekPartDate == 3).ToList();
            //if (week == 4)
            //    data = data.Where(x => x.WeekPartDate == 4).ToList();
            //if (week == 5)
            //    data = data.Where(x => x.WeekPartDate == 5).ToList();
            //if (week == 6)
            //    data = data.Where(x => x.WeekPartDate == 6).ToList();
            //if (week == 7)
            //    data = data.Where(x => x.WeekPartDate == 7).ToList();
            //if (week == 8)
            //    data = data.Where(x => x.WeekPartDate == 8).ToList();
            //if (week == 9)
            //    data = data.Where(x => x.WeekPartDate == 9).ToList();
            //if (week == 10)
            //    data = data.Where(x => x.WeekPartDate == 10).ToList();
            //if (week == 11)
            //    data = data.Where(x => x.WeekPartDate == 11).ToList();
            //if (week == 12)
            //    data = data.Where(x => x.WeekPartDate == 12).ToList();
            //if (week == 13)
            //    data = data.Where(x => x.WeekPartDate == 13).ToList();
            //if (week == 14)
            //    data = data.Where(x => x.WeekPartDate == 1).ToList();
            //if (week == 1)
            //    data = data.Where(x => x.WeekPartDate == 14).ToList();
            //if (week == 15)
            //    data = data.Where(x => x.WeekPartDate == 15).ToList();
            //if (week == 16)
            //    data = data.Where(x => x.WeekPartDate == 16).ToList();
            //if (week == 17)
            //    data = data.Where(x => x.WeekPartDate == 17).ToList();
            //if (week == 18)
            //    data = data.Where(x => x.WeekPartDate == 18).ToList();
            //if (week == 19)
            //    data = data.Where(x => x.WeekPartDate == 19).ToList();
            //if (week == 20)
            //    data = data.Where(x => x.WeekPartDate == 20).ToList();
            //if (week == 21)
            //    data = data.Where(x => x.WeekPartDate == 21).ToList();
            //if (week == 22)
            //    data = data.Where(x => x.WeekPartDate == 22).ToList();
            //if (week == 23)
            //    data = data.Where(x => x.WeekPartDate == 23).ToList();
            //if (week == 24)
            //    data = data.Where(x => x.WeekPartDate == 24).ToList();
            //if (week == 25)
            //    data = data.Where(x => x.WeekPartDate == 25).ToList();
            //if (week == 26)
            //    data = data.Where(x => x.WeekPartDate == 26).ToList();
            //if (week == 27)
            //    data = data.Where(x => x.WeekPartDate == 27).ToList();
            //if (week == 28)
            //    data = data.Where(x => x.WeekPartDate == 28).ToList();
            //if (week == 29)
            //    data = data.Where(x => x.WeekPartDate == 29).ToList();
            //if (week == 30)
            //    data = data.Where(x => x.WeekPartDate == 30).ToList();
            //if (week == 31)
            //    data = data.Where(x => x.WeekPartDate == 31).ToList();
            //if (week == 32)
            //    data = data.Where(x => x.WeekPartDate == 32).ToList();
            //if (week == 33)
            //    data = data.Where(x => x.WeekPartDate == 33).ToList();
            //if (week == 34)
            //    data = data.Where(x => x.WeekPartDate == 34).ToList();
            //if (week == 35)
            //    data = data.Where(x => x.WeekPartDate == 35).ToList();
            //if (week == 36)
            //    data = data.Where(x => x.WeekPartDate == 36).ToList();
            //if (week == 37)
            //    data = data.Where(x => x.WeekPartDate == 37).ToList();
            //if (week == 38)
            //    data = data.Where(x => x.WeekPartDate == 38).ToList();
            //if (week == 39)
            //    data = data.Where(x => x.WeekPartDate == 39).ToList();
            //if (week == 40)
            //    data = data.Where(x => x.WeekPartDate == 40).ToList();
            //if (week == 41)
            //    data = data.Where(x => x.WeekPartDate == 41).ToList();
            //if (week == 42)
            //    data = data.Where(x => x.WeekPartDate == 42).ToList();
            //if (week == 43)
            //    data = data.Where(x => x.WeekPartDate == 43).ToList();
            //if (week == 44)
            //    data = data.Where(x => x.WeekPartDate == 44).ToList();
            //if (week == 45)
            //    data = data.Where(x => x.WeekPartDate == 45).ToList();
            //if (week == 46)
            //    data = data.Where(x => x.WeekPartDate == 46).ToList();
            //if (week == 47)
            //    data = data.Where(x => x.WeekPartDate == 47).ToList();
            //if (week == 48)
            //    data = data.Where(x => x.WeekPartDate == 48).ToList();
            //if (week == 49)
            //    data = data.Where(x => x.WeekPartDate == 49).ToList();
            //if (week == 50)
            //    data = data.Where(x => x.WeekPartDate == 50).ToList();
            //if (week == 51)
            //    data = data.Where(x => x.WeekPartDate == 51).ToList();
            //if (week == 52)
            //    data = data.Where(x => x.WeekPartDate == 52).ToList();
            //if (week == 53)
            //    data = data.Where(x => x.WeekPartDate == 53).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachineryFuelMachPartFoldings
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryFuelMachPartFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachineryFuelMachPartId = x.EquipmentMachineryFuelMachPartId,
                    PartNumber = x.PartNumber,
                    PartDate = x.PartDate.Date.ToDateString(),
                    PartHour = x.PartHour,
                    Horometer = x.Horometer,
                    Gallon = x.Gallon,
                    FuelProviderId = x.FuelProviderId,
                    FuelProviderFoldingId = x.FuelProviderFoldingId,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                    FuelProviderPriceFoldingId = x.FuelProviderPriceFoldingId
                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryFuelMachPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dates = await _context.ProjectCalendarWeeks.Include(x => x.ProjectCalendar).Where(x => x.ProjectCalendar.ProjectId == GetProjectId()).FirstOrDefaultAsync(x => x.WeekStart <= model.PartDate.ToDateTime() && x.WeekEnd >= model.PartDate.ToDateTime() || x.WeekEnd == model.PartDate.ToDateTime() || x.WeekStart == model.PartDate.ToDateTime());
            var equipmentMachineryTransport = await _context.EquipmentMachineryFuelMachParts
                .Where(x => x.Id == model.EquipmentMachineryFuelMachPartId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.FoldingNumber++;

            var newRenovation = new EquipmentMachineryFuelMachPartFolding()
            {

                EquipmentMachineryFuelMachPartId = model.EquipmentMachineryFuelMachPartId,
                PartNumber = model.PartNumber,
                PartDate = model.PartDate.ToDateTime(),
                PartHour = model.PartHour,
                Horometer = model.Horometer,
                Gallon = model.Gallon,
                Order = equipmentMachineryTransport.FoldingNumber,
                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                FuelProviderId = model.FuelProviderId,
                FuelProviderFoldingId = model.FuelProviderFoldingId,
                FuelProviderPriceFoldingId = model.FuelProviderPriceFoldingId,
                ProjectCalendarWeekId = dates.Id
            };


            equipmentMachineryTransport.AcumulatedGallon += model.Gallon;





            await _context.EquipmentMachineryFuelMachPartFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryFuelMachPartFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dates = await _context.ProjectCalendarWeeks.Include(x => x.ProjectCalendar).Where(x => x.ProjectCalendar.ProjectId == GetProjectId()).FirstOrDefaultAsync(x => x.WeekStart <= model.PartDate.ToDateTime() && x.WeekEnd >= model.PartDate.ToDateTime() || x.WeekEnd == model.PartDate.ToDateTime() || x.WeekStart == model.PartDate.ToDateTime());

            var equipmentMachineryTransport = await _context.EquipmentMachineryFuelMachParts
                .Where(x => x.Id == model.EquipmentMachineryFuelMachPartId)
                .FirstOrDefaultAsync();

            var equipmentMachineryFuel = await _context.EquipmentMachineryFuelMachPartFoldings
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachineryFuelMachPartFoldings
    .FirstOrDefaultAsync(x => x.Id == id);



            folding.PartNumber = model.PartNumber;
            folding.PartDate = model.PartDate.ToDateTime();
            folding.PartHour = model.PartHour;
            folding.Horometer = model.Horometer;
            folding.Gallon = model.Gallon;
            folding.EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId;
            folding.FuelProviderId = model.FuelProviderId;
            folding.FuelProviderFoldingId = model.FuelProviderFoldingId;
            folding.FuelProviderPriceFoldingId = model.FuelProviderPriceFoldingId;

            equipmentMachineryTransport.AcumulatedGallon = equipmentMachineryTransport.AcumulatedGallon - equipmentMachineryFuel.Gallon;
            equipmentMachineryTransport.AcumulatedGallon = equipmentMachineryTransport.AcumulatedGallon + model.Gallon;

            folding.ProjectCalendarWeekId = dates.Id;

            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachineryFuelMachPartFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var equipment = await _context.EquipmentMachineryFuelMachParts
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachineryFuelMachPartId);
            equipment.FoldingNumber--;



            _context.EquipmentMachineryFuelMachPartFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            var dates = await _context.EquipmentMachineryFuelMachPartFoldings.
                FirstOrDefaultAsync(x => x.Order == equipment.FoldingNumber
                && x.EquipmentMachineryFuelMachPartId == equipment.Id);



            if (equipment.FoldingNumber == 0)
            {
                equipment.AcumulatedGallon = 0;
            }
            else
            {
                equipment.AcumulatedGallon = equipment.AcumulatedGallon - dates.Gallon;

            }
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
