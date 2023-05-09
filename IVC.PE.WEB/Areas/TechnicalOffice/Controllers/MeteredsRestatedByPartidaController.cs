using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.MeteredsRestatedByPartidaViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/metrados-replanteo-por-partida")]
    public class MeteredsRestatedByPartidaController : BaseController
    {
        public MeteredsRestatedByPartidaController(IvcDbContext context,
            ILogger<MeteredsRestatedByPartidaController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();


        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? workFrontHeadId = null, Guid? budgetTitleId = null,
            Guid? workFrontId = null, Guid? sewerGroupId = null)
        {
            var pId = GetProjectId();

            var metereds = await _context.MeteredsRestatedByPartidas.AsNoTracking().ToListAsync();

            var query = new List<MeteredsRestatedByPartidaViewModel>();

            foreach (var item in metereds)
            {
                var folding = await _context.FoldingMeteredsRestatedByPartidas
                    .Include(x => x.SewerGroup)
                    .Where(x => x.MeteredsRestatedByPartidaId == item.Id)
                    .AsNoTracking().ToListAsync();

                var F501 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C01");
                var F501A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C01A");

                var F502 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C02");
                var F502A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C02A");

                var F503 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C03");
                var F503A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C03A");

                var F504 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C04");
                var F504A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C04A");

                var F505 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C05");
                var F505A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C05A");

                var F506 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C06");
                var F506A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C06A");

                var F507 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C07");
                var F507A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C07A");

                var F508 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C08");
                var F508A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C08A");

                var F509 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C09");
                var F509A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C09A");

                var F510 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C10");
                var F510A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C10A");

                var F511 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C11");
                var F511A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C11A");

                var F512 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C12");
                var F512A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C12A");

                var F513 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C13");
                var F513A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C13A");

                var F514 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C14");
                var F514A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C14A");

                var F515 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C15");
                var F515A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C15A");

                var F516 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C16");
                var F516A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C16A");

                var F517 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C17");
                var F517A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C17A");

                var F518 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C18");
                var F518A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C18A");

                var F519 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C19");
                var F519A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C19A");

                var F520 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C20");
                var F520A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C20A");

                var F521 = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C21");
                var F521A = folding.FirstOrDefault(x => x.SewerGroup.Code == "F5/6-C21A");

                var F5_C01Metered = "";

                var F5_C01Amount = "";

                var F5_C01AMetered = "";

                var F5_C01AAmount = "";

                var F5_C02Metered = "";

                var F5_C02Amount = "";

                var F5_C02AMetered = "";

                var F5_C02AAmount = "";

                var F5_C03Metered = "";

                var F5_C03Amount = "";

                var F5_C03AMetered = "";

                var F5_C03AAmount = "";

                var F5_C04Metered = "";

                var F5_C04Amount = "";

                var F5_C04AMetered = "";

                var F5_C04AAmount = "";

                var F5_C05Metered = "";

                var F5_C05Amount = "";

                var F5_C05AMetered = "";

                var F5_C05AAmount = "";

                var F5_C06Metered = "";

                var F5_C06Amount = "";

                var F5_C06AMetered = "";

                var F5_C06AAmount = "";

                var F5_C07Metered = "";

                var F5_C07Amount = "";

                var F5_C07AMetered = "";

                var F5_C07AAmount = "";

                var F5_C08Metered = "";

                var F5_C08Amount = "";

                var F5_C08AMetered = "";

                var F5_C08AAmount = "";

                var F5_C09Metered = "";

                var F5_C09Amount = "";

                var F5_C09AMetered = "";

                var F5_C09AAmount = "";

                var F5_C10Metered = "";

                var F5_C10Amount = "";

                var F5_C10AMetered = "";

                var F5_C10AAmount = "";

                var F5_C11Metered = "";

                var F5_C11Amount = "";

                var F5_C11AMetered = "";

                var F5_C11AAmount = "";

                var F5_C12Metered = "";

                var F5_C12Amount = "";

                var F5_C12AMetered = "";

                var F5_C12AAmount = "";

                var F5_C13Metered = "";

                var F5_C13Amount = "";

                var F5_C13AMetered = "";

                var F5_C13AAmount = "";

                var F5_C14Metered = "";

                var F5_C14Amount = "";

                var F5_C14AMetered = "";

                var F5_C14AAmount = "";

                var F5_C15Metered = "";

                var F5_C15Amount = "";

                var F5_C15AMetered = "";

                var F5_C15AAmount = "";

                var F5_C16Metered = "";

                var F5_C16Amount = "";

                var F5_C16AMetered = "";

                var F5_C16AAmount = "";

                var F5_C17Metered = "";

                var F5_C17Amount = "";

                var F5_C17AMetered = "";

                var F5_C17AAmount = "";

                var F5_C18Metered = "";

                var F5_C18Amount = "";

                var F5_C18AMetered = "";

                var F5_C18AAmount = "";

                var F5_C19Metered = "";

                var F5_C19Amount = "";

                var F5_C19AMetered = "";

                var F5_C19AAmount = "";

                var F5_C20Metered = "";

                var F5_C20Amount = "";

                var F5_C20AMetered = "";

                var F5_C20AAmount = "";

                var F5_C21Metered = "";

                var F5_C21Amount = "";

                var F5_C21AMetered = "";

                var F5_C21AAmount = "";

                if(F501 != null)
                {
                    F5_C01Metered = F501.Metered.ToString();

                    F5_C01Amount = F501.Amount.ToString();
                }

                if(F501A != null)
                {
                    F5_C01AMetered = F501A.Metered.ToString();

                    F5_C01AAmount = F501A.Amount.ToString();
                }

                if (F502 != null)
                {
                    F5_C02Metered = F502.Metered.ToString();

                    F5_C02Amount = F502.Amount.ToString();
                }

                if (F502A != null)
                {
                    F5_C02AMetered = F502A.Metered.ToString();

                    F5_C02AAmount = F502A.Amount.ToString();
                }

                if (F503 != null)
                {
                    F5_C03Metered = F503.Metered.ToString();

                    F5_C03Amount = F503.Amount.ToString();
                }

                if (F503A != null)
                {
                    F5_C03AMetered = F503A.Metered.ToString();

                    F5_C03AAmount = F503A.Amount.ToString();
                }

                if (F504 != null)
                {
                    F5_C04Metered = F504.Metered.ToString();

                    F5_C04Amount = F504.Amount.ToString();
                }

                if (F504A != null)
                {
                    F5_C04AMetered = F504A.Metered.ToString();

                    F5_C04AAmount = F504A.Amount.ToString();
                }

                if (F505 != null)
                {
                    F5_C05Metered = F505.Metered.ToString();

                    F5_C05Amount = F505.Amount.ToString();
                }

                if (F505A != null)
                {
                    F5_C05AMetered = F505A.Metered.ToString();

                    F5_C05AAmount = F505A.Amount.ToString();
                }

                if (F506 != null)
                {
                    F5_C06Metered = F506.Metered.ToString();

                    F5_C06Amount = F506.Amount.ToString();
                }

                if (F506A != null)
                {
                    F5_C06AMetered = F506A.Metered.ToString();

                    F5_C06AAmount = F506A.Amount.ToString();
                }

                if (F507 != null)
                {
                    F5_C07Metered = F507.Metered.ToString();

                    F5_C07Amount = F507.Amount.ToString();
                }

                if (F507A != null)
                {
                    F5_C07AMetered = F507A.Metered.ToString();

                    F5_C07AAmount = F507A.Amount.ToString();
                }

                if (F508 != null)
                {
                    F5_C08Metered = F508.Metered.ToString();

                    F5_C08Amount = F508.Amount.ToString();
                }

                if (F508A != null)
                {
                    F5_C08AMetered = F508A.Metered.ToString();

                    F5_C08AAmount = F508A.Amount.ToString();
                }

                if (F509 != null)
                {
                    F5_C09Metered = F509.Metered.ToString();

                    F5_C09Amount = F509.Amount.ToString();
                }

                if (F509A != null)
                {
                    F5_C09AMetered = F509A.Metered.ToString();

                    F5_C09AAmount = F509A.Amount.ToString();
                }

                if (F510 != null)
                {
                    F5_C10Metered = F510.Metered.ToString();

                    F5_C10Amount = F510.Amount.ToString();
                }

                if (F510A != null)
                {
                    F5_C10AMetered = F510A.Metered.ToString();

                    F5_C10AAmount = F510A.Amount.ToString();
                }

                if (F511 != null)
                {
                    F5_C11Metered = F511.Metered.ToString();

                    F5_C11Amount = F511.Amount.ToString();
                }

                if (F511A != null)
                {
                    F5_C11AMetered = F511A.Metered.ToString();

                    F5_C11AAmount = F511A.Amount.ToString();
                }

                if (F512 != null)
                {
                    F5_C12Metered = F512.Metered.ToString();

                    F5_C12Amount = F512.Amount.ToString();
                }

                if (F512A != null)
                {
                    F5_C12AMetered = F512A.Metered.ToString();

                    F5_C12AAmount = F512A.Amount.ToString();
                }

                if (F513 != null)
                {
                    F5_C13Metered = F513.Metered.ToString();

                    F5_C13Amount = F513.Amount.ToString();
                }

                if (F513A != null)
                {
                    F5_C13AMetered = F513A.Metered.ToString();

                    F5_C13AAmount = F513A.Amount.ToString();
                }

                if (F514 != null)
                {
                    F5_C14Metered = F514.Metered.ToString();

                    F5_C14Amount = F514.Amount.ToString();
                }

                if (F514A != null)
                {
                    F5_C14AMetered = F514A.Metered.ToString();

                    F5_C14AAmount = F514A.Amount.ToString();
                }

                if (F515 != null)
                {
                    F5_C15Metered = F515.Metered.ToString();

                    F5_C15Amount = F515.Amount.ToString();
                }

                if (F515A != null)
                {
                    F5_C15AMetered = F515A.Metered.ToString();

                    F5_C15AAmount = F515A.Amount.ToString();
                }

                if (F516 != null)
                {
                    F5_C16Metered = F516.Metered.ToString();

                    F5_C16Amount = F516.Amount.ToString();
                }

                if (F516A != null)
                {
                    F5_C16AMetered = F516A.Metered.ToString();

                    F5_C16AAmount = F516A.Amount.ToString();
                }

                if (F517 != null)
                {
                    F5_C17Metered = F517.Metered.ToString();

                    F5_C17Amount = F517.Amount.ToString();
                }

                if (F517A != null)
                {
                    F5_C17AMetered = F517A.Metered.ToString();

                    F5_C17AAmount = F517A.Amount.ToString();
                }

                if (F518 != null)
                {
                    F5_C18Metered = F518.Metered.ToString();

                    F5_C18Amount = F518.Amount.ToString();
                }

                if (F518A != null)
                {
                    F5_C18AMetered = F518A.Metered.ToString();

                    F5_C18AAmount = F518A.Amount.ToString();
                }

                if (F519 != null)
                {
                    F5_C19Metered = F519.Metered.ToString();

                    F5_C19Amount = F519.Amount.ToString();
                }

                if (F519A != null)
                {
                    F5_C19AMetered = F519A.Metered.ToString();

                    F5_C19AAmount = F519A.Amount.ToString();
                }

                if (F520 != null)
                {
                    F5_C20Metered = F520.Metered.ToString();

                    F5_C20Amount = F520.Amount.ToString();
                }

                if (F520A != null)
                {
                    F5_C20AMetered = F520A.Metered.ToString();

                    F5_C20AAmount = F520A.Amount.ToString();
                }

                if(F521 != null)
                {
                    F5_C21Metered = F521.Metered.ToString();

                    F5_C21Amount = F521.Amount.ToString();
                }

                if(F521A != null)
                {
                    F5_C21AMetered = F521A.Metered.ToString();

                    F5_C21AAmount = F521A.Amount.ToString();
                }

                query.Add(new MeteredsRestatedByPartidaViewModel
                {
                    Id = item.Id,
                    WorkFrontHeadId = item.WorkFrontHeadId,
                    BudgetTitleId = item.BudgetTitleId,
                    WorkFrontId = item.WorkFrontId,
                    SewerGroupId = item.SewerGroupId,
                    ItemNumber = item.ItemNumber,
                    Description = item.Description,
                    Unit = item.Unit,
                    Metered = item.Metered.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = item.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = item.Parcial.ToString("N", CultureInfo.InvariantCulture),
                    F5_C01Amount = F5_C01Amount,
                    F5_C01Metered = F5_C01Metered,
                    F5_C01AAmount = F5_C01AAmount,
                    F5_C01AMetered = F5_C01AMetered,
                    F5_C02Amount = F5_C02Amount,
                    F5_C02Metered = F5_C02Metered,
                    F5_C02AAmount = F5_C02AAmount,
                    F5_C02AMetered = F5_C02AMetered,
                    F5_C03Amount = F5_C03Amount,
                    F5_C03Metered = F5_C03Metered,
                    F5_C03AAmount = F5_C03AAmount,
                    F5_C03AMetered = F5_C03AMetered,
                    F5_C04Amount = F5_C04Amount,
                    F5_C04Metered = F5_C04Metered,
                    F5_C04AAmount = F5_C04AAmount,
                    F5_C04AMetered = F5_C04AMetered,
                    F5_C05Amount = F5_C05Amount,
                    F5_C05Metered = F5_C05Metered,
                    F5_C05AAmount = F5_C05AAmount,
                    F5_C05AMetered = F5_C05AMetered,
                    F5_C06Amount = F5_C06Amount,
                    F5_C06Metered = F5_C06Metered,
                    F5_C06AAmount = F5_C06AAmount,
                    F5_C06AMetered = F5_C06AMetered,
                    F5_C07Amount = F5_C07Amount,
                    F5_C07Metered = F5_C07Metered,
                    F5_C07AAmount = F5_C07AAmount,
                    F5_C07AMetered = F5_C07AMetered,
                    F5_C08Amount = F5_C08Amount,
                    F5_C08Metered = F5_C08Metered,
                    F5_C08AAmount = F5_C08AAmount,
                    F5_C08AMetered = F5_C08AMetered,
                    F5_C09Amount = F5_C09Amount,
                    F5_C09Metered = F5_C09Metered,
                    F5_C09AAmount = F5_C09AAmount,
                    F5_C09AMetered = F5_C09AMetered,
                    F5_C10Amount = F5_C10Amount,
                    F5_C10Metered = F5_C10Metered,
                    F5_C10AAmount = F5_C10AAmount,
                    F5_C10AMetered = F5_C10AMetered,
                    F5_C11Amount = F5_C11Amount,
                    F5_C11Metered = F5_C11Metered,
                    F5_C11AAmount = F5_C11AAmount,
                    F5_C11AMetered = F5_C11AMetered,
                    F5_C12Amount = F5_C12Amount,
                    F5_C12Metered = F5_C12Metered,
                    F5_C12AAmount = F5_C12AAmount,
                    F5_C12AMetered = F5_C12AMetered,
                    F5_C13Amount = F5_C13Amount,
                    F5_C13Metered = F5_C13Metered,
                    F5_C13AAmount = F5_C13AAmount,
                    F5_C13AMetered = F5_C13AMetered,
                    F5_C14Amount = F5_C14Amount,
                    F5_C14Metered = F5_C14Metered,
                    F5_C14AAmount = F5_C14AAmount,
                    F5_C14AMetered = F5_C14AMetered,
                    F5_C15Amount = F5_C15Amount,
                    F5_C15Metered = F5_C15Metered,
                    F5_C15AAmount = F5_C15AAmount,
                    F5_C15AMetered = F5_C15AMetered,
                    F5_C16Amount = F5_C16Amount,
                    F5_C16Metered = F5_C16Metered,
                    F5_C16AAmount = F5_C16AAmount,
                    F5_C16AMetered = F5_C16AMetered,
                    F5_C17Amount = F5_C17Amount,
                    F5_C17Metered = F5_C17Metered,
                    F5_C17AAmount = F5_C17AAmount,
                    F5_C17AMetered = F5_C17AMetered,
                    F5_C18Amount = F5_C18Amount,
                    F5_C18Metered = F5_C18Metered,
                    F5_C18AAmount = F5_C18AAmount,
                    F5_C18AMetered = F5_C18AMetered,
                    F5_C19Amount = F5_C19Amount,
                    F5_C19Metered = F5_C19Metered,
                    F5_C19AAmount = F5_C19AAmount,
                    F5_C19AMetered = F5_C19AMetered,
                    F5_C20Amount = F5_C20Amount,
                    F5_C20Metered = F5_C20Metered,
                    F5_C20AAmount = F5_C20AAmount,
                    F5_C20AMetered = F5_C20AMetered,
                    F5_C21Amount = F5_C21Amount,
                    F5_C21Metered = F5_C21Metered,
                    F5_C21AAmount = F5_C21AAmount,
                    F5_C21AMetered = F5_C21AMetered,
                    AccumulatedMetered = item.AccumulatedMetered.ToString("N", CultureInfo.InvariantCulture),
                    AccumulatedAmount = item.AccumulatedAmount.ToString("N", CultureInfo.InvariantCulture)
                });
            }

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var pId = GetProjectId();

            var data = await _context.MeteredsRestatedByPartidas
                .Include(x => x.WorkFrontHead)
                .Include(x => x.BudgetTitle)
                .Include(x => x.WorkFront)
                .Include(x => x.SewerGroup)
                .Select(x => new MeteredsRestatedByPartidaViewModel
                {
                    Id = x.Id,
                    WorkFrontHeadId = x.WorkFrontHeadId,
                    BudgetTitleId = x.BudgetTitleId,
                    WorkFrontId = x.WorkFrontId,
                    SewerGroupId = x.SewerGroupId,
                    ItemNumber = x.ItemNumber,
                    Description = x.Description,
                    Unit = x.Unit,
                    Metered = x.Metered.ToString("N", CultureInfo.InvariantCulture),
                    UnitPrice = x.UnitPrice.ToString("N", CultureInfo.InvariantCulture),
                    Parcial = x.Parcial.ToString("N", CultureInfo.InvariantCulture),
                    AccumulatedMetered = x.AccumulatedMetered.ToString("N", CultureInfo.InvariantCulture),
                    AccumulatedAmount = x.AccumulatedAmount.ToString("N", CultureInfo.InvariantCulture)
                }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(data);
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportData(IFormFile file, MeteredsRestatedByPartidaViewModel model)
        {
            var aux = _context.MeteredsRestatedByPartidas.Count();

            var metereds = new List<MeteredsRestatedByPartida>();

            var foldings = new List<FoldingMeteredsRestatedByPartida>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using(var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;

                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var item = new MeteredsRestatedByPartida();

                        var folding = new FoldingMeteredsRestatedByPartida();

                        var existeItem = await _context.MeteredsRestatedByPartidas
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x=>x.ItemNumber == workSheet.Cell($"B{counter}").GetString());

                        var meteredExcel = workSheet.Cell($"E{counter}").GetString();

                        var itemNumberExcel = workSheet.Cell($"B{counter}").GetString();

                        var budget = await _context.Budgets
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.NumberItem == itemNumberExcel && x.UnitPrice != 0);

                        if (budget == null)
                            return BadRequest("No existe el presupuestos " + itemNumberExcel);

                        if (existeItem == null)
                        {
                            item.Id = Guid.NewGuid();
                            item.ItemNumber = workSheet.Cell($"B{counter}").GetString();

                            item.WorkFrontHeadId = model.WorkFrontHeadId;
                            item.BudgetTitleId = model.BudgetTitleId;
                            item.WorkFrontId = model.WorkFrontId;
                            item.SewerGroupId = model.SewerGroupId;
                            item.Description = workSheet.Cell($"C{counter}").GetString();
                            item.Unit = workSheet.Cell($"D{counter}").GetString();
                            item.Metered = budget.Metered;
                            item.UnitPrice = budget.UnitPrice;
                            item.Parcial = budget.TotalPrice;

                            metereds.Add(item);

                            folding.Id = Guid.NewGuid();
                            folding.SewerGroupId = model.SewerGroupId;
                            folding.MeteredsRestatedByPartidaId = item.Id;

                            if (!string.IsNullOrEmpty(meteredExcel))
                            {
                                if (Double.TryParse(meteredExcel, out double metered))
                                    folding.Metered = metered;
                            }

                            folding.Amount = Math.Round(folding.Metered * budget.UnitPrice, 2);

                            foldings.Add(folding);

                        }
                        else
                        {
                            folding.Id = Guid.NewGuid();
                            folding.SewerGroupId = model.SewerGroupId;
                            folding.MeteredsRestatedByPartidaId = existeItem.Id;

                            if (!string.IsNullOrEmpty(meteredExcel))
                            {
                                if (Double.TryParse(meteredExcel, out double metered))
                                    folding.Metered = metered;
                            }

                            folding.Amount = Math.Round(folding.Metered * budget.UnitPrice, 2);

                            foldings.Add(folding);
                        }

                        counter++;
                        aux++;
                    }
                }
                mem.Close();
            }

            await _context.MeteredsRestatedByPartidas.AddRangeAsync(metereds);
            await _context.FoldingMeteredsRestatedByPartidas.AddRangeAsync(foldings);
            await _context.SaveChangesAsync();

            var meteredsExistentes = await _context.MeteredsRestatedByPartidas.ToListAsync();

            foreach(var item in meteredsExistentes)
            {
                var foldingsExistentes = await _context.FoldingMeteredsRestatedByPartidas.Where(x => x.MeteredsRestatedByPartidaId == item.Id).ToListAsync();
                var sumaMetered = 0.0;
                var sumaAmount = 0.0;
                foreach(var folding in foldingsExistentes)
                {
                    sumaAmount += folding.Amount;
                    sumaMetered += folding.Metered;
                }
                item.AccumulatedMetered = sumaMetered;
                item.AccumulatedAmount = sumaAmount;
            }

            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpGet("excel-carga-masiva")]
        public FileResult ExportExcelMassiveLoad()
        {
            string fileName = "MetradoReplanteoCarga.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"B2").Value = "Item";
                workSheet.Cell($"C2").Value = "Descripción";
                workSheet.Cell($"D2").Value = "Und.";
                workSheet.Cell($"E2").Value = "Metrado";

                workSheet.Cell($"B3").Value = "Info Aquí";
                workSheet.Cell($"B3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 13;
                workSheet.Column(3).Width = 67;
                workSheet.Column(4).Width = 13;
                workSheet.Column(5).Width = 17;

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range("B2:E9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:E9").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _context.MeteredsRestatedByPartidas.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return BadRequest("No se ha encontrado el Metrado Replanto por Partida");

            var foldings = await _context.FoldingMeteredsRestatedByPartidas
                .Where(x => x.MeteredsRestatedByPartidaId == entity.Id)
                .ToListAsync();

            _context.FoldingMeteredsRestatedByPartidas.RemoveRange(foldings);

            _context.MeteredsRestatedByPartidas.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
