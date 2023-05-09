using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.ViewModels;
using IVC.PE.DATA.Context;
using Microsoft.AspNetCore.Identity;
using IVC.PE.ENTITIES.Models.General;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.CORE.Helpers;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace IVC.PE.WEB.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IvcDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly IConfiguration _configuration;
        protected readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BaseController(IvcDbContext context)
        {
            _context = context;
        }

        public BaseController(IvcDbContext context, 
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public BaseController(IvcDbContext context,
            ILogger<BaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public BaseController(IvcDbContext context,
            IConfiguration configuration,
            ILogger<BaseController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<BaseController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<BaseController> logger)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ILogger<BaseController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public BaseController(IvcDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            ILogger<BaseController> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        protected virtual async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
        
        protected virtual string GetUserId() => _userManager.GetUserId(User);

        protected virtual Guid GetProjectId()
        {
            var projectIdStr = HttpContext.Session.GetString("ProjectId");
            if(!string.IsNullOrEmpty(projectIdStr))
            {
                var projectId = Guid.Parse(projectIdStr);
                return projectId;
            }
            return Guid.Empty;
        }

        protected virtual string GetUserProjects()
        {
            var projectsClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Projects");

            return projectsClaim.Value.ToString();
        }

        protected virtual Guid GetBankId()
        {
            var bankIdStr = HttpContext.Session.GetString("BankId");
            if (!string.IsNullOrEmpty(bankIdStr))
            {
                var bankId = Guid.Parse(bankIdStr);
                return bankId;
            }
            return Guid.Empty;
        }

        protected virtual PaginationViewModel GetPaginationParameters()
            => new PaginationViewModel
            {
                Search = Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.SEARCH_VALUE].ToString(),
                CurrentNumber = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.PAGING_FIRST_RECORD]),
                RecordsPerPage = Convert.ToInt32(Request.Query[ConstantHelpers.Datatable.ServerSide.SentParameters.RECORDS_PER_DRAW]),
            };

        protected virtual PagedListViewModel<T> GetPagedList<T>(int totalRecords, int filteredRecords, List<T> data)
            => new PagedListViewModel<T>
            {
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data
            };
    }
}
