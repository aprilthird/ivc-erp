using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using DocumentFormat.OpenXml.Office.CustomUI;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
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
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
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

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/requerimientos")]
    public class RequestController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public RequestController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            IConfiguration configuration) : base(context, userManager, configuration)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("{id}/detalles")]
        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _context.Requests.FindAsync(id);
            ViewBag.RequestId = request.Id;
            ViewBag.RequestCode = request.CorrelativeCode;
            return View();
        }

        [HttpGet("listado")]
        public IActionResult List() => View();

        [HttpGet("atención")]
        public IActionResult Attention() => View();

        [HttpGet("ordenes-compra")]
        public IActionResult Order() => View();


        [HttpGet("ordenes-servicio")]
        public IActionResult OrderService() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var summary = await _context.RequestSummaries
                .FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());

            var query = await _context.Requests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectId == GetProjectId()
                && (x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED)
                .AsNoTracking()
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
                    },
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code + "-" + x.ProjectFormula.Name
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                    //SupplyFamilyId = (Guid)x.SupplyFamilyId,
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = x.SupplyFamily.Name
                    //},
                    ProjectFormulaId = (Guid)x.ProjectFormulaId,
                    IssuedUserId = x.IssuedUserId
                }).ToListAsync();

            var users = await _context.Users.ToListAsync();

            //var reqUsers = await _context.RequestUsers.Include(x => x.User).ToListAsync();

            var reqItems = await _context.RequestItems
                .Include(x => x.PreRequestItem)
                .Include(x => x.PreRequestItem.PreRequest)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .ToListAsync();

            foreach (var req in query)
            {
                req.RequestUsernames = users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;

                req.PreRequestNames = string.Join(" / ",
                    reqItems.Where(x => x.PreRequestItemId != null && x.RequestId == req.Id)
                    .GroupBy(x=>x.PreRequestItem.PreRequestId)
                    .AsEnumerable()
                    .Select(x => x.FirstOrDefault().PreRequestItem.PreRequest.CorrelativePrefix
                    + "-" + x.FirstOrDefault().PreRequestItem.PreRequest.CorrelativeCode.ToString("D4")).ToList());

                req.GroupNames = string.Join(" / ",
                    reqItems.Where(x => x.RequestId == req.Id)
                    .GroupBy(x => x.Supply.SupplyGroupId)
                    .AsEnumerable()
                    .Select(x => x.FirstOrDefault().Supply.SupplyGroup.Name).ToList());
            }

            return Ok(query);
        }

        [HttpGet("listado/listar")]
        public async Task<IActionResult> GetAllListed(int status = 0, int type = 0, string user = null, Guid? budgetTitleId = null, Guid? supplyGroupId = null,
            int attentionStatus = 0)
        {
            var oficinaTecnica = await _context.UserRoles
                .Include(x => x.Role)
                .Where(x => x.Role.Name == "Oficina Técnica" || x.Role.Name == "Superadmin")
                .Select(x => x.UserId)
                .ToListAsync();

            var usuario = GetUserId();
            
            var query = _context.Requests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
                //.Include(x => x.SupplyFamily)
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectId == GetProjectId() 
                && (x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ISSUED
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ANSWER_PENDING) &&
                x.AttentionStatus == ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING
                && (x.IssuedUserId == usuario || oficinaTecnica.Contains(usuario)));

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);

            var aux = query
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
                    },
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Code = x.ProjectFormula.Code + "-" + x.ProjectFormula.Name
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    AttentionStatus = x.AttentionStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                    IssuedUserId = x.IssuedUserId,
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                        //Name = x.SupplyFamily.Name
                    //},
                }).ToList();

            var users = await _context.Users.ToListAsync();
            //var reqUsers = await _context.RequestUsers.Include(x => x.User).ToListAsync();

            var reqItems = await _context.RequestItems
               .Include(x => x.PreRequestItem)
               .Include(x => x.PreRequestItem.PreRequest)
               .Include(x => x.Supply)
               .Include(x => x.Supply.SupplyGroup)
               .ToListAsync();

            var data = new List<RequestViewModel>();

            foreach (var req in aux)
            {
                if (status != 0 && req.OrderStatus != status)
                {
                    continue;
                }

                if (attentionStatus != 0 && req.AttentionStatus != attentionStatus)
                {
                    continue;
                }

                if (type != 0 && req.RequestType != type)
                {
                    continue;
                }

                if (user != "Todos" && req.IssuedUserId == user)
                {
                    continue;
                }

                if (supplyGroupId.HasValue && !reqItems.Where(x => x.RequestId == req.Id).Select(x => x.Supply.SupplyGroupId).Contains(supplyGroupId))
                {
                    continue;
                }
                
                req.RequestUsernames = users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;
                
                req.PreRequestNames = string.Join(" / ",
                   reqItems.Where(x => x.PreRequestItemId != null && x.RequestId == req.Id)
                   .GroupBy(x => x.PreRequestItem.PreRequestId)
                   .AsEnumerable()
                   .Select(x => x.FirstOrDefault().PreRequestItem.PreRequest.CorrelativePrefix
                   + "-" + x.FirstOrDefault().PreRequestItem.PreRequest.CorrelativeCode.ToString("D4")).ToList());

                req.GroupNames = string.Join(" / ",
                    reqItems.Where(x => x.RequestId == req.Id)
                    .GroupBy(x => x.Supply.SupplyGroupId)
                    .AsEnumerable()
                    .Select(x => x.FirstOrDefault().Supply.SupplyGroup.Name).ToList());
               
                var reqsAux = reqItems.Where(x => x.RequestId == req.Id)
                   .Select(y => y.PreRequestItemId).ToList();

                var prerequests = _context.PreRequestItems
                   .Include(x => x.PreRequest.ProjectFormula)
                   .Include(x => x.PreRequest)
                   .Where(x => reqsAux.Contains(x.Id))
                   .ToList();

                if(prerequests.Count() > 0)
                {
                    var projectFormulas = prerequests
                        .Select(x => x.PreRequest.ProjectFormula)
                        .Distinct().ToList();
                    var mergeFormula = new List<string>();
                    foreach (var form in projectFormulas)
                    {
                        var auxForm = form.Code + "-" + form.Name;
                        mergeFormula.Add(auxForm);
                    }
                    mergeFormula = mergeFormula.Distinct().ToList();
                    req.ProjectFormula.Code = string.Join("/", mergeFormula);

                    var authsNames = new List<string>();
                    foreach (var pr in prerequests)
                    {
                        var usu = users
                            .FirstOrDefault(x => x.Id == pr.PreRequest.IssuedUserId);
                        authsNames.Add(usu.FullName);
                    }
                    authsNames = authsNames.Distinct().ToList();

                    req.RequestUsernames = string.Join("/", authsNames);
                }

                data.Add(req);
            }

            return Ok(data);
        }

        [HttpGet("atencion/listar")]
        public async Task<IActionResult> GetAllAttented()
        {
            var summary = await _context.RequestSummaries
                .FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());

            var query = await _context.Requests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
                .Where(x => x.ProjectId == GetProjectId() && x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED)
                .AsNoTracking()
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                }).ToListAsync();

            //var reqUsers = await _context.RequestUsers.Include(x => x.User).ToListAsync();
            var users = await _context.Users.ToListAsync();

            foreach (var req in query)
            {
                req.RequestUsernames = users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;
            }

            return Ok(query);
        }

        [HttpGet("ordenes/listar")]
        public async Task<IActionResult> GetAllActived(int status = 0, int type = 0, string user = null, Guid? budgetTitleId = null)
        {
            var query = _context.Requests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
                .Where(x => x.ProjectId == GetProjectId() && (x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C
                || x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S)
                && x.AttentionStatus != ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING);

            if (budgetTitleId.HasValue)
                query = query.Where(x => x.BudgetTitleId == budgetTitleId);

            var aux = query
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    Project = new ProjectViewModel
                    {
                        CostCenter = x.Project.CostCenter,
                        Abbreviation = x.Project.Abbreviation
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                    IssuedUserId = x.IssuedUserId,
                    AttentionStatus = x.AttentionStatus
                }).ToList();

            //var reqUsers = await _context.RequestUsers.Include(x => x.User).ToListAsync();
            var users = await _context.Users.ToListAsync();

            var reqItems = await _context.RequestItems
               .Include(x => x.PreRequestItem)
               .Include(x => x.PreRequestItem.PreRequest)
               .Include(x => x.Supply)
               .Include(x => x.Supply.SupplyGroup)
               .ToListAsync();

            var data = new List<RequestViewModel>();

            foreach (var req in aux)
            {
                if (status != 0 && req.OrderStatus != status)
                {
                    continue;
                }

                if (req.RequestType != type)
                {
                    continue;
                }

                if (user != "Todos" && req.IssuedUserId == user)
                {
                    continue;
                }

                req.RequestUsernames = users.FirstOrDefault(x => x.Id == req.IssuedUserId).FullName;

                req.PreRequestNames = string.Join(" / ",
                   reqItems.Where(x => x.PreRequestItemId != null && x.RequestId == req.Id)
                   .GroupBy(x => x.PreRequestItem.PreRequestId)
                   .AsEnumerable()
                   .Select(x => x.FirstOrDefault().PreRequestItem.PreRequest.CorrelativePrefix
                   + "-" + x.FirstOrDefault().PreRequestItem.PreRequest.CorrelativeCode.ToString("D4")).ToList());

                data.Add(req);
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var hasFiles = await _context.RequestFiles
                .Where(x => x.RequestId == id)
                .CountAsync() > 0;

            var data = await _context.Requests
                .Where(x => x.Id == id)
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    RequestType = x.RequestType,
                    BudgetTitleId = x.BudgetTitleId,
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    //RequestUserIds = x.RequestUsers.Select(i => i.UserId).ToList(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    WarehouseId = x.WarehouseId,
                    Observations = x.Observations,
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    TechnicalInformation = x.TechnicalInformation,
                    CalibrationCertificate = x.CalibrationCertificate,
                    Catalog = x.Catalog,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    //SupplyFamilyId = (Guid)x.SupplyFamilyId,
                    ProjectFormulaId = (Guid)x.ProjectFormulaId,
                    HasFiles = hasFiles
                }).AsNoTracking()
                .FirstOrDefaultAsync();

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
        public async Task<IActionResult> Create(RequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //int compare = DateTime.Compare(model.DeliveryDate.ToDateTime(), DateTime.Today);

            //if (compare < 0)
              //  return BadRequest("La fecha de entrega no puede ser anterior al día hoy");

            var correlative = await _context.RequestSummaries
                .Where(x => x.ProjectId == GetProjectId())
                .FirstOrDefaultAsync();
            correlative.TotalOfRequest++;
            var newReq = _context.Requests
                .Where(x => x.ProjectId == GetProjectId() && x.CorrelativeCode < 1000)
                .ToList();
            int newCorrelative;
            if (newReq.Count() > 0)
            {
                newCorrelative = newReq
                    .Select(x => x.CorrelativeCode).Max() + 1;
            }
            else
            {
                newCorrelative = 1;
            }

            var request = new Request
            {
                ProjectId = GetProjectId(),
                CorrelativeCode = newCorrelative,
                CorrelativePrefix = correlative.CodePrefix,
                BudgetTitleId = model.BudgetTitleId,
                RequestType = model.RequestType,
                IssueDate = DateTime.Today,
                DeliveryDate = model.DeliveryDate.ToDateTime(),
                WarehouseId = model.WarehouseId,
                Observations = string.Empty,
                ProjectFormulaId = model.ProjectFormulaId,
                //SupplyFamilyId = model.SupplyFamilyId,
                IssuedUserId = GetUserId()
            };

            //Lógica para carga de Archivos

            //await _context.RequestUsers.AddRangeAsync(
            //    model.RequestUserIds.Select(x => new RequestUser
            //    {
            //        Request = request,
            //        UserId = x
            //    }).ToList());
            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();

            return Ok(request.Id);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("crear-existente")]
        public async Task<IActionResult> CreateExisting(ExistingRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.WarehouseId == Guid.Empty)
                return BadRequest("No se ha ingresado el almacén");

            var correlative = await _context.RequestSummaries
                .Where(x => x.ProjectId == GetProjectId())
                .FirstOrDefaultAsync();
            correlative.TotalOfRequest++;

            var supplies = await _context.Supplies
                .ToListAsync();

            var preRequest = await _context.PreRequests
                .FirstOrDefaultAsync(x => x.Id == model.PreRequestIds.FirstOrDefault());

            var newUsers = new List<RequestUser>();

            var newItems = new List<RequestItem>();
            var newReq = _context.Requests
               .Where(x => x.ProjectId == GetProjectId() && x.CorrelativeCode < 1000)
               .ToList();
            int newCorrelative;
            if (newReq.Count() > 0)
            {
                newCorrelative = newReq
                    .Select(x => x.CorrelativeCode).Max() + 1;
            }
            else
            {
                newCorrelative = 1;
            }

            var request = new Request
            {
                Id = Guid.NewGuid(),
                ProjectId = GetProjectId(),
                CorrelativeCode = newCorrelative,
                CorrelativePrefix = correlative.CodePrefix,
                BudgetTitleId = preRequest.BudgetTitleId,
                RequestType = preRequest.RequestType,
                IssueDate = DateTime.Today,
                DeliveryDate = preRequest.DeliveryDate,
                WarehouseId = model.WarehouseId,
                Observations = string.Empty,
                ProjectFormulaId = preRequest.ProjectFormulaId,
                //SupplyFamilyId = preRequest.SupplyFamilyId,
                IssuedUserId = GetUserId()
            };

            foreach (var stringItem in model.Items)
            {
                var stringSplit = stringItem.Split(",");
                Guid reqId = Guid.Parse(stringSplit[0]);
                var reqItem = await _context.PreRequestItems
                    .Include(x => x.Supply)
                    .Include(x => x.WorkFront)
                    .Include(x => x.PreRequest)
                    .FirstOrDefaultAsync(x => x.Id == reqId);
                var cant = stringSplit[1].ToDoubleString();
                reqItem.MeasureInAttention += cant;

                var insumo = supplies.FirstOrDefault(x => x.Id == reqItem.SupplyId);

                if (reqItem.Supply == null)
                    return BadRequest("No se puede crear con un ingreso manual");

                //if (meta == null)
                //    return BadRequest("No se han encontrado el item en insumos meta " + reqItem.Supply.Description + " en el frente " + reqItem.WorkFront.Code);

                //if (cant > reqItem.Measure || reqItem.MeasureInAttention > reqItem.Measure)
                //    return BadRequest("Se ha superado el límite de metrado para atender");

                if (reqItem.Observations == null) reqItem.Observations = "";
                if (cant > 0)
                    newItems.Add(new RequestItem
                    {
                        RequestId = request.Id,
                        PreRequestItemId = reqItem.Id,
                        Measure = cant,
                        SupplyId = insumo.Id,
                        WorkFrontId = reqItem.WorkFrontId,
                        Observations = reqItem.Observations
                    });
            }

            foreach (var Id in model.PreRequestIds)
            {
                var aux = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == Id);

                var completo = true;

                var preReqItems = await _context.PreRequestItems.Where(x => x.PreRequestId == aux.Id).ToListAsync();

                foreach(var preItem in preReqItems)
                {
                    if (preItem.Measure != preItem.MeasureInAttention)
                        completo = false;
                }

                if (completo == true)
                    aux.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                else
                    aux.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;

                //var usersPre = await _context.PreRequestUsers.Where(x => x.PreRequestId == Id).ToListAsync();

                //foreach (var user in usersPre)
                //{
                //    newUsers.Add(new RequestUser
                //    {
                //        UserId = user.UserId,
                //        RequestId = request.Id
                //    });
                //}
            }


            await _context.Requests.AddAsync(request);
            //await _context.RequestUsers.AddRangeAsync(newUsers);
            await _context.RequestItems.AddRangeAsync(newItems);
            await _context.SaveChangesAsync();

            return Ok(request.Id);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, RequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = await _context.Requests
                .FirstOrDefaultAsync(x => x.Id == id);

            if (request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED)
                return BadRequest("El Requerimiento está cancelado");

            if (request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
            {
                request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED;
            }

            request.RequestType = model.RequestType;
            request.BudgetTitleId = model.BudgetTitleId;
            request.DeliveryDate = model.DeliveryDate.ToDateTime();
            request.WarehouseId = model.WarehouseId;
            //request.SupplyFamilyId = model.SupplyFamilyId;
            request.ProjectFormulaId = model.ProjectFormulaId;

            /*
            if (request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
                || request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED
                || request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.ISSUED;
            */
                //Lógica de Archivos

            //    var users = await _context.RequestUsers
            //    .Where(x => x.RequestId == id).ToListAsync();

            //_context.RequestUsers.RemoveRange(users);
            //await _context.RequestUsers.AddRangeAsync(
            //    model.RequestUserIds.Select(x => new RequestUser { 
            //        RequestId = id,
            //        UserId = x
            //    }).ToList());

            await _context.SaveChangesAsync();

            return Ok(id);
        }

        [HttpPut("emitir/{id}")]
        public async Task<IActionResult> UpdateStatusIssued(Guid id)
        {
            var request = await _context.Requests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .FirstOrDefaultAsync(x => x.Id == id);

            var grupos = await _context.RequestItems
                .Where(x => x.RequestId == id)
                .Include(x => x.Supply.SupplyGroup)
                .Select(x => x.Supply.SupplyGroup.Name)
                .Distinct()
                .ToListAsync();

            var logRes = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId)
                .ToListAsync();
            
            request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.ISSUED;
            request.ReviewDate = DateTime.Today;

            var logExiste = await _context.RequestAuthorizations.Where(x => x.RequestId == id
            && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewRequest).ToListAsync();

            _context.RemoveRange(logExiste);

            var reqAuths = new List<RequestAuthorization>();
           
            foreach (var review in logRes.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewRequest || 
                                                     x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest ||
                                                     x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest ||
                                                     x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.FailRequest)) 
            {
                reqAuths.Add(new RequestAuthorization
                {
                    RequestId = (Guid)request.Id,
                    UserId = review.UserId,
                    UserType = review.UserType,
                    IsApproved = false
                });
            }
 
            List<Attachment> archivos = Files(id, 0);

            var code = request.CorrelativePrefix + "-" + request.CorrelativeCode.ToString("D4");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{request.CorrelativePrefix} - Aviso de emisión de Requerimiento {code}"
            };

            if (archivos != null)
                foreach (var item in archivos)
                    mailMessage.Attachments.Add(item);

            foreach (var auth in logRes.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }

            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Henrry"));
            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Se ha emitido el Requerimiento {code} en el centro de costo {request.Project.Abbreviation},<br />" +
                $"<br />" +
                //$"Familia {request.SupplyFamily.Name}<br />" +
                $"Grupos {string.Join(", ", grupos)} <br />" +
                $"<br />" +
                $"Se solicita de su Revisión, Observación u Aprobación. <br /><br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
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

            await _context.RequestAuthorizations.AddRangeAsync(reqAuths);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("degradar/{id}")]
        public async Task<IActionResult> UpdateStatusDowngrade(Guid id)
        {
            var request = await _context.Requests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .FirstOrDefaultAsync(x => x.Id == id);

            var grupos = await _context.RequestItems
                .Where(x => x.RequestId == id)
                .Include(x => x.Supply.SupplyGroup)
                .Select(x => x.Supply.SupplyGroup.Name)
                .Distinct()
                .ToListAsync();

            var logRes = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == request.ProjectId)
                .ToListAsync();

            var logExiste = await _context.RequestAuthorizations.Where(x => x.RequestId == id).ToListAsync();

            _context.RemoveRange(logExiste);

            var logAuths = logRes
                .Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest)
                .ToList();

            string userId = GetUserId();
            
            if (logAuths.Select(x => x.UserId).Contains(userId) == false)
                return BadRequest("Usted no tiene los permisos para degradar esta orden de compra");
            
            request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED;

            List<Attachment> archivos = Files(id, 0);

            var code = request.CorrelativePrefix + "-" + request.CorrelativeCode.ToString("D4");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{request.CorrelativePrefix} - Aviso de emisión de Requerimiento {code}"
            };

            if (archivos != null)
                foreach (var item in archivos)
                    mailMessage.Attachments.Add(item);

            foreach (var auth in logRes.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.FailRequest))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }

            //mailMessage.To.Add(new MailAddress("henrrypaul_22@hotmail.com", "Henrry"));
            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Se ha degradado el Requerimiento {code} en el centro de costo {request.Project.Abbreviation},<br />" +
                $"<br />" +
                //$"Familia {request.SupplyFamily.Name}<br />" +
                $"Grupos {string.Join(", ", grupos)} <br />" +
                $"<br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
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

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> UpdateStatusCanceled(Guid id)
        {
            var request = await _context.Requests
                .Include(x => x.Project)
                //.Include(x => x.SupplyFamily)
                .FirstOrDefaultAsync(x => x.Id == id);

            var preReq = await _context.RequestItems
                .Include(x => x.PreRequestItem)
                .Where(x => x.RequestId == id)
                .ToListAsync();

            foreach (var item in preReq)
            {
                var flag = true;
                var preReqItem = await _context.PreRequestItems.FirstOrDefaultAsync(x => x.Id == item.PreRequestItemId);
                var req = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == preReqItem.PreRequestId);

                var auxCont = _context.RequestItems
                    .Where(x => x.RequestId != id
                    && x.PreRequestItemId != null)
                    .Select(x => x.PreRequestItemId).ToList();
                var cont = _context.PreRequestItems.Where(x => x.PreRequestId == req.Id).Select(x => x.Id).ToList();
                foreach (var contItem in cont)
                {
                    if (auxCont.Contains(contItem))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    req.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.APPROVED;
                    req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                }

                item.PreRequestItem.MeasureInAttention -= item.Measure;

                item.PreRequestItemId = null;
            }

            request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.CANCELED;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("aprobar/{id}")]
        public async Task<IActionResult> UpdateStatusApproved(Guid id)
        {
            var request = await _context.Requests
                .FirstOrDefaultAsync(x => x.Id == id);

            request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
            request.ApproveDate = DateTime.Today;

            var logAuths = _context.RequestAuthorizations.Where(x => x.RequestId == id);
            var items = await _context.RequestItems
                .Where(x => x.RequestId == id)
                .Include(x => x.Supply.SupplyGroup).ToListAsync();

            var grupos = items
                .Select(x => x.Supply.SupplyGroup.Name)
                .Distinct()
                .ToList();

            string userId = GetUserId();

            if (logAuths.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest)
                .Select(x => x.UserId).Contains(userId) == false)
                return BadRequest("Usted no tiene los permisos para aprobar esta orden de compra");

            var prereq = _context.PreRequestItems
                    .Where(x => items.Select(y => y.PreRequestItemId)
                    .Contains(x.Id)).Select(x => x.PreRequestId);
            var auths = _context.PreRequestAuthorizations.ToList();
            var prerequests = _context.PreRequests
              .Where(x => prereq.Contains(x.Id));

            var deliveryDate = request.DeliveryDate.Value;  
            var issDate = request.IssueDate.Value;
            var maxDate = request.ApproveDate;

            if (prerequests.Count() > 0)
            {
                deliveryDate = prerequests.Select(x => x.DeliveryDate).Max().Value;
                issDate = prerequests.Select(x => x.IssueDate).Max().Value;
                maxDate = auths.Where(x => 
                    prerequests.Select(y => y.Id).Contains(x.PreRequestId))
                    .Select(x => x.ApprovedDate)
                    .Max().Value;
            }

            var dateDif = deliveryDate - issDate;
            request.DeliveryDate = maxDate.Value.Add(dateDif.Duration());

            var prereqData = "";
            var datePrereq = "";
            foreach(var pr in prerequests)
            {
                var authDate = auths.Where(x => x.PreRequestId == pr.Id).OrderByDescending(x => x.ApprovedDate).FirstOrDefault().ApprovedDate;
                if (authDate != null)
                    datePrereq = authDate.Value.ToDateString();
                else
                    datePrereq = "---";
                prereqData += $"• {pr.CorrelativePrefix}{pr.CorrelativeCode.ToString("D4")} - Fecha de aprobación: {datePrereq} <br />";
            }

            logAuths.FirstOrDefault(x => x.UserId == userId
            && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).IsApproved = true;
            logAuths.FirstOrDefault(x => x.UserId == userId
            && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).ApprovedDate = DateTime.Now;

            

            //_context.RemoveRange(logExiste);

            //var reqAuths = new List<RequestAuthorization>();

            //foreach (var auth in logAuths)
            //{
            //    reqAuths.Add(new RequestAuthorization
            //    {fla
            //        RequestId = request.Id,
            //        UserId = auth.UserId,
            //        UserType = auth.UserType,
            //        IsApproved = true
            //    });
            //}

            List<Attachment> archivos = Files(id, 1);

            var code = request.CorrelativePrefix + "-" + request.CorrelativeCode.ToString("D4");

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{request.CorrelativePrefix} - Aviso de aprobación de Requerimiento {code}"
            };

            if (archivos != null)
                foreach (var item in archivos)
                    mailMessage.Attachments.Add(item);
            
            foreach (var auth in logAuths.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);

                mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            }
            
            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Henrry"));
            mailMessage.Body =
                $"Hola, <br /><br /> " +
                $"Se ha aprobado el Requerimiento {code} en el centro de costo {request.Project.Abbreviation},<br />" +
                $"<br />" +
                $"La aprobación se realizó en base al siguiente detalle:<br />" +
                $"<br />" +
                $"{prereqData} <br /><br />" + 
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
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

            #region Correo Cambio Fecha
            var mailMessage2 = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = $"{request.CorrelativePrefix} - Aviso de reprogramación de Requerimiento {code}"
            };

            foreach (var auth in logAuths.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == auth.UserId);

                mailMessage2.To.Add(new MailAddress(user.Email, user.RawFullName));
            }

            //mailMessage.To.Add(new MailAddress("arian.cc@hotmail.com", "Henrry"));
            mailMessage2.Body =
                $"Hola, <br /><br /> " +
                $"Se ha aprobado el Requerimiento {code} en el centro de costo {request.Project.Abbreviation} " +
                $"y en base a la última aprobación, la fecha de entrega se reprogramó para el {request.DeliveryDate.Value.ToDateString()}.<br />" +
                $"<br />" +
                $"La aprobación se realizó en base al siguiente detalle:<br />" +
                $"<br />" +
                $"{prereqData} <br /><br />" +
                $"Saludos <br />" +
                $"Sistema IVC <br />" +
                $"Control de Logística";
            mailMessage2.IsBodyHtml = true;

            //Mandar Correo
            using (var client = new SmtpClient("smtp.office365.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("sistemaerp@ivc.pe", "S1st3m4erp");
                client.EnableSsl = true;
                await client.SendMailAsync(mailMessage2);
            }
            #endregion


            // await _context.RequestAuthorizations.AddRangeAsync(reqAuths);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPut("obs/{id}")]
        public async Task<IActionResult> UpdateObservations(Guid id, RequestObsViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id);
            request.Observations = model.Observations;
            request.QualityCertificate = model.QualityCertificate;
            request.Blueprint = model.Blueprint;
            request.TechnicalInformation = model.TechnicalInformation;
            request.CalibrationCertificate = model.CalibrationCertificate;
            request.Catalog = model.Catalog;
            request.Other = model.Other;
            request.OtherDescription = model.OtherDescription;

            if (request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
            {
                request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED;
            }

            if (model.RequestFiles != null)
            {
                var newFiles = new List<RequestFile>();
                var storage = new CloudStorageService(_storageCredentials);
                foreach (var file in model.RequestFiles)
                {
                    newFiles.Add(new RequestFile
                    {
                        RequestId = id,
                        FileUrl = await storage.UploadFile(file.OpenReadStream(),
                                    ConstantHelpers.Storage.Containers.LOGISTICS,
                                    Path.GetExtension(file.FileName),
                                    ConstantHelpers.Storage.Blobs.REQUEST_FILES,
                                    $"{request.CorrelativePrefix}-{request.CorrelativeCode}-{Path.GetFileNameWithoutExtension(file.FileName)}")
                    });
                }
                await _context.RequestFiles.AddRangeAsync(newFiles);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpPost("{id}/actualizar-atencion")]
        public async Task<IActionResult> UpdateAttention(Guid id, int status)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var request = await _context.Requests.FindAsync(id);
            request.AttentionStatus = status;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = await _context.Requests
                .FirstOrDefaultAsync(x => x.Id == id);

            var items = await _context.RequestItems
                .Include(x => x.PreRequestItem)
                .Where(x => x.RequestId == id).ToListAsync();

            //var users = await _context.RequestUsers
            //    .Where(x => x.RequestId == id).ToListAsync();

            foreach (var item in items)
            {
                if (item.PreRequestItemId != null)
                {
                    var flag = true;
                    var preReqItem = await _context.PreRequestItems.FirstOrDefaultAsync(x => x.Id == item.PreRequestItemId);
                    var req = await _context.PreRequests.FirstOrDefaultAsync(x => x.Id == preReqItem.PreRequestId);

                    var auxCont = _context.RequestItems
                        .Where(x => x.RequestId != id
                        && x.PreRequestItemId != null)
                        .Select(x => x.PreRequestItemId).ToList();
                    var cont = _context.PreRequestItems.Where(x => x.PreRequestId == req.Id).Select(x => x.Id).ToList();
                    foreach(var contItem in cont)
                    {
                        if (auxCont.Contains(contItem))
                        {
                            flag = false;
                            req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                            break;
                        }
                    }
                    if (flag)
                    {
                        req.OrderStatus = ConstantHelpers.Logistics.PreRequest.Status.APPROVED;
                        req.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                    }
                    
                    item.PreRequestItem.MeasureInAttention -= item.Measure;
                }
            }

            _context.RequestItems.RemoveRange(items);
            //_context.RequestUsers.RemoveRange(users);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("archivo/eliminar")]
        public async Task<IActionResult> DeleteFile(Uri url)
        {
            var file = await _context.RequestFiles
                .FirstOrDefaultAsync(x => x.FileUrl == url);

            if (file.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.REQUEST_FILES}/{file.FileUrl.AbsolutePath.Split('/').Last()}",
                    ConstantHelpers.Storage.Containers.LOGISTICS);
            }

            _context.RequestFiles.Remove(file);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("detalles/listar")]
        public async Task<IActionResult> GetAllDetails(Guid? reqId = null)
        {
            if (!reqId.HasValue)
                return Ok(new List<RequestItemViewModel>());

            var summary = await _context.RequestSummaries
            .FirstOrDefaultAsync(x => x.ProjectId == GetProjectId());

            var query = await _context.RequestItems
                .Include(x => x.PreRequestItem)
                .Include(x => x.Request)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.RequestId == reqId.Value)
                .AsNoTracking()
                .Select(x => new RequestItemViewModel
                {
                    Id = x.Id,
                    //GoalBudgetInput = new GoalBudgetInputViewModel
                    //{
                    //    UnitPrice = x.GoalBudgetInput.UnitPrice.ToString("N2"),
                    //    Supply = new SupplyViewModel
                    //    {
                    //        Description = x.GoalBudgetInput.Supply.FullCode + " - " + x.GoalBudgetInput.Supply.Description
                    //    },
                    //    Metered = x.GoalBudgetInput.Metered.ToString("N2")
                    //},
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.FullCode + " - " + x.Supply.Description
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    RequestId = x.RequestId,
                    Request = new RequestInRequestItemsViewModel
                    {
                        CorrelativeCode = x.Request.CorrelativeCode,
                        CorrelativeCodeStr = x.Request.CorrelativePrefix + "-" + x.Request.CorrelativeCode.ToString("D4"),
                    },
                    PreRequestItemId = x.PreRequestItemId,
                    PreRequestItem = new PreRequestItemViewModel
                    {
                        PreRequest = new PreRequestViewModel
                        {
                            RequestType = 1,
                            CorrelativeCode = x.PreRequestItem.PreRequest.CorrelativeCode,
                            CorrelativeCodeStr = x.PreRequestItem.PreRequest.CorrelativePrefix + "-" + x.PreRequestItem.PreRequest.CorrelativeCode.ToString("D4"),
                        },
                        Measure = x.PreRequestItem.Measure,
                        MeasureInAttention = x.PreRequestItem.MeasureInAttention,
                    },
                    Measure = x.Measure.ToString(),
                    MeasureUnit = x.Supply.MeasurementUnit.Abbreviation,
                    SupplyGroupStr = x.Supply.SupplyGroup.Name,
                    Observations = x.Observations
                }).ToListAsync();

            return Ok(query);
        }

        [HttpPost("ordenes/detalles/listar")]
        public async Task<IActionResult> GetAllDetailsInOrders(List<Guid> reqIds)
        {
            var data = new List<RequestItem>();

            foreach (var reqId in reqIds)
            {
                var query = await _context.RequestItems
                    .Include(x => x.PreRequestItem)
                    .Include(x => x.Request)
                    .Include(x => x.WorkFront)
                    .Include(x => x.Supply.SupplyGroup)
                    .Include(x => x.Supply.SupplyFamily)
                    .Include(x => x.Supply.MeasurementUnit)
                    .Where(x => x.RequestId == reqId
                    && x.Measure != x.MeasureInAttention)
                    .AsNoTracking().ToListAsync();

                data.AddRange(query);
            }

            var result = new List<RequestItemViewModel>();

            var aux = data.GroupBy(x => new { x.SupplyId, x.Observations });

            foreach (var grupo in aux) 
            {
                var metrado = 0.0;
                var metradoAtencion = 0.0;
                var list = new List<string>();

                foreach(var item in grupo)
                {
                    metrado += item.Measure;
                    metradoAtencion += item.MeasureInAttention;
                }

                foreach (var item in grupo.GroupBy(x => x.RequestId))
                {
                    list.Add(item.FirstOrDefault().Request.CorrelativePrefix
                        + "-" + item.FirstOrDefault().Request.CorrelativeCode.ToString("D4"));
                }

                result.Add(new RequestItemViewModel
                {
                    Id = grupo.FirstOrDefault().SupplyId,
                    Supply = new SupplyViewModel
                    {
                        Description = grupo.FirstOrDefault().Supply.Description,
                        SupplyFamily = new SupplyFamilyViewModel
                        {
                            Code = grupo.FirstOrDefault().Supply.SupplyFamily.Code
                        },
                        SupplyGroupId = grupo.FirstOrDefault().Supply.SupplyGroupId,
                        SupplyGroup = new SupplyGroupViewModel
                        {
                            Code = grupo.FirstOrDefault().Supply.SupplyGroup.Code
                        },
                        CorrelativeCode = grupo.FirstOrDefault().Supply.CorrelativeCode
                    },
                    WorkFrontId = grupo.FirstOrDefault().WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = grupo.FirstOrDefault().WorkFront.Code
                    },
                    RequestId = grupo.FirstOrDefault().RequestId,
                    Request = new RequestInRequestItemsViewModel
                    {
                        CorrelativeCodeStr = string.Join(" / ", list),
                    },
                    Measure = metrado.ToString(),
                    MeasureUnit = grupo.FirstOrDefault().Supply.MeasurementUnit.Abbreviation,
                    SupplyGroupStr = grupo.FirstOrDefault().Supply.SupplyGroup.Name,
                    Observations = grupo.FirstOrDefault().Observations,
                    MeasureInAttention = metradoAtencion,
                    MeasureToAttent = metrado - metradoAtencion
                });
            }

            return Ok(result);
        }

        [HttpGet("detalles/{id}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var data = await _context.RequestItems
                .Where(x => x.Id == id)
                .Select(x => new RequestItemViewModel
                {
                    Id = x.Id,
                    RequestId = x.RequestId,
                    WorkFrontId = x.WorkFrontId,
                    Measure = x.Measure.ToString(),
                    SupplyId = x.SupplyId,
                    Supply = new SupplyViewModel
                    {
                        SupplyGroupId = x.Supply.SupplyGroupId
                    },
                    Observations = x.Observations,
                    MeasureInAttention = x.MeasureInAttention
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(data);
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPost("detalles/crear")]
        public async Task<IActionResult> CreateDetail(RequestItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var aux = _context.Requests.FirstOrDefault(x => x.Id == model.RequestId);

            if (aux.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED)
                return BadRequest("El Requerimiento está cancelado");

            //var goalBudgetInput = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == model.GoalBudgetInputId);

            //if (model.Measure.ToDoubleString() > goalBudgetInput.CurrentMetered)
            //    return BadRequest("No se puede ingresar un metrado mayor al límite.");
            if (model.Observations == null) model.Observations = "";
            var requestItem = new RequestItem
            {
                RequestId = model.RequestId,
                Measure = model.Measure.ToDoubleString(),
                //GoalBudgetInputId = model.GoalBudgetInputId,
                SupplyId = model.SupplyId,
                WorkFrontId = model.WorkFrontId,
                Observations = model.Observations
            };

            var request = await _context.Requests
         .FirstOrDefaultAsync(x => x.Id == requestItem.RequestId);

            if (request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED
                || request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED)
                request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.ISSUED;

            await _context.RequestItems.AddAsync(requestItem);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [DisableRequestSizeLimit]
        [HttpPut("detalles/editar/{id}")]
        public async Task<IActionResult> EditDetail(Guid id, RequestItemViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var goalBudgetInput = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == model.GoalBudgetInputId);
            var supply = await _context.Supplies.FirstOrDefaultAsync(x => x.Id == model.SupplyId);

            var requestItems = await _context.RequestItems
                .Include(x => x.PreRequestItem)
                .Include(x => x.PreRequestItem.PreRequest).ToListAsync();

            var suma = 0.0;

            var requeridos = requestItems
                .Where(x => x.SupplyId == model.SupplyId).ToList();

            var completo = true;

            foreach (var item in requeridos)
            {
                suma += item.Measure;
            }

            var requestItem = requestItems
                .FirstOrDefault(x => x.Id == id);

            var aux2 = _context.Requests.FirstOrDefault(x => x.Id == requestItem.RequestId);

            if (aux2.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.CANCELED)
                return BadRequest("El Requerimiento está cancelado");

            var preReq = await _context.PreRequestItems.FirstOrDefaultAsync(x => x.Id == requestItem.PreRequestItemId);

            var measure = model.Measure.ToDoubleString();

            if (requestItem.PreRequestItemId != null)
            {
                var aux = requestItem.PreRequestItem.Measure - requestItem.PreRequestItem.MeasureInAttention;
                requestItem.PreRequestItem.MeasureInAttention -= requestItem.Measure;
                requestItem.PreRequestItem.MeasureInAttention += measure;
            }
            else
            {
                requestItem.SupplyId = model.SupplyId;
                requestItem.WorkFrontId = model.WorkFrontId;
            }
            requestItem.Measure = measure;
            requestItem.Observations = model.Observations;


            var request = await _context.Requests
                .FirstOrDefaultAsync(x => x.Id == requestItem.RequestId);

            if (request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED)
                request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED;

            if (requestItem.PreRequestItemId != null)
            {
                var reqItems = requestItems.Where(x => x.PreRequestItemId == requestItem.PreRequestItemId).ToList();

                foreach (var item in reqItems)
                {
                    if (item.Measure != item.MeasureInAttention)
                        completo = false;

                    if (completo == true)
                        item.PreRequestItem.PreRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                    else
                        item.PreRequestItem.PreRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;

                    suma += item.Measure;
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.FULL_ACCESS)]
        [HttpDelete("detalles/eliminar/{id}")]
        public async Task<IActionResult> DeleteDetail(Guid id)
        {
            var requestItem = await _context.RequestItems
                .Include(x => x.PreRequestItem)
                .FirstOrDefaultAsync(x => x.Id == id);

            var pre = await _context.PreRequestItems
                .Include(x => x.PreRequest)
                .FirstOrDefaultAsync(x => x.Id == requestItem.PreRequestItemId);
  
            if (requestItem == null)
                return BadRequest($"Elemento con Id '{id}' no encontrado.");

            if (pre != null)
            {
                requestItem.PreRequestItem.MeasureInAttention -= requestItem.Measure;
                _context.RequestItems.Remove(requestItem);

                var requeridos = await _context.PreRequestItems
                    .Include(x => x.PreRequest)
                    .Where(x => x.PreRequestId == pre.PreRequestId).ToListAsync();

                var completo = true;
                var pendiente = false;

                foreach (var item in requeridos)
                {
                    if (item.Measure != item.MeasureInAttention)
                    {
                        completo = false;
                        if (item.MeasureInAttention == 0)
                            pendiente = true;
                    }
                }

                if (completo == true)
                    pre.PreRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL;
                else
                {
                    if(pendiente == false)
                    pre.PreRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL;
                    else
                        pre.PreRequest.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;
                }
            }
            else
                _context.RequestItems.Remove(requestItem);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("insumo-meta/{id}")]
        public async Task<IActionResult> GetGoalBudgetInputRequired(Guid id)
        {
            /*
            var data = await _context.RequestItems
                .Include(x => x.Request)
                .Where(x => x.GoalBudgetInputId == id && x.Request.OrderStatus != ConstantHelpers.Logistics.RequestOrder.Status.CANCELED)
                .ToListAsync();
            */
            var goalBudgetInput = await _context.GoalBudgetInputs.FirstOrDefaultAsync(x => x.Id == id);

            if (goalBudgetInput == null)
                return BadRequest("No se ha encontrado el insumo meta");
            /*

            var sumaMetered = 0.0;

            foreach(var item in data)
            {
                sumaMetered += item.Measure;
            }
            */
            var res = goalBudgetInput.CurrentMetered;

            return Ok(Math.Round(res, 2));
        }

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


        [HttpGet("emitir-requerimiento-doc/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePdfRequest(Guid id)
        {
            var request = await _context.Requests
                .Include(x => x.Project)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
                .Where(x => x.Id == id)
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    Project = new ProjectViewModel
                    {
                        Name = x.Project.Name,
                        Abbreviation = x.Project.Abbreviation
                    },
                    CorrelativeCode = x.CorrelativeCode,
                    OrderStatus = x.OrderStatus,
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name,
                        Abbreviation = x.BudgetTitle.Abbreviation
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    WarehouseId = x.WarehouseId,
                    Warehouse = new WarehouseViewModel
                    {
                       Address =  x.Warehouse.Address
                    },
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    AttentionStatus = x.AttentionStatus,
                    Observations = x.Observations,
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    TechnicalInformation = x.TechnicalInformation,
                    CalibrationCertificate = x.CalibrationCertificate,
                    Catalog = x.Catalog,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    IssuedUserId = x.IssuedUserId
                }).FirstOrDefaultAsync();

            var users = await _context.Users
                .ToListAsync();
            request.IssuedUserName = users.First(x => x.Id == request.IssuedUserId).FullName;

            //var reqUsers = await _context.RequestUsers
            //    .Include(x => x.User)
            //    .Where(x => x.RequestId == id)
            //    .Select(x => x.User.FullName)
            //    .ToListAsync();
            //request.RequestUsernames = string.Join(" - ", reqUsers);

            var items = await _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.MeasurementUnit)
                .Where(x => x.RequestId == id)
                .Select(x => new RequestItemViewModel
                {
                    Id = x.Id,
                    RequestId = x.RequestId,
                    Supply = new SupplyViewModel
                    {
                        Description = x.Supply.Description
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    MeasureUnit = x.Supply.MeasurementUnit.Abbreviation,
                    Measure = x.Measure.ToString(),
                    Observations = x.Observations,
                    MeasureInAttention = x.MeasureInAttention
                }).ToListAsync();

            request.RequestItems = items;

            return View("Request",request);
        }

        [HttpGet("excel/{id}")]
        public FileResult ExportExcel(Guid id)
        {
            var request = _context.Requests
                .Include(x => x.Project)
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
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
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    ReviewDate = x.ReviewDate.Value.ToDateString(),
                    ApproveDate = x.ApproveDate.Value.ToDateString(),
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                    //SupplyFamily = new SupplyFamilyViewModel
                    //{
                    //    Name = x.SupplyFamily.Name,
                    //    Code = x.SupplyFamily.Code
                    //},
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    TechnicalInformation = x.TechnicalInformation,
                    CalibrationCertificate = x.CalibrationCertificate,
                    Catalog = x.Catalog,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    IssuedUserId = x.IssuedUserId
                }).FirstOrDefault(x => x.Id == id);
            
            string fileName = $"Requerimiento-{request.CorrelativeCodeStr}.xlsx";
            using (XLWorkbook wb = ExcelGenerator(id, request))
            {

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    //return File(stream.ToArray(), "application/pdf", fileName);
                }
            }
        }

        [HttpGet("file")]
        public List<Attachment> Files(Guid id, int flag)
        {
            var request = _context.Requests
                .Include(x => x.Project)
                .Include(x => x.ProjectFormula)
                .Include(x => x.BudgetTitle)
                .Include(x => x.Warehouse)
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
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    OrderStatus = x.OrderStatus,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    RequestType = x.RequestType,
                    IssueDate = x.IssueDate.Value.ToDateString(),
                    DeliveryDate = x.DeliveryDate.Value.ToDateString(),
                    ReviewDate = x.ReviewDate.Value.ToDateString(),
                    ApproveDate = x.ApproveDate.Value.ToDateString(),
                    Warehouse = new WarehouseViewModel
                    {
                        Address = x.Warehouse.Address
                    },
                    QualityCertificate = x.QualityCertificate,
                    Blueprint = x.Blueprint,
                    TechnicalInformation = x.TechnicalInformation,
                    CalibrationCertificate = x.CalibrationCertificate,
                    Catalog = x.Catalog,
                    Other = x.Other,
                    OtherDescription = x.OtherDescription,
                    IssuedUserId = x.IssuedUserId
                }).FirstOrDefault(x => x.Id == id);

            var files = _context.RequestFiles
            .Where(x => x.RequestId == id).ToList();

            var userString = _context.Users
                .Include(x => x.WorkAreaEntity)
                .FirstOrDefault(x => x.Id == request.IssuedUserId);

            request.RequestUsernames = userString.FullName;

            var thecnicalSpecs = _context.TechnicalSpecs.ToList();

            var items = _context.RequestItems
                .Where(x => x.RequestId == id).ToList();

            var folding = items.AsEnumerable()
                  .GroupBy(x => x.Supply.SupplyGroupId)
                  .Where(x => x.Count() > 0);

            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);

            if (request == null || project == null)
                return null;

            List<Attachment> archivos = new List<Attachment>();
            MemoryStream stream = new MemoryStream();
            WebClient client = new WebClient();

            string fileName = $"Requerimiento-{request.CorrelativeCodeStr}.xlsx";


            using (XLWorkbook wb = ExcelGenerator(id, request))
            {
                archivos.Add(new Attachment(stream, fileName));

                wb.SaveAs(stream);
                stream.Position = 0;
                stream.Seek(0, SeekOrigin.Begin);
            }
            foreach (var item in files)
            {
                Stream aux = client.OpenRead(item.FileUrl);
                archivos.Add(new Attachment(aux, item.FileUrl.LocalPath));
            }
           
            if (flag == 1)
            {
                #region Excel Prerequerimientos

                var prereq = _context.PreRequestItems
                    .Include(x => x.PreRequest)
                    .Where(x => items.Select(y => y.PreRequestItemId)
                    .Contains(x.Id)).Select(x => x.PreRequestId).Distinct();
                //var prereqFiles = _context.PreRequestFiles
                //  .Where(x => prereq.Contains(x.PreRequestId));

                foreach (var item in prereq)
                {
                    MemoryStream stream2 = new MemoryStream();
                    WebClient client2 = new WebClient();
                    var prerequest = _context.PreRequests
                     .Include(x => x.Project)
                     .Include(x => x.ProjectFormula)
                     .Include(x => x.BudgetTitle)
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
                 }).FirstOrDefault(x => x.Id == item);

                    var summaryAux = _context.RequestSummaries
                        .FirstOrDefault(x => x.ProjectId == prerequest.ProjectId);

                    var meta = _context.GoalBudgetInputs.ToList();

                    var userStringAux = "";
                   
                    var logAuths = _context.LogisticResponsibles
                        .Where(x => x.ProjectId == prerequest.ProjectId && x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest)
                        .ToList();


                    var prereqItemsAux = _context.PreRequestItems
                        .Include(x => x.Supply)
                        .Include(x => x.Supply.SupplyGroup)
                        .Include(x => x.Supply.MeasurementUnit)
                        .Include(x => x.WorkFront)
                        .Where(x => x.PreRequestId == item).ToList();

                    var projectAux = _context.Projects.FirstOrDefault(x => x.Id == prerequest.ProjectId);

                    if (prerequest == null || projectAux == null)
                        return null;

                    string fileName2 = $"PreRequerimiento-{prerequest.CorrelativeCodeStr}.xlsx";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var workSheet = wb.Worksheets.Add("Formato");

                        workSheet.ShowGridLines = false;
                        #region Contenido

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

                        var enlace = projectAux.LogoUrl.ToString();

                        WebClient clientAux = new WebClient();
                        Stream img = clientAux.OpenRead(enlace);
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
                        workSheet.Cell($"N2").Value = summaryAux.CodePrefix + "/GCO-For-05-A";
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
                        workSheet.Cell($"C6").Value = projectAux.Abbreviation;
                        workSheet.Cell($"C6").Style.Alignment.WrapText = true;
                        workSheet.Range($"C6:D6").Merge();

                        workSheet.Cell($"E6").Value = "Obra: ";
                        workSheet.Cell($"E6").Style.Font.Bold = true;
                        workSheet.Cell($"F6").Value = projectAux.Name;
                        workSheet.Cell($"F6").Style.Font.Bold = true;
                        workSheet.Cell($"F6").Style.Alignment.WrapText = true;
                        workSheet.Range($"F6:L6").Merge();

                        workSheet.Cell($"M6").Value = "Solicitud N°:";
                        workSheet.Cell($"M6").Style.Font.Bold = true;
                        workSheet.Cell($"N6").Value = prerequest.CorrelativeCodeStr;

                        workSheet.Cell($"B7").Value = "COMPRA";
                        workSheet.Range($"B7:C7").Merge();
                        workSheet.Range($"B7:C7").Style.Font.Bold = true;

                        workSheet.Cell($"B8").Value = "SERVICIO";
                        workSheet.Range($"B8:C8").Merge();
                        workSheet.Range($"B8:C8").Style.Font.Bold = true;

                        if (prerequest.RequestType == 1)
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
                        workSheet.Cell($"E8").Value = prerequest.BudgetTitle.Name;
                        workSheet.Range($"E8:F8").Merge();

                        workSheet.Cell($"G7").Value = "Familia";
                        workSheet.Range($"G7:I7").Merge();
                        workSheet.Range($"G7:I7").Style.Font.Bold = true;
                        //workSheet.Cell($"G8").Value = prerequest.SupplyFamily.FullName;
                        workSheet.Range($"G8:I8").Merge();

                        workSheet.Cell($"J7").Value = "Fórmulas";
                        workSheet.Range($"J7:N7").Merge();
                        workSheet.Range($"J7:N7").Style.Font.Bold = true;
                        workSheet.Cell($"J8").Value = prerequest.ProjectFormula.Code + " - " + prerequest.ProjectFormula.Name;
                        workSheet.Range($"J8:N8").Merge();

                        workSheet.Cell($"B9").Value = "Solicitado por:";
                        workSheet.Range($"B9:C9").Merge();
                        workSheet.Range($"B9:C9").Style.Font.Bold = true;

                        var reqUserAux = _context.Users.FirstOrDefault(x => x.Id == prerequest.IssuedUserId);

                        workSheet.Cell($"D9").Value = reqUserAux.FullName;
                        workSheet.Range($"D9:F9").Merge();
                        workSheet.Cell($"D9").Style.Alignment.WrapText = true;

                        if (reqUserAux.SignatureUrl != null)
                        {

                            var enl = reqUserAux.SignatureUrl.ToString();

                            Stream imgSig = clientAux.OpenRead(enl);
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

                        workSheet.Cell($"K9").SetValue(prerequest.IssueDate);

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

                        if (prerequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED ||
                           prerequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.APPROVED_PARTIALLY ||
                           prerequest.OrderStatus == ConstantHelpers.Logistics.PreRequest.Status.ISSUED)
                        {
                            var auths = _context.PreRequestAuthorizations.Where(x => x.PreRequestId == item && x.ApprovedDate != null).ToList();
                            var responsible1 = auths.First();
                            var responsible2 = auths.Last();

                            var userAux = _context.Users.Where(x => x.Id == responsible1.UserId || x.Id == responsible2.UserId).ToList();

                            workSheet.Cell($"D10").Value = userAux.First().FullName;
                            var res1Date = auths.FirstOrDefault(x => x.UserId == responsible1.UserId).ApprovedDate;
                            workSheet.Cell($"K10").SetValue(res1Date.HasValue ? res1Date.Value.ToString("dd/MM/yyyy") : "");

                            if (userAux.First().SignatureUrl != null)
                            {
                                var enlace2 = userAux.First().SignatureUrl.ToString();

                                Stream imgFirma = clientAux.OpenRead(enlace2);
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
                                workSheet.Cell($"D11").Value = userAux.Last().FullName;
                                var res2Date = auths.FirstOrDefault(x => x.UserId == responsible2.UserId).ApprovedDate;
                                workSheet.Cell($"K11").SetValue(res2Date.HasValue ? res2Date.Value.ToString("dd/MM/yyyy") : "");

                                if (userAux.Last().SignatureUrl != null)
                                {
                                    var enlace3 = userAux.Last().SignatureUrl.ToString();

                                    Stream imgFirma2 = clientAux.OpenRead(enlace3);
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

                        if (prerequest.OrderStatus == 1)
                            workSheet.Cell($"L10").Value = "PRE-EMITIDO";
                        else if (prerequest.OrderStatus == 2)
                            workSheet.Cell($"L10").Value = "EMITIDO";
                        else if (prerequest.OrderStatus == 6)
                            workSheet.Cell($"L10").Value = "OBSERVADO";
                        else if (prerequest.OrderStatus == 7)
                            workSheet.Cell($"L10").Value = "PENDIENTE DE RESPUESTA";
                        else if (prerequest.OrderStatus == 8)
                            workSheet.Cell($"L10").Value = "APROBADO";
                        else if (prerequest.OrderStatus == 9)
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

                        foreach (var itemAux in prereqItemsAux)
                        {
                            var goalBudgetInput = meta.FirstOrDefault(x => x.SupplyId == itemAux.SupplyId && x.WorkFrontId == itemAux.WorkFrontId);

                            workSheet.Cell($"B${fila}").Value = contador;
                            workSheet.Cell($"C${fila}").Value = itemAux.Measure;
                            workSheet.Cell($"D${fila}").Value = itemAux.SupplyId != null ? itemAux.Supply.MeasurementUnit.Abbreviation : itemAux.MeasurementUnitName;
                            workSheet.Cell($"E${fila}").Value = " " + (itemAux.SupplyId != null ? itemAux.Supply.Description : itemAux.SupplyName);
                            workSheet.Range($"E${fila}:G${fila}").Merge();
                            workSheet.Cell($"H${fila}").Value = itemAux.SupplyId != null ? itemAux.Supply.SupplyGroup.Name : "";
                            workSheet.Cell($"H${fila}").Style.Alignment.WrapText = true;
                            //workSheet.Cell($"I${fila}").SetValue(goalBudgetInput.UnitPrice.ToString("N2", CultureInfo.InvariantCulture));
                            workSheet.Cell($"J${fila}").SetValue(prerequest.DeliveryDate);
                            workSheet.Cell($"K${fila}").Value = pendiente;
                            workSheet.Cell($"L${fila}").Value = itemAux.WorkFront.Code;
                            workSheet.Range($"L${fila}:M${fila}").Merge();
                            workSheet.Cell($"N${fila}").Value = itemAux.Observations;

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
                        workSheet.Range($"D9:F9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range($"E8:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range($"G8:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        workSheet.Range($"J8:N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        #endregion

                        archivos.Add(new Attachment(stream2, fileName2));

                        wb.SaveAs(stream2);
                        stream2.Position = 0;
                        stream2.Seek(0, SeekOrigin.Begin);
                    }
                }
                #endregion
            }
            
            foreach (var itemAux in folding)
            {
                var thecnicalSpec = thecnicalSpecs
                    .FirstOrDefault(x => x.SupplyFamilyId == itemAux.FirstOrDefault().Supply.SupplyFamilyId
                    && x.SupplyGroupId == itemAux.FirstOrDefault().Supply.SupplyGroupId && x.ProjectId == project.Id);

                if (thecnicalSpec != null && thecnicalSpec.FileUrl != null)
                {
                    Stream aux = client.OpenRead(thecnicalSpec.FileUrl);
                    archivos.Add(new Attachment(aux, "Especificación-Técnica-" + itemAux.FirstOrDefault().Supply.SupplyGroup.Name + ".pdf"));
                }
            }

            return archivos;
        }

        [HttpGet("az5")]
        public XLWorkbook ExcelGenerator(Guid id, RequestViewModel request)
        {

            var files = _context.RequestFiles
            .Where(x => x.RequestId == id).ToList();

            var summary = _context.RequestSummaries
                .FirstOrDefault(x => x.ProjectId == request.ProjectId);

            var userString = _context.Users
                .Include(x => x.WorkAreaEntity)
                .FirstOrDefault(x => x.Id == request.IssuedUserId);

            request.RequestUsernames = userString.FullName;

            var items = _context.RequestItems
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.WorkFront)
                .Include(x => x.PreRequestItem)
                .Include(x => x.PreRequestItem.PreRequest)
                .Where(x => x.RequestId == id).ToList();

            var project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);

            if (request == null || project == null)
                return null;

            var responsibles = _context.RequestAuthorizations
                .Where(x => x.RequestId == request.Id)
                .ToList();

            var users = _context.Users.Include(x => x.WorkAreaEntity).ToList();

            var logRes = new LogisticResponsibleViewModel();

            var resPr = responsibles.ToList();
            var reqAuths = new List<string>();
            var reqOks = new List<string>();
            var reqFails = new List<string>();
            var reqReviews = new List<string>();

            foreach (var res in resPr)
            {
                var usName = users.First(x => x.Id == res.UserId).FullName;
                switch (res.UserType)
                {
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.FailRequest:
                        reqFails.Add(usName);
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewRequest:
                        reqReviews.Add(usName);
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest:
                        reqOks.Add(usName);
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest:
                        reqAuths.Add(usName);
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.FailOrder:
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewOrder:
                        break;
                    case ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder:
                        break;
                    default:
                        break;
                }
            }

            logRes = new LogisticResponsibleViewModel
            {
                ProjectId = request.ProjectId,
                RequestAuthNames = string.Join(" - ", reqAuths),
                RequestOkNames = string.Join(" - ", reqOks),
                RequestFailNames = string.Join(" - ", reqFails),
                RequestReviewNames = string.Join(" - ", reqReviews)
            };

            var prerequests = _context.PreRequestItems
                   .Include(x => x.PreRequest)
                   .Include(x => x.PreRequest.ProjectFormula)
                   .Where(x => items.Select(y => y.PreRequestItemId)
                   .Contains(x.Id));

            XLWorkbook wb = new XLWorkbook();
            var workSheet = wb.Worksheets.Add("Formato");
            workSheet.ShowGridLines = false;
            if (prerequests.Count() > 0)
            {
                #region Excel base a pre-requerimientos
                var authsNames = new List<string>();
                var areasName = new List<string>();
                var projectFormulas = prerequests
                    .Select(x => x.PreRequest.ProjectFormula)
                    .Distinct().ToList();
                var mergeFormula = new List<string>();
                var fecha = prerequests.First().PreRequest.IssueDate;
                foreach (var pr in prerequests)
                {
                    var usu = _context.Users
                        .Include(x => x.WorkAreaEntity)
                        .FirstOrDefault(x => x.Id == pr.PreRequest.IssuedUserId);
                    authsNames.Add(usu.FullName);
                    areasName.Add(usu.WorkAreaEntity.Name);
                    if (pr.PreRequest.IssueDate < fecha)
                        fecha = pr.PreRequest.IssueDate;
                }
                authsNames = authsNames.Distinct().ToList();
                areasName = areasName.Distinct().ToList();

                foreach (var form in projectFormulas)
                {
                    var auxForm = form.Code + "-" + form.Name;
                    mergeFormula.Add(auxForm);
                }
                mergeFormula = mergeFormula.Distinct().ToList();

                var lastPrereq = _context.PreRequestAuthorizations
                    .Include(x => x.PreRequest)
                    .Where(x => prerequests.Select(x => x.PreRequestId).Contains(x.PreRequestId));

                var auxId = lastPrereq.OrderByDescending(x => x.ApprovedDate).First().PreRequestId;
                var lastNames = new List<string>();
                var lastDates = new List<string>();
                var listUser = new List<ApplicationUser>();

                foreach (var lasts in lastPrereq.Where(y => y.PreRequestId == auxId).Select(x => x.UserId))
                {
                    var us = _context.Users
                       .FirstOrDefault(x => x.Id == lasts);
                    lastDates.Add(lastPrereq.FirstOrDefault(x => x.UserId == lasts).ApprovedDate != null ?
                        lastPrereq.FirstOrDefault(x => x.UserId == lasts).ApprovedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "");
                    listUser.Add(us);
                }

                //--------------CAMPOS------------

                var pendiente = "-";
                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 9;
                workSheet.Column(3).Width = 9;
                workSheet.Column(4).Width = 8;
                workSheet.Column(5).Width = 7;
                workSheet.Column(6).Width = 40;
                workSheet.Column(7).Width = 15;
                workSheet.Column(8).Width = 13;
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

                workSheet.Range($"B2:D5").Merge();

                var aux = workSheet.AddPicture(bitmap, "logo");

                aux.MoveTo(16, 22);
                aux.Height = 82;
                aux.Width = 190;

                workSheet.Cell($"E2").Value = "GESTIÓN DE COMPRAS EN OBRA";
                workSheet.Range($"E2:L2").Merge();
                workSheet.Range($"E2:L2").Style.Font.Bold = true;

                workSheet.Cell($"E3").Value = "REQUERIMIENTO DE COMPRA / SERVICIO";
                workSheet.Range($"E3:L5").Merge();
                workSheet.Range($"E3:L5").Style.Font.Bold = true;

                workSheet.Cell($"M2").Value = "Código";
                workSheet.Cell($"N2").Value = summary.CodePrefix + "/GCO-For-05-A";
                workSheet.Cell($"N2").Style.Font.Bold = true;

                workSheet.Cell($"M3").Value = "Versión";
                workSheet.Cell($"N3").Value = "1";
                workSheet.Cell($"N3").Style.Font.Bold = true;

                workSheet.Cell($"M4").Value = "Fecha";
                workSheet.Cell($"N4").Value = "08/01/2022";
                workSheet.Cell($"N4").Style.Font.Bold = true;

                workSheet.Cell($"M5").Value = "Página";
                //workSheet.Cell($"N5").DataType = XLDataType.Text;
                var pagina = "1 de 1";
                workSheet.Cell($"N5").SetValue(pagina);
                workSheet.Cell($"N5").Style.Font.Bold = true;

                workSheet.Row(2).Height = 22;
                workSheet.Row(3).Height = 15;
                workSheet.Row(4).Height = 15;
                workSheet.Row(5).Height = 15;

                workSheet.Range("B2:N5").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:N5").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //----------FILA-6-FILA-11---------

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
                workSheet.Cell($"J8").Value = string.Join(",", mergeFormula);
                workSheet.Range($"J8:N8").Merge();

                workSheet.Cell($"B9").Value = "Solicitado por:";
                workSheet.Range($"B9:C9").Merge();
                workSheet.Range($"B9:C9").Style.Font.Bold = true;

                workSheet.Cell($"D9").Value = string.Join("-", authsNames);
                workSheet.Range($"D9:F9").Merge();
                workSheet.Cell($"D9").Style.Alignment.WrapText = true;

                workSheet.Cell($"G9").Value = "Proceso Solicitante:";
                workSheet.Cell($"G9").Style.Font.Bold = true;
                workSheet.Cell($"G9").Style.Alignment.WrapText = true;

                workSheet.Cell($"H9").Value = string.Join("-", areasName);
                workSheet.Range($"H9:I9").Merge();

                workSheet.Cell($"J9").Value = "Fecha:";
                workSheet.Cell($"J9").Style.Font.Bold = true;

                workSheet.Cell($"K9").SetValue(fecha.Value.ToDateString());

                workSheet.Cell($"L9").Value = "ESTADO:";
                workSheet.Range($"L9:N9").Merge();
                workSheet.Range($"L9:N9").Style.Font.Bold = true;

                workSheet.Cell($"B10").Value = "Autorizado por:";
                workSheet.Range($"B10:C10").Merge();
                workSheet.Range($"B10:C10").Style.Font.Bold = true;

                workSheet.Cell($"D10").Value = listUser.First().FullName;
                workSheet.Range($"D10:G10").Merge();
                workSheet.Cell($"D10").Style.Alignment.WrapText = true;

                if (listUser.Count() != 0)
                {
                    var authUser = users.FirstOrDefault(x => x.Id == listUser.First().Id && x.SignatureUrl != null);

                    if (authUser == null)
                        return null;

                    var enlace2 = authUser.SignatureUrl.ToString();

                    Stream imgFirma = client.OpenRead(enlace2);
                    Bitmap bitmapFirma = new Bitmap(imgFirma);

                    var aux2 = workSheet.AddPicture(bitmapFirma).MoveTo(workSheet.Cell("H11")).WithSize(120, 60);

                    aux2.MoveTo(695, 290);
                    aux2.Height = 60;
                    aux2.Width = 120;
                }

                workSheet.Cell($"J10").Value = "Fecha:";
                workSheet.Cell($"J10").Style.Font.Bold = true;

                if (lastDates.First() != "01/01/0001")
                    workSheet.Cell($"K10").SetValue(lastDates.First());

                workSheet.Cell($"B11").Value = "Autorizado por:";
                workSheet.Range($"B11:C11").Merge();
                workSheet.Range($"B11:C11").Style.Font.Bold = true;

                workSheet.Cell($"D11").Value = listUser.Count() > 1 ? listUser.Last().FullName : "";
                workSheet.Range($"D11:G11").Merge();
                workSheet.Cell($"D11").Style.Alignment.WrapText = true;

                if (listUser.Count() > 1)
                {
                    var authUser = users.FirstOrDefault(x => x.Id == listUser.Last().Id && x.SignatureUrl != null);

                    if (authUser == null)
                        return null;

                    var enlace2 = authUser.SignatureUrl.ToString();

                    Stream imgFirma = client.OpenRead(enlace2);
                    Bitmap bitmapFirma = new Bitmap(imgFirma);

                    var aux2 = workSheet.AddPicture(bitmapFirma).MoveTo(workSheet.Cell("H11")).WithSize(120, 60);

                    aux2.MoveTo(695, 370);
                    aux2.Height = 60;
                    aux2.Width = 120;
                }
                /*
                if (responsibles.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).Count() != 0)
                {
                    var uId = responsibles.FirstOrDefault(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).UserId;
                    var authUser = users.FirstOrDefault(x => x.Id == uId && x.SignatureUrl != null);

                    if (authUser == null)
                        return null;

                    var enlace2 = authUser.SignatureUrl.ToString();

                    Stream imgFirma = client.OpenRead(enlace2);
                    Bitmap bitmapFirma = new Bitmap(imgFirma);

                    var aux2 = workSheet.AddPicture(bitmapFirma).MoveTo(workSheet.Cell("H11")).WithSize(120, 60);

                    aux2.MoveTo(695, 340);
                    aux2.Height = 60;
                    aux2.Width = 120;
                }
                */
                workSheet.Range($"H11:I11").Merge();

                workSheet.Cell($"J11").Value = "Fecha:";
                workSheet.Cell($"J11").Style.Font.Bold = true;

                if (lastDates.Last() != "01/01/0001")
                    workSheet.Cell($"K11").SetValue(lastDates.Count() > 1 ? lastDates.Last() : "");

                workSheet.Cell($"L10").Value = ConstantHelpers.Logistics.RequestOrder.Status.VALUES.Where(x => x.Key == request.OrderStatus).Select(x => x.Value);

                workSheet.Range($"L10:N11").Merge();
                workSheet.Range($"L10:N11").Style.Font.Bold = true;

                workSheet.Row(6).Height = 46;
                workSheet.Row(7).Height = 20;
                workSheet.Row(8).Height = 20;
                workSheet.Row(9).Height = 45;
                workSheet.Row(10).Height = 60;
                workSheet.Row(11).Height = 60;


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
                workSheet.Range("J10:K10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("B11:N11").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
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
                workSheet.Cell($"I13").Value = "COSTO APROXIMADO (S/)";
                workSheet.Cell($"I13").Style.Alignment.WrapText = true;
                workSheet.Cell($"I13").Style.Font.Bold = true;
                workSheet.Cell($"J13").Value = "FECHA DE ENTREGA";
                workSheet.Cell($"J13").Style.Alignment.WrapText = true;
                workSheet.Cell($"J13").Style.Font.Bold = true;
                workSheet.Cell($"K13").Value = "STOCK EN ALMACEN";
                workSheet.Cell($"K13").Style.Alignment.WrapText = true;
                workSheet.Cell($"K13").Style.Font.Bold = true;
                workSheet.Cell($"L13").Value = "PRE-REQUERIMIENTO";
                workSheet.Cell($"L13").Style.Font.Bold = true;
                workSheet.Cell($"L13").Style.Alignment.WrapText = true;
                workSheet.Cell($"M13").Value = "PARA SER USADO EN:";
                workSheet.Cell($"M13").Style.Font.Bold = true;
                workSheet.Cell($"M13").Style.Alignment.WrapText = true;
                workSheet.Cell($"N13").Value = "OBSERVACIONES";
                workSheet.Cell($"N13").Style.Font.Bold = true;

                workSheet.Range("B13:N13").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B13:N13").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B13:N13").Style.Fill.SetBackgroundColor(XLColor.LightGray);

                var fila = 14;
                var contador = 1;

                foreach (var item in items)
                {
                    var goalBudgetInput = _context.Supplies.FirstOrDefault(x => x.Id == item.SupplyId);

                    var suma = 0.0;

                    var requeridos = _context.RequestItems.Where(x => x.SupplyId == item.SupplyId).ToList();

                    foreach (var req in requeridos)
                    {
                        suma += req.Measure;
                    }

                    //var limite = Math.Round(goalBudgetInput.Metered - suma, 2);

                    workSheet.Cell($"B${fila}").Value = contador;
                    workSheet.Cell($"C${fila}").Value = item.Measure;
                    workSheet.Cell($"D${fila}").Value = item.Supply.MeasurementUnit.Abbreviation;
                    workSheet.Cell($"E${fila}").Value = item.Supply.FullCode.ToString() + " - " + item.Supply.Description;
                    workSheet.Range($"E${fila}:G${fila}").Merge();
                    workSheet.Cell($"H${fila}").Value = item.Supply.SupplyGroup.Name;
                    workSheet.Cell($"H${fila}").Style.Alignment.WrapText = true;
                    //workSheet.Cell($"I${fila}").SetValue(item.UnitPrice.ToString("N2", CultureInfo.InvariantCulture));
                    workSheet.Cell($"I${fila}").SetValue("");
                    workSheet.Cell($"J${fila}").SetValue(request.DeliveryDate);
                    workSheet.Cell($"K${fila}").Value = pendiente;

                    if (item.PreRequestItem != null)
                        workSheet.Cell($"L${fila}").Value = item.PreRequestItem.PreRequest.CorrelativePrefix + "-" + item.PreRequestItem.PreRequest.CorrelativeCode.ToString("D4");
                    workSheet.Cell($"L${fila}").Style.Alignment.WrapText = true;
                    workSheet.Cell($"M${fila}").Value = item.WorkFront.Code;
                    workSheet.Cell($"M${fila}").Style.Alignment.WrapText = true;
                    workSheet.Cell($"N${fila}").Value = item.Observations;
                    workSheet.Cell($"N${fila}").Style.Alignment.WrapText = true;

                    fila++;
                    contador++;
                }

                var auxFila = fila + 2;

                if (fila < 34)
                    auxFila = 34;

                while (fila < auxFila)
                {
                    workSheet.Range($"E${fila}:G${fila}").Merge();

                    fila++;
                }

                workSheet.Range($"B14:N{fila - 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B14:N{fila - 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range($"E7:F7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G7:I7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J7:N7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"B9:C9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Range($"E14:G34").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Range($"E8:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G8:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J8:N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Cell($"B${fila + 2}").Value = "Se adjunta:";
                workSheet.Cell($"B${fila + 2}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 2}:N${fila + 2}").Merge();

                workSheet.Cell($"B${fila + 3}").Value = " ☒ Especificaciones Técnicas";
                workSheet.Cell($"B${fila + 3}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 3}:F${fila + 3}").Merge();

                workSheet.Cell($"G${fila + 3}").Value = " ☐ Cuadros comparativos";
                workSheet.Cell($"G${fila + 3}").Style.Font.Bold = true;
                workSheet.Range($"G${fila + 3}:J${fila + 3}").Merge();

                if (files.Count == 0)
                    workSheet.Cell($"K${fila + 3}").Value = " ☐ Otros";
                else
                    workSheet.Cell($"K${fila + 3}").Value = " ☒ Otros";

                workSheet.Cell($"K${fila + 3}").Style.Font.Bold = true;
                workSheet.Range($"K${fila + 3}:N${fila + 3}").Merge();

                workSheet.Cell($"B${fila + 4}").Value = "Marcar la documentación requerida al proveedor, segúin la necesidad del proyecto:";
                workSheet.Cell($"B${fila + 4}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 4}:N${fila + 4}").Merge();

                if (request.QualityCertificate == false)
                    workSheet.Cell($"B${fila + 5}").Value = " ☐ Certificados de calidad";
                else
                    workSheet.Cell($"B${fila + 5}").Value = " ☒ Certificados de calidad";

                workSheet.Cell($"B${fila + 5}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 5}:F${fila + 5}").Merge();

                if (request.Blueprint == false)
                    workSheet.Cell($"G${fila + 5}").Value = " ☐ Planos";
                else
                    workSheet.Cell($"G${fila + 5}").Value = " ☒ Planos";

                workSheet.Cell($"G${fila + 5}").Style.Font.Bold = true;
                workSheet.Range($"G${fila + 5}:J${fila + 5}").Merge();

                if (request.TechnicalInformation == false)
                    workSheet.Cell($"K${fila + 5}").Value = " ☐ Información Técnica";
                else
                    workSheet.Cell($"K${fila + 5}").Value = " ☒ Información Técnica";

                workSheet.Cell($"K${fila + 5}").Style.Font.Bold = true;
                workSheet.Range($"K${fila + 5}:N${fila + 5}").Merge();

                if (request.CalibrationCertificate == false)
                    workSheet.Cell($"B${fila + 6}").Value = " ☐ Certificados de calibración";
                else
                    workSheet.Cell($"B${fila + 6}").Value = " ☒ Certificados de calibración";

                workSheet.Cell($"B${fila + 6}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 6}:F${fila + 6}").Merge();

                if (request.Catalog == false)
                    workSheet.Cell($"G${fila + 6}").Value = " ☐ Catálogos";
                else
                    workSheet.Cell($"G${fila + 6}").Value = " ☒ Catálogos";

                workSheet.Cell($"G${fila + 6}").Style.Font.Bold = true;
                workSheet.Range($"G${fila + 6}:J${fila + 6}").Merge();

                if (request.Other == false)
                    workSheet.Cell($"K${fila + 6}").Value = " ☐ Otros:";
                else
                    workSheet.Cell($"K${fila + 6}").Value = " ☒ Otros: " + request.OtherDescription;

                workSheet.Cell($"K${fila + 6}").Style.Font.Bold = true;
                workSheet.Range($"K${fila + 6}:N${fila + 6}").Merge();

                workSheet.Cell($"D{fila + 7}").Value = "Revisado por: GCO";
                workSheet.Cell($"H{fila + 7}").Value = "Aprobado por: GAO";
                workSheet.Cell($"M{fila + 7}").Value = "SIG/SGAS";
                workSheet.Range($"B{fila + 7}:N{fila + 7}").Style.Font.FontColor = XLColor.BlueGray;

                workSheet.Range($"B${fila + 3}:N${fila + 3}").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B${fila + 2}:N${fila + 6}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                #endregion
            }
            else
            {
                #region Excel sin pre-requerimientos
                //--------------CAMPOS------------

                var pendiente = "-";
                workSheet.Column(1).Width = 1;
                workSheet.Column(2).Width = 9;
                workSheet.Column(3).Width = 9;
                workSheet.Column(4).Width = 8;
                workSheet.Column(5).Width = 7;
                workSheet.Column(6).Width = 40;
                workSheet.Column(7).Width = 15;
                workSheet.Column(8).Width = 13;
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

                workSheet.Range($"B2:D5").Merge();

                var aux = workSheet.AddPicture(bitmap, "logo");

                aux.MoveTo(16, 22);
                aux.Height = 82;
                aux.Width = 190;

                workSheet.Cell($"E2").Value = "GESTIÓN DE COMPRAS EN OBRA";
                workSheet.Range($"E2:L2").Merge();
                workSheet.Range($"E2:L2").Style.Font.Bold = true;

                workSheet.Cell($"E3").Value = "REQUERIMIENTO DE COMPRA / SERVICIO";
                workSheet.Range($"E3:L5").Merge();
                workSheet.Range($"E3:L5").Style.Font.Bold = true;

                workSheet.Cell($"M2").Value = "Código";
                workSheet.Cell($"N2").Value = summary.CodePrefix + "/GCO-For-05-A";
                workSheet.Cell($"N2").Style.Font.Bold = true;

                workSheet.Cell($"M3").Value = "Versión";
                workSheet.Cell($"N3").Value = "1";
                workSheet.Cell($"N3").Style.Font.Bold = true;

                workSheet.Cell($"M4").Value = "Fecha";
                workSheet.Cell($"N4").Value = "08/01/2022";
                workSheet.Cell($"N4").Style.Font.Bold = true;

                workSheet.Cell($"M5").Value = "Página";
                //workSheet.Cell($"N5").DataType = XLDataType.Text;
                var pagina = "1 de 1";
                workSheet.Cell($"N5").SetValue(pagina);
                workSheet.Cell($"N5").Style.Font.Bold = true;

                workSheet.Row(2).Height = 22;
                workSheet.Row(3).Height = 15;
                workSheet.Row(4).Height = 15;
                workSheet.Row(5).Height = 15;

                workSheet.Range("B2:N5").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B2:N5").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                //----------FILA-6-FILA-11---------

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
                workSheet.Cell($"G9").Style.Alignment.WrapText = true;

                workSheet.Cell($"H9").Value = userString.WorkAreaEntity.Name;
                workSheet.Range($"H9:I9").Merge();

                workSheet.Cell($"J9").Value = "Fecha:";
                workSheet.Cell($"J9").Style.Font.Bold = true;

                workSheet.Cell($"K9").SetValue(request.IssueDate);

                workSheet.Cell($"L9").Value = "ESTADO:";
                workSheet.Range($"L9:N9").Merge();
                workSheet.Range($"L9:N9").Style.Font.Bold = true;

                workSheet.Cell($"B10").Value = "Revisado por:";
                workSheet.Range($"B10:C10").Merge();
                workSheet.Range($"B10:C10").Style.Font.Bold = true;

                workSheet.Cell($"D10").Value = logRes.RequestReviewNames;
                workSheet.Range($"D10:I10").Merge();
                workSheet.Cell($"D10").Style.Alignment.WrapText = true;

                workSheet.Cell($"J10").Value = "Fecha:";
                workSheet.Cell($"J10").Style.Font.Bold = true;

                if (request.ReviewDate != "01/01/0001")
                    workSheet.Cell($"K10").SetValue(request.ReviewDate);

                workSheet.Cell($"B11").Value = "Autorizado por:";
                workSheet.Range($"B11:C11").Merge();
                workSheet.Range($"B11:C11").Style.Font.Bold = true;

                workSheet.Cell($"D11").Value = logRes.RequestAuthNames;
                workSheet.Range($"D11:G11").Merge();
                workSheet.Cell($"D11").Style.Alignment.WrapText = true;

                if (responsibles.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).Count() != 0)
                {
                    var uId = responsibles.FirstOrDefault(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).UserId;
                    var authUser = users.FirstOrDefault(x => x.Id == uId && x.SignatureUrl != null);

                    if (authUser == null)
                        return null;

                    var enlace2 = authUser.SignatureUrl.ToString();

                    Stream imgFirma = client.OpenRead(enlace2);
                    Bitmap bitmapFirma = new Bitmap(imgFirma);

                    var aux2 = workSheet.AddPicture(bitmapFirma).MoveTo(workSheet.Cell("H11")).WithSize(120, 60);

                    aux2.MoveTo(695, 340);
                    aux2.Height = 60;
                    aux2.Width = 120;
                }

                workSheet.Range($"H11:I11").Merge();

                workSheet.Cell($"J11").Value = "Fecha:";
                workSheet.Cell($"J11").Style.Font.Bold = true;

                if (request.ApproveDate != "01/01/0001")
                    workSheet.Cell($"K11").SetValue(request.ApproveDate);

                workSheet.Cell($"L10").Value = ConstantHelpers.Logistics.RequestOrder.Status.VALUES.Where(x => x.Key == request.OrderStatus).Select(x => x.Value);

                workSheet.Range($"L10:N11").Merge();
                workSheet.Range($"L10:N11").Style.Font.Bold = true;

                workSheet.Row(6).Height = 46;
                workSheet.Row(7).Height = 20;
                workSheet.Row(8).Height = 20;
                workSheet.Row(9).Height = 45;
                workSheet.Row(10).Height = 36;
                workSheet.Row(11).Height = 60;


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
                workSheet.Range("J10:K10").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

                workSheet.Range("B11:N11").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
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
                workSheet.Cell($"I13").Value = "COSTO APROXIMADO (S/)";
                workSheet.Cell($"I13").Style.Alignment.WrapText = true;
                workSheet.Cell($"I13").Style.Font.Bold = true;
                workSheet.Cell($"J13").Value = "FECHA DE ENTREGA";
                workSheet.Cell($"J13").Style.Alignment.WrapText = true;
                workSheet.Cell($"J13").Style.Font.Bold = true;
                workSheet.Cell($"K13").Value = "STOCK EN ALMACEN";
                workSheet.Cell($"K13").Style.Alignment.WrapText = true;
                workSheet.Cell($"K13").Style.Font.Bold = true;
                workSheet.Cell($"L13").Value = "PRE-REQUERIMIENTO";
                workSheet.Cell($"L13").Style.Font.Bold = true;
                workSheet.Cell($"L13").Style.Alignment.WrapText = true;
                workSheet.Cell($"M13").Value = "PARA SER USADO EN:";
                workSheet.Cell($"M13").Style.Font.Bold = true;
                workSheet.Cell($"M13").Style.Alignment.WrapText = true;
                workSheet.Cell($"N13").Value = "OBSERVACIONES";
                workSheet.Cell($"N13").Style.Font.Bold = true;

                workSheet.Range("B13:N13").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B13:N13").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range("B13:N13").Style.Fill.SetBackgroundColor(XLColor.LightGray);

                var fila = 14;
                var contador = 1;

                foreach (var item in items)
                {
                    var goalBudgetInput = _context.Supplies.FirstOrDefault(x => x.Id == item.SupplyId);

                    var suma = 0.0;

                    var requeridos = _context.RequestItems.Where(x => x.SupplyId == item.SupplyId).ToList();

                    foreach (var req in requeridos)
                    {
                        suma += req.Measure;
                    }

                    //var limite = Math.Round(goalBudgetInput.Metered - suma, 2);

                    workSheet.Cell($"B${fila}").Value = contador;
                    workSheet.Cell($"C${fila}").Value = item.Measure;
                    workSheet.Cell($"D${fila}").Value = item.Supply.MeasurementUnit.Abbreviation;
                    workSheet.Cell($"E${fila}").Value = item.Supply.FullCode.ToString() + " - " + item.Supply.Description;
                    workSheet.Range($"E${fila}:G${fila}").Merge();
                    workSheet.Cell($"H${fila}").Value = item.Supply.SupplyGroup.Name;
                    workSheet.Cell($"H${fila}").Style.Alignment.WrapText = true;
                    //workSheet.Cell($"I${fila}").SetValue(item.GoalBudgetInput.UnitPrice.ToString("N2", CultureInfo.InvariantCulture));
                    workSheet.Cell($"I${fila}").SetValue("");
                    workSheet.Cell($"J${fila}").SetValue(request.DeliveryDate);
                    workSheet.Cell($"K${fila}").Value = pendiente;

                    if (item.PreRequestItem != null)
                        workSheet.Cell($"L${fila}").Value = item.PreRequestItem.PreRequest.CorrelativePrefix + "-" + item.PreRequestItem.PreRequest.CorrelativeCode.ToString("D4");
                    workSheet.Cell($"L${fila}").Style.Alignment.WrapText = true;
                    workSheet.Cell($"M${fila}").Value = item.WorkFront.Code;
                    workSheet.Cell($"M${fila}").Style.Alignment.WrapText = true;
                    workSheet.Cell($"N${fila}").Value = item.Observations;
                    workSheet.Cell($"N${fila}").Style.Alignment.WrapText = true;

                    fila++;
                    contador++;
                }

                var auxFila = fila + 2;

                if (fila < 34)
                    auxFila = 34;

                while (fila < auxFila)
                {
                    workSheet.Range($"E${fila}:G${fila}").Merge();

                    fila++;
                }

                workSheet.Range($"B14:N{fila - 1}").Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B14:N{fila - 1}").Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);

                workSheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                workSheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                workSheet.Range($"E7:F7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G7:I7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J7:N7").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"B9:C9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Range($"E14:G34").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Range($"E8:F8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"G8:I8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                workSheet.Range($"J8:N8").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                workSheet.Cell($"B${fila + 2}").Value = "Se adjunta:";
                workSheet.Cell($"B${fila + 2}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 2}:N${fila + 2}").Merge();

                workSheet.Cell($"B${fila + 3}").Value = " ☒ Especificaciones Técnicas";
                workSheet.Cell($"B${fila + 3}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 3}:F${fila + 3}").Merge();

                workSheet.Cell($"G${fila + 3}").Value = " ☐ Cuadros comparativos";
                workSheet.Cell($"G${fila + 3}").Style.Font.Bold = true;
                workSheet.Range($"G${fila + 3}:J${fila + 3}").Merge();

                if (files.Count == 0)
                    workSheet.Cell($"K${fila + 3}").Value = " ☐ Otros";
                else
                    workSheet.Cell($"K${fila + 3}").Value = " ☒ Otros";

                workSheet.Cell($"K${fila + 3}").Style.Font.Bold = true;
                workSheet.Range($"K${fila + 3}:N${fila + 3}").Merge();

                workSheet.Cell($"B${fila + 4}").Value = "Marcar la documentación requerida al proveedor, segúin la necesidad del proyecto:";
                workSheet.Cell($"B${fila + 4}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 4}:N${fila + 4}").Merge();

                if (request.QualityCertificate == false)
                    workSheet.Cell($"B${fila + 5}").Value = " ☐ Certificados de calidad";
                else
                    workSheet.Cell($"B${fila + 5}").Value = " ☒ Certificados de calidad";

                workSheet.Cell($"B${fila + 5}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 5}:F${fila + 5}").Merge();

                if (request.Blueprint == false)
                    workSheet.Cell($"G${fila + 5}").Value = " ☐ Planos";
                else
                    workSheet.Cell($"G${fila + 5}").Value = " ☒ Planos";

                workSheet.Cell($"G${fila + 5}").Style.Font.Bold = true;
                workSheet.Range($"G${fila + 5}:J${fila + 5}").Merge();

                if (request.TechnicalInformation == false)
                    workSheet.Cell($"K${fila + 5}").Value = " ☐ Información Técnica";
                else
                    workSheet.Cell($"K${fila + 5}").Value = " ☒ Información Técnica";

                workSheet.Cell($"K${fila + 5}").Style.Font.Bold = true;
                workSheet.Range($"K${fila + 5}:N${fila + 5}").Merge();

                if (request.CalibrationCertificate == false)
                    workSheet.Cell($"B${fila + 6}").Value = " ☐ Certificados de calibración";
                else
                    workSheet.Cell($"B${fila + 6}").Value = " ☒ Certificados de calibración";

                workSheet.Cell($"B${fila + 6}").Style.Font.Bold = true;
                workSheet.Range($"B${fila + 6}:F${fila + 6}").Merge();

                if (request.Catalog == false)
                    workSheet.Cell($"G${fila + 6}").Value = " ☐ Catálogos";
                else
                    workSheet.Cell($"G${fila + 6}").Value = " ☒ Catálogos";

                workSheet.Cell($"G${fila + 6}").Style.Font.Bold = true;
                workSheet.Range($"G${fila + 6}:J${fila + 6}").Merge();

                if (request.Other == false)
                    workSheet.Cell($"K${fila + 6}").Value = " ☐ Otros:";
                else
                    workSheet.Cell($"K${fila + 6}").Value = " ☒ Otros: " + request.OtherDescription;

                workSheet.Cell($"K${fila + 6}").Style.Font.Bold = true;
                workSheet.Range($"K${fila + 6}:N${fila + 6}").Merge();

                workSheet.Cell($"D{fila + 7}").Value = "Revisado por: GCO";
                workSheet.Cell($"H{fila + 7}").Value = "Aprobado por: GAO";
                workSheet.Cell($"M{fila + 7}").Value = "SIG/SGAS";
                workSheet.Range($"B{fila + 7}:N{fila + 7}").Style.Font.FontColor = XLColor.BlueGray;

                workSheet.Range($"B${fila + 3}:N${fila + 3}").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                workSheet.Range($"B${fila + 2}:N${fila + 6}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                #endregion
            }

            return wb;
        }

        [HttpPost("importar-items/{id}")]
        public async Task<IActionResult> ImportItems(Guid id, IFormFile file)
        {
            var request = await _context.Requests
                .FirstOrDefaultAsync(x => x.Id == id);

            var supplies = await _context.Supplies
                .ToListAsync();

            var frentes = await _context.WorkFrontProjectPhases
                .Include(x => x.WorkFront)
                .Where(x => x.ProjectPhase.ProjectFormulaId == request.ProjectFormulaId)
                .ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 3;
                    var items = new List<RequestItem>();
                    while (!workSheet.Cell($"B{counter}").IsEmpty())
                    {
                        var item = new RequestItem();
                        var workFrontExcel = workSheet.Cell($"B{counter}").GetString();

                        var workFront = frentes.FirstOrDefault(x => x.WorkFront.Code == workFrontExcel);

                        if (workFront == null)
                            return BadRequest("No se ha encontrado el frente de la fila " + counter);

                        var insumoExcel = workSheet.Cell($"C{counter}").GetString();

                        var insumo = supplies.FirstOrDefault(x => x.Description == insumoExcel);

                        if (insumo == null)
                            return BadRequest("No se ha encontrado el insumo de la fila " + counter);

                        item.WorkFrontId = workFront.WorkFrontId;
                        item.SupplyId = insumo.Id;
                        item.RequestId = id;

                        var metradoExcel = workSheet.Cell($"D{counter}").GetString();
                        double.TryParse(metradoExcel, out double metrado);
                        //if (metrado > insumo.CurrentMetered)
                        //    return BadRequest("Se ha superado el límite del metrado en la fila " + counter);

                        item.Measure = metrado;

                        var observacionExcel = workSheet.Cell($"E{counter}").GetString();
                        item.Observations = observacionExcel;
                        items.Add(item);
                        ++counter;
                    }
                    await _context.RequestItems.AddRangeAsync(items);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("excel-modelo/{id}")]
        public FileResult GetExcelSample(Guid id)
        {
            var request = _context.Requests
                .FirstOrDefault(x => x.Id == id);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add("ITEMS");

                workSheet.Cell($"B2").Value = "Frente";
                workSheet.Cell($"C2").Value = "Insumo";
                workSheet.Cell($"D2").Value = "Metrado";
                workSheet.Cell($"E2").Value = "Observaciones";

                workSheet.Column(2).Width = 50;

                workSheet.Column(3).Width = 70;

                workSheet.Column(4).Width = 15;

                workSheet.Column(5).Width = 35;

                workSheet.Range("B3:E3").Style.Fill.SetBackgroundColor(XLColor.Yellow);

                var meta = _context.GoalBudgetInputs
                    .Include(x => x.Supply)
                    .Include(x => x.WorkFront)
                    .Include(x => x.Supply.SupplyGroup)
                    .Where(x => x.ProjectFormulaId == request.ProjectFormulaId)
                    //&& x.Supply.SupplyFamilyId == request.SupplyFamilyId)
                    .OrderBy(x => x.WorkFront.Code)
                    .ToList();

                DataTable dtItems = new DataTable();
                dtItems.TableName = "Insumos Meta";
                dtItems.Columns.Add("Frente", typeof(string));
                dtItems.Columns.Add("Grupo", typeof(string));
                dtItems.Columns.Add("Insumo", typeof(string));
                dtItems.Columns.Add("Límite", typeof(int));
                foreach (var item in meta)
                    dtItems.Rows.Add(item.WorkFront.Code, item.Supply.SupplyGroup.Name, item.Supply.Description, item.CurrentMetered);
                dtItems.AcceptChanges();

                var workSheetFamily = wb.Worksheets.Add(dtItems);

                workSheetFamily.Column(1).Width = 60;
                workSheetFamily.Column(2).Width = 50;
                workSheetFamily.Column(3).Width = 70;
                workSheetFamily.Column(4).Width = 15;

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Position = 0;
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CargaItemsRequerimiento.xlsx");
                }
            }
        }

        [HttpPost("importar")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var pId = GetProjectId();

            var summaries = await _context.RequestSummaries
                .FirstOrDefaultAsync(x => x.ProjectId == pId);

            var budgetTitles = await _context.BudgetTitles
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var users = await _context.Users
                .ToListAsync();

            var formulas = await _context.ProjectFormulas
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var workFronts = await _context.WorkFronts
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            var supplies = await _context.Supplies
                .Include(x => x.SupplyFamily)
                .Include(x => x.SupplyGroup)
                .ToListAsync();

            var warehouses = await _context.Warehouses
                .Include(x => x.WarehouseType)
                .Where(x => x.WarehouseType.ProjectId == pId)
                .ToListAsync();

            using (var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                using (var workBook = new XLWorkbook(mem))
                {
                    var workSheet = workBook.Worksheets.FirstOrDefault();
                    var counter = 2;

                    var requests = new List<Request>();
                    //var requestUsers = new List<RequestUser>();
                    var items = new List<RequestItem>();

                    var request = new Request();

                    var correlativeAux = 0;

                    while (!workSheet.Cell($"A{counter}").IsEmpty())
                    {
                        var correlativeExcel = workSheet.Cell($"A{counter}").GetString();
                        var correlative = Int32.Parse(correlativeExcel);
                        if (correlative != correlativeAux)
                        {
                            correlativeAux = correlative;
                            request = new Request();

                            request.Id = Guid.NewGuid();
                            request.ProjectId = pId;
                            request.CorrelativeCode = correlative;
                            request.CorrelativePrefix = summaries.CodePrefix;

                            var typeExcel = workSheet.Cell($"B{counter}").GetString();

                            if (typeExcel == "Compra")
                                request.RequestType = ConstantHelpers.Logistics.RequestOrder.Type.PURCHASE;
                            else
                                request.RequestType = ConstantHelpers.Logistics.RequestOrder.Type.SERVICE;

                            request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.APPROVED;
                            request.AttentionStatus = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;

                            var titleExcel = workSheet.Cell($"C{counter}").GetString();

                            var title = budgetTitles.FirstOrDefault(x => x.Name == titleExcel);

                            if (title == null)
                                return BadRequest($"No se encontró el título de presupuesto en la fila {counter}");
                            request.BudgetTitleId = title.Id;

                            request.IssuedUserId = "0ee709d2-618b-4fa0-8aed-a19ddc852231";

                            //requestUsers.Add(new RequestUser
                            //{
                            //    RequestId = request.Id,
                            //    UserId = "0ee709d2-618b-4fa0-8aed-a19ddc852231"
                            //});

                            var dateExcel = workSheet.Cell($"E{counter}").GetDateTime();

                            request.IssueDate = dateExcel;

                            var deliveryDateExcel = workSheet.Cell($"F{counter}").GetDateTime();

                            request.DeliveryDate = deliveryDateExcel;

                            var formulaExcel = workSheet.Cell($"G{counter}").GetString();

                            var formula = formulas.FirstOrDefault(x => x.Code == formulaExcel);

                            if (formula == null)
                                return BadRequest($"No se encontró la fórmula en la fila {counter}");
                            request.ProjectFormulaId = formula.Id;

                            var warehouseExcel = workSheet.Cell($"I{counter}").GetString();

                            var warehouse = warehouses.FirstOrDefault(x => x.Address == warehouseExcel);

                            if (warehouse == null)
                                return BadRequest($"No se encontró el almacén en la fila {counter}");
                            request.WarehouseId = warehouse.Id;

                            requests.Add(request);
                        }

                        var item = new RequestItem();

                        item.RequestId = request.Id;

                        var workFrontExcel = workSheet.Cell($"M{counter}").GetString();

                        var workFront = workFronts.FirstOrDefault(x => x.Code == workFrontExcel);

                        if (workFront == null)
                            return BadRequest("No se ha encontrado el frente de la fila " + counter);
                        item.WorkFrontId = workFront.Id;

                        var insumoExcel = workSheet.Cell($"J{counter}").GetString();

                        var insumo = supplies.FirstOrDefault(x => x.FullCode == insumoExcel);

                        if (insumo == null)
                            return BadRequest("No se ha encontrado el insumo de la fila " + counter + "en insumo meta");
                        item.SupplyId = insumo.Id;

                        var metradoExcel = workSheet.Cell($"L{counter}").GetString();
                        double.TryParse(metradoExcel, out double metrado);
                        //if (metrado > insumo.CurrentMetered)
                        //    return BadRequest("Se ha superado el límite del metrado en la fila " + counter);
                        item.Measure = metrado;

                        //var observacionExcel = workSheet.Cell($"E{counter}").GetString();
                        //item.Observations = observacionExcel;
                        items.Add(item);
                        ++counter;
                    }
                    //await _context.RequestUsers.AddRangeAsync(requestUsers);
                    await _context.RequestItems.AddRangeAsync(items);
                    await _context.Requests.AddRangeAsync(requests);
                    await _context.SaveChangesAsync();
                }
                mem.Close();
            }
            return Ok();
        }

        [HttpGet("exportar-ordenes")]
        public async Task<IActionResult> ExportOrder()
        {
            var req = _context.RequestItems
                .Include(x => Request)
                .Include(x => x.Supply)
                .Include(x => x.Supply.SupplyFamily)
                .Include(x => x.Supply.SupplyGroup)
                .Include(x => x.Supply.MeasurementUnit)
                .Include(x => x.PreRequestItem).ThenInclude(x => x.PreRequest)
                .Where(x => x.Request.ProjectId == GetProjectId() &&
                (x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED || x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.APPROVED_PARTIALLY
                || x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_C || x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ORDER_S ||
                x.Request.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ISSUED))
                .ToList();
            

            #region columns
            var dt = new DataTable("LISTADO DE REQUERIMIENTOS");

            dt.Columns.Add("CENTRO DE COSTO", typeof(string));       
            dt.Columns.Add("PROYECTO", typeof(string));
            dt.Columns.Add("CÓDIGO", typeof(string));
            dt.Columns.Add("ESTADO", typeof(string));                
            dt.Columns.Add("ESTADO ATENCIÓN", typeof(string));       
            dt.Columns.Add("FORMULA", typeof(string));              
            dt.Columns.Add("SOLICITA", typeof(string));              
            dt.Columns.Add("TIPO SOLICITUD", typeof(string));        
            dt.Columns.Add("FECHA EMISIÓN", typeof(DateTime));       
            dt.Columns.Add("FECHA ENTREGA", typeof(DateTime));       
            dt.Columns.Add("PRE-REQUERIMIENTO", typeof(string));     
            dt.Columns.Add("CODIGO IVC - INSUMO", typeof(string));   
            dt.Columns.Add("UND", typeof(string));                   
            dt.Columns.Add("METRADO", typeof(string));               
            dt.Columns.Add("ATENDIDO ACUMULADO", typeof(string));    
            dt.Columns.Add("POR ATENDER", typeof(string));           
            dt.Columns.Add("ESTADO ATENCIÓN DETALLADO", typeof(string));
            dt.Columns.Add("TECHO MAX");                              
            dt.Columns.Add("PRECIO APROX.", typeof(string));          
            dt.Columns.Add("PARA SER USADO EN:", typeof(string));     
            dt.Columns.Add("OBSERVACIONES", typeof(string));

            #endregion

            

            foreach(var aux in req)
            {
                var item = _context.Requests
                    .Include(x => x.Project)
                    .Include(x => x.ProjectFormula)
                    .FirstOrDefault(x => x.Id == aux.RequestId);
                var attentionStatus = "";
                var type = "";
                var username = "";
                var statusStr = "";
                var code = "";
                if (item != null)
                {
                    switch (item.AttentionStatus)
                    {
                        case ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING:
                            attentionStatus = "Pendiente";
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PARTIAL:
                            attentionStatus = "Atendido parcialmente";
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.AttentionStatus.TOTAL:
                            attentionStatus = "Atendido totalmente";
                            break;
                    }
                    if (item.RequestType == 1)
                    {
                        type = "Compra";
                    }
                    else
                    {
                        type = "Servicio";
                    }
                    username = _context.Users.FirstOrDefault(x => x.Id == item.IssuedUserId).FullName;
                    statusStr = ConstantHelpers.Logistics.RequestOrder.Status.VALUES[item.OrderStatus];
                    code = $"{item.CorrelativePrefix}-{item.CorrelativeCode.ToString("D4")}";
                }

                var prereq = "";
                if(aux.PreRequestItemId == null)
                {
                    prereq = "0";
                }
                else
                {
                    prereq = $"{aux.PreRequestItem.PreRequest.CorrelativePrefix}-{aux.PreRequestItem.PreRequest.CorrelativeCode.ToString("D4")}";
                }

                var estado = "";
                if (aux.MeasureInAttention == 0)
                {
                    estado = "Por atender";
                }
                else if (aux.MeasureInAttention < aux.Measure)
                {
                    estado = "Atendido Parcialmente";
                }
                else if (aux.MeasureInAttention == aux.Measure)
                {
                    estado = "Atendido totalmente";
                }

                var supplyName = $"{aux.Supply.FullCode} - {aux.Supply.Description}";

                dt.Rows.Add(item.Project.CostCenter, item.Project.Abbreviation, code, statusStr, attentionStatus, item.ProjectFormula.Code + "-" + item.ProjectFormula.Name,
                username, type, item.IssueDate, item.DeliveryDate, prereq, supplyName, aux.Supply.MeasurementUnit.Abbreviation, aux.Measure,
                aux.MeasureInAttention, aux.Measure - aux.MeasureInAttention, estado, "", "",
                aux.WorkFront.Code, aux.Observations);
            }
            
            var project = _context.Projects.FirstOrDefault(x => x.Id == GetProjectId()).Abbreviation;
            var fileName = $"Listado Requerimientos Órdenes de Compra {project}.xlsx";
            using (var wb = new XLWorkbook())
            {
                var workSheet = wb.Worksheets.Add(dt);

                workSheet.Column(1).Width = 10;
                workSheet.Column(2).Width = 46;
                workSheet.Column(3).Width = 11;
                workSheet.Column(4).Width = 12;
                workSheet.Column(5).Width = 26;
                workSheet.Column(6).Width = 12;
                workSheet.Column(7).Width = 35;
                workSheet.Column(8).Width = 11;
                workSheet.Column(9).Width = 11;
                workSheet.Column(10).Width = 11;
                workSheet.Column(11).Width = 12;
                workSheet.Column(12).Width = 51;
                workSheet.Column(13).Width = 7;
                workSheet.Column(14).Width = 9;
                workSheet.Column(15).Width = 11;
                workSheet.Column(16).Width = 11;
                workSheet.Column(17).Width = 28;
                workSheet.Column(18).Width = 11;
                workSheet.Column(19).Width = 11;
                workSheet.Column(20).Width = 16;
                workSheet.Column(21).Width = 26;
                
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}