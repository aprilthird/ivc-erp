using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedBudgetInputViewModels
{
    public class ConsolidatedBudgetInputViewModel
    {
        public Guid? Id { get; set; }

        public Guid SupplyFamilyId { get; set; }

        public SupplyFamilyViewModel SupplyFamily { get; set; }

        public Guid SupplyGroupId { get; set; }

        public SupplyGroupViewModel SupplyGroup { get; set; }

        public string NumberItem { get; set; }

        public string Description { get; set; }

        public string ContractualAmount { get; set; }

        public string DeductiveAmount1 { get; set; }

        public string DeductiveAmount2 { get; set; }

        public string DeductiveAmount3 { get; set; }

        public string Deductives { get; set; }

        public string NetContractual { get; set; }

        public string AdicionalAmount1 { get; set; }

        public string AdicionalAmount2 { get; set; }

        public string AdicionalAmount3 { get; set; }

        public string Adicionals { get; set; }

        public string AccumulatedAmount { get; set; }
    }
}
