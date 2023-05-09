using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionRenovationViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.RenovationTypeViewModels;
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

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/permisos/renovaciones")]
    public class PermissionRenovationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public PermissionRenovationController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
                ILogger<PermissionRenovationController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid bondId)
        {
            if (bondId == Guid.Empty)
                return Ok(new List<PermissionRenovationViewModel>());

            var renovations = await _context.PermissionRenovations
                .Include(x => x.Permission)
                .Include(x => x.RenovationType)
                .Where(x => x.PermissionId == bondId)
                .Select(x => new PermissionRenovationViewModel
                {
                    Id = x.Id,
                    PermissionId = x.PermissionId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    FileUrl = x.FileUrl,
                    AuthorizationNumber = x.AuthorizationNumber,
                    RenovationTypeId = x.RenovationTypeId,
                    RenovationType = new RenovationTypeViewModel 
                    { 
                        Description = x.RenovationType.Description
                    },
                    Order = x.Order

                }).OrderByDescending(x => x.Order)
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.PermissionRenovations
                .Include(x => x.Permission)
                .Where(x => x.Id == id)
                .Select(x => new PermissionRenovationViewModel
                {
                    Id = x.Id,
                    PermissionId = x.PermissionId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    FileUrl = x.FileUrl,
                    AuthorizationNumber = x.AuthorizationNumber,
                    RenovationTypeId = x.RenovationTypeId,
                }).FirstOrDefaultAsync();

            renovation.Responsibles = await _context.PermissionRenovationApplicationUsers
                .Where(x => x.PermissionRenovationId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PermissionRenovationViewModel model)
        {
            if (model.PermissionRenovationId == Guid.Empty)
                return BadRequest("No se envió ID de la carta fianza a renovar.");

            var toRenovation = await _context.PermissionRenovations.Include(x => x.Permission).FirstOrDefaultAsync(x => x.Id == model.PermissionRenovationId);

            var bond = await _context.Permissions.FirstOrDefaultAsync(x => x.Id == toRenovation.PermissionId);
            bond.NumberOfPermissions++;

            var newRenovation = new PermissionRenovation()
            {
                AuthorizationNumber = model.AuthorizationNumber,
                PermissionId = toRenovation.PermissionId,
                Order = bond.NumberOfPermissions,
                StartDate = model.StartDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                Days15 = false,
                Days30 = false,
                RenovationTypeId = model.RenovationTypeId
            };

            
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PERMS,
                    $"permiso-{newRenovation.AuthorizationNumber}-{newRenovation.Order}");
            }

            await _context.PermissionRenovationApplicationUsers.AddRangeAsync(
                model.Responsibles.Select(x => new PermissionRenovationApplicationUser
                {
                    PermissionRenovation = newRenovation,
                    UserId = x
                }));

            await _context.PermissionRenovations.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PermissionRenovationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renovation = await _context.PermissionRenovations.Include(x => x.Permission).FirstOrDefaultAsync(x => x.Id == id);

            renovation.StartDate = model.StartDate.ToDateTime();
            renovation.EndDate = model.EndDate.ToDateTime();
            renovation.AuthorizationNumber = model.AuthorizationNumber;

            renovation.RenovationTypeId = model.RenovationTypeId;

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renovation.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                renovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PERMS,
                    $"permiso-{renovation.AuthorizationNumber}-{renovation.Order}");
            }

            var renovationResponsiblesOld = await _context.PermissionRenovationApplicationUsers
                .Where(x => x.PermissionRenovationId == renovation.Id)
                .ToListAsync();

            _context.PermissionRenovationApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.PermissionRenovationApplicationUsers.AddRangeAsync(
                model.Responsibles.Select(x => new PermissionRenovationApplicationUser
                {
                    PermissionRenovation = renovation,
                    UserId = x
                }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.PermissionRenovations.FirstOrDefaultAsync(x => x.Id == id);

            if (renovation.Order == 1)
            {
                return BadRequest("No se puede eliminar la inicial.");
            }

            var bondUsers = await _context.PermissionRenovationApplicationUsers.Where(x => x.PermissionRenovationId == renovation.Id).ToListAsync();

            if (bondUsers.Count() > 0)
                _context.PermissionRenovationApplicationUsers.RemoveRange(bondUsers);

            if (renovation.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
            }

            var bondAdd = await _context.Permissions
                .FirstOrDefaultAsync(x => x.Id == renovation.PermissionId);
            bondAdd.NumberOfPermissions--;

            _context.PermissionRenovations.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
