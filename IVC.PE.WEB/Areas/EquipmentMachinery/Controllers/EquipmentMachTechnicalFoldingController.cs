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
    [Route("equipos/equipo-maquinaria-tecnica")]
    public class EquipmentMachTechnicalFoldingController : BaseController
    {

        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachTechnicalFoldingController(IvcDbContext context,
IOptions<CloudStorageCredentials> storageCredentials,
ILogger<EquipmentMachTechnicalFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<EquipmentMachViewModel>());

            var renovations = await _context.EquipmentMachTechnicalRevisions
                .Include(x => x.EquipmentMach)
                .Where(x => x.EquipmentMachId == equipId)
                .Select(x => new EquipmentMachTechnicalFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachId = x.EquipmentMachId,

                    StartDateTechnical = x.StartDateTechnicalRevision.HasValue
                        ? x.StartDateTechnicalRevision.Value.Date.ToDateString() : String.Empty,
                    EndDateTechnical = x.EndDateTechnicalRevision.HasValue
                        ? x.EndDateTechnicalRevision.Value.Date.ToDateString() : String.Empty,
                    TechnicalFileUrl = x.TechnicalRevisionFileUrl,
                    OrderTechnical = x.TechnicalOrder
                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachTechnicalRevisions
                .Include(x => x.EquipmentMach)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachTechnicalFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachId = x.EquipmentMachId,
                    StartDateTechnical = x.StartDateTechnicalRevision.HasValue
                        ? x.StartDateTechnicalRevision.Value.Date.ToDateString() : String.Empty,
                    EndDateTechnical = x.EndDateTechnicalRevision.HasValue
                        ? x.EndDateTechnicalRevision.Value.Date.ToDateString() : String.Empty,
                    TechnicalFileUrl = x.TechnicalRevisionFileUrl

                }).FirstOrDefaultAsync();

            renovation.ResponsiblesTec = await _context.EquipmentMachTechnicalRevisionFoldingApplications
                .Where(x => x.EquipmentMachTechnicalRevisionFoldingId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachTechnicalFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.TechincalNumber++;

            var newRenovation = new EquipmentMachTechnicalRevisionFolding()
            {

                EquipmentMachId = model.EquipmentMachId,
                StartDateTechnicalRevision = string.IsNullOrEmpty(model.StartDateTechnical)
                    ? (DateTime?)null : model.StartDateTechnical.ToDateTime(),
                EndDateTechnicalRevision = string.IsNullOrEmpty(model.EndDateTechnical)
                    ? (DateTime?)null : model.EndDateTechnical.ToDateTime(),

                TechnicalOrder = equipmentMachineryTransport.TechincalNumber

            };



            if (model.TechnicalFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.TechnicalRevisionFileUrl = await storage.UploadFile(model.TechnicalFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.TechnicalFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_TECHNICAL_REVISION,
                    $"seguro_{equipmentMachineryTransport.Brand}_{equipmentMachineryTransport.Plate}_{equipmentMachineryTransport.SerieNumber}_{newRenovation.TechnicalOrder}");
            }

            await _context.EquipmentMachTechnicalRevisionFoldingApplications.AddRangeAsync(
    model.ResponsiblesTec.Select(x => new EquipmentMachTechnicalRevisionFoldingApplicationUser
    {
        EquipmentMachTechnicalRevisionFolding = newRenovation,
        UserId = x
    }));

            await _context.EquipmentMachTechnicalRevisions.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachTechnicalFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTransport = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachId)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachTechnicalRevisions
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.StartDateTechnicalRevision = string.IsNullOrEmpty(model.StartDateTechnical)
                    ? (DateTime?)null : model.StartDateTechnical.ToDateTime();
            folding.EndDateTechnicalRevision = string.IsNullOrEmpty(model.EndDateTechnical)
                ? (DateTime?)null : model.EndDateTechnical.ToDateTime();


            if (model.TechnicalFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.TechnicalRevisionFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_TECHNICAL_REVISION}/{folding.TechnicalRevisionFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
                folding.TechnicalRevisionFileUrl = await storage.UploadFile(model.TechnicalFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.TechnicalFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_TECHNICAL_REVISION,
                    $"seguro_{equipmentMachineryTransport.Brand}_{equipmentMachineryTransport.Plate}_{equipmentMachineryTransport.SerieNumber}_{folding.TechnicalOrder}");
            }



            var renovationResponsiblesOld = await _context.EquipmentMachTechnicalRevisionFoldingApplications
                .Where(x => x.EquipmentMachTechnicalRevisionFoldingId == folding.Id)
                .ToListAsync();

            _context.EquipmentMachTechnicalRevisionFoldingApplications.RemoveRange(renovationResponsiblesOld);

            await _context.EquipmentMachTechnicalRevisionFoldingApplications.AddRangeAsync(
                model.ResponsiblesTec.Select(x => new EquipmentMachTechnicalRevisionFoldingApplicationUser
                {
                    EquipmentMachTechnicalRevisionFolding = folding,
                    UserId = x
                }));



            await _context.SaveChangesAsync();



            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachTechnicalRevisions.FirstOrDefaultAsync(x => x.Id == id);



            var users = await _context.EquipmentMachTechnicalRevisionFoldingApplications.Where(x => x.EquipmentMachTechnicalRevisionFoldingId == renovation.Id).ToListAsync();

            if (users.Count() > 0)
                _context.EquipmentMachTechnicalRevisionFoldingApplications.RemoveRange(users);

            if (renovation.TechnicalRevisionFileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_TECHNICAL_REVISION}/{renovation.TechnicalRevisionFileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
            }

            var equipment = await _context.EquipmentMachs
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachId);
            equipment.TechincalNumber--;



            _context.EquipmentMachTechnicalRevisions.Remove(renovation);

            await _context.SaveChangesAsync();

            var dates = await _context.EquipmentMachTechnicalRevisions.FirstOrDefaultAsync(x => x.TechnicalOrder == equipment.TechincalNumber && x.EquipmentMachId == equipment.Id);


            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
