using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels;
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
    [Route("calidad/patron-calibracion/renovaciones")]
    public class PatternCalibrationRenewalController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public PatternCalibrationRenewalController(IvcDbContext context,
    ILogger<PatternCalibrationRenewalController> logger,
    IOptions<CloudStorageCredentials> storageCredentials) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new List<PatternCalibrationRenewalViewModel>());
            var data = await _context.PatternCalibrationRenewals
                .Include(x => x.PatternCalibration)
                .Where(x => x.PatternCalibrationId == id)
                .Select(x => new PatternCalibrationRenewalViewModel
                {
                    Id = x.Id,
                    PatternCalibrationId = x.PatternCalibrationId,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    ReferenceNumber = x.ReferenceNumber,
                    EquipmentCertifyingEntityId = x.EquipmentCertifyingEntityId ?? Guid.Empty,
                    EquipmentCertifyingEntity = new EquipmentCertifyingEntityViewModel
                    {
                        CertifyingEntityName = x.EquipmentCertifyingEntity.CertifyingEntityName
                    },
                    Requestioner = x.Requestioner,
                    RenewalOrder = x.RenewalOrder
                }).OrderByDescending(x => x.RenewalOrder)
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.PatternCalibrationRenewals
                .Include(x => x.PatternCalibration)
                .Where(x => x.Id == id)
                .Select(x => new PatternCalibrationRenewalViewModel
                {
                    Id = x.Id,
                    RenewalOrder = x.RenewalOrder,
                    PatternCalibrationId = x.PatternCalibrationId,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    ReferenceNumber = x.ReferenceNumber,
                    EquipmentCertifyingEntityId = x.EquipmentCertifyingEntityId ?? Guid.Empty,
                    FileUrl = x.FileUrl,
                    Requestioner = x.Requestioner
                }).FirstOrDefaultAsync();

            data.Responsibles = await _context.PatternCalibrationRenewalApplicationUsers
    .Where(x => x.PatternCalibrationRenewalId == id)
    .Select(x => x.UserId)
    .ToListAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PatternCalibrationRenewalViewModel model)
        {
            if (model.RenewalId == Guid.Empty)
                return BadRequest("No se envio el ID del patron de calibración a renovar");

            var toRenewal = await _context.PatternCalibrationRenewals
                .Include(x => x.PatternCalibration)
                .FirstOrDefaultAsync(x => x.Id == model.RenewalId);

            var patternCalibration = await _context.PatternCalibrations.
                FirstOrDefaultAsync(x => x.Id == toRenewal.PatternCalibrationId);
            patternCalibration.NumberOfRenovations++;

            var data = new PatternCalibrationRenewal()
            {
                PatternCalibrationId = toRenewal.PatternCalibrationId,
                CreateDate = model.CreateDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                ReferenceNumber = model.ReferenceNumber,
                EquipmentCertifyingEntityId = model.EquipmentCertifyingEntityId,
                RenewalOrder = patternCalibration.NumberOfRenovations,
                Requestioner = model.Requestioner,
                Days15 = false,
                Days30 = false
            };

            if (data.CreateDate > data.EndDate)
                return BadRequest("Rango de fechas equivocado.");

            if (model.File != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                data.FileUrl = await storage2.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS,
                    $"cp_{data.PatternCalibrationId}-ctf_{data.RenewalOrder}");
            }

            await _context.PatternCalibrationRenewalApplicationUsers
    .AddRangeAsync(
    model.Responsibles.Select(x => new PatternCalibrationRenewalApplicationUser
    {
        PatternCalibrationRenewal = data,
        UserId = x
    }));

            await _context.PatternCalibrationRenewals.AddAsync(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PatternCalibrationRenewalViewModel mod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renewal = await _context.PatternCalibrationRenewals
                .Include(x => x.PatternCalibration)
                .FirstOrDefaultAsync(x => x.Id == id);

            renewal.CreateDate = mod.CreateDate.ToDateTime();
            renewal.EndDate = mod.EndDate.ToDateTime();
            renewal.ReferenceNumber = mod.ReferenceNumber;
            renewal.EquipmentCertifyingEntityId = mod.EquipmentCertifyingEntityId;
            renewal.Requestioner = mod.Requestioner;

            if (mod.File != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                if (renewal.FileUrl != null)
                    await storage2.TryDelete($"{ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS}/{renewal.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                renewal.FileUrl = await storage2.UploadFile(mod.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(mod.File.FileName),
                    ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS,
                    $"cp_{renewal.PatternCalibrationId}-ctf_{renewal.RenewalOrder}");
            }

            var responsibles = await _context.PatternCalibrationRenewalApplicationUsers
    .Where(x => x.PatternCalibrationRenewalId == renewal.Id)
    .ToListAsync();

            _context.PatternCalibrationRenewalApplicationUsers.RemoveRange(responsibles);

            await _context.PatternCalibrationRenewalApplicationUsers
                .AddRangeAsync(mod.Responsibles
                    .Select(x => new PatternCalibrationRenewalApplicationUser
                    {
                        PatternCalibrationRenewal = renewal,
                        UserId = x
                    }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.PatternCalibrationRenewals.FirstOrDefaultAsync(x => x.Id == id);

            if (data.RenewalOrder == 1)
            {
                return BadRequest("No se puede eliminar la inicial.");
            }

            var responsibles = await _context.PatternCalibrationRenewalApplicationUsers
    .Where(x => x.PatternCalibrationRenewalId == data.Id)
    .ToListAsync();

            if (responsibles.Count() > 0)
                _context.PatternCalibrationRenewalApplicationUsers.RemoveRange(responsibles);



            if (data.FileUrl != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                await storage2.TryDelete($"{ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS}/{data.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.QUALITY);
            }

            _context.PatternCalibrationRenewals.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
