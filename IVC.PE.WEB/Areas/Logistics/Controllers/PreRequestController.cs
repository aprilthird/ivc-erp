using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.LogisticResponsibleViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.PreRequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/pre-requerimientos")]
    public class PreRequestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public PreRequestController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            IConfiguration configuration) : base(context, userManager, configuration)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listado")]
        public IActionResult List() => View();

        [HttpGet("{id}/detalles")]
        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _context.PreRequests.FindAsync(id);
            ViewBag.RequestId = request.Id;
            ViewBag.RequestCode = request.CorrelativeCode;
            return View();
        }

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var summary = await _context.RequestSummaries
                .FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());

            var query = await _context.PreRequests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Where(x => x.ProjectId == GetProjectId() && 
                (x.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED || 
                x.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED))
                .AsNoTracking()
                .Select(x => new PreRequestViewModel
                {
                    Id = x.Id,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
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
                    Observations = x.Observations,
                    //SupplyFamilyId = (Guid)x.SupplyFamilyId,
                    ProjectFormulaId = (Guid)x.ProjectFormulaId,
                    IssuedUserId = x.IssuedUserId
                }).ToListAsync();

            var users = await _context.Users.ToListAsync();
            
            foreach (var req in query)
            {
                var userString = users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;
                req.RequestUsernames = userString;
            }

            return Ok(query);
        }

        [HttpGet("listado/listar")]
        public async Task<IActionResult> GetAllListed(int status = 0, int attentionStatus = 0)
        {
            var summary = await _context.RequestSummaries
                .FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());
            var auths = _context.PreRequestAuthorizations.ToList();
            var query = await _context.PreRequests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Where(x => x.ProjectId == GetProjectId() && x.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED
                && x.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
                .AsNoTracking()
                .Select(x => new PreRequestViewModel
                {
                    Id = x.Id,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
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
                    IssuedUserId = x.IssuedUserId,
                    AttentionStatus = x.AttentionStatus,
                    
                }).ToListAsync();

            var users = await _context.Users.ToListAsync();
            
            foreach (var req in query)
            {
                var userString = users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;
                var authDate = auths.Where(x => x.PreRequestId == req.Id).OrderByDescending(x => x.ApprovedDate).FirstOrDefault().ApprovedDate;
                if (authDate != null)
                    req.ApproveDate = authDate.Value.ToDateString();
                else
                    req.ApproveDate = "---";
                req.RequestUsernames = userString;
            }

            if (status != 0)
            {
                query = query.Where(x => x.OrderStatus == status).ToList();
            }

            if (attentionStatus != 0)
            {
                query = query.Where(x => x.AttentionStatus == attentionStatus).ToList();
            }

            return Ok(query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var hasFiles = await _context.PreRequestFiles
                .Where(x => x.PreRequestId == id)
                .CountAsync() > 0;

            var data = await _context.PreRequests
                .Where(x => x.Id == id)
                .Select(x => new PreRequestViewModel
                {
                    Id = x.Id,
                    RequestType = x.RequestType,
                    BudgetTitleId = x.BudgetTitleId,
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.CorrelativePrefix + x.CorrelativeCode.ToString("D4"),
                    //PreRequestUserIds = x.PreRequestUsers.Select(i => i.UserId).ToList(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    Observations = x.Observations,
                    //SupplyFamilyId = (Guid)x.SupplyFamilyId,
                    ProjectFormulaId = (Guid)x.ProjectFormulaId,
                    IssuedUserId = x.IssuedUserId,
                    HasFiles = hasFiles
                }).AsNoTracking()
                .FirstOrDefaultAsync();

            data.IssuedUserName = _context.Users.FirstOrDefault(x => x.Id == data.IssuedUserId).FullName;

            /*
            var files = await _context.RequestFiles
                .Where(x => x.RequestId == id)
                .Select(x => Path.GetFileName(x.FileUrl.LocalPath))
                .AsNoTracking()
                .ToListAsync();
            data.RequestFileNames = files;
            */

            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("crear")]
        public async Task<IActionResult> Create(PreRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //int compare = DateTime.Compare(model.DeliveryDate.ToDateTime(), DateTime.Today);

            //if (compare < 0)
              //  return BadRequest("La fecha de entrega no puede ser anterior al día hoy");

            var correlative = await _context.RequestSummaries
                .Where(x => x.ProjectId == GetProjectId())
                .FirstOrDefaultAsync();

            var count = 0;

            if(_context.PreRequests.Count() != 0)
                count = _context.PreRequests.OrderBy(x => x.CorrelativeCode).Last().CorrelativeCode;

            var request = new PreRequest
            {
                ProjectId = GetProjectId(),
                CorrelativeCode = (count + 1),
                CorrelativePrefix = correlative.CodePrefix + "-PR",
                BudgetTitleId = model.BudgetTitleId,
                RequestType = model.RequestType,
                IssueDate = DateTime.Today,
                DeliveryDate = model.DeliveryDate.ToDateTime(),
                Observations = string.Empty,
                ProjectFormulaId = model.ProjectFormulaId,
                //SupplyFamilyId = model.SupplyFamilyId,
                IssuedUserId = GetUserId()
            };

            //Lógica para carga de Archivos
            /*
            await _context.PreRequestUsers.AddRangeAsync(
                model.PreRequestUserIds.Select(x => new PreRequestUser
                {
                    PreRequest = request,
                    UserId = x
                }).ToList());
            */
            await _context.PreRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            return Ok(request.Id);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PreRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.PreRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            if(request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }

            request.RequestType = model.RequestType;
            request.BudgetTitleId = model.BudgetTitleId;
            request.DeliveryDate = model.DeliveryDate.ToDateTime();
            //request.SupplyFamilyId = model.SupplyFamilyId;
            request.ProjectFormulaId = model.ProjectFormulaId;

            //Lógica de Archivos
            /*
            var users = await _context.PreRequestUsers
                .Where(x => x.PreRequestId == id).ToListAsync();

            _context.PreRequestUsers.RemoveRange(users);
            await _context.PreRequestUsers.AddRangeAsync(
                model.PreRequestUserIds.Select(x => new PreRequestUser
                {
                    PreRequestId = id,
                    UserId = x
                }).ToList());
            */
            await _context.SaveChangesAsync();

            return Ok(id);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var request = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);
            var responsibles = _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId).ToList();
            var logAuths = responsibles
                .FirstOrDefault(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest);

            var secAuths = responsibles
                .FirstOrDefault(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest);

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
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"Aviso de emisión de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
            };
            await _context.SaveChangesAsync();
            List<Attachment> archivos = Files(id);
            if (archivos != null)
                foreach (var item in archivos)
                    mailMessage.Attachments.Add(item);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == logAuths.UserId);
           
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
            if (secAuths != null)
            {
                var secUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == secAuths.UserId);
                var secauthor = new PreRequestAuthorization
                {
                    Id = Guid.NewGuid(),
                    PreRequestId = id,
                    UserId = secUser.Id,
                    UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest,
                    IsApproved = false,
                    ApprovedDate = null
                };
                await _context.PreRequestAuthorizations.AddAsync(secauthor);
            }
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
            

            //await _context.RequestAuthorizations.AddRangeAsync(reqAuths);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("obs/{id}")]
        public async Task<IActionResult> UpdateObservations(Guid id, PreRequestObsViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var prerequest = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (model.PreRequestFiles != null)
            {
                var newFiles = new List<PreRequestFile>();
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var file in model.PreRequestFiles)
                {
                    newFiles.Add(new PreRequestFile
                    {
                        PreRequestId = id,
                        FileUrl = await storage.UploadFile(file.OpenReadStream(),
                                    ConstantHelpers.Storage.Containers.LOGISTICS,
                                    Path.GetExtension(file.FileName),
                                    ConstantHelpers.Storage.Blobs.PREREQUEST_FILES,
                                    $"{prerequest.CorrelativePrefix}-{prerequest.CorrelativeCode}-{Path.GetFileNameWithoutExtension(file.FileName)}")
                    });
                }
                await _context.PreRequestFiles.AddRangeAsync(newFiles);
            }

            //prerequest.Observations = model.Observations;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("{id}/actualizar-atencion")]
        public async Task<IActionResult> UpdateAttention(Guid id, int status)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = await _context.PreRequests.FindAsync(id);
            request.AttentionStatus = status;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
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

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("archivo/eliminar")]
        public async Task<IActionResult> DeleteFile(Uri url)
        {
            var file = await _context.PreRequestFiles
                .Include(x=>x.PreRequest)
                .FirstOrDefaultAsync(x => x.FileUrl == url);

            if (file.PreRequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                file.PreRequest.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }

            if (file.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.PREREQUEST_FILES}/{file.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.LOGISTICS);
            }

            _context.PreRequestFiles.Remove(file);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetAllDetails(Guid? reqId = null)
        {
            if (!reqId.HasValue)
                return Ok(new List<PreRequestItemViewModel>());

            var query = await _context.PreRequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.PreRequestId == reqId.Value)
                .AsNoTracking()
                .Select(x => new PreRequestItemViewModel
                {
                    Id = x.Id,
                    Supply = new SupplyViewModel
                    {
                        Description = x.SupplyId != null ? x.Supply.Description : "",
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.SupplyId != null ? x.Supply.MeasurementUnit.Abbreviation : ""
                        }
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    Measure = x.Measure,
                    MeasureInAttention = x.MeasureInAttention,
                    MeasurementUnitName = x.MeasurementUnitName,
                    SupplyGroupStr = x.SupplyId != null ? x.Supply.SupplyGroup.Name : "",
                    Observations = x.Observations,
                    UsedFor = x.UsedFor,
                    SupplyName = x.SupplyName
                }).ToListAsync();

            return Ok(query);
        }

        [HttpGet("detalles/{id}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var data = await _context.PreRequestItems
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.Id == id)
                .Select(x => new PreRequestItemViewModel
                {
                    Id = x.Id,
                    PreRequestId = x.PreRequestId,
                    WorkFrontId = x.WorkFrontId,
                    Measure = x.Measure,
                    SupplyId = (Guid)x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        MeasurementUnit = new MeasurementUnitViewModel
                        {
                            Abbreviation = x.SupplyId != null ? x.Supply.MeasurementUnit.Abbreviation : ""
                        }
                    },
                    Observations = x.Observations,
                    UsedFor = x.UsedFor,
                    SupplyName = x.SupplyName,
                    MeasurementUnitName = x.MeasurementUnitName,
                    SupplyManual = string.IsNullOrEmpty(x.SupplyName) ? false : true
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("detalles/crear")]
        public async Task<IActionResult> CreateDetail(PreRequestItemViewModel model)
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

            var prerequest = _context.PreRequests.FirstOrDefault(x => x.Id == model.PreRequestId);

            if (prerequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                prerequest.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }
            
            if (model.SupplyManual == true)
            {
                if (string.IsNullOrEmpty(model.SupplyName) || string.IsNullOrEmpty(model.MeasurementUnitName))
                    return BadRequest("Si el ingreso es manual debe completarse el nombre del insumo y su unidad");

                requestItem.SupplyName = model.SupplyName;
                requestItem.MeasurementUnitName = model.MeasurementUnitName;
            }
            else
            {
                if (model.SupplyId == null)
                    return BadRequest("No se ha ingresado el insumo");

                requestItem.SupplyId = model.SupplyId;
            }

            await _context.PreRequestItems.AddAsync(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("detalles/editar/{id}")]
        public async Task<IActionResult> EditDetail(Guid id, PreRequestItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var requestItem = await _context.PreRequestItems.Include(x => x.PreRequest)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(requestItem.PreRequest.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL ||
               requestItem.PreRequest.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL)
            {
                return BadRequest("El pre-requerimiento se encuentra procesado");
            }

            if(requestItem.PreRequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                requestItem.PreRequest.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }

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

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("detalles/eliminar/{id}")]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var requestItem = await _context.PreRequestItems.Include(x => x.PreRequest)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (requestItem == null)
                return BadRequest($"Elemento con Id '{id}' no encontrado.");
            
            if (requestItem.PreRequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                requestItem.PreRequest.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }
            
            
            _context.PreRequestItems.Remove(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*
        [HttpGet("insumo-meta/{id}")]
        public async Task<IActionResult> GetGoalBudgetInputRequired(Guid id)
        {
            var data = await _context.PreRequestItems
                .Where(x => x.GoalBudgetInputId == id)
                .ToListAsync();

            var goalBudgetInput = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == id);

            if (goalBudgetInput == null)
                return BadRequest("No se ha encontrado el insumo meta");

            var sumaMetered = 0.0;

            foreach (var item in data)
            {
                sumaMetered += item.Measure;
            }

            var res = goalBudgetInput.CurrentMetered - sumaMetered;

            return Ok(Math.Round(res, 2));
        }
        */
        [HttpGet("descargar-requerimiento")]
        public async Task<IActionResult> DownloadPdfRequest(string url)
        {
            Uri serviceUrl = new Uri($"https://erp-ivc-pdf.azurewebsites.net/api/functionapp");

            if (!String.IsNullOrEmpty(url))
            {
                serviceUrl = new Uri(serviceUrl + $"?url={url}");
            }

            using (var wc = new WebClient())
            {
                byte[] pdfBytes = wc.DownloadData(serviceUrl);
                return File(pdfBytes, "application/pdf", "requerimiento.pdf");
            }
        }

        [HttpPost("requerimientos/detalles/listar")]
        public async Task<IActionResult> GetAllDetailsInRequests(List<Guid> preReqIds)
        {
            var data = new List<PreRequestItemViewModel>();

            var meta = await _context.GoalBudgetInputs
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .ToListAsync();

            foreach (var id in preReqIds)
            {
                var query = await _context.PreRequestItems
                    .Include(x => x.PreRequest)
                    .Include(x => x.Supply)
                    .Include(x => x.Supply.SupplyGroup)
                    .Include(x => x.Supply.MeasurementUnit)
                    .Where(x => x.PreRequestId == id
                    && x.Measure != x.MeasureInAttention)
                    .AsNoTracking()
                    .Select(x => new PreRequestItemViewModel
                    {
                        Id = x.Id,
                        SupplyId = (Guid)x.SupplyId,
                        Supply = new SupplyViewModel
                        {
                            Description = x.Supply.Description,
                            SupplyFamily = new SupplyFamilyViewModel
                            {
                                Code = x.Supply.SupplyFamily.Code
                            },
                            SupplyGroupId = x.Supply.SupplyGroupId,
                            SupplyGroup = new SupplyGroupViewModel
                            {
                                Code = x.Supply.SupplyGroup.Code
                            },
                            CorrelativeCode = x.Supply.CorrelativeCode
                        },
                        WorkFrontId = x.WorkFrontId,
                        WorkFront = new WorkFrontViewModel
                        {
                            Code = x.WorkFront.Code
                        },
                        PreRequestId = x.PreRequestId,
                        PreRequest = new PreRequestViewModel
                        {
                            RequestType = x.PreRequest.RequestType,
                            CorrelativeCodeStr = x.PreRequest.CorrelativePrefix + x.PreRequest.CorrelativeCode.ToString("D4"),
                        },
                        Measure = x.Measure,
                        MeasurementUnitName = x.Supply.MeasurementUnit.Abbreviation,
                        SupplyGroupStr = x.Supply.SupplyGroup.Name,
                        Observations = x.Observations,
                        MeasureToAttent = (x.Measure - x.MeasureInAttention),
                        MeasureInAttention = x.MeasureInAttention
                    }).ToListAsync();

                data.AddRange(query);
            }

            foreach(var item in data)
            {
                var aux = meta.FirstOrDefault(x => x.SupplyId == item.SupplyId && x.WorkFrontId == item.WorkFrontId);

                if (aux == null)
                    item.Validator = "Sin techo";
                else
                    item.Validator = aux.CurrentMetered.ToString(CultureInfo.InvariantCulture);
            }

            return Ok(data);
        }

        [HttpGet("excel/{id}")]
        public FileResult ExportExcel(Guid id)
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
                    IssuedUserId = x.IssuedUserId,
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = x.SupplyFamily.Name,
                    //    Code = x.SupplyFamily.Code
                    //}
                }).FirstOrDefault(x => x.Id == id);

            var summary = _context.RequestSummaries
                .FirstOrDefault(x => x.ProjectId == request.ProjectId);

            var meta = _context.GoalBudgetInputs.ToList();

            var userString = "";
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
                    userString = userString + " y "  + user.FullName;
                else
                    userString = userString + ", " + user.FullName;
            }
            */
            var logAuths = _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest)
                .ToList();

            //request.RequestUsernames = userString;

            var items = _context.PreRequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.WorkFront)
                .Where(x => x.PreRequestId == id).ToList();

            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);

            if (request == null || project == null)
                return null;

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

                WebClient client = new WebClient();
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

                var reqUserAux = _context.Users.FirstOrDefault(x => x.Id == request.IssuedUserId);

                workSheet.Cell($"D9").Value = reqUserAux.FullName;
                workSheet.Range($"D9:F9").Merge();
                workSheet.Cell($"D9").Style.Alignment.WrapText = true;

                if (reqUserAux.SignatureUrl != null)
                {
                    
                    var enl = reqUserAux.SignatureUrl.ToString();

                    Stream imgSig = client.OpenRead(enl);
                    Bitmap bitmapSig = new Bitmap(imgSig);

                    var auxSig = workSheet.AddPicture(bitmapSig).MoveTo(workSheet.Cell("F9")).WithSize(80, 40);
                    auxSig.MoveTo(395, 225);
                    auxSig.Height = 40;
                    auxSig.Width = 80;
                }

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

                if(request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ISSUED)
                {
                    var auths = _context.PreRequestAuthorizations.Where(x => x.PreRequestId == id && x.ApprovedDate != null).ToList();
                    var responsible1 = auths.First();
                    var responsible2 = auths.Last();

                    var users = _context.Users.Where(x => x.Id == responsible1.UserId || x.Id == responsible2.UserId).ToList();
                    
                    workSheet.Cell($"D10").Value = users.First().FullName;
                    var res1Date = auths.FirstOrDefault(x => x.UserId == responsible1.UserId).ApprovedDate;
                    workSheet.Cell($"K10").SetValue(res1Date.HasValue ? res1Date.Value.ToString("dd/MM/yyyy") : "");

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

                    if (responsible1.Id != responsible2.Id)
                    {
                        workSheet.Cell($"D11").Value = users.Last().FullName;
                        var res2Date = auths.FirstOrDefault(x => x.UserId == responsible2.UserId).ApprovedDate;
                        workSheet.Cell($"K11").SetValue(res2Date.HasValue ? res2Date.Value.ToString("dd/MM/yyyy") : "");

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
                    }
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

                workSheet.Range($"B14:N${fila-1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B14:N${fila-1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

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
                workSheet.Range($"D9:F9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"E8:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G8:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J8:N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        [HttpPost("importar-items/{id}")]
        public async Task<IActionResult> ImportItems(Guid id, IFormFile file)
        {
            var request = await _context.PreRequests
                .FirstOrDefaultAsync(x => x.Id == id);

            var insumos = await _context.Supplies.ToListAsync();

            var frentes = await _context.WorkFrontProjectPhases
                .Include(x => x.WorkFront)
                .Where(x => x.ProjectPhase.ProjectFormulaId == request.ProjectFormulaId)
                .ToListAsync();

            if (request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 4;
                    var items = new List<PreRequestItem>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var item = new PreRequestItem();
                        var workFrontExcel = workSheet.Cell($"B{counter}").GetString();

                        var workFront = frentes.FirstOrDefault(x => x.WorkFront.Code == workFrontExcel);

                        if (workFront == null)
                            return BadRequest("No se ha encontrado el frente de la fila " + counter);

                        var insumoExcel = workSheet.Cell($"C{counter}").GetString();

                        var insumoManual = workSheet.Cell($"E{counter}").GetString();

                        var unidadManual = workSheet.Cell($"F{counter}").GetString();

                        var insumo = insumos.FirstOrDefault(x => x.Description == insumoExcel);

                        if (string.IsNullOrEmpty(insumoManual) && string.IsNullOrEmpty(unidadManual))
                        {
                            if (string.IsNullOrEmpty(insumoExcel))
                                return BadRequest("Debe Ingresarse la descripción del artículo y su unidad en la fila " + counter);
                            else
                            {
                                insumo = insumos.FirstOrDefault(x => x.Description == insumoExcel);

                                if (insumo == null)
                                    return BadRequest("No se ha encontrado el insumo de la fila " + counter);

                                item.SupplyId = insumo.Id;
                            }
                        } else
                        {

                            item.SupplyName = insumoManual;
                            item.MeasurementUnitName = unidadManual;
                        }

                        item.WorkFrontId = workFront.WorkFrontId;
                        item.PreRequestId = id;

                        var metradoExcel = workSheet.Cell($"G{counter}").GetString();
                        double.TryParse(metradoExcel, out double metrado);
                        /*
                        if (metrado > insumo.CurrentMetered)
                            return BadRequest("Se ha superado el límite del metrado en la fila " + counter);
                        */
                        item.Measure = metrado;

                        var observacionExcel = workSheet.Cell($"H{counter}").GetString();
                        item.Observations = observacionExcel;
                        items.Add(item);
                        ++counter;
                    }
                    await _context.PreRequestItems.AddRangeAsync(items);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("excel-modelo/{id}")]
        public FileResult GetExcelSample(Guid id)
        {
            var request = _context.PreRequests
                .FirstOrDefault(x => x.Id == id);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("ITEMS");

                workSheet.Cell($"B2").Value = "Frente";

                workSheet.Range("B2:B3").Merge();

                workSheet.Cell($"C2").Value = "Catálogo de Insumos";

                workSheet.Range("C2:D2").Merge();

                workSheet.Cell($"C3").Value = "Descripción del Artículo";
                workSheet.Cell($"D3").Value = "Unidad";

                workSheet.Cell($"E2").Value = "Registro Manual";

                workSheet.Range("E2:F2").Merge();

                workSheet.Cell($"E3").Value = "Descripción del Artículo";
                workSheet.Cell($"F3").Value = "Unidad";

                workSheet.Cell($"G2").Value = "Metrado";

                workSheet.Range("G2:G3").Merge();

                workSheet.Cell($"H2").Value = "Observaciones";

                workSheet.Range("H2:H3").Merge();

                workSheet.Column(1).Width = 2;

                workSheet.Column(2).Width = 40;

                workSheet.Column(3).Width = 55;

                workSheet.Column(4).Width = 15;

                workSheet.Column(5).Width = 55;

                workSheet.Column(6).Width = 15;

                workSheet.Column(7).Width = 25;

                workSheet.Column(8).Width = 50;


                workSheet.Range("B2:H3").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:H3").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:H3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Range("B4:H4").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                var insumos = _context.Supplies
                    .Include(x => x.SupplyGroup)
                    .Include(x => x.MeasurementUnit)
                    //.Where(x => x.SupplyFamilyId == request.SupplyFamilyId)
                    .ToList();

                DataTable dtItems2 = new DataTable();
                dtItems2.TableName = "Catálogo de Insumos";
                dtItems2.Columns.Add("Insumo", typeof(string));
                dtItems2.Columns.Add("Unidad", typeof(string));
                foreach (var item in insumos)
                    dtItems2.Rows.Add(item.Description, item.MeasurementUnit.Abbreviation);
                dtItems2.AcceptChanges();

                var workSheetFamily2 = wb.Worksheets.Add(dtItems2);

                workSheetFamily2.Column(1).Width = 80;
                workSheetFamily2.Column(2).Width = 20;

                var frentes = _context.WorkFrontProjectPhases
                    .Include(x => x.ProjectPhase)
                    .Include(x => x.WorkFront)
                    .Where(x => x.ProjectPhase.ProjectFormulaId == request.ProjectFormulaId)
                    .ToList();

                DataTable dtItems3 = new DataTable();
                dtItems3.TableName = "Frentes";
                dtItems3.Columns.Add("Insumo", typeof(string));
                foreach (var item in frentes.GroupBy(x => x.WorkFrontId))
                    dtItems3.Rows.Add(item.FirstOrDefault().WorkFront.Code);
                dtItems3.AcceptChanges();

                var workSheetFamily3 = wb.Worksheets.Add(dtItems3);

                workSheetFamily3.Column(1).Width = 60;

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CargaItemsPreRequerimiento.xlsx");
                }
            }
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("degradar/{id}")]
        public async Task<IActionResult> DegradeStatus(Guid id)
        {
            var request = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);
            if (request.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.APPROVED)
            {
                request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
                var auth = await _context.PreRequestAuthorizations.Where(x => x.PreRequestId == id).ToListAsync();
                foreach (var item in auth)
                {
                    item.ApprovedDate = null;
                    item.IsApproved = false;
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest("No se puede degradar un pre-requerimiento procesado");
            }
            return Ok();
        }

        [HttpGet("evaluar/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> AskApproval(Guid id, string userId, bool isApproved)
        {
            var auth = _context.LogisticResponsibles.Where(x => x.UserId == userId);
            var request = _context.PreRequests.FirstOrDefault(x => x.Id == id);
            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);
            var authorizations = await _context.PreRequestAuthorizations.FirstOrDefaultAsync(x => x.PreRequestId == id && x.UserId == userId);
            var logAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest)
                .FirstOrDefaultAsync();
            var approveAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkPreRequest)
                .ToListAsync();
            var declineAuths = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.FailPreRequest)
                .ToListAsync();
            var secAuths = await _context.LogisticResponsibles
                .FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId &&
                x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest);

            if (authorizations.ApprovedDate != null)
            {
                return RedirectToAction("Index", "Message", new
                {
                    title = $"Error",
                    message = $"Usted ya ha aprobado o rechazado este pre-requerimiento",
                    icon = Url.Content("~/media/file_rejected.png")
                });
            }

            if (isApproved)
            {
                if(request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ANSWER_PENDING 
                    || request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY)
                {
                    authorizations.IsApproved = true;
                    authorizations.ApprovedDate = DateTime.Now;
                    if(request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY)
                    {
                        request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.APPROVED;
                        foreach (var authItem in approveAuths)
                        {
                            var mailMessage = new MailMessage
                            {
                                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                                Subject = $"Aviso de aprobación de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
                            };

                            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == authItem.UserId);
                            
                            mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

                            mailMessage.Body =
                               $"Hola, {user.RawFullName}<br /><br /> " +
                               $"Se le informa que el pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ha sido aprobado.<br />" +
                               $"<br />" +
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
                    }
                    else
                    {
                        if(secAuths == null)
                        {
                            request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.APPROVED;
                            foreach (var authItem in approveAuths)
                            {
                                var mailMessage = new MailMessage
                                {
                                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                                    Subject = $"Aviso de aprobación de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
                                };

                                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == authItem.UserId);

                                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

                                mailMessage.Body =
                                   $"Hola, {user.RawFullName}<br /><br /> " +
                                   $"Se le informa que el pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ha sido aprobado.<br />" +
                                   $"<br />" +
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
                        }
                        else
                        {
                            request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY;
                            /* Mandar correo al segundo */

                            //var linkkk = "https://localhost:44307";
                            //var linkAuth = $"{linkkk}/logistica/pre-requerimientos/evaluar/{id}";
                            var linkAuth = $"{ConstantHelpers.SystemUrl.Url}logistica/pre-requerimientos/evaluar/{id}";
                            await _context.SaveChangesAsync();
                            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Arian"));

                            var mailMessage = new MailMessage
                            {
                                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                                Subject = $"Aviso de emisión de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
                            };
                            List<Attachment> archivos = Files(id);
                           
                            if (archivos != null)
                                foreach (var item in archivos)
                                    mailMessage.Attachments.Add(item);

                            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == secAuths.UserId);
                            var linkResponsible1 = $"{linkAuth}?userId={user.Id}";
                            var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                            var linkReject1 = $"{linkResponsible1}&isApproved=false";

                            mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

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
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Message", new
                    {
                        title = $"Pre-Requerimiento {(isApproved ? "Aprobado" : "Rechazado")}",
                        message = $"El pre-requerimiento con código {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ha sido {(isApproved ? "aprobado" : "rechazado")}.",
                        icon = isApproved ? Url.Content("~/media/file_approved.png") : Url.Content("~/media/file_rejected.png")
                    });
                }
                else
                {
                    authorizations.ApprovedDate = null;
                    authorizations.IsApproved = false;
                    return RedirectToAction("Index", "Message", new
                    {
                        title = $"Error",
                        message = $"El pre-requerimiento con código {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ya ha sido observado.",
                        icon = Url.Content("~/media/file_rejected.png")
                    });
                }
            }

            if (!isApproved)
            {
                if(request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
                {
                    // el otro tambien tiene que estar en null
                    var author2 = _context.PreRequestAuthorizations.FirstOrDefault(x => x.PreRequestId == id && x.UserId != userId);
                    author2.ApprovedDate = null;
                    author2.IsApproved = false;
                    authorizations.ApprovedDate = null;
                    return RedirectToAction("Index", "Message", new
                    {
                        title = $"Error",
                        message = $"El pre-requerimiento con código {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ya ha sido observado.",
                        icon = Url.Content("~/media/file_rejected.png")
                    });
                }
                foreach (var authItem in declineAuths)
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                        Subject = $"Aviso de observación de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
                    };

                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == authItem.UserId);

                    mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

                    mailMessage.Body =
                        $"Hola, {user.RawFullName}<br /><br /> " +
                        $"Se le informa que el pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ha sido observado.<br />" +
                        $"<br />" +
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
                authorizations.ApprovedDate = DateTime.Now;
                return RedirectToAction("PreRequestObs", "Message", new
                {
                    title = $"Pre-Requerimiento {(isApproved ? "Aprobado" : "Rechazado")}",
                    message = $"El pre-requerimiento con código {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")} para el proyecto {project.Abbreviation} ha sido {(isApproved ? "aprobado" : "rechazado")}.",
                    icon = isApproved ? Url.Content("~/media/file_approved.png") : Url.Content("~/media/file_rejected.png"),
                    id = id.ToString()
                });
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("enviar-observacion/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> SendObs(Guid id, string observation)
        {
            var request = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);
            request.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.OBSERVED;
            request.Observations = observation;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("adjuntar/{id}")]
        public async Task<IActionResult> UpdateObservations(Guid id, PreRequestFileViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var prerequest = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);

            if (prerequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.OBSERVED)
            {
                prerequest.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED;
            }

            if (model.PreRequestFiles != null)
            {
                var newFiles = new List<PreRequestFile>();
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var file in model.PreRequestFiles)
                {
                    newFiles.Add(new PreRequestFile
                    {
                        PreRequestId = id,
                        FileUrl = await storage.UploadFile(file.OpenReadStream(),
                                    ConstantHelpers.Storage.Containers.LOGISTICS,
                                    Path.GetExtension(file.FileName),
                                    ConstantHelpers.Storage.Blobs.PREREQUEST_FILES,
                                    $"{prerequest.CorrelativePrefix}-{prerequest.CorrelativeCode}-{Path.GetFileNameWithoutExtension(file.FileName)}")
                    });
                }
                await _context.PreRequestFiles.AddRangeAsync(newFiles);
            }

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

                var reqUserAux = _context.Users.FirstOrDefault(x => x.Id == request.IssuedUserId);

                workSheet.Cell($"D9").Value = reqUserAux.FullName;
                workSheet.Range($"D9:F9").Merge();
                workSheet.Cell($"D9").Style.Alignment.WrapText = true;

                if (reqUserAux.SignatureUrl != null)
                {
                    
                    var enl = reqUserAux.SignatureUrl.ToString();

                    Stream imgSig = client.OpenRead(enl);
                    Bitmap bitmapSig = new Bitmap(imgSig);

                    var auxSig = workSheet.AddPicture(bitmapSig).MoveTo(workSheet.Cell("F9")).WithSize(80, 40);
                    auxSig.MoveTo(395, 225);
                    auxSig.Height = 40;
                    auxSig.Width = 80;
                }

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

                if(request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY ||
                   request.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ISSUED)
                {
                    var auths = _context.PreRequestAuthorizations.Where(x => x.PreRequestId == id && x.ApprovedDate != null).ToList();
                    var responsible1 = auths.First();
                    var responsible2 = auths.Last();

                    var users = _context.Users.Where(x => x.Id == responsible1.UserId || x.Id == responsible2.UserId).ToList();
                    
                    workSheet.Cell($"D10").Value = users.First().FullName;
                    var res1Date = auths.FirstOrDefault(x => x.UserId == responsible1.UserId).ApprovedDate;
                    workSheet.Cell($"K10").SetValue(res1Date.HasValue ? res1Date.Value.ToString("dd/MM/yyyy") : "");

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

                    if (responsible1.Id != responsible2.Id)
                    {
                        workSheet.Cell($"D11").Value = users.Last().FullName;
                        var res2Date = auths.FirstOrDefault(x => x.UserId == responsible2.UserId).ApprovedDate;
                        workSheet.Cell($"K11").SetValue(res2Date.HasValue ? res2Date.Value.ToString("dd/MM/yyyy") : "");

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
                    }
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

                workSheet.Range($"B14:N${fila-1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B14:N${fila-1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

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
                workSheet.Range($"D9:F9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
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

        [HttpGet("exportar")]
        public async Task<IActionResult> Export()
        {

            var reqItems = _context.PreRequestItems
                .Include(x => x.PreRequest)
                .Include(x => x.WorkFront)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.PreRequest.ProjectId == GetProjectId()
                && (x.PreRequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED || 
                x.PreRequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY ||
                x.PreRequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ISSUED))
                .ToList();

            var dt = new DataTable("LISTADO DE PREREQUERIMIENTOS");
            dt.Columns.Add("CENTRO DE COSTO", typeof(string));
            dt.Columns.Add("PROYECTO", typeof(string));
            dt.Columns.Add("COD. PRE-REQUERIMIENTO", typeof(string));
            dt.Columns.Add("ESTADO", typeof(string));
            dt.Columns.Add("ESTADO ATENCIÓN", typeof(string));
            dt.Columns.Add("SOLICITA", typeof(string));
            dt.Columns.Add("TIPO SOLICITUD", typeof(string));
            dt.Columns.Add("FECHA EMISIÓN", typeof(DateTime));
            dt.Columns.Add("FECHA ENTREGA", typeof(DateTime));
            dt.Columns.Add("GRUPO", typeof(string));
            dt.Columns.Add("MATERIAL", typeof(string));
            dt.Columns.Add("UND", typeof(string));
            dt.Columns.Add("METRADO", typeof(string));

            dt.Columns.Add("ATENCIÓN ACUMULADA", typeof(string));
            dt.Columns.Add("POR ATENDER", typeof(string));
            dt.Columns.Add("ESTADO ATENCIÓN DETALLADO", typeof(string));

            dt.Columns.Add("PARA SER USADO EN", typeof(string));
            dt.Columns.Add("OBSERVACIONES", typeof(string));



            reqItems.ForEach(item =>
            {
                var req = _context.PreRequests.Include(x => x.Project).FirstOrDefault(x => x.Id == item.PreRequestId);

                var status = "";
                var attentionStatus = "";
                switch (item.PreRequest.OrderStatus)
                {
                    case ConstantHelpers.Logistics.PreRequest.Status.PRE_ISSUED:
                        status = "Pre-Emitido";
                        break;
                    case ConstantHelpers.Logistics.PreRequest.Status.ISSUED:
                        status = "Emitido";
                        break;
                    case ConstantHelpers.Logistics.PreRequest.Status.APPROVED:
                        status = "Aprobado";
                        break;
                    case ConstantHelpers.Logistics.PreRequest.Status.CANCELED:
                        status = "Anulado";
                        break;
                    case ConstantHelpers.Logistics.PreRequest.Status.OBSERVED:
                        status = "Observado";
                        break;
                    case ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY:
                        status = "Aprobado Parcialmente";
                        break;
                    case ConstantHelpers.Logistics.PreRequest.Status.ANSWER_PENDING:
                        status = "Pendiente de Respuesta";
                        break;
                }

                switch (item.PreRequest.AttentionStatus)
                {
                    case ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL:
                        attentionStatus = "Parcialmente Atendido";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING:
                        attentionStatus = "Pendiente";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL:
                        attentionStatus = "Totalmente Atendido";
                        break;
                }
                var type = "";
                switch (req.RequestType)
                {
                    case ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE:
                        type = "Compra";
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.Type.SERVICE:
                        type = "Servicio";
                        break;
                }

                var estado = "";
                if(item.MeasureInAttention == 0)
                {
                    estado = "Por atender";
                }else if (item.MeasureInAttention < item.Measure){
                    estado = "Atendido Parcialmente";
                }
                else if (item.MeasureInAttention == item.Measure){
                    estado = "Atendido totalmente";
                }

                var code = $"{req.CorrelativePrefix}{req.CorrelativeCode.ToString("D4")}";
                var username = _context.Users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;
                dt.Rows.Add(req.Project.CostCenter, req.Project.Abbreviation, code, status, attentionStatus,
                    username, type, req.IssueDate, req.DeliveryDate, item.Supply != null ? item.Supply.SupplyGroup.Name : item.SupplyName, item.Supply != null ? item.Supply.Description: item.SupplyName,
                    item.Supply != null ? item.Supply.MeasurementUnit.Abbreviation : item.SupplyName, item.Measure, item.MeasureInAttention, item.Measure - item.MeasureInAttention, estado, item.WorkFront.Code, item.Observations);
            });
            var project = _context.Projects.FirstOrDefault(x => x.Id == GetProjectId()).Abbreviation;
            var fileName = $"Listado de PreRequerimientos {project}.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 7;
                workSheet.Column(2).Width = 46;
                workSheet.Column(3).Width = 12;
                workSheet.Column(4).Width = 15;
                workSheet.Column(5).Width = 23;
                workSheet.Column(6).Width = 32;
                workSheet.Column(7).Width = 10;
                workSheet.Column(8).Width = 10;
                workSheet.Column(9).Width = 10;
                workSheet.Column(10).Width = 10;
                workSheet.Column(11).Width = 40;
                workSheet.Column(12).Width = 5;
                workSheet.Column(13).Width = 6;

                workSheet.Column(14).Width = 23;
                workSheet.Column(15).Width = 19;
                workSheet.Column(16).Width = 25;

                workSheet.Column(17).Width = 37;
                workSheet.Column(18).Width = 30;

                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpGet("enviar-recordatorio/{id}")]
        public async Task<IActionResult> SendReminder(Guid id)
        {
            var request = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == id);

            if(request == null)
            {
                return BadRequest("El prerequerimiento no existe");
            }
            if (request.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.ANSWER_PENDING && 
                request.OrderStatus != ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY)
            {
                return BadRequest("Solo se puede enviar recordatorios a prerequerimientos con estados 'Pendiente de respuesta' o 'Aprobado parcialmente'");
            }

            var logAuths = await _context.PreRequestAuthorizations
                .FirstOrDefaultAsync(x => x.PreRequestId == id && x.IsApproved == false
                && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest);
            var logSecAuth = await _context.PreRequestAuthorizations
                .FirstOrDefaultAsync(x => x.PreRequestId == id && x.IsApproved == false
                && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest);

            if (logAuths == null && logSecAuth == null)
            {
                return BadRequest("El prerequerimiento no tiene una respuesta pendiente");
            }

            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);

            //var linkkk = "https://localhost:44307";
            //var linkAuth = $"{linkkk}/logistica/pre-requerimientos/evaluar/{id}";
            var linkAuth = $"{ConstantHelpers.SystemUrl.Url}logistica/pre-requerimientos/evaluar/{id}";

            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Arian"));
            if(logAuths != null) { // El autorizante principal aún no ha respondido el correo
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                    Subject = $"Aviso de emisión de Pre-requerimiento {request.CorrelativePrefix}{request.CorrelativeCode.ToString("D4")}"
                };
                List<Attachment> archivos = Files(id);
                if (archivos != null)
                    foreach (var item in archivos)
                        mailMessage.Attachments.Add(item);

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == logAuths.UserId);
                var linkResponsible1 = $"{linkAuth}?userId={user.Id}";
                var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                var linkReject1 = $"{linkResponsible1}&isApproved=false";

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

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
            else if(logAuths == null && logSecAuth != null) // El primer autorizante ya respondió pero el secundario, no
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

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == logSecAuth.UserId);
                var linkResponsible1 = $"{linkAuth}?userId={user.Id}";
                var linkApprove1 = $"{linkResponsible1}&isApproved=true";
                var linkReject1 = $"{linkResponsible1}&isApproved=false";

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));

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

            return Ok();
        }

    }
}
