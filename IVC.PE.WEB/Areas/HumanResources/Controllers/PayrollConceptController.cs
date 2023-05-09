using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementDetailViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/conceptos")]
    public class PayrollConceptController : BaseController
    {
        public PayrollConceptController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<PayrollConceptController> logger)
            : base(context, userManager, logger)
        {
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var concepts = await _context.PayrollConcepts
                .Select(x => new PayrollConceptViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    ShortDescription = x.ShortDescription,
                    CategoryId = x.CategoryId,
                    Code = x.Code
                }).ToListAsync();

            return Ok(concepts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var concept = await _context.PayrollConcepts
                .Where(x => x.Id == id)
                .Select(x => new PayrollConceptViewModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Code = x.Code,
                    Description = x.Description,
                    ShortDescription = x.ShortDescription,
                    SunatCode = x.SunatCode
                }).FirstOrDefaultAsync();

            return Ok(concept);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(PayrollConceptViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conceptDb = await _context.PayrollConcepts.FirstOrDefaultAsync(x => x.Code == model.Code);

            if (conceptDb != null)
            {
                return BadRequest("El código del concepto ya está en uso.");
            }

            var concept = new PayrollConcept
                {
                    Description = model.Description,
                    ShortDescription = model.ShortDescription,
                    CategoryId = model.CategoryId,
                    Code = model.Code,
                    SunatCode = model.SunatCode
                };

            await _context.PayrollConcepts.AddAsync(concept);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PayrollConceptViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var concept = await _context.PayrollConcepts
                .FirstOrDefaultAsync(x => x.Id == id);

            concept.Description = model.Description;
            concept.ShortDescription = model.ShortDescription;
            concept.SunatCode = model.SunatCode;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var concept = await _context.PayrollConcepts
                .FirstOrDefaultAsync(x => x.Id == id);

            var formulas = await _context.PayrollConceptFormulas
                .Where(x => x.PayrollConceptId == id)
                .CountAsync();
            if (formulas > 0)
                return BadRequest("Hay formulas afiliadas");

            var hasDetails = await _context.PayrollMovementDetails
                .AnyAsync(x => x.PayrollConceptId == id);
            if (hasDetails)
                return BadRequest("Hay planillas usando el concepto.");

            _context.PayrollConcepts.Remove(concept);

            await _context.SaveChangesAsync();

            return Ok();
        }


        //Metodos para Editar por Obrero
        [HttpGet("obrero/listar")]
        public async Task<IActionResult> GetAllByWorker(Guid? weekId = null, Guid? workerId = null)
        {
            if (!weekId.HasValue || !workerId.HasValue)
                return Ok(new List<PayrollMovementDetailViewModel>());

            var header = await _context.PayrollMovementHeaders
                .FirstOrDefaultAsync(x => x.ProjectCalendarWeekId == weekId.Value);

            if (header == null)
                return Ok(new List<PayrollMovementDetailViewModel>());

            var concepts = await _context.PayrollConceptFormulas
                .Include(x => x.PayrollConcept)
                .Where(x => x.LaborRegimeId == ConstantHelpers.PayrollConceptFormula.LaborRegime.CIVILCONSTRUCTION &&
                            (x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.INCOMES ||
                             x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.DISCOUNTS ||
                             x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.CONTRIBUTIONS))
                .Select(x => new WorkerPayrollDetailViewModel
                {
                    Id = null,
                    PayrollConcept = new PayrollConceptViewModel
                    {
                        CategoryId = x.PayrollConcept.CategoryId,
                        Code = x.PayrollConcept.Code,
                        Description = x.PayrollConcept.Description,
                        ShortDescription = x.PayrollConcept.ShortDescription,
                        SunatCode = x.PayrollConcept.SunatCode
                    },
                    PayrollConceptId = x.PayrollConceptId,
                    PayrollMovementHeaderId = header.Id,
                    WorkerId = workerId.Value,
                    Value = 0
                })
                .OrderBy(x => x.PayrollConcept.CategoryId).ThenBy(x => x.PayrollConcept.Code)
                .ToListAsync();

            var workerConcepts = await _context.PayrollMovementDetails
                .Include(x => x.PayrollConcept)
                .Where(x => x.PayrollMovementHeaderId == header.Id && x.WorkerId == workerId &&
                    (x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.INCOMES ||
                     x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.DISCOUNTS || 
                     x.PayrollConcept.CategoryId == ConstantHelpers.PayrollConcept.Category.CONTRIBUTIONS))
                .ToListAsync();

            foreach (var item in concepts)
            {
                var wc = workerConcepts.FirstOrDefault(x => x.PayrollConceptId == item.PayrollConceptId);
                if (wc != null)
                {
                    item.Id = wc.Id;
                    item.Value = wc.Value;
                }
            }

            return Ok(concepts);
        }

        [HttpGet("obrero")]
        public async Task<IActionResult> GetWorkerConcept(Guid wid, Guid? did = null, Guid? hid = null, Guid? cid = null)
        {
            if (did.HasValue)
            {
                var workerConcept = await _context.PayrollMovementDetails
                .Include(x => x.PayrollConcept)
                .Where(x => x.Id == did.Value)
                .Select(x => new WorkerPayrollDetailViewModel
                {
                    Id = x.Id,
                    PayrollConceptId = x.PayrollConceptId,
                    PayrollMovementHeaderId = x.PayrollMovementHeaderId,
                    WorkerId = x.WorkerId,
                    Value = x.Value
                })
                .FirstOrDefaultAsync();

                return Ok(workerConcept);
            }
            else
            {
                var workerConcept = new WorkerPayrollDetailViewModel
                {
                    Id = null,
                    PayrollConceptId = cid.Value,
                    PayrollMovementHeaderId = hid.Value,
                    WorkerId = wid,
                    Value = 0
                };

                return Ok(workerConcept);
            }
        }

        [HttpPut("obrero/editar")]
        public async Task<IActionResult> UpdateWorkerConcept(WorkerPayrollDetailViewModel model)
        {
            if (model.Id.HasValue)
            {
                var workerConcept = await _context.PayrollMovementDetails
                .FirstOrDefaultAsync(x => x.Id == model.Id.Value);

                if (model.Value == 0)
                {
                    _context.PayrollMovementDetails.Remove(workerConcept);
                } else
                {
                    workerConcept.Value = model.Value;
                }
            }
            else
            {
                await _context.PayrollMovementDetails.AddAsync(new PayrollMovementDetail
                {
                    PayrollConceptId = model.PayrollConceptId.Value,
                    PayrollMovementHeaderId = model.PayrollMovementHeaderId,
                    Value = model.Value,
                    WorkerId = model.WorkerId
                });
            }
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                var concepts = await _context.PayrollMovementDetails
                    .Include(x => x.PayrollConcept)
                    .Where(x => x.PayrollMovementHeaderId == model.PayrollMovementHeaderId &&
                                x.WorkerId == model.WorkerId)
                    .ToListAsync();

                var rem = concepts.Where(x => x.PayrollConcept.Code.Contains("R")).ToList();
                var des = concepts.Where(x => x.PayrollConcept.Code.Contains("D")).ToList();
                var apo = concepts.Where(x => x.PayrollConcept.Code.Contains("A")).ToList();
                
                var T01 = concepts.FirstOrDefault(x => x.PayrollConcept.Code.Equals("T01"));
                if (T01 != null)
                    T01.Value = rem.Sum(x => x.Value);

                var T02 = concepts.FirstOrDefault(x => x.PayrollConcept.Code.Equals("T02"));
                if (T02 != null)
                    T02.Value = des.Sum(x => x.Value);

                var T03 = concepts.FirstOrDefault(x => x.PayrollConcept.Code.Equals("T03"));
                if (T03 != null)
                    T03.Value = apo.Sum(x => x.Value);

                var T04 = concepts.FirstOrDefault(x => x.PayrollConcept.Code.Equals("T04"));
                if (T04 != null)
                    T04.Value = T01.Value + T03.Value;

                var T05 = concepts.FirstOrDefault(x => x.PayrollConcept.Code.Equals("T05"));
                if (T05 != null)
                    T05.Value = T01.Value - T02.Value;

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}