using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ConsolidatedSteel
    {
        public Guid Id { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public Guid ProjectPhaseId { get; set; }

        public ProjectPhase ProjectPhase { get; set; }

        public Guid WorkFrontId { get; set; }

        public WorkFront WorkFront { get; set; }

        public int OrderNumber { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public double Metered { get; set; }

        public double ContractualMetered { get; set; }

        public double Rod6mm { get; set; }

        public double Rod8mm { get; set; }

        public double Rod3x8 { get; set; }

        public double Rod1x2 { get; set; }

        public double Rod5x8 { get; set; }

        public double Rod3x4 { get; set; }

        public double Rod1 { get; set; }

        public double ContractualStaked { get; set; }
    }
}
