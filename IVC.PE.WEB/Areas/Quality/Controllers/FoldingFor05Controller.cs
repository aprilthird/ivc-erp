
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.FoldingFor05ViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor05ViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/foldingfor05-for05-colector-descarga")]
    public class FoldingFor05Controller : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public FoldingFor05Controller(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<FoldingFor05Controller> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? SewerManifoldFor05Id = null)
        {
            if (!SewerManifoldFor05Id.HasValue)
                return Ok(new List<FoldingFor05ViewModel>());

            var foldingsFor05s = await _context.FoldingFor05s
                .Include(x => x.SewerManifoldFor05)
                .Include(x => x.FillingLaboratoryTest)
                .Where(x => x.SewerManifoldFor05Id == SewerManifoldFor05Id)
                .Select(x => new FoldingFor05ViewModel
                {
                    Id = x.Id,
                    LayerNumber = x.LayerNumber,
                    TestDate = x.TestDate.ToDateString(),
                    SewerManifoldFor05Id = x.SewerManifoldFor05Id,
                    FillingLaboratoryTest = new FillingLaboratoryTestViewModel
                    {
                        RecordNumber = x.FillingLaboratoryTest.RecordNumber,
                        MaxDensity = x.FillingLaboratoryTest.MaxDensity,
                        OptimumMoisture = x.FillingLaboratoryTest.OptimumMoisture
                    },
                    WetDensity = x.WetDensity.ToString(),
                    MoisturePercentage = x.MoisturePercentage.ToString(),
                    DryDensity = x.DryDensity.ToString(),
                    PercentageRequiredCompaction = x.PercentageRequiredCompaction.ToString(),
                    PercentageRealCompaction = x.PercentageRealCompaction.ToString(),
                    Status = x.Status
                }).ToListAsync();

            return Ok(foldingsFor05s);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var foldingFor05 = await _context.FoldingFor05s
                .Include(x => x.SewerManifoldFor05)
                .Include(x => x.FillingLaboratoryTest)
                .Where(x => x.Id == id)
                .Select(x => new FoldingFor05ViewModel
                {
                    Id = x.Id,
                    LayerNumber = x.LayerNumber,
                    TestDate = x.TestDate.ToDateString(),
                    SewerManifoldFor05Id = x.SewerManifoldFor05Id,
                    FillingLaboratoryTestId = x.FillingLaboratoryTestId,
                    FillingLaboratoryTest = new FillingLaboratoryTestViewModel
                    {
                        RecordNumber = x.FillingLaboratoryTest.RecordNumber,
                        MaxDensity = x.FillingLaboratoryTest.MaxDensity,
                        OptimumMoisture = x.FillingLaboratoryTest.OptimumMoisture,
                        SerialNumber = x.FillingLaboratoryTest.SerialNumber
                    },
                    WetDensity = x.WetDensity.ToString(),
                    MoisturePercentage = x.MoisturePercentage.ToString(),
                    DryDensity = x.DryDensity.ToString(),
                    PercentageRequiredCompaction = x.PercentageRequiredCompaction.ToString(),
                    PercentageRealCompaction = x.PercentageRealCompaction.ToString(),
                    Status = x.Status
                }).FirstOrDefaultAsync();

            return Ok(foldingFor05);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FoldingFor05ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.TestDate == null || model.TestDate == "")
                return BadRequest("No se ha ingresado la fecha");

            var FLT = await _context.FillingLaboratoryTests
                .FirstOrDefaultAsync(x => x.Id == model.FillingLaboratoryTestId);

            var FormulaDryDensity = model.WetDensity.ToDoubleString() / (1 + (model.MoisturePercentage.ToDoubleString() / 100));
            var FormulaPercentageRealCompaction = (FormulaDryDensity / FLT.MaxDensity) * 100;

            var FormulaStatus = "Aprobado";
            var FijoPercentageRequiredCompaction = 95;
            if (model.LayerNumber == "Base" || model.LayerNumber == "BASE")
                FijoPercentageRequiredCompaction = 100;
            if (FormulaPercentageRealCompaction < FijoPercentageRequiredCompaction)
                FormulaStatus = "No Aprobado";

            var foldingFor05 = new FoldingFor05
            {
                SewerManifoldFor05Id = model.SewerManifoldFor05Id,
                FillingLaboratoryTestId = model.FillingLaboratoryTestId,
                LayerNumber = model.LayerNumber,
                TestDate = model.TestDate.ToDateTime(),
                WetDensity = model.WetDensity.ToDoubleString(),
                MoisturePercentage = model.MoisturePercentage.ToDoubleString(),
                DryDensity = Math.Round(FormulaDryDensity,3),
                PercentageRequiredCompaction = FijoPercentageRequiredCompaction,
                PercentageRealCompaction = Math.Round(FormulaPercentageRealCompaction,1),
                Status = FormulaStatus
            };

            await _context.FoldingFor05s.AddAsync(foldingFor05);
            await _context.SaveChangesAsync();
            //--------------------------------------------------------
            var LNumbers = await _context.FoldingFor05s.Where(x => x.SewerManifoldFor05Id == model.SewerManifoldFor05Id).ToListAsync();
            var cant = LNumbers.Count();
            var max = 0.0;
            var lastDate = LNumbers[0].TestDate;
            var BaseExist = false;
            var status = "Aprobado";
            for (int i = 0;i < cant; i++)
            {
                if (LNumbers[i].LayerNumber != "Base" && LNumbers[i].LayerNumber != "BASE")
                {
                    if (max < LNumbers[i].LayerNumber.ToDoubleString())
                    {
                        max = LNumbers[i].LayerNumber.ToDoubleString();
                    }
                }
                else
                    BaseExist = true;

                if (LNumbers[i].TestDate > lastDate)
                    lastDate = LNumbers[i].TestDate;

                if (LNumbers[i].Status == "No Aprobado")
                    status = "No Aprobado";
            }

            var For05 = await _context.SewerManifoldFor05s.Where(x => x.Id == model.SewerManifoldFor05Id).FirstOrDefaultAsync();
            if (BaseExist == true)
            {
                if (max != 0.0)
                    For05.LayerNumber = max.ToString() + " + Base";
                else
                    For05.LayerNumber = "Base";
            }
            else
                For05.LayerNumber = max.ToString();
            For05.TestDate = lastDate;
            For05.Status = status;
            For05.LayersNumber = cant;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FoldingFor05ViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.TestDate == null || model.TestDate == "")
                return BadRequest("No se ha ingresado la fecha");

            var FLT = await _context.FillingLaboratoryTests
                .FirstOrDefaultAsync(x => x.Id == model.FillingLaboratoryTestId);

            var FormulaDryDensity = model.WetDensity.ToDoubleString() / (1 + (model.MoisturePercentage.ToDoubleString() / 100));

            var FormulaPercentageRealCompaction = (FormulaDryDensity / FLT.MaxDensity) * 100;

            var FormulaStatus = "Aprobado";
            var FijoPercentageRequiredCompaction = 95;
            if (model.LayerNumber == "Base" || model.LayerNumber == "BASE")
                FijoPercentageRequiredCompaction = 100;
            if (FormulaPercentageRealCompaction < FijoPercentageRequiredCompaction)
                FormulaStatus = "No Aprobado";

            var foldingFor05 = await _context.FoldingFor05s
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            foldingFor05.LayerNumber = model.LayerNumber;
            foldingFor05.TestDate = model.TestDate.ToDateTime();
            foldingFor05.WetDensity = model.WetDensity.ToDoubleString();
            foldingFor05.MoisturePercentage = model.MoisturePercentage.ToDoubleString();
            foldingFor05.DryDensity = Math.Round(FormulaDryDensity,3);
            foldingFor05.PercentageRequiredCompaction = FijoPercentageRequiredCompaction;
            foldingFor05.PercentageRealCompaction = Math.Round(FormulaPercentageRealCompaction,1);
            foldingFor05.Status = FormulaStatus;

            await _context.SaveChangesAsync();
            //-----------------------------------------------------------
            var LNumbers = await _context.FoldingFor05s.Where(x => x.SewerManifoldFor05Id == model.SewerManifoldFor05Id).ToListAsync();
            var cant = LNumbers.Count();
            var max = 0.0;
            var lastDate = LNumbers[0].TestDate;
            var BaseExist = false;
            var status = "Aprobado";
            for (int i = 0; i < cant; i++)
            {
                if (LNumbers[i].LayerNumber != "Base" && LNumbers[i].LayerNumber != "BASE")
                {
                    if (max < LNumbers[i].LayerNumber.ToDoubleString())
                    {
                        max = LNumbers[i].LayerNumber.ToDoubleString();
                    }
                }
                else
                    BaseExist = true;

                if (LNumbers[i].TestDate > lastDate)
                    lastDate = LNumbers[i].TestDate;

                if (LNumbers[i].Status == "No Aprobado")
                    status = "No Aprobado";
            }

            var For05 = await _context.SewerManifoldFor05s.Where(x => x.Id == model.SewerManifoldFor05Id).FirstOrDefaultAsync();
            if (BaseExist == true)
                if (max != 0.0)
                    For05.LayerNumber = max.ToString() + " + Base";
                else
                    For05.LayerNumber = "Base";
            else
                For05.LayerNumber = max.ToString();
            For05.TestDate = lastDate;
            For05.Status = status;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var foldingFor05 = await _context.FoldingFor05s
                .FirstOrDefaultAsync(x => x.Id == id);

            var aux = foldingFor05.SewerManifoldFor05Id;

            if (foldingFor05 == null)
                return BadRequest($"Folding For05 con Id '{id}' no se halló.");

            _context.FoldingFor05s.Remove(foldingFor05);
            await _context.SaveChangesAsync();
            //---------------------------------------------------------------------
            var foldingAux = await _context.FoldingFor05s
                .Where(x => x.SewerManifoldFor05Id == foldingFor05.SewerManifoldFor05Id)
                .ToListAsync();

            //---------------------------------------------------------------------
            if (foldingAux.Count() != 0)
            {
                var ent = foldingAux.LastOrDefault();
                var LNumbers = await _context.FoldingFor05s.Where(x => x.SewerManifoldFor05Id == ent.SewerManifoldFor05Id).ToListAsync();
                var cant = LNumbers.Count();
                var max = 0.0;
                var lastDate = LNumbers[0].TestDate;
                var BaseExist = false;
                var status = "Aprobado";
                for (int i = 0; i < cant; i++)
                {
                    if (LNumbers[i].LayerNumber != "Base" && LNumbers[i].LayerNumber != "BASE")
                    {
                        if (max < LNumbers[i].LayerNumber.ToDoubleString())
                        {
                            max = LNumbers[i].LayerNumber.ToDoubleString();
                        }
                    }
                    else
                        BaseExist = true;

                    if (LNumbers[i].TestDate > lastDate)
                        lastDate = LNumbers[i].TestDate;

                    if (LNumbers[i].Status == "No Aprobado")
                        status = "No Aprobado";
                }

                var For05 = await _context.SewerManifoldFor05s.Where(x => x.Id == ent.SewerManifoldFor05Id).FirstOrDefaultAsync();
                if (BaseExist == true)
                    if (max != 0.0)
                        For05.LayerNumber = max.ToString() + " + Base";
                    else
                        For05.LayerNumber = "Base";
                else
                    For05.LayerNumber = max.ToString();
                For05.TestDate = lastDate;
                For05.Status = status;
                For05.LayersNumber = cant;

                await _context.SaveChangesAsync();
            }
            else
            {
                var For05 = await _context.SewerManifoldFor05s.Where(x => x.Id == aux).FirstOrDefaultAsync();
                For05.LayerNumber = string.Empty;
                For05.TestDate = new DateTime();
                For05.Status = string.Empty;
                For05.LayersNumber = 0;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var folding = await _context.FoldingFor05s.ToListAsync();

            var data = folding.Count();

            return Ok(data);
        }
    }
}