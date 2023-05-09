using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondRenovationViewModels;
using IVC.PE.WEB.Options;
using Microsoft.Extensions.Options;
using IVC.PE.WEB.Services;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondAddViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.FINANCE)]
    [Route("finanzas/cartas-fianza/renovaciones")]
    public class BondRenovationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public BondRenovationController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
                ILogger<BondRenovationController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid bondId)
        {
            if (bondId == Guid.Empty)
                return Ok(new List<BondRenovationViewModel>());

            var renovations = await _context.BondRenovations
                .Include(x => x.BondAdd)
                .Where(x => x.BondAddId == bondId)
                .Select(x => new BondRenovationViewModel
                {
                    Id = x.Id,
                    BondAddId = x.BondAddId,
                    BondAdd = new BondAddViewModel
                    {
                        BondNumber = x.BondAdd.BondNumber
                    },
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    PenAmmount = x.PenAmmount,
                    UsdAmmount = x.UsdAmmount,
                    currencyType = x.currencyType,
                    daysLimitTerm = x.daysLimitTerm,
                    guaranteeDesc = x.guaranteeDesc,
                    BondOrder = x.BondOrder,
                    IsTheLast = x.IsTheLast,
                    BondName = x.BondName,
                    IssueCost = x.IssueCost,
                    FileUrl = x.FileUrl,
                    IssueFileUrl = x.IssueFileUrl
                }).OrderByDescending(x => x.BondOrder)
                .ToListAsync();

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.BondRenovations
                .Include(x => x.BondAdd)
                .Where(x => x.Id == id)
                .Select(x => new BondRenovationViewModel
                {
                    Id = x.Id,
                    BondOrder = x.BondOrder,
                    BondName = x.BondName,
                    BondAddId = x.BondAddId,
                    BondAdd = new BondAddViewModel
                    {
                        BondNumber = x.BondAdd.BondNumber
                    },
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    PenAmmount = x.PenAmmount,
                    currencyType = x.currencyType,
                    daysLimitTerm = x.daysLimitTerm,
                    guaranteeDesc = x.guaranteeDesc,
                    FileUrl = x.FileUrl,
                    IssueFileUrl = x.IssueFileUrl,
                    IsTheLast = x.IsTheLast,
                    IssueCost = x.IssueCost
                }).FirstOrDefaultAsync();

            renovation.Responsibles = await _context.BondRenovationApplicationUsers
                .Where(x => x.BondRenovationId == id)
                .Select(x => x.UserId)
                .ToListAsync();

            return Ok(renovation);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BondRenovationViewModel model)
        {
            if (model.BondRenovationId == Guid.Empty)
                return BadRequest("No se envió ID de la carta fianza a renovar.");

            var toRenovation = await _context.BondRenovations.Include(x => x.BondAdd).FirstOrDefaultAsync(x => x.Id == model.BondRenovationId);

            var bond = await _context.BondAdds.FirstOrDefaultAsync(x => x.Id == toRenovation.BondAddId);
            bond.NumberOfRenovations++;

            var newRenovation = new BondRenovation()
            {
                BondName = model.BondName,
                BondAddId = toRenovation.BondAddId,
                BondOrder = bond.NumberOfRenovations,
                CreateDate = model.CreateDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                currencyType = model.currencyType,
                PenAmmount = model.PenAmmount,
                UsdAmmount = model.UsdAmmount,
                guaranteeDesc = model.guaranteeDesc,
                daysLimitTerm = 0,
                Days15 = false,
                Days30 = false,
                IssueCost = model.IssueCost,
                IsTheLast = model.IsTheLast
            };

            if (newRenovation.CreateDate > newRenovation.EndDate)
                return BadRequest("Rango de fechas equivocado.");
            if (newRenovation.CreateDate < toRenovation.EndDate)
                return BadRequest("Fecha de Creación de la Renovación no puede ser menor a la Fecha de Fin de la carta fianza a renovar.");

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"carta_fianza_nro{toRenovation.BondAdd.BondNumber}-{newRenovation.BondOrder}");
            }

            if (model.IssueFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                newRenovation.IssueFileUrl = await storage.UploadFile(model.IssueFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.IssueFile.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"costo_emision_nro{toRenovation.BondAdd.BondNumber}-{newRenovation.BondOrder}");
            }

            await _context.BondRenovationApplicationUsers.AddRangeAsync(
                model.Responsibles.Select(x => new BondRenovationApplicationUser
                    {
                        BondRenovation = newRenovation,
                        UserId = x
                    }));

            await _context.BondRenovations.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BondRenovationViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var renovation = await _context.BondRenovations.Include(x => x.BondAdd).FirstOrDefaultAsync(x => x.Id == id);

            renovation.CreateDate = model.CreateDate.ToDateTime();
            renovation.EndDate = model.EndDate.ToDateTime();
            renovation.PenAmmount = model.PenAmmount;
            renovation.currencyType = model.currencyType;
            renovation.daysLimitTerm = model.daysLimitTerm;
            renovation.guaranteeDesc = model.guaranteeDesc;
            renovation.IsTheLast = model.IsTheLast;
            renovation.BondName = model.BondName;
            renovation.IssueCost = model.IssueCost;
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renovation.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.FINANCE);
                renovation.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE, 
                    System.IO.Path.GetExtension(model.File.FileName), 
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"carta_fianza_nro{renovation.BondAdd.BondNumber}-{renovation.BondOrder}");
            }

            if (model.IssueFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renovation.IssueFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{renovation.IssueFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.FINANCE);
                renovation.IssueFileUrl = await storage.UploadFile(model.IssueFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.IssueFile.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"costo_emision_nro{renovation.BondAdd.BondNumber}-{renovation.BondOrder}");
            }

            var renovationResponsiblesOld = await _context.BondRenovationApplicationUsers
                .Where(x => x.BondRenovationId == renovation.Id)
                .ToListAsync();

            _context.BondRenovationApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.BondRenovationApplicationUsers.AddRangeAsync(
                model.Responsibles.Select(x => new BondRenovationApplicationUser
                {
                    BondRenovation = renovation,
                    UserId = x
                }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var renovation = await _context.BondRenovations.FirstOrDefaultAsync(x => x.Id == id);

            if (renovation.BondOrder == 1)
            {
                return BadRequest("No se puede eliminar la inicial.");
            }

            var bondUsers = await _context.BondRenovationApplicationUsers.Where(x => x.BondRenovationId == renovation.Id).ToListAsync();

            if (bondUsers.Count() > 0)
                _context.BondRenovationApplicationUsers.RemoveRange(bondUsers);

            if (renovation.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.FINANCE);
            }

            if (renovation.IssueFileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{renovation.IssueFileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.FINANCE);
            }

            var bondAdd = await _context.BondAdds
                .FirstOrDefaultAsync(x => x.Id == renovation.BondAddId);
            bondAdd.NumberOfRenovations--;

            _context.BondRenovations.Remove(renovation);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}