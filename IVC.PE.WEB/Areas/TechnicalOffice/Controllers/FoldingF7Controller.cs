using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.FoldingF7ViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/folding-f7-pdp")]
    public class FoldingF7Controller : BaseController
    {
        public FoldingF7Controller(IvcDbContext context,
            ILogger<FoldingF7Controller> logger)
            : base(context, logger)
        {

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? ProductionDailyPartId = null)
        {
            if (!ProductionDailyPartId.HasValue)
                return Ok(new List<FoldingF7ViewModel>());

            var foldingsF7s = await _context.FoldingF7s
                .OrderBy(x => x.Date)
                .Where(x => x.ProductionDailyPartId == ProductionDailyPartId)
                .Select(x => new FoldingF7ViewModel
                {
                    Id = x.Id,
                    ProductionDailyPartId = x.ProductionDailyPartId,
                    Date = x.Date.ToDateString(),
                    ExcavatedLength = x.ExcavatedLength.ToString(),
                    InstalledLength = x.InstalledLength.ToString(),
                    RefilledLength = x.RefilledLength.ToString(),
                    GranularBaseLength = x.GranularBaseLength.ToString(),
                    CalendarDate = x.Date
                }).ToListAsync();

            return Ok(foldingsF7s);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var foldingsF7 = await _context.FoldingF7s
                .Where(x => x.Id == id)
                .Select(x => new FoldingF7ViewModel
                {
                    Id = x.Id,
                    ProductionDailyPartId = x.ProductionDailyPartId,
                    Date = x.Date.ToDateString(),
                    ExcavatedLength = x.ExcavatedLength.ToString(),
                    InstalledLength = x.InstalledLength.ToString(),
                    RefilledLength = x.RefilledLength.ToString(),
                    GranularBaseLength = x.GranularBaseLength.ToString()
                }).FirstOrDefaultAsync();

            return Ok(foldingsF7);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FoldingF7ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Date == null || model.Date == "")
                return BadRequest("No se ha ingresado la fecha");

            if (model.ExcavatedLength == null)
                model.ExcavatedLength = "0";

            if (model.InstalledLength == null)
                model.InstalledLength = "0";

            if (model.RefilledLength == null)
                model.RefilledLength = "0";

            if (model.GranularBaseLength == null)
                model.GranularBaseLength = "0";

            var pdp = await _context.ProductionDailyParts.FirstOrDefaultAsync(x => x.Id == model.ProductionDailyPartId);

            var sewerManifoldRefs = await _context.SewerManifoldReferences
                .Include(y => y.SewerManifoldReview)
                .Include(y => y.SewerManifoldExecution)
                .FirstOrDefaultAsync(x => x.SewerManifoldExecution.ProductionDailyPartId == model.ProductionDailyPartId);

            decimal maxExcavated = (decimal)sewerManifoldRefs.SewerManifoldReview.LengthOfDigging - (decimal)pdp.ExcavatedLength;
            decimal maxInstalled = (decimal)sewerManifoldRefs.SewerManifoldReview.LengthOfPipelineInstalled - (decimal)pdp.InstalledLength;
            decimal maxRefilled = (decimal)pdp.FillLength - (decimal)pdp.RefilledLength;
            decimal maxGranularBase = (decimal)sewerManifoldRefs.SewerManifoldReview.LengthOfDigging - (decimal)pdp.GranularBaseLength;

            if ((decimal)model.ExcavatedLength.ToDoubleString() > maxExcavated)
                return BadRequest("Se ha superado el saldo por ejecutar Excavada");

            if ((decimal)model.InstalledLength.ToDoubleString() > maxInstalled)
                return BadRequest("Se ha superado el saldo por ejecutar Instalada");

            if ((decimal)model.RefilledLength.ToDoubleString() > maxRefilled)
                return BadRequest("Se ha superado el saldo por ejecutar Rellenada");

            if ((decimal)model.GranularBaseLength.ToDoubleString() > maxGranularBase)
                return BadRequest("Se ha superado el saldo por ejecutar Base Granular");

            var foldingF7 = new FoldingF7
            {
                ProductionDailyPartId = model.ProductionDailyPartId,
                Date = model.Date.ToDateTime(),
                ExcavatedLength = model.ExcavatedLength.ToDoubleString(),
                InstalledLength = model.InstalledLength.ToDoubleString(),
                RefilledLength = model.RefilledLength.ToDoubleString(),
                GranularBaseLength = model.GranularBaseLength.ToDoubleString()
            };

            await _context.FoldingF7s.AddAsync(foldingF7);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FoldingF7ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Date == null || model.Date == "")
                return BadRequest("No se ha ingresado la fecha");

            if (model.ExcavatedLength == null)
                model.ExcavatedLength = "0";

            if (model.InstalledLength == null)
                model.InstalledLength = "0";

            if (model.RefilledLength == null)
                model.RefilledLength = "0";

            if (model.GranularBaseLength == null)
                model.GranularBaseLength = "0";

            var foldingF7 = await _context.FoldingF7s
                .FirstOrDefaultAsync(x => x.Id == id);

            var pdp = await _context.ProductionDailyParts.FirstOrDefaultAsync(x => x.Id == foldingF7.ProductionDailyPartId);

            var smReview = await _context.SewerManifoldReferences
                .Include(x => x.SewerManifoldExecution)
                .Where(x => x.SewerManifoldExecution.ProductionDailyPartId == foldingF7.ProductionDailyPartId)
                .Select(x => x.SewerManifoldReview)
                .FirstOrDefaultAsync();

            var PreFolding = await _context.FoldingF7s.Where(x => x.ProductionDailyPartId == foldingF7.ProductionDailyPartId).ToListAsync();

            var ExcavatedLengthSuma = 0.0;
            var InstalledLengthSuma = 0.0;
            var RefilledLengthSuma = 0.0;
            var GranularBaseLengthSuma = 0.0;

            foreach (var folding in PreFolding)
            {
                if (folding != foldingF7)
                {
                    ExcavatedLengthSuma = ExcavatedLengthSuma + folding.ExcavatedLength;
                    InstalledLengthSuma = InstalledLengthSuma + folding.InstalledLength;
                    RefilledLengthSuma = RefilledLengthSuma + folding.RefilledLength;
                    GranularBaseLengthSuma = GranularBaseLengthSuma + folding.GranularBaseLength;
                }
            }

            decimal maxExcavated = (decimal)smReview.LengthOfDigging - (decimal)ExcavatedLengthSuma;
            decimal maxInstalled = (decimal)smReview.LengthOfPipelineInstalled - (decimal)InstalledLengthSuma;
            decimal maxRefilled = (decimal)pdp.FillLength - (decimal)RefilledLengthSuma;
            decimal maxGranularBase = (decimal)smReview.LengthOfDigging - (decimal)GranularBaseLengthSuma;

            if ((decimal)model.ExcavatedLength.ToDoubleString() > maxExcavated)
                return BadRequest("Se ha superado el salgo por ejecutar Excavada");

            if ((decimal)model.InstalledLength.ToDoubleString() > maxInstalled)
                return BadRequest("Se ha superado el salgo por ejecutar Instalada");

            if ((decimal)model.RefilledLength.ToDoubleString() > maxRefilled)
                return BadRequest("Se ha superado el salgo por ejecutar Rellenada");

            if ((decimal)model.GranularBaseLength.ToDoubleString() > maxGranularBase)
                return BadRequest("Se ha superado el salgo por ejecutar Base Granular");

            foldingF7.Date = model.Date.ToDateTime();
            foldingF7.ExcavatedLength = model.ExcavatedLength.ToDoubleString();
            foldingF7.InstalledLength = model.InstalledLength.ToDoubleString();
            foldingF7.RefilledLength = model.RefilledLength.ToDoubleString();
            foldingF7.GranularBaseLength = model.GranularBaseLength.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var foldingD7 = await _context.FoldingF7s
                .FirstOrDefaultAsync(x => x.Id == id);

            if (foldingD7 == null)
                return BadRequest("No se encontró el Folding F7");

            _context.FoldingF7s.Remove(foldingD7);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> EditPdp(Guid id)
        {

            var pdp = await _context.ProductionDailyParts.FirstOrDefaultAsync(x => x.Id == id);

            var foldingTodos = await _context.FoldingF7s.Where(x => x.ProductionDailyPartId == pdp.Id).ToListAsync();


            var ExcavatedLengthSuma = 0.0;
            var InstalledLengthSuma = 0.0;
            var RefilledLengthSuma = 0.0;
            var GranularBaseLengthSuma = 0.0;

            var CantExcavation = 0;
            var CantInstallation = 0;
            var CantFilled = 0;

            foreach (var folding in foldingTodos)
            {
                ExcavatedLengthSuma = ExcavatedLengthSuma + folding.ExcavatedLength;
                InstalledLengthSuma = InstalledLengthSuma + folding.InstalledLength;
                RefilledLengthSuma = RefilledLengthSuma + folding.RefilledLength;
                GranularBaseLengthSuma = GranularBaseLengthSuma + folding.GranularBaseLength;
                if (folding.ExcavatedLength != 0)
                    CantExcavation++;
                if (folding.InstalledLength != 0)
                    CantInstallation++;
                if (folding.RefilledLength != 0)
                    CantFilled++;
            }

            var EFormula = 0.0;
            var IFormula = 0.0;
            var FFormula = 0.0;
            var Status = "En Ejecución";

            if (foldingTodos.Count != 0)
            {
                if(CantExcavation != 0)
                    EFormula = Math.Round(ExcavatedLengthSuma / CantExcavation, 2);
                if (CantInstallation != 0)
                    IFormula = Math.Round(InstalledLengthSuma / CantInstallation, 2);
                if (CantFilled != 0)
                    FFormula = Math.Round(RefilledLengthSuma / CantFilled, 2);
            }

            pdp.ExcavatedLength = ExcavatedLengthSuma;
            pdp.InstalledLength = InstalledLengthSuma;
            pdp.RefilledLength = RefilledLengthSuma;
            pdp.GranularBaseLength = GranularBaseLengthSuma;
            pdp.Excavation = EFormula;
            pdp.Installation = IFormula;
            pdp.Filled = FFormula;
            pdp.Status = Status;

            var sewerManifoldRefs = await _context.SewerManifoldReferences
                .Include(y => y.SewerManifoldReview)
                .Include(y => y.SewerManifoldExecution)
                .FirstOrDefaultAsync(x => x.SewerManifoldExecution.ProductionDailyPartId == pdp.Id);

            if ((decimal)GranularBaseLengthSuma == (decimal)sewerManifoldRefs.SewerManifoldReview.LengthOfDigging)
                pdp.Status = "Ejecutada";

            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}