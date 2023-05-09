using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConsolidatedAmountSteelViewModels
{
    public class ConsolidatedAmountSteelViewModel
    {
        public Guid? Id { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Fase de Proyecto")]
        public Guid ProjectPhaseId { get; set; }

        public ProjectPhaseViewModel ProjectPhase { get; set; }

        [Display(Name = "Frente de Trabajo")]
        public Guid WorkFrontId { get; set; }

        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "N° Item", Prompt = "N° Item")]
        public string ItemNumber { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Unidad")]
        public string Unit { get; set; }

        [Display(Name = "Metrado")]
        public string Metered { get; set; }

        [Display(Name = "Metrado Contractual (S/)", Prompt = "Metrado Contractual")]
        public string ContractualMetered { get; set; }

        [Display(Name = "6mm", Prompt = "6mm")]
        public string Rod6mm { get; set; }

        [Display(Name = "8mm", Prompt = "8mm")]
        public string Rod8mm { get; set; }

        [Display(Name = "3/8\"", Prompt = "3/8\"")]
        public string Rod3x8 { get; set; }

        [Display(Name = "1/2\"", Prompt = "1/2\"")]
        public string Rod1x2 { get; set; }

        [Display(Name = "5/8\"", Prompt = "5/8\"")]
        public string Rod5x8 { get; set; }

        [Display(Name = "3/4\"", Prompt = "3/4\"")]
        public string Rod3x4 { get; set; }

        [Display(Name = "1\"", Prompt = "1\"")]
        public string Rod1 { get; set; }

        [Display(Name = "Metrado Replanteo (S/)", Prompt = "Metrado Replanteo")]
        public string ContractualStaked { get; set; }
    }
}
