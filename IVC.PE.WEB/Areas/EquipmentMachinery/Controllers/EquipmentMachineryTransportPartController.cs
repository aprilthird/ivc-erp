using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-equipo-transporte")]
    public class EquipmentMachineryTransportPartController : BaseController
    {

        public EquipmentMachineryTransportPartController(IvcDbContext context,
        ILogger<EquipmentMachineryTransportPartController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, int? month = null, int? year = null ,Guid? providerId = null)
        {
            var pId = GetProjectId();

            var data = await _context.Set<UspEquipmentMachineryTransportPart>().FromSqlRaw("execute EquipmentMachinery_uspTransportPart")
         .IgnoreQueryFilters()
         .ToListAsync();

            data = data.Where(x => x.ProjectId == pId).OrderBy(x => x.TradeName).ToList();

            if (month == 1)
                data = data.Where(x => x.MonthPartDate == 1 && x.FoldingId != null).ToList();
            if (month == 2)
                data = data.Where(x => x.MonthPartDate == 2 && x.FoldingId != null).ToList();
            if (month == 3)
                data = data.Where(x => x.MonthPartDate == 3 && x.FoldingId != null).ToList();
            if (month == 4)
                data = data.Where(x => x.MonthPartDate == 4 && x.FoldingId != null).ToList();
            if (month == 5)
                data = data.Where(x => x.MonthPartDate == 5 && x.FoldingId != null).ToList();
            if (month == 6)
                data = data.Where(x => x.MonthPartDate == 6 && x.FoldingId != null).ToList();
            if (month == 7)
                data = data.Where(x => x.MonthPartDate == 7 && x.FoldingId != null).ToList();
            if (month == 8)
                data = data.Where(x => x.MonthPartDate == 8 && x.FoldingId != null).ToList();
            if (month == 9)
                data = data.Where(x => x.MonthPartDate == 9 && x.FoldingId != null).ToList();
            if (month == 10)
                data = data.Where(x => x.MonthPartDate == 10 && x.FoldingId != null).ToList();
            if (month == 11)
                data = data.Where(x => x.MonthPartDate == 11 && x.FoldingId != null).ToList();
            if (month == 12)
                data = data.Where(x => x.MonthPartDate == 12 && x.FoldingId != null).ToList();


            if(year.HasValue)
                data = data.Where(x => x.YearPartDate == year.Value && x.FoldingId != null).ToList();

            if(providerId.HasValue)
            {
                data = data.Where(x => x.EquipmentProviderId == providerId.Value).ToList();
            }
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachineryTransportParts
                .Include(x => x.EquipmentProvider.Provider)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryTransportPartViewModel
                {
                    Id = x.Id,
                    //PartNumber = x.PartNumber,
                    //PartDate = x.PartDate.Date.ToDateString(),
                    EquipmentMachineryTypeTransportId = x.EquipmentMachineryTypeTransportId,
                    //UserId = x.UserId,
                    //UserName = x.UserName,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentProvider = new EquipmentProviderViewModel
                    {
                        ProviderId = x.EquipmentProvider.ProviderId

                    },
                    EquipmentMachineryTransportId = x.EquipmentMachineryTransportId,
                    Year = x.Year,
                    Month = x.Month,
                    //EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,

                }).FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTransportPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = new EquipmentMachineryTransportPart
            {
                ProjectId = GetProjectId(),
                //PartNumber = model.PartNumber,
                //PartDate = model.PartDate.ToDateTime(),
                EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeTransportId,
                EquipmentProviderId = model.EquipmentProviderId,
                EquipmentMachineryTransportId = model.EquipmentMachineryTransportId,
                //EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                //UserId = model.UserId,
                //UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,
                Month = model.Month,
                Year = model.Year
                

            };

            await _context.EquipmentMachineryTransportParts.AddAsync(softpart);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTransportPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = await _context.EquipmentMachineryTransportParts
                .FirstOrDefaultAsync(x => x.Id == id);
            softpart.EquipmentMachineryTypeTransportId = model.EquipmentMachineryTypeTransportId;
            softpart.EquipmentProviderId = model.EquipmentProviderId;
            softpart.EquipmentMachineryTransportId = model.EquipmentMachineryTransportId;
            softpart.Year = model.Year;
            softpart.Month = model.Month;
            //softpart.UserId = model.UserId;
            //softpart.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.EquipmentMachineryTransportParts.FirstOrDefaultAsync(x => x.Id == id);


            _context.EquipmentMachineryTransportParts.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
