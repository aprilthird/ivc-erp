using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QRCoder;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros")]
    public class WorkerController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public WorkerController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<WorkerController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, userManager, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(string status = null, int? category = null, int? origin = null, int? workgroup = null)
        {
            var projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var query = await _context.Set<UspWorker>().FromSqlRaw("execute HumanResources_uspWorkers @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (status != "todos")
                if (status == "activo")
                    query = query.Where(x => x.IsActive).ToList();
                else if (status == "cesado")
                    query = query.Where(x => !x.IsActive).ToList();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value).ToList();
            if (origin.HasValue)
                query = query.Where(x => x.Origin == origin.Value).ToList();
            if (workgroup.HasValue)
                query = query.Where(x => x.Workgroup == workgroup.Value).ToList();

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorker(Guid id)
        {
            //TODO: Actualizar el modal de Editar
            var model = await _context.Workers
                .Where(x => x.Id == id)
                .Select(x => new WorkerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    DocumentType = x.DocumentType,
                    Document = x.Document,
                    BirthDate = x.BirthDate.HasValue ? x.BirthDate.Value.ToDateString() : null,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    BankId = x.BankId,
                    BankAccount = x.BankAccount,
                    Gender = x.Gender
                }).FirstOrDefaultAsync();

            var period = await _context.WorkerWorkPeriods
                .OrderByDescending(x => x.EntryDate)
                .FirstOrDefaultAsync(x => x.WorkerId == id);

            model.WorkPeriodId = period.Id;
            model.WorkPeriod = new WorkerWorkPeriodViewModel
            {
                Id = period.Id,
                WorkerId = period.WorkerId.Value,
                EntryDate = period.EntryDate.Value.ToDateString(),
                ProjectId = period.ProjectId,
                PensionFundAdministratorId = period.PensionFundAdministratorId.Value,
                PensionFundUniqueIdentificationCode = period.PensionFundUniqueIdentificationCode,
                Category = period.Category,
                Origin = period.Origin,
                Workgroup = period.Workgroup,
                WorkerPositionId = period.WorkerPositionId,
                NumberOfChildren = period.NumberOfChildren,
                HasUnionFee = period.HasUnionFee,
                HasSctr = period.HasSctr,
                SctrHealthType = period.SctrHealthType,
                SctrPensionType = period.SctrPensionType,
                JudicialRetentionFixedAmmount = period.JudicialRetentionFixedAmmount,
                JudicialRetentionPercentRate = period.JudicialRetentionPercentRate,
                HasWeeklySettlement = period.HasWeeklySettlement,
                LaborRegimen = period.LaborRegimen,
                HasEPS = period.HasEPS,
                HasEsSaludPlusVida = period.HasEsSaludPlusVida,
                CeaseDate = period.CeaseDate.HasValue ? period.CeaseDate.Value.ToDateString() : string.Empty,
                IsActive = period.IsActive
            };

            return Ok(model);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WorkerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var activeWorker = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Include(x => x.Project)
                .OrderByDescending(x => x.EntryDate.Value)
                .FirstOrDefaultAsync(x => x.Worker.Document == model.Document);
            if (activeWorker != null)
                if (activeWorker.IsActive)
                    return Ok($"El obrero ya existe en la base de datos y se encuentra activo en {activeWorker.Project.Abbreviation}.");
                else
                    return Ok($"El obrero ya existe en la base de datos y se encuentra cesado en {activeWorker.Project.Abbreviation}.");

            var worker = new Worker
            {
                Name = model.Name,
                MiddleName = model.MiddleName,
                PaternalSurname = model.PaternalSurname,
                MaternalSurname = model.MaternalSurname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DocumentType = model.DocumentType,
                Document = model.Document,
                BirthDate = model.BirthDate.ToDateTime(),
                BankId = model.BankId,
                BankAccount = model.BankAccount,
                Gender = model.Gender
            };

            var period = new WorkerWorkPeriod
            {
                Worker = worker,
                EntryDate = model.WorkPeriod.EntryDate.ToDateTime(),
                //ProjectId = model.WorkPeriod.ProjectId,
                ProjectId = GetProjectId(),
                Category = model.WorkPeriod.Category,
                Origin = model.WorkPeriod.Origin,
                Workgroup = model.WorkPeriod.Workgroup,
                WorkerPositionId = model.WorkPeriod.WorkerPositionId,
                PensionFundAdministratorId = model.WorkPeriod.PensionFundAdministratorId,
                PensionFundUniqueIdentificationCode = model.WorkPeriod.PensionFundUniqueIdentificationCode,
                NumberOfChildren = model.WorkPeriod.NumberOfChildren,
                HasUnionFee = true,
                HasSctr = true,
                SctrHealthType = 2,
                SctrPensionType = 2,
                JudicialRetentionFixedAmmount = model.WorkPeriod.JudicialRetentionFixedAmmount,
                JudicialRetentionPercentRate = model.WorkPeriod.JudicialRetentionPercentRate,
                HasWeeklySettlement = true,
                LaborRegimen = 27,
                HasEPS = false,
                HasEsSaludPlusVida = true,
                IsActive = true,
            };

            if (model.PhotoFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                worker.PhotoUrl = await storage.UploadFile(model.PhotoFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.WORKERS,
                    System.IO.Path.GetExtension(model.PhotoFile.FileName),
                    ConstantHelpers.Storage.Blobs.WORKERS_PHOTOS,
                    $"foto-{worker.Document}");
            }

            await _context.WorkerWorkPeriods.AddAsync(period);
            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WorkerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);

            var period = await _context.WorkerWorkPeriods.FirstOrDefaultAsync(x => x.Id == model.WorkPeriodId);

            if (period.EntryDate.Value.Date < model.WorkPeriod.EntryDate.ToDateTime().Date)
            {
                var hasTasks = await _context.WorkerDailyTasks
                    .Where(x => x.Date.Date >= period.EntryDate.Value.Date && x.Date.Date < model.WorkPeriod.EntryDate.ToDateTime().Date)
                    .AsNoTracking()
                    .CountAsync();

                if (hasTasks > 0)
                    return BadRequest("El obrero tiene tareos registrados entre la fecha de ingreso actual y la nueva fecha ingresada.");
            }

            worker.Name = model.Name;
            worker.PaternalSurname = model.PaternalSurname;
            worker.MaternalSurname = model.MaternalSurname;
            worker.DocumentType = model.DocumentType;
            worker.Document = model.Document;
            worker.PhoneNumber = model.PhoneNumber;
            worker.Email = model.Email;
            worker.BirthDate = model.BirthDate.ToDateTime();
            worker.BankId = model.BankId;
            worker.BankAccount = model.BankAccount;
            worker.Gender = model.Gender;


            period.EntryDate = model.WorkPeriod.EntryDate.ToDateTime();
            period.PensionFundAdministratorId = model.WorkPeriod.PensionFundAdministratorId;
            period.PensionFundUniqueIdentificationCode = model.WorkPeriod.PensionFundUniqueIdentificationCode;
            period.Category = model.WorkPeriod.Category;
            period.Origin = model.WorkPeriod.Origin;
            period.Workgroup = model.WorkPeriod.Workgroup;
            period.WorkerPositionId = model.WorkPeriod.WorkerPositionId;
            period.NumberOfChildren = model.WorkPeriod.NumberOfChildren;
            period.JudicialRetentionFixedAmmount = model.WorkPeriod.JudicialRetentionFixedAmmount;
            period.JudicialRetentionPercentRate = model.WorkPeriod.JudicialRetentionPercentRate;


            if (model.PhotoFile != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (worker.PhotoUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.WORKERS_PHOTOS}/{worker.PhotoUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.WORKERS);
                worker.PhotoUrl = await storage.UploadFile(model.PhotoFile.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.WORKERS,
                    System.IO.Path.GetExtension(model.PhotoFile.FileName),
                    ConstantHelpers.Storage.Blobs.WORKERS_PHOTOS,
                    $"foto-{worker.Document}");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == id);
            if (worker == null)
                return BadRequest($"Obrero con Id '{id}' no encontrado.");

            if (worker.PhotoUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.WORKERS_PHOTOS}/{worker.PhotoUrl.AbsolutePath.Split('/').Last()}",
                            ConstantHelpers.Storage.Containers.WORKERS);
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("importar/excel_nuevos")]
        public async Task<IActionResult> ImportNewWorkersSample()
        {
            string fileName = "ObrerosNuevos.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Nro. Documento";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "T/Documento";
                workSheet.Cell($"B2").Value = "(1:DNI, 2-CEXT, 4-PASAPORTE)";
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C1").Value = "Apellido Paterno";
                workSheet.Range("C1:C2").Merge();
                workSheet.Range("C1:C2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D1").Value = "Apellido Materno";
                workSheet.Range("D1:D2").Merge();
                workSheet.Range("D1:D2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E1").Value = "Nombres";
                workSheet.Range("E1:E2").Merge();
                workSheet.Range("E1:E2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"F1").Value = "Fecha Nacimiento";
                workSheet.Range("F1:F2").Merge();
                workSheet.Range("F1:F2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"G1").Value = "Celular";
                workSheet.Range("G1:G2").Merge();
                workSheet.Range("G1:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H1").Value = "Email";
                workSheet.Range("H1:H2").Merge();
                workSheet.Range("H1:H2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"I1").Value = "Fecha Ingreso";
                workSheet.Range("I1:I2").Merge();
                workSheet.Range("I1:I2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"J1").Value = "Fondo de Pensión";
                workSheet.Cell($"J2").Value = "(1-ONP, 2-PRO, 3-PRO MIXTA, 4-INT, 5-INT MIXTA, 6-PRI, 7-PRI MIXTA, 8-HAB MIXTA, 9-SIN REG PENSIONARIO)";
                workSheet.Range("J1:J2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"K1").Value = "CUSSP";
                workSheet.Range("K1:K2").Merge();
                workSheet.Range("K1:K2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"L1").Value = "Categoría";
                workSheet.Cell($"L2").Value = "(1-PE, 2-OP, 3-OF)";
                workSheet.Range("L1:L2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"M1").Value = "Procedencia";
                workSheet.Cell($"M2").Value = "(1-POB, 2-SIN, 3-IVC, 4-COLAB)";
                workSheet.Range("M1:M2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"N1").Value = "Destino";
                workSheet.Cell($"N2").Value = "(1-CASA, 2-COLAB)";
                workSheet.Range("N1:N2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"O1").Value = "Cargo (Id)";
                workSheet.Range("O1:O2").Merge();
                workSheet.Range("O1:O2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"P1").Value = "Nro. Hijos";
                workSheet.Range("P1:P2").Merge();
                workSheet.Range("P1:P2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"Q1").Value = "Sexo (0:M/1:F)";
                workSheet.Range("Q1:Q2").Merge();
                workSheet.Range("Q1:Q2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"R1").Value = "Banco (Id)";
                workSheet.Range("R1:R2").Merge();
                workSheet.Range("R1:R2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"S1").Value = "Cta. Bancaria";
                workSheet.Range("S1:S2").Merge();
                workSheet.Range("S1:S2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                workSheet.Range("A1:S10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("A1:S10").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var positions = await _context.WorkPositions.Where(x => x.Type == ConstantHelpers.WorkPositions.Type.WORKER).ToListAsync();
                workSheet = wb.Worksheets.Add("Cargos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Cargo";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                for (int i = 0; i < positions.Count; i++)
                {
                    workSheet.Cell($"A${3 + i}").Value = positions[i].Id;
                    workSheet.Cell($"B${3 + i}").Value = positions[i].Name;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                var bancos = await _context.Banks.ToListAsync();
                workSheet = wb.Worksheets.Add("Bancos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Banco";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                for (int i = 0; i < bancos.Count; i++)
                {
                    workSheet.Cell($"A${3 + i}").Value = bancos[i].Id;
                    workSheet.Cell($"B${3 + i}").Value = bancos[i].Name;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("importar/excel_reingresos")]
        public async Task<IActionResult> ImportReEntrySample()
        {
            string fileName = "ObrerosReingresos.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Nro. Documento";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Fecha Ingreso";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C1").Value = "Fondo de Pensión";
                workSheet.Cell($"C2").Value = "(1-ONP, 2-PRO, 3-PRO MIXTA, 4-INT, 5-INT MIXTA, 6-PRI, 7-PRI MIXTA, 8-HAB MIXTA, 9-SIN REG PENSIONARIO)";
                workSheet.Range("C1:C2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D1").Value = "CUSSP";
                workSheet.Range("D1:D2").Merge();
                workSheet.Range("D1:D2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E1").Value = "Categoría";
                workSheet.Cell($"E2").Value = "(1-PE, 2-OP, 3-OF)";
                workSheet.Range("E1:E2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"F1").Value = "Procedencia";
                workSheet.Cell($"F2").Value = "(1-POB, 2-SIN, 3-IVC, 4-COLAB)";
                workSheet.Range("F1:F2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"G1").Value = "Destino";
                workSheet.Cell($"G2").Value = "(1-CASA, 2-COLAB)";
                workSheet.Range("G1:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H1").Value = "Cargo (Id)";
                workSheet.Range("H1:H2").Merge();
                workSheet.Range("H1:H2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"I1").Value = "Nro. Hijos";
                workSheet.Range("I1:I2").Merge();
                workSheet.Range("I1:I2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                workSheet.Range("A1:I10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("A1:I10").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var positions = await _context.WorkPositions.Where(x => x.Type == ConstantHelpers.WorkPositions.Type.WORKER).ToListAsync();
                workSheet = wb.Worksheets.Add("Cargos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Cargo";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                for (int i = 0; i < positions.Count; i++)
                {
                    workSheet.Cell($"A${2 + i}").Value = positions[i].Id;
                    workSheet.Cell($"B${2 + i}").Value = positions[i].Name;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("importar/excel_ceses")]
        public async Task<IActionResult> ImportCeaseSample()
        {
            string fileName = "ObrerosCeses.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Nro. Documento";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Fecha Cese";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                //workSheet.Range("A1:B10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                //workSheet.Range("A1:B10").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("importar/actualizaciones")]
        public async Task<IActionResult> ImportUpdatesSample()
        {
            string fileName = "ObrerosActualizar.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("CargaMasiva");

                workSheet.Cell($"A1").Value = "Nro. Documento";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Fecha Nacimiento";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"C1").Value = "Celular";
                workSheet.Range("C1:C2").Merge();
                workSheet.Range("C1:C2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"D1").Value = "Email";
                workSheet.Range("D1:D2").Merge();
                workSheet.Range("D1:D2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"E1").Value = "Fondo de Pensión";
                workSheet.Cell($"E2").Value = "(1-ONP, 2-PRO, 3-PRO MIXTA, 4-INT, 5-INT MIXTA, 6-PRI, 7-PRI MIXTA, 8-HAB MIXTA, 9-SIN REG PENSIONARIO)";
                workSheet.Range("E1:E2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"F1").Value = "CUSSP";
                workSheet.Range("F1:F2").Merge();
                workSheet.Range("F1:F2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"G1").Value = "Categoría";
                workSheet.Cell($"G2").Value = "(1-PE, 2-OP, 3-OF)";
                workSheet.Range("G1:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"H1").Value = "Procedencia";
                workSheet.Cell($"H2").Value = "(1-POB, 2-SIN, 3-IVC, 4-COLAB)";
                workSheet.Range("H1:H2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"I1").Value = "Destino";
                workSheet.Cell($"I2").Value = "(1-CASA, 2-COLAB)";
                workSheet.Range("I1:I2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"J1").Value = "Cargo (Id)";
                workSheet.Range("J1:J2").Merge();
                workSheet.Range("J1:J2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"K1").Value = "Nro. Hijos";
                workSheet.Range("K1:K2").Merge();
                workSheet.Range("K1:K2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"L1").Value = "Sexo (0:M/1:F)";
                workSheet.Range("L1:L2").Merge();
                workSheet.Range("L1:L2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"M1").Value = "Banco (Id)";
                workSheet.Range("M1:M2").Merge();
                workSheet.Range("M1:M2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"N1").Value = "Cta. Bancaria";
                workSheet.Range("N1:N2").Merge();
                workSheet.Range("N1:N2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                workSheet.Range("A1:N10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("A1:N10").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                var positions = await _context.WorkPositions.Where(x => x.Type == ConstantHelpers.WorkPositions.Type.WORKER).ToListAsync();
                workSheet = wb.Worksheets.Add("Cargos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Cargo";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                for (int i = 0; i < positions.Count; i++)
                {
                    workSheet.Cell($"A${3 + i}").Value = positions[i].Id;
                    workSheet.Cell($"B${3 + i}").Value = positions[i].Name;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();

                var bancos = await _context.Banks.ToListAsync();
                workSheet = wb.Worksheets.Add("Bancos");
                workSheet.Cell($"A1").Value = "Id";
                workSheet.Range("A1:A2").Merge();
                workSheet.Range("A1:A2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                workSheet.Cell($"B1").Value = "Banco";
                workSheet.Range("B1:B2").Merge();
                workSheet.Range("B1:B2").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                for (int i = 0; i < bancos.Count; i++)
                {
                    workSheet.Cell($"A${3 + i}").Value = bancos[i].Id;
                    workSheet.Cell($"B${3 + i}").Value = bancos[i].Name;
                }

                workSheet.Columns().AdjustToContents();
                workSheet.Rows().AdjustToContents();
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                //workSheet.Range($"A1:B{positions.Count + 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Export(string status = null, int? category = null, int? origin = null, int? workgroup = null)
        {
            var projectId = GetProjectId();
            var projectParam = new SqlParameter("@ProjectId", projectId);

            var query = await _context.Set<UspWorkerExport>().FromSqlRaw("execute HumanResources_uspWorkersExport @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            if (status != "todos")
                if (status == "activo")
                    query = query.Where(x => x.IsActive).ToList();
                else if (status == "cesado")
                    query = query.Where(x => !x.IsActive).ToList();

            if (category.HasValue)
                query = query.Where(x => x.Category == category.Value).ToList();
            if (origin.HasValue)
                query = query.Where(x => x.Origin == origin.Value).ToList();
            if (workgroup.HasValue)
                query = query.Where(x => x.Workgroup == workgroup.Value).ToList();

            var dtWorkers = new DataTable();
            dtWorkers.TableName = "Obreros";
            dtWorkers.Columns.Add("Doc.", typeof(string));
            dtWorkers.Columns.Add("Nro. Doc", typeof(string));
            dtWorkers.Columns.Add("Nombres", typeof(string));
            dtWorkers.Columns.Add("Sexo", typeof(string));
            dtWorkers.Columns.Add("Nro. Hijos", typeof(string));
            dtWorkers.Columns.Add("Fecha Nac.", typeof(string));
            dtWorkers.Columns.Add("Teléfono", typeof(string));
            dtWorkers.Columns.Add("Email", typeof(string));
            dtWorkers.Columns.Add("Fecha Ingreso.", typeof(string));
            dtWorkers.Columns.Add("Fondo de Pensión", typeof(string));
            dtWorkers.Columns.Add("CUSSP", typeof(string));
            dtWorkers.Columns.Add("Categoría", typeof(string));
            dtWorkers.Columns.Add("Procedencia", typeof(string));
            dtWorkers.Columns.Add("Grupo de Trabajo", typeof(string));
            dtWorkers.Columns.Add("Puesto de trabajo", typeof(string));
            dtWorkers.Columns.Add("Banco", typeof(string));
            dtWorkers.Columns.Add("Cuenta Bancaria", typeof(string));
            dtWorkers.Columns.Add("Fecha Cese", typeof(string));
            dtWorkers.Columns.Add("Estado", typeof(string));
            //13

            foreach (var item in query)
                dtWorkers.Rows.Add(
                    item.DocumentTypeDesc,
                    item.Document, 
                    item.FullName,
                    item.GenderDesc, 
                    item.NumberOfChildren, 
                    item.BirthDateStr, 
                    item.PhoneNumber,
                    item.Email,
                    item.EntryDateStr,
                    item.PensionFundCode, 
                    item.PensionFundUniqueIdentificationCode, 
                    item.CategoryDesc, 
                    item.OriginDesc, 
                    item.WorkgroupDesc, 
                    item.WorkPositionName, 
                    item.BankName, 
                    item.BankAccount, 
                    item.CeaseDateStr, 
                    item.IsActiveStr);
            dtWorkers.AcceptChanges();

            //20

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtWorkers).Columns(1, 20).AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);

                    var project = await _context.Projects.FindAsync(projectId);
                    var fileName = $"Obreros - {project.Abbreviation}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        

        [HttpPost("importar/nuevos")]
        public async Task<IActionResult> ImportNewWorkers(IFormFile file)
        {
            var errors = new List<KeyValuePair<string, string>>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var workersDb = await _context.Workers
                        .AsNoTracking()
                        .ToListAsync();

                    var newWorkers = new List<Worker>();
                    var newWorkerPeriods = new List<WorkerWorkPeriod>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var document = workSheet.Cell($"A{counter}").GetString();
                        var workerExist = workersDb.FirstOrDefault(x => x.Document == document);
                        if (workerExist == null)
                        {
                            try
                            {
                                var worker = new Worker
                                {
                                    DocumentType = GetDocumentType(workSheet.Cell($"B{counter}").GetString()),
                                    Document = document,
                                    PaternalSurname = workSheet.Cell($"C{counter}").GetString(),
                                    MaternalSurname = workSheet.Cell($"D{counter}").GetString(),
                                    Name = workSheet.Cell($"E{counter}").GetString(),
                                    BirthDate = GetDate(workSheet, counter, "F"),
                                    PhoneNumber = workSheet.Cell($"G{counter}").GetString(),
                                    Email = workSheet.Cell($"H{counter}").GetString(),
                                    Gender = GetGender(workSheet.Cell($"Q{counter}").GetString()),
                                    BankId = GetBank(workSheet.Cell($"R{counter}").GetString()),
                                    BankAccount = workSheet.Cell($"S{counter}").GetString()
                                };

                                var workerWorkPeriod = new WorkerWorkPeriod
                                {
                                    Worker = worker,
                                    EntryDate = GetDate(workSheet, counter, "I"),
                                    ProjectId = GetProjectId(),
                                    PensionFundAdministratorId = GetAFPGuid(workSheet.Cell($"J{counter}").GetString()),
                                    PensionFundUniqueIdentificationCode = workSheet.Cell($"K{counter}").GetString(),
                                    Category = GetCategoryId(workSheet.Cell($"L{counter}").GetString()),
                                    Origin = GetOriginId(workSheet.Cell($"M{counter}").GetString()),
                                    Workgroup = GetWorkgroupId(workSheet.Cell($"N{counter}").GetString()),
                                    WorkerPositionId = GetWorkerPositionId(workSheet.Cell($"O{counter}").GetString()),
                                    NumberOfChildren = GetNumOfChildren(workSheet.Cell($"P{counter}").GetString()),
                                    HasUnionFee = true,
                                    HasSctr = true,
                                    SctrHealthType = 2,
                                    SctrPensionType = 2,
                                    JudicialRetentionFixedAmmount = 0,
                                    JudicialRetentionPercentRate = 0,
                                    HasWeeklySettlement = true,
                                    LaborRegimen = 27,
                                    HasEPS = false,
                                    HasEsSaludPlusVida = true,
                                    IsActive = true,
                                };

                                //Chequear errores
                                var haveErrors = false;
                                if (worker.DocumentType == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Tipo de Documento inválido"));
                                    haveErrors = true;
                                }
                                if (!workerWorkPeriod.EntryDate.HasValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Fecha de Ingreso inválida"));
                                    haveErrors = true;
                                }
                                if (!workerWorkPeriod.WorkerPositionId.HasValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Id de Cargo inválido"));
                                }
                                if (workerWorkPeriod.Category == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Categoría inválida"));
                                    haveErrors = true;
                                }
                                if (workerWorkPeriod.Workgroup == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Procedencia inválida"));
                                    haveErrors = true;
                                }
                                if (workerWorkPeriod.Origin == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Destino inválido"));
                                    haveErrors = true;
                                }

                                if (haveErrors)
                                {
                                    counter++;
                                    continue;
                                }

                                newWorkers.Add(worker);
                                newWorkerPeriods.Add(workerWorkPeriod);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.StackTrace);
                            }
                        };
                        counter++;
                    }

                    await _context.Workers.AddRangeAsync(newWorkers);
                    await _context.WorkerWorkPeriods.AddRangeAsync(newWorkerPeriods);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            if (errors.Count != 0)
                TempData["Errores"] = JsonConvert.SerializeObject(errors);

            return Ok(errors.Count());
        }

        [HttpPost("importar/reingresos")]
        public async Task<IActionResult> ImportReEntrys(IFormFile file)
        {
            var errors = new List<KeyValuePair<string, string>>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;

                    var workersDb = await _context.Set<UspWorkerImport>().FromSqlRaw(@"select distinct w.WorkerId,
	                            p.*,
	                            k.*
                            from WorkerWorkPeriods w cross apply (
	                            select top 1 ps.Id as PeriodId,
                                    ps.EntryDate,
		                            ps.CeaseDate,
		                            ps.IsActive
	                            from WorkerWorkPeriods ps
	                            where ps.WorkerId = w.WorkerId
	                            order by ps.EntryDate desc
                            ) p cross apply (
	                            select wks.Document
	                            from Workers wks
	                            where wks.Id = w.WorkerId
                            ) k")
                        .AsNoTracking()
                        .ToListAsync();

                    //var workersDb = await _context.WorkerWorkPeriods
                    //    .AsNoTracking()
                    //    .Include(x => x.Worker)
                    //    .OrderByDescending(x => x.EntryDate)
                    //    .GroupBy(x => x.WorkerId)
                    //    .Select(x => x.First())
                    //    .ToListAsync();

                    var newWorkerPeriods = new List<WorkerWorkPeriod>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var document = workSheet.Cell($"A{counter}").GetString();
                        var workerExist = workersDb.FirstOrDefault(x => x.Document == document);
                        if (workerExist != null && !workerExist.IsActive)
                        {
                            try
                            {
                                var workerWorkPeriod = new WorkerWorkPeriod
                                {
                                    //Worker = workerExist.Worker,
                                    WorkerId = workerExist.WorkerId,
                                    EntryDate = GetDate(workSheet, counter, "B"),
                                    ProjectId = GetProjectId(),
                                    PensionFundAdministratorId = GetAFPGuid(workSheet.Cell($"C{counter}").GetString()),
                                    PensionFundUniqueIdentificationCode = workSheet.Cell($"D{counter}").GetString(),
                                    Category = GetCategoryId(workSheet.Cell($"E{counter}").GetString()),
                                    Origin = GetOriginId(workSheet.Cell($"F{counter}").GetString()),
                                    Workgroup = GetWorkgroupId(workSheet.Cell($"G{counter}").GetString()),
                                    WorkerPositionId = GetWorkerPositionId(workSheet.Cell($"H{counter}").GetString()),
                                    NumberOfChildren = GetNumOfChildren(workSheet.Cell($"I{counter}").GetString()),
                                    HasUnionFee = true,
                                    HasSctr = true,
                                    SctrHealthType = 2,
                                    SctrPensionType = 2,
                                    JudicialRetentionFixedAmmount = 0,
                                    JudicialRetentionPercentRate = 0,
                                    HasWeeklySettlement = true,
                                    LaborRegimen = 27,
                                    HasEPS = false,
                                    HasEsSaludPlusVida = true,
                                    IsActive = true,
                                };

                                //Chequear errores
                                var haveErrors = false;
                                if (!workerWorkPeriod.WorkerPositionId.HasValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Id de Cargo inválido"));
                                }
                                if (!workerWorkPeriod.EntryDate.HasValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Fecha de Ingreso inválida"));
                                    haveErrors = true;
                                }
                                if (workerWorkPeriod.Category == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Categoría inválida"));
                                    haveErrors = true;
                                }
                                if (workerWorkPeriod.Workgroup == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Procedencia inválida"));
                                    haveErrors = true;
                                }
                                if (workerWorkPeriod.Origin == 0)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Destino inválido"));
                                    haveErrors = true;
                                }
                                if (workerExist.CeaseDate.Value.Date >= workerWorkPeriod.EntryDate.Value.Date)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Fecha Ingreso es menor a la última fecha de cese reportada"));
                                    haveErrors = true;
                                }

                                if (haveErrors)
                                {
                                    counter++;
                                    continue;
                                }

                                newWorkerPeriods.Add(workerWorkPeriod);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.StackTrace);
                            }
                        };
                        counter++;
                    }

                    await _context.WorkerWorkPeriods.AddRangeAsync(newWorkerPeriods);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            if (errors.Count != 0)
                TempData["Errores"] = JsonConvert.SerializeObject(errors);

            return Ok();
        }

        [HttpPost("importar/ceses")]
        public async Task<IActionResult> ImportCeases(IFormFile file)
        {
            var errors = new List<KeyValuePair<string, string>>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault(x => x.Name.ToUpper() == "FRENTE");
                    var counter = 3;
                    var workersDb = await _context.Set<UspWorkerImport>().FromSqlRaw(@"select distinct w.WorkerId,
	                            p.*,
	                            k.*
                            from WorkerWorkPeriods w cross apply (
	                            select top 1 ps.EntryDate,
		                            ps.CeaseDate,
		                            ps.IsActive
	                            from WorkerWorkPeriods ps
	                            where ps.WorkerId = w.WorkerId
	                            order by ps.EntryDate desc
                            ) p cross apply (
	                            select wks.Document
	                            from Workers wks
	                            where wks.Id = w.WorkerId
                            ) k")
                        .AsNoTracking()
                        .ToListAsync();

                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var document = workSheet.Cell($"A{counter}").GetString();
                        var workerExist = workersDb.FirstOrDefault(x => x.Document == document);
                        if (workerExist != null && workerExist.IsActive)
                        {
                            try
                            {
                                var ceaseDate = GetDate(workSheet, counter, "B");

                                //Chequear errores
                                var haveErrors = false;
                                if (!ceaseDate.HasValue)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Fecha inválida"));
                                    haveErrors = true;
                                }
                                else if (workerExist.EntryDate >= ceaseDate.Value)
                                {
                                    errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Fecha de cese es menor a la fecha de ingreso registrada"));
                                    haveErrors = true;
                                }

                                if (haveErrors)
                                {
                                    counter++;
                                    continue;
                                }

                                workerExist.CeaseDate = ceaseDate.Value;
                                workerExist.IsActive = false;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.StackTrace);
                            }
                        };
                        ++counter;
                    }

                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            if (errors.Count != 0)
                TempData["Errores"] = JsonConvert.SerializeObject(errors);

            return Ok();
        }

        [HttpPost("importar/errores")]
        public async Task<IActionResult> ImportErrors()
        {
            if (TempData["Errores"] == null)
            {
                RedirectToAction("Empty");
            }

            var importErrors = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(TempData["Errores"].ToString());

            DataTable dt = GetData(importErrors);
            //Name of File
            string fileName = "ErroresCarga.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("importar/actualizaciones")]
        public async Task<IActionResult> ImportUpdates(IFormFile file)
        {
            var errors = new List<KeyValuePair<string, string>>();
            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;

                    var workersDb = await _context.Set<UspWorkerImport>().FromSqlRaw(@"select distinct w.WorkerId,
	                            p.*,
	                            k.*
                            from WorkerWorkPeriods w cross apply (
	                            select top 1 ps.Id as PeriodId,
                                    ps.EntryDate,
		                            ps.CeaseDate,
		                            ps.IsActive
	                            from WorkerWorkPeriods ps
	                            where ps.WorkerId = w.WorkerId
	                            order by ps.EntryDate desc
                            ) p cross apply (
	                            select wks.Document
	                            from Workers wks
	                            where wks.Id = w.WorkerId
                            ) k")
                        .AsNoTracking()
                        .ToListAsync();

                    //var workersDb = await _context.Workers
                    //    .AsNoTracking()
                    //    .ToListAsync();

                    //var newWorkers = new List<Worker>();
                    //var newWorkerPeriods = new List<WorkerWorkPeriod>();
                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var document = workSheet.Cell($"A{counter}").GetString();
                        var workerExist = workersDb.FirstOrDefault(x => x.Document == document);
                        if (workerExist != null)
                        {
                            try
                            {
                                var wDb = await _context.Workers.FirstAsync(x => x.Id == workerExist.WorkerId);
                                if (!workSheet.Cell($"B{counter}").IsEmpty()) wDb.BirthDate = GetDate(workSheet, counter, "B");
                                if (!workSheet.Cell($"C{counter}").IsEmpty()) wDb.PhoneNumber = workSheet.Cell($"C{counter}").GetString();
                                if (!workSheet.Cell($"D{counter}").IsEmpty()) wDb.Email = workSheet.Cell($"D{counter}").GetString();
                                if (!workSheet.Cell($"L{counter}").IsEmpty()) wDb.Gender = GetGender(workSheet.Cell($"L{counter}").GetString());
                                if (!workSheet.Cell($"M{counter}").IsEmpty()) wDb.BankId = GetBank(workSheet.Cell($"M{counter}").GetString());
                                if (!workSheet.Cell($"N{counter}").IsEmpty()) wDb.BankAccount = workSheet.Cell($"N{counter}").GetString();

                                var pDb = await _context.WorkerWorkPeriods.FirstAsync(x => x.Id == workerExist.PeriodId);
                                if (!workSheet.Cell($"E{counter}").IsEmpty()) pDb.PensionFundAdministratorId = GetAFPGuid(workSheet.Cell($"E{counter}").GetString());
                                if (!workSheet.Cell($"F{counter}").IsEmpty()) pDb.PensionFundUniqueIdentificationCode = workSheet.Cell($"F{counter}").GetString();
                                if (!workSheet.Cell($"G{counter}").IsEmpty()) pDb.Category = GetCategoryId(workSheet.Cell($"G{counter}").GetString());
                                if (!workSheet.Cell($"H{counter}").IsEmpty()) pDb.Origin = GetOriginId(workSheet.Cell($"H{counter}").GetString());
                                if (!workSheet.Cell($"I{counter}").IsEmpty()) pDb.Workgroup = GetWorkgroupId(workSheet.Cell($"I{counter}").GetString());
                                if (!workSheet.Cell($"J{counter}").IsEmpty()) pDb.WorkerPositionId = GetWorkerPositionId(workSheet.Cell($"J{counter}").GetString());
                                if (!workSheet.Cell($"K{counter}").IsEmpty()) pDb.NumberOfChildren = GetNumOfChildren(workSheet.Cell($"K{counter}").GetString());

                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.StackTrace);
                            }
                        }
                        else
                        {
                            errors.Add(new KeyValuePair<string, string>(counter.ToString(), "Nº Documento no exite"));
                        }
                        counter++;
                    }

                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }

            if (errors.Count != 0)
                TempData["Errores"] = JsonConvert.SerializeObject(errors);

            return Ok(errors.Count());
        }

        [HttpGet("fotocheck/{id}")]
        public async Task<IActionResult> GeneratePhotocheck(Guid id)
        {
            var periods = await _context.WorkerWorkPeriods
                .Include(x => x.Worker)
                .Include(x => x.Project)
                .Include(x => x.Project.Business)
                .Where(x => x.WorkerId.Value == id)
                .OrderByDescending(x => x.EntryDate.Value.Date)
                .AsNoTracking()
                .ToListAsync();

            var worker = periods.Select(x => new WorkerViewModel
            {
                Id = x.WorkerId,
                Name = x.Worker.Name,
                PaternalSurname = x.Worker.PaternalSurname,
                MaternalSurname = x.Worker.MaternalSurname,
                DocumentType = x.Worker.DocumentType,
                Document = x.Worker.Document,
                PhotoUrl = x.Worker.PhotoUrl,
                CategoryStr = ConstantHelpers.Worker.Category.VALUES[x.Category],
                LogoUrl = x.Project.LogoUrl,
                ProjectPhoneNumber = x.Project.Business.PhoneNumber,
                ProjectAddress = x.Project.Business.Address
            }).First();

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(worker.Document, QRCodeGenerator.ECCLevel.L);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            return View(worker);
        }

        private DataTable GetData(List<KeyValuePair<string, string>> importErrors)
        {
            //Creating DataTable
            DataTable dt = new DataTable();
            //Setiing Table Name
            dt.TableName = "ErrorsData";
            //Add Columns
            dt.Columns.Add("Línea", typeof(int));
            dt.Columns.Add("Error", typeof(string));
            //Add Rows in DataTable
            foreach (var item in importErrors)
            {
                dt.Rows.Add(item.Key, item.Value);
            }
            dt.AcceptChanges();
            return dt;
        }

        private int GetNumOfChildren(string numStr)
        {
            var success = Int32.TryParse(numStr, out int num);
            if (success)
                return num;
            return 0;
        }

        private int GetDocumentType(string documentTypeStr)
        {
            var success = Int32.TryParse(documentTypeStr, out int documentType);
            if (success && (documentType == 1 || documentType == 2 || documentType == 4))
                return documentType;
            return 0;
        }

        private Guid? GetWorkerPositionId(string positionstr)
        {
            var success = Guid.TryParse(positionstr, out Guid position);
            if (success)
                return position;
            return null;
        }

        private int GetWorkgroupId(string workgroupStr)
        {
            var success = Int32.TryParse(workgroupStr, out int workgroup);
            if (success && workgroup >= 1 && workgroup <= 2)
                return workgroup;
            return 0;
        }

        private int GetOriginId(string originStr)
        {
            var success = Int32.TryParse(originStr, out int origin);
            if (success)
                return origin;
            return 0;
        }

        private int GetCategoryId(string categoryStr)
        {
            var success = Int32.TryParse(categoryStr, out int category);
            if (success)
                return category;
            return 0;
        }

        private Guid GetAFPGuid(string afpStr)
        {
            var success = Int32.TryParse(afpStr, out int afp);
            if (success)
                switch (afp)
                {
                    case 1: return Guid.Parse("E804DF0C-D783-4534-2B3B-08D7AB570506");
                    case 2: return Guid.Parse("19DB4712-40E4-483D-2B3D-08D7AB570506");
                    case 3: return Guid.Parse("2078074F-D5E8-4D20-2B3A-08D7AB570506");
                    case 4: return Guid.Parse("0D166874-E319-4DF5-2B3C-08D7AB570506");
                    case 5: return Guid.Parse("F064BCC1-563B-48F4-2B3F-08D7AB570506");
                    case 6: return Guid.Parse("7CC8A397-80A5-4F4F-2B40-08D7AB570506");
                    case 7: return Guid.Parse("CC345D37-320C-44C5-2B3E-08D7AB570506");
                    case 8: return Guid.Parse("FE43D32E-19B9-4C62-2B41-08D7AB570506");
                    case 9: return Guid.Parse("8A48449A-5869-482C-2B42-08D7AB570506");
                    default: return Guid.Parse("8A48449A-5869-482C-2B42-08D7AB570506");
                }
            return Guid.Parse("8A48449A-5869-482C-2B42-08D7AB570506");
        }

        private DateTime? GetDate(IXLWorksheet workSheet, int counter, string column)
        {
            if (!workSheet.Cell($"{column}{counter}").IsEmpty())
            {
                if (workSheet.Cell($"{column}{counter}").DataType == XLDataType.DateTime)
                {
                    try
                    {
                        var datetime = workSheet.Cell($"{column}{counter}").GetDateTime();
                        return new DateTime(datetime.Year, datetime.Month, datetime.Day);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.StackTrace);
                    }
                }
            }
            return null;
        }

        private Guid? GetBank(string v)
        {
            var success = Guid.TryParse(v, out Guid position);
            if (success)
                return position;
            return null;
        }

        private int GetGender(string v)
        {
            var success = Int32.TryParse(v, out int category);
            if (success)
                switch (category)
                {
                    case 0:
                        return ConstantHelpers.Worker.Gender.MALE;
                    default:
                        return ConstantHelpers.Worker.Gender.FEMALE;
                }
            return ConstantHelpers.Worker.Gender.MALE;
        }

        [HttpGet("altas")]
        public async Task<IActionResult> GetTregUpFiles(string idate, string edate)
        {
            SqlParameter initDateParam = new SqlParameter("@InitDate", idate.ToDateTime());
            SqlParameter endDateParam = new SqlParameter("@EndDate", edate.ToDateTime());
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var query = await _context.Set<UspWorkersSunatTreg>().FromSqlRaw("execute HumanResources_uspWorkersSunatTreg @InitDate, @EndDate, @ProjectId"
                , initDateParam, endDateParam, projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var project = await _context.Projects.AsNoTracking().FirstAsync(x => x.Id == GetProjectId());

            //Creando archivo zip
            var zipFileMemoryStream = new MemoryStream();
            using (ZipArchive archive = new ZipArchive(zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true))
            {
                //Archivo .ide
                var ide = archive.CreateEntry($"RP_{project.RucCompany}.ide");
                using (var streamWriter = new StreamWriter(ide.Open()))
                {
                    foreach (var item in query)
                    {
                        streamWriter.WriteLine(
                            $"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.BirthDate.ToDateString()}|" +
                            $"{item.PaternalSurname}|" +
                            $"{item.MaternalSurname}|" +
                            $"{item.Name}|" +
                            $"{item.Sex}|" +
                            $"{item.Nationality}||" +
                            $"{item.PhoneNumber}|" +
                            $"{item.Email}|||||||||||||||||||||||||||||" +
                            $"{item.AddressIndicator}|");
                    }
                }

                //Archivo .tra
                var tra = archive.CreateEntry($"RP_{project.RucCompany}.tra");
                using (var streamWriter = new StreamWriter(tra.Open()))
                {
                    foreach (var item in query)
                    {
                        streamWriter.WriteLine(
                            $"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.LaborRegimen}|" +
                            $"{item.EducativeSituation}|" +
                            $"{item.SunatOcupation}|" +
                            $"{item.Disability}|" +
                            $"{item.PensionFundUniqueIdentificationCode}|" +
                            $"{item.SctrPensionType}|" +
                            $"{item.ContractType}|" +
                            $"{item.AlternativeRegimen}|" +
                            $"{item.MaxWorkTime}|" +
                            $"{item.NocturnalRegimen}|" +
                            $"{item.Sindicalize}|" +
                            $"{item.PaymentPeriodicity}||" +
                            $"{item.ActiveSituation}|" +
                            $"{item.ExemptIncome}|" +
                            $"{item.SpecialRegimen}|" +
                            $"{item.PaymentMethod}|" +
                            $"{item.OcupationalCategory}|" +
                            $"{item.DoubleTributation}||");
                    }
                }

                //Archivo .per
                var per = archive.CreateEntry($"RP_{project.RucCompany}.per");
                using (var streamWriter = new StreamWriter(per.Open()))
                {
                    foreach (var item in query)
                    {
                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.PeriodCategory}|1|" +
                            $"{item.EntryDate}||||");

                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.PeriodCategory}|2|" +
                            $"{item.EntryDate}||" +
                            $"{item.LaborRegimen}||");

                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.PeriodCategory}|3|" +
                            $"{item.EntryDate}||" +
                            $"{item.HealthRegimen}||");

                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.PeriodCategory}|4|" +
                            $"{item.EntryDate}||" +
                            $"{item.PensionFund}||");

                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.PeriodCategory}|5|" +
                            $"{item.EntryDate}||" +
                            $"{item.SctrHealthType}||");
                    }
                }

                //Archivo .est
                var est = archive.CreateEntry($"RP_{project.RucCompany}.est");
                using (var streamWriter = new StreamWriter(est.Open()))
                {
                    foreach (var item in query)
                    {
                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.RucCompany}|" +
                            $"{item.EstablishmentCode}|");
                    }
                }

                //Archivo .cta
                var cta = archive.CreateEntry($"RP_{project.RucCompany}.cta");
                using (var streamWriter = new StreamWriter(cta.Open()))
                {
                    foreach (var item in query)
                    {
                        streamWriter.WriteLine($"0{item.DocumentType}|" +
                            $"{item.Document}||" +
                            $"{item.BankCode}|" +
                            $"{item.BankAccount}|");
                    }
                }
            }

            zipFileMemoryStream.Seek(0, SeekOrigin.Begin);
            return File(zipFileMemoryStream.ToArray(), "application/zip", $"treg_{project.RucCompany}.zip");
        }
    }
}
