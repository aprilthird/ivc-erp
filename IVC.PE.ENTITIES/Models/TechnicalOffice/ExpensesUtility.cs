using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ExpensesUtility
    {
        public Guid Id { get; set; }

        public int Group { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public Guid? BudgetFormulaId { get; set; }

        public BudgetFormula BudgetFormula { get; set; }

        public Guid? BudgetTypeId { get; set; }

        public BudgetType BudgetType { get; set; }

        public double FixedGeneralExpense { get; set; }

        public double FixedGeneralPercentage { get; set; }

        public double VariableGeneralExpense { get; set; }

        public double VariableGeneralPercentage { get; set; }

        public double TotalGeneralExpense { get; set; }

        public double TotalGeneralPercentage { get; set; }

        public double Utility { get; set; }

        public double UtilityPercentage { get; set; }

        public Uri FileUrl { get; set; }
    }
}
