using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ExpensesUtilityViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using IVC.PE.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.TechnicalOffice.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.TECHNICAL_OFFICE)]
    [Route("oficina-tecnica/gastos-utilidades")]
    public class ExpensesUtilityController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public ExpensesUtilityController(IvcDbContext context,
            IOptions<CloudStorageCredentials> storageCredentials,
            ILogger<ExpensesUtilityController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();

        [HttpGet("listar")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.ExpensesUtilities
                .Include(x => x.ProjectFormula)
                .Where(x => x.ProjectFormula.ProjectId == GetProjectId())
                .Select(x => new ExpensesUtilityViewModel
                {
                    Id = x.Id,
                    Group = x.Group,
                    BudgetType = new BudgetTypeViewModel
                    {
                        Name = x.BudgetType.Name
                    },
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Name = x.ProjectFormula.Name,
                        Code = x.ProjectFormula.Code
                    },
                    FixedGeneralExpense = x.FixedGeneralExpense.ToString("N", CultureInfo.InvariantCulture),
                    FixedGeneralPercentage = x.FixedGeneralPercentage.ToString(),
                    VariableGeneralExpense = x.VariableGeneralExpense.ToString("N", CultureInfo.InvariantCulture),
                    VariableGeneralPercentage = x.VariableGeneralPercentage.ToString(),
                    TotalGeneralExpense = x.TotalGeneralExpense.ToString("N", CultureInfo.InvariantCulture),
                    TotalGeneralPercentage = x.TotalGeneralPercentage.ToString(),
                    Utility = x.Utility.ToString("N", CultureInfo.InvariantCulture),
                    UtilityPercentage = x.UtilityPercentage.ToString(),
                    FileUrl = x.FileUrl
                }).AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var data = await _context.ExpensesUtilities
                .Select(x => new ExpensesUtilityViewModel
                {
                    Id = x.Id, 
                    Group = x.Group,
                    BudgetTypeId = x.BudgetTypeId,
                    BudgetType = new BudgetTypeViewModel
                    {
                        Name = x.BudgetType.Name
                    },
                    BudgetTitleId = x.BudgetTitleId,
                    BudgetTitle = new BudgetTitleViewModel
                    {
                        Name = x.BudgetTitle.Name
                    },
                    ProjectFormulaId = x.ProjectFormulaId,
                    ProjectFormula = new ProjectFormulaViewModel
                    {
                        Name = x.ProjectFormula.Name,
                        Code = x.ProjectFormula.Code
                    },
                    FixedGeneralExpense = x.FixedGeneralExpense.ToString("N", CultureInfo.InvariantCulture),
                    FixedGeneralPercentage = x.FixedGeneralPercentage.ToString(),
                    VariableGeneralExpense = x.VariableGeneralExpense.ToString("N", CultureInfo.InvariantCulture),
                    VariableGeneralPercentage = x.VariableGeneralPercentage.ToString(),
                    TotalGeneralExpense = x.TotalGeneralExpense.ToString("N", CultureInfo.InvariantCulture),
                    TotalGeneralPercentage = x.TotalGeneralPercentage.ToString(),
                    Utility = x.Utility.ToString("N", CultureInfo.InvariantCulture),
                    UtilityPercentage = x.UtilityPercentage.ToString(),
                    FileUrl = x.FileUrl
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            return Ok(data);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Create(ExpensesUtilityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var totalGeneralExpenseFormula = Math.Round(model.FixedGeneralExpense.ToDoubleString() + model.VariableGeneralExpense.ToDoubleString(), 2);
            var fixedGeneralPercentage = 0.0;
            var variableGeneralPercentage = 0.0;
            var totalGeneralPercentageFormula = 0.0;
            if (totalGeneralExpenseFormula != 0)
            {
                fixedGeneralPercentage = Math.Round(model.FixedGeneralExpense.ToDoubleString() / totalGeneralExpenseFormula * 100, 2);
                variableGeneralPercentage = Math.Round(model.VariableGeneralExpense.ToDoubleString() / totalGeneralExpenseFormula * 100, 2);
                totalGeneralPercentageFormula = Math.Round(fixedGeneralPercentage + variableGeneralPercentage, 2);
            } else
            {
                fixedGeneralPercentage = 0;
                variableGeneralPercentage = 0;
                totalGeneralExpenseFormula = 0;
            }

            var data = new ExpensesUtility
            {
                Id = Guid.NewGuid(),
                Group = model.Group,
                BudgetTypeId = model.BudgetTypeId,
                BudgetTitleId = model.BudgetTitleId,
                ProjectFormulaId = model.ProjectFormulaId,
                FixedGeneralExpense = model.FixedGeneralExpense.ToDoubleString(),
                FixedGeneralPercentage = fixedGeneralPercentage,
                VariableGeneralExpense = model.VariableGeneralExpense.ToDoubleString(),
                VariableGeneralPercentage = variableGeneralPercentage,
                TotalGeneralExpense = totalGeneralExpenseFormula,
                TotalGeneralPercentage = totalGeneralPercentageFormula,
                UtilityPercentage = model.UtilityPercentage.ToDoubleString()
            };

            if(model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                data.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EXPENSES_UTILITY,
                    $"gastos-y-utilidades-{data.Id}");
            }

            await _context.ExpensesUtilities.AddAsync(data);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editar/{id}")]

        public async Task<IActionResult> Edit(Guid id, ExpensesUtilityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _context.ExpensesUtilities.FirstOrDefaultAsync(x => x.Id == id);

            var totalGeneralExpenseFormula = Math.Round(model.FixedGeneralExpense.ToDoubleString() + model.VariableGeneralExpense.ToDoubleString(), 2);
            var fixedGeneralPercentage = Math.Round(model.FixedGeneralExpense.ToDoubleString() / totalGeneralExpenseFormula * 100, 2);
            var variableGeneralPercentage = Math.Round(model.VariableGeneralExpense.ToDoubleString() / totalGeneralExpenseFormula * 100, 2);
            var totalGeneralPercentageFormula = Math.Round(fixedGeneralPercentage + variableGeneralPercentage, 2);

            data.Group = model.Group;
            data.BudgetTypeId = model.BudgetTypeId;
            data.BudgetTitleId = model.BudgetTitleId;
            data.ProjectFormulaId = model.ProjectFormulaId;
            data.FixedGeneralExpense = model.FixedGeneralExpense.ToDoubleString();
            data.FixedGeneralPercentage = fixedGeneralPercentage;
            data.VariableGeneralExpense = model.VariableGeneralExpense.ToDoubleString();
            data.VariableGeneralPercentage = variableGeneralPercentage;
            data.TotalGeneralExpense = totalGeneralExpenseFormula;
            data.TotalGeneralPercentage = totalGeneralPercentageFormula;
            data.UtilityPercentage = model.UtilityPercentage.ToDoubleString();

            if (model.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if(data.FileUrl != null) 
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EXPENSES_UTILITY}/{data.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
                data.FileUrl = await storage.UploadFile(model.File.OpenReadStream(),
                    ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE,
                    System.IO.Path.GetExtension(model.File.FileName),
                    ConstantHelpers.Storage.Blobs.EXPENSES_UTILITY,
                    $"gastos-y-utilidades-{data.Id}");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var data = await _context.ExpensesUtilities.FirstOrDefaultAsync(x => x.Id == id);

            if (data == null)
                return BadRequest("No se ha encontrado la información");

            if (data.FileUrl != null)
            {
                var storage = new CloudStorageService(_storageCredentials);
                if (data.FileUrl != null)
                    await storage.TryDelete($"{ConstantHelpers.Storage.Blobs.EXPENSES_UTILITY}/{data.FileUrl.AbsolutePath.Split('/').Last()}",
                        ConstantHelpers.Storage.Containers.TECHNICAL_OFFICE);
            }

                _context.ExpensesUtilities.Remove(data);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
