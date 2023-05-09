using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels;
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
    [Route("equipos/equipo-transporte-soat")]
    public class EquipmentMachineryTransportSOATFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryTransportSOATFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachineryTransportSOATFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<EquipmentMachineryTransportViewModel>());

            var renovations = await _context.EquipmentMachineryTransportSOATFoldings
                .Include(x => x.EquipmentMachineryTransport)
                .Where(x => x.EquipmentMachineryTransportId == equipId)
                .Select(x => new EquipmentMachineryTransportSOATFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTransportId = x.EquipmentMachineryTransportId,

                    StartDateSoat = x.StartDateSOAT.HasValue
                        ? x.StartDateSOAT.Value.Date.ToDateString() : String.Empty,
                    EndDateSoat = x.EndDateSOAT.HasValue
                        ? x.EndDateSOAT.Value.Date.ToDateString() : String.Empty,
                    SoatFileUrl = x.SOATFileUrl,
                    OrderSoat = x.SoatOrder
                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachineryTransportSOATFoldings
                .Include(x => x.EquipmentMachineryTransport)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryTransportSOATFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachineryTransportId = x.EquipmentMachineryTransportId,
                    StartDateSoat = x.StartDateSOAT.HasValue
                        ? x.StartDateSOAT.Value.Date.ToDateString() : String.Empty,
                    EndDateSoat = x.EndDateSOAT.HasValue
                        ? x.EndDateSOAT.Value.Date.ToDateString() : String.Empty,
                    SoatFileUrl = x.SOATFileUrl

                }).FirstOrDefaultAsync();

            renovation.ResponsiblesSoat = await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers
                .Where(x => x.EquipmentMachineryTransportSOATFoldingId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTransportSOATFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachineryTransportId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.SoatNumber++;

            var newRenovation = new EquipmentMachineryTransportSOATFolding()
            {

                EquipmentMachineryTransportId = model.EquipmentMachineryTransportId,
                StartDateSOAT = string.IsNullOrEmpty(model.StartDateSoat)
                    ? (DateTime?)null : model.StartDateSoat.ToDateTime(),
                EndDateSOAT = string.IsNullOrEmpty(model.EndDateSoat)
                    ? (DateTime?)null : model.EndDateSoat.ToDateTime(),
                
                SoatOrder = equipmentMachineryTransport.SoatNumber

            };




            if (model.SoatFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.SOATFileUrl = await storage.UploadFile(model.SoatFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.SoatFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_SOAT,
                    $"seguro_{equipmentMachineryTransport.EquipmentPlate}_{newRenovation.SoatOrder}");
            }

            await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.AddRangeAsync(
    model.ResponsiblesSoat.Select(x => new EquipmentMachineryTransportSOATFoldingApplicationUser
    {
        EquipmentMachineryTransportSOATFolding = newRenovation,
        UserId = x
    }));

            await _context.EquipmentMachineryTransportSOATFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTransportSOATFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTransport = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachineryTransportId)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachineryTransportSOATFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.StartDateSOAT = string.IsNullOrEmpty(model.StartDateSoat)
                    ? (DateTime?)null : model.StartDateSoat.ToDateTime();
            folding.EndDateSOAT = string.IsNullOrEmpty(model.EndDateSoat)
                ? (DateTime?)null : model.EndDateSoat.ToDateTime();
            

            if (model.SoatFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.SOATFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_SOAT}/{folding.SOATFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
                folding.SOATFileUrl = await storage.UploadFile(model.SoatFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.SoatFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_SOAT,
                    $"seguro_{equipmentMachineryTransport.EquipmentPlate}_{folding.SoatOrder}");
            }



            var renovationResponsiblesOld = await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers
                .Where(x => x.EquipmentMachineryTransportSOATFoldingId == folding.Id)
                .ToListAsync();

            _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.AddRangeAsync(
                model.ResponsiblesSoat.Select(x => new EquipmentMachineryTransportSOATFoldingApplicationUser
                {
                    EquipmentMachineryTransportSOATFolding = folding,
                    UserId = x
                }));



            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachineryTransportSOATFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var users = await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.Where(x => x.EquipmentMachineryTransportSOATFoldingId == renovation.Id).ToListAsync();

            if (users.Count() > 0)
                _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.RemoveRange(users);

            if (renovation.SOATFileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_SOAT}/{renovation.SOATFileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
            }

            var equipment = await _context.EquipmentMachineryTransports
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachineryTransportId);
            equipment.SoatNumber--;



            _context.EquipmentMachineryTransportSOATFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            var dates = await _context.EquipmentMachineryTransportSOATFoldings.FirstOrDefaultAsync(x => x.SoatOrder == equipment.SoatNumber && x.EquipmentMachineryTransportId == equipment.Id);



            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}

