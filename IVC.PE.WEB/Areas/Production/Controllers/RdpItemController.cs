using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Production;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.RdpItemViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.Production.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Production.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.PRODUCTION)]
    [Route("produccion/items-rdp")]
    public class RdpItemController : BaseController
    {
        public RdpItemController(IvcDbContext context,
            ILogger<RdpItemController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid ppId)
        {
            if (ppId == Guid.Empty)
                return Ok(new List<RdpItemViewModel>());

            var items = await _context.RdpItems
                .Include(x => x.ProjectPhase)
                .Where(x => x.ProjectPhaseId == ppId)
                .Select(x => new RdpItemViewModel
                {
                    Id = x.Id,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ProjectPhase = new ProjectPhaseViewModel
                    {
                        Code = x.ProjectPhase.Code,
                        Description = x.ProjectPhase.Description
                    },
                    ItemGroup = x.ItemGroup,
                    ItemPhaseCode = x.ItemPhaseCode,
                    ItemDescription = x.ItemDescription,
                    ItemUnit = x.ItemUnit,
                    ItemContractualAmmount = x.ItemContractualAmmount,
                    ItemStakeOutAmmount = x.ItemStakeOutAmmount
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var item = await _context.RdpItems
                .Where(x => x.Id == id)
                .Select(x => new RdpItemViewModel
                {
                    Id = x.Id,
                    ProjectPhaseId = x.ProjectPhaseId,
                    ItemGroup = x.ItemGroup,
                    ItemPhaseCode = x.ItemPhaseCode,
                    ItemDescription = x.ItemDescription,
                    ItemContractualAmmount = x.ItemContractualAmmount,
                    ItemStakeOutAmmount = x.ItemStakeOutAmmount,
                    ItemUnit = x.ItemUnit
                })
                .FirstOrDefaultAsync();

            return Ok(item);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, RdpItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.RdpItems
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            item.ItemDescription = model.ItemDescription;
            item.ItemGroup = model.ItemGroup;
            item.ItemContractualAmmount = model.ItemContractualAmmount;
            item.ItemStakeOutAmmount = model.ItemStakeOutAmmount;
            item.ItemUnit = model.ItemUnit;
            item.ItemPhaseCode = model.ItemPhaseCode;
            item.ProjectPhaseId = model.ProjectPhaseId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.RdpItems
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            _context.RdpItems.Remove(item);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.First();
                    var counter = 2;
                    var projectPhases = await _context.ProjectPhases.ToListAsync();
                    var rdpItemsDb = await _context.RdpItems.ToListAsync();
                    var rdpItems = new List<RdpItem>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        string projectPhase = workSheet.Cell($"A{counter}").GetString();

                        var projectPhaseDb = projectPhases.FirstOrDefault(x => x.Code.Equals(projectPhase));

                        if (projectPhaseDb != null)
                        {
                            var rdpItemDb = rdpItems.FirstOrDefault(x => x.ItemPhaseCode.Equals(workSheet.Cell($"C{counter}").GetString()));

                            if (rdpItemDb == null)
                            {
                                rdpItems.Add(new RdpItem
                                {
                                    ProjectPhaseId = projectPhaseDb.Id,
                                    ItemGroup = int.Parse(workSheet.Cell($"B{counter}").GetString()),
                                    ItemPhaseCode = workSheet.Cell($"C{counter}").GetString(),
                                    ItemDescription = workSheet.Cell($"D{counter}").GetString(),
                                    ItemUnit = workSheet.Cell($"E{counter}").GetString(),
                                    ItemContractualAmmount = decimal.Parse(workSheet.Cell($"F{counter}").GetString()),
                                    ItemStakeOutAmmount = decimal.Parse(workSheet.Cell($"G{counter}").GetString())
                                });
                            } else
                            {
                                rdpItemDb.ItemGroup = int.Parse(workSheet.Cell($"B{counter}").GetString());
                                rdpItemDb.ItemPhaseCode = workSheet.Cell($"C{counter}").GetString();
                                rdpItemDb.ItemDescription = workSheet.Cell($"D{counter}").GetString();
                                rdpItemDb.ItemUnit = workSheet.Cell($"E{counter}").GetString();
                                rdpItemDb.ItemContractualAmmount = decimal.Parse(workSheet.Cell($"F{counter}").GetString());
                                rdpItemDb.ItemStakeOutAmmount = decimal.Parse(workSheet.Cell($"G{counter}").GetString());
                            }
                        }

                        counter++;
                    }

                    if(rdpItems.Count > 0)
                    {
                        await _context.RdpItems.AddRangeAsync(rdpItems);
                        await _context.SaveChangesAsync();
                    }
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("excel-carga")]
        public FileResult GenerateLoadItemExcel()
        {
            string fileName = "ActividadesCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Fase Principal";
                workSheet.Cell($"A2").Value = "010201";

                workSheet.Cell($"B1").Value = "Grupo (0: Obras Concreto, 1:Arquitectura)";
                workSheet.Cell($"B2").Value = "0";

                workSheet.Cell($"C1").Value = "Fase";
                workSheet.Column(3).Style.NumberFormat.Format = "@";
                workSheet.Cell($"C2").Value = "01020103020101";

                workSheet.Cell($"D1").Value = "Descripción";
                workSheet.Cell($"D2").Value = "Concreto pre-mezclado f'c 280 kg/cm2";

                workSheet.Cell($"E1").Value = "Unidad";
                workSheet.Cell($"E2").Value = "m3";

                workSheet.Cell($"F1").Value = "metrado contractual";
                workSheet.Cell($"F2").Value = "419.81";

                workSheet.Cell($"G1").Value = "metrado replanteo";
                workSheet.Cell($"G2").Value = "419.81";

                //workSheet.Columns("A", "Q").AdjustToContents();
                workSheet.Columns().AdjustToContents();
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
