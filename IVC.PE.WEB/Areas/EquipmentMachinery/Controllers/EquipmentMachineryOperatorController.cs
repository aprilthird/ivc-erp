using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/operadores")]
    public class EquipmentMachineryOperatorController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryOperatorController(IvcDbContext context,
        ILogger<EquipmentMachineryOperatorController> logger,
        IOptions<CloudStorageCredentials> storageCredentials) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? equipmentTypeId = null, int? hiringType = null, Guid? machineryId = null)
        {

            var pId = GetProjectId();

            var query = _context.EquipmentMachineryOperators
              .AsQueryable();

            var softs = _context.EquipmentMachineryTypeSofts
                .AsQueryable();

            var types = _context.EquipmentMachineryTypeTypes
                .AsQueryable();

            var transports = _context.EquipmentMachineryTypeTransports
                .AsQueryable();

            var data = await query
                .Include(x => x.Worker)
                .Include(x => x.EquipmentMachineryType)
                .Include(x=> x.EquipmentMachineryTypeSoft)
                .Include(x => x.EquipmentMachineryTypeType)
                .Include(x => x.EquipmentMachineryTypeTransport)
                .Where(x => x.ProjectId == pId)
                .Select(x => new EquipmentMachineryOperatorViewModel
                {
                    Id = x.Id,

                    OperatorName = x.OperatorName,
                    PhoneOperator = x.PhoneOperator,
                    DNIOperator = x.DNIOperator,

                    FromOtherName = x.FromOtherName,
                    FromOtherPhone = x.FromOtherPhone,
                    FromOtherDNI = x.FromOtherDNI,
                    ActualName = x.ActualName,
                    ActualDni = x.ActualDni,
                    HiringType = x.HiringType,

                    WorkerId = x.WorkerId,
                    Worker = x.WorkerId !=null ?  new WorkerViewModel
                    {
                        
                        Name = x.Worker.Name,
                        MiddleName = x.Worker.MiddleName,
                        PaternalSurname = x.Worker.PaternalSurname,
                        MaternalSurname = x.Worker.MaternalSurname,
                        PhoneNumber = x.Worker.PhoneNumber,
                        Document = x.Worker.Document,
                        DocumentType = x.Worker.DocumentType,
                        //Category = x.Worker.Category,
                        //Origin = x.Worker.Origin,
                        //Workgroup = x.Worker.Workgroup,
                        //IsActive = x.Worker.IsActive,
                        //CeaseDate = x.Worker.CeaseDate.HasValue
                    //? x.Worker.CeaseDate.Value.Date.ToDateString() : String.Empty,


                    } : null,
                    EquipmentMachineryTypeId = x.EquipmentMachineryTypeId,
                    EquipmentMachineryType = new EquipmentMachineryTypeViewModel
                    {
                        Description = x.EquipmentMachineryType.Description
                    },
                    EquipmentMachineryTypeSoftId = x.EquipmentMachineryTypeSoftId,
                    EquipmentMachineryTypeSoft = new EquipmentMachineryTypeSoftViewModel
                    {
                        Description = x.EquipmentMachineryTypeSoft.Description
                    },
                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId,
                    EquipmentMachineryTypeType = new EquipmentMachineryTypeTypeViewModel
                    {
                        Description = x.EquipmentMachineryTypeType.Description
                    },
                    EquipmentMachineryTypeTransportId = x.EquipmentMachineryTypeTransportId,
                    EquipmentMachineryTypeTransport = new EquipmentMachineryTypeTransportViewModel
                    {
                        Description = x.EquipmentMachineryTypeTransport.Description
                    },
                    StartDate = x.StartDate.HasValue
                    ? x.StartDate.Value.Date.ToDateString() : String.Empty,
                    FileUrl = x.FileUrl,
                    

                })
                .ToListAsync();

            foreach (var item in data)
            {
                if (item.WorkerId.HasValue)
                {
                    var lastPeriod = (_context.WorkerWorkPeriods
                    .Where(x => x.WorkerId == item.WorkerId)
                    .OrderByDescending(x => x.EntryDate))
                        .FirstOrDefault();
                    item.Worker.CeaseDateStr = lastPeriod.CeaseDate.HasValue ?
                                                lastPeriod.CeaseDate.Value.ToDateString() :
                                                string.Empty;
                    item.Worker.IsActive = lastPeriod.IsActive;
                }                
            }


            if (equipmentTypeId.HasValue)
                data = data.Where(x => x.EquipmentMachineryTypeId == equipmentTypeId.Value).ToList();

            if (hiringType == 1)
                data = data.Where(x => x.HiringType == 1).ToList();
            if (hiringType == 2)
                data = data.Where(x => x.HiringType == 2).ToList();
            if (hiringType == 3)
                data = data.Where(x => x.HiringType == 3).ToList();

            if (machineryId.HasValue)
            {

                if (types.Where(x=>x.Id == machineryId.Value).Count() > 0)
                    data = data.Where(x => x.EquipmentMachineryTypeTypeId == machineryId.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
                else if (softs.Where(x => x.Id == machineryId.Value).Count() > 0)
                        data = data.Where(x => x.EquipmentMachineryTypeSoftId == machineryId.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeTypeId == null).ToList();
                else if (transports.Where(x => x.Id == machineryId.Value).Count() > 0)
                    data = data.Where(x => x.EquipmentMachineryTypeTransportId == machineryId.Value && x.EquipmentMachineryTypeTypeId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
            
            }

            //if (machineryId.HasValue)
            //{
            //    //data = data.Where(x => x.EquipmentMachineryTypeTypeId == machineryId.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
            //    data = data.Where(x => x.EquipmentMachineryTypeTransportId == machineryId.Value && x.EquipmentMachineryTypeTypeId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
            //    //data = data.Where(x => x.EquipmentMachineryTypeSoftId == machineryId.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeTypeId == null).ToList();

            //}

            //if (machineryId.HasValue)
            //{
            //    //data = data.Where(x => x.EquipmentMachineryTypeTypeId == machineryId.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
            //    //data = data.Where(x => x.EquipmentMachineryTypeTransportId == machineryId.Value && x.EquipmentMachineryTypeTypeId == null && x.EquipmentMachineryTypeSoftId == null).ToList();
            //    data = data.Where(x => x.EquipmentMachineryTypeSoftId == machineryId.Value && x.EquipmentMachineryTypeTransportId == null && x.EquipmentMachineryTypeTypeId == null).ToList();

            //}

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = _context.EquipmentMachineryOperators
               .AsQueryable();

            var data = await query
                .Include(x => x.Project)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryOperatorViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTypeId = x.EquipmentMachineryTypeId,

                    OperatorName = x.OperatorName,
                    PhoneOperator = x.PhoneOperator,
                    DNIOperator = x.DNIOperator,

                    FromOtherName = x.FromOtherName,
                    FromOtherPhone = x.FromOtherPhone,
                    FromOtherDNI = x.FromOtherDNI,

                    HiringType = x.HiringType,

                    WorkerId = x.WorkerId,
                    StartDate = x.StartDate.HasValue
                    ? x.StartDate.Value.Date.ToDateString() : String.Empty,

                    FileUrl = x.FileUrl,
                    EquipmentMachineryTypeSoftId = x.EquipmentMachineryTypeSoftId,
                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId,
                    EquipmentMachineryTypeTransportId = x.EquipmentMachineryTypeTransportId

                })
                .FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryOperatorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var workerName = await _context.Workers
                .FirstOrDefaultAsync(x=>x.Id == model.WorkerId);

            var equipTypeSoft = await _context.EquipmentMachineryTypes    
                .FirstOrDefaultAsync(x=>x.Description == "Equipos Menores");

            var equipTypeType = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Maquinaria");

            var equipTransport = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Transporte");

            var machineOperator = new EquipmentMachineryOperator
            {
                ProjectId = GetProjectId(),                                                

                OperatorName = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_EMPLOYEE 
                                            ?  model.OperatorName: string.Empty,
                PhoneOperator = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_EMPLOYEE
                                            ? model.PhoneOperator : string.Empty,
                DNIOperator = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_EMPLOYEE
                                            ? model.DNIOperator: string.Empty,

                FromOtherName = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_NONE
                                            ? model.FromOtherName : string.Empty,
                FromOtherPhone = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_NONE
                                            ? model.FromOtherPhone : string.Empty,
                FromOtherDNI = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_NONE
                                            ? model.FromOtherDNI : string.Empty,

                HiringType = model.HiringType ,

                EquipmentMachineryTypeId = model.EquipmentMachineryTypeId,

                EquipmentMachineryTypeSoftId = model.EquipmentMachineryTypeId == equipTypeSoft.Id
                                            ? model.EquipmentMachineryTypeSoftId : null,
                EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeId == equipTypeType.Id
                                            ? model.EquipmentMachineryTypeTypeId : null,
                EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeId == equipTransport.Id
                                            ? model.EquipmentMachineryTypeTransportId : null,

                StartDate = string.IsNullOrEmpty(model.StartDate)
                ? (DateTime?)null : model.StartDate.ToDateTime(),


                WorkerId = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_WORKER
                                            ? model.WorkerId : null
            };

            switch(machineOperator.HiringType)
            {
                case 1:
                    machineOperator.ActualName = machineOperator.OperatorName;
                    machineOperator.ActualDni = machineOperator.DNIOperator;
                    break;
                case 2:
                    machineOperator.ActualName = machineOperator.FromOtherName;
                    machineOperator.ActualDni = machineOperator.FromOtherDNI;
                    break;
                case 3:
                    machineOperator.ActualName = workerName.Name + " " + workerName.MiddleName + " " + workerName.PaternalSurname + " " + workerName.MaternalSurname;
                    machineOperator.ActualDni = workerName.Document;
                    break;
                    //case 3:
                //    model.ActualName = 

            }


            await _context.EquipmentMachineryOperators.AddAsync(machineOperator);
            await _context.SaveChangesAsync();

            if (model.File != null) 
            {
                var storage = new CloudStorageService(_storageCredentials);
                machineOperator.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_OPERATORS,
                    $"om_{machineOperator.Id}");

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryOperatorViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var workerName = await _context.Workers
    .FirstOrDefaultAsync(x => x.Id == model.WorkerId);

            var machineOperator = await _context.EquipmentMachineryOperators
                .FirstOrDefaultAsync(x=>x.Id == id);
            var equipTypeSoft = await _context.EquipmentMachineryTypes
            .FirstOrDefaultAsync(x => x.Description == "Equipos Menores");

            var equipTypeType = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Maquinaria");

            var equipTransport = await _context.EquipmentMachineryTypes
            .FirstOrDefaultAsync(x => x.Description == "Transporte");




            machineOperator.OperatorName = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_EMPLOYEE 
                                            ? model.OperatorName : string.Empty;
            machineOperator.PhoneOperator = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_EMPLOYEE
                                            ? model.PhoneOperator : string.Empty;
            machineOperator.DNIOperator = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_EMPLOYEE 
                                            ? model.DNIOperator: string.Empty;

            machineOperator.FromOtherName = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_NONE
                                            ? model.FromOtherName : string.Empty;
            machineOperator.FromOtherPhone = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_NONE
                                        ? model.FromOtherPhone : string.Empty;
            machineOperator.FromOtherDNI = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_NONE
                                        ? model.FromOtherDNI : string.Empty;


            machineOperator.HiringType = model.HiringType;

            machineOperator.EquipmentMachineryTypeId = model.EquipmentMachineryTypeId;

            machineOperator.EquipmentMachineryTypeSoftId = model.EquipmentMachineryTypeId == equipTypeSoft.Id
                                            ? model.EquipmentMachineryTypeSoftId : null;
            machineOperator.EquipmentMachineryTypeTypeId = model.EquipmentMachineryTypeId == equipTypeType.Id
                                            ? model.EquipmentMachineryTypeTypeId : null;
            machineOperator.EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeId == equipTransport.Id
                                            ? model.EquipmentMachineryTypeTransportId : null;

            machineOperator.WorkerId = model.HiringType == ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.HIRING_TYPE_WORKER
                                            ? model.WorkerId : null;

            machineOperator.StartDate = string.IsNullOrEmpty(model.StartDate)
  ? (DateTime?)null : model.StartDate.ToDateTime();



            switch (machineOperator.HiringType)
            {
                case 1:
                    machineOperator.ActualName = machineOperator.OperatorName;
                    machineOperator.ActualDni = machineOperator.DNIOperator;
                    break;
                case 3:
                    machineOperator.ActualName = machineOperator.FromOtherName;
                    machineOperator.ActualDni = machineOperator.FromOtherDNI;
                    break;
                case 2:
                    machineOperator.ActualName = workerName.Name + " " + workerName.PaternalSurname + " " + workerName.MaternalSurname;
                    machineOperator.ActualDni = workerName.Document;
                    break;
                    //case 3:
                    //    model.ActualName = 

            }

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (machineOperator.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_OPERATORS}/{machineOperator.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
                machineOperator.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_OPERATORS,
                    $"om_{machineOperator.Id}");
            }



            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.EquipmentMachineryOperators.FirstOrDefaultAsync(x => x.Id == id);

            if (data.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_MACHINERY_OPERATORS}/{data.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.EQUIPMENT_MACHINERY);
            }

            _context.EquipmentMachineryOperators.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("qr/{id}")]
        public async Task<IActionResult> QrGenerator(Guid id)
        {


            var bprint = await _context.EquipmentMachineryOperators
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(bprint.ActualDni, QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms.ToArray(), "image/jpeg", "QR_" + bprint.ActualDni + ".jpg");
                }

            }

        }

    }
}
