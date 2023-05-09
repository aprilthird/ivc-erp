using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.MachineryPhaseViewModels;
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
    [Route("equipos/fase-maquinaria")]
    public class MachineryPhaseController : BaseController
    {

        public MachineryPhaseController(IvcDbContext context,
        ILogger<MachineryPhaseController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {

            var query = _context.MachineryPhases
              .AsQueryable();

            var data = await query
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new MachineryPhaseViewModel
                {
                    Id = x.Id,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    ProjectId = x.ProjectId


                })
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.MachineryPhases

                 .Where(x => x.Id == id)
                 .Select(x => new MachineryPhaseViewModel
                 {
                     Id = x.Id,
                     ProjectPhaseId = x.ProjectPhaseId,

                 }).FirstOrDefaultAsync();

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(MachineryPhaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryType = new MachineryPhase
            {

                ProjectPhaseId = model.ProjectPhaseId,
                ProjectId = GetProjectId()
            };

            await _context.MachineryPhases.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, MachineryPhaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTypes = await _context.MachineryPhases
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentMachineryTypes.ProjectPhaseId = model.ProjectPhaseId;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.MachineryPhases
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipmentMachineryTypes == null)
                return BadRequest($"Tipo de certificado con Id '{id}' no encontrado.");
            _context.MachineryPhases.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("importar-datos")]
        public async Task<IActionResult> ImportData(IFormFile file)
        {
            var pId = GetProjectId();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var mp = new MachineryPhase();


                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {

                        //---------------Creación del For05
                        var phaseselected = await _context.MachineryPhases
                            .FirstOrDefaultAsync(x => x.ProjectPhase.Code== workSheet.Cell($"A{counter}").GetString() && x.ProjectId == GetProjectId());

                        
                            var neweqpselected = await _context.ProjectPhases
                           .FirstOrDefaultAsync(x => x.Code == workSheet.Cell($"A{counter}").GetString() && x.ProjectId == GetProjectId());
                            mp.Id = Guid.NewGuid();
                            mp.ProjectPhaseId = neweqpselected.Id;
                            mp.ProjectId = pId;

                            await _context.MachineryPhases.AddAsync(mp);
                            await _context.SaveChangesAsync();

                            ++counter;
                        }
                    }
                mem.Close();
            }
                
            

            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "CargaMasivaFaseMaquinaria.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Código de Fase de Equipo Maquinaria";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);




                workSheet.Column(1).Width = 35;
                



                workSheet.Rows().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
