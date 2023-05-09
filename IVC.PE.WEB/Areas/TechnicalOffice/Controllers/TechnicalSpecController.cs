using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalSpecViewModels;
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

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/especificaciones-tecnicas")]
    public class TechnicalSpecController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public TechnicalSpecController(IvcDbContext context,
            ILogger<TechnicalSpecController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? familyId = null, Guid? groupId = null, Guid? specId = null)
        {

            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@SpecialityId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)specId ?? DBNull.Value;


            var data = await _context.Set<UspTechnicalSpec>().FromSqlRaw("execute TechnicalOffice_uspTechnicalSpec @SpecialityId"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            data = data.Where(x => x.ProjectId == pId).ToList();
            if(specId.HasValue)
            {
                data = data.Where(x => x.Specialities != null).ToList();
            }

            if (familyId.HasValue)
            {
                data = data.Where(x=>x.SupplyFamilyId == familyId.Value).ToList();
            }

            if (groupId.HasValue)
            {
                data = data.Where(x => x.SupplyGroupId == groupId.Value).ToList();
            }

            //if (specId.HasValue)
            //    data = data.Where(x=>x.SpecialityId == specId.Value).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.TechnicalSpecs

                 .Where(x => x.Id == id)
                 .Select(x => new TechnicalSpecViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     SupplyFamilyId = x.SupplyFamilyId,
                     SupplyFamily = new SupplyFamilyViewModel
                     {
                         Code = x.SupplyFamily.Code
                     },
                     SupplyGroupId = x.SupplyGroupId,
                     SupplyGroup = new SupplyGroupViewModel
                     {
                         Code = x.SupplyGroup.Code
                     },
                     //SpecialityId = x.SpecialityId.Value,
                     //Speciality = new SpecialityViewModel
                     //{
                     //    Description = x.Speciality.Description
                     //},
                     FileUrl = x.FileUrl
                 }).FirstOrDefaultAsync();

            var specialities = await _context.TechnicalSpecSpecialities
.Where(x => x.TechnicalSpecId == id)
.Select(x => x.SpecialityId)
.ToListAsync();

            data.Specialities = specialities;

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TechnicalSpecViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spec = new TechnicalSpec
            {
                Name = model.Name,
                SupplyFamilyId = model.SupplyFamilyId,
                SupplyGroupId = model.SupplyGroupId,
                //SpecialityId = model.SpecialityId,
                ProjectId = GetProjectId()
                

                //TechnicalSpecDateStr = x.TechnicalSpecDate.Date.ToDateString(),

            };

            if (model.Specialities != null)
            await _context.TechnicalSpecSpecialities.AddRangeAsync(
            model.Specialities.Select(x => new TechnicalSpecSpeciality
            {
            TechnicalSpec = spec,
            SpecialityId = x
            }).ToList());

            await _context.TechnicalSpecs.AddAsync(spec);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                spec.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.TECHNICAL_SPEC,
                    $"especificacion_{spec.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TechnicalSpecViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var spec = await _context.TechnicalSpecs
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            spec.Name = model.Name;
            spec.SupplyFamilyId = model.SupplyFamilyId;
            spec.SupplyGroupId = model.SupplyGroupId;
            //spec.SpecialityId = model.SpecialityId;



           


            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (spec.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.TECHNICAL_SPEC}/{spec.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                spec.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.TECHNICAL_SPEC,
                    $"especificacion_{spec.Id}");
            }

            var SpecialitiesDb = await _context.TechnicalSpecSpecialities
.Where(x => x.SpecialityId == id)
.ToListAsync();

            _context.TechnicalSpecSpecialities.RemoveRange(SpecialitiesDb);
            if (model.Specialities != null)
                await _context.TechnicalSpecSpecialities.AddRangeAsync(
                model.Specialities.Select(x => new TechnicalSpecSpeciality
                {
                    TechnicalSpec = spec,
                    SpecialityId = x
                }).ToList());

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spec = await _context.TechnicalSpecs
                .FirstOrDefaultAsync(x => x.Id == id);

            var specialitiesDb = await _context.TechnicalSpecSpecialities
.Where(x => x.TechnicalSpecId == id)
.ToListAsync();

            if (spec == null)
            {
                return BadRequest($"TECHNICAL_SPEC con Id '{id}' no se halló.");
            }

            if (spec.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.TECHNICAL_SPEC}/{spec.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.TechnicalSpecSpecialities.RemoveRange(specialitiesDb);
            _context.TechnicalSpecs.Remove(spec);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
