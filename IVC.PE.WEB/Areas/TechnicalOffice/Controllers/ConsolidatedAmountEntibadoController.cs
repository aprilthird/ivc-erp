using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedAmountEntibadoViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/monto-consolidado-entibados")]
    public class ConsolidatedAmountEntibadoController : BaseController
    {
        public ConsolidatedAmountEntibadoController(IvcDbContext context,
       ILogger<ConsolidatedAmountEntibadoController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId);
            if (projectPhaseId.HasValue)
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);

            var entibados = await query
                .Include(x => x.WorkFront)
                .OrderBy(x => x.OrderNumber)
                .Select(x => new ConsolidatedAmountEntibadoViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    Performance = x.Performance.ToString("N", CultureInfo.InvariantCulture),
                    KS60xMinibox = x.KS60xMinibox.ToString("N", CultureInfo.InvariantCulture),
                    KS100xKMC100 = x.KS100xKMC100.ToString("N", CultureInfo.InvariantCulture),
                    RealzaxExtension = x.RealzaxExtension.ToString("N", CultureInfo.InvariantCulture),
                    Corredera = x.Corredera.ToString("N", CultureInfo.InvariantCulture),
                    Paralelo = x.Paralelo.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .ToListAsync();

            return Ok(entibados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = await _context.ConsolidatedAmountEntibados
                .Select(x => new ConsolidatedAmountEntibadoViewModel
                {
                    Id = x.Id,
                    BudgetTitleId = x.BudgetTitleId,
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectPhaseId = x.ProjectPhaseId,
                    WorkFrontId = x.WorkFrontId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    Performance = x.Performance.ToString("N", CultureInfo.InvariantCulture),
                    KS60xMinibox = x.KS60xMinibox.ToString("N", CultureInfo.InvariantCulture),
                    KS100xKMC100 = x.KS100xKMC100.ToString("N", CultureInfo.InvariantCulture),
                    RealzaxExtension = x.RealzaxExtension.ToString("N", CultureInfo.InvariantCulture),
                    Corredera = x.Corredera.ToString("N", CultureInfo.InvariantCulture),
                    Paralelo = x.Paralelo.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(query);
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, ConsolidatedAmountEntibadoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entibado = await _context.ConsolidatedAmountEntibados.FirstOrDefaultAsync(x => x.Id == id);

            entibado.ItemNumber = model.ItemNumber;
            entibado.Description = model.Description;
            entibado.Unit = model.Unit;
            if (model.Unit == null)
                entibado.Unit = "";
            entibado.Metered = model.Metered.ToDoubleString();
            entibado.Performance = model.Performance.ToDoubleString();
            entibado.KS60xMinibox = model.KS60xMinibox.ToDoubleString();
            entibado.KS100xKMC100 = model.KS100xKMC100.ToDoubleString();
            entibado.RealzaxExtension = model.RealzaxExtension.ToDoubleString();
            entibado.Corredera = model.Corredera.ToDoubleString();
            entibado.Paralelo = model.Paralelo.ToDoubleString();

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entibado = await _context.ConsolidatedAmountEntibados.FirstOrDefaultAsync(x => x.Id == id);

            if (entibado == null)
                return BadRequest("No se ha encontrado el acero");

            _context.ConsolidatedAmountEntibados.Remove(entibado);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("cargar")]
        public async Task<IActionResult> Load()
        {
            var consolidados = new List<ConsolidatedAmountEntibado>();

            var steels = await _context.ConsolidatedEntibados.Include(x => x.ProjectFormula).Include(x => x.BudgetTitle).OrderBy(x => x.OrderNumber).ToListAsync();

            var projectFormulas = await _context.ProjectFormulas.ToListAsync();

            var count = _context.ConsolidatedAmountEntibados.Count();

            var ks60 = await _context.EntibadoVariables.Include(x => x.Supply).Include(x => x.BudgetInput).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "ALQUILER DE ENTIBADOS KS60/MINIBOX");
            var ks100 = await _context.EntibadoVariables.Include(x => x.Supply).Include(x => x.BudgetInput).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "ALQUILER ENTIBADO KS100/KMC 100");
            var realzaExtension = await _context.EntibadoVariables.Include(x => x.Supply).Include(x => x.BudgetInput).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "ALQUILER ENTIBADO REALZA/EXTENSION");
            var corredera = await _context.EntibadoVariables.Include(x => x.Supply).Include(x => x.BudgetInput).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "ALQUILER ENTIBADO CORREDERA");
            var paralelo = await _context.EntibadoVariables.Include(x => x.Supply).Include(x => x.BudgetInput).AsNoTracking().FirstOrDefaultAsync(x => x.Supply.Description == "ALQUILER ENTIBADO PARALELO");

            if (ks60 == null)
                return BadRequest("No se ha hallado la variable ALQUILER DE ENTIBADOS KS60/MINIBOX");
            if (ks100 == null)
                return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO KS100/KMC 100");
            if (realzaExtension == null)
                return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO KS100/KMC 100");
            if (corredera == null)
                return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO CORREDERA");
            if (paralelo == null)
                return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO PARALELO");

            foreach (var item in steels)
            {

                var consolidatedEntibado = new ConsolidatedAmountEntibado();

                var existe = await _context.ConsolidatedAmountEntibados.FirstOrDefaultAsync(x => x.ItemNumber == item.ItemNumber && x.BudgetTitleId == item.BudgetTitleId);

                if (item.Unit != "")
                {

                    var formula = projectFormulas.FirstOrDefault(x => x.Code == item.ProjectFormula.Code);

                    if (existe == null)
                    {
                        consolidatedEntibado.Id = Guid.NewGuid();
                        consolidatedEntibado.BudgetTitleId = item.BudgetTitleId;
                        consolidatedEntibado.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedEntibado.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedEntibado.WorkFrontId = item.WorkFrontId;
                        consolidatedEntibado.OrderNumber = count;
                        consolidatedEntibado.ItemNumber = item.ItemNumber;
                        consolidatedEntibado.Description = item.Description;
                        consolidatedEntibado.Unit = item.Unit;
                        consolidatedEntibado.Metered = item.Metered;
                        if (item.KS60xMinibox != 0)
                            consolidatedEntibado.KS60xMinibox = Math.Round(item.Metered * ks60.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (item.KS100xKMC100 != 0)
                            consolidatedEntibado.KS100xKMC100 = Math.Round(item.Metered * ks100.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (item.RealzaxExtension != 0)
                            consolidatedEntibado.RealzaxExtension = Math.Round(item.Metered * realzaExtension.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (item.Corredera != 0)
                            consolidatedEntibado.Corredera = Math.Round(item.Metered * corredera.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        if (item.Paralelo != 0)
                            consolidatedEntibado.Paralelo = Math.Round(item.Metered * paralelo.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);

                        consolidados.Add(consolidatedEntibado);
                        count++;
                    }
                    else
                    {
                        existe.Metered = item.Metered;
                        if (item.KS60xMinibox != 0)
                            existe.KS60xMinibox = Math.Round(item.Metered * ks60.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        else
                            existe.KS60xMinibox = 0;

                        if (item.KS100xKMC100 != 0)
                            existe.KS100xKMC100 = Math.Round(item.Metered * ks100.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        else
                            existe.KS100xKMC100 = 0;

                        if (item.RealzaxExtension != 0)
                            existe.RealzaxExtension = Math.Round(item.Metered * realzaExtension.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        else
                            existe.RealzaxExtension = 0;

                        if (item.Corredera != 0)
                            existe.Corredera = Math.Round(item.Metered * corredera.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        else
                            existe.Corredera = 0;

                        if (item.Paralelo != 0)
                            existe.Paralelo = Math.Round(item.Metered * paralelo.BudgetInput.SaleUnitPrice, 2, MidpointRounding.AwayFromZero);
                        else
                            existe.Paralelo = 0;
                    }
                }
                else
                {
                    if (existe == null)
                    {
                        consolidatedEntibado.Id = Guid.NewGuid();
                        consolidatedEntibado.BudgetTitleId = item.BudgetTitleId;
                        consolidatedEntibado.ProjectFormulaId = item.ProjectFormulaId;
                        consolidatedEntibado.ProjectPhaseId = item.ProjectPhaseId;
                        consolidatedEntibado.WorkFrontId = item.WorkFrontId;
                        consolidatedEntibado.OrderNumber = count;
                        consolidatedEntibado.ItemNumber = item.ItemNumber;
                        consolidatedEntibado.Description = item.Description;
                        consolidatedEntibado.Unit = item.Unit;

                        consolidados.Add(consolidatedEntibado);
                        count++;
                    }
                }
            }
            if (consolidados.Count != 0)
                await _context.ConsolidatedAmountEntibados.AddRangeAsync(consolidados);
            //_context.ConsolidatedAmountEntibados.UpdateRange(consolidadosUpdate);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("metrado-ks60")]
        public async Task<IActionResult> GetKS60Suma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var lista = new List<string>();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var query2 = _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var ks60Suma = 0.0;
            var ks60Suma2 = 0.0;

            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
                query2 = query2.Where(x => x.ProjectFormulaId == projectFormulaId);
            }
            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
                query2 = query2.Where(x => x.BudgetTitleId == budgetTitleId);
            }
            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId);
                query2 = query2.Where(x => x.WorkFrontId == workFrontId);
            }
            if (projectPhaseId.HasValue)
            {
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
                query2 = query2.Where(x => x.ProjectPhaseId == projectPhaseId);
            }

            var data = await query.ToListAsync();
            var data2 = await query2.ToListAsync();

            foreach (var item in data)
            {
                ks60Suma += item.KS60xMinibox;
            }

            foreach (var item in data2)
            {
                ks60Suma2 += item.KS60xMinibox;
            }

            lista.Add(ks60Suma.ToString("N2", CultureInfo.InvariantCulture));

            var op = 0.0;

            if (ks60Suma2 != 0)
                op = Math.Round(ks60Suma / ks60Suma2, 2);

            lista.Add(op.ToString("N2", CultureInfo.InvariantCulture));

            return Ok(lista);
        }

        [HttpGet("metrado-ks100")]
        public async Task<IActionResult> GetKS100Suma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var lista = new List<string>();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var query2 = _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var ks100Suma = 0.0;
            var ks100Suma2 = 0.0;

            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
                query2 = query2.Where(x => x.ProjectFormulaId == projectFormulaId);
            }
            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
                query2 = query2.Where(x => x.BudgetTitleId == budgetTitleId);
            }
            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId);
                query2 = query2.Where(x => x.WorkFrontId == workFrontId);
            }
            if (projectPhaseId.HasValue)
            {
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
                query2 = query2.Where(x => x.ProjectPhaseId == projectPhaseId);
            }

            var data = await query.ToListAsync();
            var data2 = await query2.ToListAsync();

            foreach (var item in data)
            {
                ks100Suma += item.KS100xKMC100;
            }

            foreach (var item in data2)
            {
                ks100Suma2 += item.KS100xKMC100;
            }

            lista.Add(ks100Suma.ToString("N2", CultureInfo.InvariantCulture));

            var op = 0.0;

            if (ks100Suma2 != 0)
                op = Math.Round(ks100Suma / ks100Suma2, 2);

            lista.Add(op.ToString("N2", CultureInfo.InvariantCulture));

            return Ok(lista);
        }

        [HttpGet("metrado-realza-extension")]
        public async Task<IActionResult> GetRealzaExtensionSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var lista = new List<string>();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var query2 = _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var realzaExtensionSuma = 0.0;
            var realzaExtensionSuma2 = 0.0;

            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
                query2 = query2.Where(x => x.ProjectFormulaId == projectFormulaId);
            }
            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
                query2 = query2.Where(x => x.BudgetTitleId == budgetTitleId);
            }
            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId);
                query2 = query2.Where(x => x.WorkFrontId == workFrontId);
            }
            if (projectPhaseId.HasValue)
            {
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
                query2 = query2.Where(x => x.ProjectPhaseId == projectPhaseId);
            }

            var data = await query.ToListAsync();
            var data2 = await query2.ToListAsync();

            foreach (var item in data)
            {
                realzaExtensionSuma += item.RealzaxExtension;
            }

            foreach (var item in data2)
            {
                realzaExtensionSuma2 += item.RealzaxExtension;
            }

            lista.Add(realzaExtensionSuma.ToString("N2", CultureInfo.InvariantCulture));

            var op = 0.0;

            if (realzaExtensionSuma2 != 0)
                op = Math.Round(realzaExtensionSuma / realzaExtensionSuma2, 2);

            lista.Add(op.ToString("N2", CultureInfo.InvariantCulture));

            return Ok(lista);
        }

        [HttpGet("metrado-corredera")]
        public async Task<IActionResult> GetCorrederaSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var lista = new List<string>();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var query2 = _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var correderaSuma = 0.0;
            var correderaSuma2 = 0.0;

            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
                query2 = query2.Where(x => x.ProjectFormulaId == projectFormulaId);
            }
            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
                query2 = query2.Where(x => x.BudgetTitleId == budgetTitleId);
            }
            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId);
                query2 = query2.Where(x => x.WorkFrontId == workFrontId);
            }
            if (projectPhaseId.HasValue)
            {
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
                query2 = query2.Where(x => x.ProjectPhaseId == projectPhaseId);
            }

            var data = await query.ToListAsync();
            var data2 = await query2.ToListAsync();

            foreach (var item in data)
            {
                correderaSuma += item.Corredera;
            }

            foreach (var item in data2)
            {
                correderaSuma2 += item.Corredera;
            }

            lista.Add(correderaSuma.ToString("N2", CultureInfo.InvariantCulture));

            var op = 0.0;

            if (correderaSuma2 != 0)
                op = Math.Round(correderaSuma / correderaSuma2, 2);

            lista.Add(op.ToString("N2", CultureInfo.InvariantCulture));

            return Ok(lista);
        }

        [HttpGet("metrado-paralelo")]
        public async Task<IActionResult> GetParaleloSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var lista = new List<string>();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var query2 = _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var paraleloSuma = 0.0;
            var paraleloSuma2 = 0.0;

            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
                query2 = query2.Where(x => x.ProjectFormulaId == projectFormulaId);
            }
            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
                query2 = query2.Where(x => x.BudgetTitleId == budgetTitleId);
            }
            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId);
                query2 = query2.Where(x => x.WorkFrontId == workFrontId);
            }
            if (projectPhaseId.HasValue)
            {
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
                query2 = query2.Where(x => x.ProjectPhaseId == projectPhaseId);
            }

            var data = await query.ToListAsync();
            var data2 = await query2.ToListAsync();

            foreach (var item in data)
            {
                paraleloSuma += item.Paralelo;
            }

            foreach (var item in data2)
            {
                paraleloSuma2 += item.Paralelo;
            }

            lista.Add(paraleloSuma.ToString("N2", CultureInfo.InvariantCulture));

            var op = 0.0;

            if (paraleloSuma2 != 0)
                op = Math.Round(paraleloSuma / paraleloSuma2, 2);

            lista.Add(op.ToString("N2", CultureInfo.InvariantCulture));

            return Ok(lista);
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotalSuma(Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null, Guid? projectPhaseId = null)
        {
            var pId = GetProjectId();

            var lista = new List<string>();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var query2 = _context.ConsolidatedEntibados
               .Include(x => x.ProjectFormula)
               .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "");

            var totalSuma = 0.0;
            var totalSuma2 = 0.0;

            if (projectFormulaId.HasValue)
            {
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
                query2 = query2.Where(x => x.ProjectFormulaId == projectFormulaId);
            }
            if (budgetTitleId.HasValue)
            {
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
                query2 = query2.Where(x => x.BudgetTitleId == budgetTitleId);
            }
            if (workFrontId.HasValue)
            {
                query = query.Where(x => x.WorkFrontId == workFrontId);
                query2 = query2.Where(x => x.WorkFrontId == workFrontId);
            }
            if (projectPhaseId.HasValue)
            {
                query = query.Where(x => x.ProjectPhaseId == projectPhaseId);
                query2 = query2.Where(x => x.ProjectPhaseId == projectPhaseId);
            }

            var data = await query.ToListAsync();
            var data2 = await query2.ToListAsync();

            foreach (var item in data)
            {
                totalSuma += item.KS60xMinibox + item.KS100xKMC100 + item.RealzaxExtension + item.Corredera + item.Paralelo;
            }

            foreach (var item in data2)
            {
                totalSuma2 += item.KS60xMinibox + item.KS100xKMC100 + item.RealzaxExtension + item.Corredera + item.Paralelo;
            }

            lista.Add(totalSuma.ToString("N2", CultureInfo.InvariantCulture));

            var op = 0.0;

            if (totalSuma2 != 0)
                op = Math.Round(totalSuma / totalSuma2, 2);

            lista.Add(op.ToString("N2", CultureInfo.InvariantCulture));

            return Ok(lista);
        }

        [HttpDelete("eliminar-filtro")]
        public async Task<IActionResult> DeleteByFilters(Guid? projectFormulaId = null, Guid? budgetTitleId = null)
        {
            var pId = GetProjectId();

            var query = _context.ConsolidatedAmountEntibados
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId);

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);
            else
                return BadRequest("No se ha escogido el título de presupuesto");

            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId);
            else
                return BadRequest("No se ha escogido la fórmula");

            var data = await query.ToListAsync();

            _context.ConsolidatedAmountEntibados.RemoveRange(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
