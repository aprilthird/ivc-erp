using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class CementVariable
    {
        public Guid Id { get; set; }

        public Guid? SupplyId { get; set; }

        public Supply Supply { get; set; }

        public Guid? BudgetInputId { get; set; }

        public BudgetInput BudgetInput { get; set; }

        public string Name { get; set; }

        public double UnitPrice { get; set; }
    }
}
