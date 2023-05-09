using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseResponsibleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Warehouse.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.WAREHOUSE)]
    [Route("almacenes/responsables")]
    public class WarehouseResponsibleController : BaseController
    {
        public WarehouseResponsibleController(IvcDbContext context,
      ILogger<WarehouseResponsibleController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();
        
        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var responsibles = await _context.WarehouseResponsibles
                .Include(x => x.Project)
                .Where(x => x.ProjectId == GetProjectId())
                .ToListAsync();

            var users = await _context.Users.ToListAsync();

            var warRes = new List<WarehouseResponsibleViewModel>();

            var projectIds = responsibles.Select(x => x.ProjectId).Distinct();

            foreach(var project in projectIds)
            {
                var resPr = responsibles.Where(x => x.ProjectId == project).ToList();
                var theOff = new List<string>();
                var ordReq = new List<string>();
                var stoKee = new List<string>();
                foreach(var res in resPr)
                {
                    var usName = users.First(x => x.Id == res.UserId).FullName;
                    switch (res.UserType)
                    {
                        case ConstantHelpers.Warehouse.UserTypes.ThecnicalOfficeControl:
                            theOff.Add(usName);
                            break;
                        case ConstantHelpers.Warehouse.UserTypes.OrderRequests:
                            ordReq.Add(usName);
                            break;
                        case ConstantHelpers.Warehouse.UserTypes.StoreKeepers:
                            stoKee.Add(usName);
                            break;
                    }
                }

                warRes.Add(new WarehouseResponsibleViewModel
                {
                    ProjectId = project,
                    Project = new ProjectViewModel
                    {
                        Abbreviation = resPr.First().Project.Abbreviation
                    },
                    ThecnicalOfficeNames = string.Join(" - ", theOff),
                    OrderRequestNames = string.Join(" - ", ordReq),
                    StoreKeeperNames = string.Join(" - ", stoKee)
                });
            }

            return Ok(warRes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var resPr = await _context.WarehouseResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var warRes = new WarehouseResponsibleViewModel
            {
                ProjectId = id,
                ThecnicalOfficeIds = resPr.Where(x => x.UserType == ConstantHelpers.Warehouse.UserTypes.ThecnicalOfficeControl).Select(x => x.UserId).ToList(),
                OrderRequestIds = resPr.Where(x => x.UserType == ConstantHelpers.Warehouse.UserTypes.OrderRequests).Select(x => x.UserId).ToList(),
                StoreKeeperIds = resPr.Where(x => x.UserType == ConstantHelpers.Warehouse.UserTypes.StoreKeepers).Select(x => x.UserId).ToList()
            };

            return Ok(warRes);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WarehouseResponsibleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pId = GetProjectId();

            var logRes = await _context.WarehouseResponsibles
                .Where(x => x.ProjectId == pId)
                .ToListAsync();

            _context.WarehouseResponsibles.RemoveRange(logRes);

            if (model.ThecnicalOfficeIds != null)
                foreach (var item in model.ThecnicalOfficeIds)
                {
                    await _context.WarehouseResponsibles
                        .AddAsync(new WarehouseResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Warehouse.UserTypes.ThecnicalOfficeControl
                        });
                }

            if (model.OrderRequestIds != null)
                foreach (var item in model.OrderRequestIds)
                {
                    await _context.WarehouseResponsibles
                        .AddAsync(new WarehouseResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Warehouse.UserTypes.OrderRequests
                        });
                }

            if (model.StoreKeeperIds != null)
                foreach (var item in model.StoreKeeperIds)
                {
                    await _context.WarehouseResponsibles
                        .AddAsync(new WarehouseResponsible
                        {
                            ProjectId = pId,
                            UserId = item,
                            UserType = ConstantHelpers.Warehouse.UserTypes.StoreKeepers
                        });
                }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var warRes = await _context.WarehouseResponsibles
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.WarehouseResponsibles.RemoveRange(warRes);
            await _context.SaveChangesAsync();

            return Ok();
        }
        
    }
}
