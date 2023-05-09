using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.RdpItemViewModels
{
    public class RdpItemViewModel
    {
        public Guid Id { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public int ItemGroup { get; set; }
        public string ItemGroupStr => ConstantHelpers.Productions.RDPs.GROUPS[ItemGroup];

        public string ItemPhaseCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemUnit { get; set; }

        public decimal ItemContractualAmmount { get; set; }
        public decimal ItemStakeOutAmmount { get; set; }
    }
}
