using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ConsolidatedBudgetInput
    {
        public Guid Id { get; set; }

        public Guid? SupplyFamilyId { get; set; }

        public SupplyFamily SupplyFamily { get; set; }

        public Guid? SupplyGroupId { get; set; }

        public SupplyGroup SupplyGroup { get; set; }

        public string NumberItem { get; set; }

        public string Description { get; set; }

        public double ContractualAmount { get; set; }

        public double DeductiveAmount1 { get; set; }

        public double DeductiveAmount2 { get; set; }

        public double DeductiveAmount3 { get; set; }

        public double Deductives { get; set; }

        public double NetContractual { get; set; }

        public double AdicionalAmount1 { get; set; }

        public double AdicionalAmount2 { get; set; }

        public double AdicionalAmount3 { get; set; }

        public double Adicionals { get; set; }

        public double AccumulatedAmount { get; set; }
    }
}
