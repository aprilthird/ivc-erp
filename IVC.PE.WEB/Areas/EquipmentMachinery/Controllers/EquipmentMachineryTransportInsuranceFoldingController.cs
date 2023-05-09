using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.InsuranceEntityViewModels;
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
    [Route("equipos/equipo-transporte-seguro")]

    public class EquipmentMachineryTransportInsuranceFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryTransportInsuranceFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachineryTransportInsuranceFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<EquipmentMachineryTransportViewModel>());

            var renovations = await _context.EquipmentMachineryTransportInsuranceFoldings
                .Include(x => x.EquipmentMachineryTransport)
                .Include(x => x.InsuranceEntity)
                .Where(x => x.EquipmentMachineryTransportId == equipId)
                .Select(x => new EquipmentMachineryTransportInsuranceFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTransportId = x.EquipmentMachineryTransportId,

                    StartDateInsurance = x.StartDateInsurance.HasValue
                        ? x.StartDateInsurance.Value.Date.ToDateString() : String.Empty,
                    EndDateInsurance = x.EndDateInsurance.HasValue
                        ? x.EndDateInsurance.Value.Date.ToDateString() : String.Empty,
                    InsuranceFileUrl = x.InsuranceFileUrl,
                    OrderInsurance = x.OrderInsurance,
                    InsuranceEntityId = x.InsuranceEntityId,
                    InsuranceEntity = new InsuranceEntityViewModel
                    {
                        Description = x.InsuranceEntity.Description
                    },
                    Number = x.Number

                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachineryTransportInsuranceFoldings
                .Include(x => x.EquipmentMachineryTransport)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryTransportInsuranceFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachineryTransportId = x.EquipmentMachineryTransportId,
                    StartDateInsurance = x.StartDateInsurance.HasValue
                        ? x.StartDateInsurance.Value.Date.ToDateString() : String.Empty,
                    EndDateInsurance = x.EndDateInsurance.HasValue
                        ? x.EndDateInsurance.Value.Date.ToDateString() : String.Empty,
                    InsuranceFileUrl = x.InsuranceFileUrl,
                    InsuranceEntityId = x.InsuranceEntityId,
                    Number = x.Number

                }).FirstOrDefaultAsync();

            renovation.Responsibles = await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers
                .Where(x => x.EquipmentMachineryTransportInsuranceFoldingId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTransportInsuranceFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachineryTransportId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.InsuranceNumber++;

            var newRenovation = new EquipmentMachineryTransportInsuranceFolding()
            {

                EquipmentMachineryTransportId = model.EquipmentMachineryTransportId,
                StartDateInsurance = string.IsNullOrEmpty(model.StartDateInsurance)
                    ? (DateTime?)null : model.StartDateInsurance.ToDateTime(),
                EndDateInsurance = string.IsNullOrEmpty(model.EndDateInsurance)
                    ? (DateTime?)null : model.EndDateInsurance.ToDateTime(),
                OrderInsurance = equipmentMachineryTransport.InsuranceNumber,
                InsuranceEntityId = model.InsuranceEntityId,
                Number = model.Number

            };


            if (model.InsuranceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.InsuranceFileUrl = await storage.UploadFile(model.InsuranceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.InsuranceFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_INSURANCES,
                    $"seguro_{equipmentMachineryTransport.EquipmentPlate}_{newRenovation.OrderInsurance}");
            }

            await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.AddRangeAsync(
    model.Responsibles.Select(x => new EquipmentMachineryTransportInsuranceFoldingApplicationUser
    {
        EquipmentMachineryTransportInsuranceFolding = newRenovation,
        UserId = x
    }));

            await _context.EquipmentMachineryTransportInsuranceFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTransportInsuranceFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTransport = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachineryTransportId)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachineryTransportInsuranceFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.Number = model.Number;
            folding.InsuranceEntityId = model.InsuranceEntityId;

            folding.StartDateInsurance = string.IsNullOrEmpty(model.StartDateInsurance)
                    ? (DateTime?)null : model.StartDateInsurance.ToDateTime();
            folding.EndDateInsurance = string.IsNullOrEmpty(model.EndDateInsurance)
                ? (DateTime?)null : model.EndDateInsurance.ToDateTime();


            if (model.InsuranceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.InsuranceFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_INSURANCES}/{folding.InsuranceFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
                folding.InsuranceFileUrl = await storage.UploadFile(model.InsuranceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.InsuranceFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_INSURANCES,
                    $"seguro_{equipmentMachineryTransport.EquipmentPlate}_{folding.OrderInsurance}");
            }

            var renovationResponsiblesOld = await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers
                .Where(x => x.EquipmentMachineryTransportInsuranceFoldingId == folding.Id)
                .ToListAsync();

            _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.AddRangeAsync(
                model.Responsibles.Select(x => new EquipmentMachineryTransportInsuranceFoldingApplicationUser
                {
                    EquipmentMachineryTransportInsuranceFolding = folding,
                    UserId = x
                }));



            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachineryTransportInsuranceFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var users = await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.Where(x => x.EquipmentMachineryTransportInsuranceFoldingId == renovation.Id).ToListAsync();

            if (users.Count() > 0)
                _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.RemoveRange(users);

            if (renovation.InsuranceFileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_INSURANCES}/{renovation.InsuranceFileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
            }

            var equipment = await _context.EquipmentMachineryTransports
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachineryTransportId);
            equipment.InsuranceNumber--;



            _context.EquipmentMachineryTransportInsuranceFoldings.Remove(renovation);

            await _context.SaveChangesAsync();


            return Ok();
        }


    }
}