using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.TransportPhaseViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-equipo-transporte-folding")]
    public class EquipmentMachineryTransportPartFoldingController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EquipmentMachineryTransportPartFoldingController(IvcDbContext context,
    IOptions<CloudStorageCredentials> storageCredentials,
        ILogger<EquipmentMachineryTransportPartFoldingController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid softPartId, int? month = null, int? year = null) 
        {
            if (softPartId == Guid.Empty)
                return Ok(new List<EquipmentMachineryTransportPartViewModel>());

            var renovations = await _context.EquipmentMachineryTransportPartFoldings
                .Include(x => x.EquipmentMachineryTransportPart)
                .Include(x => x.EquipmentMachineryOperator)
                .Include(x => x.EquipmentMachineryOperator.Worker)
                .Include(x => x.TransportPhase)
                .Where(x => x.EquipmentMachineryTransportPartId == softPartId)
                .Select(x => new EquipmentMachineryTransportPartFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTransportPartId = x.EquipmentMachineryTransportPartId,
                    EquipmentMachineryTransportPart = new EquipmentMachineryTransportPartViewModel
                    {
                        EquipmentMachineryTypeTransportId = x.EquipmentMachineryTransportPart.EquipmentMachineryTypeTransportId,
                        EquipmentProviderId = x.EquipmentMachineryTransportPart.EquipmentProviderId,
                        EquipmentProvider = new EquipmentProviderViewModel
                        {
                            ProviderId = x.EquipmentMachineryTransportPart.EquipmentProvider.ProviderId,
                        }

                    },

                    PartDate = x.PartDate.Date.ToDateString(),
                    PartNumber = x.PartNumber,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                    EquipmentMachineryOperator = new EquipmentMachineryOperatorViewModel
                    {
                        OperatorName = x.EquipmentMachineryOperator.OperatorName,
                        FromOtherName = x.EquipmentMachineryOperator.FromOtherName,
                        HiringType = x.EquipmentMachineryOperator.HiringType,
                        WorkerId = x.EquipmentMachineryOperator.WorkerId,

                        Worker = x.EquipmentMachineryOperator.WorkerId != null ? new WorkerViewModel
                        {

                            Name = x.EquipmentMachineryOperator.Worker.Name,
                            MiddleName = x.EquipmentMachineryOperator.Worker.MiddleName,
                            PaternalSurname = x.EquipmentMachineryOperator.Worker.PaternalSurname,
                            MaternalSurname = x.EquipmentMachineryOperator.Worker.MaternalSurname,
                            PhoneNumber = x.EquipmentMachineryOperator.Worker.PhoneNumber,
                            Document = x.EquipmentMachineryOperator.Worker.Document,
                            DocumentType = x.EquipmentMachineryOperator.Worker.DocumentType
                        } : null,

                    },
                    InitMileage = x.InitMileage,
                    EndMileage = x.EndMileage,
                    Specific = x.Specific,

                    UserId = x.UserId,
                    UserName = x.UserName,
                    WorkArea = x.WorkArea,

                    TransportPhaseId = x.TransportPhaseId,
                    TransportPhase = new TransportPhaseViewModel
                    {
                        ProjectPhaseId = x.TransportPhase.ProjectPhaseId,
                        ProjectPhase = new ProjectPhaseViewModel
                        {
                            Code = x.TransportPhase.ProjectPhase.Code
                        }
                    }

                })
                .ToListAsync();

            
            if (month == 1)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 1 ).ToList();
            if (month == 2)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 2).ToList();
            if (month == 3)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 3).ToList();
            if (month == 4)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 4).ToList();
            if (month == 5)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 5).ToList();
            if (month == 6)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 6).ToList();
            if (month == 7)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 7).ToList();
            if (month == 8)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 8).ToList();
            if (month == 9)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 9).ToList();
            if (month == 10)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 10).ToList();
            if (month == 11)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 11).ToList();
            if (month == 12)
                renovations = renovations.Where(x => x.PartDate.ToMonth() == 12).ToList();

            if(year == 2021)
                renovations = renovations.Where(x => x.PartDate.ToYear()== 2021).ToList();

            if (year == 2020)
                renovations = renovations.Where(x => x.PartDate.ToYear() == 2020).ToList();

            if (year == 2019)
                renovations = renovations.Where(x => x.PartDate.ToYear() == 2019).ToList();

            if (year == 2018)
                renovations = renovations.Where(x => x.PartDate.ToYear() == 2018).ToList();

            foreach (var folding in renovations)
            { 
                var plus = await _context.EquipmentMachineryTransportPartPlusUltra
                    .Include(x => x.EquipmentMachineryTypeTransportActivity)
                    .Where(x => x.EquipmentMachineryTransportPartFoldingId == folding.Id)
                    .Select(x => x.EquipmentMachineryTypeTransportActivity.Description)
                    .ToListAsync();
                folding.Activities = string.Join(", ", plus);
            }

            return Ok(renovations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var renovation = await _context.EquipmentMachineryTransportPartFoldings
                .Include(x => x.EquipmentMachineryTransportPart)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachineryTransportPartFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTransportPartId = x.EquipmentMachineryTransportPartId,
                    PartDate = x.PartDate.Date.ToDateString(),
                    PartNumber = x.PartNumber,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                    InitMileage = x.InitMileage,
                    EndMileage = x.EndMileage,
                    Specific = x.Specific,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    WorkArea = x.WorkArea,
                    TransportPhaseId = x.TransportPhaseId

                   
                }).FirstOrDefaultAsync();
            var activities = await _context.EquipmentMachineryTransportPartPlusUltra
    .Where(x => x.EquipmentMachineryTransportPartFoldingId == id)
    .Select(x => x.EquipmentMachineryTypeTransportActivityId)
    .ToListAsync();

            renovation.EquipmentMachineryTypeTransportActivities = activities;
            return Ok(renovation);
        }


        [HttpGet("domingo/{fatherid}")]
        public async Task<IActionResult> GetSunday(Guid fatherid)
        {
            var code = await _context.TransportPhases.Include(x=>x.ProjectPhase).FirstOrDefaultAsync(x => x.ProjectPhase.Code == "160205");

            var renovation = await _context.EquipmentMachineryTransportPartFoldings
                .Include(x => x.EquipmentMachineryTransportPart)
                .Include(x => x.EquipmentMachineryTransportPart.EquipmentProvider)
                .Include(x => x.EquipmentMachineryTransportPart.EquipmentProvider.Provider)
                .Include(x => x.EquipmentMachineryTransportPart.EquipmentMachineryTypeTransport)
                .Where(x => x.EquipmentMachineryTransportPartId == fatherid )
                .Select(x => new EquipmentMachineryTransportPartFoldingViewModel
                {
                    Id = x.Id,
                    EquipmentMachineryTransportPartId = x.EquipmentMachineryTransportPartId,
                    EquipmentMachineryTransportPart = new EquipmentMachineryTransportPartViewModel
                    {
                        EquipmentMachineryTypeTransportId = x.EquipmentMachineryTransportPart.EquipmentMachineryTypeTransportId,
                        EquipmentMachineryTransportId = x.EquipmentMachineryTransportPart.EquipmentMachineryTransportId,
                    },
                    PartDate = x.PartDate.Date.ToDateString(),
                    PartNumber = x.PartNumber,
                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                    InitMileage = x.InitMileage,
                    EndMileage = x.EndMileage,
                    Specific = x.Specific,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    WorkArea = x.WorkArea,
                    Order = x.Order,
                    TransportPhaseId = code.Id,
                    
                }).OrderByDescending(x=>x.Order).FirstOrDefaultAsync();

            var activities = await _context.EquipmentMachineryTypeTransportActivities
                .Where(x => x.EquipmentMachineryTypeTransportId == renovation.EquipmentMachineryTransportPart.EquipmentMachineryTypeTransportId && x.Description == "Domingo")
                .Select(x => x.Id).ToListAsync();
            renovation.EquipmentMachineryTypeTransportActivities = activities;


            return Ok(renovation);
        }


        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTransportPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var equipmentMachineryTransportPart = await _context.EquipmentMachineryTransportParts.Where(x => x.Id == model.EquipmentMachineryTransportPartId).FirstOrDefaultAsync();

            equipmentMachineryTransportPart.FoldingNumber++;
            var newRenovation = new EquipmentMachineryTransportPartFolding()
            {
                EquipmentMachineryTransportPartId = model.EquipmentMachineryTransportPartId,
                PartDate = model.PartDate.ToDateTime(),
                PartNumber = model.PartNumber,
                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId.HasValue? model.EquipmentMachineryOperatorId : null
                ,
                InitMileage = model.InitMileage,
                EndMileage = model.EndMileage,
                Specific = model.Specific,
                UserId = model.UserId,
                UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,
                WorkArea = users.WorkArea,
                TransportPhaseId = model.TransportPhaseId,
                //ProjectPhaseId = model.ProjectPhaseId.HasValue? model.ProjectPhaseId : null,
                //
                Order = equipmentMachineryTransportPart.FoldingNumber
            };

            if(equipmentMachineryTransportPart.FoldingNumber == 1)
            {
                equipmentMachineryTransportPart.LastInitMileage = model.InitMileage;
            }

            equipmentMachineryTransportPart.LastEndMileage = model.EndMileage;


            await _context.EquipmentMachineryTransportPartPlusUltra.AddRangeAsync(
    model.EquipmentMachineryTypeTransportActivities.Select(x => new EquipmentMachineryTransportPartPlus
    {
        EquipmentMachineryTransportPartFolding = newRenovation,
        EquipmentMachineryTypeTransportActivityId = x
    }).ToList());


            await _context.EquipmentMachineryTransportPartFoldings.AddAsync(newRenovation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTransportPartFoldingViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
    
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var equipmentMachineryTransportPart = await _context.EquipmentMachineryTransportParts.Where(x => x.Id == model.EquipmentMachineryTransportPartId).FirstOrDefaultAsync();

            var renovation = await _context.EquipmentMachineryTransportPartFoldings
                .FirstOrDefaultAsync(x => x.Id == id);


            renovation.PartDate = model.PartDate.ToDateTime();
            renovation.PartNumber = model.PartNumber;
            renovation.EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId.HasValue ? model.EquipmentMachineryOperatorId : null;
            renovation.InitMileage = model.InitMileage;
            renovation.EndMileage = model.EndMileage;
            renovation.Specific = model.Specific;

            renovation.UserId = model.UserId;
            renovation.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;
            renovation.WorkArea = users.WorkArea;
            renovation.TransportPhaseId = model.TransportPhaseId;
            //renovation.ProjectPhaseId = model.ProjectPhaseId.HasValue ? model.ProjectPhaseId : null;

            if (equipmentMachineryTransportPart.FoldingNumber == 1)
            {
                equipmentMachineryTransportPart.LastInitMileage = model.InitMileage;
            }

            equipmentMachineryTransportPart.LastEndMileage = model.EndMileage;

            var activitiesDb = await _context.EquipmentMachineryTransportPartPlusUltra
    .Where(x => x.EquipmentMachineryTransportPartFoldingId == id)
    .ToListAsync();
            _context.EquipmentMachineryTransportPartPlusUltra.RemoveRange(activitiesDb);
            await _context.EquipmentMachineryTransportPartPlusUltra.AddRangeAsync(
                model.EquipmentMachineryTypeTransportActivities.Select(x => new EquipmentMachineryTransportPartPlus
                {
                    EquipmentMachineryTransportPartFoldingId = id,
                    EquipmentMachineryTypeTransportActivityId = x
                }).ToList());

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.EquipmentMachineryTransportPartFoldings.FirstOrDefaultAsync(x => x.Id == id);


            var activitiesDb = await _context.EquipmentMachineryTransportPartPlusUltra
    .Where(x => x.EquipmentMachineryTransportPartFoldingId == id)
    .ToListAsync();

            var equipment = await _context.EquipmentMachineryTransportParts
.FirstOrDefaultAsync(x => x.Id == data.EquipmentMachineryTransportPartId);
            equipment.FoldingNumber--;

            _context.EquipmentMachineryTransportPartPlusUltra.RemoveRange(activitiesDb);
            _context.EquipmentMachineryTransportPartFoldings.Remove(data);

            await _context.SaveChangesAsync();


            var vars = await _context.EquipmentMachineryTransportPartFoldings.
    FirstOrDefaultAsync(x => x.Order == equipment.FoldingNumber
    && x.EquipmentMachineryTransportPartId == equipment.Id);

            if (equipment.FoldingNumber == 0)
            {
                equipment.LastInitMileage = 0;
                equipment.LastEndMileage = 0;
            }
            
            else
            {
                equipment.LastEndMileage = vars.EndMileage;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
