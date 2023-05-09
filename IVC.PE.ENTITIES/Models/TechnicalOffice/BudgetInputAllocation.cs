using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class BudgetInputAllocation
    {
        public Guid Id { get; set; }

        public Guid? BudgetInputId { get; set; }

        public BudgetInput BudgetInput { get; set; }

        public Guid BudgetId { get; set; }
        
        public Budget Budget { get; set; }
        

        public int Type { get; set; } = ConstantHelpers.Budget.Type.CONTRACTUAL;

        public int Group { get; set; } = ConstantHelpers.Budget.Group.MAIN_COMPONENT;

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid BudgetFormulaId { get; set; }

        public BudgetFormula BudgetFormula { get; set; }

        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public double Measure { get; set; }

        public IEnumerable<BudgetInputAllocationGroup> BudgetInputAllocationGroups { get; set; }
    }
}
