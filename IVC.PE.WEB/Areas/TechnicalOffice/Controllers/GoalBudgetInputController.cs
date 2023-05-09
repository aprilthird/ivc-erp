using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/meta-insumos")]
    public class GoalBudgetInputController : BaseController
    {
        public GoalBudgetInputController(IvcDbContext context,
            ILogger<GoalBudgetInputController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? measurementUnitId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
            Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString();
            var currentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]);
            var recordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]);

            var query = _context.GoalBudgetInputs
                .Include(x => x.Supply)
                .Include(x => x.WorkFront)
                .Include(x => x.BudgetTitle)
                .Include(x => x.MeasurementUnit)
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .OrderBy(x => x.OrderNumber)
                .AsQueryable();

            var items = await _context.RequestItems
                .Include(x => x.Request)
                .Include(x => x.Supply)
                .Where(x => x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C
                || x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S)
                .ToListAsync();

            if (measurementUnitId.HasValue)
                query = query.Where(x => x.MeasurementUnitId == measurementUnitId.Value);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);

            var totalRecords = query.Count();

            var aux = query.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                aux = aux.Where(x => x.Supply.Description.Contains(search) ||
                    x.Supply.SupplyFamily.Name.Contains(search) ||
                    x.Supply.SupplyGroup.Name.Contains(search) || 
                    x.Supply.FullCode.Contains(search)).ToList();
            }

            var metered = aux.Sum(x => x.Metered);
            var parcial = aux.Sum(x => x.Parcial);

            var data = aux
                .Skip(currentNumber)
                .Take(recordsPerPage)
                .Select(x => new GoalBudgetInputViewModel
                {
                    Id = x.Id,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    SupplyId = x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description,
                        SupplyFamilyId = x.Supply.SupplyFamilyId,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = x.Supply.SupplyFamily.Code,
                            Name = x.Supply.SupplyFamily.Name
                        },
                        SupplyGroupId = x.Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = x.Supply.SupplyGroup.Code,
                            Name = x.Supply.SupplyGroup.Name
                        },
                        CorrelativeCode = x.Supply.CorrelativeCode
                    },
                    MeasurementUnitId = x.MeasurementUnitId,
                    MeasurementUnit = new MeasurementUnitViewModel
                    {
                        Abbreviation = x.MeasurementUnit.Abbreviation
                    },
                    Metered = x.Metered.ToString("N2", CultureInfo.InvariantCulture),
                    CurrentMetered = x.CurrentMetered.ToString("N2", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N2", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N2", CultureInfo.InvariantCulture),
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code,
                        Name = x.ProjectFormula.Name
                    },
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    MeteredAux = Math.Round(x.Metered, 2),
                    ParcialAux = Math.Round(x.Parcial, 2)
                })
                .ToList();

            foreach (var item in data)
                item.AccumulatedRequestItems = items.Where(x => x.SupplyId == item.SupplyId).Sum(x => x.MeasureInAttention).ToString("N2", CultureInfo.InvariantCulture);

            return Ok(new
            {
                draw = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER,
                recordsTotal = totalRecords,
                recordsFiltered = aux.Count(),
                data,
                metered,
                parcial
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.GoalBudgetInputs
                .Where(x => x.Id == id)
                .Select(x => new GoalBudgetInputViewModel
                {
                    Id = x.Id,
                    SupplyId = x.SupplyId,
                    MeasurementUnitId = x.MeasurementUnitId,
                    Metered = x.Metered.ToString(),
                    UnitPrice = x.UnitPrice.ToString(),
                    Parcial = x.Parcial.ToString(),
                    ProjectFormulaId = x.ProjectFormulaId,
                    BudgetTitleId = x.BudgetTitleId,
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(GoalBudgetInputViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var budgetInput = new GoalBudgetInput
            {

                MeasurementUnitId = model.MeasurementUnitId,
                Metered = model.Metered.ToDoubleString(),
                Parcial = model.Parcial.ToDoubleString(),
                ProjectFormulaId = model.ProjectFormulaId,
                BudgetTitleId = model.BudgetTitleId,
            };
            await _context.GoalBudgetInputs.AddAsync(budgetInput);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, GoalBudgetInputViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var budgetInput = await _context.GoalBudgetInputs.FindAsync(id);
            budgetInput.MeasurementUnitId = model.MeasurementUnitId;
            budgetInput.Metered = model.Metered.ToDoubleString();
            budgetInput.Parcial = model.Parcial.ToDoubleString();
            budgetInput.BudgetTitleId = model.BudgetTitleId;
            budgetInput.ProjectFormulaId = model.ProjectFormulaId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var budgetInput = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == id);
            if (budgetInput == null)
                return BadRequest($"Insumo con Id '{id}' no encontrado.");
            _context.GoalBudgetInputs.Remove(budgetInput);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("cargar")]
        public async Task<IActionResult> Load()
        {
            var pId = GetProjectId();

            var diversos = await _context.DiverseInputs
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .ToListAsync();

            var goalBudgetInputs = await _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .ToListAsync();

            var count = goalBudgetInputs.Count();

            var carga = new List<GoalBudgetInput>();
            //var diversosFormulas = diversos.GroupBy(x => x.ProjectFormulaId).Where(x => x.Count() > 0);
            var formulas = diversos.Select(x => x.ProjectFormulaId).Distinct().ToList();

            foreach (var formula in formulas)
            {
                var frentes = diversos.Where(x => x.ProjectFormulaId == formula).ToList();
                var auxFrentes = frentes.Select(x => x.WorkFrontId).Distinct().ToList();
                foreach (var frente in auxFrentes) 
                {
                    var titulos = frentes.Where(x => x.WorkFrontId == frente).ToList();
                    var auxTitulos = titulos.Select(x => x.BudgetTitleId).Distinct().ToList();
                    foreach(var titulo in auxTitulos)
                    {
                        var insumos = titulos.Where(x => x.BudgetTitleId == titulo).ToList();
                        var auxInsumos = insumos.Select(x => x.SupplyId).Distinct().ToList();
                        foreach(var insumo in auxInsumos)
                        {
                            var existe = goalBudgetInputs
                                .FirstOrDefault(x => x.ProjectFormulaId == formula
                                && x.WorkFrontId == frente
                                && x.BudgetTitleId == titulo
                                && x.SupplyId == insumo);

                            var meta = new GoalBudgetInput();

                            var metered = 0.0;
                            var saldo = 0.0;

                            var divs = insumos.Where(x => x.SupplyId == insumo).ToList();

                            var aux = divs.FirstOrDefault(x => x.SupplyId == insumo);

                            foreach (var cant in divs)
                                metered += cant.Metered;

                            saldo = metered;

                            if (existe == null)
                            {
                                meta.Id = Guid.NewGuid();
                                meta.SupplyId = insumo;
                                meta.BudgetTitleId = titulo;
                                meta.ProjectFormulaId = formula;
                                meta.WorkFrontId = frente;
                                meta.MeasurementUnitId = aux.MeasurementUnitId;
                                meta.OrderNumber = count;
                                meta.Metered = metered;
                                meta.UnitPrice = aux.UnitPrice;
                                meta.Parcial = Math.Round(metered * aux.UnitPrice, 2);
                                meta.CurrentMetered = metered;

                                count++;
                                carga.Add(meta);
                            }
                            else
                            {
                                existe.Metered = metered;

                                //existe.UnitPrice = insumo.FirstOrDefault().UnitPrice;
                                existe.CurrentMetered = metered;

                                existe.Parcial = Math.Round(metered * aux.UnitPrice, 2);
                            }
                        }
                    }
                }
            }
            #region ACEROS

            var consolidatedSteels = _context.ConsolidatedSteels
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == pId &&
                x.Unit != "")
                .ToList();

            var steelVariables = await _context.SteelVariables
                .Include(x => x.BudgetInput)
                .Where(x => x.BudgetInput.ProjectId == pId)
                .ToListAsync();

            var steels = consolidatedSteels.Select(x => x.ProjectFormulaId).Distinct().ToList(); ;

            var rod6mmVariable = steelVariables.FirstOrDefault(x => x.RodDiameterMilimeters == 6);
            if (rod6mmVariable == null)
                return BadRequest("No se ha encontrado la variable de 6mm");

            var rod8mmVariable = steelVariables.FirstOrDefault(x => x.RodDiameterMilimeters == 8);
            if (rod8mmVariable == null)
                return BadRequest("No se ha encontrado la variable de 8mm");

            var rod3x8Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "3/8");
            if (rod3x8Variable == null)
                return BadRequest("No se ha encontrado la variable de 3/8\"");

            var rod1x2Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "1/2");
            if (rod1x2Variable == null)
                return BadRequest("No se ha encontrado la variable de 1/2\"");

            var rod5x8Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "5/8");
            if (rod5x8Variable == null)
                return BadRequest("No se ha encontrado la variable de 5/8\"");

            var rod3x4Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "3/4");
            if (rod3x4Variable == null)
                return BadRequest("No se ha encontrado la variable de 3/4\"");

            var rod1Variable = steelVariables.FirstOrDefault(x => x.RodDiameterInch == "1");
            if (rod1Variable == null)
                return BadRequest("No se ha encontrado la variable de 1\"");

            var unidadAcero = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Abbreviation == "VAR");

            foreach (var formula in steels)
            {
                var frentes = consolidatedSteels.Where(x => x.ProjectFormulaId == formula).ToList();
                var auxFrentes = frentes.Select(x => x.WorkFrontId).Distinct().ToList();
                foreach (var frente in auxFrentes)
                {
                    var titulos = frentes.Where(x => x.WorkFrontId == frente).ToList();
                    var auxTitulos = titulos.Select(x => x.BudgetTitleId).Distinct().ToList();
                    foreach (var titulo in auxTitulos)
                    {
                        var data = consolidatedSteels.Where(x => x.BudgetTitleId == titulo).ToList();

                        //var primero = data.FirstOrDefault();

                        /*
                        //Solución Óptima, pero el módulo de acero solo contiene las varillas vistas
                        //Propuesta pero descartada
                        foreach(var rods in steelVariables)
                        {
                            var existe = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rods.SupplyId && x.ProjectFormulaId == primero.ProjectFormulaId &&
                            x.WorkFrontId == primero.WorkFrontId && x.BudgetTitleId == primero.BudgetTitleId);
                        }
                        */

                        var rod6mmExiste = true;
                        var rod8mmExiste = true;
                        var rod3x8Existe = true;
                        var rod1x2Existe = true;
                        var rod5x8Existe = true;
                        var rod3x4Existe = true;
                        var rod1Existe = true;

                        var rod6mmSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod6mmVariable.SupplyId 
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod6mmSuma == null)
                        {
                            rod6mmSuma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod6mmVariable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod6mmVariable.PricePerRod
                            };
                            rod6mmExiste = false;
                            count++;
                        }

                        var rod8mmSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod8mmVariable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod8mmSuma == null)
                        {
                            rod8mmSuma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod8mmVariable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod8mmVariable.PricePerRod
                            };
                            rod8mmExiste = false;
                            count++;
                        }

                        var rod3x8Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod3x8Variable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod3x8Suma == null)
                        {
                            rod3x8Suma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod3x8Variable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod3x8Variable.PricePerRod
                            };
                            rod3x8Existe = false;
                            count++;
                        }

                        var rod1x2Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod1x2Variable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod1x2Suma == null)
                        {
                            rod1x2Suma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod1x2Variable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod1x2Variable.PricePerRod
                            };
                            rod1x2Existe = false;
                            count++;
                        }

                        var rod5x8Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod5x8Variable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod5x8Suma == null)
                        {
                            rod5x8Suma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod5x8Variable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod5x8Variable.PricePerRod
                            };
                            rod5x8Existe = false;
                            count++;
                        }

                        var rod3x4Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod3x4Variable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod3x4Suma == null)
                        {
                            rod3x4Suma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod3x4Variable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod3x4Variable.PricePerRod
                            };
                            rod3x4Existe = false;
                            count++;
                        }

                        var rod1Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == rod1Variable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (rod1Suma == null)
                        {
                            rod1Suma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)rod1Variable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadAcero.Id,
                                OrderNumber = count,
                                UnitPrice = rod1Variable.PricePerRod
                            };
                            rod1Existe = false;
                            count++;
                        }

                        rod6mmSuma.Metered = 0.0;
                        rod8mmSuma.Metered = 0.0;
                        rod3x8Suma.Metered = 0.0;
                        rod1x2Suma.Metered = 0.0;
                        rod5x8Suma.Metered = 0.0;
                        rod3x4Suma.Metered = 0.0;
                        rod1Suma.Metered = 0.0;

                        foreach (var item in data)
                        {
                            rod6mmSuma.Metered += item.Rod6mm;
                            rod8mmSuma.Metered += item.Rod8mm;
                            rod3x8Suma.Metered += item.Rod3x8;
                            rod1x2Suma.Metered += item.Rod1x2;
                            rod5x8Suma.Metered += item.Rod5x8;
                            rod3x4Suma.Metered += item.Rod3x4;
                            rod1Suma.Metered += item.Rod1;
                        }

                        rod6mmSuma.Metered = Math.Round(rod6mmSuma.Metered);
                        rod8mmSuma.Metered = Math.Round(rod8mmSuma.Metered);
                        rod3x8Suma.Metered = Math.Round(rod3x8Suma.Metered);
                        rod1x2Suma.Metered = Math.Round(rod1x2Suma.Metered);
                        rod5x8Suma.Metered = Math.Round(rod5x8Suma.Metered);
                        rod3x4Suma.Metered = Math.Round(rod3x4Suma.Metered);
                        rod1Suma.Metered = Math.Round(rod1Suma.Metered);

                        rod6mmSuma.CurrentMetered = Math.Round(rod6mmSuma.Metered);
                        rod8mmSuma.CurrentMetered = Math.Round(rod8mmSuma.Metered);
                        rod3x8Suma.CurrentMetered = Math.Round(rod3x8Suma.Metered);
                        rod1x2Suma.CurrentMetered = Math.Round(rod1x2Suma.Metered);
                        rod5x8Suma.CurrentMetered = Math.Round(rod5x8Suma.Metered);
                        rod3x4Suma.CurrentMetered = Math.Round(rod3x4Suma.Metered);
                        rod1Suma.CurrentMetered = Math.Round(rod1Suma.Metered);

                        rod6mmSuma.WarehouseCurrentMetered = Math.Round(rod6mmSuma.Metered);
                        rod8mmSuma.WarehouseCurrentMetered = Math.Round(rod8mmSuma.Metered);
                        rod3x8Suma.WarehouseCurrentMetered = Math.Round(rod3x8Suma.Metered);
                        rod1x2Suma.WarehouseCurrentMetered = Math.Round(rod1x2Suma.Metered);
                        rod5x8Suma.WarehouseCurrentMetered = Math.Round(rod5x8Suma.Metered);
                        rod3x4Suma.WarehouseCurrentMetered = Math.Round(rod3x4Suma.Metered);
                        rod1Suma.WarehouseCurrentMetered = Math.Round(rod1Suma.Metered);

                        rod6mmSuma.Parcial = Math.Round(rod6mmSuma.Metered * rod6mmSuma.UnitPrice, 2);
                        rod8mmSuma.Parcial = Math.Round(rod8mmSuma.Metered * rod8mmSuma.UnitPrice, 2);
                        rod3x8Suma.Parcial = Math.Round(rod3x8Suma.Metered * rod3x8Suma.UnitPrice, 2);
                        rod1x2Suma.Parcial = Math.Round(rod1x2Suma.Metered * rod1x2Suma.UnitPrice, 2);
                        rod5x8Suma.Parcial = Math.Round(rod5x8Suma.Metered * rod5x8Suma.UnitPrice, 2);
                        rod3x4Suma.Parcial = Math.Round(rod3x4Suma.Metered * rod3x4Suma.UnitPrice, 2);
                        rod1Suma.Parcial = Math.Round(rod1Suma.Metered * rod1Suma.UnitPrice, 2);

                        if (rod6mmExiste == false)
                            carga.Add(rod6mmSuma);

                        if (rod8mmExiste == false)
                            carga.Add(rod8mmSuma);

                        if (rod3x8Existe == false)
                            carga.Add(rod3x8Suma);

                        if (rod1x2Existe == false)
                            carga.Add(rod1x2Suma);

                        if (rod5x8Existe == false)
                            carga.Add(rod5x8Suma);

                        if (rod3x4Existe == false)
                            carga.Add(rod3x4Suma);

                        if (rod1Existe == false)
                            carga.Add(rod1Suma);

                    }
                }

            }
            #endregion
            
            #region CEMENTOS

                var consolidatedCements = _context.ConsolidatedCements
                    .Include(x => x.ProjectFormula)
                    .Where(x => x.ProjectFormula.ProjectId == pId && x.Unit != "")
                    .ToList();

                var cements = consolidatedCements.Select(x => x.ProjectFormulaId).Distinct().ToList();

                var typeOneVariable = await _context.CementVariables
                    .Include(x => x.Supply)
                    .Include(x => x.BudgetInput)
                    .Where(x => x.BudgetInput.ProjectId == pId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Supply.Description == "CEMENTO PORTLAND TIPO I (42.5 KG.)");

                if (typeOneVariable == null)
                    return BadRequest("No se ha encontrado la variable Cemento Tipo I");

                var typeFiveVariable = await _context.CementVariables
                    .Include(x => x.Supply)
                    .Include(x => x.BudgetInput)
                    .Where(x => x.BudgetInput.ProjectId == pId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Supply.Description == "CEMENTO PORTLAND TIPO V (42.5 KG.)");

                if (typeFiveVariable == null)
                    return BadRequest("No se ha encontrado la variable Cemento Tipo V");

                var unidadCemento = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Abbreviation == "BLS");

            foreach (var formula in cements)
            {
                var frentes = consolidatedCements.Where(x => x.ProjectFormulaId == formula).ToList();
                var auxFrentes = frentes.Select(x => x.WorkFrontId).Distinct().ToList();
                foreach (var frente in auxFrentes)
                {
                    var titulos = frentes.Where(x => x.WorkFrontId == frente).ToList();
                    var auxTitutlos = titulos.Select(x => x.BudgetTitleId).Distinct().ToList();
                    foreach (var titulo in auxTitutlos)
                    {
                        var data = titulos.Where(x => x.BudgetTitleId == titulo).ToList();

                        var primero = data.FirstOrDefault();

                        var typeOneExiste = true;
                        var typeFiveExiste = true;

                        var typeOneSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == typeOneVariable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                        if (typeOneSuma == null)
                        {
                            typeOneSuma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)typeOneVariable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadCemento.Id,
                                OrderNumber = count,
                                UnitPrice = typeOneVariable.UnitPrice
                            };
                            typeOneExiste = false;
                            count++;
                        }

                        var typeFiveSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == typeFiveVariable.SupplyId
                            && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);
                        if (typeFiveSuma == null)
                        {
                            typeFiveSuma = new GoalBudgetInput
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = (Guid)typeFiveVariable.SupplyId,
                                WorkFrontId = frente,
                                ProjectFormulaId = formula,
                                BudgetTitleId = titulo,
                                MeasurementUnitId = unidadCemento.Id,
                                OrderNumber = count,
                                UnitPrice = typeFiveVariable.UnitPrice
                            };
                            typeFiveExiste = false;
                            count++;
                        }

                        typeOneSuma.Metered = 0.0;
                        typeFiveSuma.Metered = 0.0;

                        foreach (var item in data)
                        {
                            typeOneSuma.Metered += item.ContractualRestatedTypeOne;
                            typeFiveSuma.Metered += item.ContractualRestatedTypeFive;
                        }

                        typeOneSuma.Metered = Math.Round(typeOneSuma.Metered);
                        typeFiveSuma.Metered = Math.Round(typeFiveSuma.Metered);
            
                        typeOneSuma.CurrentMetered = Math.Round(typeOneSuma.Metered);
                        typeFiveSuma.CurrentMetered = Math.Round(typeFiveSuma.Metered);

                        typeOneSuma.Parcial = Math.Round(typeOneSuma.Metered * typeOneSuma.UnitPrice, 2);
                        typeFiveSuma.Parcial = Math.Round(typeFiveSuma.Metered * typeFiveSuma.UnitPrice, 2);

                        if (typeOneExiste == false)
                            carga.Add(typeOneSuma);

                        if (typeFiveExiste == false)
                            carga.Add(typeFiveSuma);

                    }
                }
            }
            
            #endregion
            
            #region ENTIBADOS
            if (pId != new Guid("CB9CD712-E2DB-421A-52F0-08D88325D938"))
            {
                var entibadosConsolidados = _context.ConsolidatedEntibados
                    .Include(x => x.ProjectFormula)
                    .Where(x => x.ProjectFormula.ProjectId == pId &&
                    x.Unit != "")
                    .ToList();

                var entibadosVariables = await _context.EntibadoVariables
                    .Include(x => x.Supply)
                    .Include(x => x.BudgetInput)
                    .Where(x => x.BudgetInput.ProjectId == pId).ToListAsync();

                var entibados = entibadosConsolidados.Select(x => x.ProjectFormulaId).Distinct().ToList();

                var ks60Variable = entibadosVariables.FirstOrDefault(x => x.Supply.FullCode == "01020016001");
                var ks100Variable = entibadosVariables.FirstOrDefault(x => x.Supply.FullCode == "01020016002");
                var realzaExtensionVariable = entibadosVariables.FirstOrDefault(x => x.Supply.FullCode == "01020016003");
                var correderaVariable = entibadosVariables.FirstOrDefault(x => x.Supply.FullCode == "01020016004");
                var paraleloVariable = entibadosVariables.FirstOrDefault(x => x.Supply.FullCode == "01020016005");

                if (ks60Variable == null)
                    return BadRequest("No se ha hallado la variable ALQUILER DE ENTIBADOS KS60/MINIBOX");
                if (ks100Variable == null)
                    return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO KS100/KMC 100");
                if (realzaExtensionVariable == null)
                    return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO REALZA/EXTENSION");
                if (correderaVariable == null)
                    return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO CORREDERA");
                if (paraleloVariable == null)
                    return BadRequest("No se ha hallado la variable ALQUILER ENTIBADO PARALELO");

                var unidadEntibado = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Abbreviation == "JGO-MES");

                foreach (var formula in entibados)
                {
                    var frentes = entibadosConsolidados.Where(x => x.ProjectFormulaId == formula).ToList();
                    var auxFrentes = frentes.Select(x => x.WorkFrontId).Distinct().ToList();
                    foreach (var frente in auxFrentes)
                    {
                        var titulos = frentes.Where(x => x.WorkFrontId == frente).ToList();
                        var auxTitulos = titulos.Select(x => x.BudgetTitleId).Distinct().ToList();
                        foreach (var titulo in auxTitulos)
                        {
                            var data = titulos.Where(x => x.BudgetTitleId == titulo).ToList();

                            //var primero = data.FirstOrDefault();

                            var ks60Existe = true;
                            var ks100Existe = true;
                            var realzaExtensionExiste = true;
                            var correderaExiste = true;
                            var paraleloExiste = true;

                            var ks60Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == ks60Variable.SupplyId
                                && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                            if (ks60Suma == null)
                            {
                                ks60Suma = new GoalBudgetInput
                                {
                                    Id = Guid.NewGuid(),
                                    SupplyId = (Guid)ks60Variable.SupplyId,
                                    WorkFrontId = frente,
                                    ProjectFormulaId = formula,
                                    BudgetTitleId = titulo,
                                    MeasurementUnitId = unidadEntibado.Id,
                                    OrderNumber = count,
                                    UnitPrice = ks60Variable.BudgetInput.SaleUnitPrice
                                };
                                ks60Existe = false;
                                count++;
                            }

                            var ks100Suma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == ks100Variable.SupplyId
                                && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                            if (ks100Suma == null)
                            {
                                ks100Suma = new GoalBudgetInput
                                {
                                    Id = Guid.NewGuid(),
                                    SupplyId = (Guid)ks100Variable.SupplyId,
                                    WorkFrontId = frente,
                                    ProjectFormulaId = formula,
                                    BudgetTitleId = titulo,
                                    MeasurementUnitId = unidadEntibado.Id,
                                    OrderNumber = count,
                                    UnitPrice = ks100Variable.BudgetInput.SaleUnitPrice
                                };
                                ks100Existe = false;
                                count++;
                            }

                            var realzaExtensionSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == realzaExtensionVariable.SupplyId
                                && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                            if (realzaExtensionSuma == null)
                            {
                                realzaExtensionSuma = new GoalBudgetInput
                                {
                                    Id = Guid.NewGuid(),
                                    SupplyId = (Guid)realzaExtensionVariable.SupplyId,
                                    WorkFrontId = frente,
                                    ProjectFormulaId = formula,
                                    BudgetTitleId = titulo,
                                    MeasurementUnitId = unidadEntibado.Id,
                                    OrderNumber = count,
                                    UnitPrice = realzaExtensionVariable.BudgetInput.SaleUnitPrice
                                };
                                realzaExtensionExiste = false;
                                count++;
                            }

                            var correderaSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == correderaVariable.SupplyId
                                && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                            if (correderaSuma == null)
                            {
                                correderaSuma = new GoalBudgetInput
                                {
                                    Id = Guid.NewGuid(),
                                    SupplyId = (Guid)correderaVariable.SupplyId,
                                    WorkFrontId = frente,
                                    ProjectFormulaId = formula,
                                    BudgetTitleId = titulo,
                                    MeasurementUnitId = unidadEntibado.Id,
                                    OrderNumber = count,
                                    UnitPrice = correderaVariable.BudgetInput.SaleUnitPrice
                                };
                                correderaExiste = false;
                                count++;
                            }

                            var paraleloSuma = goalBudgetInputs.FirstOrDefault(x => x.SupplyId == paraleloVariable.SupplyId
                                && x.ProjectFormulaId == formula && x.WorkFrontId == frente && x.BudgetTitleId == titulo);

                            if (paraleloSuma == null)
                            {
                                paraleloSuma = new GoalBudgetInput
                                {
                                    Id = Guid.NewGuid(),
                                    SupplyId = (Guid)paraleloVariable.SupplyId,
                                    WorkFrontId = frente,
                                    ProjectFormulaId = formula,
                                    BudgetTitleId = titulo,
                                    MeasurementUnitId = unidadEntibado.Id,
                                    OrderNumber = count,
                                    UnitPrice = paraleloVariable.BudgetInput.SaleUnitPrice
                                };
                                paraleloExiste = false;
                                count++;
                            }

                            ks60Suma.Metered = 0.0;
                            ks100Suma.Metered = 0.0;
                            realzaExtensionSuma.Metered = 0.0;
                            correderaSuma.Metered = 0.0;
                            paraleloSuma.Metered = 0.0;

                            foreach (var item in data)
                            {
                                ks60Suma.Metered += item.KS60xMinibox;
                                ks100Suma.Metered += item.KS100xKMC100;
                                realzaExtensionSuma.Metered += item.RealzaxExtension;
                                correderaSuma.Metered += item.Corredera;
                                paraleloSuma.Metered += item.Paralelo;
                            }

                            ks60Suma.Metered = Math.Round(ks60Suma.Metered);
                            ks100Suma.Metered = Math.Round(ks100Suma.Metered);
                            realzaExtensionSuma.Metered = Math.Round(realzaExtensionSuma.Metered);
                            correderaSuma.Metered = Math.Round(correderaSuma.Metered);
                            paraleloSuma.Metered = Math.Round(paraleloSuma.Metered);

                            ks60Suma.CurrentMetered = Math.Round(ks60Suma.Metered);
                            ks100Suma.CurrentMetered = Math.Round(ks100Suma.Metered);
                            realzaExtensionSuma.CurrentMetered = Math.Round(realzaExtensionSuma.Metered);
                            correderaSuma.CurrentMetered = Math.Round(correderaSuma.Metered);
                            paraleloSuma.CurrentMetered = Math.Round(paraleloSuma.Metered);

                            ks60Suma.WarehouseCurrentMetered = Math.Round(ks60Suma.Metered);
                            ks100Suma.WarehouseCurrentMetered = Math.Round(ks100Suma.Metered);
                            realzaExtensionSuma.WarehouseCurrentMetered = Math.Round(realzaExtensionSuma.Metered);
                            correderaSuma.WarehouseCurrentMetered = Math.Round(correderaSuma.Metered);
                            paraleloSuma.WarehouseCurrentMetered = Math.Round(paraleloSuma.Metered);

                            ks60Suma.Parcial = Math.Round(ks60Suma.Metered * ks60Variable.BudgetInput.SaleUnitPrice, 2);
                            ks100Suma.Parcial = Math.Round(ks100Suma.Metered * ks100Variable.BudgetInput.SaleUnitPrice, 2);
                            realzaExtensionSuma.Parcial = Math.Round(realzaExtensionSuma.Metered * realzaExtensionVariable.BudgetInput.SaleUnitPrice, 2);
                            correderaSuma.Parcial = Math.Round(correderaSuma.Metered * correderaVariable.BudgetInput.SaleUnitPrice, 2);
                            paraleloSuma.Parcial = Math.Round(paraleloSuma.Metered * paraleloVariable.BudgetInput.SaleUnitPrice, 2);

                            if (ks60Existe == false)
                                carga.Add(ks60Suma);

                            if (ks100Existe == false)
                                carga.Add(ks100Suma);

                            if (realzaExtensionExiste == false)
                                carga.Add(realzaExtensionSuma);

                            if (correderaExiste == false)
                                carga.Add(correderaSuma);

                            if (paraleloExiste == false)
                                carga.Add(paraleloSuma);
                        }
                    }
                }
            }
            #endregion

            await _context.SaveChangesAsync();

            var orders = await _context.RequestsInOrders
                .Include(x => x.Order)
                .Where(x => x.Order.Status == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                .ToListAsync();

            var orderItems = await _context.OrderItems
                .Include(x => x.Order)
                .Where(x => orders.Select(x => x.OrderId).Contains(x.OrderId))
                .ToListAsync();

            //var requestItems = await _context.RequestItems
            //    .Include(x => x.Request)
            //    .Include(x => x.Supply)
            //    .Where(x => orders.Select(x => x.RequestId).Contains(x.RequestId))
            //    .ToListAsync();

            foreach (var itemOrd in orderItems)
            {
                var saldo = itemOrd.Measure;

                var ordersAux = orders.Where(x => x.OrderId == itemOrd.OrderId).ToList();

                //var aux = requestItems.Where(x => x.SupplyId == itemOrd.SupplyId
                //&& ordersAux.Select(x => x.RequestId).Contains(x.RequestId)).ToList();
                //foreach(var item in aux)
                //{
                //    item.GoalBudgetInput.CurrentMetered -= item.MeasureInAttention;
                //    saldo -= item.MeasureInAttention;
                //}
                /*
                if(item.GoalBudgetInput.CurrentMetered < saldo)
                {
                    if (item.GoalBudgetInput.CurrentMetered < 0)
                        continue;
                    // 20 - 20 = 0
                    // 20 - 12 = 8
                    // 20 - 0 = 20
                    item.GoalBudgetInput.CurrentMetered -= item.MeasureInAttention;
                    saldo -= item.GoalBudgetInput.CurrentMetered;
                }
                else
                {

                    item.GoalBudgetInput.CurrentMetered -= saldo;
                    saldo = 0;
                }*/

                //if (saldo > 0)
                  //  return BadRequest("No se ha ingresado totalmente el saldo");
            }

            await _context.SaveChangesAsync();

            var pedidos = await _context.FieldRequestFoldings
                .Include(x => x.FieldRequest)
                .Where(x => x.FieldRequest.Status == ConstantHelpers.Warehouse.FieldRequest.Status.ATTENDED)
                .ToListAsync();

            foreach(var item in pedidos)
            {
                item.GoalBudgetInput.WarehouseAccumulatedMetered -= item.DeliveredQuantity;
            }

            if (carga.Count != 0)
                await _context.GoalBudgetInputs.AddRangeAsync(carga);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {
            var dt = new DataTable("INSUMOS");
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Unidad", typeof(string));
            dt.Columns.Add("Familia", typeof(string));
            dt.Columns.Add("Grupo", typeof(string));
            dt.Columns.Add("P.U. Venta", typeof(decimal));
            dt.Columns.Add("Metrado", typeof(decimal));
            dt.Columns.Add("Parcial", typeof(decimal));

            var data = await _context.GoalBudgetInputs
                .Include(x => x.MeasurementUnit)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .AsNoTracking()
                .ToListAsync();
            data.ForEach(item => {
                dt.Rows.Add(item.Supply.FullCode, item.Supply.Description, 
                    item.MeasurementUnit.Abbreviation, item.Supply.SupplyFamily.Code + "-" + item.Supply.SupplyFamily.Name, 
                    item.Supply.SupplyGroup.Code + "-" + item.Supply.SupplyGroup.Name, item.UnitPrice, item.Metered, item.Parcial);
            });

            var fileName = "Insumos (Presupuesto).xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                //--------------CAMPOS------------

                var pendiente = "-";
                workSheet.Column(1).Width = 15;
                workSheet.Column(2).Width = 165;
                workSheet.Column(3).Width = 8;
                workSheet.Column(4).Width = 40;
                workSheet.Column(5).Width = 40;
                workSheet.Column(6).Width = 18;
                workSheet.Column(7).Width = 18;
                workSheet.Column(8).Width = 18;
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "InsumosCargaMasiva.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "CODIGO S10";

                workSheet.Cell($"C2").Value = "MATERIAL";

                workSheet.Cell($"D2").Value = "UND";

                workSheet.Cell($"E2").Value = "METRADO";

                workSheet.Cell($"F2").Value = "P.U.";

                workSheet.Cell($"G2").Value = "PARCIAL";

                workSheet.Cell($"H2").Value = "GRUPO";

                workSheet.Cell($"I2").Value = "Familia";

                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 15;
                workSheet.Column(3).Width = 60;
                workSheet.Column(4).Width = 7;
                workSheet.Column(5).Width = 15;
                workSheet.Column(6).Width = 10;
                workSheet.Column(7).Width = 18;
                workSheet.Column(8).Width = 63;
                workSheet.Column(9).Width = 63;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:I2").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:I2").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var groups = _context.SupplyGroups.AsNoTracking().ToList();

                DataTable dtGroups = new DataTable();
                dtGroups.TableName = "Grupo de Insumos";
                dtGroups.Columns.Add("Código", typeof(string));
                dtGroups.Columns.Add("Descripción", typeof(string));
                foreach (var item in groups)
                    dtGroups.Rows.Add(item.Code, item.Name);
                dtGroups.AcceptChanges();

                var workSheetGroup = wb.Worksheets.Add(dtGroups);

                workSheetGroup.Column(2).Width = 70;

                var families = _context.SupplyFamilies.AsNoTracking().ToList();

                DataTable dtFamilies = new DataTable();
                dtFamilies.TableName = "Familia de Grupo";
                dtFamilies.Columns.Add("Código", typeof(string));
                dtFamilies.Columns.Add("Descripción", typeof(string));
                foreach (var item in families)
                    dtFamilies.Rows.Add(item.Code, item.Name);
                dtFamilies.AcceptChanges();

                var workSheetFamily = wb.Worksheets.Add(dtFamilies);

                workSheetFamily.Column(2).Width = 70;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("metrado")]
        public async Task<IActionResult> GetMetered(string code = null, Guid? measurementUnitId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
            Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var pId = GetProjectId();

            var query = _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .AsEnumerable();

            var meteredSuma = 0.0;

            if (measurementUnitId.HasValue)
                query = query.Where(x => x.MeasurementUnitId == measurementUnitId.Value);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);
            if (!string.IsNullOrEmpty(code))
                query = query.Where(x => x.Supply.FullCode == code);

            meteredSuma = query.Sum(x => x.Metered);

            return Ok(meteredSuma.ToString("N2", CultureInfo.InvariantCulture));
        }

        [HttpGet("parcial")]
        public async Task<IActionResult> GetParcial(string code = null, Guid? measurementUnitId = null, Guid? supplyFamilyId = null, Guid? supplyGroupId = null,
          Guid? projectFormulaId = null, Guid? budgetTitleId = null, Guid? workFrontId = null)
        {
            var pId = GetProjectId();

            var query = _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.ProjectFormula.ProjectId == pId)
                .AsEnumerable();

            var parcialSuma = 0.0;

            if (measurementUnitId.HasValue)
                query = query.Where(x => x.MeasurementUnitId == measurementUnitId.Value);
            if (supplyFamilyId.HasValue)
                query = query.Where(x => x.Supply.SupplyFamilyId == supplyFamilyId.Value);
            if (supplyGroupId.HasValue)
                query = query.Where(x => x.Supply.SupplyGroupId == supplyGroupId.Value);
            if (projectFormulaId.HasValue)
                query = query.Where(x => x.ProjectFormulaId == projectFormulaId.Value);
            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId.Value);
            if (workFrontId.HasValue)
                query = query.Where(x => x.WorkFrontId == workFrontId.Value);
            if (!string.IsNullOrEmpty(code))
                query = query.Where(x => x.Supply.FullCode == code);

            parcialSuma = query.Sum(x => x.Parcial);

            return Ok(parcialSuma.ToString("N2", CultureInfo.InvariantCulture));
        }


        [HttpGet("obtener-unidad/{id}")]
        public async Task<IActionResult> GetMeasureUnit(Guid id)
        {
            var unit = await _context.GoalBudgetInputs
                .Include(x => x.MeasurementUnit)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (unit == null)
                return BadRequest();

            return Ok(unit.MeasurementUnit
                    .Abbreviation);
        }

        [HttpGet("requerimientos/detalles")]
        public async Task<IActionResult> GetRequestDetail(Guid id)
        {
            var items = await _context.RequestItems
                .Include(x => x.Request)
                .Include(x => x.Supply)
                .Where(x => x.SupplyId == id)
                .Where(x => x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C
                || x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S)
                .Select(x => new RequestItemViewModel
                {
                    RequestId = x.RequestId,
                    Request = new RequestInRequestItemsViewModel
                    {
                        CorrelativeCodeStr = x.Request.CorrelativePrefix + "-" + x.Request.CorrelativeCode.ToString("D4")
                    },
                    //GoalBudgetInputId = x.GoalBudgetInputId,
                    //GoalBudgetInput = new GoalBudgetInputViewModel
                    //{
                    //    Supply = new SupplyViewModel
                    //    {
                    //        Description = x.GoalBudgetInput.Supply.Description
                    //    }
                    //},
                    SupplyId = x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description
                    },
                    Measure = x.Measure.ToString("N2", CultureInfo.InvariantCulture),
                    MeasureInAttention = x.MeasureInAttention
                })
                .ToListAsync();

            return Ok(items);
        }
        /*
        [HttpPut("actualizar")]
        public async Task<IActionResult> Update()
        {
            var meta = await _context.GoalBudgetInputs
                .Where(x => x.Metered > 9999)
                .ToListAsync();

            foreach(var item in meta)
            {
                item.Metered = 1000;
                item.CurrentMetered = 1000;
                item.WarehouseCurrentMetered = 1000;

                item.Parcial = Math.Round(item.UnitPrice * 1000, 2);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }*/
    }
}
