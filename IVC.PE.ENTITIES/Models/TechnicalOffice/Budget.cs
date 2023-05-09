using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class Budget
    {
        public Guid Id { get; set; }

        public int OrderNumber { get; set; }

        public string NumberItem { get; set; }

        public int Type { get; set; }

        public int Group { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid ProjectFormulaId  {get; set;}

        public ProjectFormula ProjectFormula { get; set; }

        public Guid? BudgetTypeId { get; set; }

        public BudgetType BudgetType { get; set; }

        public string Unit { get; set; }

        public double Metered { get; set; }

        public double UnitPrice { get; set; }

        public double TotalPrice { get; set; }

        public string Description { get; set; }
        public double ContractualMO { get; set; }
        public double ContractualEQ { get; set; }
        public double ContractualSubcontract { get; set; }
        public double ContractualMaterials { get; set; }
        public double CollaboratorMO { get; set; }
        public double CollaboratorEQ { get; set; }
    }
}
