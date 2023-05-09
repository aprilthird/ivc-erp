using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollParameter
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public decimal UIT { get; set; }

        public decimal MinimumWage { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal DollarExchangeRate { get; set; }

        public decimal MaximumInsurableRemuneration { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal SCTRRate { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal SCTRHealthFixed { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal SCTRPensionFixed { get; set; }

        public decimal EsSaludMasVidaCost { get; set; }

        public decimal UnionFee { get; set; }
    }
}
