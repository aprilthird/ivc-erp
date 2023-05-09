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
using IVC.PE.ENTITIES.UspModels.Biddings;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/empresas")]
    public class BusinessController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public BusinessController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<BusinessController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Set<UspBusiness>().FromSqlRaw("execute Bidding_uspBusiness")
             .IgnoreQueryFilters()
             .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Businesses
                .Where(x => x.Id == id)
                .Select(x => new BusinessViewModel
                {
                    Id = x.Id,
                    BusinessName = x.BusinessName,
                    Tradename = x.Tradename,
                    RUC = x.RUC,

                   
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,

                   
                    CollectionAreaContactName = x.CollectionAreaContactName,
                    CollectionAreaEmail = x.CollectionAreaEmail,
                    CollectionAreaPhoneNumber = x.CollectionAreaPhoneNumber,
                    RucUrl = x.RucUrl,
                    TestimonyUrl = x.TestimonyUrl,
                    CreateDate = x.CreateDate.Date.ToDateString(),
                    Type = x.Type,
                    DefaultParticipation = x.DefaultParticipation,
                    BusinessConsortium1 = x.BusinessConsortium1,
                    BusinessConsortium2 = x.BusinessConsortium2,
                    BusinessConsortium3 = x.BusinessConsortium3,
                    BusinessConsortium4 = x.BusinessConsortium4,
                    BusinessConsortium5 = x.BusinessConsortium5,
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(BusinessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var last = await _context.Businesses.CountAsync();
            double? ndouble = null;
            var provider = new Business
            {

                CodeNumber = last+1,
                BusinessName = model.BusinessName,
                Tradename = model.Tradename,
                RUC = model.RUC,
                
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,



                CollectionAreaContactName = model.CollectionAreaContactName,
                CollectionAreaEmail = model.CollectionAreaEmail,
                CollectionAreaPhoneNumber = model.CollectionAreaPhoneNumber,
                CreateDate = model.CreateDate.ToDateTime(),
                Type = model.Type,
                DefaultParticipation = model.Type==1?100:ndouble,
                BusinessConsortium1 = model.Type==2?model.BusinessConsortium1:null,
                BusinessConsortium2 = model.Type == 2 ? model.BusinessConsortium2 : null,
                BusinessConsortium3 = model.Type == 2 ? model.BusinessConsortium3 : null,
                BusinessConsortium4 = model.Type == 2 ? model.BusinessConsortium4 : null,
                BusinessConsortium5 = model.Type == 2 ? model.BusinessConsortium5 : null,
               

            };

            if (model.FileRuc != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                provider.RucUrl= await storage.UploadFile(model.FileRuc.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileRuc.FileName),
                    ConstantHelpers.Storage.Blobs.BUSINESS,
                    $"ruc-{model.RUC}");
            }

            if (model.FileTestimony != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                provider.TestimonyUrl = await storage.UploadFile(model.FileTestimony.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    Path.GetExtension(model.FileTestimony.FileName),
                    ConstantHelpers.Storage.Blobs.BUSINESS,
                    $"testimonio-{model.RUC}");
            }
            //if(model.Files?.Any() ?? false)
            //{
            //    var storage = new CloudStorageService(_storageCredentials);
            //    var files = new List<ProviderFile>();
            //    var counter = 0;
            //    foreach (var fileModel in model.Files)
            //    {
            //        var file = new ProviderFile
            //        {
            //            Name = $"{provider.Code}_{counter}",
            //            Type = fileModel.Type,
            //            Provider = provider
            //        };
            //        if(fileModel.File != null)
            //        {
            //            file.FileUrl = await storage.UploadFile(fileModel.File.OpenReadStream(),
            //                ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(fileModel.File.FileName),
            //                ConstantHelpers.Provider.FileType.BLOBS[file.Type], file.Name);
            //        }
            //        ++counter;
            //        files.Add(file);
            //    }
            //    await _context.ProviderFiles.AddRangeAsync(files);
            //}

            await _context.Businesses.AddAsync(provider);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BusinessViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            double? ndouble = null;
            var provider = await _context.Businesses
                .FirstOrDefaultAsync(x => x.Id == id);
            provider.BusinessName = model.BusinessName;
            provider.Tradename = model.Tradename;
            provider.RUC = model.RUC;
          
                provider.Address = model.Address;
                provider.PhoneNumber = model.PhoneNumber;


        

             
            provider.CollectionAreaContactName = model.CollectionAreaContactName;
                provider.CollectionAreaEmail = model.CollectionAreaEmail;
                provider.CollectionAreaPhoneNumber = model.CollectionAreaPhoneNumber;
                provider.CreateDate = model.CreateDate.ToDateTime();

            provider.Type = model.Type;
            provider.DefaultParticipation = model.Type == 1? 100: ndouble;

            provider.BusinessConsortium1 = model.Type == 2 ? model.BusinessConsortium1 : null;
            provider.BusinessConsortium2 = model.Type == 2 ? model.BusinessConsortium2 : null;
            provider.BusinessConsortium3 = model.Type == 2 ? model.BusinessConsortium3 : null;
            provider.BusinessConsortium4 = model.Type == 2 ? model.BusinessConsortium4 : null;
            provider.BusinessConsortium5 = model.Type == 2 ? model.BusinessConsortium5 : null;

            



            if (model.FileRuc != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (provider.RucUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{provider.RucUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                provider.RucUrl= await storage.UploadFile(model.FileRuc.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    Path.GetExtension(model.FileRuc.FileName),
                    ConstantHelpers.Storage.Blobs.BUSINESS,
                    $"ruc-{model.RUC}");
            }

            if (model.FileTestimony != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (provider.TestimonyUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PERMS}/{provider.TestimonyUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.DOCUMENTARY_CONTROL);
                provider.TestimonyUrl = await storage.UploadFile(model.FileTestimony.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    Path.GetExtension(model.FileTestimony.FileName),
                    ConstantHelpers.Storage.Blobs.BUSINESS,
                    $"testimonio-{model.RUC}");
            }
            //if (provider.ProviderFiles.Any())
            //{
            //    if (model.Files?.Any() ?? false)
            //    {
            //        var storage = new CloudStorageService(_storageCredentials);
            //        var toRemoveFiles = provider.ProviderFiles.Where(p => !model.Files.Any(f => f.Id == p.Id)).ToList();
            //        foreach (var file in toRemoveFiles)
            //        {
            //            if (file.FileUrl != null)
            //                await storage.TryDelete($"{ConstantHelpers.Provider.FileType.BLOBS[file.Type]}/{file.FileUrl.AbsolutePath.Split('/').Last()}",
            //                        ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY);
            //        }
            //        _context.ProviderFiles.RemoveRange(toRemoveFiles);

            //        var files = new List<ProviderFile>();
            //        foreach (var fileModel in model.Files.Where(f => !provider.ProviderFiles.Any(p => p.Id == f.Id)).ToList())
            //        {
            //            var file = new ProviderFile
            //            {
            //                Name = fileModel.Name,
            //                Type = fileModel.Type,
            //                ProviderId = provider.Id
            //            };
            //            if (fileModel.File != null)
            //            {
            //                file.FileUrl = await storage.UploadFile(fileModel.File.OpenReadStream(),
            //                ConstantHelpers.Storage.Containers.TECHNICAL_LIBRARY, System.IO.Path.GetExtension(fileModel.File.FileName),
            //                ConstantHelpers.Provider.FileType.BLOBS[file.Type], file.Name);
            //            }
            //            files.Add(file);
            //        }
            //        await _context.ProviderFiles.AddRangeAsync(files);
            //    }
            //}

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var business = await _context.Businesses
                .FirstOrDefaultAsync(x => x.Id == id);
            if (business == null)
                return BadRequest($"Empresa con Id '{id}' no encontrado.");

            if (business.RucUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BUSINESS}/{business.RucUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }


            if (business.TestimonyUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BUSINESS}/{business.TestimonyUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            _context.Businesses.Remove(business);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //[HttpGet("{id}/archivos/listar")]
        //public async Task<IActionResult> GetAllFiles(Guid id)
        //{
        //    var query = _context.BusinessFiles
        //        .Where(x => x.BusinessId == id)
        //        .AsNoTracking().AsQueryable();

        //    var data = await query
        //        .Select(x => new BusinessFileViewModel
        //        {
        //            Id = x.Id,
        //            Type = x.Type,
        //            FileUrl = x.FileUrl
        //        }).AsNoTracking().ToListAsync();
        //    return Ok(data);
        //}

        //[Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        //[HttpPost("{id}/archivos/crear")]
        //public async Task<IActionResult> CreateFile([FromRoute]Guid id, ProviderFileViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var provider = await _context.Businesses.FindAsync(id);
        //    var businessFile = new BusinessFile
        //    {
        //        BusinessId = id,
        //        Type = model.Type
        //    };
        //    if (model.File != null)
        //    {
        //        var storage = new CloudStorageService(_storageCredentials);
        //        businessFile.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
        //                    ConstantHelpers.Storage.Containers.LOGISTICS, System.IO.Path.GetExtension(model.File.FileName),
        //                    ConstantHelpers.Business.FileType.BLOBS[businessFile.Type], $"{provider.Id}/{System.IO.Path.GetFileNameWithoutExtension(model.File.FileName)}");
        //    }
        //    await _context.BusinessFiles.AddAsync(businessFile);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        //[HttpDelete("{id}/archivos/eliminar/{fileId}")]
        //public async Task<IActionResult> DeleteFile([FromRoute]Guid id, [FromRoute]Guid fileId)
        //{
        //    var businessFile = await _context.BusinessFiles
        //        .FirstOrDefaultAsync(x => x.Id == fileId);
        //    if (businessFile == null)
        //        return BadRequest($"Archivo con Id '{fileId}' no encontrado.");
        //    if (businessFile.FileUrl != null)
        //    {
        //        var storage = new CloudStorageService(_storageCredentials);
        //        await storage.TryDelete(businessFile.FileUrl, ConstantHelpers.Storage.Containers.LOGISTICS);
        //    }
        //    _context.BusinessFiles.Remove(businessFile);
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

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
                    var newBusinesses = new List<Business>();
                    var existingBusinesses = new List<Business>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var codeStr = workSheet.Cell($"A{counter}").GetString();
                        var business = (await _context.Businesses.Where(x => "EMP-"+x.CodeNumber.ToString("D3") == codeStr).FirstOrDefaultAsync())
                            ?? new Business();
                        business.BusinessName = workSheet.Cell($"B{counter}").GetString();
                        business.Tradename = workSheet.Cell($"C{counter}").GetString();
                        business.RUC = workSheet.Cell($"D{counter}").GetString();
                        business.Address = workSheet.Cell($"F{counter}").GetString();
                        if (business.Id == Guid.Empty)
                            newBusinesses.Add(business);
                        else
                            existingBusinesses.Add(business);
                        ++counter;
                    }
                    await _context.Businesses.AddRangeAsync(newBusinesses);
                    _context.Businesses.UpdateRange(existingBusinesses);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("EMPRESAS");
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

            var fileName = "Empresas.xlsx";
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
    }
}
