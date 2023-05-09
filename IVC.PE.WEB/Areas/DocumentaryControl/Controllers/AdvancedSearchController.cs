using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.ENTITIES.UspModels.DocumentaryControl;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AdvancedSearchViewModels;
using IVC.PE.WEB.Controllers;
using IVC.PE.WEB.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IVC.PE.WEB.Areas.DocumentaryControl.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.DocumentaryControl.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.DOCUMENTARY_CONTROL)]
    [Route("control-documentario/busqueda")]
    public class AdvancedSearchController : BaseController
    {
        public AdvancedSearchController(IvcDbContext context,
               ILogger<AdvancedSearchController> logger)
               : base(context, logger)
        {
        }

        public IActionResult Index(string q) => View(model: q);

        [HttpGet("buscar")]
        public async Task<IActionResult> GetResults(string searchTerm)
        {
            var projectId = GetProjectId();

            SqlParameter projectParam = new SqlParameter("@ProjectId", projectId);
            SqlParameter searchTermParam = new SqlParameter("@SearchTerm", searchTerm);

            var letters = await _context.Set<UspAdvancedSearch>().FromSqlRaw("execute DocumentaryControl_uspAdvancedSearchLetters @ProjectId, @SearchTerm"
                , projectParam, searchTermParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var workbookSeats = await _context.Set<UspAdvancedSearch>().FromSqlRaw("execute DocumentaryControl_uspAdvancedSearchWorkbookSeats @ProjectId, @SearchTerm"
                , projectParam, searchTermParam)
                .IgnoreQueryFilters()
                .ToListAsync();

            var data = (letters.Select(x => new SearchResultViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Body = x.Body,
                Date = x.Date,
                FileUrl = x.FileUrl,
                Type = x.Type
            })).Concat(workbookSeats.Select(x => new SearchResultViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Body = x.Body,
                Date = x.Date,
                FileUrl = x.FileUrl,
                Type = 3
            })).ToList();

            return PartialView("_Results", data.OrderByDescending(x => x.Date));
        }
    }
}