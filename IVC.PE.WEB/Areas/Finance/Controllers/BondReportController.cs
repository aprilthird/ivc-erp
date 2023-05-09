using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.CORE.Helpers;
using IVC.PE.DATA.Context;
using IVC.PE.DATA.Migrations;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;

using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondLoadViewModel;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using Microsoft.EntityFrameworkCore;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Finance;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels;
using System.Security.Cryptography.Xml;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondAddViewModels;
using IVC.PE.WEB.Services;
using Microsoft.Extensions.Options;
using IVC.PE.WEB.Options;
using DocumentFormat.OpenXml.Drawing.Diagrams;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVC.PE.WEB.Areas.Finance.Controllers
{
    [Authorize(Roles = ConstantHelpers.Permission.Finance.PARTIAL_ACCESS)]
    [Area(ConstantHelpers.Areas.FINANCE)]
    [Route("finanzas/reportefianza")]
    public class BondReportController : BaseController
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public BondReportController(IvcDbContext context,
             IOptions<CloudStorageCredentials> storageCredentials,
           ILogger<BondReportController> logger) : base(context, logger)
        {
            _storageCredentials = storageCredentials;
        }

        public IActionResult Index() => View();
    }
}

