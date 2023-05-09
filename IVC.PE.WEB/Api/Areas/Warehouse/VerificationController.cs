using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.WareHouse;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.Warehouse
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/almacenes/verificacion")]
    public class VerificationController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public VerificationController(IvcDbContext context,
            ILogger<VerificationController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string equipmentQr)
        {
            SqlParameter param1 = new SqlParameter("@Serial", System.Data.SqlDbType.NVarChar);
            param1.Value = (object)equipmentQr ?? DBNull.Value;

     


            var data = await _context.Set<UspEquipmentVerification>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentMachVerification @Serial"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            var data2 = await _context.Set<UspEquipmentVerification>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentMachineryTransportVerification @Serial"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            var data3 = await _context.Set<UspEquipmentVerification>().FromSqlRaw("execute EquipmentMachinery_uspEquipmentMachinerySoftVerification @Serial"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            var datamixed = data.Union(data2.Union(data3));

            var bps = datamixed
                    .Select(x => new EquipmentMachineryVerificationResourceModel
                    {
                        Year = "Año: "+x.Year,

                        Model = "Modelo: "+x.Model,

                        Provider = "Proveedor: "+x.Provider,

                        Equipment = "Equipo: "+x.Equipment

                    }).ToList();




                return Ok(bps);
            
        }

        [HttpGet("listar-items")]
        public async Task<IActionResult> GetAllItems(string fullcodeQr, string remissionQr)
        {

         
            SqlParameter param1 = new SqlParameter("@remissionQr", System.Data.SqlDbType.NVarChar);
            param1.Value = (object)remissionQr ?? DBNull.Value;

            SqlParameter param2 = new SqlParameter("@fullcodeQr", System.Data.SqlDbType.NVarChar);
            param2.Value = (object)fullcodeQr ?? DBNull.Value;

            var data = await _context.Set<UspSupplyVerification>().FromSqlRaw("execute WareHouse_uspSupplyEntry @remissionQr,@fullcodeQr"
    , param1
    ,param2)
.IgnoreQueryFilters()
.ToListAsync();


            var supplyEntries = await _context.SupplyEntryItems
    .Include(x => x.OrderItem)
    .Include(x => x.SupplyEntry)
    .Include(x => x.SupplyEntry.Order.Provider)
    .Include(x => x.SupplyEntry.Warehouse.WarehouseType)
    .Where(x => x.SupplyEntry.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED)
    .ToListAsync();

            var fieldRequests = await _context.FieldRequestFoldings
      .Include(x => x.FieldRequest)
      .Include(x => x.FieldRequest.BudgetTitle)
      .Where(x => x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
      .ToListAsync();

            var entries = supplyEntries
              .Where(x => x.OrderItem.SupplyId == data.Select(x=>x.SupplyId).FirstOrDefault()
              )
              .ToList();

            var ingresos = entries.Sum(x => x.Measure);

            var valorizadas = entries.Where(x => x.SupplyEntry.IsValued == true)
    .Sum(x => x.Measure);

            var today = DateTime.Today;

            var requests = fieldRequests
    .Where(x => x.GoalBudgetInput.SupplyId == data.Select(x => x.SupplyId).FirstOrDefault()
    && x.FieldRequest.DeliveryDate.Year == today.Year
    && x.FieldRequest.DeliveryDate.Month < today.Month)
    .ToList();

            var salidas = requests.Sum(x => x.DeliveredQuantity);

            var contable = Math.Round(ingresos - valorizadas - salidas, 2);

            var rm = data.Select( x=> new VerificationResourceModel
            {
                Description = "Insumo: "+x.Description,
                CorrelativeCodeStr = "O/C: "+x.CorrelativeCodeStr,
                IvcCode = "Código: "+x.IvcCode,
                Tradename = "Proveedor: "+x.Tradename,
                DeliveryDateStr = "Llegada: "+x.DeliveryDate.Date.ToDateString(),
                RemissionGuide = "Guia de Remisión: "+x.RemissionGuide,
                Measure = "Stock Físico: "+x.Measure.ToString("N2", CultureInfo.InvariantCulture),
                Sums = "Stock Contable: "+contable.ToString("N2", CultureInfo.InvariantCulture)
            }
                ).ToList();

            return Ok(rm);
        }

      



    }
}
