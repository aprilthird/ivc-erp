using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollPensionFundAdministratorRate
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PensionFundAdministratorId { get; set; }

        public PensionFundAdministrator PensionFundAdministrator { get; set; }

        [Required]
        public Guid PayrollMovementHeaderId { get; set; }

        public PayrollMovementHeader PayrollMovementHeader { get; set; }

        public decimal FundRate { get; set; }

        public decimal FlowComissionRate { get; set; }

        public decimal MixedComissionRate { get; set; }

        public decimal DisabilityInsuranceRate { get; set; }

        public decimal EarlyRetirementRate { get; set; }
    }
}
