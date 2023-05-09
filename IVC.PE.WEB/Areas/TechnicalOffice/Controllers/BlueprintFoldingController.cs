using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BlueprintViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
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
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/folding-planos")]
    public class BlueprintFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public BlueprintFoldingController(IvcDbContext context,
            ILogger<BlueprintFoldingController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid bpId,Guid? versionId = null)
        {

            var query = _context.BlueprintFoldings
              .AsQueryable();
            var pId = GetProjectId();
            var data = await query
                .Include(x => x.Letter)
                .Where(x=>x.BlueprintId == bpId)
                .Select(x => new BlueprintFoldingViewModel
                {
                    Id = x.Id,
                    BlueprintId = x.BlueprintId,
                    Code = x.Code,
                    FileUrl = x.FileUrl,
                    CadUrl = x.CadUrl,
                    LetterId = x.LetterId,
                    Letter = new LetterViewModel
                    {
                        FileUrl = x.Letter.FileUrl,
                        Name = x.Letter.Name
                    },
                    TechnicalVersionId = x.TechnicalVersionId,
                    TechnicalVersion = new TechnicalVersionViewModel
                    {
                        Description = x.TechnicalVersion.Description
                    },
                    BlueprintDateStr = x.BlueprintDate.HasValue
                    ? x.BlueprintDate.Value.Date.ToDateString() : String.Empty,
                })
                .ToListAsync();

            if (versionId.HasValue)
                data = data.Where(x => x.TechnicalVersionId == versionId.Value).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.BlueprintFoldings
                 .Where(x => x.Id == id)
                 .Select(x => new BlueprintFoldingViewModel
                 {
                     Id = x.Id,
                     BlueprintId = x.BlueprintId,
                     Code = x.Code,
                     FileUrl = x.FileUrl,
                     CadUrl = x.CadUrl,
                     LetterId = x.LetterId,
                     Letter = new LetterViewModel
                     {
                         FileUrl = x.Letter.FileUrl,
                         Name = x.Letter.Name
                     },
                     TechnicalVersionId = x.TechnicalVersionId,
                     BlueprintDateStr = x.BlueprintDate.HasValue
                    ? x.BlueprintDate.Value.Date.ToDateString() : String.Empty,
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BlueprintFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var responsibles = await _context.BluePrintResponsibles
    .Where(x => x.ProjectId == GetProjectId())
    .ToListAsync();
            var users = await _context.Users.ToListAsync();
            var bp = await _context.Blueprints.FirstOrDefaultAsync(x => x.Id == model.BlueprintId);
            var ver = await _context.TechnicalVersions.FirstOrDefaultAsync(x => x.Id == model.TechnicalVersionId);
            var bprint = new BlueprintFolding
            {
                BlueprintId = model.BlueprintId,
                Code = bp.Description+"-"+bp.Sheet,
                LetterId = model.LetterId,
                TechnicalVersionId = model.TechnicalVersionId,
                  
                BlueprintDate = ver.Description == "Aprobada"?(string.IsNullOrEmpty(model.BlueprintDateStr)
                ? (DateTime?)null : model.BlueprintDateStr.ToDateTime()):null,

            };

            await _context.BlueprintFoldings.AddAsync(bprint);
            await _context.SaveChangesAsync();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bprint.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.BLUEPRINT,
                    $"plano_{bprint.Code}_{ver.Description}");

                await _context.SaveChangesAsync();
            }

            if (model.Cad != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bprint.CadUrl = await storage.UploadFile(model.Cad.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE, System.IO.Path.GetExtension(model.Cad.FileName),
                    ConstantHelpers.Storage.Blobs.BLUEPRINT,
                    $"plano_{bprint.Code}_{ver.Description}");

                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            
            if (ver.Description == "Aprobada")
            {
                var res = responsibles.FirstOrDefault(x => x.ProjectId == GetProjectId() && x.ProjectFormulaId == bprint.Blueprint.ProjectFormulaId).UserId;
                SendAlertMail(res.Split(','), bprint, users);
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BlueprintFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bprint = await _context.BlueprintFoldings
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            var ver = await _context.TechnicalVersions
                .FirstOrDefaultAsync(x => x.Id == model.TechnicalVersionId);

            bprint.TechnicalVersionId = model.TechnicalVersionId;
            bprint.LetterId = model.LetterId;
            bprint.BlueprintDate = ver.Description == "Aprobada" ? (string.IsNullOrEmpty(model.BlueprintDateStr)
            ? (DateTime?)null : model.BlueprintDateStr.ToDateTime()) : null;
            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bprint.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BLUEPRINT}/{bprint.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                bprint.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.BLUEPRINT,
                    $"plano_{bprint.Code}_{ver.Description}");
            }

            if (model.Cad != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bprint.CadUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BLUEPRINT}/{bprint.CadUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                bprint.CadUrl = await storage.UploadFile(model.Cad.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.Cad.FileName),
                    ConstantHelpers.Storage.Blobs.BLUEPRINT,
                    $"plano_{bprint.Code}_{ver.Description}");
            }

            await _context.SaveChangesAsync();




            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bprint = await _context.BlueprintFoldings
                .FirstOrDefaultAsync(x => x.Id == id);

            if (bprint == null)
            {
                return BadRequest($"plano con Id '{id}' no se halló.");
            }

            if (bprint.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BLUEPRINT}/{bprint.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            if (bprint.CadUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BLUEPRINT}/{bprint.CadUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

            _context.BlueprintFoldings.Remove(bprint);
            await _context.SaveChangesAsync();
            return Ok();
        }

        



        private async void SendAlertMail(string[] responsibles, BlueprintFolding equip, List<ApplicationUser> users)
        {
            //Cargar Archivo Adjunto
            MemoryStream ms = new MemoryStream();
            Attachment attachment = null;
            if (equip.FileUrl != null)
            {
                using (var wc = new WebClient())
                {
                    byte[] bondBytes = wc.DownloadData(equip.FileUrl);
                    ms = new MemoryStream(bondBytes);
                }
                attachment = new Attachment(ms, $"plano_{equip.Code}-{equip.TechnicalVersion.Description}.pdf");
            }

            //Buscar en users para obtener los correos.
            foreach (var responsible in responsibles)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
                };

                if (attachment != null)
                    mailMessage.Attachments.Add(attachment);

                mailMessage.Subject = $"IVC - Aprobación de Plano";

                var user = users.FirstOrDefault(x => x.Id.Equals(responsible));
                if (user != null)
                {
                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
                    mailMessage.Body =
                        $"Hola, { user.FullName }, <br /><br /> " +
                        $"Se aprobo el plano  { equip.Code}  " +
                        $"Saludos <br />" +
                        $"Sistema IVC <br />" +
                        $"Control de Planos";
                    mailMessage.IsBodyHtml = true;

                    //Mandar Correo
                    using (var client = new SmtpClient("smtp.office365.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                        client.EnableSsl = true;
                        await client.SendMailAsync(mailMessage);
                        Thread.Sleep(15000);
                    }
                }
                // mail.Body("Descarga la carta fianza <a href={bond.FileUrl}>AQUÍ</a>");
            }
        }
    }
}
