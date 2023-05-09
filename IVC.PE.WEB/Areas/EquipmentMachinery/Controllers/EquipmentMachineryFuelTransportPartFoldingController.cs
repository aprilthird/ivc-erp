using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelTransportPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("equipos/parte-combustible-transporte-folding")]
    public class EquipmentMachineryFuelTransportPartFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryFuelTransportPartFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
    ILogger<EquipmentMachineryFuelTransportPartFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid softPartId, int? month = null, int? year = null, int? week = null)
        {
            if (softPartId == Guid.Empty)
                return Ok(new List<EquipmentMachineryFuelTransportPartViewModel>());

            var data = await _context.Set<UspFuelTransportFolding>().FromSqlRaw("execute EquipmentMachinery_uspFuelTransportFolding")
.IgnoreQueryFilters()
.ToListAsync();
            data = data.Where(x => x.EquipmentMachineryFuelTransportPartId == softPartId).ToList();
            data = data.OrderBy(x => x.Order).ToList();

            if (month == 1)
                data = data.Where(x => x.MonthPartDate == 1).ToList();
            if (month == 2)
                data = data.Where(x => x.MonthPartDate == 2).ToList();
            if (month == 3)
                data = data.Where(x => x.MonthPartDate == 3).ToList();
            if (month == 4)
                data = data.Where(x => x.MonthPartDate == 4).ToList();
            if (month == 5)
                data = data.Where(x => x.MonthPartDate == 5).ToList();
            if (month == 6)
                data = data.Where(x => x.MonthPartDate == 6).ToList();
            if (month == 7)
                data = data.Where(x => x.MonthPartDate == 7).ToList();
            if (month == 8)
                data = data.Where(x => x.MonthPartDate == 8).ToList();
            if (month == 9)
                data = data.Where(x => x.MonthPartDate == 9).ToList();
            if (month == 10)
                data = data.Where(x => x.MonthPartDate == 10).ToList();
            if (month == 11)
                data = data.Where(x => x.MonthPartDate == 11).ToList();
            if (month == 12)
                data = data.Where(x => x.MonthPartDate == 12).ToList();

            if (year.HasValue)
                data = data.Where(x => x.YearPartDate == year.Value).ToList();


            if (week.HasValue)
            {
                data = data.Where(x => x.WeekPartDate == week.Value).ToList();
            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachineryFuelTransportPartFoldings
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryFuelTransportPartFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachineryFuelTransportPartId = x.EquipmentMachineryFuelTransportPartId,
                    PartNumber = x.PartNumber,
                    PartDate = x.PartDate.Date.ToDateString(),
                    PartHour = x.PartHour,
                    Mileage = x.Mileage,
                    Gallon = x.Gallon,
                    FuelProviderId = x.FuelProviderId,
                    FuelProviderFoldingId = x.FuelProviderFoldingId,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                    FuelProviderPriceFoldingId = x.FuelProviderPriceFoldingId,

                }).FirstOrDefaultAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryFuelTransportPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachineryFuelTransportParts
                .Where(x => x.Id == model.EquipmentMachineryFuelTransportPartId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.FoldingNumber++;

            var newRenovation = new EquipmentMachineryFuelTransportPartFolding()
            {

                EquipmentMachineryFuelTransportPartId = model.EquipmentMachineryFuelTransportPartId,
                PartNumber = model.PartNumber,
                PartDate = model.PartDate.ToDateTime(),
                PartHour = model.PartHour,
                Mileage = model.Mileage,
                Gallon = model.Gallon,
                Order = equipmentMachineryTransport.FoldingNumber,
                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                FuelProviderId = model.FuelProviderId,
                FuelProviderFoldingId = model.FuelProviderFoldingId,
                FuelProviderPriceFoldingId = model.FuelProviderPriceFoldingId,
            };

            equipmentMachineryTransport.AcumulatedGallon += model.Gallon;

            await _context.EquipmentMachineryFuelTransportPartFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryFuelTransportPartFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachineryFuelTransportParts
                .Where(x => x.Id == model.EquipmentMachineryFuelTransportPartId)
                .FirstOrDefaultAsync();

            var equipmentMachineryFuel = await _context.EquipmentMachineryFuelTransportPartFoldings
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachineryFuelTransportPartFoldings
    .FirstOrDefaultAsync(x => x.Id == id);



            folding.PartNumber = model.PartNumber;
            folding.PartDate = model.PartDate.ToDateTime();
            folding.PartHour = model.PartHour;
            folding.Mileage = model.Mileage;
            folding.Gallon = model.Gallon;
            folding.EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId;
            folding.FuelProviderId = model.FuelProviderId;
            folding.FuelProviderFoldingId = model.FuelProviderFoldingId;
            folding.FuelProviderPriceFoldingId = model.FuelProviderPriceFoldingId;


            equipmentMachineryTransport.AcumulatedGallon = equipmentMachineryTransport.AcumulatedGallon - equipmentMachineryFuel.Gallon;
            equipmentMachineryTransport.AcumulatedGallon = equipmentMachineryTransport.AcumulatedGallon + model.Gallon;

            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachineryFuelTransportPartFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var equipment = await _context.EquipmentMachineryFuelTransportParts
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachineryFuelTransportPartId);
            equipment.FoldingNumber--;



            _context.EquipmentMachineryFuelTransportPartFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            var dates = await _context.EquipmentMachineryFuelTransportPartFoldings.
                FirstOrDefaultAsync(x => x.Order == equipment.FoldingNumber
                && x.EquipmentMachineryFuelTransportPartId == equipment.Id);



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
