using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.ENTITIES.UspModels.Quality;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels;
using ClosedXML.Excel;
using System.Net;
using System.Drawing;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/equipos-certificados")]
    public class EquipmentCertificateController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentCertificateController(IvcDbContext context,
            ILogger<EquipmentCertificateController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? equipmentCertificateTypeId = null,int? validity = null, int? hasavoid = null, int? operation =null, int? situation = null)
        {
            var pId = GetProjectId();

            var data = await _context.Set<UspEquipmentCertificates>().FromSqlRaw("execute Quality_uspEquipmentCertificates")
                .IgnoreQueryFilters()
                .ToListAsync();
            
            data = data.Where(x => x.ProjectId == pId).ToList();

            #region EquipmentCertificateTypeFilters
            if (equipmentCertificateTypeId.HasValue)
                data = data.Where(x => x.EquipmentCertificateTypeId == equipmentCertificateTypeId.Value).ToList();
            #endregion

            #region ValidityFilters
            if (validity > 30 && validity < 9999)
                data = data.Where(x => x.Validity > 30 && x.Validity < 9999).ToList();
            if (validity > 15 && validity <= 30)
                data = data.Where(x => x.Validity > 15 && x.Validity <= 30).ToList();
            if (validity > 0 && validity <= 15)
                data = data.Where(x => x.Validity > 0 && x.Validity <= 15).ToList();
            if (validity < 0)
                data = data.Where(x => x.Validity < 0).ToList();
            #endregion

            if (hasavoid == 1)
                data = data.Where(x => x.HasAVoid == false).ToList();
            if (hasavoid == 0)
                data = data.Where(x => x.HasAVoid == true).ToList();

            if (situation == 0)
                data = data.Where(x => x.SituationStatus == 0).ToList();
            if (situation == 1)
                data = data.Where(x => x.SituationStatus == 1).ToList();
            if (situation == 2)
                data = data.Where(x => x.SituationStatus == 2).ToList();

            if (operation == 0)
                data = data.Where(x => x.OperationalStatus == 0).ToList();
            if (operation == 1)
                data = data.Where(x => x.OperationalStatus == 1).ToList();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentCertificates
                .Include(x => x.EquipmentCertificateOwner)
                .Include(x => x.EquipmentCertificateType)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentCertificateViewModel
                {
                    Id = x.Id,
                    Correlative = x.Correlative,
                    Name = x.Name,
                    Brand = x.Brand,
                    Model = x.Model,
                    Serial = x.Serial,

                    EquipmentCertificateOwnerId = x.EquipmentCertificateOwnerId,
                    EquipmentCertificateOwner = new EquipmentCertificateOwnerViewModel
                    {
                        Id = x.EquipmentCertificateOwnerId,
                        OwnerName = x.EquipmentCertificateOwner.OwnerName
                    },
                    EquipmentCertificateTypeId = x.EquipmentCertificateTypeId,
                    EquipmentCertificateType = new EquipmentCertificateTypeViewModel
                    {
                        Id = x.EquipmentCertificateTypeId,
                        CertificateTypeName = x.EquipmentCertificateType.CertificateTypeName
                    },
                    NumberOfRenovations = x.NumberOfRenovations,
                    EntryDate = x.EntryDate.Value.ToDateString(),

                }).FirstOrDefaultAsync();

            var renewal = await _context.EquipmentCertificateRenewals.
                Where(x => x.EquipmentCertificateId == data.Id && x.RenewalOrder == data.NumberOfRenovations)
                .Select(x => new EquipmentCertificateRenewalViewModel
                {
                    Id = x.Id,
                    Observation = x.Observation,
                    EquipmentCertificateId = x.EquipmentCertificateId,
                    RenewalOrder = x.RenewalOrder,
                    EquipmentCertificateNumber = x.EquipmentCertificateNumber,
                    StartDate = x.StartDate.ToDateString(),
                    EndDate = x.EndDate.ToDateString(),
                    EquipmentCertificateUserOperatorId = x.EquipmentCertificateUserOperatorId,
                    EquipmentCertifyingEntityId = x.EquipmentCertifyingEntityId,
                    PatternCalibrationRenewalId = x.PatternCalibrationRenewalId,
                    OperationalStatus = x.OperationalStatus,
                    SituationStatus = x.SituationStatus,
                    QualityFrontId = x.QualityFrontId.Value,
                    FileUrl = x.FileUrl,
                    InspectionType = x.InspectionType,
                    CalibrationFrecuency = x.CalibrationFrecuency,
                    CalibrationMethod = x.CalibrationMethod

                }).FirstOrDefaultAsync();

            var responsibles = await _context.EquipmentCertificateRenewalApplicationUsers
                .Where(x => x.EquipmentCertificateRenewalId == renewal.Id)
                .Select(x => x.UserId)
                .ToListAsync();

            renewal.Responsibles = responsibles;
            data.EquipmentCertificateRenewal = renewal;

            return Ok(data);
        }
        [HttpGet("patron/{id}")]
        public async Task<IActionResult> GetPattern(Guid id)
        {
            var data = await _context.PatternCalibrationRenewals
                .Where(x => x.Id == id)
                .Select(x => new PatternCalibrationRenewalViewModel
                {
                    Id = x.Id,
                    FileUrl = x.FileUrl
                }).FirstOrDefaultAsync();



            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentCertificateViewModel mod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var data = new EquipmentCertificate
            {
                Correlative = mod.Correlative,
                Name = mod.Name,
                Brand = mod.Brand,
                Model = mod.Model,
                Serial = mod.Serial,
                EntryDate = mod.EntryDate.ToDateTime(),

                EquipmentCertificateOwnerId = mod.EquipmentCertificateOwnerId,
                EquipmentCertificateTypeId = mod.EquipmentCertificateTypeId,
                ProjectId = GetProjectId(),
                NumberOfRenovations = 1
            };

            var renewal = new EquipmentCertificateRenewal
            {

                EquipmentCertificate = data,
                Observation = mod.EquipmentCertificateRenewal.Observation,
                EndDate = mod.EquipmentCertificateRenewal.EndDate.ToDateTime(),
                EquipmentCertificateNumber = mod.EquipmentCertificateRenewal.EquipmentCertificateNumber,
                OperationalStatus = mod.EquipmentCertificateRenewal.OperationalStatus,
                RenewalOrder = data.NumberOfRenovations,
                EquipmentCertifyingEntityId = mod.EquipmentCertificateRenewal.EquipmentCertifyingEntityId,
                EquipmentCertificateUserOperatorId = mod.EquipmentCertificateRenewal.EquipmentCertificateUserOperatorId,
                SituationStatus = mod.EquipmentCertificateRenewal.SituationStatus,
                StartDate = mod.EquipmentCertificateRenewal.StartDate.ToDateTime(),
                PatternCalibrationRenewalId = mod.EquipmentCertificateRenewal.PatternCalibrationRenewalId,
                Days15 = false,
                Days30=false,
                QualityFrontId = mod.EquipmentCertificateRenewal.QualityFrontId.Value,
                InspectionType = mod.EquipmentCertificateRenewal.InspectionType,
                CalibrationFrecuency = mod.EquipmentCertificateRenewal.CalibrationFrecuency,
                CalibrationMethod = mod.EquipmentCertificateRenewal.CalibrationMethod
            };



            await _context.EquipmentCertificateRenewalApplicationUsers
                .AddRangeAsync(mod.EquipmentCertificateRenewal.Responsibles
                    .Select(x => new EquipmentCertificateRenewalApplicationUser
                    {
                        EquipmentCertificateRenewal = renewal,
                        UserId = x
                    }));

            await _context.EquipmentCertificateRenewals.AddAsync(renewal);
            await _context.EquipmentCertificates.AddAsync(data);
            await _context.SaveChangesAsync();

            if (mod.EquipmentCertificateRenewal.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                renewal.FileUrl = await storage.UploadFile(mod.EquipmentCertificateRenewal.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(mod.EquipmentCertificateRenewal.File.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES,
                    $"eqp_{data.Id}-ctf_{renewal.RenewalOrder}");

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentCertificateViewModel mod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.EquipmentCertificates.FirstOrDefaultAsync(x => x.Id == id);
            data.Correlative = mod.Correlative;
            data.Name = mod.Name;
            data.Brand = mod.Brand;
            data.Model = mod.Model;
            data.Serial = mod.Serial;
            data.EquipmentCertificateOwnerId = mod.EquipmentCertificateOwnerId;
            data.EquipmentCertificateTypeId = mod.EquipmentCertificateTypeId;
            data.EntryDate = mod.EntryDate.ToDateTime();


            var renewal = await _context.EquipmentCertificateRenewals.FirstOrDefaultAsync(x => x.Id == mod.EquipmentCertificateRenewal.Id);
            renewal.Observation = mod.EquipmentCertificateRenewal.Observation;
            renewal.StartDate = mod.EquipmentCertificateRenewal.StartDate.ToDateTime();
            renewal.EndDate = mod.EquipmentCertificateRenewal.EndDate.ToDateTime();
            renewal.EquipmentCertificateNumber = mod.EquipmentCertificateRenewal.EquipmentCertificateNumber;
            renewal.OperationalStatus = mod.EquipmentCertificateRenewal.OperationalStatus;
            renewal.EquipmentCertifyingEntityId = mod.EquipmentCertificateRenewal.EquipmentCertifyingEntityId;
            renewal.EquipmentCertificateUserOperatorId = mod.EquipmentCertificateRenewal.EquipmentCertificateUserOperatorId;
            renewal.SituationStatus = mod.EquipmentCertificateRenewal.SituationStatus;
            renewal.PatternCalibrationRenewalId = mod.EquipmentCertificateRenewal.PatternCalibrationRenewalId;
            renewal.QualityFrontId = mod.EquipmentCertificateRenewal.QualityFrontId.Value;
            renewal.InspectionType = mod.EquipmentCertificateRenewal.InspectionType;
            renewal.CalibrationFrecuency = mod.EquipmentCertificateRenewal.CalibrationFrecuency;
            renewal.CalibrationMethod = mod.EquipmentCertificateRenewal.CalibrationMethod;
            if (mod.EquipmentCertificateRenewal.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (renewal.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES}/{renewal.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.QUALITY);
                renewal.FileUrl = await storage.UploadFile(mod.EquipmentCertificateRenewal.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.QUALITY,
                    System.IO.Path.GetExtension(mod.EquipmentCertificateRenewal.File.FileName),
                    ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES,
                    $"eqp_{data.Id}-ctf_{renewal.RenewalOrder}");
            }
            
            var responsibles = await _context.EquipmentCertificateRenewalApplicationUsers
                .Where(x => x.EquipmentCertificateRenewalId == renewal.Id)
                .ToListAsync();

            _context.EquipmentCertificateRenewalApplicationUsers.RemoveRange(responsibles);

            await _context.EquipmentCertificateRenewalApplicationUsers
                .AddRangeAsync(mod.EquipmentCertificateRenewal.Responsibles
                    .Select(x => new EquipmentCertificateRenewalApplicationUser
                    {
                        EquipmentCertificateRenewal = renewal,
                        UserId = x
                    }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.EquipmentCertificates.FirstOrDefaultAsync(x => x.Id == id);

            var renewals = await _context.EquipmentCertificateRenewals
                .Where(x => x.EquipmentCertificateId == id)
                .ToListAsync();

            var responsibles = await _context.EquipmentCertificateRenewalApplicationUsers
                .Include(x => x.EquipmentCertificateRenewal)
                .Where(x => x.EquipmentCertificateRenewal.EquipmentCertificateId == data.Id)
                .ToListAsync();

            _context.EquipmentCertificateRenewalApplicationUsers.RemoveRange(responsibles);
            foreach (var renewal in renewals )
            {
                if(renewal.FileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EQUIPMENT_CERTIFICATES}/{renewal.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.QUALITY);
                }

            }

            _context.EquipmentCertificateRenewals.RemoveRange(renewals);
            _context.EquipmentCertificates.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("reporte-calidad")]
        public async Task<IActionResult> ExcelReportMonth()
        {

            var pid = GetProjectId();
            var data = await _context.Set<UspEquipmentCertificates>().FromSqlRaw("execute Quality_uspEquipmentCertificates")
    .IgnoreQueryFilters()
    .ToListAsync();


            data = data.Where(x => x.ProjectId == pid).ToList();


  
            var project = _context.Projects.Where(x => x.Id == pid).FirstOrDefault();


            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add("For-35");

                var enlace = project.LogoUrl.ToString();

                WebClient client = new WebClient();
                Stream img = client.OpenRead(enlace);
                Bitmap bitmap; bitmap = new Bitmap(img);

                Image image = (Image)bitmap;

                ws.Range($"A1:B3").Merge();

                var aux = ws.AddPicture(bitmap);
                aux.MoveTo(10, 10);
                aux.Height = 45;
                aux.Width = 250;

                ws.Cell($"C1").Value = "GESTIÓN DE CONTROL DE CALIDAD";
                ws.Range($"C1:P1").Merge();
                SetRowBorderStyle(ws, 1, "R");
                ws.Cell($"Q1").Value = "Código";
                ws.Cell($"Q2").Value = "Versión";
                ws.Cell($"Q3").Value = "Fecha";
                if(project.CostCenter == "050-2018")
                    ws.Cell($"R1").Value = "JIC/GCC-For-35";
                if (project.CostCenter == "001")
                    ws.Cell($"R1").Value = "CSH/GCC-For-35";

                ws.Cell($"R2").Value = "1";
                ws.Cell($"R3").Value = "28/09/2021";

                ws.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Cell($"C2").Value = "PROGRAMA DE CALIBRACION Y CONTROL DE EQUIPOS DE MEDICION EXTERNA";
                ws.Range($"C2:P3").Merge();
                SetRowBorderStyle(ws, 2, "R");
                SetRowBorderStyle(ws, 3, "R");


                var count = 8;
                //ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"A{count}").Value = "Item";
                ws.Range($"A{count}:A{count+1}").Merge();
                ws.Cell($"B{count}").Value = "NOMBRE DEL EQUIPO Y/O INSTRUMENTO";
                ws.Range($"B{count}:B{count + 1}").Merge();
                ws.Cell($"C{count}").Value = "MARCA";
                ws.Range($"C{count}:C{count + 1}").Merge();
                ws.Cell($"D{count}").Value = "MODELO";
                ws.Range($"D{count}:D{count + 1}").Merge();
                ws.Cell($"E{count}").Value = "SERIE/COD";
                ws.Range($"E{count}:E{count + 1}").Merge();
                ws.Cell($"F{count}").Value = "PROPIETARIO";
                ws.Range($"F{count}:F{count + 1}").Merge();
                ws.Cell($"G{count}").Value = "TIPO DE INSPECCIÓN";
                ws.Range($"G{count}:G{count + 1}").Merge();
                ws.Cell($"H{count}").Value = "METODO DE CALIBRACIÓN/INSPECCIÓN";
                ws.Range($"H{count}:H{count + 1}").Merge();
                ws.Cell($"I{count}").Value = "FRECUENCIA DE CALIBRACIÓN";
                ws.Range($"I{count}:I{count + 1}").Merge();

                ws.Cell($"J{count}").Value = "CALIBRACIÓN VIGENTE";
                ws.Range($"J{count}:L{count}").Merge();
                ws.Cell($"M{count}").Value = "CONTROL";
                ws.Range($"M{count}:N{count}").Merge();
                ws.Cell($"J{count+1}").Value = "N° DE CERTIFICADO";
                ws.Cell($"K{count+1}").Value = "ENTIDAD QUE REALIZA CALIBRACION";
                ws.Cell($"L{count+1}").Value = "FECHA CALIBRACIÓN";
                ws.Cell($"M{count+1}").Value = "VIGENCIA";

                ws.Cell($"N{count+1}").Value = "ESTADO DE VIGENCIA";
                ws.Cell($"O{count}").Value = "ESTADO";
                ws.Range($"O{count}:O{count + 1}").Merge();
                ws.Cell($"P{count}").Value = "SITUACION";
                ws.Range($"P{count}:P{count + 1}").Merge();
                ws.Cell($"Q{count}").Value = "FECHA DE INGRESO";
                ws.Range($"Q{count}:Q{count + 1}").Merge();
                ws.Cell($"R{count}").Value = "OBSERVACIONES";
                ws.Range($"R{count}:R{count + 1}").Merge();
                SetRowBorderStyle2(ws, count, "R");
                SetRowBorderStyle2(ws, count+1, "R");
                ws.Row(count).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(count+1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Range($"A{count}:R{count}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));
                ws.Range($"A{count+1}:R{count+1}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));


                count = 10;
                //ws.Column(12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(12).Style.NumberFormat.Format = "d-mm-yy";
                //ws.Column(13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(13).Style.NumberFormat.Format = "d-mm-yy";
                //ws.Column(17).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(17).Style.NumberFormat.Format = "d-mm-yy";
                foreach (var first in data)
                {
                    ws.Cell($"A{count}").Value = first.Correlative;
                    ws.Cell($"B{count}").Value = first.Name;
                    ws.Cell($"C{count}").Value = first.Brand;
                    ws.Cell($"D{count}").Value = first.Model;
                    ws.Cell($"E{count}").Value = first.Serial;

                    ws.Cell($"F{count}").Value = first.EquipmentOwnerName;
                    ws.Cell($"G{count}").Value = first.InspectionTypeDesc;
                    ws.Cell($"H{count}").Value = first.CalibrationMethodDesc;
                    ws.Cell($"I{count}").Value = first.CalibrationFrecuencyDesc;

   
                    ws.Cell($"J{count}").Value = first.EquipmentCertificateNumber;
                    ws.Cell($"K{count}").Value = first.EquipmentCertifyingEntityName;
                    ws.Cell($"L{count}").Value = first.StartDate.Value;
                    ws.Cell($"M{count}").Value = first.EndDate.Value;


                    if (first.Validity > 30)
                        ws.Cell($"N{count}").Value = "Vigente";
                    if (first.Validity > 15 && first.Validity <= 30)
                        ws.Cell($"N{count}").Value = "Renovación";
                    if (first.Validity > 0 && first.Validity <= 15)
                        ws.Cell($"N{count}").Value = "Tramitar";
                    if (first.Validity <= 0)
                        ws.Cell($"N{count}").Value = "Vencido";
                    ws.Cell($"O{count}").Value = first.OperationStatusDesc;
                    ws.Cell($"P{count}").Value = first.SituationStatusDesc;
                    if (first.EntryDate.HasValue)
                    {

                        ws.Cell($"Q{count}").Value = first.EntryDate.Value;
                    }
                    else
                    {
                        ws.Cell($"Q{count}").Value = string.Empty;
                    }
                    ws.Cell($"R{count}").Value = first.Observation;
                    count++;
                    SetRowBorderStyle2(ws, count - 1, "R");

                }
               

                ws.Column(2).Width = 40;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 15;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                ws.Column(10).Width = 20;
                ws.Column(11).Width = 33;
                ws.Column(12).Width = 20;
                ws.Column(13).Width =20;
                ws.Column(14).Width =20;
                ws.Column(15).Width =20;
                ws.Column(16).Width =20;
                ws.Column(17).Width =20;
                ws.Column(18).Width =20;






                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "For 35.xlsx");
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
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }
    }
}
