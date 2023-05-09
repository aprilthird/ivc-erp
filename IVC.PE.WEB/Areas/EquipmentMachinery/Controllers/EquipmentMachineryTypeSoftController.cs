using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("equipos/tipo-de-equipo-liviano")]
    public class EquipmentMachineryTypeSoftController : BaseController
    {
        public EquipmentMachineryTypeSoftController(IvcDbContext context,
        ILogger<EquipmentMachineryTypeSoftController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.EquipmentMachineryTypeSofts
              .AsQueryable();

            var data = await query
                .Select(x => new EquipmentMachineryTypeSoftViewModel
                {
                    Id = x.Id,
                    Description = x.Description,

                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachineryTypeSofts

                 .Where(x => x.Id == id)
                 .Select(x => new EquipmentMachineryTypeSoftViewModel
                 {
                     Id = x.Id,
                     Description = x.Description
                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachineryTypeSoftViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryType = new EquipmentMachineryTypeSoft
            {

                Description = model.Description
            };

            await _context.EquipmentMachineryTypeSofts.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachineryTypeSoftViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTypes = await _context.EquipmentMachineryTypeSofts
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentMachineryTypes.Description = model.Description;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.EquipmentMachineryTypeSofts
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Tipo de certificado con Id '{id}' no encontrado.");
            _context.EquipmentMachineryTypeSofts.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 1;
                    var machinery = new EquipmentMachineryTypeSoft();

                    while (!workSheet.Cell($"C{counter}").IsEmpty())
                    {


                        machinery.Id = Guid.NewGuid();
                        machinery.Description = workSheet.Cell($"C{counter}").GetString();

                        await _context.EquipmentMachineryTypeSofts.AddAsync(machinery);
                        await _context.SaveChangesAsync();

                        ++counter;
                    }
                }
                mem.Close();
            }

            return Ok();
        }

    }
}

