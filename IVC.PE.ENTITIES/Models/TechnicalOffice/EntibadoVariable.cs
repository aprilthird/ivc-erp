using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class EntibadoVariable
    {
        public Guid Id { get; set; }

        public Guid? SupplyId { get; set; }

        public Supply Supply { get; set; }

        public Guid? BudgetInputId { get; set; }

        public BudgetInput BudgetInput { get; set; }

        public string Name { get; set; }

        public string Dimensions { get; set; }

        public double LDimension { get; set; }

        public double HDimension { get; set; }

        public double Thickness { get; set; }

        public double Weight { get; set; }

        public double FreeDitchTope { get; set; }

        public double FreeDitchFondo { get; set; }

        public double MaxDitch { get; set; }

        public double UnitPrice { get; set; }

        public double UseFactor { get; set; }

        public int Quantity { get; set; }

        public double RestatedPerformance { get; set; }
    }
}
