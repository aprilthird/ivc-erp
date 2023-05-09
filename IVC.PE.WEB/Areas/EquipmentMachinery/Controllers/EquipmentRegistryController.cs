using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentRegistryViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/registro")]
    public class EquipmentRegistryController : BaseController
    {
        public EquipmentRegistryController(IvcDbContext context,
            ILogger<EquipmentRegistryController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Equipment.AsQueryable();

            var data = await query.Select(x => new EquipmentViewModel
            {
                Id = x.Id,
                EquipmentType = x.EquipmentType.Name,
                SerialNumber = x.SerialNumber,
                Model = x.Model,
                Code = x.Code,
                Propietary = x.Propietary,
                Operator = x.Operator,
                Status = x.Status
            }).ToListAsync();

            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.FULL_ACCESS)]
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.First();
                    var counter = 11;
                    var equipmentList = new List<Equipment>();
                    var equipmentTypes = new List<EquipmentType>();
                    while (!workSheet.Cell($"E{counter}").IsEmpty())
                    {
                        var equipmentTypeName = workSheet.Cell($"B{counter}").GetString();
                        var equipmentType = await _context.EquipmentTypes.FirstOrDefaultAsync(x => x.Name.ToUpper() == equipmentTypeName);
                        if(equipmentType == null)
                        {
                            equipmentType = equipmentTypes.FirstOrDefault(x => x.Name.ToUpper() == equipmentTypeName);
                            if(equipmentType == null)
                            {
                                equipmentType = new EquipmentType
                                {
                                    Name = equipmentTypeName
                                };
                                equipmentTypes.Add(equipmentType);
                            }
                        }
                        var equipment = new Equipment();
                        equipment.EquipmentType = equipmentType;
                        equipment.Model = workSheet.Cell($"D{counter}").GetString();
                        equipment.SerialNumber = workSheet.Cell($"E{counter}").GetString();
                        equipment.Code = workSheet.Cell($"F{counter}").GetString();
                        equipment.Propietary = workSheet.Cell($"K{counter}").GetString();
                        equipment.Operator = workSheet.Cell($"L{counter}").GetString();
                        equipment.Status = ConstantHelpers.Equipment.Status.NORMALIZED_VALUES.FirstOrDefault(x => x.Value.ToUpper() == workSheet.Cell($"N{counter}").GetString()).Key;
                        equipmentList.Add(equipment);
                        ++counter;
                    }
                    await _context.EquipmentTypes.AddRangeAsync(equipmentTypes);
                    await _context.Equipment.AddRangeAsync(equipmentList);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }
    }
}