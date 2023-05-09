using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels;
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
    [Route("equipos/equipo-maquinaria-seguro")]

    public class EquipmentMachInsuranceFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachInsuranceFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachInsuranceFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid equipId)
        {
            if (equipId == Guid.Empty)
                return Ok(new List<EquipmentMachViewModel>());

            var renovations = await _context.EquipmentMachInsuranceFoldings
                .Include(x=>x.InsuranceEntity)
                .Include(x => x.EquipmentMach)
                .Where(x => x.EquipmentMachId == equipId)
                .Select(x => new EquipmentMachInsuranceFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachId = x.EquipmentMachId,
                    InsuranceEntityId = x.InsuranceEntityId,
                    InsuranceEntity = new InsuranceEntityViewModel
                    {
                        Description = x.InsuranceEntity.Description
                    },
                    Number = x.Number,
                    StartDateInsurance = x.StartDateInsurance.HasValue
                        ? x.StartDateInsurance.Value.Date.ToDateString() : String.Empty,
                    EndDateInsurance = x.EndDateInsurance.HasValue
                        ? x.EndDateInsurance.Value.Date.ToDateString() : String.Empty,
                    InsuranceFileUrl = x.InsuranceFileUrl,
                    OrderInsurance = x.OrderInsurance
                })
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachInsuranceFoldings
                .Include(x => x.EquipmentMach)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachInsuranceFoldingViewModel
                {
                    Id = x.Id,

                    EquipmentMachId = x.EquipmentMachId,
                    InsuranceEntityId = x.InsuranceEntityId,
                    Number = x.Number,
                    StartDateInsurance = x.StartDateInsurance.HasValue
                        ? x.StartDateInsurance.Value.Date.ToDateString() : String.Empty,
                    EndDateInsurance = x.EndDateInsurance.HasValue
                        ? x.EndDateInsurance.Value.Date.ToDateString() : String.Empty,
                    InsuranceFileUrl = x.InsuranceFileUrl

                }).FirstOrDefaultAsync();

            renovation.Responsibles = await _context.EquipmentMachInsuranceFoldingApplicationUsers
                .Where(x => x.EquipmentMachInsuranceFoldingId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachInsuranceFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryTransport = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachId)
                .FirstOrDefaultAsync();

            equipmentMachineryTransport.InsuranceNumber++;

            var newRenovation = new EquipmentMachInsuranceFolding()
            {

                EquipmentMachId = model.EquipmentMachId,
                StartDateInsurance = string.IsNullOrEmpty(model.StartDateInsurance)
                    ? (DateTime?)null : model.StartDateInsurance.ToDateTime(),
                EndDateInsurance = string.IsNullOrEmpty(model.EndDateInsurance)
                    ? (DateTime?)null : model.EndDateInsurance.ToDateTime(),
                OrderInsurance = equipmentMachineryTransport.InsuranceNumber,
                Number = model.Number,
                InsuranceEntityId = model.InsuranceEntityId,
                //InsuranceName = model.InsuranceName
            };


            if (model.InsuranceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.InsuranceFileUrl = await storage.UploadFile(model.InsuranceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.InsuranceFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_INSURANCES,
                    $"seguro_{equipmentMachineryTransport.Brand}_{equipmentMachineryTransport.Plate}_{equipmentMachineryTransport.SerieNumber}_{newRenovation.OrderInsurance}");
            }

            await _context.EquipmentMachInsuranceFoldingApplicationUsers.AddRangeAsync(
    model.Responsibles.Select(x => new EquipmentMachInsuranceFoldingApplicationUser
    {
        EquipmentMachInsuranceFolding = newRenovation,
        UserId = x
    }));

            await _context.EquipmentMachInsuranceFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

           
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachInsuranceFoldingViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTransport = await _context.EquipmentMachs
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProvider)
                .Where(x => x.Id == model.EquipmentMachId)
                .FirstOrDefaultAsync();

            var folding = await _context.EquipmentMachInsuranceFoldings
    .FirstOrDefaultAsync(x => x.Id == id);

            folding.StartDateInsurance = string.IsNullOrEmpty(model.StartDateInsurance)
                    ? (DateTime?)null : model.StartDateInsurance.ToDateTime();
            folding.EndDateInsurance = string.IsNullOrEmpty(model.EndDateInsurance)
                ? (DateTime?)null : model.EndDateInsurance.ToDateTime();
            folding.Number = model.Number;
            folding.InsuranceEntityId = model.InsuranceEntityId;

            if (model.InsuranceFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (folding.InsuranceFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_INSURANCES}/{folding.InsuranceFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
                folding.InsuranceFileUrl = await storage.UploadFile(model.InsuranceFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.InsuranceFile.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACH_INSURANCES,
                    $"seguro_{equipmentMachineryTransport.Brand}_{equipmentMachineryTransport.Plate}_{equipmentMachineryTransport.SerieNumber}_{folding.OrderInsurance}");
            }

            var renovationResponsiblesOld = await _context.EquipmentMachInsuranceFoldingApplicationUsers
                .Where(x => x.EquipmentMachInsuranceFoldingId == folding.Id)
                .ToListAsync();

            _context.EquipmentMachInsuranceFoldingApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.EquipmentMachInsuranceFoldingApplicationUsers.AddRangeAsync(
                model.Responsibles.Select(x => new EquipmentMachInsuranceFoldingApplicationUser
                {
                    EquipmentMachInsuranceFolding = folding,
                    UserId = x
                }));



            await _context.SaveChangesAsync();

            

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.EquipmentMachInsuranceFoldings.FirstOrDefaultAsync(x => x.Id == id);



            var users = await _context.EquipmentMachInsuranceFoldingApplicationUsers.Where(x => x.EquipmentMachInsuranceFoldingId == renovation.Id).ToListAsync();

            if (users.Count() > 0)
                _context.EquipmentMachInsuranceFoldingApplicationUsers.RemoveRange(users);

            if (renovation.InsuranceFileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_INSURANCES}/{renovation.InsuranceFileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
            }

            var equipment = await _context.EquipmentMachs
    .FirstOrDefaultAsync(x => x.Id == renovation.EquipmentMachId);
            equipment.InsuranceNumber--;



            _context.EquipmentMachInsuranceFoldings.Remove(renovation);

            await _context.SaveChangesAsync();

            
            return Ok();
        }
    }
}
