using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class RdpItem
    {
        public Guid Id { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public int ItemGroup { get; set; }

        public string ItemPhaseCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemUnit { get; set; }

        public decimal ItemContractualAmmount { get; set; }
        public decimal ItemStakeOutAmmount { get; set; }
    }
}
