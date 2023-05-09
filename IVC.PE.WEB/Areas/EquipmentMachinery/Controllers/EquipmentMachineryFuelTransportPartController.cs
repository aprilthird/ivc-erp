using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelTransportPartViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-combustible-transporte")]

    public class EquipmentMachineryFuelTransportPartController : BaseController
    {

        public EquipmentMachineryFuelTransportPartController(IvcDbContext context,
        ILogger<EquipmentMachineryFuelTransportPartController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentMachineryTransportPartId = null, Guid? transportId = null,Guid? equipmentProviderId = null, int? month = null, int? year = null, int? week = null)
        {
            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@Year", System.Data.SqlDbType.Int);
            param1.Value = (object)year ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Month", System.Data.SqlDbType.Int);
            param2.Value = (object)month ?? DBNull.Value;
            SqlParameter param3 = new SqlParameter("@Week", System.Data.SqlDbType.Int);
            param3.Value = (object)week ?? DBNull.Value;

            var data = await _context.Set<UspFuelTransport>().FromSqlRaw("execute EquipmentMachinery_uspFuelTransport @Month, @Year ,@Week"
                , param2
                , param1
                , param3)
         .IgnoreQueryFilters()
         .ToListAsync();

            data = data.OrderBy(x => x.TradeName).ToList();

            data = data.Where(x => x.ProjectId == pId).ToList();

            if (equipmentMachineryTransportPartId.HasValue)
                data = data.Where(x => x.EquipmentMachineryTransportPartId == equipmentMachineryTransportPartId.Value).ToList();
            
            if(transportId.HasValue)
                data = data.Where(x => x.EquipmentMachineryTypeTransportId == transportId.Value).ToList();

            if (equipmentProviderId.HasValue)
                data = data.Where(x => x.EquipmentProviderId == equipmentProviderId.Value).ToList();

            if (month.HasValue)
            {
                data = data.Where(x => x.FoldingId != null).ToList();
            }

            if (year.HasValue)
            {
                data = data.Where(x => x.FoldingId != null).ToList();
            }

            if (week.HasValue)
            {
                data = data.Where(x => x.FoldingId != null).ToList();
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachineryFuelTransportParts
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryFuelTransportPartViewModel
                {
                    Id = x.Id,
                    //PartNumber = x.PartNumber,
                    //PartDate = x.PartDate.Date.ToDateString(),
                    
                    //UserId = x.UserId,
                    //UserName = x.UserName,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentMachineryTransportPartId = x.EquipmentMachineryTransportPartId,
                    AcumulatedGallon = x.AcumulatedGallon
                    //EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,

                }).FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryFuelTransportPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = new EquipmentMachineryFuelTransportPart
            {
                

                EquipmentProviderId = model.EquipmentProviderId,
                EquipmentMachineryTransportPartId = model.EquipmentMachineryTransportPartId,
                //EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                ProjectId = GetProjectId(),
                //UserId = model.UserId,
                //UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,

            };

            await _context.EquipmentMachineryFuelTransportParts.AddAsync(softpart);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryFuelTransportPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = await _context.EquipmentMachineryFuelTransportParts
                .FirstOrDefaultAsync(x => x.Id == id);
            
            softpart.EquipmentProviderId = model.EquipmentProviderId;
            softpart.EquipmentMachineryTransportPartId = model.EquipmentMachineryTransportPartId;
            //softpart.UserId = model.UserId;
            //softpart.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.EquipmentMachineryFuelTransportParts
                .FirstOrDefaultAsync(x => x.Id == id);


            _context.EquipmentMachineryFuelTransportParts.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
