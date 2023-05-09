using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/proveedores")]
    public class ProviderController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ProviderController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<ProviderController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? supplyFamilyId = null, Guid? supplyGroupId = null)
        {
            /*
            if (supplyFamilyId.HasValue || supplyGroupId.HasValue)
            {
                var query = new List<ProviderViewModel>();

                query.AddRange(
                        await _context.ProviderSupplyFamilies
                            .Include(x => x.Provider)
                            .Include(x => x.SupplyFamily)
                            .Where(x => x.SupplyFamilyId == supplyFamilyId)
                            .Select(x => new ProviderViewModel
                            {
                                Id = x.ProviderId,
                                Code = x.Provider.Code,
                                BusinessName = x.Provider.BusinessName,
                                Tradename = x.Provider.Tradename,
                                RUC = x.Provider.RUC,
                                Address = x.Provider.Address,
                                PhoneNumber = x.Provider.PhoneNumber,
                                CollectionAreaContactName = x.Provider.CollectionAreaContactName
                            }).ToListAsync()
                    );

                query.AddRange(
                        await _context.ProviderSupplyGroups
                            .Include(x => x.Provider)
                            .Include(x => x.SupplyGroup)
                            .Where(x => x.SupplyGroupId == supplyGroupId)
                            .Select(x => new ProviderViewModel
                            {
                                Id = x.ProviderId,
                                Code = x.Provider.Code,
                                BusinessName = x.Provider.BusinessName,
                                Tradename = x.Provider.Tradename,
                                RUC = x.Provider.RUC,
                                Address = x.Provider.Address,
                                PhoneNumber = x.Provider.PhoneNumber,
                                CollectionAreaContactName = x.Provider.CollectionAreaContactName
                            }).ToListAsync()
                    );

                var data = query.GroupBy(x => x.Id).Select(x => x.First()).ToList();

                return Ok(data);
            }
            else
            {
                var data = await _context.Providers
                .Select(x => new ProviderViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    BusinessName = x.BusinessName,
                    Tradename = x.Tradename,
                    RUC = x.RUC,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CollectionAreaContactName = x.CollectionAreaContactName
                }).ToListAsync();

                return Ok(data);
            }
            */

            var providers = await _context.Providers.AsNoTracking().ToListAsync();

            var families = await _context.ProviderSupplyFamilies.Include(x => x.SupplyFamily).ToListAsync();
            var groups = await _context.ProviderSupplyGroups.Include(x => x.SupplyGroup).ToListAsync();

            var data = new List<ProviderViewModel>();

            foreach (var provider in providers)
            {
                var familiesFilter = families.FirstOrDefault(x => x.ProviderId == provider.Id && x.SupplyFamilyId == supplyFamilyId);
                var groupsFilter = groups.FirstOrDefault(x => x.ProviderId == provider.Id && x.SupplyGroupId == supplyGroupId);

                var familiesString = string.Join(" - ",
                    families.Where(x => x.ProviderId == provider.Id)
                    .Select(x => x.SupplyFamily.Name)
                    .ToList());

                var groupsString = string.Join(" - ",
                    groups.Where(x => x.ProviderId == provider.Id)
                    .Select(x => x.SupplyGroup.Name)
                    .ToList());

                if ((supplyFamilyId != null && supplyFamilyId != null && familiesFilter != null && groupsFilter != null)
                    || (supplyFamilyId.HasValue && familiesFilter != null && supplyGroupId == null) 
                    || (supplyGroupId.HasValue && groupsFilter != null && supplyFamilyId == null)
                    || (supplyFamilyId == null && supplyGroupId == null))
                {
                    data.Add(new ProviderViewModel
                    {
                        Id = provider.Id,
                        Code = provider.Code,
                        BusinessName = provider.BusinessName,
                        Tradename = provider.Tradename,
                        RUC = provider.RUC,
                        Address = provider.Address,
                        PhoneNumber = provider.PhoneNumber,
                        CollectionAreaContactName = provider.CollectionAreaContactName,
                        SupplyFamilyNames = familiesString,
                        SupplyGroupNames =groupsString,
                    });
                }

            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Providers
                .Where(x => x.Id == id)
                .Select(x => new ProviderViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    BusinessName = x.BusinessName,
                    Tradename = x.Tradename,
                    RUC = x.RUC,
                    LegalAgent = x.LegalAgent,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CollectionAreaContactName = x.CollectionAreaContactName,
                    CollectionAreaEmail = x.CollectionAreaEmail,
                    CollectionAreaPhoneNumber = x.CollectionAreaPhoneNumber,
                    BankId = x.BankId,
                    BankAccountType = x.BankAccountType,
                    BankAccountNumber = x.BankAccountNumber,
                    BankAccountCCI = x.BankAccountCCI,
                    ForeignBankId = x.ForeignBankId,
                    ForeignBankAccountType = x.ForeignBankAccountType,
                    ForeignBankAccountCurrency = x.ForeignBankAccountCurrency,
                    ForeignBankAccountNumber = x.ForeignBankAccountNumber,
                    ForeignBankAccountCCI = x.ForeignBankAccountCCI,
                    TaxBankId = x.TaxBankId,
                    TaxBankAccountNumber = x.TaxBankAccountNumber,
                    PropertyServiceType = x.PropertyServiceType,
                    PropertyServiceCode = x.PropertyServiceCode
                }).AsNoTracking()
                .FirstOrDefaultAsync();

            var families = await _context.ProviderSupplyFamilies
                .Where(x => x.ProviderId == id)
                .Select(x => x.SupplyFamilyId)
                .ToListAsync();

            var groups = await _context.ProviderSupplyGroups
                .Where(x => x.ProviderId == id)
                .Select(x => x.SupplyGroupId)
                .ToListAsync();

            data.SupplyFamilyIds = families;
            data.SupplyGroupIds = groups;

            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProviderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cant = _context.Providers.Count() + 1;

            var provider = new Provider
            {
                Code = "CP"+ cant.ToString("D4"),
                BusinessName = model.BusinessName,
                Tradename = model.Tradename,
                RUC = model.RUC,
                LegalAgent = model.LegalAgent,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                CollectionAreaContactName = model.CollectionAreaContactName,
                CollectionAreaEmail = model.CollectionAreaEmail,
                CollectionAreaPhoneNumber = model.CollectionAreaPhoneNumber,
                BankId = model.BankId,
                BankAccountType = model.BankAccountType,
                BankAccountNumber = model.BankAccountNumber,
                BankAccountCCI = model.BankAccountCCI,
                ForeignBankId = model.ForeignBankId,
                ForeignBankAccountType = model.ForeignBankAccountType,
                ForeignBankAccountNumber = model.ForeignBankAccountNumber,
                ForeignBankAccountCurrency = model.ForeignBankAccountCurrency,
                ForeignBankAccountCCI = model.ForeignBankAccountCCI,
                TaxBankId = model.TaxBankId,
                TaxBankAccountNumber = model.TaxBankAccountNumber,
                PropertyServiceType = model.PropertyServiceType,
                PropertyServiceCode = model.PropertyServiceCode
            };

            if (model.SupplyFamilyIds != null)
                await _context.ProviderSupplyFamilies.AddRangeAsync(
                    model.SupplyFamilyIds.Select(x => new ProviderSupplyFamily
                    {
                        Provider = provider,
                        SupplyFamilyId = x
                    }).ToList());

            if (model.SupplyGroupIds != null)
                await _context.ProviderSupplyGroups.AddRangeAsync(
                    model.SupplyGroupIds.Select(x => new ProviderSupplyGroup
                    {
                        Provider = provider,
                        SupplyGroupId = x
                    }).ToList());

            await _context.Providers.AddAsync(provider);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var provider = await _context.Providers
                .FirstOrDefaultAsync(x => x.Id == id);

            provider.BusinessName = model.BusinessName;
            provider.Tradename = model.Tradename;
            provider.RUC = model.RUC;
            provider.LegalAgent = model.LegalAgent;
            provider.Address = model.Address;
            provider.PhoneNumber = model.PhoneNumber;
            provider.CollectionAreaContactName = model.CollectionAreaContactName;
            provider.CollectionAreaEmail = model.CollectionAreaEmail;
            provider.CollectionAreaPhoneNumber = model.CollectionAreaPhoneNumber;
            provider.BankId = model.BankId;
            provider.BankAccountType = model.BankAccountType;
            provider.BankAccountNumber = model.BankAccountNumber;
            provider.BankAccountCCI = model.BankAccountCCI;
            provider.ForeignBankId = model.ForeignBankId;
            provider.ForeignBankAccountType = model.ForeignBankAccountType;
            provider.ForeignBankAccountCurrency = model.ForeignBankAccountCurrency;
            provider.ForeignBankAccountNumber = model.ForeignBankAccountNumber;
            provider.ForeignBankAccountCCI = model.ForeignBankAccountCCI;
            provider.TaxBankId = model.TaxBankId;
            provider.TaxBankAccountNumber = model.TaxBankAccountNumber;
            provider.PropertyServiceType = model.PropertyServiceType;
            provider.PropertyServiceCode = model.PropertyServiceCode;

            var familiesDb = await _context.ProviderSupplyFamilies
                .Where(x => x.ProviderId == id)
                .ToListAsync();
            _context.ProviderSupplyFamilies.RemoveRange(familiesDb);
            if (model.SupplyFamilyIds != null)
                await _context.ProviderSupplyFamilies.AddRangeAsync(
                model.SupplyFamilyIds.Select(x => new ProviderSupplyFamily
                {
                    ProviderId = id,
                    SupplyFamilyId = x
                }).ToList());


            var groupsDb = await _context.ProviderSupplyGroups
                .Where(x => x.ProviderId == id)
                .ToListAsync();
            _context.ProviderSupplyGroups.RemoveRange(groupsDb);
            if (model.SupplyGroupIds != null)
                await _context.ProviderSupplyGroups.AddRangeAsync(
                    model.SupplyGroupIds.Select(x => new ProviderSupplyGroup
                    {
                        ProviderId = id,
                        SupplyGroupId = x
                    }).ToList());

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var provider = await _context.Providers
                .Include(x => x.ProviderFiles)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (provider == null)
                return BadRequest($"Proveedor con Id '{id}' no encontrado.");
            if (provider.ProviderFiles.Any())
            {
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var file in provider.ProviderFiles)
                {
                    if(file.FileUrl != null)
                        await storage.TryDelete($"{ConstantHelpers.Provider.FileType.BLOBS[file.Type]}/{file.FileUrl.AbsolutePath.Split('/').Last()}",
                                ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
                }
                _context.ProviderFiles.RemoveRange(provider.ProviderFiles);
            }

            var familiesDb = await _context.ProviderSupplyFamilies
                .Where(x => x.ProviderId == id)
                .ToListAsync();
            
            var groupsDb = await _context.ProviderSupplyGroups
                .Where(x => x.ProviderId == id)
                .ToListAsync();
            
            _context.ProviderSupplyFamilies.RemoveRange(familiesDb);
            _context.ProviderSupplyGroups.RemoveRange(groupsDb);
            _context.Providers.Remove(provider);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}/archivos/listar")]
        public async Task<IActionResult> GetAllFiles(Guid id)
        {
            var query = _context.ProviderFiles
                .Where(x => x.ProviderId == id)
                .AsNoTracking().AsQueryable();

            var data = await query
                .Select(x => new ProviderFileViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    FileUrl = x.FileUrl
                }).AsNoTracking().ToListAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("{id}/archivos/crear")]
        public async Task<IActionResult> CreateFile([FromRoute]Guid id, ProviderFileViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var provider = await _context.Providers.FindAsync(id);
            var providerFile = new ProviderFile
            {
                ProviderId = id,
                Type = model.Type
            };
            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                providerFile.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                            ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.File.FileName),
                            ConstantHelpers.Provider.FileType.BLOBS[providerFile.Type], $"{provider.Id}/{System.IO.Path.GetFileNameWithoutExtension(model.File.FileName)}");
            }
            await _context.ProviderFiles.AddAsync(providerFile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("{id}/archivos/eliminar/{fileId}")]
        public async Task<IActionResult> DeleteFile([FromRoute]Guid id, [FromRoute]Guid fileId)
        {
            var providerFile = await _context.ProviderFiles
                .FirstOrDefaultAsync(x => x.Id == fileId);
            if (providerFile == null)
                return BadRequest($"Archivo con Id '{fileId}' no encontrado.");
            if(providerFile.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete(providerFile.FileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);
            }
            _context.ProviderFiles.Remove(providerFile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var defaultMeasurementUnit = await _context.MeasurementUnits.FirstOrDefaultAsync();
                    var newProviders = new List<Provider>();
                    var existingProviders = new List<Provider>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var codeStr = workSheet.Cell($"A{counter}").GetString();
                        var provider = (await _context.Providers.Where(x => x.Code == codeStr).FirstOrDefaultAsync())
                            ?? new Provider();
                        provider.BusinessName = workSheet.Cell($"B{counter}").GetString();
                        provider.Tradename = workSheet.Cell($"C{counter}").GetString();
                        provider.RUC = workSheet.Cell($"D{counter}").GetString();
                        provider.Address = workSheet.Cell($"F{counter}").GetString();
                        if (provider.Id == Guid.Empty)
                            newProviders.Add(provider);
                        else
                            existingProviders.Add(provider);
                        ++counter;
                    }
                    await _context.Providers.AddRangeAsync(newProviders);
                    _context.Providers.UpdateRange(existingProviders);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("PROVEEDORES");
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Razón Social", typeof(string));
            dt.Columns.Add("Nombre Comercial", typeof(string));
            dt.Columns.Add("RUC", typeof(string));
            dt.Columns.Add("Dirección", typeof(string));
            
            var data = await _context.Providers
                .AsNoTracking().ToListAsync();
            data.ForEach(item => {
                dt.Rows.Add(item.Code, item.BusinessName, item.Tradename, item.RUC, item.Address);
            });

            var fileName = "Proveedores.xlsx";
            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPut("codigo")]
        public async Task<IActionResult> UpdateCode()
        {
            var providers = await _context.Providers.ToListAsync();

            var code = 1;

            foreach(var provider in providers)
            {
                provider.Code = "CP" + code.ToString("D4");
                code++;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}