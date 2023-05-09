using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.EquipmentMachinery;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels;
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
    [Route("equipos/proveedores-de-combustible")]


    public class FuelProviderController : BaseController
    {
        public FuelProviderController(IvcDbContext context,
        ILogger<FuelProviderController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? projectId = null)
        {

            var pId = GetProjectId();

            var query = _context.FuelProviders
              .AsQueryable();

            var data = await query
                .Include(x=>x.Provider)
                .Where(x => x.ProjectId == pId)
                .Select(x => new FuelProviderViewModel
                {
                    Id = x.Id,
                    ProviderId = x.ProviderId,
                    Provider = new ProviderViewModel
                    {
                        BusinessName = x.Provider.BusinessName,
                        RUC = x.Provider.RUC,
                        Address = x.Provider.Address,
                        PhoneNumber = x.Provider.PhoneNumber,
                        CollectionAreaContactName = x.Provider.CollectionAreaContactName,
                        CollectionAreaEmail = x.Provider.CollectionAreaEmail,
                        CollectionAreaPhoneNumber = x.Provider.CollectionAreaPhoneNumber
                    },
                    LastPrice = x.LastPrice

                })
                .ToListAsync();


            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = _context.FuelProviders
               .AsQueryable();

            var data = await query
                .Include(x => x.Project)
                .Where(x => x.Id == id)
                .Select(x => new FuelProviderViewModel
                {
                    Id = x.Id,
                    ProviderId = x.ProviderId,

                })
                .FirstOrDefaultAsync();


            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(FuelProviderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fuel = new FuelProvider
            {
                ProjectId = GetProjectId(),

                ProviderId = model.ProviderId

            };

            await _context.FuelProviders.AddAsync(fuel);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, FuelProviderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fuel = await _context.FuelProviders
                .FirstOrDefaultAsync(x => x.Id == id);
                fuel.ProviderId = model.ProviderId;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.FuelProviders.FirstOrDefaultAsync(x => x.Id == id);



            _context.FuelProviders.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
