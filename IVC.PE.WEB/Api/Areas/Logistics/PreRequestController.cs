using ClosedXML.Excel;
using IVC.PE.BINDINGRESOURCES.Areas.Logistic;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Api.Areas.Logistics
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/logistica/pre-requerimientos")]
    public class PreRequestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        public PreRequestController(IvcDbContext context,
            ILogger<PreRequestController> logger,
            IOptions<CloudStorageCredentials> storageCredentials)
            : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");



            var bps = await _context.PreRequests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(X => X.ProjectId == projectId.Value)
                .Select(x => new PreRequestAllListResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    ProjectAbbreviation = x.Project.Abbreviation,
                    CorrelativeCodeStr = x.CorrelativePrefix + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitleId = x.BudgetTitleId, //
                    BudgetName = x.BudgetTitle.Name,
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    //SupplyFamilyId = x.SupplyFamilyId.Value,     //
                    //SupplyFamilyCode = x.SupplyFamily.Code,
                    //SupplyFamilyName = x.SupplyFamily.Name,
                    ProjectFormulaId = x.ProjectFormulaId.Value, //
                    ProjectFormulaCode = x.ProjectFormula.Code,
                    ProjectFormulaName = x.ProjectFormula.Name,
                }).ToListAsync();

            
            return Ok(bps);
        }
        [HttpGet("listar-filtrado")]
        public async Task<IActionResult> GetAllFiltered(Guid? projectId, string userId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");



            var bps = await _context.PreRequests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(X => X.ProjectId == projectId.Value && userId == X.IssuedUserId && (X.OrderStatus == 1 || X.OrderStatus == 6))
                
                .Select(x => new PreRequestAllListResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    ProjectAbbreviation = x.Project.Abbreviation,
                    CorrelativeCodeStr = x.CorrelativePrefix + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    //Status = x.OrderStatus == 1 ? "PRE-EMITIDO" : (x.OrderStatus == 2 ? "EMITIDO" : "APROBADO"),
                    IssuedUserId = x.IssuedUserId,

                    BudgetTitleId = x.BudgetTitleId, //
                    BudgetName = x.BudgetTitle.Name,
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    //SupplyFamilyId = x.SupplyFamilyId.Value,     //
                    //SupplyFamilyCode = x.SupplyFamily.Code,
                    //SupplyFamilyName = x.SupplyFamily.Name,
                    ProjectFormulaId = x.ProjectFormulaId.Value, //
                    ProjectFormulaCode = x.ProjectFormula.Code,
                    ProjectFormulaName = x.ProjectFormula.Name,
                    Observations = x.Observations
                }).ToListAsync();


            return Ok(bps);
        }

        [HttpGet("listar-filtrado-emitido")]
        public async Task<IActionResult> GetAllFilteredEmited(Guid? projectId, string userId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("Debe seleccionar un proyecto.");



            var bps = await _context.PreRequests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectId == projectId.Value && userId == x.IssuedUserId && x.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED
                && x.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)

                .Select(x => new PreRequestAllListResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    ProjectAbbreviation = x.Project.Abbreviation,
                    CorrelativeCodeStr = x.CorrelativePrefix + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    AttentionStatus = x.AttentionStatus,
                    //Status = x.OrderStatus == 1 ? "PRE-EMITIDO" : (x.OrderStatus == 2 ? "EMITIDO" : "APROBADO"),
                    IssuedUserId = x.IssuedUserId,
                    BudgetTitleId = x.BudgetTitleId, //
                    BudgetName = x.BudgetTitle.Name,
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    //SupplyFamilyId = x.SupplyFamilyId.Value,     //
                    //SupplyFamilyCode = x.SupplyFamily.Code,
                    //SupplyFamilyName = x.SupplyFamily.Name,
                    ProjectFormulaId = x.ProjectFormulaId.Value, //
                    ProjectFormulaCode = x.ProjectFormula.Code,
                    ProjectFormulaName = x.ProjectFormula.Name,
                    Observations = x.Observations
                }).ToListAsync();


            return Ok(bps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(Guid? id)
        {


            var bps = await _context.PreRequests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(X => X.Id == id.Value)

                .Select(x => new PreRequestAllListResourceModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    ProjectAbbreviation = x.Project.Abbreviation,
                    CorrelativeCodeStr = x.CorrelativePrefix + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    IssuedUserId = x.IssuedUserId,
                    BudgetTitleId = x.BudgetTitleId, //
                    BudgetName = x.BudgetTitle.Name,
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    //SupplyFamilyId = x.SupplyFamilyId.Value,     //
                    //SupplyFamilyCode = x.SupplyFamily.Code,
                    //SupplyFamilyName = x.SupplyFamily.Name,
                    ProjectFormulaId = x.ProjectFormulaId.Value, //
                    ProjectFormulaCode = x.ProjectFormula.Code,
                    ProjectFormulaName = x.ProjectFormula.Name,

                }).ToListAsync();


            return Ok(bps);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> CreatePart([FromBody] PreRequestRegisterResourceModel model)
        {
            var correlative = await _context.RequestSummaries
             .Where(x => x.ProjectId == model.ProjectId)
             .FirstOrDefaultAsync();

            var count = 0;

            if (_context.PreRequests.Count() != 0)
                count = _context.PreRequests.OrderBy(x => x.CorrelativeCode).Last().CorrelativeCode;
            

            var request = new PreRequest
            {
                ProjectId = model.ProjectId,
                CorrelativeCode = (count + 1),
                CorrelativePrefix = correlative.CodePrefix + "-PR",
                BudgetTitleId = model.BudgetTitleId,
                RequestType = model.RequestType,
                IssueDate = DateTime.Now,
                DeliveryDate = model.DeliveryDate,
                Observations = model.Observations,
                ProjectFormulaId = model.ProjectFormulaId,
                //SupplyFamilyId = model.SupplyFamilyId,
                IssuedUserId = model.IssuedUserId,  
            };

            await _context.PreRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            //var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.IssuedUserId);


            //var preuser = new PreRequestUser
            //{
            //    PreRequestId = request.Id,
            //    UserId = model.IssuedUserId,
            //    FullName = users.PaternalSurname + " " + users.MaternalSurname +"," + users.Name,

            //};

            //await _context.PreRequestUsers.AddAsync(preuser);
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] PreRequestEditResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.PreRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            request.RequestType = model.RequestType;
            request.BudgetTitleId = model.BudgetTitleId;
            request.DeliveryDate = model.DeliveryDate;
            //request.SupplyFamilyId = model.SupplyFamilyId;
            request.ProjectFormulaId = model.ProjectFormulaId;
            request.IssuedUserId = model.IssuedUserId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var request = await _context.PreRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            var items = await _context.PreRequestItems
                .Where(x => x.PreRequestId == id).ToListAsync();

            var reqAuths = await _context.PreRequestAuthorizations
                .Where(x => x.PreRequestId == id).ToListAsync();

            var prereqFiles = await _context.PreRequestFiles
                .Where(x => x.PreRequestId == id).ToListAsync();

            //var users = await _context.PreRequestUsers
            //    .Where(x => x.PreRequestId == id).ToListAsync();

            _context.PreRequestItems.RemoveRange(items);
            //_context.PreRequestUsers.RemoveRange(users);
            _context.PreRequestFiles.RemoveRange(prereqFiles);
            _context.PreRequestAuthorizations.RemoveRange(reqAuths);
            _context.PreRequests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);

            var logAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest)
                .ToListAsync();
            if (request.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED)
            {
                return BadRequest("Solo se pueden emitir pre-requerimientos pre-emitidos");
            }
            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);
            //var linkkk = "https://localhost:44307";
            //var linkAuth = $"{linkkk}/logistica/pre-requerimientos/evaluar/{id}";
            var linkAuth = $"{ConstantHelpers.SystemUrl.Url}logistica/pre-requerimientos/evaluar/{id}";

            request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.ANSWER_PENDING;

            var logExiste = await _context.PreRequestAuthorizations.Where(x => x.PreRequestId == id).ToListAsync();

            _context.RemoveRange(logExiste);

            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Arian"));
            foreach (var auth in logAuths)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                    Subject = $"Aviso de emisión de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
                };
                List<Attachment> archivos = Files(id);
                if (archivos != null)
                    foreach (var item in archivos)
                        mailMessage.Attachments.Add(item);

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);
                var linkResponsible1 = $"{linkAuth}?userId={user.Id}";
                var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                var linkReject1 = $"{linkResponsible1}&isApproved=false";

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

                var author = new PreRequestAuthorization
                {
                    Id = Guid.NewGuid(),
                    PreRequestId = id,
                    UserId = user.Id,
                    UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest,
                    IsApproved = false,
                    ApprovedDate = null
                };
                await _context.PreRequestAuthorizations.AddAsync(author);
                mailMessage.Body =
                   $"Hola, {user.RawFullName}<br /><br /> " +
                   $"Se le solicita revisar y aprobar el pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation}.<br />" +
                   $"<br />" +
                   $"El excel del pre-requerimiento y demás archivos aparecen adjuntos al correo.<br />" +
                   $"<br />" +
                   $"Si todo es conforme haga click en el siguiente enlace: <a href='{linkApprove1}'><span style='color:green'>APROBAR.</span></a><br />" +
                   $"De encontrar alguna disconformidad haga click en el siguiente enlace: <a href='{linkReject1}'><span style='color:red'>DESAPROBAR.</span></a><br />" +
                   $"Saludos Cordiales<br /><br />Sistema IVC <br />" +
                   $"Control de Logística";
                mailMessage.IsBodyHtml = true;
                //Mandar Correo
                using (var client = new SmtpClient("smtp.office365.com", 587))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                    client.EnableSsl = true;
                    await client.SendMailAsync(mailMessage);
                }
            }

            //await _context.RequestAuthorizations.AddRangeAsync(reqAuths);

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("file")]
        public List<Attachment> Files(Guid id)
        {
            var request = _context.PreRequests
                .Include(x => x.Project)
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetTitle)
                //.Include(x => x.SupplyFamily)
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
                    },
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code,
                        Name = x.ProjectFormula.Name
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.CorrelativePrefix + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    IssuedUserId = x.IssuedUserId
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = x.SupplyFamily.Name,
                    //    Code = x.SupplyFamily.Code
                    //}
                }).FirstOrDefault(x => x.Id == id);

            var files = _context.PreRequestFiles
            .Where(x => x.PreRequestId == id).ToList();

            var summary = _context.RequestSummaries
                .FirstOrDefault(x => x.ProjectId == request.ProjectId);

            var meta = _context.GoalBudgetInputs.ToList();

            var logAuths = _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest)
                .ToList();

            var userString = _context.Users.FirstOrDefault(x => x.Id == request.IssuedUserId).FullName;
            /*
            var reqUsers = _context.PreRequestUsers.Where(x => x.PreRequestId == id).ToList();

            var last = reqUsers.Last();
            var first = reqUsers.First();
            foreach (var item in reqUsers)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == item.UserId);
                if (item == first)
                    userString = userString + user.FullName;
                else if (item == last)
                    userString = userString + " y " + user.FullName;
                else
                    userString = userString + ", " + user.FullName;
            }*/
            request.RequestUsernames = userString;

            var thecnicalSpecs = _context.TechnicalSpecs.ToList();

            var items = _context.PreRequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.WorkFront)
                .Where(x => x.PreRequestId == id).ToList();

            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);

            if (request == null || project == null)
                return null;

            List<Attachment> archivos = new List<Attachment>();

            MemoryStream stream = new MemoryStream();
            WebClient client = new WebClient();

            string fileName = $"PreRequerimiento-{request.CorrelativeCodeStr}.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("Formato");

                workSheet.ShowGridLines = false;

                //--------------CAMPOS------------

                var pendiente = "-";
                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 9;
                workSheet.Column(3).Width = 9;
                workSheet.Column(4).Width = 8;
                workSheet.Column(5).Width = 7;
                workSheet.Column(6).Width = 29;
                workSheet.Column(7).Width = 19;
                workSheet.Column(8).Width = 9;
                workSheet.Column(9).Width = 13;
                workSheet.Column(10).Width = 11;
                workSheet.Column(11).Width = 11;
                workSheet.Column(12).Width = 12;
                workSheet.Column(13).Width = 12;
                workSheet.Column(14).Width = 17;
                workSheet.Column(15).Width = 1;

                //----------FILA-1-FILA-5---------

                var enlace = project.LogoUrl.ToString();

                Stream img = client.OpenRead(enlace);
                Bitmap bitmap; bitmap = new Bitmap(img);

                Image image = (Image)bitmap;

                workSheet.Range($"B2:D5").Merge();

                var aux = workSheet.AddPicture(bitmap);
                aux.MoveTo(16, 22);
                aux.Height = 81;
                aux.Width = 190;

                workSheet.Cell($"E2").Value = "GESTIÓN DE COMPRAS EN OBRA";
                workSheet.Range($"E2:L2").Merge();
                workSheet.Range($"E2:L2").Style.Font.Bold = true;

                workSheet.Cell($"E3").Value = "PRE-REQUERIMIENTO DE COMPRA / SERVICIO";
                workSheet.Range($"E3:L5").Merge();
                workSheet.Range($"E3:L5").Style.Font.Bold = true;

                workSheet.Cell($"M2").Value = "Código";
                workSheet.Cell($"N2").Value = summary.CodePrefix + "/GCO-For-05A";
                workSheet.Cell($"N2").Style.Font.Bold = true;

                workSheet.Cell($"M3").Value = "Versión";
                workSheet.Cell($"N3").Value = "1";
                workSheet.Cell($"N3").Style.Font.Bold = true;

                workSheet.Cell($"M4").Value = "Fecha";
                workSheet.Cell($"N4").Value = "03/01/2022";
                workSheet.Cell($"N4").Style.Font.Bold = true;

                workSheet.Cell($"M5").Value = "Página";
                var pagina = "1 de 1";
                workSheet.Cell($"N5").SetValue(pagina);
                workSheet.Cell($"N5").Style.Font.Bold = true;

                workSheet.Row(2).Height = 22;
                workSheet.Row(3).Height = 15;
                workSheet.Row(4).Height = 15;
                workSheet.Row(5).Height = 15;

                workSheet.Range("B2:N5").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:N5").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //----------FILA-6-FILA-9---------

                workSheet.Cell($"B6").Value = "Cod. Obra";
                workSheet.Cell($"B6").Style.Font.Bold = true;
                workSheet.Cell($"C6").Value = project.Abbreviation;
                workSheet.Cell($"C6").Style.Alignment.WrapText = true;
                workSheet.Range($"C6:D6").Merge();

                workSheet.Cell($"E6").Value = "Obra: ";
                workSheet.Cell($"E6").Style.Font.Bold = true;
                workSheet.Cell($"F6").Value = project.Name;
                workSheet.Cell($"F6").Style.Font.Bold = true;
                workSheet.Cell($"F6").Style.Alignment.WrapText = true;
                workSheet.Range($"F6:L6").Merge();

                workSheet.Cell($"M6").Value = "Solicitud N°:";
                workSheet.Cell($"M6").Style.Font.Bold = true;
                workSheet.Cell($"N6").Value = request.CorrelativeCodeStr;

                workSheet.Cell($"B7").Value = "COMPRA";
                workSheet.Range($"B7:C7").Merge();
                workSheet.Range($"B7:C7").Style.Font.Bold = true;

                workSheet.Cell($"B8").Value = "SERVICIO";
                workSheet.Range($"B8:C8").Merge();
                workSheet.Range($"B8:C8").Style.Font.Bold = true;

                if (request.RequestType == 1)
                {
                    workSheet.Cell($"D7").Value = " ☒ ";
                    workSheet.Cell($"D8").Value = " ☐ ";
                }
                else
                {
                    workSheet.Cell($"D7").Value = " ☐ ";
                    workSheet.Cell($"D8").Value = " ☒ ";
                }


                workSheet.Cell($"E7").Value = "Presupuesto";
                workSheet.Range($"E7:F7").Merge();
                workSheet.Range($"E7:F7").Style.Font.Bold = true;
                workSheet.Cell($"E8").Value = request.BudgetTitle.Name;
                workSheet.Range($"E8:F8").Merge();

                workSheet.Cell($"G7").Value = "Familia";
                workSheet.Range($"G7:I7").Merge();
                workSheet.Range($"G7:I7").Style.Font.Bold = true;
                //workSheet.Cell($"G8").Value = request.SupplyFamily.FullName;
                workSheet.Range($"G8:I8").Merge();

                workSheet.Cell($"J7").Value = "Fórmulas";
                workSheet.Range($"J7:N7").Merge();
                workSheet.Range($"J7:N7").Style.Font.Bold = true;
                workSheet.Cell($"J8").Value = request.ProjectFormula.Code + " - " + request.ProjectFormula.Name;
                workSheet.Range($"J8:N8").Merge();

                workSheet.Cell($"B9").Value = "Solicitado por:";
                workSheet.Range($"B9:C9").Merge();
                workSheet.Range($"B9:C9").Style.Font.Bold = true;

                workSheet.Cell($"D9").Value = request.RequestUsernames;
                workSheet.Range($"D9:F9").Merge();
                workSheet.Cell($"D9").Style.Alignment.WrapText = true;

                workSheet.Cell($"G9").Value = "Proceso Solicitante:";
                workSheet.Cell($"G9").Style.Font.Bold = true;

                workSheet.Cell($"H9").Value = "Ejecución de Obra";
                workSheet.Range($"H9:I9").Merge();

                workSheet.Cell($"J9").Value = "Fecha:";
                workSheet.Cell($"J9").Style.Font.Bold = true;

                workSheet.Cell($"K9").SetValue(request.IssueDate);

                workSheet.Cell($"L9").Value = "Estado:";
                workSheet.Range($"L9:N9").Merge();
                workSheet.Cell($"L9").Style.Font.Bold = true;

                workSheet.Cell($"J10").Value = "Fecha:";
                workSheet.Cell($"J10").Style.Font.Bold = true;

                workSheet.Cell($"J11").Value = "Fecha:";
                workSheet.Cell($"J11").Style.Font.Bold = true;

                workSheet.Cell($"B10").Value = "Autorizado por:";
                workSheet.Range($"B10:C10").Merge();
                workSheet.Range($"D10:G10").Merge();
                workSheet.Range($"B10:C10").Style.Font.Bold = true;

                workSheet.Cell($"B11").Value = "Autorizado por:";
                workSheet.Range($"B11:C11").Merge();
                workSheet.Range($"D11:G11").Merge();
                workSheet.Range($"B11:C11").Style.Font.Bold = true;

                var responsible1 = logAuths.First();
                var responsible2 = logAuths.Last();

                var users = _context.Users.Where(x => x.Id == responsible1.UserId || x.Id == responsible2.UserId).ToList();

                workSheet.Cell($"D10").Value = users.First().FullName;
                workSheet.Cell($"D11").Value = users.Last().FullName;

                if (request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ISSUED ||
                   //request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.PROCESSED ||
                   //request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.PROCESSED_PARTIALLY ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ANSWER_PENDING)
                {
                    var auths = _context.PreRequestAuthorizations.Where(x => x.PreRequestId == id).ToList();

                    var res1Date = auths.FirstOrDefault(x => x.UserId == responsible1.UserId).ApprovedDate;
                    var res2Date = auths.FirstOrDefault(x => x.UserId == responsible2.UserId).ApprovedDate;

                    workSheet.Cell($"K10").SetValue(res1Date.HasValue ? res1Date.Value.ToString("dd/MM/yyyy") : "");
                    workSheet.Cell($"K11").SetValue(res2Date.HasValue ? res2Date.Value.ToString("dd/MM/yyyy") : "");
                }
                if (users.First().SignatureUrl != null)
                {
                    var enlace2 = users.First().SignatureUrl.ToString();

                    Stream imgFirma = client.OpenRead(enlace2);
                    Bitmap bitmapFirma = new Bitmap(imgFirma);

                    var aux2 = workSheet.AddPicture(bitmapFirma).MoveTo(workSheet.Cell("H10")).WithSize(120, 60);

                    aux2.MoveTo(650, 272);
                    aux2.Height = 60;
                    aux2.Width = 120;
                }
                else
                {
                    workSheet.Range($"H10:I10").Merge();
                    workSheet.Cell($"H10").Value = "Sin firma";
                }

                if (users.Last().SignatureUrl != null)
                {
                    var enlace3 = users.Last().SignatureUrl.ToString();

                    Stream imgFirma2 = client.OpenRead(enlace3);
                    Bitmap bitmapFirma2 = new Bitmap(imgFirma2);

                    var aux3 = workSheet.AddPicture(bitmapFirma2).MoveTo(workSheet.Cell("H11")).WithSize(120, 60);

                    aux3.MoveTo(650, 337);
                    aux3.Height = 60;
                    aux3.Width = 120;
                }
                else
                {
                    workSheet.Range($"H11:I11").Merge();
                    workSheet.Cell($"H11").Value = "Sin firma";
                }

                if (request.OrderStatus == 1)
                    workSheet.Cell($"L10").Value = "PRE-EMITIDO";
                else if (request.OrderStatus == 2)
                    workSheet.Cell($"L10").Value = "EMITIDO";
                else if (request.OrderStatus == 6)
                    workSheet.Cell($"L10").Value = "OBSERVADO";
                else if (request.OrderStatus == 7)
                    workSheet.Cell($"L10").Value = "PENDIENTE DE RESPUESTA";
                else if (request.OrderStatus == 8)
                    workSheet.Cell($"L10").Value = "APROBADO";
                else if (request.OrderStatus == 9)
                    workSheet.Cell($"L10").Value = "APROBADO PARCIALMENTE";
                else
                    workSheet.Cell($"L10").Value = "PROCESADO";
                workSheet.Range($"L10:N11").Merge();

                workSheet.Row(6).Height = 46;
                workSheet.Row(7).Height = 20;
                workSheet.Row(8).Height = 20;
                workSheet.Row(9).Height = 36;
                workSheet.Row(10).Height = 48;
                workSheet.Row(11).Height = 48;


                workSheet.Range("B6:D6").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B6:D6").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("E6:L6").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("M6:N6").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("M6:N6").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("B7:D8").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("E7:F8").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("G7:I8").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("J7:N8").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("B9:N9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("J9:K9").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("B10:N10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("H10:I10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("B11:N11").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("H11:I11").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("J10:K10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("J11:K11").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);


                //-------------TABLA------------

                workSheet.Row(12).Height = 12;
                workSheet.Range($"B12:N12").Merge();

                workSheet.Cell($"B13").Value = "ITEM";
                workSheet.Cell($"B13").Style.Font.Bold = true;
                workSheet.Cell($"C13").Value = "CANTIDAD";
                workSheet.Cell($"C13").Style.Font.Bold = true;
                workSheet.Cell($"D13").Value = "UNIDAD";
                workSheet.Cell($"D13").Style.Font.Bold = true;
                workSheet.Cell($"E13").Value = "DESCRIPCIÓN DETALLADA DEL PRODUCTO Y/O SERVICIO";
                workSheet.Range($"E13:G13").Merge();
                workSheet.Range($"E13:G13").Style.Font.Bold = true;
                workSheet.Cell($"H13").Value = "GRUPO";
                workSheet.Cell($"H13").Style.Font.Bold = true;
                workSheet.Cell($"I13").Value = "COSTO APROXIMADO";
                workSheet.Cell($"I13").Style.Alignment.WrapText = true;
                workSheet.Cell($"I13").Style.Font.Bold = true;
                workSheet.Cell($"J13").Value = "FECHA DE ENTREGA";
                workSheet.Cell($"J13").Style.Alignment.WrapText = true;
                workSheet.Cell($"J13").Style.Font.Bold = true;
                workSheet.Cell($"K13").Value = "STOCK EN ALMACEN";
                workSheet.Cell($"K13").Style.Alignment.WrapText = true;
                workSheet.Cell($"K13").Style.Font.Bold = true;
                workSheet.Cell($"L13").Value = "PARA SER USADO EN:";
                workSheet.Range($"L13:M13").Merge();
                workSheet.Range($"L13:M13").Style.Font.Bold = true;
                workSheet.Cell($"N13").Value = "OBSERVACIONES";
                workSheet.Cell($"N13").Style.Font.Bold = true;

                workSheet.Range("B13:N13").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B13:N13").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B13:N13").Style.Fill.SetBackgroundColor(XLColor.LightGray);

                var fila = 14;
                var contador = 1;

                foreach (var item in items)
                {
                    var goalBudgetInput = meta.FirstOrDefault(x => x.SupplyId == item.SupplyId && x.WorkFrontId == item.WorkFrontId);

                    workSheet.Cell($"B${fila}").Value = contador;
                    workSheet.Cell($"C${fila}").Value = item.Measure;
                    workSheet.Cell($"D${fila}").Value = item.SupplyId != null ? item.Supply.MeasurementUnit.Abbreviation : item.MeasurementUnitName;
                    workSheet.Cell($"E${fila}").Value = " " + (item.SupplyId != null ? item.Supply.Description : item.SupplyName);
                    workSheet.Range($"E${fila}:G${fila}").Merge();
                    workSheet.Cell($"H${fila}").Value = item.SupplyId != null ? item.Supply.SupplyGroup.Name : "";
                    workSheet.Cell($"H${fila}").Style.Alignment.WrapText = true;
                    //workSheet.Cell($"I${fila}").SetValue(goalBudgetInput.UnitPrice.ToString("N2", CultureInfo.InvariantCulture));
                    workSheet.Cell($"J${fila}").SetValue(request.DeliveryDate);
                    workSheet.Cell($"K${fila}").Value = pendiente;
                    workSheet.Cell($"L${fila}").Value = item.WorkFront.Code;
                    workSheet.Range($"L${fila}:M${fila}").Merge();
                    workSheet.Cell($"N${fila}").Value = item.Observations;

                    fila++;
                    contador++;
                }

                while (fila < 32)
                {
                    workSheet.Range($"E${fila}:G${fila}").Merge();
                    workSheet.Range($"L${fila}:M${fila}").Merge();

                    fila++;
                }

                workSheet.Range($"B14:N${fila - 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B14:N${fila - 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                workSheet.Cell($"E${fila}").Value = "Revisado por: GCO";
                workSheet.Cell($"H${fila}").Value = "Aprobado por: GAO";
                workSheet.Cell($"L${fila}").Value = "SIG/SGAS";
                workSheet.Range($"L${fila}:M${fila}").Merge();
                workSheet.Range($"B${fila}:N${fila}").Style.Font.FontColor = XLColor.BlueGray;


                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range($"E7:F7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G7:I7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J7:N7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //workSheet.Range($"B9:C9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"E14:G${fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Range($"E8:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G8:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J8:N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                archivos.Add(new Attachment(stream, fileName)); ////////////////

                wb.SaveAs(stream);              ///////////////////////////////
                stream.Position = 0;            ///////////////////////////////
                stream.Seek(0, SeekOrigin.Begin);   ///////////////////////////

                foreach (var item in files)
                {
                    Stream auxA = client.OpenRead(item.FileUrl);
                    archivos.Add(new Attachment(auxA, item.FileUrl.LocalPath));
                }


                return archivos;
            }

        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetAllDetails(Guid? reqId = null)
        {

            var query = await _context.PreRequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.WorkFront)
                .Include(x => x.Supply.SupplyGroup)
                .Where(x => x.PreRequestId == reqId.Value)
                .AsNoTracking()
                .Select(x => new PreRequestDetailListResourceModel
                {
                    Id = x.Id,
                    PreRequestId = x.PreRequestId,
                    SupplyId = (Guid)x.SupplyId,
                    SupplyGroupStr = x.Supply.SupplyGroup.Name != null? x.Supply.SupplyGroup.Name : "Ingreso Manual",
                    Color = x.Supply.SupplyGroup.Name != null ? "Black" : "Red",
                    Metered  = x.Supply.MeasurementUnit.Abbreviation != null? x.Supply.MeasurementUnit.Abbreviation : null,
                    SupplyDescription = x.Supply.Description != null ? x.Supply.Description : null,
                    WorkFrontId = x.WorkFrontId,
                    WorkFrontCode = "Frente: " + x.WorkFront.Code,
                    MeasureStr = "Metrado: " + x.Measure.ToString(),
                    MeasureInAttention = x.MeasureInAttention,
                    Observations = "Observación: " + x.Observations,
                    UsedFor = x.UsedFor,
                    SupplyName = x.SupplyName,
                    MeasurementUnitName = x.MeasurementUnitName
                    
                }).ToListAsync();

            return Ok(query);
        }


        [HttpPost("detalles/registrar")]
        public async Task<IActionResult> CreateDetailtPart([FromBody] PreRequestDetailRegisterResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestItem = new PreRequestItem
            {
                PreRequestId = model.PreRequestId,
                Measure = model.Measure,
                WorkFrontId = model.WorkFrontId,
                Observations = model.Observations,
                UsedFor = model.UsedFor
            };

            if (model.SupplyManual == true)
            {
                if (string.IsNullOrEmpty(model.SupplyName) || string.IsNullOrEmpty(model.MeasurementUnitName))
                    return BadRequest("Si el ingreso es manual debe completarse el nombre del insumo y su unidad");

                requestItem.SupplyName = model.SupplyName;
                requestItem.MeasurementUnitName = model.MeasurementUnitName;
                requestItem.SupplyId = null;
            }

            else
            {
                if (model.SupplyId == null)
                    return BadRequest("No se ha ingresado el insumo");
                requestItem.SupplyName = null;
                requestItem.MeasurementUnitName = null;
                requestItem.SupplyId = model.SupplyId;
            }

            await _context.PreRequestItems.AddAsync(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("detalles/editar/{id}")]
        public async Task<IActionResult> EditDetail(Guid id, [FromBody] PreRequestDetailRegisterResourceModel model)
        {


            var requestItem = await _context.PreRequestItems
                .FirstOrDefaultAsync(x => x.Id == id);


            requestItem.Measure = model.Measure;
            requestItem.Observations = model.Observations;
            requestItem.WorkFrontId = model.WorkFrontId;
            requestItem.UsedFor = model.UsedFor;


            if (model.SupplyManual == true)
            {
                requestItem.SupplyName = model.SupplyName;
                requestItem.MeasurementUnitName = model.MeasurementUnitName;
                requestItem.SupplyId = null;
            }

            else
            {
                if (model.SupplyId == null)
                    return BadRequest("No se ha ingresado el insumo");

                requestItem.SupplyId = model.SupplyId;
                requestItem.SupplyName = null;
                requestItem.MeasurementUnitName = null;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("detalles/eliminar/{id}")]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var requestItem = await _context.PreRequestItems
                .FirstOrDefaultAsync(x => x.Id == id);
            if (requestItem == null)
                return BadRequest($"Elemento con Id '{id}' no encontrado.");
            _context.PreRequestItems.Remove(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("obtener-unidad-insumo-meta/{id}")]
        public async Task<IActionResult> GetMeasureUnit(Guid id)
        {
            var unit = await _context.Supplies
                .Include(x => x.MeasurementUnit)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (unit == null)
                return BadRequest();

            return Ok(unit.MeasurementUnit
                    .Abbreviation);
        }

        public void UpdateOrder(int status,string str)
        {
            switch (status)
            {
                case 1:
                    str = "PRE-EMITIDO";
                    break;
                case 2:
                    str = "EMITIDO";
                    break;
                case 3:
                    str = "APROBADO";
                    break;
                default:
                    break;
            }
        }
    }
}
