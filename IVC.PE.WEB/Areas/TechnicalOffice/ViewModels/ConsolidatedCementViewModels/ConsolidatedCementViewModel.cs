using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedCementViewModels
{
    public class ConsolidatedCementViewModel
    {
        public Guid? Id { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        public Guid ProjectPhaseId { get; set; }

        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public Guid WorkFrontId { get; set; }

        public WorkFrontViewModel WorkFront { get; set; }

        public int OrderNumber { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public string Metered { get; set; }

        public string ContractualMeteredTypeOne { get; set; }

        public string ContractualMeteredTypeFive { get; set; }

        public string ContractualRestatedTypeOne { get; set; }

        public string ContractualRestatedTypeFive { get; set; }
    }
}
