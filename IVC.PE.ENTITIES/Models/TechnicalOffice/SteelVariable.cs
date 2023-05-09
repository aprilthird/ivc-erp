using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SteelVariable
    {
        public Guid Id { get; set; }

        public Guid? SupplyId { get; set; }

        public Supply Supply { get; set; }

        public Guid? BudgetInputId { get; set; }

        public BudgetInput BudgetInput { get; set; }

        public string RodDiameterInch { get; set; }

        public double RodDiameterMilimeters { get; set; }

        public int Section { get; set; }

        public double Perimeter { get; set; }

        public double NominalWeight { get; set; }

        public double PricePerRod { get; set; }
    }
}
