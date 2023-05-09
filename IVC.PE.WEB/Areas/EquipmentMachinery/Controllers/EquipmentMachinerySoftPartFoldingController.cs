//using IVC.PE.CORE.Helpers;
//using IVC.PE.DATA.Context;
//using IVC.PE.ENTITIES.Models.EquipmentMachinery;
//using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
//using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftPartViewModels;
//using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
//using IVC.PE.WEB.Controllers;
//using IVC.PE.WEB.Options;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
//{
//    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
//    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
//    [Route("equipos/parte-equipo-liviano-folding")]

//    public class EquipmentMachinerySoftPartFoldingController : BaseController
//    {
//        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

//        public EquipmentMachinerySoftPartFoldingController(IvcDbContext context,
//    IOptions<CloudStorageCredentials> storageCredentials,
//        ILogger<EquipmentMachinerySoftPartFoldingController> logger) : base(context, logger)
//        {
//            _storageCredentials = storageCredentials;
//        }

//        [HttpGet("listar")]
//        public async Task<IActionResult> GetAll(Guid softPartId)
//        {
//            if (softPartId == Guid.Empty)
//                return Ok(new List<EquipmentMachinerySoftPartViewModel>());

//            var renovations = await _context.EquipmentMachinerySoftPartFoldings
//                .Include(x => x.EquipmentMachinerySoftPart)
//                .Include(x => x.EquipmentMachineryOperator)
//                .Where(x => x.EquipmentMachinerySoftPartId == softPartId)
//                .Select(x => new EquipmentMachinerySoftPartFoldingViewModel
//                {
//                    Id = x.Id,
//                    EquipmentMachinerySoftPartId = x.EquipmentMachinerySoftPartId,
//                    EquipmentMachinerySoftPart = new EquipmentMachinerySoftPartViewModel
//                    {
//                        EquipmentMachineryTypeSoftId = x.EquipmentMachinerySoftPart.EquipmentMachineryTypeSoftId,
//                        EquipmentProviderId = x.EquipmentMachinerySoftPart.EquipmentProviderId,
//                        EquipmentProvider  = new EquipmentProviderViewModel
//                        {
//                            ProviderId = x.EquipmentMachinerySoftPart.EquipmentProvider.ProviderId,
//                        }
                        
//                    },

//                    PartDate = x.PartDate.Date.ToDateString(),
//                    PartNumber = x.PartNumber,
//                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
//                    EquipmentMachineryOperator = new EquipmentMachineryOperatorViewModel
//                    {
//                        OperatorName = x.EquipmentMachineryOperator.OperatorName,
//                        FromOtherName = x.EquipmentMachineryOperator.FromOtherName,
//                        HiringType = x.EquipmentMachineryOperator.HiringType,
//                        IsFrom = x.EquipmentMachineryOperator.IsFrom
//                    },
//                    InitMileage = x.InitMileage,
//                    EndMileage = x.EndMileage,
//                    Specific = x.Specific,

//                })
//                .ToListAsync();



//                foreach( var folding in renovations)
//            {
//                var plus = await _context.EquipmentMachinerySoftPartPlusUltra
//                    .Include(x => x.EquipmentMachineryTypeSoftActivity)
//                    .Where(x => x.EquipmentMachinerySoftPartFoldingId == folding.Id)
//                    .Select(x => x.EquipmentMachineryTypeSoftActivity.Description)
//                    .ToListAsync();
//                folding.Activities = string.Join(", ", plus);
//            }

//            return Ok(renovations);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(Guid id)
//        {
//            var renovation = await _context.EquipmentMachinerySoftPartFoldings
//                .Include(x => x.EquipmentMachinerySoftPart)
//                .Where(x => x.Id == id)
//                .Select(x => new EquipmentMachinerySoftPartFoldingViewModel
//                {
//                    Id = x.Id,
//                    EquipmentMachinerySoftPartId = x.EquipmentMachinerySoftPartId,
//                    PartDate = x.PartDate.Date.ToDateString(),
//                    PartNumber = x.PartNumber,
//                    EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
//                    InitMileage = x.InitMileage,
//                    EndMileage = x.EndMileage,
//                    Specific = x.Specific
//                }).FirstOrDefaultAsync();
//            var activities = await _context.EquipmentMachinerySoftPartPlusUltra
//    .Where(x => x.EquipmentMachinerySoftPartFoldingId == id)
//    .Select(x => x.EquipmentMachineryTypeSoftActivityId)
//    .ToListAsync();


//            renovation.EquipmentMachineryTypeSoftActivities = activities;
//            return Ok(renovation);
//        }

//        [HttpPost("crear")]
//        public async Task<IActionResult> Create(EquipmentMachinerySoftPartFoldingViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);


//            var newRenovation = new EquipmentMachinerySoftPartFolding()
//            {
//                EquipmentMachinerySoftPartId = model.EquipmentMachinerySoftPartId,
//                PartDate = model.PartDate.ToDateTime(),
//                PartNumber = model.PartNumber,
//                EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
//                InitMileage = model.InitMileage,
//                EndMileage = model.EndMileage,
//                Specific = model.Specific
//            };

//            await _context.EquipmentMachinerySoftPartPlusUltra.AddRangeAsync(
//    model.EquipmentMachineryTypeSoftActivities.Select(x => new EquipmentMachinerySoftPartPlus
//    {
//        EquipmentMachinerySoftPartFolding = newRenovation,
//        EquipmentMachineryTypeSoftActivityId = x
//    }).ToList());


//            await _context.EquipmentMachinerySoftPartFoldings.AddAsync(newRenovation);
//            await _context.SaveChangesAsync();

//            return Ok();
//        }

//        [HttpPut("editar/{id}")]
//        public async Task<IActionResult> Edit(Guid id, EquipmentMachinerySoftPartFoldingViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var renovation = await _context.EquipmentMachinerySoftPartFoldings
//                .FirstOrDefaultAsync(x => x.Id == id);


//            renovation.PartDate = model.PartDate.ToDateTime();
//            renovation.PartNumber = model.PartNumber;
//            renovation.EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId;
//            renovation.InitMileage = model.InitMileage;
//            renovation.EndMileage = model.EndMileage;
//            renovation.Specific = model.Specific;

//            var activitiesDb = await _context.EquipmentMachinerySoftPartPlusUltra
//    .Where(x => x.EquipmentMachinerySoftPartFoldingId == id)
//    .ToListAsync();
//            _context.EquipmentMachinerySoftPartPlusUltra.RemoveRange(activitiesDb);
//            await _context.EquipmentMachinerySoftPartPlusUltra.AddRangeAsync(
//                model.EquipmentMachineryTypeSoftActivities.Select(x => new EquipmentMachinerySoftPartPlus
//                {
//                    EquipmentMachinerySoftPartFoldingId = id,
//                    EquipmentMachineryTypeSoftActivityId = x
//                }).ToList());

//            await _context.SaveChangesAsync();

//            return Ok();
//        }

//        [HttpDelete("eliminar/{id}")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var data = await _context.EquipmentMachinerySoftPartFoldings.FirstOrDefaultAsync(x => x.Id == id);


//            var activitiesDb = await _context.EquipmentMachinerySoftPartPlusUltra
//    .Where(x => x.EquipmentMachinerySoftPartFoldingId == id)
//    .ToListAsync();

//            _context.EquipmentMachinerySoftPartPlusUltra.RemoveRange(activitiesDb);
//            _context.EquipmentMachinerySoftPartFoldings.Remove(data);

//            await _context.SaveChangesAsync();

//            return Ok();
//        }
//    }
//}
