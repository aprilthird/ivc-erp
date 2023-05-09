using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using Microsoft.AspNetCore.Identity;
using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.ViewModels.HomeViewModels;
using Microsoft.EntityFrameworkCore;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollAuthorizationRequestViewModels;
using Microsoft.Data.SqlClient;
using IVC.PE.ENTITIES.UspModels.Dashboard;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using System.Net.Mail;
using System.Net;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;

namespace IVC.PE.WEB.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        
        public HomeController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<HomeController> logger)
            : base(context, userManager, roleManager, logger)
        {
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("tareas-pendientes")]
        public async Task<IActionResult> GetPendingTasks(int? term = null)
        {
            var userId = GetUserId();
            var query = _context.Letters
                .Where(x => x.Type == ConstantHelpers.Letter.Type.SENT)
                .Where(x => x.EmployeeId == userId)
                .OrderByDescending(x => x.CreateDate)
                .AsQueryable();
            if (term.HasValue)
            {
                switch(term.Value)
                {
                    case 1:
                        query = query.Where(x => x.CreateDate.Date == DateTime.UtcNow.Date);
                        break;
                    case 2:
                        query = query.Where(x => x.CreateDate.Month == DateTime.UtcNow.Month);
                        break;
                }
            }
            var data = await query.Select(x => new PendingTaskViewModel
                {
                    Name = $"Carta Enviada ({x.Name})",
                    DateTime = x.CreateDate,
                    Url = Url.Action("Index", "LettersSent", new { Area = ConstantHelpers.Areas.DOCUMENTARY_CONTROL }, this.Request.Scheme)
                }).ToListAsync();
            return Ok(data);
        }

        [HttpGet("autorizaciones-pendientes")]
        public async Task<IActionResult> GetPendingAuths()
        {
            string userId = GetUserId();
            //string userId = "6f4ad04c-229d-43a3-bf6f-dd688405392b";

            var auths = await _context.PayrollAuthorizationRequests
                .Where(x => (x.TaskUserAuth1Id == userId || x.TaskUserAuth2Id == userId) && (!x.WeeklyTaskAuth1 || !x.WeeklyTaskAuth2))
                .Select(x => new PayrollAuthorizationRequestViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Text = x.Text,
                    WeekId = x.WeekId
                })
                .OrderByDescending(x => x.Title)
                .ToListAsync();

            if (auths.Count == 0)
            {
                auths = await _context.PayrollAuthorizationRequests
                    .Where(x => x.PayrollUserAuthId == userId && !x.WeeklyPayrollAuth && !x.IsPayrollOk &&
                        x.WeeklyTaskAuth1 && x.WeeklyTaskAuth2)
                    .Select(x => new PayrollAuthorizationRequestViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Text = x.Text,
                        WeekId = x.WeekId
                    })
                    .OrderByDescending(x => x.Title)
                    .ToListAsync();
            }

            return PartialView("_PendingAuths",auths);
        }

        [HttpGet("obreros-semana")]
        public async Task<IActionResult> GetWorkersByWeek(Guid? sgId = null, int? category = null)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter sewerGroupParam = new SqlParameter("@SewerGroupId", System.Data.SqlDbType.UniqueIdentifier);
            sewerGroupParam.Value = (object)sgId ?? DBNull.Value;
            SqlParameter categoryParam = new SqlParameter("@Category", System.Data.SqlDbType.Int);
            categoryParam.Value = (object)category ?? DBNull.Value;

            var workers = await _context.Set<UspWorkersByWeek>().FromSqlRaw("execute Dashboard_uspWorkersByWeek @ProjectId, @SewerGroupId, @Category"
                , projectParam, sewerGroupParam, categoryParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            workers = workers.OrderBy(x => x.YearWeekNumber).ToList();

            var labels = workers.Select(x => x.YearWeekNumber).ToList();
            var quantity = workers.Select(x => x.Workers).ToList();

            return Ok(new
            {
                labels,
                data = new {
                    quantity
                }
            });
        }

        [HttpGet("horas-semana")]
        public async Task<IActionResult> GetHoursByWeek(Guid? sgId = null, int? category = null)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter sewerGroupParam = new SqlParameter("@SewerGroupId", System.Data.SqlDbType.UniqueIdentifier);
            sewerGroupParam.Value = (object)sgId ?? DBNull.Value;
            SqlParameter categoryParam = new SqlParameter("@Category", System.Data.SqlDbType.Int);
            categoryParam.Value = (object)category ?? DBNull.Value;

            var hours = await _context.Set<UspHoursByWeek>().FromSqlRaw("execute Dashboard_uspHoursByWeek @ProjectId, @SewerGroupId, @Category"
                , projectParam, sewerGroupParam, categoryParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            hours = hours.OrderBy(x => x.YearWeekNumber).ToList();

            var labels = hours.Select(x => x.YearWeekNumber).ToList();
            var quantity = hours.Select(x => x.Hours ?? 0.0M).ToList();

            return Ok(new
            {
                labels,
                data = new
                {
                    quantity
                }
            });
        }

        [HttpGet("costos-semana")]
        public async Task<IActionResult> GetCostsByWeek(Guid? sgId = null, int? category = null)
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());
            SqlParameter sewerGroupParam = new SqlParameter("@SewerGroupId", System.Data.SqlDbType.UniqueIdentifier);
            sewerGroupParam.Value = (object)sgId ?? DBNull.Value;
            SqlParameter categoryParam = new SqlParameter("@Category", System.Data.SqlDbType.Int);
            categoryParam.Value = (object)category ?? DBNull.Value;

            var hours = await _context.Set<UspCostsByWeek>().FromSqlRaw("execute Dashboard_uspCostsByWeek @ProjectId, @SewerGroupId, @Category"
                , projectParam, sewerGroupParam, categoryParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            hours = hours.OrderBy(x => x.YearWeekNumber).ToList();

            var labels = hours.Select(x => x.YearWeekNumber).ToList();
            var quantity = hours.Select(x => x.TotalCost ?? 0.0M).ToList();
            
            return Ok(new
            {
                labels,
                data = new
                {
                    quantity
                }
            });
        }

        [HttpGet("racs-diario")]
        public async Task<IActionResult> GetRacsByDay()
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var racs = await _context.Set<UspRacsByDay>().FromSqlRaw("execute Dashboard_uspRacsByDay @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            racs = racs.OrderBy(x => x.ReportDate).ToList();

            var labels = racs.Select(x => x.ReportDate.ToDateString()).ToList();
            var quantity = racs.Select(x => x.TotalRacs).ToList();

            return Ok(new
            {
                labels,
                data = new
                {
                    quantity
                }
            });
        }

        [HttpGet("cartas-enviadas-recibidas")]
        public async Task<IActionResult> GetChartLetters()
        {
            var letters = await _context.Letters
                .Where(x => x.ProjectId == GetProjectId())
                .Select(x => new
                {
                    Type = x.Type
                }).ToListAsync();

            var labels = ConstantHelpers.Letter.Type.VALUES.Select(x => x.Value).ToList();
            var quantity = new List<int>
            {
                letters.Where(x => x.Type == ConstantHelpers.Letter.Type.SENT).Count(),
                letters.Where(x => x.Type == ConstantHelpers.Letter.Type.RECEIVED).Count()
            };

            return Ok(new
            {
                labels,
                data = new
                {
                    quantity
                }
            });
        }

        [HttpGet("cartas-fianza")]
        public async Task<IActionResult> GetActiveBonds()
        {
            SqlParameter projectParam = new SqlParameter("@ProjectId", GetProjectId());

            var query = await _context.Set<UspBondsActive>().FromSqlRaw("execute Dashboard_uspBonds @ProjectId"
                , projectParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var labels = query.Select(x => x.BondNumber).ToList();
            var quantity = query.Select(x => x.PenAmmount).ToList();
            
            var total = labels.Count();
            var totalPen = quantity.Sum();

            return Ok(new
            {
                total,
                totalPen,
                labels,
                data = new
                {
                    quantity
                }
            });
        }


        [HttpGet("requerimientos-pendientes")]
        public async Task<IActionResult> GetRequest()
        {
            var data = await _context.Requests
                .Where(x => x.OrderStatus == ConstantHelpers.Logistics.RequestOrder.Status.ISSUED
                && x.ProjectId == GetProjectId())
                .Select(x => new RequestViewModel
                {
                    Id = x.Id,
                    RequestType = x.RequestType,
                    CorrelativeCodeStr = x.CorrelativePrefix + "-" + x.CorrelativeCode.ToString("D4"),
                    ReviewDate = x.ReviewDate.Value.Date.ToDateString()
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("requerimiento/{id}")]
        public async Task<IActionResult> GetRequestById(Guid id)
        {
            var query = await _context.Requests.FirstOrDefaultAsync(x => x.Id == id);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == query.IssuedUserId);

            var data = new SendRequestObsViewModel
            {
                Id = query.Id,
                Email = user.Email,
                Name = user.FullName
            };

            return Ok(data);
        }

        [HttpPost("enviar-obs/{id}")]
        public async Task<IActionResult> SendObs(SendRequestObsViewModel model)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(x => x.Id == model.Id);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.IssuedUserId);

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = "IVC - Aviso de Observación de Requerimiento "
            };
            mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            mailMessage.Body =
                $"Hola, {user.RawFullName}, <br /><br /> " +
                $"El Requerimiento {request.CorrelativePrefix + "-" + request.CorrelativeCode.ToString("D4")} ha sido observador por los siguientes motivos:<br />" +
                $"<br />" +
                $"{model.Obs}<br />" +
                $"<br />" +
                $"Saludos <br />" +
                $"Sistema IVC";
            mailMessage.IsBodyHtml = true;

            request.OrderStatus = ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED;

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

        [HttpGet("ordenes-pendientes")]
        public async Task<IActionResult> GetOrder()
        {
            var data = await _context.Orders
                .Include(x=>x.Requests)
                .Where(x => x.Status == ConstantHelpers.Logistics.RequestOrder.Status.ISSUED
                && x.Requests.FirstOrDefault().Request.ProjectId == GetProjectId())
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    Type = x.Type,
                    CorrelativeCodeStr = x.Requests.FirstOrDefault().Request.Project.CostCenter + "-" + x.CorrelativeCode.ToString("D4") + "-" + x.ReviewDate.Value.Date.ToDateString(),
                    ReviewDate = x.ReviewDate.Value.Date.ToDateString()
                }).ToListAsync();

            return Ok(data);
        }

        [HttpGet("orden/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var query = await _context.RequestsInOrders.Include(x => x.Request).FirstOrDefaultAsync(x => x.OrderId == id);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == query.Request.IssuedUserId);

            var data = new SendRequestObsViewModel
            {
                Id = query.OrderId,
                Email = user.Email,
                Name = user.FullName
            };

            return Ok(data);
        }

        [HttpPost("enviar-obs-order/{id}")]
        public async Task<IActionResult> SendOrderObs(SendOrderObsViewModel model)
        {
            var query = await _context.RequestsInOrders
                .Include(x => x.Request)
                .Include(x => x.Request.Project)
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x => x.OrderId == model.Id);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == query.Request.IssuedUserId);

            var mailMessage = new MailMessage
            {
                From = new MailAddress("sistemaerp@ivc.pe", "Sistema IVC"),
                Subject = "IVC - Aviso de Observación de Orden "
            };
            mailMessage.To.Add(new MailAddress(user.Email, user.RawFullName));
            mailMessage.Body =
                $"Hola, {user.RawFullName}, <br /><br /> " +
                $"El Requerimiento {query.Request.Project.CostCenter + "-" + query.Order.CorrelativeCode.ToString("D4") + "-" + query.Order.ReviewDate} ha sido observador por los siguientes motivos:<br />" +
                $"<br />" +
                $"{model.Obs}<br />" +
                $"<br />" +
                $"Saludos <br />" +
                $"Sistema IVC";
            mailMessage.IsBodyHtml = true;

            query.Order.Status = ConstantHelpers.Logistics.RequestOrder.Status.OBSERVED;

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
    }
}
