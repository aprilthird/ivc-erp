using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.WEB.Areas.Quality.ViewModels.FoldingFor37AViewModels;
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

namespace IVC.PE.WEB.Areas.Quality.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Quality.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.QUALITY)]
    [Route("calidad/foldingfor37A-for37A-colector-descarga")]
    public class FoldingFor37AController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public FoldingFor37AController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<FoldingFor05Controller> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? SewerManifoldFor37AId = null)
        {
            if (!SewerManifoldFor37AId.HasValue)
                return Ok(new List<FoldingFor37AViewModel>());

            var foldingFor37As = await _context.FoldingFor37As
                .Where(x => x.SewerManifoldFor37AId == SewerManifoldFor37AId)
                .OrderBy(x=>x.MeetingNumber)
                .Select(x => new FoldingFor37AViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor37AId = x.SewerManifoldFor37AId,
                    WeldingType = x.WeldingType,
                    MeetingNumber = x.MeetingNumber,
                    Date = x.Date.ToDateString()
                }).ToListAsync();

            return Ok(foldingFor37As);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var foldingFor37A = await _context.FoldingFor37As
                .Where(x => x.Id == id)
                .Select(x => new FoldingFor37AViewModel
                {
                    Id = x.Id,
                    SewerManifoldFor37AId = x.SewerManifoldFor37AId,
                    WeldingType = x.WeldingType,
                    MeetingNumber = x.MeetingNumber,
                    Date = x.Date.ToDateString()
                }).FirstOrDefaultAsync();

            return Ok(foldingFor37A);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FoldingFor37AViewModel model)
        {
            var folding37A = await _context.FoldingFor37As
                .Where(x => x.SewerManifoldFor37AId == model.SewerManifoldFor37AId).ToListAsync();

            var countInit = folding37A.Count();

            for (int i = 0; i < countInit; i++)
            {
                if (model.MeetingNumber == folding37A[i].MeetingNumber)
                    return BadRequest("El número de reunión ya existe");
            }

            if (model.MeetingNumber == 0)
                return BadRequest("No se ha ingresado el N°  de junta");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Date == null || model.Date == string.Empty)
                return BadRequest("No se ha ingresado la fecha");


            var foldingFor37A = new FoldingFor37A
            {
                SewerManifoldFor37AId = model.SewerManifoldFor37AId,
                WeldingType = model.WeldingType,
                MeetingNumber = model.MeetingNumber,
                Date = model.Date.ToDateTime()
            };

            await _context.FoldingFor37As.AddAsync(foldingFor37A);
            await _context.SaveChangesAsync();

            //-----------------------------------------------

            var for37A = await _context.SewerManifoldFor37As.FirstOrDefaultAsync(x => x.Id == model.SewerManifoldFor37AId);

            var folding = await _context.FoldingFor37As.Where(x => x.SewerManifoldFor37AId == model.SewerManifoldFor37AId).ToListAsync();
            var count = folding.Count();
            var Inicio = folding.FirstOrDefault().Date;
            var Fin = folding.LastOrDefault().Date;

            for (int i = 0; i < count; i++)
            {
                if (Inicio > folding[i].Date)
                    Inicio = folding[i].Date;

                if (Fin < folding[i].Date)
                    Fin = folding[i].Date;
            }

            var formulaTermoNumber = folding.Where(x => x.WeldingType == 1).Count();
            var formulaElectroTubNumber = folding.Where(x => x.WeldingType == 2).Count();
            var formulaElectroPasNumber = folding.Where(x => x.WeldingType == 3).Count();

            for37A.StartElectrofusionDate = Inicio;
            for37A.EndElectrofusionDate = Fin;
            for37A.HotMeltsNumber = formulaTermoNumber;
            for37A.ElectrofusionsNumber = formulaElectroTubNumber;
            for37A.ElectrofusionsPasNumber = formulaElectroPasNumber;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FoldingFor37AViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Date == null || model.Date == "")
                return BadRequest("No se ha ingresado la fecha");

            var foldingFor37A = await _context.FoldingFor37As
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            foldingFor37A.WeldingType = model.WeldingType;
            foldingFor37A.MeetingNumber = model.MeetingNumber;
            foldingFor37A.Date = model.Date.ToDateTime();

            await _context.SaveChangesAsync();

            //-----------------------------------------------

            var for37A = await _context.SewerManifoldFor37As.FirstOrDefaultAsync(x => x.Id == model.SewerManifoldFor37AId);

            var folding = await _context.FoldingFor37As.Where(x => x.SewerManifoldFor37AId == model.SewerManifoldFor37AId).ToListAsync();
            var count = folding.Count();
            var Inicio = folding.FirstOrDefault().Date;
            var Fin = folding.LastOrDefault().Date;

            for (int i = 0; i < count; i++)
            {
                if (Inicio > folding[i].Date)
                    Inicio = folding[i].Date;

                if (Fin < folding[i].Date)
                    Fin = folding[i].Date;
            }

            var formulaTermoNumber = folding.Where(x => x.WeldingType == 1).Count();
            var formulaElectroTubNumber = folding.Where(x => x.WeldingType == 2).Count();
            var formulaElectroPasNumber = folding.Where(x => x.WeldingType == 3).Count();

            for37A.StartElectrofusionDate = Inicio;
            for37A.EndElectrofusionDate = Fin;
            for37A.HotMeltsNumber = formulaTermoNumber;
            for37A.ElectrofusionsNumber = formulaElectroTubNumber;
            for37A.ElectrofusionsPasNumber = formulaElectroPasNumber;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var foldingFor37A = await _context.FoldingFor37As
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (foldingFor37A == null)
                return BadRequest($"Folding For37A con Id '{id}' no se halló.");

            _context.FoldingFor37As.Remove(foldingFor37A);
            await _context.SaveChangesAsync();

            //-----------------------------------------------

            var for37A = await _context.SewerManifoldFor37As.FirstOrDefaultAsync(x => x.Id == foldingFor37A.SewerManifoldFor37AId);

            var folding = await _context.FoldingFor37As.Where(x => x.SewerManifoldFor37AId == foldingFor37A.SewerManifoldFor37AId).ToListAsync();
            var count = folding.Count();
            var Inicio = folding.FirstOrDefault().Date;
            var Fin = folding.LastOrDefault().Date;

            for (int i = 0; i < count; i++)
            {
                if (Inicio > folding[i].Date)
                    Inicio = folding[i].Date;

                if (Fin < folding[i].Date)
                    Fin = folding[i].Date;
            }

            var formulaTermoNumber = folding.Where(x => x.WeldingType == 1).Count();
            var formulaElectroTubNumber = folding.Where(x => x.WeldingType == 2).Count();
            var formulaElectroPasNumber = folding.Where(x => x.WeldingType == 3).Count();

            for37A.StartElectrofusionDate = Inicio;
            for37A.EndElectrofusionDate = Fin;
            for37A.HotMeltsNumber = formulaTermoNumber;
            for37A.ElectrofusionsNumber = formulaElectroTubNumber;
            for37A.ElectrofusionsPasNumber = formulaElectroPasNumber;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
