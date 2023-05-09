using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcedureViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcessViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/procedimiento")]
    public class ProcedureController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProcedureController(IvcDbContext context,
            ILogger<ProcedureController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll( Guid? processId = null,Guid? documentTypeId = null)
        {

            SqlParameter param1 = new SqlParameter("@ProcessId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)processId ?? DBNull.Value;


            var data = await _context.Set<UspProcedureProcess>().FromSqlRaw("execute TechnicalOffice_uspProcedureProcess @ProcessId"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            data = data.Where(x=>x.ProjectId == GetProjectId()).ToList();

            if(processId.HasValue)
                data = data.Where(x => x.Processes != null).ToList();
            if (documentTypeId.HasValue)
                data = data.Where(x => x.DocumentTypeId == documentTypeId.Value).ToList();


            //if (specId.HasValue)
            //    data = data.Where(x=>x.SpecialityId == specId.Value).ToList();


            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Procedures

                 .Where(x => x.Id == id)
                 .Select(x => new ProcedureViewModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Code = x.Code,
                     DocumentTypeId = x.DocumentTypeId,
                     //ProcessId = x.ProcessId,
                     
                     FileUrl = x.FileUrl,
                     FileUrl2 = x.FileUrl2
                 }).FirstOrDefaultAsync();

            var processes = await _context.ProceduresProcesses
.Where(x => x.ProcedureId == id)
.Select(x => x.ProcessId)
.ToListAsync();

            data.Processes = processes;

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProcedureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var procedure = new Procedure
            {
                Name = model.Name,
                Code = model.Code,
                ProjectId = GetProjectId(),
                DocumentTypeId = model.DocumentTypeId
                //ProcessId = model.ProcessId,

            };

            if (model.Processes != null)
                await _context.ProceduresProcesses.AddRangeAsync(
                model.Processes.Select(x => new ProceduresProcess
                {
                    Procedure = procedure,
                    ProcessId = x
                }).ToList());

            await _context.Procedures.AddAsync(procedure);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                procedure.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PROCEDURE,
                    $"procedimiento_{procedure.Id}");
                await _context.SaveChangesAsync();
            }

            if (model.File2 != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                procedure.FileUrl2 = await storage.UploadFile(model.File2.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File2.FileName),
                    ConstantHelpers.Storage.Blobs.PROCEDURE,
                    $"procedimiento_adjunto2_{procedure.Id}");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProcedureViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Procedure = await _context.Procedures
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            Procedure.Name = model.Name;
            Procedure.Code = model.Code;
            Procedure.DocumentTypeId = model.DocumentTypeId;
            //Procedure.ProcessId = model.ProcessId;

            var SpecialitiesDb = await _context.ProceduresProcesses
.Where(x => x.ProcedureId == id)
.ToListAsync();

            _context.ProceduresProcesses.RemoveRange(SpecialitiesDb);
            if (model.Processes != null)
                await _context.ProceduresProcesses.AddRangeAsync(
                model.Processes.Select(x => new ProceduresProcess
                {
                    Procedure = Procedure,
                    ProcessId = x
                }).ToList());

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (Procedure.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PROCEDURE}/{Procedure.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                Procedure.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PROCEDURE,
                    $"procedimiento_{Procedure.Id}");
            }

            if (model.File2 != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (Procedure.FileUrl2 != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PROCEDURE}/{Procedure.FileUrl2.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                Procedure.FileUrl2 = await storage.UploadFile(model.File2.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File2.FileName),
                    ConstantHelpers.Storage.Blobs.PROCEDURE,
                    $"procedimiento_{Procedure.Id}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var procedure = await _context.Procedures
                .FirstOrDefaultAsync(x => x.Id == id);
            var specialitiesDb = await _context.ProceduresProcesses
.Where(x => x.ProcedureId == id)
.ToListAsync();

            if (procedure == null)
            {
                return BadRequest($"Procedimiento con Id '{id}' no se halló.");
            }

            if (procedure.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PROCEDURE}/{procedure.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            if (procedure.FileUrl2 != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PROCEDURE}/{procedure.FileUrl2.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }
            _context.ProceduresProcesses.RemoveRange(specialitiesDb);
            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("descargar/{id}")]
        public async Task<IActionResult> GetFileUrl2(Guid id)
        {
            MemoryStream ms = null;
            var data = await _context.Procedures

                 .Where(x => x.Id == id)
                 .Select(x => new ProcedureViewModel
                 {
                     FileUrl2 = x.FileUrl2
                 }).FirstOrDefaultAsync();

            byte[] dt = null;




            using (var wc = new System.Net.WebClient())
            using (MemoryStream stream = new MemoryStream(wc.DownloadData(data.FileUrl2)))
            {


                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Val."+".xlsx");
            }

        }

        [HttpGet("qr/{id}")]
        public async Task<IActionResult> QrGenerator(Guid id)
        {
            var data = await _context.Procedures
                .FirstOrDefaultAsync(x => x.Id == id);

            if (string.IsNullOrEmpty(data.FileUrl.ToString()))
                return BadRequest("No se ha cargado el archivo pdf");

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data.FileUrl.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ms.Position = 0;
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms.ToArray(), "image/jpeg", "QR_" + data.Name + ".jpg");
                }
            }

        }

        [HttpPost("adjuntar-archivo/{id}")]
        public async Task<IActionResult> AddFiles(Guid id, ProcedureViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var procedure = await _context.Procedures.FirstOrDefaultAsync(x => x.Id == id);

            var newFile = new ProcedureFile();
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                
                newFile.ProcedureId = id;

                var currentFiles = _context.ProcedureFiles.Where(x => x.ProcedureId == id);
                var version = currentFiles.Count();

                if(version > 0)
                {
                    newFile.Version = currentFiles.Max(x => x.Version) + 1;
                }
                else
                {
                    newFile.Version = 1;
                }

                newFile.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                                    Path.GetExtension(model.File.FileName),
                                    ConstantHelpers.Storage.Blobs.PROCEDURE,
                                    $"procedimiento_{procedure.Id}_v{newFile.Version}");

                if (procedure.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PROCEDURE}/{procedure.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                procedure.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.PROCEDURE,
                    $"procedimiento_{procedure.Id}");

                await _context.ProcedureFiles.AddAsync(newFile);
            }

            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("archivo/eliminar")]
        public async Task<IActionResult> DeleteFile(Uri url)
        {
            var file = await _context.ProcedureFiles
                .Include(x => x.Procedure)
                .FirstOrDefaultAsync(x => x.FileUrl == url);

            if (file.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PROCEDURE}/{file.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.ProcedureFiles.Remove(file);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("qr/archivo")]
        public async Task<IActionResult> GetFileQR(Uri url)
        {
            var file = await _context.ProcedureFiles
                .Include(x => x.Procedure)
                .FirstOrDefaultAsync(x => x.FileUrl == url);

            if (file.FileUrl != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(file.FileUrl.ToString(), QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);

                    using (Bitmap bitMap = qrCode.GetGraphic(20))
                    {
                        bitMap.Save(ms, ImageFormat.Png);
                        ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        ms.Position = 0;
                        ms.Seek(0, SeekOrigin.Begin);
                        return File(ms.ToArray(), "image/jpeg", "QR_" + file.Procedure.Name + "_v" + file.Version + ".jpg");
                    }
                }
            }
            else
            {
                return BadRequest();
            }

        }
        
    }
}
