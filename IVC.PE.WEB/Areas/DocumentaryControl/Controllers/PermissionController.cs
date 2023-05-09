using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionRenovationViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionViewModels;
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
    [Route("control-documentario/permisos")]
    public class PermissionController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public PermissionController(IvcDbContext context,
             IOptions<CloudStorageCredentials> storageCredentials,
           ILogger<PermissionController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? bankId = null, Guid? bondTypeId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var bonds = await _context.Set<UspPermissions>().FromSqlRaw("execute DocumentaryControl_uspPermissions")
                .IgnoreQueryFilters()
                .ToListAsync();

            bonds = bonds.Where(x => x.ProjectId == pId).ToList();

            //if (projectId.HasValue)
            //    bonds = bonds.Where(x => x.ProjectId == projectId.Value).ToList();

            //if (bankId.HasValue)
            //    bonds = bonds.Where(x => x.BankId == bankId.Value).ToList();

            //if (bondTypeId.HasValue)
            //    bonds = bonds.Where(x => x.BondTypeId == bondTypeId.Value).ToList();

            //if (budgetTitleId.HasValue)
            //    bonds = bonds.Where(x => x.BudgetTitleId == budgetTitleId.Value).ToList();

            return Ok(bonds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var bond = await _context.Permissions
                .Where(x => x.Id == id)
                .Select(x => new PermissionViewModel
                {
                    Id = x.Id,
                    ProjectFormulaId = x.ProjectFormulaId,
                    PrincipalWay = x.PrincipalWay,
                    From = x.From,
                    To = x.To,
                    AuthorizingEntityId = x.AuthorizingEntityId,
                    Length = x.Length,
                    NumberOfPermissions = x.NumberOfPermissions,
                    AuthorizationTypeId = x.AuthorizationTypeId,
                    
                }).FirstOrDefaultAsync();

            var bondRenovation = await _context.PermissionRenovations
                .Where(x => x.PermissionId == bond.Id && x.Order == bond.NumberOfPermissions)
                .Select(x => new PermissionRenovationViewModel
                {
                    Id = x.Id,
                    PermissionId = x.PermissionId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    FileUrl = x.FileUrl,
                    AuthorizationNumber = x.AuthorizationNumber,
                    RenovationTypeId = x.RenovationTypeId,
                    IsTheLast = x.IsTheLast,
                    Order = x.Order
                }).FirstOrDefaultAsync();

            var responsibles = await _context.PermissionRenovationApplicationUsers
                .Where(x => x.PermissionRenovationId == bondRenovation.Id)
                .Select(x => x.UserId)
                .ToListAsync();

            bondRenovation.Responsibles = responsibles;
            bond.PermissionRenovation = bondRenovation;

            return Ok(bond);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PermissionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bondAdd = new Permission
            {
                ProjectFormulaId = model.ProjectFormulaId,
                PrincipalWay = model.PrincipalWay,
                From = model.From,
                To = model.To,
                AuthorizingEntityId = model.AuthorizingEntityId,
                Length = model.Length,
                NumberOfPermissions = 1,
                AuthorizationTypeId = model.AuthorizationTypeId,
                ProjectId = GetProjectId()
            };

            var bondRenovation = new PermissionRenovation
            {
                Permission = bondAdd,
                Order = bondAdd.NumberOfPermissions,
                StartDate = model.PermissionRenovation.StartDate.ToDateTime(),
                EndDate = model.PermissionRenovation.EndDate.ToDateTime(),
                AuthorizationNumber = model.PermissionRenovation.AuthorizationNumber,
                RenovationTypeId = model.PermissionRenovation.RenovationTypeId,
                Days15 = false,
                Days30 = false,
                IsTheLast = model.PermissionRenovation.IsTheLast
            };

            if (model.PermissionRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bondRenovation.FileUrl = await storage.UploadFile(model.PermissionRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
                    System.IO.Path.GetExtension(model.PermissionRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.PERMS,
                    $"permiso_-{bondRenovation.AuthorizationNumber}-{bondRenovation.Order}");
             
            }

            await _context.PermissionRenovationApplicationUsers
                .AddRangeAsync(model.PermissionRenovation.Responsibles
                    .Select(x => new PermissionRenovationApplicationUser
                    {
                        PermissionRenovation = bondRenovation,
                        UserId = x
                    }));

            await _context.PermissionRenovations.AddAsync(bondRenovation);
            await _context.Permissions.AddAsync(bondAdd);
            await _context.SaveChangesAsync();






            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PermissionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bondAdd = await _context.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            bondAdd.ProjectFormulaId = model.ProjectFormulaId;
            bondAdd.PrincipalWay = model.PrincipalWay;
            bondAdd.From = model.From;
            bondAdd.To = model.To;
            bondAdd.AuthorizingEntityId = model.AuthorizingEntityId;
            bondAdd.Length = model.Length;
            bondAdd.AuthorizationTypeId = model.AuthorizationTypeId;

            var bondRen = await _context.PermissionRenovations.FirstOrDefaultAsync(x => x.Id == model.PermissionRenovation.Id);

            bondRen.StartDate = model.PermissionRenovation.StartDate.ToDateTime();
            bondRen.EndDate = model.PermissionRenovation.EndDate.ToDateTime();
            bondRen.AuthorizationNumber = model.PermissionRenovation.AuthorizationNumber;
            bondRen.RenovationTypeId = model.PermissionRenovation.RenovationTypeId;
            bondRen.IsTheLast = model.PermissionRenovation.IsTheLast;
            if (model.PermissionRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bondRen.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{bondRen.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                bondRen.FileUrl = await storage.UploadFile(model.PermissionRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL,
                    System.IO.Path.GetExtension(model.PermissionRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"permiso_-{bondRen.AuthorizationNumber}-{bondRen.Order}");

            }

            var renovationResponsiblesOld = await _context.PermissionRenovationApplicationUsers
                .Where(x => x.PermissionRenovationId == bondRen.Id)
                .ToListAsync();

            _context.PermissionRenovationApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.PermissionRenovationApplicationUsers.AddRangeAsync(
                model.PermissionRenovation.Responsibles.Select(x => new PermissionRenovationApplicationUser
                {
                    PermissionRenovation = bondRen,
                    UserId = x
                }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.Permissions.FirstOrDefaultAsync(x => x.Id == id);

            var bondRens = await _context.PermissionRenovations.Where(x => x.PermissionId == id).ToListAsync();

            var bondUsers = await _context.PermissionRenovationApplicationUsers
                    .Include(x => x.PermissionRenovation)
                    .Where(x => x.PermissionRenovation.PermissionId == bondAdd.Id)
                    .ToListAsync();

            _context.PermissionRenovationApplicationUsers.RemoveRange(bondUsers);

            foreach (var renovation in bondRens)
            {
                if (renovation.FileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                }
            }

            _context.PermissionRenovations.RemoveRange(bondRens);
            _context.Permissions.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
