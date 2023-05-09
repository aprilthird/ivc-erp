    using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QRCoder;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{

        [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
        [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
        [Route("equipos/equipo-transporte")]
        public class EquipmentMachineryTransportController : BaseController
        {

            private readonly IOptions<CloudStorageCredentials> _storageCredentials;

            public EquipmentMachineryTransportController(IvcDbContext context,
            ILogger<EquipmentMachineryTransportController> logger,
            IOptions<CloudStorageCredentials> storageCredentials) : base(context, logger)
            {
                _storageCredentials = storageCredentials;
            }

            public IActionResult Index() => View();

            [HttpGet("listar")]
            public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? equipmentProviderId = null, Guid? equipmentMachineryTypeTransportId = null)
            {

                var pId = GetProjectId();

            var data = await _context.Set<UspEquipmentMachineryTransport>().FromSqlRaw("execute EquipmentMachinery_uspTransport")
             .IgnoreQueryFilters()
             .ToListAsync();

            data = data.Where(x => x.ProjectId == pId).OrderBy(x=>x.TradeName).ToList();

            if (equipmentProviderId.HasValue)
                    data = data.Where(x => x.EquipmentProviderId == equipmentProviderId.Value).OrderBy(x => x.TradeName).ToList();

            if (equipmentMachineryTypeTransportId.HasValue)
                    data = data.Where(x => x.EquipmentMachineryTypeTransportId == equipmentMachineryTypeTransportId.Value).OrderBy(x => x.TradeName).ToList();

                return Ok(data);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> Get(Guid id)
            {
                var query = _context.EquipmentMachineryTransports
                   .AsQueryable();

                var data = await query
                    .Include(x => x.Project)
                    .Include(x => x.EquipmentProvider.Provider)
                    .Where(x => x.Id == id)
                    .Select(x => new EquipmentMachineryTransportViewModel
                    {
                        Id = x.Id,

                        EquipmentProviderId = x.EquipmentProviderId,
                        EquipmentProviderFoldingId = x.EquipmentProviderFoldingId,

                        //UserId = x.UserId,
                        //UserName = x.UserName,
                        //WorkArea = x.WorkArea,

                        EquipmentYear = x.EquipmentYear,
                        EquipmentPlate = x.EquipmentPlate,
                        EquipmentSerie = x.EquipmentSerie,
                        Model = x.Model,
                        Brand = x.Brand,
                        StartDate = x.StartDate.HasValue
                        ? x.StartDate.Value.Date.ToDateString() : String.Empty,


                        Status = x.Status,
                        ServiceCondition = x.ServiceCondition,
                        UnitPrice = x.UnitPrice,
                        
                    })
                    .FirstOrDefaultAsync();
 
                return Ok(data);
            }

            [HttpPost("crear")]
            public async Task<IActionResult> Create(EquipmentMachineryTransportViewModel mod)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var soft = new EquipmentMachineryTransport
                {
                    ProjectId = GetProjectId(),

                    EquipmentProviderId = mod.EquipmentProviderId,
                    EquipmentProviderFoldingId = mod.EquipmentProviderFoldingId,

                    Model = mod.Model,
                    Brand = mod.Brand,
                    //UserId = mod.UserId,
                    //UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,
                    //WorkArea = users.WorkArea,



                    EquipmentYear = mod.EquipmentYear,
                    EquipmentPlate = mod.EquipmentPlate,
                    EquipmentSerie = mod.EquipmentSerie,
                    StartDate = string.IsNullOrEmpty(mod.StartDate)
                    ? (DateTime?)null : mod.StartDate.ToDateTime(),


                    Status = mod.Status,
                    ServiceCondition = mod.ServiceCondition,
                    UnitPrice = mod.UnitPrice,




                };


                await _context.EquipmentMachineryTransports.AddAsync(soft);
                await _context.SaveChangesAsync();

                return Ok();
            }

            [HttpPut("editar/{id}")]
            public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTransportViewModel mod)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var soft = await _context.EquipmentMachineryTransports
                    .FirstOrDefaultAsync(x => x.Id == id);

                soft.EquipmentProviderId = mod.EquipmentProviderId;
                soft.EquipmentProviderFoldingId = mod.EquipmentProviderFoldingId;
                soft.Model = mod.Model;
                soft.Brand = mod.Brand;

            //    soft.UserId = mod.UserId;
            //    soft.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;
            //soft.WorkArea = users.WorkArea;

                soft.EquipmentYear = mod.EquipmentYear;
                soft.EquipmentPlate = mod.EquipmentPlate;
                soft.EquipmentSerie = mod.EquipmentSerie;
                soft.StartDate = string.IsNullOrEmpty(mod.StartDate)
    ? (DateTime?)null : mod.StartDate.ToDateTime();

                soft.Status = mod.Status;
                soft.ServiceCondition = mod.ServiceCondition;
                soft.UnitPrice = mod.UnitPrice;
                

                await _context.SaveChangesAsync();

                return Ok();
            }

            [HttpDelete("eliminar/{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var soft = await _context.EquipmentMachineryTransports.FirstOrDefaultAsync(x => x.Id == id);

                _context.EquipmentMachineryTransports.Remove(soft);
                await _context.SaveChangesAsync();

                return Ok();
            }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {


            var pId = GetProjectId();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 4;
                    var project = await _context.Projects.FirstOrDefaultAsync();

                    var responsibles = await _context.EquipmentMachineryResponsibles
                        .Where(x => x.ProjectId == GetProjectId())
                        .Select(x => x.UserId)
                        .ToListAsync();

                    var resIds = string.Join(",", responsibles);

                    var eqlist = new List<EquipmentMachineryTransport>();

                    var mtms = new List<EquipmentMachineryTransportInsuranceFolding>();

                    var mmtms = new List<EquipmentMachineryTransportInsuranceFoldingApplicationUser>();
                    var mmmmmtms = new List<EquipmentMachineryTransportSOATFolding>();
                    var mmmmmmtms = new List<EquipmentMachineryTransportTechnicalRevisionFolding>();
                    var mmmtms = new List<EquipmentMachineryTransportSOATFoldingApplicationUser>();
                    var mmmmtms = new List<EquipmentMachineryTransportTechnicalRevisionFoldingApplicationUser>();

                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {
                        var eqmt = new EquipmentMachineryTransport();

                        //---------------Creación del For05
                        var neweqpselected = await _context.EquipmentProviders
                            .FirstOrDefaultAsync(x => x.Provider.Tradename == workSheet.Cell($"C{counter}").GetString());

                        var insuranceselected = await _context.InsuranceEntity
                            .FirstOrDefaultAsync(x => x.Description == workSheet.Cell($"N{counter}").GetString());

                        var newfoldingselected = await _context.EquipmentProviderFoldings
                            .FirstOrDefaultAsync(x => x.EquipmentMachineryTypeTransport.Description == workSheet.Cell($"D{counter}").GetString());
                        eqmt.Id = Guid.NewGuid();
                        eqmt.ProjectId = pId;
                        eqmt.EquipmentProviderId = neweqpselected.Id;
                        eqmt.EquipmentProviderFoldingId = newfoldingselected.Id;
                        eqmt.Brand = workSheet.Cell($"E{counter}").GetString();
                        eqmt.Model = workSheet.Cell($"F{counter}").GetString();
                        eqmt.EquipmentYear = workSheet.Cell($"G{counter}").GetString();
                        eqmt.EquipmentPlate = workSheet.Cell($"H{counter}").GetString();
                        eqmt.EquipmentSerie = workSheet.Cell($"I{counter}").GetString();
                        if (!workSheet.Cell($"J{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"J{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {

                                    var date = workSheet.Cell($"J{counter}").GetDateTime();
                                    var newdate = new DateTime(date.Year, date.Month, date.Day);
                                    eqmt.StartDate = newdate;
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"J{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    eqmt.StartDate = date.ToShortDateString().ToDateTime();
                            }
                        }
                        //eqms.WorkFrontHeadId = newworkselected.Id;
                        var Statusstr = workSheet.Cell($"K{counter}").GetString();
                        if (Statusstr == "Stand By")
                            eqmt.Status = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS_STAND_BY;
                        else if (Statusstr == "En Uso")
                            eqmt.Status = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS_IN_USE;
                        else if (Statusstr == "Inoperativo")
                            eqmt.Status = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS_INOPERATIVE;
                        else
                            eqmt.Status = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS_RETIRED;

                        var Servicestr = workSheet.Cell($"L{counter}").GetString();
                        if (Servicestr == "Seca-S/operador")
                            eqmt.ServiceCondition = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION_S_OPERATOR;
                        else if (Servicestr == "A todo costo")
                            eqmt.ServiceCondition = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION_ALL_COST;
                        else
                            eqmt.ServiceCondition = ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION_C_OPERATOR;

                        var unitpricestring = workSheet.Cell($"M{counter}").GetString();
                        eqmt.UnitPrice = Double.Parse(unitpricestring);


                        {
                            var insurancefolding = new EquipmentMachineryTransportInsuranceFolding();
                            insurancefolding.EquipmentMachineryTransportId = eqmt.Id;

                            insurancefolding.InsuranceEntityId = insuranceselected.Id;


                            insurancefolding.Number = workSheet.Cell($"O{counter}").GetString();

                            if (!workSheet.Cell($"P{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"P{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {
                                        var date = workSheet.Cell($"P{counter}").GetDateTime();
                                        var newdate = new DateTime(date.Year, date.Month, date.Day);
                                        insurancefolding.StartDateInsurance = newdate;
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"P{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        insurancefolding.StartDateInsurance = date.ToShortDateString().ToDateTime(); ;

                                }
                            }

                            if (!workSheet.Cell($"Q{counter}").IsEmpty())
                            {
                                if (workSheet.Cell($"Q{counter}").DataType == XLDataType.DateTime)
                                {
                                    try
                                    {

                                        var date = workSheet.Cell($"Q{counter}").GetDateTime();
                                        var newdate = new DateTime(date.Year, date.Month, date.Day);
                                        insurancefolding.EndDateInsurance = newdate;
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError(e.StackTrace);
                                    }
                                }
                                else
                                {
                                    var dateTimeStr = workSheet.Cell($"Q{counter}").GetString();
                                    if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                        insurancefolding.EndDateInsurance = date.ToShortDateString().ToDateTime();

                                }
                            }

                            insurancefolding.OrderInsurance = 1;

                            var insuranceappusers = new EquipmentMachineryTransportInsuranceFoldingApplicationUser
                            {
                                EquipmentMachineryTransportInsuranceFolding = insurancefolding,
                                UserId = resIds
                            };

                            mtms.Add(insurancefolding);

                            mmtms.Add(insuranceappusers);

                            await _context.EquipmentMachineryTransportInsuranceFoldings.AddRangeAsync(mtms);
                            await _context.EquipmentMachineryTransportInsuranceFoldingApplicationUsers.AddRangeAsync(mmtms);

                        }



                        var soatfolding = new EquipmentMachineryTransportSOATFolding();
                        soatfolding.EquipmentMachineryTransportId = eqmt.Id;

                        if (!workSheet.Cell($"R{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"R{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    var date = workSheet.Cell($"R{counter}").GetDateTime();
                                    var newdate = new DateTime(date.Year, date.Month, date.Day);
                                    soatfolding.StartDateSOAT = newdate;
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"R{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    soatfolding.StartDateSOAT = date.ToShortDateString().ToDateTime();

                            }
                        }

                        if (!workSheet.Cell($"S{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"S{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    var date = workSheet.Cell($"S{counter}").GetDateTime();
                                    var newdate = new DateTime(date.Year, date.Month, date.Day);

                                    soatfolding.EndDateSOAT = newdate;
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"S{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    soatfolding.EndDateSOAT = date.ToShortDateString().ToDateTime();

                            }
                        }



                        var tecfolding = new EquipmentMachineryTransportTechnicalRevisionFolding();
                        tecfolding.EquipmentMachineryTransportId = eqmt.Id;
                        if (!workSheet.Cell($"T{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"T{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    var date = workSheet.Cell($"T{counter}").GetDateTime();
                                    var newdate = new DateTime(date.Year, date.Month, date.Day);
                                    tecfolding.StartDateTechnicalRevision = newdate;

                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"T{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    tecfolding.StartDateTechnicalRevision = date.ToShortDateString().ToDateTime();

                            }
                        }

                        if (!workSheet.Cell($"U{counter}").IsEmpty())
                        {
                            if (workSheet.Cell($"U{counter}").DataType == XLDataType.DateTime)
                            {
                                try
                                {
                                    var date = workSheet.Cell($"U{counter}").GetDateTime();
                                    var newdate = new DateTime(date.Year, date.Month, date.Day);

                                    tecfolding.EndDateTechnicalRevision = newdate;
                                
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError(e.StackTrace);
                                }
                            }
                            else
                            {
                                var dateTimeStr = workSheet.Cell($"U{counter}").GetString();
                                if (!string.IsNullOrEmpty(dateTimeStr) && DateTime.TryParse(dateTimeStr, out DateTime date))
                                    tecfolding.EndDateTechnicalRevision = date.ToShortDateString().ToDateTime();

                            }
                        }



                        eqmt.InsuranceNumber = 1;

                        eqmt.SoatNumber = 1;
                        soatfolding.SoatOrder = 1;
                        eqmt.TechincalNumber = 1;
                        tecfolding.TechnicalOrder = 1;




                        var soatappusers = new EquipmentMachineryTransportSOATFoldingApplicationUser
                        {
                            EquipmentMachineryTransportSOATFolding = soatfolding,
                            UserId = resIds
                        };

                        var tecappusers = new EquipmentMachineryTransportTechnicalRevisionFoldingApplicationUser
                        {
                            EquipmentMachineryTransportTechnicalRevisionFolding = tecfolding,
                            UserId = resIds
                        };

                        eqlist.Add(eqmt);



                        mmmmmtms.Add(soatfolding);
                        mmmmmmtms.Add(tecfolding);

                        mmmtms.Add(soatappusers);
                        mmmmtms.Add(tecappusers);
                        ++counter;


                    }

                    await _context.EquipmentMachineryTransports.AddRangeAsync(eqlist);

                    await _context.EquipmentMachineryTransportSOATFoldings.AddRangeAsync(mmmmmtms);
                    await _context.EquipmentMachineryTransportTechnicalRevisions.AddRangeAsync(mmmmmmtms);

                    await _context.EquipmentMachineryTransportSOATFoldingApplicationUsers.AddRangeAsync(mmmtms);
                    await _context.EquipmentMachineryTransportTechnicalRevisionFoldingApplications.AddRangeAsync(mmmmtms);
                    //await _context.EquipmentMachinerySoftApplicationUsers.AddRangeAsync(mtms);

                    await _context.SaveChangesAsync();

                }
                mem.Close();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "CargaMasivaTransporte.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"C2").Value = "Nombre Comercial Proveedor";
                workSheet.Range("C2:C3").Merge();
                workSheet.Range("C2:C3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D2").Value = "Equipos Transporte";
                workSheet.Range("D2:D3").Merge();
                workSheet.Range("D2:D3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E2").Value = "Marca";
                workSheet.Range("E2:E3").Merge();
                workSheet.Range("E2:E3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"F2").Value = "Modelo";
                workSheet.Range("F2:F3").Merge();
                workSheet.Range("F2:F3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"G2").Value = "Año";
                workSheet.Range("G2:G3").Merge();
                workSheet.Range("G2:G3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H2").Value = "Placa";
                workSheet.Range("H2:H3").Merge();
                workSheet.Range("H2:H3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"I2").Value = "# de Serie";
                workSheet.Range("I2:I3").Merge();
                workSheet.Range("I2:I3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"J2").Value = "Fecha de Inicio";
                workSheet.Range("J2:J3").Merge();
                workSheet.Range("J2:J3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"K2").Value = "Estado";
                workSheet.Range("K2:K3").Merge();
                workSheet.Range("K2:K3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"L2").Value = "Condición de Servicio";
                workSheet.Range("L2:L3").Merge();
                workSheet.Range("L2:L3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"M2").Value = "Precio Unitario";
                workSheet.Range("M2:M3").Merge();
                workSheet.Range("M2:M3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"N2").Value = "Nombre Aseguradora";
                workSheet.Range("N2:N3").Merge();
                workSheet.Range("N2:N3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"O2").Value = "# de Poliza";
                workSheet.Range("O2:O3").Merge();
                workSheet.Range("O2:O3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"P2").Value = "Inicio Poliza de Seguro";
                workSheet.Range("P2:P3").Merge();
                workSheet.Range("P2:P3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"Q2").Value = "Fin Poliza de seguro";
                workSheet.Range("Q2:Q3").Merge();
                workSheet.Range("Q2:Q3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"R2").Value = "Inicio Soat";
                workSheet.Range("R2:R3").Merge();
                workSheet.Range("R2:R3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"S2").Value = "Fin Soat";
                workSheet.Range("S2:S3").Merge();
                workSheet.Range("S2:S3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"T2").Value = "Inicio Revisión Técnica";
                workSheet.Range("T2:T3").Merge();
                workSheet.Range("T2:T3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"U2").Value = "Fin Revisión Técnica";
                workSheet.Range("U2:U3").Merge();
                workSheet.Range("U2:U3").Style.Fill.SetBackgroundColor(XLColor.Yellow);




                workSheet.Column(3).Width = 27;
                workSheet.Column(4).Width = 25;
                workSheet.Column(5).Width = 25;
                workSheet.Column(6).Width = 10;
                workSheet.Column(7).Width = 12;
                workSheet.Column(8).Width = 12;
                workSheet.Column(9).Width = 17;
                workSheet.Column(10).Width = 25;
                workSheet.Column(11).Width = 25;
                workSheet.Column(12).Width = 25;
                workSheet.Column(13).Width = 12;
                workSheet.Column(14).Width = 20;
                workSheet.Column(15).Width = 15;
                workSheet.Column(16).Width = 25;
                workSheet.Column(17).Width = 25;
                workSheet.Column(18).Width = 20;
                workSheet.Column(19).Width = 20;
                workSheet.Column(20).Width = 20;
                workSheet.Column(21).Width = 20;



                workSheet.Rows().AdjustToContents();
                workSheet.Range("C2:U3").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("C2:U3").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("reporte-equipos")]
        public async Task<IActionResult> ExcelReport(Guid? versionId = null)
        {

            var pId = GetProjectId();




            var data = await _context.Set<UspEquipmentMachineryTransport>().FromSqlRaw("execute EquipmentMachinery_uspTransport")
                        .IgnoreQueryFilters()
                        .ToListAsync();

            data = data.Where(x => x.ProjectId == pId).ToList();



            var project = _context.Projects.Where(x => x.Id == pId).FirstOrDefault();


            using (XLWorkbook wb = new XLWorkbook())
            {

                var ws = wb.Worksheets.Add("Equipos Transporte");

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

                ws.Cell($"C1").Value = "GESTION DE EQUIPOS DE OBRAS    ";
                ws.Range($"C1:W1").Merge();
                SetRowBorderStyle(ws, 1, "Y");
                ws.Cell($"X1").Value = "Código";
                ws.Cell($"X2").Value = "Versión";
                ws.Cell($"X3").Value = "Fecha";

                if (project.CostCenter == "050-2018")
                    ws.Cell($"Y1").Value = "JIC/GEQ-For-10";
                if (project.CostCenter == "001")
                    ws.Cell($"Y1").Value = "CSH/GEQ-For-10";


                ws.Cell($"Y2").Value = "1";
                ws.Cell($"Y3").Value = "28/09/2021";

                ws.Cell($"C2").Value = "INVENTARIO DE EQUIPO/MAQUINARIA EN OBRA";
                ws.Range($"C2:W3").Merge();

                ws.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(2).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Row(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                SetRowBorderStyle(ws, 2, "Y");
                SetRowBorderStyle(ws, 3, "Y");


                var count = 8;
                //ws.Cell($"A{count}").Value = "Proveedor";
                ws.Cell($"A{count}").Value = "Proveedor";
                ws.Range($"A{count}:A{count + 1}").Merge();

                ws.Cell($"B{count}").Value = "Equipo Transporte";
                ws.Range($"B{count}:B{count + 1}").Merge();

                ws.Cell($"C{count}").Value = "Marca";
                ws.Range($"C{count}:C{count + 1}").Merge();

                ws.Cell($"D{count}").Value = "Modelo";
                ws.Range($"D{count}:D{count + 1}").Merge();

                ws.Cell($"E{count}").Value = "Año";
                ws.Range($"E{count}:E{count + 1}").Merge();

                ws.Cell($"F{count}").Value = "Placa";
                ws.Range($"F{count}:F{count + 1}").Merge();

                ws.Cell($"G{count}").Value = "#Serie";
                ws.Range($"G{count}:G{count + 1}").Merge();

                ws.Cell($"H{count}").Value = "Fecha de Inicio";
                ws.Range($"H{count}:H{count + 1}").Merge();


                ws.Cell($"I{count}").Value = "Estado";
                ws.Range($"I{count}:I{count + 1}").Merge();

                ws.Cell($"J{count}").Value = "Kilometraje Inicial";
                ws.Range($"J{count}:J{count + 1}").Merge();

                ws.Cell($"K{count}").Value = "Kilometraje Final";
                ws.Range($"K{count}:K{count + 1}").Merge();

                ws.Cell($"L{count}").Value = "Diferencia";
                ws.Range($"L{count}:L{count + 1}").Merge();

                ws.Cell($"M{count}").Value = "Condición de Servicio";
                ws.Range($"M{count}:M{count + 1}").Merge();

                ws.Cell($"N{count}").Value = "Precio Unitario";
                ws.Range($"N{count}:N{count + 1}").Merge();

                ws.Cell($"O{count}").Value = "Poliza de Seguro";
                ws.Range($"O{count}:S{count}").Merge();

                ws.Cell($"O{count + 1}").Value = "Aseguradora";
                ws.Cell($"P{count + 1}").Value = "N° de Poliza";
                ws.Cell($"Q{count + 1}").Value = "Inicio Poliza";
                ws.Cell($"R{count + 1}").Value = "Fin Poliza";
                ws.Cell($"S{count + 1}").Value = "Vigencia Poliza";

                ws.Cell($"T{count}").Value = "Seguro SOAT";
                ws.Range($"T{count}:V{count}").Merge();

                ws.Cell($"T{count + 1}").Value = "Inicio SOAT";
                ws.Cell($"U{count + 1}").Value = "Fin SOAT";
                ws.Cell($"V{count + 1}").Value = "Vigencia SOAT";

                ws.Cell($"W{count}").Value = "Revisión Técnica";
                ws.Range($"W{count}:Y{count}").Merge();

                ws.Cell($"W{count + 1}").Value = "Inicio R.T.";
                ws.Cell($"X{count + 1}").Value = "Fin R.T.";
                ws.Cell($"Y{count + 1}").Value = "Vigencia R.T.";


                SetRowBorderStyle2(ws, count, "Y");
                SetRowBorderStyle2(ws, count + 1, "Y");
                ws.Row(count).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Row(count + 1).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                ws.Range($"A{count}:Y{count}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));
                ws.Range($"A{count + 1}:Y{count + 1}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(211, 211, 211));


                count = 10;
                //ws.Column(8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                //ws.Column(8).Style.NumberFormat.Format = "d-mm-yy";

                foreach (var first in data)
                {
                    ws.Cell($"A{count}").Value = first.TradeName;
                    ws.Cell($"B{count}").Value = first.Description;
                    ws.Cell($"C{count}").Value = first.Brand;
                    ws.Cell($"D{count}").Value = first.Model;
                    ws.Cell($"E{count}").Value = first.EquipmentYear;

                    ws.Cell($"F{count}").Value = first.EquipmentPlate;
                    ws.Cell($"G{count}").Value = first.EquipmentSerie;

                    if (first.StartDate.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"H{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"H{count}").Value = string.Empty;
                    }




                    ws.Cell($"I{count}").Value = first.StatusDesc;
                    ws.Cell($"J{count}").Value = first.InitMileage;
                    ws.Cell($"K{count}").Value = first.EndMileage;
                    ws.Cell($"L{count}").Value = first.Dif;
                    ws.Cell($"M{count}").Value = first.ServiceConditionDesc;
                    ws.Cell($"N{count}").Value = first.UnitPrice;
                    ws.Cell($"O{count}").Value = first.LastInsuranceNameDesc;
                    ws.Cell($"P{count}").Value = first.LastInsuranceNumber;
                    //
                    if (first.LastStartDateInsurance.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"Q{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"Q{count}").Value = string.Empty;
                    }

                    if (first.LastEndDateInsurance.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"R{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"R{count}").Value = string.Empty;
                    }

                    //
                    ws.Cell($"S{count}").Value = first.LastValidityInsurance;
                    //
                    if (first.LastStartDateSoat.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"T{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"T{count}").Value = string.Empty;
                    }

                    if (first.LastEndDateSoat.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"U{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"U{count}").Value = string.Empty;
                    }

                    //
                    ws.Cell($"V{count}").Value = first.LastValiditySoat;
                    //

                    if (first.LastStartDateTechnical.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"W{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"W{count}").Value = string.Empty;
                    }

                    if (first.LastEndDateTechnical.HasValue)
                    {
                        DateTime dateTime = first.StartDate.Value;
                        ws.Cell($"X{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    }
                    else
                    {
                        ws.Cell($"X{count}").Value = string.Empty;
                    }


                    //

                    ws.Cell($"Y{count}").Value = first.LastValidityTec;

                    //if (first.DateType.HasValue)
                    //{
                    //    DateTime dateTime = first.DateType.Value;
                    //    ws.Cell($"M{count}").Value = dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year;
                    //}
                    //else
                    //{
                    //    ws.Cell($"M{count}").Value = string.Empty;
                    //}
                    //ws.Cell($"N{count}").Value = first.UserName;
                    //ws.Cell($"O{count}").Value = first.Quantity;
                    count++;
                    SetRowBorderStyle2(ws, count - 1, "Y");

                }


                ws.Column(1).Width = 20;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 27;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 12;
                ws.Column(8).Width = 12;
                ws.Column(9).Width = 17;
                ws.Column(10).Width = 25;
                ws.Column(11).Width = 25;
                ws.Column(12).Width = 40;
                ws.Column(13).Width = 25;
                ws.Column(14).Width = 25;
                ws.Column(15).Width = 25;
                ws.Rows().AdjustToContents();






                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de Equipos.xlsx");
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

        [HttpGet("qr/{id}")]
        public async Task<IActionResult> QrGenerator(Guid id)
        {
            var transport = await _context.EquipmentMachineryTransports
                .Include(x => x.EquipmentProvider.Provider)
                .Include(x => x.EquipmentProviderFolding.EquipmentMachineryTypeTransport)
                .FirstOrDefaultAsync(x => x.Id == id);

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(transport.EquipmentSerie, QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);

                PdfDocument doc = new PdfDocument();

                PdfPageBase page = doc.Pages.Add(new SizeF(580, 220), new PdfMargins(0));

                PdfTrueTypeFont font = new PdfTrueTypeFont("Helvetica", 20f, PdfFontStyle.Bold, true);

                //page.Canvas.DrawString(bprint.OrderItem.Supply.Description, new PdfFont(PdfFontFamily.Helvetica, 12f), new PdfSolidBrush(Color.Black), 180, 30);

                //page.Canvas.DrawString(transport.EquipmentProvider.Provider.Tradename, font, new PdfSolidBrush(Color.Black), 220, 29);

                page.Canvas.DrawString(transport.EquipmentProvider.Provider.Tradename, font, new PdfSolidBrush(Color.Black), new RectangleF(220, 23, 350, 55));

                page.Canvas.DrawString(transport.EquipmentProviderFolding.EquipmentMachineryTypeTransport.Description, font,
                    new PdfSolidBrush(Color.Black), 220, 72);

                page.Canvas.DrawString(transport.Brand, font, new PdfSolidBrush(Color.Black), 220, 96);

                page.Canvas.DrawString(transport.Model, font, new PdfSolidBrush(Color.Black), 220, 119);

                page.Canvas.DrawString(transport.EquipmentSerie, font, new PdfSolidBrush(Color.Black), 220, 142);
                
                page.Canvas.DrawString(transport.EquipmentPlate, font, new PdfSolidBrush(Color.Black), 220, 165);

                MemoryStream img = new MemoryStream();


                using (Bitmap bitMap = qrCode.GetGraphic(10))
                {
                    Bitmap resized = new Bitmap(bitMap, new Size(290, 290));

                    resized.Save(img, ImageFormat.Png);

                    PdfImage image = PdfImage.FromStream(img);

                    page.Canvas.DrawImage(image, new PointF(0, 0));
                    doc.SaveToStream(ms);
                    doc.Close();
                    return File(ms.ToArray(), "application/pdf", transport.EquipmentSerie + ".pdf");
                }

            }

        }

    }
}
