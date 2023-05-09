using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
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
    [Route("equipos/proveedores-de-combustible-folding-precio")]
    public class FuelProviderPriceFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public FuelProviderPriceFoldingController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
                ILogger<FuelProviderPriceFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }



        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentProviderId = null)
        {
            if (!equipmentProviderId.HasValue)
                return Ok(new List<FuelProviderPriceFoldingViewModel>());

            var folding = await _context.FuelProviderPriceFoldings
                .Include(x => x.FuelProvider)
                .Where(x => x.FuelProviderId == equipmentProviderId)
                .Select(x => new FuelProviderPriceFoldingViewModel
                {
                    Id = x.Id,
                    FuelProviderId = x.FuelProviderId,
                    Price = x.Price,
                    PublicationDate = x.PublicationDate.ToDateString(),
                    Order = x.Order,
                    FileUrl = x.FileUrl
                }).ToListAsync();

            return Ok(folding);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var equip = await _context.FuelProviderPriceFoldings
                .Where(x => x.Id == id)
                .Select(x => new FuelProviderPriceFoldingViewModel
                {
                    Id = x.Id,
                    FuelProviderId = x.FuelProviderId,
                    Price = x.Price,
                    PublicationDate = x.PublicationDate.ToDateString(),
                    Order = x.Order,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();

            return Ok(equip);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FuelProviderPriceFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var fuel = await _context.FuelProviders.Where(x => x.Id == model.FuelProviderId).FirstOrDefaultAsync();

            fuel.PriceNumber++;
            var equip = new FuelProviderPriceFolding
            {

                FuelProviderId = model.FuelProviderId,
                Price = model.Price,
                PublicationDate = model.PublicationDate.ToDateTime(),
                Order = fuel.PriceNumber

            };

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                model.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FUEL_PROVIDERS,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FUEL_PRICES,
                    $"precio-combustible-{equip.Order}-{equip.FuelProviderId}");
            }

            fuel.LastPrice = model.Price;

            await _context.FuelProviderPriceFoldings.AddAsync(equip);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FuelProviderPriceFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fuel = await _context.FuelProviders.Where(x => x.Id == model.FuelProviderId).FirstOrDefaultAsync();

            var equipment = await _context.FuelProviderPriceFoldings
                .FirstOrDefaultAsync(x => x.Id == id);

            equipment.Price = model.Price;
            equipment.PublicationDate = model.PublicationDate.ToDateTime();


            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (equipment.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FUEL_PRICES}/{equipment.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.FUEL_PROVIDERS);
                equipment.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FUEL_PROVIDERS,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.FUEL_PRICES,
  $"precio-combustible-{equipment.Order}-{equipment.FuelProviderId}");
            }

            fuel.LastPrice = model.Price;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.FuelProviderPriceFoldings.FirstOrDefaultAsync(x => x.Id == id);

            var equipment = await _context.FuelProviders
.FirstOrDefaultAsync(x => x.Id == data.FuelProviderId);
            equipment.PriceNumber--;

            _context.FuelProviderPriceFoldings.Remove(data);
            await _context.SaveChangesAsync();
            var dates = await _context.FuelProviderPriceFoldings.FirstOrDefaultAsync(x => x.Order == equipment.PriceNumber && x.FuelProviderId == equipment.Id);

            await _context.SaveChangesAsync();

            if (data.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.FUEL_PRICES}/{data.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.FUEL_PROVIDERS);
            }

            if (equipment.PriceNumber == 0)
            {
                equipment.LastPrice = 0;
            }
            else
            {
                equipment.LastPrice = dates.Price;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
