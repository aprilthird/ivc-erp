using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementDetailViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollPensionFundAdministratorRateViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollWorkerCategoryWageViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarMonthViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementHeaderViewModels
{
    public class PayrollMovementHeaderViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid? ProjectId { get; set; }

        //*********************************
        //Parametros de la Planilla********
        //*********************************
        [Required]
        public decimal UIT { get; set; }

        [Required]
        public decimal MinimumWage { get; set; } = 930.00M;

        [Required]
        public decimal DollarExchangeRate { get; set; }

        [Required]
        public decimal MaximumInsurableRemuneration { get; set; }

        [Required]
        public decimal SCTRRate { get; set; }

        [Required]
        public decimal SCTRHealthFixed { get; set; }

        [Required]
        public decimal SCTRPensionFixed { get; set; }

        [Required]
        public decimal EsSaludMasVidaCost { get; set; } = 5;

        [Required]
        public decimal UnionFee { get; set; }

        public IEnumerable<PayrollPensionFundAdministratorRateViewModel> PayrollPensionFundAdministratorRates { get; set; }

        public PayrollPensionFundAdministratorRateViewModel PayrollPensionFundAdministratorRate { get; set; }

        public IEnumerable<PayrollWorkerCategoryWageViewModel> PayrollWorkerCategoryWages { get; set; }

        public IEnumerable<PayrollMovementDetailViewModel> PayrollMovementDetails { get; set; }
    }
}
