using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Bidding;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingCurrencyTypeViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkComponentViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkTypeViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessParticipationFoldingViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/obras")]
    public class BiddingWorkController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BiddingWorkController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<BiddingWorkController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? biddingWorkType, int? Period,int ? ivc)
        {
            var query = _context.BiddingWorks
                .AsQueryable();

            var data = await query
                .Include(x => x.BiddingWorkType)
                .Include(x => x.Business)
                .Select(x => new BiddingWorkViewModel
                {
                    Id = x.Id,
                    Number = x.CodeNumber.ToString("D3"),
                    Name = x.Name,
                    CurrencyType = x.CurrencyType,
                    BiddingCurrencyTypeId = x.BiddingCurrencyTypeId,
                    BiddingCurrencyType = x.BiddingCurrencyTypeId != null? new BiddingCurrencyTypeViewModel
                    {
                      Currency = x.BiddingCurrencyType.Currency
                    } : null,
                    BusinessParticipationFoldingId = x.BusinessParticipationFoldingId,
                    BusinessParticipationFolding = new BusinessParticipationFoldingViewModel
                    {
                        IvcParticipation = x.BusinessId == Guid.Parse("78746F22-62C2-4E68-9FDC-08D7F8F4D82F")?
                        100 : x.BusinessParticipationFolding.IvcParticipation
                    },
                    BiddingWorkTypeId = x.BiddingWorkTypeId,
                    BiddingWorkType = new BiddingWorkTypeViewModel
                    {
                        Name = x.BiddingWorkType.Name,
                        PillColor = x.BiddingWorkType.PillColor
                    },
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    DifDate = x.DifDate,
                    ReceivedDate = x.ReceivedDate.HasValue
                        ? x.ReceivedDate.Value.Date.ToDateString() : String.Empty,
                    LiquidationDate = x.LiquidationDate.HasValue
                        ? x.LiquidationDate.Value.Date.ToDateString() : String.Empty,

                    BusinessId = x.BusinessId,
                    Business = new BusinessViewModel
                    {
                        Tradename = x.Business.Tradename,
                        RUC = x.Business.RUC,
                        CreateDate = x.Business.CreateDate.Date.ToDateString()
                    },
                    ContractUrl = x.ContractUrl,
                    InVoiceUrl = x.InVoiceUrl,
                    LiquidationUrl = x.LiquidationUrl,
                    ReceivedActUrl = x.ReceivedActUrl,
                    ConfirmedWork = x.ConfirmedWork,
                    LiquidationAmmount = x.LiquidationAmmount.Value,
                    ContractAmmount  =x.ContractAmmount.Value,
     
                    ParticipationAmmount = x.ParticipationAmmount,
                    LiquidationDollarAmmount = x.LiquidationDollarAmmount.Value,
                    ContractDollarAmmount = x.ContractDollarAmmount,
                    ParticipationDollarAmmount = x.ParticipationDollarAmmount,

                }).ToListAsync();

            switch(Period)
            {
                case 5:
                    data = data.Where(x => EF.Functions.DateDiffYear(x.EndDate.ToDateTime(), DateTime.Now) < 5).ToList();
                    break;
                case 10:
                    data = data.Where(x => EF.Functions.DateDiffYear(x.EndDate.ToDateTime(), DateTime.Now) < 10).ToList();
                    break;

                case 15:
                    data = data.Where(x => EF.Functions.DateDiffYear(x.EndDate.ToDateTime(), DateTime.Now) < 15).ToList();
                    break;
                case 20:
                    data = data.Where(x => EF.Functions.DateDiffYear(x.EndDate.ToDateTime(), DateTime.Now) < 20).ToList();
                    break;
            }

            switch (ivc)
            {
                case 2:
                    data = data.Where(x => x.ParticipationAmmount > 0.00).ToList();
                    break;

               case 3:
                    data = data.Where(x => x.ParticipationAmmount == 0.00).ToList();
                    break;
            }

            if(biddingWorkType.HasValue)
            {
                data = data.Where(x => x.BiddingWorkTypeId == biddingWorkType.Value).ToList();
            }

            foreach (var folding in data)
            {
                var plus = await _context.BiddingWorkComponentDetails
                    .Include(x => x.BiddingWorkComponent)
                    .Where(x => x.BiddingWorkId == folding.Id)
                    .Select(x => x.BiddingWorkComponent.Description)
                    .ToListAsync();
                folding.Components = string.Join(", ", plus);
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //TODO: Actualizar el modal de Editar
            var model = await _context.BiddingWorks
                .Where(x => x.Id == id)
                .Include(x => x.BiddingWorkType)
                .Include(x => x.Business)
                .Select(x => new BiddingWorkViewModel
                {
                    Id = x.Id,
                    Number = x.CodeNumber.ToString("D3"),
                    Name = x.Name,
                    BiddingWorkTypeId = x.BiddingWorkTypeId,
                    StartDate = x.StartDate.Date.ToDateString(),
                    EndDate = x.EndDate.Date.ToDateString(),
                    DifDate = x.DifDate,
                    ReceivedDate = x.ReceivedDate.HasValue
                        ? x.ReceivedDate.Value.Date.ToDateString() : String.Empty,
                    LiquidationDate = x.LiquidationDate.HasValue
                        ? x.LiquidationDate.Value.Date.ToDateString() : String.Empty,
                    BiddingCurrencyTypeId = x.BiddingCurrencyTypeId,
                   CurrencyType = x.CurrencyType,
                    BusinessId = x.BusinessId,
                    ContractUrl = x.ContractUrl,
                    InVoiceUrl = x.InVoiceUrl,
                    LiquidationUrl = x.LiquidationUrl,
                    ReceivedActUrl = x.ReceivedActUrl,
                    ConfirmedWork = x.ConfirmedWork,
                    LiquidationAmmount = x.LiquidationAmmount.Value,
                    ContractAmmount = x.ContractAmmount.Value,
                    BusinessParticipationFoldingId = x.BusinessParticipationFoldingId,

                    ParticipationAmmount = x.ParticipationAmmount,

                    LiquidationDollarAmmount = x.LiquidationDollarAmmount.Value,
                    ContractDollarAmmount = x.ContractDollarAmmount,
                    ParticipationDollarAmmount = x.ParticipationDollarAmmount,
                }).FirstOrDefaultAsync();

            var components = await _context.BiddingWorkComponentDetails
.Where(x => x.BiddingWorkId == id)
.Select(x => x.BiddingWorkComponentId)
.ToListAsync();

            model.BiddingWorkComponents = components;
            return Ok(model);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BiddingWorkViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            double? nulld = null;
            var last = await _context.BiddingWorks.CountAsync();
            var curren = await _context.BiddingCurrencyTypes.FirstOrDefaultAsync(x=>x.Id == model.BiddingCurrencyTypeId);
            var parti = await _context.BusinessParticipationFoldings.FirstOrDefaultAsync(x => x.Id == model.BusinessParticipationFoldingId);
            var realparticipation = model.BusinessId == Guid.Parse("78746F22-62C2-4E68-9FDC-08D7F8F4D82F") ? 100.00 : parti.IvcParticipation;
            var professional = new BiddingWork
            {
                
                CodeNumber = last+1,
                Name = model.Name,
                BiddingWorkTypeId = model.BiddingWorkTypeId,
                StartDate = model.StartDate.ToDateTime(),
                EndDate = model.EndDate.ToDateTime(),
                DifDate = EF.Functions.DateDiffDay(model.StartDate.ToDateTime(), model.EndDate.ToDateTime()) +1,

                ReceivedDate = string.IsNullOrEmpty(model.ReceivedDate)
                    ? (DateTime?)null : model.ReceivedDate.ToDateTime(),

                LiquidationDate = string.IsNullOrEmpty(model.LiquidationDate)
                    ? (DateTime?)null : model.LiquidationDate.ToDateTime(),


                BusinessId = model.BusinessId,
                BusinessParticipationFoldingId = model.BusinessParticipationFoldingId,
                CurrencyType = model.CurrencyType,
                LiquidationAmmount = model.CurrencyType == 1?model.LiquidationAmmount.Value : model.LiquidationDollarAmmount*curren.Currency,
                ContractAmmount = model.CurrencyType == 1 ? model.ContractAmmount : model.ContractDollarAmmount * curren.Currency,
                ParticipationAmmount = model.CurrencyType==1?model.LiquidationAmmount.Value * realparticipation / 100 : model.LiquidationDollarAmmount.Value*curren.Currency* realparticipation / 100,
            
                LiquidationDollarAmmount = model.CurrencyType == 2? model.LiquidationDollarAmmount.Value : nulld,
                ContractDollarAmmount = model.CurrencyType == 2? model.ContractDollarAmmount.Value : nulld,
                ParticipationDollarAmmount = model.CurrencyType == 2? model.LiquidationDollarAmmount.Value * realparticipation / 100 : nulld,
                BiddingCurrencyTypeId = model.BiddingCurrencyTypeId

            };

            //revisar los nombes de archivos
            if (model.FileContract != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.ContractUrl = await storage.UploadFile(model.FileContract.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileContract.FileName),
                    ConstantHelpers.Storage.Blobs.CONTRACT,
                    $"contrato_{last+1}");
            }

            if (model.FileInVoice != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.InVoiceUrl = await storage.UploadFile(model.FileInVoice.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileInVoice.FileName),
                    ConstantHelpers.Storage.Blobs.INVOICE,
                     $"factura_{last + 1}");
            }

            if (model.FileLiquidation != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.LiquidationUrl = await storage.UploadFile(model.FileLiquidation.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileLiquidation.FileName),
                    ConstantHelpers.Storage.Blobs.LIQUIDATION,
                     $"liquidacion_{last + 1}");
            }

            if (model.FileReceivedAct != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.ReceivedActUrl = await storage.UploadFile(model.FileReceivedAct.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileReceivedAct.FileName),
                    ConstantHelpers.Storage.Blobs.ACT,
                $"acta_recibida_{last + 1}");
            }

            if (model.FileConfirmedWork != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.ConfirmedWork = await storage.UploadFile(model.FileConfirmedWork.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileConfirmedWork.FileName),
                    ConstantHelpers.Storage.Blobs.CONFIRMED,
                $"conformidad_{last +1}");
            }
            if (model.BiddingWorkComponents != null)
                await _context.BiddingWorkComponentDetails.AddRangeAsync(
                model.BiddingWorkComponents.Select(x => new BiddingWorkComponentDetail
                {
                BiddingWork = professional,
                BiddingWorkComponentId = x
                }).ToList());

            await _context.BiddingWorks.AddAsync(professional);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BiddingWorkViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            double? nulld = null;
            var last = await _context.BiddingWorks.Where(x=>x.Id == id).Select(x => x.CodeNumber)
    .FirstOrDefaultAsync();
            var curren = await _context.BiddingCurrencyTypes.FirstOrDefaultAsync(x => x.Id == model.BiddingCurrencyTypeId);
            var parti = await _context.BusinessParticipationFoldings.FirstOrDefaultAsync(x => x.Id == model.BusinessParticipationFoldingId);
            var realparticipation = model.BusinessId == Guid.Parse("78746F22-62C2-4E68-9FDC-08D7F8F4D82F") ? 100.00 : parti.IvcParticipation;

            var professional = await _context.BiddingWorks.FirstOrDefaultAsync(x => x.Id == id);
                professional.Name = model.Name;
                professional.BiddingWorkTypeId = model.BiddingWorkTypeId;
                professional.StartDate = model.StartDate.ToDateTime();
                professional.EndDate = model.EndDate.ToDateTime();
                professional.DifDate = EF.Functions.DateDiffDay(model.StartDate.ToDateTime(), model.EndDate.ToDateTime()) + 1;
                
            
            professional.BiddingCurrencyTypeId = model.BiddingCurrencyTypeId;
            professional.CurrencyType = model.CurrencyType;
            
            
            professional.ReceivedDate = string.IsNullOrEmpty(model.ReceivedDate)
                ? (DateTime?)null : model.ReceivedDate.ToDateTime();


                professional.LiquidationDate = string.IsNullOrEmpty(model.LiquidationDate)
                    ? (DateTime?)null : model.LiquidationDate.ToDateTime();
                professional.BusinessId = model.BusinessId;
            professional.BusinessParticipationFoldingId = model.BusinessParticipationFoldingId;
            professional.LiquidationAmmount = model.CurrencyType == 1 ? model.LiquidationAmmount.Value : model.LiquidationDollarAmmount * curren.Currency;
            professional.ContractAmmount = model.CurrencyType == 1 ? model.ContractAmmount : model.ContractDollarAmmount * curren.Currency;
            professional.ParticipationAmmount = model.CurrencyType == 1 ? model.LiquidationAmmount.Value * realparticipation / 100 : model.LiquidationDollarAmmount.Value * curren.Currency * realparticipation / 100;

            professional.LiquidationDollarAmmount = model.CurrencyType == 2 ? model.LiquidationDollarAmmount.Value : nulld;
            professional.ContractDollarAmmount = model.CurrencyType == 2 ? model.ContractDollarAmmount.Value : nulld;
            professional.ParticipationDollarAmmount = model.CurrencyType == 2 ? model.LiquidationDollarAmmount.Value * realparticipation / 100 : nulld;
            
            if (model.FileContract != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.ContractUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CONTRACT}/{professional.ContractUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.ContractUrl = await storage.UploadFile(model.FileContract.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileContract.FileName),
                    ConstantHelpers.Storage.Blobs.CONTRACT,
                     $"contrato_{last}");
            }

            if (model.FileInVoice != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.InVoiceUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.INVOICE}/{professional.InVoiceUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.InVoiceUrl = await storage.UploadFile(model.FileInVoice.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileInVoice.FileName),
                    ConstantHelpers.Storage.Blobs.INVOICE,
                     $"factura_{last}");
            }

            if (model.FileLiquidation != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.LiquidationUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LIQUIDATION}/{professional.LiquidationUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.LiquidationUrl = await storage.UploadFile(model.FileLiquidation.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileLiquidation.FileName),
                    ConstantHelpers.Storage.Blobs.LIQUIDATION,
                     $"liquidacion_{last}");
            }

            if (model.FileReceivedAct != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.ReceivedActUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.ACT}/{professional.ReceivedActUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.ReceivedActUrl = await storage.UploadFile(model.FileReceivedAct.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileReceivedAct.FileName),
                    ConstantHelpers.Storage.Blobs.ACT,
                     $"acta_recibida_{last}");
            }

            if (model.FileConfirmedWork != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.ConfirmedWork != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CONFIRMED}/{professional.ConfirmedWork.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.ConfirmedWork = await storage.UploadFile(model.FileConfirmedWork.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileConfirmedWork.FileName),
                    ConstantHelpers.Storage.Blobs.CONFIRMED,
                     $"conformidad_{last}");
            }

            var componentsDb = await _context.BiddingWorkComponentDetails
.Where(x => x.BiddingWorkId == id)
.ToListAsync();
            _context.BiddingWorkComponentDetails.RemoveRange(componentsDb);
            if (model.BiddingWorkComponents != null)
                await _context.BiddingWorkComponentDetails.AddRangeAsync(
                model.BiddingWorkComponents.Select(x => new BiddingWorkComponentDetail
                {
                    BiddingWorkId = id,
                    BiddingWorkComponentId = x
                }).ToList());


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var professional = await _context.BiddingWorks.FirstOrDefaultAsync(x => x.Id == id);
            if (professional == null)
                return BadRequest($"Profesional con Id '{id}' no encontrado.");

            var componentsDb = await _context.BiddingWorkComponentDetails
            .Where(x => x.BiddingWorkId == id)
            .ToListAsync();

            if (professional.InVoiceUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.INVOICE}/{professional.InVoiceUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            if (professional.ContractUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CONTRACT}/{professional.ContractUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            if (professional.LiquidationUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.LIQUIDATION}/{professional.LiquidationUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            if (professional.ReceivedActUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.ACT}/{professional.ReceivedActUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            if (professional.ConfirmedWork != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CONFIRMED}/{professional.ConfirmedWork.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            _context.BiddingWorkComponentDetails.RemoveRange(componentsDb);
            _context.BiddingWorks.Remove(professional);


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("monto")]
        public async Task<IActionResult> GetTotalInstalled(Guid? biddingWorkType, int? Period, int? ivc)
        {
            var foldings = await _context.BiddingWorks.ToListAsync();


            switch (Period)
            {
                case 5:
                    foldings = foldings.Where(x => EF.Functions.DateDiffYear(x.EndDate, DateTime.Now) < 5).ToList();
                    break;
                case 10:
                    foldings = foldings.Where(x => EF.Functions.DateDiffYear(x.EndDate, DateTime.Now) < 10).ToList();
                    break;

                case 15:
                    foldings = foldings.Where(x => EF.Functions.DateDiffYear(x.EndDate, DateTime.Now) < 15).ToList();
                    break;
                case 20:
                    foldings = foldings.Where(x => EF.Functions.DateDiffYear(x.EndDate, DateTime.Now) < 20).ToList();
                    break;
            }

            switch (ivc)
            {
                case 2:
                    foldings = foldings.Where(x => x.ParticipationAmmount > 0.00).ToList();
                    break;

                case 3:
                    foldings = foldings.Where(x => x.ParticipationAmmount == 0.00).ToList();
                    break;
            }

            if (biddingWorkType.HasValue)
            {
                foldings = foldings.Where(x => x.BiddingWorkTypeId == biddingWorkType.Value).ToList();
            }

            var Suma = 0.0;

            foreach (var item in foldings)
                Suma += item.ParticipationAmmount;

            string str = String.Format(new CultureInfo("es-PE"), "{0:C}", Suma);
            return Ok(str);
        }
    
    }
}
