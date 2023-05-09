using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class Cement
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

        public double ContractualMeteredTypeOne { get; set; }

        public double ContractualMeteredTypeFive { get; set; }

        public double ContractualRestatedTypeOne { get; set; }

        public double ContractualRestatedTypeFive { get; set; }
    }
}
