using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelMachPartViewModels;
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
    [Route("equipos/parte-combustible-maquinaria")]

    public class EquipmentMachineryFuelMachPartController : BaseController
    {
        public EquipmentMachineryFuelMachPartController(IvcDbContext context,
        ILogger<EquipmentMachineryFuelMachPartController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? equipmentMachineryTransportPartId = null, Guid? transportId = null, Guid? equipmentProviderId = null, int? month = null, int? year = null, int? week = null)
        {
            var pId = GetProjectId();


            SqlParameter param1 = new SqlParameter("@Year", System.Data.SqlDbType.Int);
            param1.Value = (object)year ?? DBNull.Value;
            SqlParameter param2 = new SqlParameter("@Month", System.Data.SqlDbType.Int);
            param2.Value = (object)month ?? DBNull.Value;
            SqlParameter param3 = new SqlParameter("@Week", System.Data.SqlDbType.Int);
            param3.Value = (object)week ?? DBNull.Value;

            var data = await _context.Set<UspFuelMach>().FromSqlRaw("execute EquipmentMachinery_uspFuelMach @Month, @Year ,@Week"
                , param2
                , param1
                , param3)
         .IgnoreQueryFilters()
         .ToListAsync();

            data = data.OrderBy(x => x.TradeName).ToList();

            data = data.Where(x => x.ProjectId == pId).ToList();

            if (equipmentMachineryTransportPartId.HasValue)
                data = data.Where(x => x.EquipmentMachPartId == equipmentMachineryTransportPartId.Value).ToList();

            if (transportId.HasValue)
                data = data.Where(x => x.EquipmentMachineryTypeTypeId == transportId.Value).ToList();

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

            //if (month == 1)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 1).ToList();
            //if (month == 2)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 2).ToList();
            //if (month == 3)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 3).ToList();
            //if (month == 4)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 4).ToList();
            //if (month == 5)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 5).ToList();
            //if (month == 6)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 6).ToList();
            //if (month == 7)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 7).ToList();
            //if (month == 8)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 8).ToList();
            //if (month == 9)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 9).ToList();
            //if (month == 10)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 10).ToList();
            //if (month == 11)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 11).ToList();
            //if (month == 12)
            //    data = data.Where(x => x.FoldingId != null && x.MonthPartDate == 12).ToList();

            //if (year == 2021)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2021).ToList();

            //if (year == 2020)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2020).ToList();

            //if (year == 2019)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2019).ToList();

            //if (year == 2018)
            //    data = data.Where(x => x.FoldingId != null && x.YearPartDate == 2018).ToList();

            //if (week == 1)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 1).ToList();
            //if (week == 2)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 2).ToList();
            //if (week == 3)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 3).ToList();
            //if (week == 4)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 4).ToList();
            //if (week == 5)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 5).ToList();
            //if (week == 6)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 6).ToList();
            //if (week == 7)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 7).ToList();
            //if (week == 8)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 8).ToList();
            //if (week == 9)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 9).ToList();
            //if (week == 10)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 10).ToList();
            //if (week == 11)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 11).ToList();
            //if (week == 12)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 12).ToList();
            //if (week == 13)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 13).ToList();
            //if (week == 14)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 1).ToList();
            //if (week == 1)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 14).ToList();
            //if (week == 15)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 15).ToList();
            //if (week == 16)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 16).ToList();
            //if (week == 17)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 17).ToList();
            //if (week == 18)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 18).ToList();
            //if (week == 19)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 19).ToList();
            //if (week == 20)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 20).ToList();
            //if (week == 21)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 21).ToList();
            //if (week == 22)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 22).ToList();
            //if (week == 23)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 23).ToList();
            //if (week == 24)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 24).ToList();
            //if (week == 25)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 25).ToList();
            //if (week == 26)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 26).ToList();
            //if (week == 27)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 27).ToList();
            //if (week == 28)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 28).ToList();
            //if (week == 29)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 29).ToList();
            //if (week == 30)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 30).ToList();
            //if (week == 31)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 31).ToList();
            //if (week == 32)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 32).ToList();
            //if (week == 33)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 33).ToList();
            //if (week == 34)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 34).ToList();
            //if (week == 35)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 35).ToList();
            //if (week == 36)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 36).ToList();
            //if (week == 37)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 37).ToList();
            //if (week == 38)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 38).ToList();
            //if (week == 39)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 39).ToList();
            //if (week == 40)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 40).ToList();
            //if (week == 41)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 41).ToList();
            //if (week == 42)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 42).ToList();
            //if (week == 43)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 43).ToList();
            //if (week == 44)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 44).ToList();
            //if (week == 45)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 45).ToList();
            //if (week == 46)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 46).ToList();
            //if (week == 47)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 47).ToList();
            //if (week == 48)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 48).ToList();
            //if (week == 49)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 49).ToList();
            //if (week == 50)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 50).ToList();
            //if (week == 51)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 51).ToList();
            //if (week == 52)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 52).ToList();
            //if (week == 53)
            //    data = data.Where(x => x.FoldingId != null && x.WeekPartDate == 53).ToList();



            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachineryFuelMachParts
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryFuelMachPartViewModel
                {
                    Id = x.Id,
                    //PartNumber = x.PartNumber,
                    //PartDate = x.PartDate.Date.ToDateString(),

                    //UserId = x.UserId,
                    //UserName = x.UserName,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentMachPartId = x.EquipmentMachPartId,
                    AcumulatedGallon = x.AcumulatedGallon
                    //EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,

                }).FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryFuelMachPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = new EquipmentMachineryFuelMachPart
            {


                EquipmentProviderId = model.EquipmentProviderId,
                EquipmentMachPartId = model.EquipmentMachPartId,
                ProjectId = GetProjectId(),
                //EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                //UserId = model.UserId,
                //UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,

            };

            await _context.EquipmentMachineryFuelMachParts.AddAsync(softpart);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryFuelMachPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = await _context.EquipmentMachineryFuelMachParts
                .FirstOrDefaultAsync(x => x.Id == id);

            softpart.EquipmentProviderId = model.EquipmentProviderId;
            softpart.EquipmentMachPartId = model.EquipmentMachPartId;
            //softpart.UserId = model.UserId;
            //softpart.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.EquipmentMachineryFuelMachParts
                .FirstOrDefaultAsync(x => x.Id == id);


            _context.EquipmentMachineryFuelMachParts.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
