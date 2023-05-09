using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Aggregation;
using IVC.PE.WEB.Areas.Aggregation.ViewModels.AggregationVariableViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Aggregation.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Aggregation.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.AGGREGATION)]
    [Route("agregados/variables")]
    public class AggregationVariableController : BaseController
    {
        public AggregationVariableController(IvcDbContext context,
            ILogger<AggregationVariableController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("formato-carga")]
        public FileResult DownloadLoadExcelModel()
        {
            string fileName = "agregados_variables.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Tipos_Proveedor");
                workSheet.Cell($"A1").Value = "Nombre";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "Suministrador";
                workSheet.Cell($"A3").Value = "Transportista";

                workSheet = wb.Worksheets.Add("Proveedores");
                workSheet.Cell($"A1").Value = "Proveedores";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "Coorporación Tres Angeles";
                workSheet.Cell($"B1").Value = "Placas";
                workSheet.Cell("B1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("B1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"B2").Value = "F0W-125";
                workSheet.Cell($"C1").Value = "Volumen (m3)";
                workSheet.Cell("C1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("C1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"C2").Value = "20.00";

                workSheet = wb.Worksheets.Add("Tipos_Material");
                workSheet.Cell($"A1").Value = "Nombre";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "Agregados";
                workSheet.Cell($"A3").Value = "Agua";
                workSheet.Cell($"A3").Value = "Premezclado";

                workSheet = wb.Worksheets.Add("Materiales");
                workSheet.Cell($"A1").Value = "Tipo de Material";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "Agregados";
                workSheet.Cell($"A3").Value = "Agregados";
                workSheet.Cell($"A4").Value = "Agregados";
                workSheet.Cell($"A5").Value = "Agregados";
                workSheet.Cell($"B1").Value = "Producto";
                workSheet.Cell("B1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("B1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"B2").Value = "Arena de Cama";
                workSheet.Cell($"B3").Value = "Relleno Estructural";
                workSheet.Cell($"B4").Value = "Arena Gruesa";
                workSheet.Cell($"B5").Value = "Arena fina";

                workSheet = wb.Worksheets.Add("Partidas");
                workSheet.Cell($"A1").Value = "Partida";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "Acopio -> Cantera";
                workSheet.Cell($"A3").Value = "Acopio -> Frente G1";
                workSheet.Cell($"A4").Value = "Acopio -> Frente G2";

                workSheet = wb.Worksheets.Add("Preciario");
                workSheet.Cell($"A1").Value = "Tipo de Material";
                workSheet.Cell("A1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"A2").Value = "Agregados";
                workSheet.Cell($"A3").Value = "Agregados";
                workSheet.Cell($"A4").Value = "Agregados";
                workSheet.Cell($"A5").Value = "Agregados";
                workSheet.Cell($"B1").Value = "Producto";
                workSheet.Cell("B1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("B1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"B2").Value = "Arena de Cama";
                workSheet.Cell($"B3").Value = "Relleno Estructural";
                workSheet.Cell($"B4").Value = "Arena Gruesa";
                workSheet.Cell($"B5").Value = "Arena fina";
                workSheet.Cell($"C1").Value = "Partida";
                workSheet.Cell("C1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("C1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"C2").Value = "Acopio -> Cantera";
                workSheet.Cell($"C3").Value = "Acopio -> Frente G1";
                workSheet.Cell($"C4").Value = "Acopio -> Frente G2";
                workSheet.Cell($"D1").Value = "Preciario (S/)";
                workSheet.Cell("D1").Style.Font.SetFontColor(XLColor.White);
                workSheet.Cell("D1").Style.Fill.SetBackgroundColor(XLColor.Navy);
                workSheet.Cell($"D2").Value = "Data a cargar...";


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportVariables(IFormFile file)
        {
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var providerTypesWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("TIPOS_PROVEEDOR"));
                    var providersWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("PROVEEDORES"));
                    var stockTypesWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("TIPOS_MATERIAL"));
                    var stocksWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("MATERIALES"));
                    var entriesWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("PARTIDAS"));
                    var pricesWs = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper().Equals("PRECIARIO"));

                    var providerTypesDb = await _context.AggregationProviderTypes.ToListAsync();
                    var providersDb = await _context.AggregationProviders.ToListAsync();
                    var stockTypesDb = await _context.AggregationStockTypes.ToListAsync();
                    var stocksDb = await _context.AggregationStocks.ToListAsync();
                    var entriesDb = await _context.AggregationEntries.ToListAsync();
                    var pricesDb = await _context.AggregationPrices.ToListAsync();

                    if (providerTypesWs != null)
                        LoadProviderTypes(providerTypesWs, providerTypesDb);
                    if (providersWs != null)
                        LoadProviders(providersWs, providersDb);
                    if (stockTypesWs != null)
                        LoadStockTypes(stockTypesWs, stockTypesDb);
                    if (stocksWs != null)
                        LoadStocks(stocksWs, stocksDb, stockTypesDb);
                    if (entriesWs != null)
                        LoadEntries(entriesWs, entriesDb);
                    if (pricesWs != null)
                        LoadPrices(pricesWs, pricesDb, stockTypesDb, stocksDb, entriesDb);
                    
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        private async void LoadPrices(IXLWorksheet workSheet, List<AggregationPrice> myListDb, List<AggregationStockType> myTypesDb, List<AggregationStock> myStocksDb, List<AggregationEntry> myEntriesDb)
        {
            var counter = 2;
            var prices = new List<AggregationPrice>();
            while (!workSheet.Cell($"A{counter}").IsEmpty())
            {
                var stockType = workSheet.Cell($"A{counter}").GetString();
                var existType = myTypesDb.FirstOrDefault(x => x.Description.ToUpper().Equals(stockType.ToUpper()));
                if (existType == null)
                {
                    ++counter;
                    continue;
                }
                var stock = workSheet.Cell($"B{counter}").GetString();
                var existStock = myStocksDb.FirstOrDefault(x => x.Description.ToUpper().Equals(stock.ToUpper()));
                if (existStock == null)
                {
                    ++counter;
                    continue;
                }
                var entry = workSheet.Cell($"C{counter}").GetString();
                var existEntry = myEntriesDb.FirstOrDefault(x => x.Name.ToUpper().Equals(entry.ToUpper()));
                if (existEntry == null)
                {
                    ++counter;
                    continue;
                }
                var exist = myListDb.FirstOrDefault(x =>
                        x.AggregationStockTypeId == existType.Id &&
                        x.AggregationStockId == existStock.Id &&
                        x.AggregationEntryId == existEntry.Id
                    );
                if (exist != null)
                {
                    ++counter;
                    continue;
                }
                var price = workSheet.Cell($"D{counter}").GetString();
                prices.Add(new AggregationPrice
                {
                    AggregationStockTypeId = existType.Id,
                    AggregationStockId = existStock.Id,
                    AggregationEntryId = existEntry.Id,
                    Price = 0.00
                });
                ++counter;
            }
            await _context.AggregationPrices.AddRangeAsync(prices);
        }

        private async void LoadEntries(IXLWorksheet workSheet, List<AggregationEntry> myListDb)
        {
            var counter = 2;
            var entries = new List<AggregationEntry>();
            while (!workSheet.Cell($"A{counter}").IsEmpty())
            {
                var description = workSheet.Cell($"A{counter}").GetString();
                var exist = myListDb.FirstOrDefault(x => x.Name.ToUpper().Equals(description.ToUpper()));
                if (exist != null)
                {
                    ++counter;
                    continue;
                }
                entries.Add(new AggregationEntry
                {
                    Name = description
                });
                ++counter;
            }
            await _context.AggregationEntries.AddRangeAsync(entries);
        }

        private async void LoadStocks(IXLWorksheet workSheet, List<AggregationStock> myListDb, List<AggregationStockType> myTypesDb)
        {
            var counter = 2;
            var stocks = new List<AggregationStock>();
            while (!workSheet.Cell($"A{counter}").IsEmpty())
            {
                var type = workSheet.Cell($"A{counter}").GetString();
                var existType = myTypesDb.FirstOrDefault(x => x.Description.ToUpper().Equals(type.ToUpper()));
                if (existType == null)
                {
                    ++counter;
                    continue;
                }
                var description = workSheet.Cell($"B{counter}").GetString();
                var exist = myListDb.FirstOrDefault(x => x.Description.ToUpper().Equals(description.ToUpper()));
                if (exist != null)
                {
                    ++counter;
                    continue;
                }
                stocks.Add(new AggregationStock
                {
                    AggregationStockTypeId = existType.Id,
                    Description = description
                });
                ++counter;
            }
            await _context.AggregationStocks.AddRangeAsync(stocks);
        }

        private async void LoadStockTypes(IXLWorksheet workSheet, List<AggregationStockType> myListDb)
        {
            var counter = 2;
            var stockTypes = new List<AggregationStockType>();
            while (!workSheet.Cell($"A{counter}").IsEmpty())
            {
                var description = workSheet.Cell($"A{counter}").GetString();
                var exist = myListDb.FirstOrDefault(x => x.Description.ToUpper().Equals(description.ToUpper()));
                if (exist != null)
                {
                    ++counter;
                    continue;
                }
                stockTypes.Add(new AggregationStockType
                {
                    Description = description
                });
                ++counter;
            }
            await _context.AggregationStockTypes.AddRangeAsync(stockTypes);
        }

        private async void LoadProviders(IXLWorksheet workSheet, List<AggregationProvider> myListDb)
        {
            var counter = 2;
            var providers = new List<AggregationProvider>();
            while (!workSheet.Cell($"A{counter}").IsEmpty())
            {
                var description = workSheet.Cell($"A{counter}").GetString();
                var exist = myListDb.FirstOrDefault(x => x.Name.ToUpper().Equals(description.ToUpper()));
                if (exist != null)
                {
                    ++counter;
                    continue;
                }
                var licensePlate = workSheet.Cell($"B{counter}").GetString();
                var volume = workSheet.Cell($"C{counter}").GetString();
                providers.Add(new AggregationProvider
                {
                    Name = description,
                    LicensePlate = licensePlate,
                    Volume = volume
                });
                ++counter;
            }
            await _context.AggregationProviders.AddRangeAsync(providers);
        }

        private async void LoadProviderTypes(IXLWorksheet workSheet, List<AggregationProviderType> myListDb)
        {
            var counter = 2;            
            var providerTypes = new List<AggregationProviderType>();
            while (!workSheet.Cell($"A{counter}").IsEmpty())
            {
                var description = workSheet.Cell($"A{counter}").GetString();
                var exist = myListDb.FirstOrDefault(x => x.Description.ToUpper().Equals(description.ToUpper()));
                if (exist != null)
                {
                    ++counter;
                    continue;
                }
                providerTypes.Add(new AggregationProviderType
                {
                    Description = description
                });
                ++counter;
            }
            await _context.AggregationProviderTypes.AddRangeAsync(providerTypes);
        }

        #region ProviderTypes
        [HttpGet("tipos-proveedor/listar")]
        public async Task<IActionResult> GetAllProviderTypes()
        {
            var data = await _context.AggregationProviderTypes
                .Select(x => new AggregationProviderTypeViewModel
                {
                    Id = x.Id,
                    Description = x.Description
                })
                .ToListAsync();

            return Ok(data);
        }
        #endregion

        #region Providers
        [HttpGet("proveedor/listar")]
        public async Task<IActionResult> GetAllProviders()
        {
            var data = await _context.AggregationProviders
                .Select(x => new AggregationProviderViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    LicensePlate = x.LicensePlate,
                    Volume = x.Volume,
                    VolumeCertificate = x.VolumeCertificate
                })
                .ToListAsync();

            return Ok(data);
        }
        #endregion

        #region StockTypes
        [HttpGet("tipos-material/listar")]
        public async Task<IActionResult> GetAllStockTypes()
        {
            var data = await _context.AggregationStockTypes
                .Select(x => new AggregationStockTypeViewModel
                {
                    Id = x.Id,
                    Description = x.Description
                })
                .ToListAsync();

            return Ok(data);
        }
        #endregion

        #region Stocks
        [HttpGet("material/listar")]
        public async Task<IActionResult> GetAllStocks()
        {
            var data = await _context.AggregationStocks
                .Include(x => x.AggregationStockType)
                .Select(x => new AggregationStockViewModel
                {
                    Id = x.Id,
                    AggregationStockTypeId = x.AggregationStockTypeId,
                    AggregationStockType = new AggregationStockTypeViewModel
                    {
                        Description = x.AggregationStockType.Description
                    },
                    Description = x.Description,
                    QuarryApprovalCertificate = x.QuarryApprovalCertificate
                })
                .ToListAsync();

            return Ok(data);
        }
        #endregion

        #region Entries
        [HttpGet("partida/listar")]
        public async Task<IActionResult> GetAllEntries()
        {
            var data = await _context.AggregationEntries
                .Select(x => new AggregationEntryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return Ok(data);
        }
        #endregion

        #region Prices
        [HttpGet("preciario/listar")]
        public async Task<IActionResult> GetAllPrices()
        {
            var data = await _context.AggregationPrices
                .Include(x => x.AggregationStockType)
                .Include(x => x.AggregationStock)
                .Include(x => x.AggregationEntry)
                .Select(x => new AggregationPriceViewModel
                {
                    Id = x.Id,
                    AggregationStockTypeId = x.AggregationStockTypeId,
                    AggregationStockType = new AggregationStockTypeViewModel
                    {
                        Description = x.AggregationStockType.Description
                    },
                    AggregationStockId = x.AggregationStockId,
                    AggregationStock = new AggregationStockViewModel
                    {
                        Description = x.AggregationStock.Description
                    },
                    AggregationEntryId = x.AggregationEntryId,
                    AggregationEntry = new AggregationEntryViewModel
                    {
                        Name = x.AggregationEntry.Name
                    },
                    Price = x.Price.ToString("0.00")
                })
                .ToListAsync();

            return Ok(data);
        }
        #endregion
    }
}
