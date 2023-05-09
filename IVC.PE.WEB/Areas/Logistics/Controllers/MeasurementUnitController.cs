using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/unidades")]
    public class MeasurementUnitController : BaseController
    {


        public MeasurementUnitController(IvcDbContext context,
            ILogger<MeasurementUnitController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.MeasurementUnits
                .Select(x => new MeasurementUnitViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbreviation
                }).AsNoTracking()
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var measurementUnit = await _context.MeasurementUnits
                .Where(x => x.Id == id)
                .Select(x => new MeasurementUnitViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Abbreviation = x.Abbreviation
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(measurementUnit);
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(MeasurementUnitViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var measurementUnit = new MeasurementUnit
            {
                Name = model.Name,
                Abbreviation = model.Abbreviation
            };
            await _context.MeasurementUnits.AddAsync(measurementUnit);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, MeasurementUnitViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var measurementUnit = await _context.MeasurementUnits.FindAsync(id);
            measurementUnit.Name = model.Name;
            measurementUnit.Abbreviation = model.Abbreviation;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var measurementUnit = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Id == id);
            if (measurementUnit == null)
                return BadRequest($"Unidad de Medida con Id '{id}' no encontrado.");
            _context.MeasurementUnits.Remove(measurementUnit);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.Logistics.FULL_ACCESS)]
        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "TIPO DE INSUMO");
                    var counter = 7;
                    var measurementUnits = new List<MeasurementUnit>();
                    while (!workSheet.Cell($"H{counter}").IsEmpty())
                    {
                        var measurementUnit = new MeasurementUnit();
                        measurementUnit.Abbreviation = workSheet.Cell($"H{counter}").GetString();
                        measurementUnit.Name = workSheet.Cell($"I{counter}").GetString();
                        measurementUnits.Add(measurementUnit);
                        ++counter;
                    }
                    await _context.MeasurementUnits.AddRangeAsync(measurementUnits);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }
    }
}