using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ConsolidatedAmountEntibado
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

        public double Performance { get; set; }

        public double KS60xMinibox { get; set; }

        public double KS100xKMC100 { get; set; }

        public double RealzaxExtension { get; set; }

        public double Corredera { get; set; }

        public double Paralelo { get; set; }
    }
}
