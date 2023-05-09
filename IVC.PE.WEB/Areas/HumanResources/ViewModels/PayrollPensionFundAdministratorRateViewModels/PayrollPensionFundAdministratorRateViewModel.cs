using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollPensionFundAdministratorRateViewModels
{
    public class PayrollPensionFundAdministratorRateViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid PensionFundAdministratorId { get; set; }

        public PensionFundAdministratorViewModel PensionFundAdministrator { get; set; }

        public decimal FundRate { get; set; }

        public decimal FlowComissionRate { get; set; }

        public decimal MixedComissionRate { get; set; }

        public decimal DisabilityInsuranceRate { get; set; }

        public decimal EarlyRetirementRate { get; set; }
    }
}
