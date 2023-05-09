using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.DATA.Migrations;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;

using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondLoadViewModel;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels;
using System.Security.Cryptography.Xml;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondAddViewModels;
using IVC.PE.WEB.Services;
using Microsoft.Extensions.Options;
using IVC.PE.WEB.Options;
using IVC.PE.ENTITIES.UspModels.Finances;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondRenovationViewModels;
using Newtonsoft.Json;
using System.Data;
using ClosedXML.Excel;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.FINANCE)]
    [Route("finanzas/cartas-fianza")]
    public class BondAddController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public BondAddController(IvcDbContext context,
             IOptions<CloudStorageCredentials> storageCredentials,
           ILogger<BondAddController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(bool? last,Guid? projectId = null, Guid? bankId = null, Guid? bondTypeId = null, Guid? budgetTitleId = null,Guid? bondGuarantorId = null)
        {
            var pId = GetProjectId();

            var bonds = await _context.Set<UspBonds>().FromSqlRaw("execute Finances_uspBonds")
                .IgnoreQueryFilters()
                .ToListAsync();

            bonds = bonds.Where(x => x.ProjectId == pId).ToList();

            //if (projectId.HasValue)
            //    bonds = bonds.Where(x => x.ProjectId == projectId.Value).ToList();

            if (bankId.HasValue)
                bonds = bonds.Where(x => x.BankId == bankId.Value).ToList();

            if (bondTypeId.HasValue)
                bonds = bonds.Where(x => x.BondTypeId == bondTypeId.Value).ToList();

            if (budgetTitleId.HasValue)
                bonds = bonds.Where(x => x.BudgetTitleId == budgetTitleId.Value).ToList();

            if (last.HasValue)

            {
                if (last == false)
                    bonds = bonds.Where(x => x.IsTheLast == false).ToList();
                if (last == true)
                    bonds = bonds.Where(x => x.IsTheLast == true).ToList();
            }

            if (bondGuarantorId.HasValue)
                bonds = bonds.Where(x => x.BondGuarantorId == bondGuarantorId.Value).ToList();
            return Ok(bonds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var bond = await _context.BondAdds
                .Include(x => x.Bank)
                .Include(x => x.BondGuarantor)
                .Include(x => x.BondType)
                .Include(x => x.BudgetTitle)
                .Where(x => x.Id == id)
                .Select(x => new BondAddViewModel
                {
                    Id = x.Id,
                    BankId = x.BankId,
                    Bank = new BankViewModel
                    {
                        Id = x.BankId,
                        Name = x.Bank.Name
                    },
                    BondGuarantorId = x.BondGuarantorId,
                    BondGuarantor = new BondGuarantorViewModel
                    {
                        Id = x.BondGuarantorId,
                        Name = x.BondGuarantor.Name
                    },
                    BondTypeId = x.BondTypeId,
                    BondType = new BondTypeViewModel
                    {
                        Id = x.BondTypeId,
                        Name = x.BondType.Name
                    },
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Id = x.BudgetTitleId,
                        Name = x.BudgetTitle.Name,
                        Abbreviation = x.BudgetTitle.Abbreviation
                    },
                    BondNumber = x.BondNumber,
                    NumberOfRenovations = x.NumberOfRenovations,
                }).FirstOrDefaultAsync();

            var bondRenovation = await _context.BondRenovations
                .Where(x => x.BondAddId == bond.Id && x.BondOrder == bond.NumberOfRenovations)
                .Select(x => new BondRenovationViewModel
                {
                    Id = x.Id,
                    BondAddId = x.BondAddId,
                    BondOrder = x.BondOrder,
                    CreateDate = x.CreateDate.ToDateString(),
                    currencyType = x.currencyType,
                    EndDate = x.EndDate.ToDateString(),
                    FileUrl = x.FileUrl,
                    IssueFileUrl = x.IssueFileUrl,
                    guaranteeDesc = x.guaranteeDesc,
                    PenAmmount = x.PenAmmount,
                    UsdAmmount = x.UsdAmmount,
                    IsTheLast = x.IsTheLast,
                    BondName = x.BondName,
                    IssueCost = x.IssueCost,
                }).FirstOrDefaultAsync();

            var responsibles = await _context.BondRenovationApplicationUsers
                .Where(x => x.BondRenovationId == bondRenovation.Id)
                .Select(x => x.UserId)
                .ToListAsync();

            bondRenovation.Responsibles = responsibles;
            bond.BondRenovation = bondRenovation;

            return Ok(bond);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(BondAddViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bondAdd = new BondAdd
            {
                BankId = model.BankId,
                BondGuarantorId = model.BondGuarantorId,
                BondTypeId = model.BondTypeId,
                BondNumber = model.BondNumber,
                BudgetTitleId = model.BudgetTitleId,
                NumberOfRenovations = 1,
                ProjectId = GetProjectId()
            };

            var bondRenovation = new BondRenovation
            {
                BondAdd = bondAdd,
                BondName = model.BondRenovation.BondName,
                BondOrder = bondAdd.NumberOfRenovations,
                CreateDate = model.BondRenovation.CreateDate.ToDateTime(),
                EndDate = model.BondRenovation.EndDate.ToDateTime(),
                PenAmmount = model.BondRenovation.PenAmmount,
                UsdAmmount = model.BondRenovation.UsdAmmount,
                currencyType = model.BondRenovation.currencyType,
                daysLimitTerm = model.BondRenovation.daysLimitTerm,
                guaranteeDesc = model.BondRenovation.guaranteeDesc,
                Days15 = false,
                Days30 = false,
                IsTheLast = model.BondRenovation.IsTheLast,
                IssueCost = model.BondRenovation.IssueCost
            };

            if (model.BondRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bondRenovation.FileUrl = await storage.UploadFile(model.BondRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.BondRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"carta_fianza_nro{bondAdd.BondNumber}-{bondRenovation.BondOrder}");
            }

            if (model.BondRenovation.IssueFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                bondRenovation.IssueFileUrl = await storage.UploadFile(model.BondRenovation.IssueFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.BondRenovation.IssueFile.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"costo_emision_nro{bondAdd.BondNumber}-{bondRenovation.BondOrder}");
            }

            await _context.BondRenovationApplicationUsers
                .AddRangeAsync(model.BondRenovation.Responsibles
                    .Select(x => new BondRenovationApplicationUser
                    {
                        BondRenovation = bondRenovation,
                        UserId = x
                    }));

            await _context.BondRenovations.AddAsync(bondRenovation);
            await _context.BondAdds.AddAsync(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, BondAddViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bondAdd = await _context.BondAdds.FirstOrDefaultAsync(x => x.Id == id);

            bondAdd.BankId = model.BankId;
            bondAdd.BondGuarantorId = model.BondGuarantorId;
            bondAdd.BondTypeId = model.BondTypeId;
            bondAdd.BudgetTitleId = model.BudgetTitleId;
            bondAdd.BondNumber = model.BondNumber;

            var bondRen = await _context.BondRenovations.FirstOrDefaultAsync(x => x.Id == model.BondRenovation.Id);

            bondRen.CreateDate = model.BondRenovation.CreateDate.ToDateTime();
            bondRen.EndDate = model.BondRenovation.EndDate.ToDateTime();
            bondRen.PenAmmount = model.BondRenovation.PenAmmount;
            bondRen.currencyType = model.BondRenovation.currencyType;
            bondRen.daysLimitTerm = model.BondRenovation.daysLimitTerm;
            bondRen.guaranteeDesc = model.BondRenovation.guaranteeDesc;
            bondRen.IsTheLast = model.BondRenovation.IsTheLast;
            bondRen.BondName = model.BondRenovation.BondName;
            bondRen.IssueCost = model.BondRenovation.IssueCost;

            if (model.BondRenovation.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bondRen.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{bondRen.FileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.FINANCE);
                bondRen.FileUrl = await storage.UploadFile(model.BondRenovation.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.BondRenovation.File.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"carta_fianza_nro{bondRen.BondName}-{bondRen.BondOrder}");
            }


            if (model.BondRenovation.IssueFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (bondRen.IssueFileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{bondRen.IssueFileUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.FINANCE);
                bondRen.IssueFileUrl = await storage.UploadFile(model.BondRenovation.IssueFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.FINANCE,
                    System.IO.Path.GetExtension(model.BondRenovation.IssueFile.FileName),
                    ConstantHelpers.Storage.Blobs.BOND_LETTERS,
                    $"costo_emision_nro{bondRen.BondName}-{bondRen.BondOrder}");
            }

            var renovationResponsiblesOld = await _context.BondRenovationApplicationUsers
                .Where(x => x.BondRenovationId == bondRen.Id)
                .ToListAsync();

            _context.BondRenovationApplicationUsers.RemoveRange(renovationResponsiblesOld);

            await _context.BondRenovationApplicationUsers.AddRangeAsync(
                model.BondRenovation.Responsibles.Select(x => new BondRenovationApplicationUser
                {
                    BondRenovation = bondRen,
                    UserId = x
                }));

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.BondAdds.FirstOrDefaultAsync(x => x.Id == id);

            var bondRens = await _context.BondRenovations.Where(x => x.BondAddId == id).ToListAsync();

            var bondUsers = await _context.BondRenovationApplicationUsers
                    .Include(x => x.BondRenovation)
                    .Where(x => x.BondRenovation.BondAddId == bondAdd.Id)
                    .ToListAsync();

            _context.BondRenovationApplicationUsers.RemoveRange(bondUsers);

            foreach (var renovation in bondRens)
            {
                if (renovation.FileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{renovation.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.FINANCE);
                }
            }

            foreach (var renovation in bondRens)
            {
                if (renovation.IssueFileUrl != null)
                {
                    var storage = new CloudStorageService(_storageCredentials);
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.BOND_LETTERS}/{renovation.IssueFileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.FINANCE);
                }
            }
            _context.BondRenovations.RemoveRange(bondRens);
            _context.BondAdds.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("reporte-excel-proyecto")]
        public async Task<IActionResult> GeneretaExcelByProject()
        {
            //var userId = GetUserId();
            var userProjectsClaim = GetUserProjects();

            var userProjects = JsonConvert.DeserializeObject<List<Project>>(userProjectsClaim.ToString());

            //var userProjectIds = await _context.UserProjects
            //    .Where(x => x.UserId == userId)
            //    .Select(x => x.ProjectId)
            //    .ToListAsync();

            var bonds = await _context.Set<UspBonds>().FromSqlRaw("execute Finances_uspBonds")
                .IgnoreQueryFilters()
                .ToListAsync();

            var bondsToExcel = new List<UspBonds>();
            foreach (var project in userProjects)
            {
                bondsToExcel.AddRange(bonds.Where(x => x.ProjectId == project.Id));
            }

            TempData["fianzas-pry"] = JsonConvert.SerializeObject(bondsToExcel);

            return Ok("fianzas-pry");
        }

        [HttpGet("reporte-excel-banco")]
        public async Task<IActionResult> GenerateExcelByBank()
        {
            var userProjectsClaim = GetUserProjects();

            var userProjects = JsonConvert.DeserializeObject<List<Project>>(userProjectsClaim.ToString());

            var bonds = await _context.Set<UspBonds>().FromSqlRaw("execute Finances_uspBonds")
                .IgnoreQueryFilters()
                .ToListAsync();

            var bondsToExcel = new List<UspBonds>();

            foreach (var project in userProjects)
            {
                bondsToExcel.AddRange(bonds.Where(x => x.ProjectId == project.Id));
            }

            TempData["fianzas-bnc"] = JsonConvert.SerializeObject(bondsToExcel);

            return Ok("fianzas-bnc");
        }

        [HttpGet("reporte-excel-historico")]
        public async Task<IActionResult> GenerateExcelHistoric()
        {
            var userProjectsClaim = GetUserProjects();

            var userProjects = JsonConvert.DeserializeObject<List<Project>>(userProjectsClaim.ToString());

            var bonds = await _context.Set<UspBondsAll>().FromSqlRaw("execute Finances_uspBondsAll")
                .IgnoreQueryFilters()
                .ToListAsync();

            var bondsToExcel = new List<UspBondsAll>();

            foreach (var project in userProjects)
            {
                bondsToExcel.AddRange(bonds.Where(x => x.ProjectId == project.Id));
            }

            TempData["fianzas-todas"] = JsonConvert.SerializeObject(bondsToExcel);

            return Ok("fianzas-todas");
        }

        [HttpGet("descargar-excel")]
        public FileResult DownloadExcel(string excelName)
        {
            if (TempData[excelName] == null)
            {
                RedirectToAction("Empty");
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Cartas Fianza");

                string fileName = string.Empty;
                switch (excelName)
                {
                    case "fianzas-pry":
                        var data_pry = JsonConvert.DeserializeObject<List<UspBonds>>(TempData["fianzas-pry"].ToString());
                        fileName = "FianzaProyecto.xlsx";
                        ExcelByProject(ws, data_pry);
                        break;
                    case "fianzas-bnc":
                        var data_bnc = JsonConvert.DeserializeObject<List<UspBonds>>(TempData["fianzas-bnc"].ToString());
                        fileName = "FianzaBanco.xlsx";
                        ExcelByBank(ws, data_bnc);
                        break;
                    case "fianzas-todas":
                        var data_all = JsonConvert.DeserializeObject<List<UspBondsAll>>(TempData["fianzas-todas"].ToString());
                        fileName = "FianzaHistorico.xlsx";
                        ExcelHistoric(ws, data_all);
                        break;
                }

                ws.Column(9).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                ws.Column(10).Style.NumberFormat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \" - \"??_);_(@_)";
                ws.Columns().AdjustToContents();                
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        private void ExcelHistoric(IXLWorksheet ws, List<UspBondsAll> data_pry)
        {
            var projects = data_pry.Select(x => new KeyValuePair<Guid, string>(x.ProjectId, x.ProjectAbbreviation)).Distinct();

            var rowCount = 1;
            foreach (var project in projects)
            {
                //Cabecera
                ws.Cell($"A{rowCount}").Value = project.Value;
                SetProjectHeaderStyle(ws, rowCount, "M");
                rowCount++;
                ws.Cell($"A{rowCount}").Value = "Nro. Fianza";
                ws.Cell($"B{rowCount}").Value = "Garante";
                ws.Cell($"C{rowCount}").Value = "Título Presupuesto";
                ws.Cell($"D{rowCount}").Value = "Tipo Fianza";
                ws.Cell($"E{rowCount}").Value = "Banco";
                ws.Cell($"F{rowCount}").Value = "Días Para Vencimiento";
                ws.Cell($"G{rowCount}").Value = "Inicio de Vigencia";
                ws.Cell($"H{rowCount}").Value = "Fin de Vigencia";
                ws.Cell($"I{rowCount}").Value = "Monto";
                ws.Cell($"J{rowCount}").Value = "Costo de Emisión";
                ws.Cell($"K{rowCount}").Value = "Moneda";
                ws.Cell($"L{rowCount}").Value = "Contra Garantía";
                ws.Cell($"M{rowCount}").Value = "Última Fianza";
                SetColumnsHeaderStyle(ws, rowCount, "M");
                SetRowBorderStyle(ws, rowCount, "M");
                rowCount++;

                //Detalle
                var bonds = data_pry.Where(x => x.ProjectId == project.Key).ToList();
                foreach (var bond in bonds)
                {
                    ws.Cell($"A{rowCount}").Value = bond.BondName;
                    ws.Cell($"B{rowCount}").Value = bond.BondGuarantor;
                    ws.Cell($"C{rowCount}").Value = bond.BudgetTitle;
                    ws.Cell($"D{rowCount}").Value = bond.BondType;
                    ws.Cell($"E{rowCount}").Value = bond.Bank;
                    if (bond.Validity > 0 || bond.IsTheLast)
                        ws.Cell($"F{rowCount}").Value = bond.IsTheLast ? 0 : bond.Validity;
                    ws.Cell($"G{rowCount}").Value = bond.CreateDate;
                    ws.Cell($"H{rowCount}").Value = bond.EndDate;
                    ws.Cell($"I{rowCount}").Value = bond.PenAmmount;
                    ws.Cell($"J{rowCount}").Value = bond.IssueCost;
                    ws.Cell($"K{rowCount}").Value = bond.currencyType;
                    ws.Cell($"L{rowCount}").Value = bond.guaranteeDesc;
                    ws.Cell($"M{rowCount}").Value = bond.IsTheLast ? "Sí" : "No";
                    SetRowBorderStyle(ws, rowCount, "M");
                    rowCount++;
                }
                rowCount += 2;
            }
        }

        private void ExcelByBank(IXLWorksheet ws, List<UspBonds> data_bnc)
        {
            var banks = data_bnc.Select(x => new KeyValuePair<Guid, string>(x.BankId, x.Bank)).Distinct();

            var rowCount = 1;
            foreach (var bank in banks)
            {
                //Cabecera
                ws.Cell($"A{rowCount}").Value = bank.Value;
                SetProjectHeaderStyle(ws, rowCount, "L");
                rowCount++;
                ws.Cell($"A{rowCount}").Value = "Nro. Fianza";
                ws.Cell($"B{rowCount}").Value = "Garante";
                ws.Cell($"C{rowCount}").Value = "Título Presupuesto";
                ws.Cell($"D{rowCount}").Value = "Tipo Fianza";
                ws.Cell($"E{rowCount}").Value = "Proyecto";
                ws.Cell($"F{rowCount}").Value = "Días Para Vencimiento";
                ws.Cell($"G{rowCount}").Value = "Inicio de Vigencia";
                ws.Cell($"H{rowCount}").Value = "Fin de Vigencia";
                ws.Cell($"I{rowCount}").Value = "Monto";
                ws.Cell($"J{rowCount}").Value = "Costo de Emisión";
                ws.Cell($"K{rowCount}").Value = "Moneda";
                ws.Cell($"L{rowCount}").Value = "Contra Garantía";
                SetColumnsHeaderStyle(ws, rowCount, "L");
                SetRowBorderStyle(ws, rowCount, "L");
                rowCount++;

                //Detalle
                var bonds = data_bnc.Where(x => x.BankId == bank.Key).ToList();
                var totalAmmount = 0.00;
                var issueTotalAmmount = 0.00;
                foreach (var bond in bonds)
                {
                    if (bond.IsTheLast == false)
                    {
                        ws.Cell($"A{rowCount}").Value = bond.BondName;
                        ws.Cell($"B{rowCount}").Value = bond.BondGuarantor;
                        ws.Cell($"C{rowCount}").Value = bond.BudgetTitle;
                        ws.Cell($"D{rowCount}").Value = bond.BondType;
                        ws.Cell($"E{rowCount}").Value = bond.ProjectAbbreviation;
                        ws.Cell($"F{rowCount}").Value = bond.Validity;
                        ws.Cell($"G{rowCount}").Value = bond.CreateDate;
                        ws.Cell($"H{rowCount}").Value = bond.EndDate;
                        ws.Cell($"I{rowCount}").Value = bond.PenAmmount;
                        ws.Cell($"J{rowCount}").Value = bond.IssueCostSum;
                        ws.Cell($"K{rowCount}").Value = bond.currencyType;
                        ws.Cell($"L{rowCount}").Value = bond.guaranteeDesc;
                        SetRowBorderStyle(ws, rowCount, "L");
                        totalAmmount += bond.PenAmmount;
                        issueTotalAmmount += bond.IssueCostSum;
                        rowCount++;
                    }
                }
                ws.Cell($"I{rowCount}").Value = totalAmmount;
                ws.Cell($"J{rowCount}").Value = issueTotalAmmount;
                SetTotalCellStyle(ws, rowCount);
                SetTotalCellStyle2(ws, rowCount);
                rowCount += 2;
            }
        }

        private void ExcelByProject(IXLWorksheet ws, List<UspBonds> data_pry)
        {
            var projects = data_pry.Select(x => new KeyValuePair<Guid, string>(x.ProjectId, x.ProjectAbbreviation)).Distinct();

            var rowCount = 1;
            foreach (var project in projects)
            {                
                //Cabecera
                ws.Cell($"A{rowCount}").Value = project.Value;
                SetProjectHeaderStyle(ws, rowCount, "L");
                rowCount++;
                ws.Cell($"A{rowCount}").Value = "Nro. Fianza";
                ws.Cell($"B{rowCount}").Value = "Garante";
                ws.Cell($"C{rowCount}").Value = "Título Presupuesto";
                ws.Cell($"D{rowCount}").Value = "Tipo Fianza";
                ws.Cell($"E{rowCount}").Value = "Banco";
                ws.Cell($"F{rowCount}").Value = "Días Para Vencimiento";
                ws.Cell($"G{rowCount}").Value = "Inicio de Vigencia";
                ws.Cell($"H{rowCount}").Value = "Fin de Vigencia";
                ws.Cell($"I{rowCount}").Value = "Monto";
                ws.Cell($"J{rowCount}").Value = "Costo de Emisión";
                ws.Cell($"K{rowCount}").Value = "Moneda";
                ws.Cell($"L{rowCount}").Value = "Contra Garantía";
                SetColumnsHeaderStyle(ws, rowCount, "L");
                SetRowBorderStyle(ws, rowCount, "L");
                rowCount++;

                //Detalle
                var bonds = data_pry.Where(x => x.ProjectId == project.Key).ToList();
                var totalAmmount = 0.00;
                var issueTotalAmmount = 0.00;
                foreach (var bond in bonds)
                {
                    if (bond.IsTheLast == false)
                    {
                        ws.Cell($"A{rowCount}").Value = bond.BondName;
                        ws.Cell($"B{rowCount}").Value = bond.BondGuarantor;
                        ws.Cell($"C{rowCount}").Value = bond.BudgetTitle;
                        ws.Cell($"D{rowCount}").Value = bond.BondType;
                        ws.Cell($"E{rowCount}").Value = bond.Bank;
                        ws.Cell($"F{rowCount}").Value = bond.IsTheLast ? 0 : bond.Validity;
                        ws.Cell($"G{rowCount}").Value = bond.CreateDate;
                        ws.Cell($"H{rowCount}").Value = bond.EndDate;
                        ws.Cell($"I{rowCount}").Value = bond.PenAmmount;
                        ws.Cell($"J{rowCount}").Value = bond.IssueCostSum;
                        ws.Cell($"K{rowCount}").Value = bond.currencyType;
                        ws.Cell($"L{rowCount}").Value = bond.guaranteeDesc;
                        SetRowBorderStyle(ws, rowCount, "L");
                        totalAmmount += bond.PenAmmount;
                        issueTotalAmmount += bond.IssueCostSum;
                        rowCount++;
                    }
                }
                ws.Cell($"I{rowCount}").Value = totalAmmount;
                ws.Cell($"J{rowCount}").Value = issueTotalAmmount;
                SetTotalCellStyle(ws, rowCount);
                SetTotalCellStyle2(ws, rowCount);
                rowCount += 2;
            }
        }

        private void SetProjectHeaderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Cell($"A{rowCount}").Style.Font.Bold = true;
            ws.Cell($"A{rowCount}").Style.Font.FontSize = 18.0;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(216, 228, 188));
            ws.Range($"A{rowCount}:{v}{rowCount}").Merge();
        }

        private void SetColumnsHeaderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Font.Bold = true;
        }

        private void SetRowBorderStyle(IXLWorksheet ws, int rowCount, string v)
        {
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range($"A{rowCount}:{v}{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetTotalCellStyle(IXLWorksheet ws, int rowCount)
        {
            ws.Cell($"I{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Cell($"I{rowCount}").Style.Font.Bold = true;
            ws.Cell($"I{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell($"I{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void SetTotalCellStyle2(IXLWorksheet ws, int rowCount)
        {
            ws.Cell($"J{rowCount}").Style.Fill.SetBackgroundColor(XLColor.FromArgb(184, 204, 228));
            ws.Cell($"J{rowCount}").Style.Font.Bold = true;
            ws.Cell($"J{rowCount}").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Cell($"J{rowCount}").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }
    }
}

