using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels;
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
    [Route("equipos/equipo-maquinaria-soat")]
    public class EquipmentMachSOATFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachSOATFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachSOATFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<EquipmentMachViewModel>());

            var renovations = await _context.EquipmentMachSOATFoldings
                .Include(x => x.EquipmentMach)
                .Where(x => x.EquipmentMachId == equipId)
                .Select(x => new EquipmentMachSOATFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachId = x.EquipmentMachId,

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
            var renovation = await _context.EquipmentMachSOATFoldings
                .Include(x => x.EquipmentMach)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachSOATFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachId = x.EquipmentMachId,
                    StartDateSoat = x.StartDateSOAT.HasValue
                        ? x.StartDateSOAT.Value.Date.ToDateString() : String.Empty,
                    EndDateSoat = x.EndDateSOAT.HasValue
                        ? x.EndDateSOAT.Value.Date.ToDateString() : String.Empty,
                    SoatFileUrl = x.SOATFileUrl

                }).FirstOrDefaultAsync();

            renovation.ResponsiblesSoat = await _context.EquipmentMachSOATFoldingApplicationUsers
                .Where(x => x.EquipmentMachSOATFoldingId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachSOATFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.SoatNumber++;

            var newRenovation = new EquipmentMachSOATFolding()
            {

                EquipmentMachId = model.EquipmentMachId,
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
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_SOAT,
                    $"seguro_{equipmentMachineryTransport.Brand}_{equipmentMachineryTransport.Plate}_{equipmentMachineryTransport.SerieNumber}_{newRenovation.SoatOrder}");
            }

            await _context.EquipmentMachSOATFoldingApplicationUsers.AddRangeAsync(
    model.ResponsiblesSoat.Select(x => new EquipmentMachSOATFoldingApplicationUser
    {
        EquipmentMachSOATFolding = newRenovation,
        UserId = x
    }));

            await _context.EquipmentMachSOATFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachSOATFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTransport = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachId)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachSOATFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.StartDateSOAT = string.IsNullOrEmpty(model.StartDateSoat)
                    ? (DateTime?)null : model.StartDateSoat.ToDateTime();
            folding.EndDateSOAT = string.IsNullOrEmpty(model.EndDateSoat)
                ? (DateTime?)null : model.EndDateSoat.ToDateTime();


            if (model.SoatFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.SOATFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_SOAT}/{folding.SOATFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
                folding.SOATFileUrl = await storage.UploadFile(model.SoatFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.SoatFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_SOAT,
                    $"seguro_{equipmentMachineryTransport.Brand}_{equipmentMachineryTransport.Plate}_{equipmentMachineryTransport.SerieNumber}_{folding.SoatOrder}");
            }



            var renovationResponsiblesOld = await _context.EquipmentMachSOATFoldingApplicationUsers
                .Where(x => x.EquipmentMachSOATFoldingId == folding.Id)
                .ToListAsync();

            _context.EquipmentMachSOATFoldingApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.EquipmentMachSOATFoldingApplicationUsers.AddRangeAsync(
                model.ResponsiblesSoat.Select(x => new EquipmentMachSOATFoldingApplicationUser
                {
                    EquipmentMachSOATFolding = folding,
                    UserId = x
                }));



            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachSOATFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var users = await _context.EquipmentMachSOATFoldingApplicationUsers.Where(x => x.EquipmentMachSOATFoldingId == renovation.Id).ToListAsync();

            if (users.Count() > 0)
                _context.EquipmentMachSOATFoldingApplicationUsers.RemoveRange(users);

            if (renovation.SOATFileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_SOAT}/{renovation.SOATFileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
            }

            var equipment = await _context.EquipmentMachs
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachId);
            equipment.SoatNumber--;



            _context.EquipmentMachSOATFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            var dates = await _context.EquipmentMachSOATFoldings.FirstOrDefaultAsync(x => x.SoatOrder == equipment.SoatNumber && x.EquipmentMachId == equipment.Id);




            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
