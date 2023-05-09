using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.EquipmentMachinery;
using IVC.PE.ENTITIES.UspModels.Finances;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.EquipmentMachinery.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.EQUIPMENT_MACHINERY)]
    [Route("equipos/parte-equipo-liviano")]
    public class EquipmentMachinerySoftPartController : BaseController
    {
        public EquipmentMachinerySoftPartController(IvcDbContext context,
        ILogger<EquipmentMachinerySoftPartController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null, Guid? equipmentProviderId = null, Guid? equipmentMachineryTypeSoftId = null)
        {
            var pId = GetProjectId();

            var query = _context.EquipmentMachinerySoftParts
              .AsQueryable();

            var data = await query
                .Include(x => x.EquipmentProvider)
                .Include(x => x.EquipmentProvider.Provider)
                .Where(x => x.ProjectId == pId)
                .Select(x => new EquipmentMachinerySoftPartViewModel
                {
                    Id = x.Id,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentProvider = new EquipmentProviderViewModel
                    {
                        Provider = new ProviderViewModel
                        {
                            Tradename = x.EquipmentProvider.Provider.Tradename
                        }
                    },
                    EquipmentMachineryTypeSoftId = x.EquipmentMachineryTypeSoftId,
                    EquipmentMachineryTypeSoft = new EquipmentMachineryTypeSoftViewModel 
                    {   
                        Description = x.EquipmentMachineryTypeSoft.Description
                    },
                    EquipmentMachinerySoftId = x.EquipmentMachinerySoftId,
                    EquipmentMachinerySoft = new EquipmentMachinerySoftViewModel
                    {
                        EquipmentPlate = x.EquipmentMachinerySoft.EquipmentPlate,
                        Model = x.EquipmentMachinerySoft.Model,
                        Brand = x.EquipmentMachinerySoft.Brand,
                        Status = x.EquipmentMachinerySoft.Status,
                    },
                    UserId = x.UserId,
                    UserName = x.UserName,
                    
              

                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.EquipmentMachinerySoftParts
                .Include(x => x.EquipmentProvider.Provider)
                .Where(x => x.Id == id)
                .Select(x => new EquipmentMachinerySoftPartViewModel
                {
                    Id = x.Id,
                    //PartNumber = x.PartNumber,
                    //PartDate = x.PartDate.Date.ToDateString(),
                    EquipmentMachineryTypeSoftId = x.EquipmentMachineryTypeSoftId,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    EquipmentProviderId = x.EquipmentProviderId,
                    EquipmentProvider = new EquipmentProviderViewModel
                    {
                        ProviderId = x.EquipmentProvider.ProviderId

                    },
                    EquipmentMachinerySoftId = x.EquipmentMachinerySoftId,
                    //EquipmentMachineryOperatorId = x.EquipmentMachineryOperatorId,
                
                }).FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(EquipmentMachinerySoftPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = new EquipmentMachinerySoftPart
            {
                ProjectId = GetProjectId(),
                //PartNumber = model.PartNumber,
                //PartDate = model.PartDate.ToDateTime(),
                EquipmentMachineryTypeSoftId = model.EquipmentMachineryTypeSoftId,
                EquipmentProviderId = model.EquipmentProviderId,
                EquipmentMachinerySoftId = model.EquipmentMachinerySoftId,
                //EquipmentMachineryOperatorId = model.EquipmentMachineryOperatorId,
                UserId = model.UserId,
                UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname,

            };

            await _context.EquipmentMachinerySoftParts.AddAsync(softpart);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, EquipmentMachinerySoftPartViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            var softpart = await _context.EquipmentMachinerySoftParts
                .FirstOrDefaultAsync(x => x.Id == id);
                softpart.EquipmentMachineryTypeSoftId = model.EquipmentMachineryTypeSoftId;
                softpart.EquipmentProviderId = model.EquipmentProviderId;
                softpart.EquipmentMachinerySoftId = model.EquipmentMachinerySoftId;
                softpart.UserId = model.UserId;
                softpart.UserName = users.Name + " " + users.PaternalSurname + " " + users.MaternalSurname;


            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var bondAdd = await _context.EquipmentMachinerySoftParts.FirstOrDefaultAsync(x => x.Id == id);


            _context.EquipmentMachinerySoftParts.Remove(bondAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
