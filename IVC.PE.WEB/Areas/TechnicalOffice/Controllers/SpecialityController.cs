using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.ENTITIES.UspModels.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/especialidades")]
    public class SpecialityController : BaseController
    {
        public SpecialityController(IvcDbContext context,
    ILogger<SpecialityController> logger) : base(context, logger)
        {
        }
        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll(Guid? formulaId = null)
        {

            var pId = GetProjectId();

            SqlParameter param1 = new SqlParameter("@FormulaId", System.Data.SqlDbType.UniqueIdentifier);
            param1.Value = (object)formulaId ?? DBNull.Value;


            var data = await _context.Set<UspSpecFormula>().FromSqlRaw("execute TechnicalOffice_uspSpecFormula @FormulaId"
                , param1)
.IgnoreQueryFilters()
.ToListAsync();

            data = data.Where(x => x.ProjectId == pId).ToList();

            //if (specId.HasValue)
            //    data = data.Where(x=>x.SpecialityId == specId.Value).ToList();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.Specialities

                 .Where(x => x.Id == id)
                 .Select(x => new SpecialityViewModel
                 {
                     Id = x.Id,
                     Description = x.Description,
                 }).FirstOrDefaultAsync();

            var formulas = await _context.SpecFormulas
.Where(x => x.SpecialityId == id)
.Select(x => x.ProjectFormulaId)
.ToListAsync();

            data.Formulas = formulas;

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(SpecialityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMachineryType = new Speciality
            {

                Description = model.Description,
                ProjectId = GetProjectId()
            };

            if (model.Formulas != null)
                await _context.SpecFormulas.AddRangeAsync(
                model.Formulas.Select(x => new SpecsFormula
                {
                    Speciality = equipmentMachineryType,
                    ProjectFormulaId = x
                }).ToList());

            await _context.Specialities.AddAsync(equipmentMachineryType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, SpecialityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var equipmentMachineryTypes = await _context.Specialities
                .FirstOrDefaultAsync(x => x.Id == id);
            equipmentMachineryTypes.Description = model.Description;

            var SpecialitiesDb = await _context.SpecFormulas
.Where(x => x.SpecialityId == id)
.ToListAsync();

            _context.SpecFormulas.RemoveRange(SpecialitiesDb);
            if (model.Formulas != null)
                await _context.SpecFormulas.AddRangeAsync(
                model.Formulas.Select(x => new SpecsFormula
                {
                    Speciality = equipmentMachineryTypes,
                    ProjectFormulaId = x
                }).ToList());

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var equipmentMachineryTypes = await _context.Specialities
                .FirstOrDefaultAsync(x => x.Id == id);

            var specialitiesDb = await _context.SpecFormulas
.Where(x => x.SpecialityId == id)
.ToListAsync();

            if (equipmentMachineryTypes == null)
                return BadRequest($"Especialidad con Id '{id}' no encontrado.");
            _context.SpecFormulas.RemoveRange(specialitiesDb);
            _context.Specialities.Remove(equipmentMachineryTypes);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
