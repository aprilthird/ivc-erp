using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldCostPerformanceViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/colector-descarga-cyr")]
    public class SewerManifoldCostPerformanceController : BaseController
    {
        public SewerManifoldCostPerformanceController(IvcDbContext context,
            ILogger<SewerManifoldCostPerformanceController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var cps = await _context.SewerManifoldCostPerformances
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new SewerManifoldCostPerformanceViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    TerrainType = x.TerrainType,
                    HeightMin = x.HeightMin,
                    HeightMax = x.HeightMax,
                    Unit = x.Unit,
                    Price = x.Price,
                    Workforce = x.Workforce,
                    Equipment = x.Equipment,
                    Services = x.Services,
                    Materials = x.Materials,
                    SecurityFactor = x.SecurityFactor
                }).ToListAsync();

            return Ok(cps);
        }

        [HttpPost("carga-base")]
        public async Task<IActionResult> LoadBaseCP(SewerManifoldLoadCPViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cps = ReadBaseCP(model);

            await _context.SewerManifoldCostPerformances.AddRangeAsync(cps);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private List<SewerManifoldCostPerformance> ReadBaseCP(SewerManifoldLoadCPViewModel model)
        {
            var cps = new List<SewerManifoldCostPerformance>();

            using (var mem = new MemoryStream())
            {
                model.File.CopyTo(mem);

                using (var workBook = new XLWorkbook(mem))
                {
                    var ws = workBook.Worksheets.First();
                    var counter = 4;
                    
                    while (!ws.Cell($"A{counter}").IsEmpty())
                    {
                        var cp = new SewerManifoldCostPerformance();

                        cp.ProjectId = GetProjectId();

                        cp.Description = ws.Cell($"A{counter}").GetString();

                        var terrainTypeStr = ws.Cell($"B{counter}").GetString();
                        if (terrainTypeStr == "N")
                            cp.TerrainType = ConstantHelpers.Terrain.Type.NORMAL;
                        else if (terrainTypeStr == "SR")
                            cp.TerrainType = ConstantHelpers.Terrain.Type.SEMIROCOUS;
                        else
                            cp.TerrainType = ConstantHelpers.Terrain.Type.ROCKY;

                        cp.HeightMin = ws.Cell($"C{counter}").GetDouble();
                        cp.HeightMax = ws.Cell($"D{counter}").GetDouble();

                        cp.Unit = ws.Cell($"E{counter}").GetString();

                        cp.Price = ws.Cell($"F{counter}").GetDouble();

                        cp.Workforce = ws.Cell($"G{counter}").GetDouble();
                        cp.Equipment = ws.Cell($"H{counter}").GetDouble();
                        cp.Services = ws.Cell($"I{counter}").GetDouble();
                        cp.Materials = ws.Cell($"J{counter}").GetDouble();

                        cp.SecurityFactor = model.SecurityFactor;

                        cps.Add(cp);

                        ++counter;
                    }
                }
            }

            return cps;
        }
    }
}
