using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.LogisticResponsibleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/responsables")]
    public class LogisticResponsibleController : BaseController
    {
        public LogisticResponsibleController(IvcDbContext context) : base(context)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var responsibles = await _context.LogisticResponsibles
                .Include(x => x.Project)
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            var logRes = new List<LogisticResponsibleViewModel>();

            var projectIds = responsibles.Select(x => x.ProjectId).Distinct();

            foreach (var project in projectIds)
            {
                var resPr = responsibles.Where(x => x.ProjectId == project).ToList();
                var reqAuths = new List<string>();
                var reqOks = new List<string>();
                var reqFails = new List<string>();
                var reqReviews = new List<string>();
                var ordAuths = new List<string>();
                var ordOks = new List<string>();
                var ordFails = new List<string>();
                var ordReviews = new List<string>();
                var preReqAuths = new List<string>();
                var preReqOks = new List<string>();
                var preReqFails = new List<string>();
                var secPrereqAuths = new List<string>();

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
                            ordFails.Add(usName);
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewOrder:
                            ordReviews.Add(usName);
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder:
                            ordOks.Add(usName);
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest:
                            preReqAuths.Add(usName);
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.UserTypes.OkPreRequest:
                            preReqOks.Add(usName);
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.UserTypes.FailPreRequest:
                            preReqFails.Add(usName);
                            break;
                        case ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest:
                            secPrereqAuths.Add(usName);
                            break;
                        default:
                            ordAuths.Add(usName);
                            break;
                    }
                }

                logRes.Add(new LogisticResponsibleViewModel
                {
                    ProjectId = project,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = resPr.First().Project.Abbreviation
                    },
                    RequestAuthNames = string.Join(" - ", reqAuths),
                    RequestOkNames = string.Join(" - ", reqOks),
                    RequestFailNames = string.Join(" - ", reqFails),
                    RequestReviewNames = string.Join(" - ", reqReviews),
                    OrderAuthNames = string.Join(" - ", ordAuths),
                    OrderOkNames = string.Join(" - ", ordOks),
                    OrderFailNames = string.Join(" - ", ordFails),
                    OrderReviewNames = string.Join(" - ", ordReviews),
                    PreRequestAuthNames = string.Join(" - ", preReqAuths),
                    PreRequestOkNames = string.Join(" - ", preReqOks),
                    PreRequestFailNames = string.Join(" - ", preReqFails),
                    SecondaryPreRequestAuthNames = string.Join(" - ", secPrereqAuths)
                });
            }

            return Ok(logRes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var resPr = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var logRes = new LogisticResponsibleViewModel
            {
                ProjectId = id,
                RequestAuthIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest).Select(x => x.UserId).ToList(),
                RequestOkIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest).Select(x => x.UserId).ToList(),
                RequestFailIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.FailRequest).Select(x => x.UserId).ToList(),
                RequestReviewIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewRequest).Select(x => x.UserId).ToList(),
                OrderAuthIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder).Select(x => x.UserId).ToList(),
                OrderOkIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder).Select(x => x.UserId).ToList(),
                OrderFailIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.FailOrder).Select(x => x.UserId).ToList(),
                OrderReviewIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewOrder).Select(x => x.UserId).ToList(),
                PreRequestAuthIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest).Select(x => x.UserId).ToList(),
                PreRequestOkIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.OkPreRequest).Select(x => x.UserId).ToList(),
                PreRequestFailIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.FailPreRequest).Select(x => x.UserId).ToList(),
                SecondaryPreRequestAuthIds = resPr.Where(x => x.UserType == ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest).Select(x => x.UserId).ToList()
            };

            return Ok(logRes);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(LogisticResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            var logRes = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            _context.LogisticResponsibles.RemoveRange(logRes);

            if (model.RequestAuthIds != null)
            {
                if (model.RequestAuthIds.Count() > 2)
                    return BadRequest("Los autorizantes no pueden ser más de 2");

                foreach (var item in model.RequestAuthIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthRequest
                        });
                }
            }
            
            if (model.RequestOkIds != null)
                foreach (var item in model.RequestOkIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.OkRequest
                        });
                }

            if (model.RequestFailIds != null)
                foreach (var item in model.RequestFailIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.FailRequest
                        });
                }

            if (model.RequestReviewIds != null)
                foreach (var item in model.RequestReviewIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewRequest
                        });
                }

            if (model.OrderAuthIds != null)
            {
                if (model.OrderAuthIds.Count() > 2)
                    return BadRequest("Los autorizantes no pueden ser más de 2");

                foreach (var item in model.OrderAuthIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthOrder
                        });
                }
            }

            if (model.OrderOkIds != null)
                foreach (var item in model.OrderOkIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.OkOrder
                        });
                }

            if (model.OrderFailIds != null)
                foreach (var item in model.OrderFailIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.FailOrder
                        });
                }

            if (model.OrderReviewIds != null)
                foreach (var item in model.OrderReviewIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.ReviewOrder
                        });
                }

            if (model.PreRequestAuthIds != null)
            {
                if (model.PreRequestAuthIds.Count() > 2)
                    return BadRequest("Los autorizantes no pueden ser más de 2");

                foreach (var item in model.PreRequestAuthIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.AuthPreRequest
                        });
                }
            }

            if (model.PreRequestOkIds != null)
                foreach (var item in model.PreRequestOkIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.OkPreRequest
                        });
                }

            if (model.PreRequestFailIds != null)
                foreach (var item in model.PreRequestFailIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.FailPreRequest
                        });
                }

            if (model.SecondaryPreRequestAuthIds != null)
            {
                if (model.SecondaryPreRequestAuthIds.Count() > 2)
                    return BadRequest("Los autorizantes no pueden ser más de 2");

                foreach (var item in model.SecondaryPreRequestAuthIds)
                {
                    await _context.LogisticResponsibles
                        .AddAsync(new LogisticResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Logistics.RequestOrder.UserTypes.SecAuthPreRequest
                        });
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var logRes = await _context.LogisticResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.LogisticResponsibles.RemoveRange(logRes);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
