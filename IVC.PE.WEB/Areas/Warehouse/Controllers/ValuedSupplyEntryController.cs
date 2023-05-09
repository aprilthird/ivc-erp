using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.SupplyEntryViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.ValuedSupplyEntryViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/guias-valorizadas")]
    public class ValuedSupplyEntryController : BaseController
    {
        public ValuedSupplyEntryController(IvcDbContext context,
               ILogger<ValuedSupplyEntryController> logger) 
            : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(int year = 0, int month = 0, 
            Guid? supplyGroupId = null, Guid? providerId = null)
        {
            var res = new List<ValuedSupplyEntryViewModel>();

            var query = _context.SupplyEntries
                .Include(x => x.Order.Provider)
                .Include(x => x.Warehouse.WarehouseType)
                .Where(x => x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.Warehouse.WarehouseType.ProjectId == GetProjectId());

            if (providerId.HasValue)
                query = query.Where(x => x.Order.ProviderId == providerId.Value);

            if (year > 0)
                query = query.Where(x => x.DeliveryDate.Year == year);

            if (month > 0)
                query = query.Where(x => x.DeliveryDate.Month == month);

            var providers = await query
                .Select(x => new { x.Order.ProviderId, x.Order.Provider.Tradename})
                .Distinct()
                .ToListAsync();

            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .ToListAsync();

            foreach(var provider in providers)
            {
                var guias = await query
                    .Where(x => x.Order.ProviderId == provider.ProviderId
                    && x.IsValued == true)
                    .ToListAsync();

                var grupos = string.Join("-",
                    items.Where(x => guias.Select(x => x.Id).Contains(x.SupplyEntryId))
                    .Select(x => x.OrderItem.Supply.SupplyGroup.Name).Distinct().ToList());

                if (supplyGroupId != null && !items.Where(x => guias.Select(x => x.Id).Contains(x.SupplyEntryId))
                    .Select(x => x.OrderItem.Supply.SupplyGroupId)
                    .Contains(supplyGroupId))
                    continue;

                var years = "";

                if (year > 0)
                    years = year.ToString();
                else
                    years = string.Join("-", guias.OrderBy(x => x.ValuedYear)
                    .Select(x => x.ValuedYear.ToString())
                    .Distinct().ToList());

                var months = "";

                if (month > 0)
                    months = ConstantHelpers.Months.VALUES[month];
                else
                    months = string.Join("-", guias.OrderBy(x => x.ValuedMonth)
                    .Select(x => ConstantHelpers.Months.VALUES[x.ValuedMonth])
                    .Distinct().ToList());

                var soles = 0.0;
                var dolares = 0.0;
                var moneda = "";

                foreach(var item in guias)
                {
                    if(item.Order.Currency == ConstantHelpers.Currency.NUEVOS_SOLES)
                    {
                        soles += item.Parcial;
                        if (item.Order.ExchangeRate > 0)
                            dolares += item.Parcial / item.Order.ExchangeRate;
                    }
                    else
                    {
                        soles += item.Parcial * item.Order.ExchangeRate;
                        dolares += item.Parcial;
                        if (item.Order.Currency == ConstantHelpers.Currency.AMERICAN_DOLLARS)
                            moneda = "$ ";
                        else
                            moneda = "€ ";
                    }
                }

                res.Add(new ValuedSupplyEntryViewModel
                {
                    Groups = grupos,
                    ProviderId = provider.ProviderId,
                    Provider = provider.Tradename,
                    Month = months,
                    Year = years,
                    Parcial = soles,
                    DolarParcial = dolares,
                    ParcialString = "S/ " + soles.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcialString = moneda + dolares.ToString("N2", CultureInfo.InvariantCulture)
                });

            }

            return Ok(res);
        }

        [HttpGet("crear-items/listar")]
        public async Task<IActionResult> GetEntriesToValue(Guid? id)
        {
            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Order)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Where(x => x.SupplyEntryId == id && x.IsValued != true)
                .Select(x => new SupplyEntryItemViewModel
                {
                    Id = x.Id,
                    OrderItem = new OrderItemViewModel
                    {
                        Id = x.Id,
                        Supply = new SupplyViewModel
                        {
                            Description = x.OrderItem.Supply.Description,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Code = x.OrderItem.Supply.SupplyFamily.Code,
                                Name = x.OrderItem.Supply.SupplyFamily.Name
                            },
                            SupplyGroupId = x.OrderItem.Supply.SupplyGroupId,
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Code = x.OrderItem.Supply.SupplyGroup.Code,
                                Name = x.OrderItem.Supply.SupplyGroup.Name
                            },
                            CorrelativeCode = x.OrderItem.Supply.CorrelativeCode,
                            MeasurementUnit = new MeasurementUnitViewModel
                            {
                                Abbreviation = x.OrderItem.Supply.MeasurementUnit.Abbreviation
                            }
                        },
                        Order = new OrderViewModel
                        {
                            Status = x.OrderItem.Order.Status
                        },
                        Measure = x.OrderItem.Measure.ToString()
                    },
                    Measure = x.Measure.ToString(),
                    Observations = x.Observations,
                    PreviousAttention = x.PreviousAttention,
                    IsValued = x.IsValued
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost("crear-items")]
        public async Task<IActionResult> CreateItems(IEnumerable<string> items, int month = 0, int year = 0)
        {
            var entryItems = await _context.SupplyEntryItems
                .Include(x => x.SupplyEntry)
                .ToListAsync();

            var entries = await _context.SupplyEntries
                .Where(x => x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.IsValued == false).ToListAsync();

            foreach (var stringItem in items)
            {
                var stringSplit = stringItem.Split("|");

                if (stringSplit[0] == "undefined" || stringSplit[1] == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                Guid id = Guid.Parse(stringSplit[0]);

                var aux = entryItems.FirstOrDefault(x => x.Id == id);

                if (aux == null)
                    return BadRequest("No se ha encontrado el item");

                var isValued = Boolean.Parse(stringSplit[1]);

                if (isValued == false)
                    continue;
                if (isValued == true)
                {
                    aux.IsValued = true;
                    aux.SupplyEntry.IsValued = true;
                    aux.SupplyEntry.ValuedMonth = month;
                    aux.SupplyEntry.ValuedYear = year;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("crear/listar")]
        public async Task<IActionResult> GetItemsToValue(Guid? supplyFamilyId = null,
            Guid? supplyGroupId = null, Guid? providerId = null)
        {
            var query = _context.SupplyEntries
                .Include(x => x.Order)
                .Include(x => x.Order.Provider)
                .Where(x => x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.IsValued == false);

            if (providerId.HasValue)
                query = query.Where(x => x.Order.ProviderId == providerId);

            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .ToListAsync();

            var res = new List<SupplyEntryViewModel>();

            foreach (var entry in query)
            {
                var aux = items.Where(x => x.SupplyEntryId == entry.Id).ToList();

                if (supplyGroupId != null && !aux.Select(x => x.OrderItem.Supply.SupplyGroupId)
                    .Contains(supplyGroupId))
                    continue;

                if (supplyFamilyId != null && !aux.Select(x => x.OrderItem.Supply.SupplyFamilyId)
                    .Contains((Guid)supplyFamilyId))
                    continue;

                var soles = 0.0;
                var dolares = 0.0;
                var moneda = "";

                if (entry.Order.Currency == 1)
                {
                    soles = entry.Parcial;
                    dolares = Math.Round(entry.Parcial / entry.Order.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(entry.Parcial * entry.Order.ExchangeRate, 2);
                    dolares = entry.Parcial;
                    if (entry.Order.Currency == ConstantHelpers.Currency.AMERICAN_DOLLARS)
                        moneda = "$ ";
                    else
                        moneda = "€ ";
                }

                res.Add(new SupplyEntryViewModel
                {
                    Id = entry.Id,
                    Order = new Logistics.ViewModels.OrderViewModels.OrderViewModel
                    {
                        Provider = new Logistics.ViewModels.ProviderViewModels.ProviderViewModel
                        {
                            Tradename = entry.Order.Provider.Tradename
                        }
                    },
                    IsValued = entry.IsValued,
                    RemissionGuideName = entry.RemissionGuide,
                    RemissionGuideUrl = entry.RemissionGuideUrl,
                    DeliveryDate = entry.DeliveryDate.ToDateString(),
                    ParcialString = "S/ " + soles.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcialString = moneda + dolares.ToString("N2", CultureInfo.InvariantCulture)
                });
            }

            return Ok(res);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(IEnumerable<string> items ,int month = 0, int year = 0)
        {
            var entries = await _context.SupplyEntries
                .Include(x => x.SupplyEntryItems)
                .Where(x => x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.IsValued == false).ToListAsync();

            foreach(var stringItem in items)
            {
                var stringSplit = stringItem.Split("|");

                if (stringSplit[0] == "undefined" || stringSplit[1] == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                Guid id = Guid.Parse(stringSplit[0]);

                var aux = entries.FirstOrDefault(x => x.Id == id);

                if (aux == null)
                    return BadRequest("No se ha encontrado el ingreso");

                var isValued = Boolean.Parse(stringSplit[1]);

                if (isValued == false)
                    continue;
                if (isValued == true)
                {
                    aux.IsValued = true;
                    aux.ValuedMonth = month;
                    aux.ValuedYear = year;


                    foreach (var entryItem in aux.SupplyEntryItems)
                        entryItem.IsValued = true;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetDetails(Guid providerId, int year = 0, int month = 0)
        {
            var entries = _context.SupplyEntries
                .Include(x => x.Order.Provider)
                .Where(x => x.Order.ProviderId == providerId
                && x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.IsValued == true);

            if (year > 0)
                entries = entries.Where(x => x.DeliveryDate.Year == year);

            if (month > 0)
                entries = entries.Where(x => x.DeliveryDate.Month == month);

            var res = new List<SupplyEntryViewModel>();

            foreach (var entry in entries)
            {
                var soles = 0.0;
                var dolares = 0.0;
                var moneda = "";

                if (entry.Order.Currency == 1)
                {
                    soles = entry.Parcial;
                    dolares = Math.Round(entry.Parcial / entry.Order.ExchangeRate, 2);
                }
                else
                {
                    soles = Math.Round(entry.Parcial * entry.Order.ExchangeRate, 2);
                    dolares = entry.Parcial;
                    if (entry.Order.Currency == ConstantHelpers.Currency.AMERICAN_DOLLARS)
                        moneda = "$ ";
                    else
                        moneda = "€ ";
                }

                res.Add(new SupplyEntryViewModel
                {
                    Id = entry.Id,
                    IsValued = entry.IsValued,
                    RemissionGuideUrl = entry.RemissionGuideUrl,
                    RemissionGuideName = entry.RemissionGuide,
                    DeliveryDate = entry.DeliveryDate.ToDateString(),
                    ParcialString = "S/ " + soles.ToString("N2", CultureInfo.InvariantCulture),
                    DolarParcialString = moneda + dolares.ToString("N2", CultureInfo.InvariantCulture),
                    ValuedMonth = ConstantHelpers.Months.VALUES
                    .First(y => y.Key == entry.ValuedMonth).Value,
                    ValuedYear = entry.ValuedYear.ToString()
                });
            }

            return Ok(res);
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(IEnumerable<string> items)
        {
            var entries = await _context.SupplyEntries
                .Include(x => x.SupplyEntryItems)
                .Where(x => x.Status == ConstantHelpers.Warehouse.SupplyEntry.Status.CONFIRMED
                && x.IsValued == true).ToListAsync();

            foreach (var stringItem in items)
            {
                var stringSplit = stringItem.Split("|");

                if (stringSplit[0] == "undefined" || stringSplit[1] == "undefined")
                    return BadRequest("Quite lo filtros de la tabla de insumos para guardar");

                Guid id = Guid.Parse(stringSplit[0]);

                var aux = entries.FirstOrDefault(x => x.Id == id);

                if (aux == null)
                    return BadRequest("No se ha encontrado el ingreso");

                var isValued = Boolean.Parse(stringSplit[1]);

                if (isValued == true)
                    continue;
                if (isValued == false)
                {
                    aux.IsValued = false;
                    aux.ValuedMonth = 0;
                    aux.ValuedYear = 0;

                    foreach (var entryItem in aux.SupplyEntryItems)
                        entryItem.IsValued = false;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("items-ingreso/listar")]
        public async Task<IActionResult> GetSupplyEntryItems(Guid id)
        {
            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem.Order)
                .Include(x => x.OrderItem.Supply.MeasurementUnit)
                .Include(x => x.OrderItem.Supply)
                .Include(x => x.OrderItem.Supply.SupplyFamily)
                .Include(x => x.OrderItem.Supply.SupplyGroup)
                .Where(x => x.SupplyEntryId == id)
                .Select(x => new SupplyEntryItemViewModel
                {
                    Id = x.Id,
                    OrderItem = new OrderItemViewModel
                    {
                        Id = x.Id,
                        Supply = new SupplyViewModel
                        {
                            Description = x.OrderItem.Supply.Description,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Code = x.OrderItem.Supply.SupplyFamily.Code,
                                Name = x.OrderItem.Supply.SupplyFamily.Name
                            },
                            SupplyGroupId = x.OrderItem.Supply.SupplyGroupId,
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Code = x.OrderItem.Supply.SupplyGroup.Code,
                                Name = x.OrderItem.Supply.SupplyGroup.Name
                            },
                            CorrelativeCode = x.OrderItem.Supply.CorrelativeCode,
                            MeasurementUnit = new MeasurementUnitViewModel
                            {
                                Abbreviation = x.OrderItem.Supply.MeasurementUnit.Abbreviation
                            }
                        },
                        Order = new OrderViewModel
                        {
                            Status = x.OrderItem.Order.Status
                        },
                        Measure = x.OrderItem.Measure.ToString()
                    },
                    Measure = x.Measure.ToString(),
                    Observations = x.Observations,
                    PreviousAttention = x.PreviousAttention,
                    IsValued = x.IsValued
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(items);
        }


        [HttpGet("vale/{id}")]
        public async Task<IActionResult> Test(Guid id)
        {
            var items = await _context.SupplyEntryItems
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.Supply)
                .ThenInclude(x => x.SupplyFamily)
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.Supply)
                .ThenInclude(x => x.SupplyGroup)
                .Include(x => x.OrderItem)
                .ThenInclude(x => x.Supply)
                .ThenInclude(x => x.MeasurementUnit)
                .Include(x => x.SupplyEntry)
                .Where(x => x.SupplyEntryId == id)
                .ToListAsync();

            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == GetProjectId());

            var entry = items.FirstOrDefault().SupplyEntry;

            var summary = await _context.RequestSummaries.FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());
;
            //var request = items.FirstOrDefault().FieldRequest;

            //var usuario = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IssuedUserId);

            //var formulas = await _context.FieldRequestProjectFormulas
            //    .Include(x => x.ProjectFormula)
            //    .Where(x => x.FieldRequestId == id)
            //    .Select(x => x.ProjectFormula.Code + " - " + x.ProjectFormula.Name)
            //    .ToListAsync();

            var stocks = await _context.Stocks
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            using (var ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms))
                {
                    int limite = 142;
                    int currentPag = 1;
                    int maxPag = (int)Math.Ceiling((double)items.Count() / 25);
                    int itemNumber = 1;
                    int aux = limite;
                    var limiteItems = 23;

                    var tLine = "";
                    var tLine2 = "";
                    var tLine3 = "";

                    var tAux = "";

                    void Cabecera()
                    {
                        tLine = "ERP-IVC";

                        tAux = " CODIGO : CSH/GAL-For-01";

                        tLine += new string(' ', limite - tAux.Length - tLine.Length);
                        tLine += tAux;

                        tw.WriteLine(tLine);

                        /// -----------------------
                        tLine = project.Abbreviation;
                        tAux = "GESTIÓN DE ALMACÉN";
                        aux = limite / 2;
                        aux -= (tAux.Length / 2);
                        aux -= tLine.Length;
                        while (aux > 0)
                        {
                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        aux = limite;
                        tAux = $"VERSIÓN :       2       ";
                        aux -= tLine.Length;
                        aux -= tAux.Length;
                        while (aux > 0)
                        {
                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tw.WriteLine(tLine);

                        /// -----------------------

                        tLine = "";

                        tAux = "  FECHA :  14/02/2022   ";

                        tLine += new string(' ', limite - tAux.Length);
                        tLine += tAux;

                        tw.WriteLine(tLine);

                        /// -----------------------

                        tAux = $"VALE DE SALIDA N° C-{summary.CodePrefix}-"
                            + entry.DocumentNumber.ToString("D6");
                        tLine = "";
                        aux = limite / 2;
                        aux -= (tAux.Length / 2);
                        while (aux > 0)
                        {

                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tAux = $" PÁGINA :    {currentPag} de {maxPag}     ";

                        aux = limite;
                        aux -= tLine.Length;
                        aux -= tAux.Length;
                        while (aux > 0)
                        {

                            tLine += " ";
                            aux -= 1;
                        }
                        tLine += tAux;
                        tw.WriteLine(tLine);

                        /// -----------------------

                        tw.WriteLine();
                        tw.WriteLine("    O.T.N°           :    "
                            + "-");
                        tw.WriteLine("    FÓRMULA          :    "
                            + "-");
                        tw.WriteLine("    FRENTE           :    "
                            + "-");
                        tw.WriteLine("    CUADRILLA        :    "
                            + "-");
                        tw.WriteLine("    COLABORADOR      :    "
                            + "-");
                        tw.WriteLine("    FECHA DE EMISIÓN :    "
                            + "-");
                        tw.WriteLine("    FECHA DE ENTREGA :    "
                            + "-");

                        tw.WriteLine();

                        tLine3 = " ------ ";

                        tLine = "  ITEM  ";
                        tAux = "CÓDIGO";

                        void Columna(int largo)
                        {
                            aux = largo / 2;
                            aux -= tAux.Length / 2;
                            while (aux > 0)
                            {
                                tLine += " ";
                                tLine3 += "-";
                                aux -= 1;
                            }

                            tLine += tAux;
                            tLine3 += new string('-', tAux.Length);

                            aux = largo / 2;
                            aux -= tAux.Length / 2;
                            while (aux > 0)
                            {
                                tLine += " ";
                                tLine3 += "-";
                                aux -= 1;
                            }

                            tLine += " ";
                            tLine3 += " ";
                        }

                        Columna(14);

                        tAux = "DESCRIPCIÓN";

                        Columna(70);

                        tAux = "UND";

                        Columna(9);

                        tAux = "FASE";

                        Columna(10);

                        tLine2 = new string(' ', tLine.Length + 1);

                        tAux = "CANTIDAD";
                        tLine2 += "SOLICITADA";
                        tLine2 += new string(' ', 4);

                        Columna(12);

                        tAux = "CANTIDAD";
                        tLine2 += "ENTREGADA";

                        Columna(12);

                        tw.WriteLine(tLine);
                        tw.WriteLine(tLine2);
                        tw.WriteLine(tLine3);
                    }

                    Cabecera();

                    var counter = 1;
                    var monto = 0.0;

                    foreach (var item in items)
                    {
                        monto += Math.Round(stocks
                            .FirstOrDefault(x => x.SupplyId == item.OrderItem.SupplyId)
                            .UnitPrice * item.Measure, 2);

                        if (counter > limiteItems)
                        {
                            tLine3 = " ";
                            tLine3 += new string('-', limite - 2);
                            tw.WriteLine(tLine3);
                            limiteItems += limiteItems;
                            currentPag++;
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            tw.WriteLine();
                            Cabecera();
                        }

                        var isLong = false;
                        tLine = "  ";
                        tLine += itemNumber.ToString("D4");
                        tLine += "   ";
                        tLine += item.OrderItem.Supply.FullCode;
                        tLine += "    ";

                        tLine2 = new string(' ', tLine.Length);
                        tLine2 += "     ";

                        var description = item.OrderItem.Supply.Description;


                        counter++;
                        if (item.OrderItem.Supply.Description.Length > 67)
                        {
                            isLong = true;
                            tLine += description.Substring(0, 67);
                            tLine2 += description.Substring(67, description.Length - 67);
                            counter++;
                        }
                        else
                        {
                            tLine += description;
                        }

                        while (tLine.Length < 94)
                        {
                            tLine += " ";
                        }

                        tLine += "   ";
                        tLine += item.OrderItem.Supply.MeasurementUnit.Abbreviation;

                        while (tLine.Length < 104)
                        {
                            tLine += " ";
                        }

                        tLine += "   ";
                        tLine += "";

                        while (tLine.Length < 115)
                        {
                            tLine += " ";
                        }

                        var cant = item.Measure.ToString("N2", CultureInfo.InvariantCulture);

                        tLine += new string(' ', 11 - cant.Length);

                        tLine += cant;

                        tLine += "  ";

                        cant = item.Measure.ToString("N2", CultureInfo.InvariantCulture);

                        tLine += new string(' ', 11 - cant.Length);
                        tLine += cant;

                        itemNumber++;

                        tw.WriteLine(tLine);

                        if (isLong == true)
                            tw.WriteLine(tLine2);
                    }
                    /*
                    while (counter < limiteItems)
                    {
                        tw.WriteLine();
                        counter++;
                    }*/

                    tw.WriteLine();

                    tLine3 = new string(' ', 116);
                    tLine3 += "------------";
                    tLine3 += " ------------";
                    tw.WriteLine(tLine3);
                    tLine3 = new string(' ', 115);
                    var total = items.Sum(x => x.Measure).ToString("N2", CultureInfo.InvariantCulture);

                    tLine3 += new string(' ', 11 - total.Length);

                    tLine3 += total;


                    tLine3 += "  ";

                    total = items.Sum(x => x.Measure).ToString("N2", CultureInfo.InvariantCulture);

                    tLine3 += new string(' ', 11 - total.Length);
                    tLine3 += total;
                    tw.WriteLine(tLine3);
                    tLine3 = " ";
                    tLine3 += new string('-', limite - 2);
                    tw.WriteLine(tLine3);
                    /*
                    var zone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                    if(zone == null)
                        zone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                    DateTime time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, zone);
                    */

                    DateTime time = DateTime.UtcNow.ToDefaultTimeZone();

                    tw.WriteLine(" OBSERVACIONES:  " + "");
                    tw.WriteLine();
                    tw.WriteLine();
                    tLine3 = " AUTORIZANTE    :  " + "";

                    tLine3 += new string(' ', 55 - tLine3.Length);

                    tAux = " RECEPTOR  :  ________________________ ";

                    tLine3 += tAux;

                    tLine3 += new string(' ', 45 - tAux.Length);

                    tAux = " MONTO DESPACHO S/  :  ";

                    var cantAux = monto.ToString("N2", CultureInfo.InvariantCulture);

                    tAux += new string(' ', 16 - cantAux.Length);

                    tAux += cantAux;

                    tLine3 += tAux;
                    tw.WriteLine(tLine3);
                    tw.WriteLine();
                    tLine3 = " FECHA DESPACHO :  " + time.Date.ToDateString();

                    tLine3 += new string(' ', 55 - tLine3.Length);

                    tAux = " DNI       :  ________________________ ";

                    tLine3 += tAux;
                    tw.WriteLine(tLine3);
                    tw.WriteLine();
                    tLine3 = " HORA           :  " + time.ToString("HH:mm");

                    tLine3 += new string(' ', 55 - tLine3.Length);

                    tAux = " FIRMA     :  ________________________ ";

                    tLine3 += tAux;
                    tw.WriteLine(tLine3);




                    tw.Flush();
                    ms.Position = 0;


                    return File(ms.ToArray(), "text/plain", entry.DocumentNumber.ToString("D6") + ".txt");
                }

            }
        }
    }
}
