using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/proveedores-de-equipos")]
    public class EquipmentProviderController : BaseController
    {

        public EquipmentProviderController(IvcDbContext context,
        ILogger<EquipmentProviderController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? classEquip = null, Guid? projectId = null, Guid? equipmentProviderId = null, Guid? equipmentType = null, Guid? typeid = null, Guid? softId = null, Guid? transportId = null)
        {

            var pId = GetProjectId();

            var softs = _context.EquipmentMachineryTypeSofts
    .AsQueryable();

            var types = _context.EquipmentMachineryTypeTypes
                .AsQueryable();

            var transports = _context.EquipmentMachineryTypeTransports
                                .AsQueryable();
            if (equipmentType.HasValue)
            {

                if (types.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    typeid = equipmentType.Value;


                }
                else if (softs.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    softId = equipmentType.Value;



                }
                else if (transports.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    transportId = equipmentType.Value;


                }

            }

            SqlParameter param1 = new SqlParameter("@EqpTypeId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)classEquip ?? DBNull.Value;

            SqlParameter param2 = new SqlParameter("@EqpTypeTypeId", System.Data.SqlDbType.UniqueIdentifier);
            param2.Value = (object)typeid ?? DBNull.Value;

            SqlParameter param3 = new SqlParameter("@EqpTypeSoftId", System.Data.SqlDbType.UniqueIdentifier);
            param3.Value = (object)softId ?? DBNull.Value;

            SqlParameter param4 = new SqlParameter("@EqpTypeTransportId", System.Data.SqlDbType.UniqueIdentifier);
            param4.Value = (object)transportId ?? DBNull.Value;


            var data = await _context.Set<UspEquipmentProvider>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentProvider @EqpTypeId, @EqpTypeTypeId, @EqpTypeSoftId, @EqpTypeTransportId"
                , param1
                , param2
                , param3
                , param4)
.IgnoreQueryFilters()
.ToListAsync();


            if (equipmentProviderId.HasValue)
                data = data.Where(x => x.Id == equipmentProviderId.Value).ToList();

            if (classEquip.HasValue)
                data = data.Where(x => x.EquipmentMachineryTypeId == classEquip.Value && x.FoldingId != null).ToList();

            if (equipmentType.HasValue)
            {

                if (types.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                   
                    data = data.Where(x => x.FoldingId != null).ToList();

                }
                else if (softs.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    
                    data = data.Where(x => x.FoldingId != null).ToList();

                
                }
                else if (transports.Where(x => x.Id == equipmentType.Value).Count() > 0)
                {
                    
                    data = data.Where(x => x.FoldingId != null).ToList();

                }

            }


            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = _context.EquipmentProviders
               .AsQueryable();

            var data = await query
                .Include(x => x.Project)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentProviderViewModel
                {
                    Id = x.Id,
                    ProviderId = x.ProviderId,

                })
                .FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentProviderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipTypeSoft = await _context.EquipmentMachineryTypes
    .FirstOrDefaultAsync(x => x.Description == "Equipos Menores");

            var equipTypeType = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Maquinaria");

            var equipment = new EquipmentProvider
            {
                ProjectId = GetProjectId(),

  
                ProviderId = model.ProviderId,



            };

            await _context.EquipmentProviders.AddAsync(equipment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentProviderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipTypeSoft = await _context.EquipmentMachineryTypes
.FirstOrDefaultAsync(x => x.Description == "Equipos Menores");

            var equipTypeType = await _context.EquipmentMachineryTypes
                .FirstOrDefaultAsync(x => x.Description == "Maquinaria");

            var equipment = await _context.EquipmentProviders
                .FirstOrDefaultAsync(x => x.Id == id);


            equipment.ProviderId = model.ProviderId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.EquipmentProviders.FirstOrDefaultAsync(x => x.Id == id);

            _context.EquipmentProviders.Remove(data);
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
                    var foldings = new List<EquipmentProviderFolding>();
                    var eqp = new EquipmentProvider();
                   

                    var eqpsoft = await _context.EquipmentMachineryTypeSofts
            .ToListAsync();
                    var eqpmach = await _context.EquipmentMachineryTypeTypes
            .ToListAsync();
                    var eqptransport = await _context.EquipmentMachineryTypeTransports
                        .ToListAsync();

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {

                        //---------------Creación del For05
                        var eqpselected = await _context.EquipmentProviders
                            .FirstOrDefaultAsync(x => x.Provider.Tradename == workSheet.Cell($"b{counter}").GetString() && x.ProjectId == pId );

                        if (eqpselected != null)
                        {

                            //eqp.Id = Guid.NewGuid();
                            //eqp.ProjectId = pId;

                            //await _context.EquipmentProviders.AddAsync(eqp);
                            //await _context.SaveChangesAsync();
                            //eqpselected = await _context.EquipmentProviders
                            //    .FirsOrDefaultAsync(x => x.Provider.Tradename == workSheet.Cell($"B{counter}").GetString());
                            var data = workSheet.Cell($"C{counter}").GetString();
                            var data2 = workSheet.Cell($"D{counter}").GetString();
                            var data3 = workSheet.Cell($"D{counter}").GetString();
                            var folding = new EquipmentProviderFolding();
                            if (!string.IsNullOrEmpty(data))
                            {
                                var eqqqqid = eqpsoft.Where(x => x.Description.Normalize().ToLower() == data.Normalize().ToLower())
                                    .Select(x => x.Id).FirstOrDefault();
                                folding.EquipmentMachineryTypeSoftId = eqqqqid;
                                folding.EquipmentMachineryTypeId = Guid.Parse("46C03667-56E8-4595-025D-08D8CDCCB0A5");
                            }
                            else if (!string.IsNullOrEmpty(data2))
                            {


                                var eqqqqqqid = eqpmach.Where(x => x.Description.Normalize().ToLower() == data2.Normalize().ToLower())
                                    .Select(x => x.Id).FirstOrDefault();
                                folding.EquipmentMachineryTypeTypeId = eqqqqqqid;
                                folding.EquipmentMachineryTypeId = Guid.Parse("32D8B481-E530-48F6-025E-08D8CDCCB0A5");
                            }
                            else if (!string.IsNullOrEmpty(data3))
                            {


                                var eqqqqqqqqid = eqptransport.Where(x => x.Description.Normalize().ToLower() == data3.Normalize().ToLower())
                                    .Select(x => x.Id).FirstOrDefault();
                                folding.EquipmentMachineryTypeTransportId = eqqqqqqqqid;
                                folding.EquipmentMachineryTypeId = Guid.Parse("9DF57BF9-7A23-4051-BD6B-08D8E1B17437");
                            }

                            folding.EquipmentProviderId = eqpselected.Id;
                            foldings.Add(folding);

                            ++counter;
                        }

                        else 
                        {
                            var neweqpselected = await _context.Providers
                           .FirstOrDefaultAsync(x => x.Tradename == workSheet.Cell($"b{counter}").GetString());
                            eqp.Id = Guid.NewGuid();
                            eqp.ProjectId = pId;
                            eqp.ProviderId = neweqpselected.Id;


                            await _context.EquipmentProviders.AddAsync(eqp);
                            await _context.SaveChangesAsync();

                            //eqpselected = await _context.EquipmentProviders
                            //    .FirsOrDefaultAsync(x => x.Provider.Tradename == workSheet.Cell($"B{counter}").GetString());
                            var data = workSheet.Cell($"C{counter}").GetString();
                            var data2 = workSheet.Cell($"D{counter}").GetString();
                            var data3 = workSheet.Cell($"D{counter}").GetString();
                            var folding = new EquipmentProviderFolding();
                            if (!string.IsNullOrEmpty(data))
                            {
                                var eqqqqid = eqpsoft.Where(x => x.Description.Normalize().ToLower() == data.Normalize().ToLower())
                                    .Select(x => x.Id).FirstOrDefault();
                                folding.EquipmentMachineryTypeSoftId = eqqqqid;
                                folding.EquipmentMachineryTypeId = Guid.Parse("46C03667-56E8-4595-025D-08D8CDCCB0A5");
                            }
                            else if (!string.IsNullOrEmpty(data2))
                            {

                                
                                var eqqqqqqid = eqpmach.Where(x => x.Description.Normalize().ToLower() == data2.Normalize().ToLower())
                                    .Select(x => x.Id).FirstOrDefault();
                                folding.EquipmentMachineryTypeTypeId = eqqqqqqid;
                                folding.EquipmentMachineryTypeId = Guid.Parse("32D8B481-E530-48F6-025E-08D8CDCCB0A5");
                            }
                            else if (!string.IsNullOrEmpty(data3))
                            {


                                var eqqqqqqqqid = eqptransport.Where(x => x.Description.Normalize().ToLower() == data3.Normalize().ToLower())
                                    .Select(x => x.Id).FirstOrDefault();
                                folding.EquipmentMachineryTypeTransportId = eqqqqqqqqid;
                                folding.EquipmentMachineryTypeId = Guid.Parse("9DF57BF9-7A23-4051-BD6B-08D8E1B17437");
                            }



                            folding.EquipmentProviderId = eqp.Id;
                            foldings.Add(folding);

                            ++counter;
                        }

                        //---------------Creación del Folding For05

                        //var folding = new EquipmentProviderFolding();
                        //if (!workSheet.Cell($"C{counter}").IsEmpty())
                        //folding.EquipmentMachineryTypeSoftId = eqpsoft.Id;
                        //else
                        //folding.EquipmentMachineryTypeTypeId = eqpmach.Id;

                        //if (!workSheet.Cell($"C{counter}").IsEmpty())
                        //    folding.EquipmentMachineryTypeId = eqtypesoft.Id;
                        //else
                        //    folding.EquipmentMachineryTypeId = eqtypetype.Id;
                        ////folding.Id = Guid.NewGuid();


                    }
                    await _context.EquipmentProviderFoldings.AddRangeAsync(foldings);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "ProveedoresEquipos.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "Nombre Comercial Proveedor";
                workSheet.Range("B2:B3").Merge();
                workSheet.Range("B2:B3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C2").Value = "Equipos Menores que posee";
                workSheet.Range("C2:C3").Merge();
                workSheet.Range("C2:C3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D2").Value = "Equipos Maquinaria que posee";
                workSheet.Range("D2:D3").Merge();
                workSheet.Range("D2:D3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E2").Value = "Equipos Transporte que posee";
                workSheet.Range("E2:E3").Merge();
                workSheet.Range("E2:E3").Style.Fill.SetBackgroundColor(XLColor.Yellow);


                workSheet.Column(2).Width = 27;
                workSheet.Column(3).Width = 27;
                workSheet.Column(4).Width = 29;
                workSheet.Rows().AdjustToContents();
                workSheet.Range("B2:D3").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:D3").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
