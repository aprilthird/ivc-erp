using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.Security;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingCategoryViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Security.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.SECURITY)]
    [Route("seguridad/capacitaciones/variables/categorias")]
    public class TrainingCategoryController : BaseController
    {
        public TrainingCategoryController(IvcDbContext context,
            ILogger<TrainingCategoryController> logger) : base(context, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.TrainingCategories.Select(
                x => new TrainingCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).AsNoTracking().ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var category = await _context.TrainingCategories
                .Where(x => x.Id == id)
                .Select(x => new TrainingCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                }).AsNoTracking()
                .FirstOrDefaultAsync();
            return Ok(category);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(TrainingCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = new TrainingCategory
            {
                Name = model.Name,
            };
            await _context.TrainingCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, TrainingCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var category = await _context.TrainingCategories.FindAsync(id);
            category.Name = model.Name;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _context.TrainingCategories.FindAsync(id);
            if (category is null)
                return BadRequest($"Categoría con Id '{id}' no encontrado.");
            _context.TrainingCategories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
