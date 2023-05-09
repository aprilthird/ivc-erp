using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class BudgetInputAllocationGroup
    {
        public Guid BudgetInputAllocationId { get; set; }

        public BudgetInputAllocation BudgetInputAllocation { get; set; }

        public Guid SewerGroupId { get; set; }

        public SewerGroup SewerGroup { get; set; }

        public double Measure { get; set; }
    }
}
