using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalsViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IVC.PE.ENTITIES.Models.Bidding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionViewModels;
using IVC.PE.WEB.Services;
using IVC.PE.WEB.Options;
using IVC.PE.ENTITIES.UspModels.Biddings;
using ClosedXML.Excel;
using System.IO;
using Microsoft.Data.SqlClient;

namespace IVC.PE.WEB.Areas.Bidding.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Bidding.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.BIDDING)]
    [Route("licitaciones/profesionales")]
    public class ProfessionalController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public ProfessionalController(IvcDbContext context,
        IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<ProfessionalController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? profe,Guid? positionId = null)
        {
            var data = await _context.Set<UspProfessional>().FromSqlRaw("execute Bidding_uspProfessional")
         .IgnoreQueryFilters()
         .ToListAsync();

            if(profe.HasValue)
            {
                data = data.Where(x => x.ProfessionId == profe ).ToList();
            }

            if (positionId.HasValue)
            {
                data = data.Where(x => x.PositionId == positionId && x.FoldingId != null).ToList();
            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            //TODO: Actualizar el modal de Editar
            var model = await _context.Professionals
                .Where(x => x.Id == id)
                .Include(x => x.Profession)
                .Select(x => new ProfessionalViewModel
                {
                    Id = x.Id,
                    Name = x.Name,

                    Address = x.Address,
                    University = x.University,
                    Nacionality = x.Nacionality,

                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    Document = x.Document,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    CIPNumber = x.CIPNumber,
                    ProfessionId = x.ProfessionId,
                    Profession = new ProfessionViewModel
                    {
                        Id = x.Profession.Id,
                        Name = x.Profession.Name,
                    },
                    BirthDateStr = x.BirthDate.Date.ToDateString(),
                    StartTitleStr = x.StartTitle.Date.ToDateString(),
                    CipDateStr = x.CipDate.Date.ToDateString(),
                ValidationSunedu = x.ValidationSunedu,
                CertiAdult = x.CertiAdult,
                DniUrl = x.DniUrl,
                TitleUrl = x.TitleUrl,
                CipUrl = x.CipUrl,
                CertiAdultUrl = x.CertiAdultUrl,
                CapacitationUrl = x.CapacitationUrl
                }).FirstOrDefaultAsync();
            return Ok(model);
        }


        [HttpGet("usp/{id}")]
        public async Task<IActionResult> GetProcedure(Guid id)
        {
            if (id == Guid.Empty)
                return Ok(new List<ProfessionalViewModel>());

            var data = _context.Set<UspProfessionalExperienceNumber>().FromSqlRaw("execute Bidding_uspProfessionalExperienceNumber")
.AsEnumerable()
.Where(x => x.Id == id)
 .Select(x => new ProfessionalViewModel
 {
     
     Dif = x.Dif,
     Name = x.Name,
     PaternalSurname = x.PaternalSurname,
     MaternalSurname = x.MaternalSurname,
     Years = x.Years,
     Months = x.Months,
     Days = x.Days,


     
 }).FirstOrDefault();


            ////TODO: Actualizar el modal de Editar
            //var model = await _context.Professionals
            //    .Where(x => x.Id == id)
            //    .Include(x => x.Profession)
            //    .Select(x => new ProfessionalViewModel
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        MiddleName = x.MiddleName,
            //        PaternalSurname = x.PaternalSurname,
            //        MaternalSurname = x.MaternalSurname,
            //        Document = x.Document,
            //        PhoneNumber = x.PhoneNumber,
            //        Email = x.Email,
            //        CIPNumber = x.CIPNumber,
            //        ProfessionId = x.ProfessionId,
            //        Profession = new ProfessionViewModel
            //        {
            //            Id = x.Profession.Id,
            //            Name = x.Profession.Name,
            //        },
            //        BirthDateStr = x.BirthDate.Date.ToDateString(),
            //        StartTitleStr = x.StartTitle.Date.ToDateString(),
            //        CipDateStr = x.CipDate.Date.ToDateString(),
            //        ValidationSunedu = x.ValidationSunedu,
            //        CertiAdult = x.CertiAdult,
            //        DniUrl = x.DniUrl,
            //        TitleUrl = x.TitleUrl,
            //        CipUrl = x.CipUrl,
            //        CertiAdultUrl = x.CertiAdultUrl,
            //        CapacitationUrl = x.CapacitationUrl
            //    }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ProfessionalViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existActive = await _context.Professionals.FirstOrDefaultAsync(x => x.Document == model.Document);
            if (existActive != null)
                return BadRequest("El Profesional ya existe en la base de datos.");

            var professional = new Professional
            {
                Address = model.Address,
                University = model.University,
                Nacionality = model.Nacionality,
                Name = model.Name,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Document = model.Document,
                ProfessionId = model.ProfessionId,
                CIPNumber = model.CIPNumber,
                    BirthDate = model.BirthDateStr.ToDateTime(),
                StartTitle = model.StartTitleStr.ToDateTime(),
                CipDate = model.CipDateStr.ToDateTime(),
                ValidationSunedu = model.ValidationSunedu,
                CertiAdult = model.CertiAdult

            };

            if (model.FileTitle != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.TitleUrl = await storage.UploadFile(model.FileTitle.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileTitle.FileName),
                    ConstantHelpers.Storage.Blobs.TITLE_PROFESSIONAL,
                    $"titulo-profesional_{model.Document}");
            }

            if (model.FileCapacitation != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.CapacitationUrl = await storage.UploadFile(model.FileCapacitation.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileCapacitation.FileName),
                    ConstantHelpers.Storage.Blobs.CAPACITATION_PROFESSIONAL,
                    $"capacitacion_{model.Document}");
            }

            if (model.FileDni != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.DniUrl = await storage.UploadFile(model.FileDni.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileDni.FileName),
                    ConstantHelpers.Storage.Blobs.DNI_PROFESSIONAL,
                    $"dni_{model.Document}");
            }

            if (model.FileCip != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.CipUrl = await storage.UploadFile(model.FileCip.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileCip.FileName),
                    ConstantHelpers.Storage.Blobs.CIP_PROFESSIONAL,
                    $"cip_{model.Document}");
            }

            if (model.FileCerti != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                professional.CertiAdultUrl = await storage.UploadFile(model.FileCerti.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileCerti.FileName),
                    ConstantHelpers.Storage.Blobs.CERTI_PROFESSIONAL,
                    $"certi_{model.Document}");
            }

            await _context.Professionals.AddAsync(professional);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ProfessionalViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var professional = await _context.Professionals.FirstOrDefaultAsync(x => x.Id == id);

            professional.Address= model.Address;
            professional.University = model.University;
            professional.Nacionality = model.Nacionality;

            professional.Name = model.Name;
            professional.PaternalSurname = model.PaternalSurname;
            professional.MaternalSurname = model.MaternalSurname;
            professional.Document = model.Document;
            professional.PhoneNumber = model.PhoneNumber;
            professional.Email = model.Email;
            professional.ProfessionId = model.ProfessionId;
            professional.CIPNumber = model.CIPNumber;
            professional.BirthDate = model.BirthDateStr.ToDateTime();
            professional.StartTitle = model.StartTitleStr.ToDateTime();
            professional.CipDate = model.CipDateStr.ToDateTime();
            professional.ValidationSunedu = model.ValidationSunedu;
            professional.CertiAdult = model.CertiAdult;
            if (model.FileTitle != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.TitleUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.TITLE_PROFESSIONAL}/{professional.TitleUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.TitleUrl = await storage.UploadFile(model.FileTitle.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileTitle.FileName),
                    ConstantHelpers.Storage.Blobs.TITLE_PROFESSIONAL,
                     $"titulo-profesional_{model.Document}");
            }

            if (model.FileDni != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.DniUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DNI_PROFESSIONAL}/{professional.DniUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.DniUrl = await storage.UploadFile(model.FileDni.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileDni.FileName),
                    ConstantHelpers.Storage.Blobs.DNI_PROFESSIONAL,
                     $"dni_{model.Document}");
            }

            if (model.FileCip != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.CipUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CIP_PROFESSIONAL}/{professional.CipUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.CipUrl = await storage.UploadFile(model.FileCip.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileCip.FileName),
                    ConstantHelpers.Storage.Blobs.CIP_PROFESSIONAL,
                     $"cip_{model.Document}");
            }

            if (model.FileCerti != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.CertiAdultUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CERTI_PROFESSIONAL}/{professional.CertiAdultUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.CertiAdultUrl = await storage.UploadFile(model.FileCerti.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileCerti.FileName),
                    ConstantHelpers.Storage.Blobs.CERTI_PROFESSIONAL,
                     $"certi_{model.Document}");
            }

            if (model.FileCapacitation != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (professional.CapacitationUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CAPACITATION_PROFESSIONAL}/{professional.CapacitationUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.BIDDING);
                professional.CapacitationUrl = await storage.UploadFile(model.FileCapacitation.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.BIDDING,
                    System.IO.Path.GetExtension(model.FileCapacitation.FileName),
                    ConstantHelpers.Storage.Blobs.CAPACITATION_PROFESSIONAL,
                     $"capacitacion_{model.Document}");
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var professional = await _context.Professionals.FirstOrDefaultAsync(x => x.Id == id);
            if (professional == null)
                return BadRequest($"Profesional con Id '{id}' no encontrado.");

            if (professional.CapacitationUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CAPACITATION_PROFESSIONAL}/{professional.CapacitationUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }

            if (professional.CertiAdultUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CERTI_PROFESSIONAL}/{professional.CertiAdultUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            if (professional.CipUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.CIP_PROFESSIONAL}/{professional.CipUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            if (professional.DniUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.DNI_PROFESSIONAL}/{professional.DniUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            if (professional.TitleUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.TITLE_PROFESSIONAL}/{professional.TitleUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.BIDDING);
            }
            _context.Professionals.Remove(professional);


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("detalle-folding-excel")]
        public async Task<IActionResult> ExcelReport(Guid proId)
        {
            SqlParameter param1 = new SqlParameter("@ProId", proId);


            var data = await _context.Set<UspProfessionalFirstExcel>()
                .FromSqlRaw("execute Bidding_uspProfessionalFirstExcel @ProId"
                , param1)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();



            var data2 = await _context.Set<UspProfessionalSecondExcel>()
                .FromSqlRaw("execute Bidding_uspProfessionalSecondExcel @ProId"
                , param1)
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToListAsync();

            var data3 = await _context.Set<UspProfessionalExperienceNumber>().FromSqlRaw("execute Bidding_uspProfessionalExperienceNumber")
.IgnoreQueryFilters()
.ToListAsync();


            using (XLWorkbook wb = new XLWorkbook())
            {


                var first = data.FirstOrDefault();

                var third = data3.Where(x=>x.Id == first.Id).FirstOrDefault();
                var ws = wb.Worksheets.Add("Resumen");
                var count = 3;

                ws.Cell($"B1").Value = "RESUMEN DE EXPERIENCIA LABORAL";
                ws.Range($"B1:I1").Merge();

                ws.Cell($"B{1}").Style.Font.Bold = true;
                ws.Cell($"B{1}").Style.Font.FontSize = 18.0;

                ws.Cell($"B{count}").Value = "Información Personal";
                ws.Cell($"B{count}").Style.Font.Bold = true;
                ws.Range($"B{count}:C{count+5}").Merge();
                //ws.Range($"A{count+1}:C{count+1}").Merge();
                //ws.Range($"A{count+2}:C{count+2}").Merge();
                //ws.Range($"A{count+3}:C{count+3}").Merge();
                //ws.Range($"A{count + 4}:C{count + 4}").Merge();
                //ws.Range($"A{count + 5}:C{count + 5}").Merge();
                SetRowBorderStyle(ws, count, "C");
                SetRowBorderStyle(ws, count+1, "C");
                SetRowBorderStyle(ws, count+2, "C");
                SetRowBorderStyle(ws, count+3, "C");
                SetRowBorderStyle(ws, count + 4, "C");
                SetRowBorderStyle(ws, count + 5, "C");

                SetRowBorderStyle(ws, count, "B");
                SetRowBorderStyle(ws, count + 1, "B");
                SetRowBorderStyle(ws, count + 2, "B");
                SetRowBorderStyle(ws, count + 3, "B");
                SetRowBorderStyle(ws, count + 4, "B");
                SetRowBorderStyle(ws, count + 5, "B");



                ws.Cell($"B{count+6}").Value = "Calificaciones Profesionales";
                ws.Range($"B{count+6}:C{count + 9}").Merge();

                ws.Cell($"B{count+6}").Style.Font.Bold = true;
                //SetRowBorderStyle(ws, count+4, "C");
                //SetRowBorderStyle(ws, count + +5, "C");
                //SetRowBorderStyle(ws, count + 6, "C");
                //SetRowBorderStyle(ws, count + 7, "C");



                ws.Column(3).Width = 30;
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 20;

                ws.Column(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                ws.Column(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                ws.Column(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Column(4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(4).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Column(5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Column(6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(6).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Column(7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(7).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Column(8).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Cell($"D{count}").Value = "Nombres y Apellidos :";
                ws.Cell($"D{count}").Style.Font.Bold = true;
                ws.Range($"D{count}:E{count}").Merge();
                ws.Cell($"D{count+1}").Value = first.Name + " " + first.PaternalSurname + " " +first.MaternalSurname;
                ws.Range($"D{count+1}:E{count+1}").Merge();

                ws.Cell($"F{count}").Value = "Documento de Identidad :";
                ws.Cell($"F{count + 1}").Value = first.Document;
                ws.Range($"F{count}:G{count}").Merge();
                ws.Range($"F{count+1}:G{count+1}").Merge();
                ws.Cell($"F{count}").Style.Font.Bold = true;

                ws.Cell($"H{count}").Value = "Fecha de Nacimiento :";
                ws.Cell($"H{count + 1}").Value = first.BirthDateString;
                ws.Cell($"H{count}").Style.Font.Bold = true;

                ws.Range($"H{count}:I{count}").Merge();
                ws.Range($"H{count+1}:I{count+1}").Merge();

                ws.Cell($"D{count + 2}").Value = "Domicilio :";
                ws.Cell($"D{count + 3}").Value = first.Address;
                ws.Range($"D{count + 2}:I{count + 2}").Merge();
                ws.Range($"D{count + 3}:I{count + 3}").Merge();
                ws.Cell($"D{count+2}").Style.Font.Bold = true;



                ws.Cell($"D{count+4}").Value = "Correo Electrónico :";
                ws.Cell($"D{count + 5}").Value = first.Email;
                ws.Range($"D{count + 4}:E{count + 4}").Merge();
                ws.Range($"D{count + 5}:E{count + 5}").Merge();
                ws.Cell($"D{count+4}").Style.Font.Bold = true;


                ws.Cell($"F{count +4}").Value = "Celular :";
                ws.Cell($"F{count +5}").Value = first.PhoneNumber;
                ws.Range($"F{count + 4}:G{count + 4}").Merge();
                ws.Range($"F{count + 5}:G{count + 5}").Merge();
                ws.Cell($"F{count + 4}").Style.Font.Bold = true;



                ws.Cell($"H{count + 4}").Value = "Nacionalidad :";
                ws.Cell($"H{count + 5}").Value = first.Nacionality;
                ws.Range($"H{count + 4}:I{count + 4}").Merge();
                ws.Range($"H{count + 5}:I{count + 5}").Merge();
                ws.Cell($"H{count + 4}").Style.Font.Bold = true;


                ws.Cell($"D{count+6}").Value = "Carrera o Especialidad :";
                ws.Cell($"D{count + 7}").Value = first.Profession;
                ws.Range($"D{count + 6}:E{count + 6}").Merge();
                ws.Range($"D{count + 7}:E{count + 7}").Merge();
                ws.Cell($"D{count + 6}").Style.Font.Bold = true;


                ws.Cell($"F{count+6}").Value = "Fecha de Expedición del Título :";
                ws.Cell($"F{count + 7}").Value = first.StartTitleString;
                ws.Range($"F{count + 6}:G{count + 6}").Merge();
                ws.Range($"F{count + 7}:G{count + 7}").Merge();
                ws.Cell($"F{count + 6}").Style.Font.Bold = true;

                ws.Cell($"H{count+6}").Value = "Sunedu :";
                ws.Cell($"H{count + 7}").Value = first.ValidationSunedu == true? "Si" : "No";
                ws.Range($"H{count + 6}:I{count + 6}").Merge();
                ws.Range($"H{count + 7}:I{count + 7}").Merge();
                ws.Cell($"H{count + 6}").Style.Font.Bold = true;

                ws.Cell($"D{count + 8}").Value = "Universidad o Centro de Estudios:";
                ws.Cell($"D{count + 9}").Value = first.University;
                ws.Range($"D{count + 8}:E{count + 8}").Merge();
                ws.Range($"D{count + 9}:E{count + 9}").Merge();
                ws.Cell($"D{count + 8}").Style.Font.Bold = true;


                ws.Cell($"F{count + 8}").Value = "Fecha de Colegiatura :";
                ws.Cell($"F{count + 9}").Value = first.CipDateString;
                ws.Range($"F{count + 8}:G{count + 8}").Merge();
                ws.Range($"F{count + 9}:G{count + 9}").Merge();
                ws.Cell($"F{count + 8}").Style.Font.Bold = true;


                ws.Cell($"H{count + 8}").Value = "Colegiatura Nro. :";
                ws.Cell($"H{count + 9}").Value = first.CIPNumber;
                ws.Range($"H{count + 8}:I{count + 8}").Merge();
                ws.Range($"H{count + 9}:I{count + 9}").Merge();
                ws.Cell($"H{count + 8}").Style.Font.Bold = true;

                //ws.Cell($"D{count + 8}").Value = "Nombre del Empleador:";
                //ws.Cell($"D{count + 9}").Value = first.Business;

                //ws.Cell($"D{count + 10}").Value = "Cargo Actual:";
                //ws.Cell($"D{count + 11}").Value = first.Position;




                SetRowBorderStyle(ws, count, "I");

                SetRowBorderStyle(ws, count + 1, "I");

                SetRowBorderStyle(ws, count + 2, "I");
  

                SetRowBorderStyle(ws, count + 3, "I");


                SetRowBorderStyle(ws, count + 4, "I");


                SetRowBorderStyle(ws, count + 5, "I");


                SetRowBorderStyle(ws, count + 6, "I");


                SetRowBorderStyle(ws, count + 7, "I");


                SetRowBorderStyle(ws, count + 8, "I");

                SetRowBorderStyle(ws, count + 9, "I");



                count = 18;
                //count = 4;
                ws.Cell($"B{count}").Value = "N°";
                ws.Cell($"B{count}").Style.Font.Bold = true;
                ws.Cell($"C{count}").Value = "Cliente o Empleador";
                ws.Cell($"C{count}").Style.Font.Bold = true;
                ws.Cell($"D{count}").Value = "Objeto de la contratación ";
                ws.Cell($"D{count}").Style.Font.Bold = true;
                ws.Cell($"E{count}").Value = "Cargo desempeñado";
                ws.Cell($"E{count}").Style.Font.Bold = true;
                ws.Cell($"F{count}").Value = "Fecha de Inicio";
                ws.Cell($"F{count}").Style.Font.Bold = true;
                ws.Cell($"G{count}").Value = "Fecha de Culminación";
                ws.Cell($"G{count}").Style.Font.Bold = true;
                ws.Cell($"H{count}").Value = "Tiempo Acumulado (días)";
                ws.Cell($"H{count}").Style.Font.Bold = true;
                ws.Cell($"I{count}").Value = "Observaciones";
                ws.Cell($"I{count}").Style.Font.Bold = true;
                SetRowBorderStyle2(ws, count, "I");
                foreach (var second in data2)
                {

                    ws.Cell($"B{count+1}").Value = second.Order;
                    ws.Cell($"C{count+1}").Value = second.Business;
                    ws.Cell($"D{count+1}").Value = second.BiddingWork;
                    ws.Cell($"E{count+1}").Value = second.Position;
                    ws.Cell($"F{count+1}").Value = second.StartDateString;
                    ws.Cell($"G{count + 1}").Value = second.EndDateString;
                    ws.Cell($"H{count+1}").Value = second.Dif;
                    ws.Cell($"I{count+1}").Value = second.Observations;


                    count++;
                    SetRowBorderStyle2(ws, count, "I");

                    ws.Cell($"D{count }").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Distributed);
                    ws.Cell($"D{count }").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Distributed);
                    

                }

                count = 18 + data2.Count +2 ;

                ws.Cell($"B{count}").Value = "La experiencia total acumulada es : Años " + third.Years +" ,Meses "+ third.Months +" Días "+ third.Days;
                ws.Range($"B{count}:E{count}").Merge();
                ws.Cell($"B{count}").Style.Font.Bold = true;
                //ws.Cell($"K{count}").Value = days;
                //ws.Cell($"M{count}").Value = ammount;
                //ws.Cell($"N{count}").Value = igv;
                //ws.Cell($"O{count}").Value = totalammount;
                //ws.Column(12).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //ws.Column(13).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //ws.Column(14).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //ws.Column(15).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                //ws.Columns().AdjustToContents();


                //ws.Range($"K{count}:K{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"K{count}:K{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //ws.Range($"M{count}:M{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"M{count}:M{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //ws.Range($"N{count}:N{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"N{count}:N{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                //ws.Range($"O{count}:O{count}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //ws.Range($"O{count}:O{count}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;


                using (MemoryStream stream = new MemoryStream())
                {
                    var frs = data.FirstOrDefault();
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Resumen de experiencia " + frs.Name + ".xlsx");
                }

            }

        }

        private void SetRowBorderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetRowBorderStyle2(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"B{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }
    }
}
