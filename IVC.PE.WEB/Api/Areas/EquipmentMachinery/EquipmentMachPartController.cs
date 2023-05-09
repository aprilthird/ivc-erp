using IVC.PE.BINDINGRESOURCES.Areas.EquipmentMachinery;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.EquipmentMachinery
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/equipos/parte-equipos-maquinaria")]
    public class EquipmentMachPartController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public EquipmentMachPartController(IvcDbContext context,
            ILogger<EquipmentMachPartController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("consultar-qr")]
        public async Task<IActionResult> GetAll(string qrString, int month, int year)
        {


            SqlParameter param = new SqlParameter("@QrString", qrString);
            SqlParameter param2 = new SqlParameter("@Month", month);
            SqlParameter param3 = new SqlParameter("@Year", year);

            var bps = await _context.Set<UspEquipmentMachPartApp>().FromSqlRaw("execute EquipmentMachinery_uspMachPartApp @QrString, @Month ,@Year"
                , param
                , param2
                , param3)
                .IgnoreQueryFilters()
                .ToListAsync();

            var query = bps
                .Select(x => new EquipmentMachPartConsultingResourceModel
                {

                    EquipmentMachId = x.EquipmentMachId,
                    EquipmentMachineryTypeTypeId = x.EquipmentMachineryTypeTypeId
                }).FirstOrDefault();

            return Ok(query);
        }

        [HttpGet("consultar-operador-qr")]
        public async Task<IActionResult> GetOperatorInfo(string qrString)
        {

            var query = await _context.EquipmentMachineryOperators.
                Where(x => x.ActualDni == qrString)
                .Select(x => new EquipmentMachineryOperatorResourceModel
                {
                    Id = x.Id,
                    ActualName = x.ActualName

                }).FirstOrDefaultAsync();
            return Ok(query);
        }


        [HttpPost("parte-padre/registrar")]
        public async Task<IActionResult> CreatePart([FromBody] EquipmentMachPartResourceModel model, string qrString)
        {

            var newMachPart = new List<EquipmentMachPart>();

            SqlParameter param = new SqlParameter("@QrString", qrString);
            var bps = await _context.Set<UspEquipmentMachApp>().FromSqlRaw("execute EquipmentMachinery_uspMachApp @QrString"
                , param)
                .IgnoreQueryFilters()
                .ToListAsync();

            var query = bps
            .FirstOrDefault();

            newMachPart.Add(new EquipmentMachPart
            {
                Month = model.Month,
                Year = model.Year,
                ProjectId = model.ProjectId,
                EquipmentProviderId = query.EquipmentproviderId,
                EquipmentMachineryTypeTypeId = query.EquipmentMachineryTypeTypeId,
                EquipmentMachId = query.EquipmentMachId,
                FoldingNumber = 0,
            }) ;

            await _context.EquipmentMachParts.AddRangeAsync(newMachPart);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("parte-diario/registrar")]
        public async Task<IActionResult> CreateFolding([FromBody] EquipmentMachPartFoldingResourceModel model, string qrString)
        {

            var newMachPart = new List<EquipmentMachPartFolding>();

            SqlParameter param = new SqlParameter("@QrString", qrString);
            SqlParameter param2 = new SqlParameter("@Month", DateTime.UtcNow.ToDefaultTimeZone().Month);
            SqlParameter param3 = new SqlParameter("@Year", DateTime.UtcNow.ToDefaultTimeZone().Year);

            var bps = await _context.Set<UspEquipmentMachPartApp>().FromSqlRaw("execute EquipmentMachinery_uspMachPartApp @QrString, @Month ,@Year"
                , param
                , param2
                , param3)
                .IgnoreQueryFilters()
                .ToListAsync();


            var query = bps
            .FirstOrDefault();

            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            newMachPart.Add(new EquipmentMachPartFolding
            {
                EquipmentMachPartId = query.EquipmentMachId,
                PartNumber = model.PartNumber,
                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                Order = query.FoldingNumber +1,
                UserId = model.UserId,
                UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,
                WorkArea = users.WorkArea,
                EquipmentMachineryTypeTypeActivityId = model.EquipmentMachineryTypeTypeActivityId,
                SewerGroupId = model.SewerGroupId,
                MachineryPhaseId = model.MachineryPhaseId,
                InitHorometer = model.InitHorometer,
                EndHorometer = model.EndHorometer,
                Dif = model.EndHorometer - model.InitHorometer,
                PartDate = DateTime.Now,
                Specific = model.Specific

                
            });
            
            await _context.EquipmentMachPartFoldings.AddRangeAsync(newMachPart);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("parte-padre-actualizar-numero")]
        public async Task<IActionResult> EditMachPart(string qrString)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            SqlParameter param = new SqlParameter("@QrString", qrString);
            SqlParameter param2 = new SqlParameter("@Month", DateTime.UtcNow.ToDefaultTimeZone().Month);
            SqlParameter param3 = new SqlParameter("@Year", DateTime.UtcNow.ToDefaultTimeZone().Year);

            var bps = await _context.Set<UspEquipmentMachPartApp>().FromSqlRaw("execute EquipmentMachinery_uspMachPartApp @QrString, @Month ,@Year"
                , param
                , param2
                , param3)
                .IgnoreQueryFilters()
                .ToListAsync();

            var query = bps
                .FirstOrDefault();
            var mp = await _context.EquipmentMachParts
                .FirstOrDefaultAsync(x => x.Id == query.EquipmentMachId);

            mp.FoldingNumber = mp.FoldingNumber + 1;
            

            await _context.SaveChangesAsync();
            return Ok();
        }

       
    }


}
