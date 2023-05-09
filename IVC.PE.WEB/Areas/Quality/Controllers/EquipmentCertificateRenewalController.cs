using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.QualityFrontViewModels;
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

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/equipos-certificados/renovaciones")]
    public class EquipmentCertificateRenewalController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentCertificateRenewalController(IvcDbContext context,
            ILogger<EquipmentCertificateRenewalController> logger,
            IOptions<CloudStorageCredentials> storageCredentials) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new List<EquipmentCertificateRenewalViewModel>());
            var data = await _context.EquipmentCertificateRenewals
                .Include(x=>x.QualiyFront)
                .Include(x => x.EquipmentCertificate)
                .Where(x => x.EquipmentCertificateId == id)
                .Select(x => new EquipmentCertificateRenewalViewModel
                {
                    Id = x.Id,
                    Observation = x.Observation,
                    EquipmentCertificateId = x.EquipmentCertificateId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    EquipmentCertificateNumber = x.EquipmentCertificateNumber,
                    OperationalStatus = x.OperationalStatus,
                    SituationStatus = x.SituationStatus,
                    EquipmentCertifyingEntityId = x.EquipmentCertifyingEntityId ?? Guid.Empty,
                    EquipmentCertifyingEntity = new EquipmentCertifyingEntityViewModel
                    {
                        CertifyingEntityName = x.EquipmentCertifyingEntity.CertifyingEntityName
                    },
                    EquipmentCertificateUserOperatorId = x.EquipmentCertificateUserOperatorId ?? Guid.Empty,
                    EquipmentCertificateUserOperator = new EquipmentCertificateUserOperatorViewModel
                    {
                        Operator = x.EquipmentCertificateUserOperator.Operator
                    },
                    PatternCalibrationRenewalId = x.PatternCalibrationRenewalId ?? Guid.Empty,
                    PatternCalibrationRenewal = new PatternCalibrationRenewalViewModel
                    {

                        ReferenceNumber = x.PatternCalibrationRenewal.ReferenceNumber
                    },
                    RenewalOrder = x.RenewalOrder,
                    QualityFrontId = x.QualityFrontId.Value,
                    QualityFront = new QualityFrontViewModel
                    {

                        Description = x.QualiyFront.Description
                    },
                    HasAVoid = x.HasAVoid,
                    InspectionType = x.InspectionType,
                    CalibrationMethod = x.CalibrationMethod,
                    CalibrationFrecuency = x.CalibrationFrecuency,
                }).OrderByDescending(x => x.RenewalOrder)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentCertificateRenewals
                .Include(x => x.EquipmentCertificate)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentCertificateRenewalViewModel
                {
                    Id = x.Id,
                    RenewalOrder = x.RenewalOrder,
                    Observation = x.Observation,
                    EquipmentCertificateId = x.EquipmentCertificateId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    EquipmentCertificateNumber = x.EquipmentCertificateNumber,
                    OperationalStatus = x.OperationalStatus,
                    SituationStatus = x.SituationStatus,
                    EquipmentCertifyingEntityId = x.EquipmentCertifyingEntityId ?? Guid.Empty,
                    EquipmentCertificateUserOperatorId = x.EquipmentCertificateUserOperatorId ?? Guid.Empty,
                    PatternCalibrationRenewalId = x.PatternCalibrationRenewalId ?? Guid.Empty,
                    HasAVoid = x.HasAVoid,
                    FileUrl = x.FileUrl,
                    QualityFrontId = x.QualityFrontId.Value,
                    InspectionType = x.InspectionType,
                    CalibrationMethod = x.CalibrationMethod,
                    CalibrationFrecuency = x.CalibrationFrecuency,
                }).FirstOrDefaultAsync();

            data.Responsibles = await _context.EquipmentCertificateRenewalApplicationUsers
                .Where(x => x.EquipmentCertificateRenewalId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("patron/{id}")]
        public async Task<IActionResult> GetPattern(Guid id)
        {
            var data = await _context.PatternCalibrationRenewals
                .Where(x => x.Id == id)
                .Select(x => new PatternCalibrationRenewalViewModel
                {
                    Id = x.Id,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();



            return Ok(data);
        }


        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertificateRenewalViewModel model)
        {
            if (model.RenewalId == Guid.Empty)
                return BadRequest("No se envio el ID del certificado de equipo a renovar");

            var toRenewal = await _context.EquipmentCertificateRenewals
                .Include(x=>x.EquipmentCertificate)
                .FirstOrDefaultAsync(x => x.Id == model.RenewalId);

            var equipmentCertificate = await _context.EquipmentCertificates.
                FirstOrDefaultAsync(x=>x.Id == toRenewal.EquipmentCertificateId);
            equipmentCertificate.NumberOfRenovations++;

            var data = new EquipmentCertificateRenewal()
            {
                
                EquipmentCertificateId = toRenewal.EquipmentCertificateId,
                Observation = model.Observation,
                StartDate = model.StartDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                EquipmentCertificateNumber = model.EquipmentCertificateNumber,
                OperationalStatus = model.OperationalStatus,
                SituationStatus = model.SituationStatus,
                EquipmentCertifyingEntityId = model.EquipmentCertifyingEntityId,
                EquipmentCertificateUserOperatorId = model.EquipmentCertificateUserOperatorId,
                PatternCalibrationRenewalId = model.PatternCalibrationRenewalId,
                RenewalOrder = equipmentCertificate.NumberOfRenovations,
                HasAVoid = false,
                Days30 = false,
                Days15 = false,
                QualityFrontId = model.QualityFrontId.Value,
                InspectionType = model.InspectionType,
                CalibrationMethod = model.CalibrationMethod,
                CalibrationFrecuency = model.CalibrationFrecuency,

            };

            if (toRenewal.EndDate.Date != data.StartDate.Date)
            {
                data.HasAVoid = true;
            }

            if (data.StartDate > data.EndDate)
                return BadRequest("Rango de fechas equivocado.");

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                data.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES,
                    $"eqp_{data.EquipmentCertificateId}-ctf_{data.RenewalOrder}");
            }



            await _context.EquipmentCertificateRenewalApplicationUsers
                .AddRangeAsync(
                model.Responsibles.Select(x => new EquipmentCertificateRenewalApplicationUser
                {
                    EquipmentCertificateRenewal = data,
                    UserId = x
                }));

            await _context.EquipmentCertificateRenewals.AddAsync(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentCertificateRenewalViewModel mod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renewal = await _context.EquipmentCertificateRenewals
                .Include(x => x.EquipmentCertificate)
                .FirstOrDefaultAsync(x => x.Id == id);

            renewal.Observation = mod.Observation;
            renewal.StartDate = mod.StartDate.ToDateTime();
            renewal.EndDate = mod.EndDate.ToDateTime();
            renewal.EquipmentCertificateNumber = mod.EquipmentCertificateNumber;
            renewal.OperationalStatus = mod.OperationalStatus;
            renewal.EquipmentCertifyingEntityId = mod.EquipmentCertifyingEntityId;
            renewal.EquipmentCertificateUserOperatorId = mod.EquipmentCertificateUserOperatorId;
            renewal.PatternCalibrationRenewalId = mod.PatternCalibrationRenewalId;
            renewal.SituationStatus = mod.SituationStatus;
            renewal.QualityFrontId = mod.QualityFrontId.Value;
            renewal.InspectionType = mod.InspectionType;
            renewal.CalibrationMethod = mod.CalibrationMethod;
            renewal.CalibrationFrecuency = mod.CalibrationFrecuency;

            var prevRenewal = await _context.EquipmentCertificateRenewals
                .FirstOrDefaultAsync(x => x.EquipmentCertificateId == renewal.EquipmentCertificateId && x.RenewalOrder == renewal.RenewalOrder - 1);

            if (renewal.RenewalOrder > 1) { 
            if (prevRenewal.EndDate.Date != renewal.StartDate.Date)
            {
                renewal.HasAVoid = true;
            }
            else
            {
                renewal.HasAVoid = false;
            }
        }
            if (mod.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renewal.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES}/{renewal.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                renewal.FileUrl = await storage.UploadFile(mod.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(mod.File.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES,
                    $"eqp_{renewal.EquipmentCertificateId}-ctf_{renewal.RenewalOrder}");
            }


            var responsibles = await _context.EquipmentCertificateRenewalApplicationUsers
                .Where(x => x.EquipmentCertificateRenewalId == renewal.Id)
                .ToListAsync();

            _context.EquipmentCertificateRenewalApplicationUsers.RemoveRange(responsibles);

            await _context.EquipmentCertificateRenewalApplicationUsers
                .AddRangeAsync(mod.Responsibles
                    .Select(x => new EquipmentCertificateRenewalApplicationUser
                    {
                        EquipmentCertificateRenewal = renewal,
                        UserId = x
                    }));



            await _context.SaveChangesAsync();

            

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.EquipmentCertificateRenewals.FirstOrDefaultAsync(x => x.Id == id);

            if (data.RenewalOrder == 1)
            {
                return BadRequest("No se puede eliminar la inicial.");
            }

            var responsibles = await _context.EquipmentCertificateRenewalApplicationUsers
                .Where(x => x.EquipmentCertificateRenewalId == data.Id)
                .ToListAsync();

            if (responsibles.Count() > 0)
                _context.EquipmentCertificateRenewalApplicationUsers.RemoveRange(responsibles);


            if (data.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES}/{data.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            var equip = await _context.EquipmentCertificates
                .FirstOrDefaultAsync(x=>x.Id == data.EquipmentCertificateId);
            equip.NumberOfRenovations--;

            _context.EquipmentCertificateRenewals.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
