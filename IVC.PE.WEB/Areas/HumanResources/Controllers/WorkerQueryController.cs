using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerQueryViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.HumanResources.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.HUMAN_RESOURCES)]
    [Route("recursos-humanos/obreros/consulta")]
    public class WorkerQueryController : BaseController
    {
        public WorkerQueryController(IvcDbContext context, ILogger<WorkerQueryController> logger)
            : base(context, logger)
        {
        }

        public IActionResult Index(string document) => View(model: document);

        [HttpGet("buscar")]
        public async Task<IActionResult> GetResults(string document)
        {
            var viewModel = await _context.Workers
                .Select(x => new SearchResultViewModel
                {
                    Name = x.Name,
                    MiddleName = x.MiddleName,
                    PaternalSurname = x.PaternalSurname,
                    MaternalSurname = x.MaternalSurname,
                    BirthDate = x.BirthDate,
                    DocumentType = x.DocumentType,
                    Document = x.Document,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    EmailConfirmed = x.EmailConfirmed,
                    EmailConfirmationDateTime = x.EmailConfirmationDateTime,
                    Gender = x.Gender,
                    Category = x.Category,
                    EntryDate = x.EntryDate,
                    PhotoUrl = x.PhotoUrl,
                    Details = x.WorkerWorkPeriods.Select(p => new SearchDetailViewModel
                    {
                        ProjectId = p.ProjectId,
                        EntryDate = p.EntryDate,
                        CeaseDate = p.CeaseDate,
                        Category = p.Category,
                        Origin = p.Origin,
                        Workgroup = p.Workgroup,
                        LaborRegimen = p.LaborRegimen,
                        PensionFundUniqueIdentificationCode = p.PensionFundUniqueIdentificationCode,
                        HasSctr = p.HasSctr,
                        SctrPensionType = p.SctrPensionType,
                        SctrHealthType = p.SctrHealthType,
                        IsActive = p.IsActive,
                        Project = new SearchDetailViewModel.ProjectViewModel
                        {
                            Name = p.Project.Abbreviation,
                            Description = p.Project.Name,
                            LogoUrl = p.Project.LogoUrl
                        }
                    }).OrderByDescending(p => p.EntryDate).ToList()
                }).FirstOrDefaultAsync(x => x.Document == document);

            return PartialView("_Result", viewModel);
        }
    }
}
