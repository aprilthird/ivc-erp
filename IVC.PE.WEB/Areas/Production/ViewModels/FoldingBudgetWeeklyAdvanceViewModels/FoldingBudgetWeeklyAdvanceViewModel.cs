using IVC.PE.WEB.Areas.Production.ViewModels.WeeklyAdvanceViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.FoldingBudgetWeeklyAdvanceViewModels
{
    public class FoldingBudgetWeeklyAdvanceViewModel
    {
        public Guid? Id { get; set; }

        public Guid WeeklyAdvanceId { get; set; }

        public WeeklyAdvanceViewModel WeeklyAdvance { get; set; }

        public string NumberItem { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public string ContractualMO { get; set; }

        public string ContractualEQ { get; set; }

        public string ContractualSubcontract { get; set; }

        public string ContractualMaterials { get; set; }

        public string ActualAdvance { get; set; }
    }
}
