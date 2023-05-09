using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/parametros")]
    public class PayrollParameterController : BaseController
    {
        public PayrollParameterController(IvcDbContext context)
        : base(context)
        {
        }

        public IActionResult Index() => View();

        #region PayrollParemeters
        [HttpGet("planilla/listar")]
        public async Task<IActionResult> GetAllParameters()
        {
            var parameters = await _context.PayrollParameters
                    .Include(x => x.Project)
                    .Select(x => new PayrollParameterViewModel
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        Project = new ProjectViewModel
                        {
                            Id = x.ProjectId,
                            Abbreviation = x.Project.Abbreviation
                        },
                        DollarExchangeRate = x.DollarExchangeRate,
                        EsSaludMasVidaCost = x.EsSaludMasVidaCost,
                        MaximumInsurableRemuneration = x.MaximumInsurableRemuneration,
                        MinimumWage = x.MinimumWage,
                        SCTRRate = x.SCTRRate,
                        SCTRPensionFixed = x.SCTRPensionFixed,
                        SCTRHealthFixed = x.SCTRHealthFixed,
                        UIT = x.UIT,
                        UnionFee = x.UnionFee
                    }).ToListAsync();

            return Ok(parameters);
        }

        [HttpGet("planilla/{id}")]
        public async Task<IActionResult> GetParameter(Guid id)
        {
            var parameter = await _context.PayrollParameters
                .Where(x => x.Id == id)
                .Select(x => new PayrollParameterViewModel
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    DollarExchangeRate = x.DollarExchangeRate,
                    EsSaludMasVidaCost = x.EsSaludMasVidaCost,
                    MaximumInsurableRemuneration = x.MaximumInsurableRemuneration,
                    MinimumWage = x.MinimumWage,
                    SCTRRate = x.SCTRRate,
                    UIT = x.UIT,
                    UnionFee = x.UnionFee
                }).FirstOrDefaultAsync();

            return Ok(parameter);
        }

        [HttpPost("planilla/crear")]
        public async Task<IActionResult> CreateParameter(PayrollParameterViewModel model)
        {
            var parameter = await _context.PayrollParameters
                .FirstOrDefaultAsync(x => x.ProjectId == model.ProjectId);

            if (parameter != null)
                await EditParameter(parameter.Id, model);

            var parameterNew = new PayrollParameter
            {
                DollarExchangeRate = model.DollarExchangeRate,
                EsSaludMasVidaCost = model.EsSaludMasVidaCost,
                MaximumInsurableRemuneration = model.MaximumInsurableRemuneration,
                MinimumWage = model.MinimumWage,
                ProjectId = model.ProjectId,
                SCTRRate = model.SCTRRate,
                SCTRHealthFixed = model.SCTRHealthFixed,
                SCTRPensionFixed = model.SCTRPensionFixed,
                UIT = model.UIT,
                UnionFee = model.UnionFee
            };

            await _context.PayrollParameters.AddAsync(parameterNew);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("planilla/editar/{id}")]
        public async Task<IActionResult> EditParameter(Guid id, PayrollParameterViewModel model)
        {
            var parameter = await _context.PayrollParameters
                .FirstOrDefaultAsync(x => x.Id == id);

            parameter.DollarExchangeRate = model.DollarExchangeRate;
            parameter.EsSaludMasVidaCost = model.EsSaludMasVidaCost;
            parameter.MaximumInsurableRemuneration = model.MaximumInsurableRemuneration;
            parameter.MinimumWage = model.MinimumWage;
            parameter.SCTRRate = model.SCTRRate;
            parameter.SCTRHealthFixed = model.SCTRHealthFixed;
            parameter.SCTRPensionFixed = model.SCTRPensionFixed;
            parameter.UIT = model.UIT;
            parameter.UnionFee = model.UnionFee;

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region WorkerCategories
        [HttpGet("categorias/listar")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.WorkerCategories
                .Select(x => new WorkerCategoryViewModel
                {
                    Id = x.Id,
                    BUCRate = x.BUCRate,
                    DayWage = x.DayWage,
                    WorkerCategoryId = x.WorkerCategoryId
                }).ToListAsync();

            return Ok(categories);
        }

        [HttpGet("categorias/{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _context.WorkerCategories
                .Where(x => x.Id == id)
                .Select(x => new WorkerCategoryViewModel
                {
                    Id = x.Id,
                    WorkerCategoryId = x.WorkerCategoryId,
                    BUCRate = x.BUCRate,
                    DayWage = x.DayWage
                }).FirstOrDefaultAsync();

            return Ok(category);
        }

        [HttpPut("categorias/editar/{id}")]
        public async Task<IActionResult> EditCategory(Guid id, WorkerCategoryViewModel model)
        {
            var category = await _context.WorkerCategories
                .FirstOrDefaultAsync(x => x.Id == id);

            category.DayWage = model.DayWage;
            category.BUCRate = model.BUCRate;

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region PensionFundAdministrators
        [HttpGet("pensiones/listar")]
        public async Task<IActionResult> GetAll()
        {
            var funds = await _context.PensionFundAdministrators
                .Select(x => new PensionFundAdministratorViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    FundRate = x.FundRate,
                    FlowComissionRate = x.FlowComissionRate,
                    MixedComissionRate = x.MixedComissionRate,
                    DisabilityInsuranceRate = x.DisabilityInsuranceRate
                }).ToListAsync();

            return Ok(funds);
        }

        [HttpGet("pensiones/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var fund = await _context.PensionFundAdministrators
                .Where(x => x.Id == id)
                .Select(x => new PensionFundAdministratorViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    FundRate = x.FundRate,
                    FlowComissionRate = x.FlowComissionRate,
                    MixedComissionRate = x.MixedComissionRate,
                    DisabilityInsuranceRate = x.DisabilityInsuranceRate
                }).FirstOrDefaultAsync();

            return Ok(fund);
        }

        [HttpPut("pensiones/editar/{id}")]
        public async Task<IActionResult> Edit(Guid id, PensionFundAdministratorViewModel model)
        {
            var fund = await _context.PensionFundAdministrators.FirstOrDefaultAsync(x => x.Id == id);

            fund.FundRate = model.FundRate;
            fund.FlowComissionRate = model.FlowComissionRate;
            fund.MixedComissionRate = model.MixedComissionRate;
            fund.DisabilityInsuranceRate = model.DisabilityInsuranceRate;

            await _context.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}