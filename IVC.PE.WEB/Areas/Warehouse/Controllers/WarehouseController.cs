using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Warehouse;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseTypeViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
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
    [Route("almacenes/almacen")]
    public class WarehouseController : BaseController
    {
        public WarehouseController(IvcDbContext context,
           ILogger<WarehouseController> logger) : base(context, logger)
        {

        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var warehouses = await _context.Warehouses
                .Include(x => x.WarehouseType)
                .Where(x => x.WarehouseType.ProjectId == GetProjectId())
                .Select(x => new WarehouseViewModel
                {
                    Id = x.Id,
                    WarehouseTypeId = x.WarehouseTypeId,
                    WarehouseType = new WarehouseTypeViewModel
                    {
                        Name = x.WarehouseType.Name
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    Address = x.Address,
                    GoogleMapsUrl = x.GoogleMapsUrl
                }).ToListAsync();

            return Ok(warehouses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var warehouse = await _context.Warehouses
                .Select(x => new WarehouseViewModel
                {
                    Id = x.Id,
                    WarehouseTypeId = x.WarehouseTypeId,
                    WarehouseType = new WarehouseTypeViewModel
                    {
                        Name = x.WarehouseType.Name
                    },
                    WorkFrontId = x.WorkFrontId,
                    WorkFront = new WorkFrontViewModel
                    {
                        Code = x.WorkFront.Code
                    },
                    Address = x.Address,
                    GoogleMapsUrl = x.GoogleMapsUrl
                }).FirstOrDefaultAsync(x => x.Id == id);

            return Ok(warehouse);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(WarehouseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var warehouse = new IVC.PE.ENTITIES.Models.Warehouse.Warehouse
            {
                WarehouseTypeId = model.WarehouseTypeId,
                WorkFrontId = model.WorkFrontId,
                Address = model.Address,
                GoogleMapsUrl = model.GoogleMapsUrl
            };

            await _context.Warehouses.AddAsync(warehouse);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, WarehouseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.Id == id);

            warehouse.WarehouseTypeId = model.WarehouseTypeId;
            warehouse.WorkFrontId = model.WorkFrontId;
            warehouse.Address = model.Address;
            warehouse.GoogleMapsUrl = model.GoogleMapsUrl;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.Id == id);

            if (warehouse == null)
                return BadRequest("No se encontró el almacén");

            _context.Warehouses.Remove(warehouse);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
