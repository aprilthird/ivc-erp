using ClosedXML.Excel;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros/boletas")]
    public class WorkerInvoiceController : BaseController
    {
        private readonly IEmailQueuedBackground _emailQueuedBackgroundService;
        private readonly IServiceScopeFactory _scopeFactory;

        public WorkerInvoiceController(IvcDbContext context,
            IEmailQueuedBackground emailQueuedBackgroundService,
            IServiceScopeFactory scopeFactory)
            : base(context)
        {
            _emailQueuedBackgroundService = emailQueuedBackgroundService;
            _scopeFactory = scopeFactory;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? weekId = null)
        {
            if (!weekId.HasValue)
                return Ok(new List<UspWorkerInvoiceSend>());

            //var header = await _context.PayrollMovementHeaders
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId);

            //if (header == null)
            //    return Ok(new List<UspWorkerInvoiceSend>());

            //SqlParameter headerParam = new SqlParameter("@HeaderId", header.Id);
            SqlParameter weekParam = new SqlParameter("@WeekId", weekId.Value);

            var workerInvoices = await _context.Set<UspWorkerInvoiceSend>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceSends @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            return Ok(workerInvoices);
        }

        [HttpGet("generar-boleta/{weekId}/{workerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateInvoice(Guid weekId, Guid workerId)
        {
            var invoice = new WorkerInvoiceViewModel();

            // SQL Parameters
            SqlParameter workerParam = new SqlParameter("@WorkerId", workerId);
            SqlParameter weekParam = new SqlParameter("@WeekId", weekId);

            // SQL Exec USP
            var workerInfo = _context.Set<UspWorkerInvoiceWokerInfo>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceWokerInfos @WorkerId, @WeekId"
                , workerParam, weekParam)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .ToList();

            var workerConcepts = _context.Set<UspWorkerInvoiceConcept>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceConcepts @WorkerId, @WeekId"
                , workerParam, weekParam)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .ToList();

            var week = await _context.ProjectCalendarWeeks
                .FirstOrDefaultAsync(x => x.Id == weekId);

            invoice.WorkerInfo = workerInfo.First();
            invoice.IncomeConcepts = workerConcepts
                .Where(x => x.CategoryId == ConstantHelpers.PayrollConcept.Category.INCOMES &&
                            x.Value > 0.0M)
                .ToList();
            invoice.DiscountConcepts = workerConcepts
                .Where(x => x.CategoryId == ConstantHelpers.PayrollConcept.Category.DISCOUNTS &&
                            x.Value > 0.0M)
                .ToList();
            invoice.ContributionConcepts = workerConcepts
                .Where(x => x.CategoryId == ConstantHelpers.PayrollConcept.Category.CONTRIBUTIONS &&
                            x.Value > 0.0M)
                .ToList();
            invoice.TotalConcepts = workerConcepts
                .Where(x => x.CategoryId == ConstantHelpers.PayrollConcept.Category.TOTALS)
                .ToList();
            invoice.DailyConcepts = workerConcepts
                .Where(x => x.CategoryId == ConstantHelpers.PayrollConcept.Category.DAILY)
                .ToList();

            invoice.MonthYear = GetMonthName(week.Month) + " - " + week.Year;
            invoice.WeekRange = "Semana " + week.WeekStart.ToDateString() + " al " + week.WeekEnd.ToDateString();

            return View("WorkerInvoice", invoice);
        }

        [HttpGet("decargar-boleta")]
        public FileResult DownloadInvoice(string url, string filename)
        {
            Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp");

            if (!String.IsNullOrEmpty(url))
            {
                serviceUrl = new Uri(serviceUrl + $"?url={url}&ls=1");
            }

            using (var wc = new WebClient())
            {
                byte[] pdfBytes = wc.DownloadData(serviceUrl);
                return File(pdfBytes, "application/pdf", $"{filename}.pdf");
            }
        }

        [HttpGet("enviar-boleta")]
        public async Task<IActionResult> SendMail(Guid weekId, Guid workerId)
        {
            var project = await _context.Projects.Where(x => x.Id == GetProjectId()).FirstOrDefaultAsync();


            var header = await _context.PayrollMovementHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId);

            var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id == weekId);
            var worker = await _context.Workers.FirstOrDefaultAsync(x => x.Id == workerId);
            var url = $"{ConstantHelpers.SystemUrl.Url}recursos-humanos/obreros/boletas/generar-boleta/{weekId}/{workerId}";
            var filename = week.YearWeekNumber + "-" + worker.Document;

            if (!string.IsNullOrEmpty(worker.Email))
            {
                //Cargar Archivo Adjunto
                var ms = new MemoryStream();
                var serviceUrl = new Uri($"{ConstantHelpers.SystemUrl.PdfGeneratorUrl}?url={url}");
                using (var wc = new WebClient())
                {
                    byte[] bondBytes = wc.DownloadData(serviceUrl);
                    ms = new MemoryStream(bondBytes);
                }
                var attachment = new Attachment(ms, $"{filename}.pdf");

                _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                {
                    To = new List<MailAddress> { new MailAddress(worker.Email, worker.FullName) },
                    Cc = new List<MailAddress>(),
                    Subject = $"IVC - Boleta - {week.YearWeekNumber}",
                    Message = $"Hola, { worker.FullName }, <br /><br /> " +
                        $"Por parte del centro de costo {project.Abbreviation} se le adjunta su boleta de la semana {week.WeekNumber} del año {week.Year}.<br />" +
                        $"<br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br /> " +
                        $"Control de RR.HH.",
                    Attachments = new List<Attachment> { attachment },
                    OnMailSucceeded = async () =>
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();
                            var isSended = await _context.WorkerInvoiceSends
                                .FirstOrDefaultAsync(x => x.WorkerId == workerId && x.PayrollMovementHeaderId == header.Id);
                            if (isSended != null)
                            {
                                isSended.DateSended = DateTime.UtcNow.ToDefaultTimeZone();
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                await _context.WorkerInvoiceSends.AddAsync(new WorkerInvoiceSend
                                {
                                    DateSended = DateTime.UtcNow.ToDefaultTimeZone(),
                                    Observation = string.Empty,
                                    PayrollMovementHeaderId = header.Id,
                                    WorkerId = workerId
                                });
                                await _context.SaveChangesAsync();
                            }
                        }
                    },
                    OnMailError = async () =>
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _context = scope.ServiceProvider.GetRequiredService<IvcDbContext>();
                            var isSended = await _context.WorkerInvoiceSends
                                .FirstOrDefaultAsync(x => x.WorkerId == workerId && x.PayrollMovementHeaderId == header.Id);
                            if (isSended != null)
                            {
                                isSended.Observation = "Envío Fallido";
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                await _context.WorkerInvoiceSends.AddAsync(new WorkerInvoiceSend
                                {
                                    Observation = "Envío Fallido",
                                    PayrollMovementHeaderId = header.Id,
                                    WorkerId = workerId
                                });
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                });
            }

            return Ok();
        }

        [HttpGet("informe-sunafil")]
        public FileResult DownloadInvoiceSendReport(Guid? weekId = null)
        {
            var header = _context.PayrollMovementHeaders
                .Include(x => x.ProjectCalendarWeek)
                .AsNoTracking()
                .FirstOrDefault(x => x.ProjectCalendarWeekId == weekId.Value);

            SqlParameter weekParam = new SqlParameter("@WeekId", weekId.Value);

            var workerInvoices = _context.Set<UspWorkerInvoiceSend>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceSends @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToList();

            using (XLWorkbook wb = new XLWorkbook())
            {
                string filename = "InformeSunafil-" + header.ProjectCalendarWeek.YearWeekNumber + ".xlsx";

                var ws = wb.Worksheets.Add("Detalle");

                ws.Cell($"B1").Value = "IVC CONTRATISTAS GENERALES SA";
                ws.Cell($"B2").Value = "RUC. 20100754755";
                ws.Cell($"B3").Value = "Año " + header.ProjectCalendarWeek.Year + " Semana " + header.ProjectCalendarWeek.WeekNumber + " Del " + header.ProjectCalendarWeek.WeekStart.ToDateString() + " al " + header.ProjectCalendarWeek.WeekEnd.ToDateString();

                ws.Cell($"B4").Value = "Nro.Doc.";
                ws.Column(2).Style.NumberFormat.Format = "@";
                ws.Cell($"C4").Value = "Trabajador";
                ws.Cell($"D4").Value = "Categoría";
                ws.Cell($"E4").Value = "Email";
                ws.Cell($"F4").Value = "Fecha de Envió";
                ws.Cell($"G4").Value = "Observación";
                ws.Range("B4:G4").Style.Font.Bold = true;

                var row = 5;
                foreach (var send in workerInvoices)
                {
                    ws.Cell($"B{row}").Value = send.Document;
                    ws.Cell($"C{row}").Value = send.FullName;
                    ws.Cell($"D{row}").Value = send.CategoryDesc;
                    ws.Cell($"E{row}").Value = send.Email;
                    ws.Cell($"F{row}").Value = send.DateSendedStr;
                    ws.Cell($"G{row}").Value = send.Observation;
                    row++;
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                }
            }
        }

        #region Helpers
        private string GetMonthName(int month)
        {
            return month switch
            {
                1 => "Enero",
                2 => "Febrero",
                3 => "Marzo",
                4 => "Abril",
                5 => "Mayo",
                6 => "Junio",
                7 => "Julio",
                8 => "Agosto",
                9 => "Setiembre",
                10 => "Octubre",
                11 => "Noviembre",
                12 => "Diciembre",
                _ => string.Empty,
            };
        }
        #endregion


        #region TestSend
        [HttpPost("envio-masivo-prueba/{id}")]
        public async Task<IActionResult> GenerateMasiveEmailSend(Guid id)
        {
            var header = await _context.PayrollMovementHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id);

            SqlParameter weekParam = new SqlParameter("@WeekId", id);

            var workerInvoices = await _context.Set<UspWorkerInvoiceSend>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceSends @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var sends = new List<WorkerInvoiceSend>();
            foreach (var invoice in workerInvoices)
            {
                if (!string.IsNullOrEmpty(invoice.Email))
                {
                    sends.Add(new WorkerInvoiceSend
                    {
                        DateSended = DateTime.UtcNow.ToDefaultTimeZone(),
                    Observation = string.Empty,
                        PayrollMovementHeaderId = header.Id,
                        WorkerId = invoice.WorkerId
                    });
                }
                else
                {
                    sends.Add(new WorkerInvoiceSend
                    {
                        DateSended = null,
                        Observation = "No tiene correo electrónico.",
                        PayrollMovementHeaderId = header.Id,
                        WorkerId = invoice.WorkerId
                    });
                }
            }

            if (sends.Count > 0)
            {
                await _context.WorkerInvoiceSends.AddRangeAsync(sends);
                await _context.SaveChangesAsync();
            }           

            return Ok();
        }
        #endregion

        [HttpPost("envio-masivo-prueba-t/{id}")]
        public async Task<IActionResult> GenerateMasiveEmailSendT(Guid id)
        {
            var project = await _context.Projects.Where(x => x.Id == GetProjectId()).FirstOrDefaultAsync();

            var header = await _context.PayrollMovementHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id);

            var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id == id);

            var weekParam = new SqlParameter("@WeekId", id);

            var workerInvoices = await _context.Set<UspWorkerInvoiceSend>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceSends @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            foreach (var a in workerInvoices)
            {
                if(!string.IsNullOrEmpty(a.Email))
                {
                    var url = $"https://erp-ivc.azurewebsites.net/recursos-humanos/obreros/boletas/generar-boleta/{id}/{a.WorkerId}";
                    var filename = week.YearWeekNumber + "-" + a.Document;

                    MemoryStream ms = new MemoryStream();
                    Attachment attachment = null;
                    Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp?url={url}");
                    using (var wc = new WebClient())
                    {
                        byte[] bondBytes = wc.DownloadData(serviceUrl);
                        ms = new MemoryStream(bondBytes);
                    }
                    attachment = new Attachment(ms, $"{filename}.pdf");

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC")
                    };

                    if (attachment != null)
                        mailMessage.Attachments.Add(attachment);

                    mailMessage.Subject = $"IVC - Boleta - {week.YearWeekNumber}";

                    mailMessage.To.Add(new MailAddress(a.Email, a.FullName));

                    mailMessage.Body =
                        $"Hola, { a.FullName }, <br /><br /> " +
                        $"Por parte del centro de costo {project.Abbreviation} se le adjunta su boleta de la semana {week.WeekNumber} del año {week.Year}.<br />" +
                        $"<br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br /> " +
                        $"Control de RR.HH.";
                    mailMessage.IsBodyHtml = true;

                    using (var client = new SmtpClient("smtp.office365.com", 587))
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                        client.EnableSsl = true;
                        client.DeliveryFormat = SmtpDeliveryFormat.International;
                        await client.SendMailAsync(mailMessage);
                        Thread.Sleep(15000);
                    }

                    var isSended = await _context.WorkerInvoiceSends
                    .FirstOrDefaultAsync(x => x.WorkerId == a.WorkerId && x.PayrollMovementHeaderId == header.Id);
                    if (isSended != null)
                    {
                        isSended.DateSended = DateTime.UtcNow.ToDefaultTimeZone();
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        await _context.WorkerInvoiceSends.AddAsync(new WorkerInvoiceSend
                        {
                            DateSended = DateTime.UtcNow.ToDefaultTimeZone(),
                        Observation = string.Empty,
                            PayrollMovementHeaderId = header.Id,
                            WorkerId = a.WorkerId

                        });
                        await _context.SaveChangesAsync();
                    }

                    //var isSended = await _context.WorkerInvoiceSends
                    //    .FirstOrDefaultAsync(x => x.WorkerId == workerId);
                    //if (isSended != null)
                    //{
                    //    isSended.DateSended = DateTime.Now;
                    //    await _context.SaveChangesAsync();
                    //}
                    //else
                    //{
                    //    await _context.WorkerInvoiceSends.AddAsync(new WorkerInvoiceSend
                    //    {
                    //        DateSended = DateTime.Now,
                    //        WorkerId = a.WorkerId
                    //    });
                    //    await _context.SaveChangesAsync();
                    //}

                }
            }
            


            return Ok();
        }

        [HttpPost("envio-masivo-prueba-restante/{id}")]
        public async Task<IActionResult> GenerateMasiveEmailSendRest(Guid id)
        {
            var project = await _context.Projects.Where(x => x.Id == GetProjectId()).FirstOrDefaultAsync();

            var header = await _context.PayrollMovementHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == id);

            var week = await _context.ProjectCalendarWeeks.FirstOrDefaultAsync(x => x.Id == id);

            var weekParam = new SqlParameter("@WeekId", id);

            var workerInvoices = await _context.Set<UspWorkerInvoiceSend>()
                .FromSqlRaw("execute HumanResources_uspWorkerInvoiceSends @WeekId"
                , weekParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            foreach (var a in workerInvoices.Where(x=>x.DateSended == null))
            {
                if (!string.IsNullOrEmpty(a.Email))
                {
                    var url = $"{ConstantHelpers.SystemUrl.Url}recursos-humanos/obreros/boletas/generar-boleta/{id}/{a.WorkerId}";
                    var filename = week.YearWeekNumber + "-" + a.Document;

                    var ms = new MemoryStream();
                    var serviceUrl = new Uri($"{ConstantHelpers.SystemUrl.PdfGeneratorUrl}?url={url}");
                    using (var wc = new WebClient())
                    {
                        byte[] bondBytes = wc.DownloadData(serviceUrl);
                        ms = new MemoryStream(bondBytes);
                    }
                    var attachment = new Attachment(ms, $"{filename}.pdf");

                    _emailQueuedBackgroundService.QueueBackgroundEmailItem(new EmailItem
                    {
                        To = new List<MailAddress> { new MailAddress(a.Email, a.FullName) },
                        Cc = new List<MailAddress>(),
                        Subject = $"IVC - Boleta - {week.YearWeekNumber}",
                        Message = $"Hola, { a.FullName }, <br /><br /> " +
                        $"Por parte del centro de costo {project.Abbreviation} se le adjunta su boleta de la semana {week.WeekNumber} del año {week.Year}.<br />" +
                        $"<br /><br />" +
                        $"Saludos <br />" +
                        $"Sistema IVC <br /> " +
                        $"Control de RR.HH.",
                        Attachments = new List<Attachment> { attachment },
                        OnMailSucceeded = async () =>
                        {
                            var isSended = await _context.WorkerInvoiceSends
                                .FirstOrDefaultAsync(x => x.WorkerId == a.WorkerId && x.PayrollMovementHeaderId == header.Id);
                            if (isSended != null)
                            {
                                isSended.DateSended = DateTime.UtcNow.ToDefaultTimeZone();
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                await _context.WorkerInvoiceSends.AddAsync(new WorkerInvoiceSend
                                {
                                    DateSended = DateTime.UtcNow.ToDefaultTimeZone(),
                                    Observation = string.Empty,
                                    PayrollMovementHeaderId = header.Id,
                                    WorkerId = a.WorkerId
                                });
                                await _context.SaveChangesAsync();
                            }
                        },
                        OnMailError = async () =>
                        {
                            var isSended = await _context.WorkerInvoiceSends
                                .FirstOrDefaultAsync(x => x.WorkerId == a.WorkerId && x.PayrollMovementHeaderId == header.Id);
                            if (isSended != null)
                            {
                                isSended.Observation = "Envío Fallido";
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                await _context.WorkerInvoiceSends.AddAsync(new WorkerInvoiceSend
                                {
                                    Observation = "Envío Fallido",
                                    PayrollMovementHeaderId = header.Id,
                                    WorkerId = a.WorkerId
                                });
                                await _context.SaveChangesAsync();
                            }
                        }
                    });

                    Thread.Sleep(15000);
                }
            }



            return Ok();
        }
    }
}
