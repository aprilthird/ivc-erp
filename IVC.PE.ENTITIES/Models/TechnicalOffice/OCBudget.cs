using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class OCBudget
    {
        public Guid Id { get; set; }

        public string NumberItem { get; set; }

        public string Description { get; set; }

        public int Group { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public Guid? BudgetFormulaId { get; set; }

        public BudgetFormula BudgetFormula { get; set; }

        public Guid? BudgetTypeId { get; set; }

        public BudgetType BudgetType { get; set; }

        public double Quantity { get; set; }

        public string Unit { get; set; }

        public double Metered { get; set; }

        public double UnitPrice { get; set; }

        public double TotalPrice { get; set; }

        public int OrderNumber { get; set; }
    }
}
