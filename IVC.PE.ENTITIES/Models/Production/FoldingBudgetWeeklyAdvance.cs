using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class FoldingBudgetWeeklyAdvance
    {
        public Guid Id { get; set; }

        public Guid WeeklyAdvanceId { get; set; }

        public WeeklyAdvance WeeklyAdvance { get; set; }

        public string NumberItem { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public double ContractualMO { get; set; }

        public double ContractualEQ { get; set; }

        public double ContractualSubcontract { get; set; }

        public double ContractualMaterials { get; set; }

        public double ActualAdvance { get; set; }
    }
}
