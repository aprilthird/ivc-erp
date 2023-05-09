using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.ENTITIES.UspModels.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.PatternCalibrationViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
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

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/patron-calibracion")]
    public class PatternCalibrationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public PatternCalibrationController(IvcDbContext context,
    ILogger<PatternCalibrationController> logger,
    IOptions<CloudStorageCredentials> storageCredentials)
    : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {
            var pId = GetProjectId();

            var data = await _context.Set<UspPatternCalibrations>().FromSqlRaw("execute Quality_uspPatternCalibrations")
                .IgnoreQueryFilters()
                .ToListAsync();

            data = data.Where(x => x.ProjectId == pId).ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.PatternCalibrations
                .Where(x => x.Id == id)
                .Select(x => new PatternCalibrationViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Name = x.Name,
                    NumberOfRenovations = x.NumberOfRenovations
                }).FirstOrDefaultAsync();

            var renewal = await _context.PatternCalibrationRenewals.
                Where(x => x.PatternCalibrationId == data.Id && x.RenewalOrder == data.NumberOfRenovations)
                .Select(x => new PatternCalibrationRenewalViewModel
                {
                    Id = x.Id,
                    PatternCalibrationId = x.PatternCalibrationId,
                    RenewalOrder = x.RenewalOrder,
                    ReferenceNumber = x.ReferenceNumber,
                    CreateDate = x.CreateDate.ToDateString(),
                    EndDate = x.EndDate.ToDateString(),
                    EquipmentCertifyingEntityId = x.EquipmentCertifyingEntityId,
                    Requestioner = x.Requestioner,                   
                    FileUrl = x.FileUrl,
                }).FirstOrDefaultAsync();

            var responsibles = await _context.PatternCalibrationRenewalApplicationUsers
    .Where(x => x.PatternCalibrationRenewalId == renewal.Id)
    .Select(x => x.UserId)
    .ToListAsync();

            renewal.Responsibles = responsibles;
            data.PatternCalibrationRenewal = renewal;

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PatternCalibrationViewModel mod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = new PatternCalibration
            {
                Name = mod.Name,
                ProjectId = GetProjectId(),
                NumberOfRenovations = 1
            };

            var renewal = new PatternCalibrationRenewal
            {
                PatternCalibration = data,
                EndDate = mod.PatternCalibrationRenewal.EndDate.ToDateTime(),
                RenewalOrder = data.NumberOfRenovations,
                EquipmentCertifyingEntityId = mod.PatternCalibrationRenewal.EquipmentCertifyingEntityId,
                Requestioner = mod.PatternCalibrationRenewal.Requestioner,
                ReferenceNumber = mod.PatternCalibrationRenewal.ReferenceNumber,
                CreateDate = mod.PatternCalibrationRenewal.CreateDate.ToDateTime(),
                Days30 = false,
                Days15 = false
            };

            await _context.PatternCalibrationRenewalApplicationUsers
                .AddRangeAsync(mod.PatternCalibrationRenewal.Responsibles
                    .Select(x => new PatternCalibrationRenewalApplicationUser
                    {
                        PatternCalibrationRenewal = renewal,
                        UserId = x
                    }));


            await _context.PatternCalibrationRenewals.AddAsync(renewal);
            await _context.PatternCalibrations.AddAsync(data);
            await _context.SaveChangesAsync();

            if (mod.PatternCalibrationRenewal.File != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                renewal.FileUrl = await storage2.UploadFile(mod.PatternCalibrationRenewal.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(mod.PatternCalibrationRenewal.File.FileName),
                    ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS,
                    $"cp_{data.Id}-ctf_{renewal.RenewalOrder}");

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PatternCalibrationViewModel mod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.PatternCalibrations.FirstOrDefaultAsync(x => x.Id == id);

            data.Name = mod.Name;


            var renewal = await _context.PatternCalibrationRenewals.FirstOrDefaultAsync(x => x.Id == mod.PatternCalibrationRenewal.Id);
            renewal.CreateDate = mod.PatternCalibrationRenewal.CreateDate.ToDateTime();
            renewal.EndDate = mod.PatternCalibrationRenewal.EndDate.ToDateTime();
            renewal.ReferenceNumber = mod.PatternCalibrationRenewal.ReferenceNumber;
            renewal.EquipmentCertifyingEntityId = mod.PatternCalibrationRenewal.EquipmentCertifyingEntityId;
            renewal.Requestioner = mod.PatternCalibrationRenewal.Requestioner;
            


            if (mod.PatternCalibrationRenewal.File != null)
            {
                var storage2 = new CloudStorageService(_storageCredentials);
                if (renewal.FileUrl != null)
                    await storage2.TryDelete($"{ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS}/{renewal.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                renewal.FileUrl = await storage2.UploadFile(mod.PatternCalibrationRenewal.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(mod.PatternCalibrationRenewal.File.FileName),
                    ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS,
                    $"cp_{data.Id}-ctf_{renewal.RenewalOrder}");
            }

            var responsibles = await _context.PatternCalibrationRenewalApplicationUsers
    .Where(x => x.PatternCalibrationRenewalId == renewal.Id)
    .ToListAsync();

            _context.PatternCalibrationRenewalApplicationUsers.RemoveRange(responsibles);

            await _context.PatternCalibrationRenewalApplicationUsers
                .AddRangeAsync(mod.PatternCalibrationRenewal.Responsibles
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
            var data = await _context.PatternCalibrations.FirstOrDefaultAsync(x => x.Id == id);

            var renewals = await _context.PatternCalibrationRenewals
                .Where(x => x.PatternCalibrationId == id)
                .ToListAsync();


            var responsibles = await _context.PatternCalibrationRenewalApplicationUsers
    .Include(x => x.PatternCalibrationRenewal)
    .Where(x => x.PatternCalibrationRenewal.PatternCalibrationId == data.Id)
    .ToListAsync();

            _context.PatternCalibrationRenewalApplicationUsers.RemoveRange(responsibles);

            foreach (var renewal in renewals)
            {
                
                if (renewal.FileUrl != null)
                {
                    var storage2 = new CloudStorageService(_storageCredentials);
                    await storage2.TryDelete($"{ConstantHelpers.Storage.Blobs.CALIBRATION_PATTERNS}/{renewal.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                }
            }

            _context.PatternCalibrationRenewals.RemoveRange(renewals);
            _context.PatternCalibrations.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
