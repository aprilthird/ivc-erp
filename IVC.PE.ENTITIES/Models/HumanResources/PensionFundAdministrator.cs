using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PensionFundAdministrator
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        public decimal FundRate { get; set; }

        public decimal FlowComissionRate { get; set; }

        public decimal MixedComissionRate { get; set; }

        public decimal DisabilityInsuranceRate { get; set; }

        public decimal EarlyRetirementRate { get; set; }

        public string SunatCode { get; set; }

        //public IEnumerable<Employee> Users { get; set; }
    }
}
