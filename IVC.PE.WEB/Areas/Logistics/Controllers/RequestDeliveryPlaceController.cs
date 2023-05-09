using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVC.PE.WEB.Areas.Logistics.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Logistics.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.LOGISTICS)]
    [Route("logistica/lugar-entrega-requerimientos")]
    public class RequestDeliveryPlaceController : BaseController
    {
        public RequestDeliveryPlaceController(IvcDbContext context) : base(context)
        {
        }
        /*
                public IActionResult Index() => View();

                [HttpGet("listar")]
                public async Task<IActionResult> GetAll()
                {
                    var places = await _context.RequestDeliveryPlaces
                        .Where(x => x.ProjectId == GetProjectId())
                        .Select(x => new RequestDeliveryPlaceViewModel
                        {
                            Id = x.Id,
                            ProjectId = x.ProjectId,
                            Project = new ProjectViewModel
                            {
                                Abbreviation = x.Project.Abbreviation
                            },
                            Description = x.Description
                        }).ToListAsync();

                    return Ok(places);
                }

                [HttpGet("{id}")]
                public async Task<IActionResult> Get(Guid id)
                {
                    var place = await _context.RequestDeliveryPlaces
                        .Where(x => x.Id == id)
                        .Select(x => new RequestDeliveryPlaceViewModel
                        {
                            Id = x.Id,
                            ProjectId = x.ProjectId,
                            Description = x.Description
                        }).FirstOrDefaultAsync();

                    return Ok(place);
                }

                [HttpPost("crear")]
                public async Task<IActionResult> Create(RequestDeliveryPlaceViewModel model)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var place = new RequestDeliveryPlace
                    {
                        ProjectId = GetProjectId(),
                        Description = model.Description
                    };

                    await _context.RequestDeliveryPlaces.AddAsync(place);
                    await _context.SaveChangesAsync();

                    return Ok();
                }

                [HttpPut("editar/{id}")]
                public async Task<IActionResult> Edit(Guid id, RequestDeliveryPlaceViewModel model)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var place = await _context.RequestDeliveryPlaces.FirstOrDefaultAsync(x => x.Id == id);

                    place.Description = model.Description;

                    await _context.SaveChangesAsync();

                    return Ok();
                }

                [HttpDelete("eliminar/{id}")]
                public async Task<IActionResult> Delete(Guid id)
                {
                    var place = await _context.RequestDeliveryPlaces.FirstOrDefaultAsync(x => x.Id == id);

                    _context.RequestDeliveryPlaces.Remove(place);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
            }
                */
    }
}
