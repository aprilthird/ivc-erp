using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.CementViewModels
{
    public class CementViewModel
    {
        public Guid? Id { get; set; }


        [Display(Name = "Titulos de Presupuesto", Prompt = "Titulos de Presupuestos")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Fórmula de Proyecto", Prompt = "Fórmula de Proyecto")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormuala { get; set; }

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

        [Display(Name = "Metrado Contractual Tipo I", Prompt = "Metrado Contractual Tipo I")]
        public string ContractualMeteredTypeOne { get; set; }

        [Display(Name = "Metrado Contractual Tipo V", Prompt = "Metrado Contractual Tipo V")]
        public string ContractualMeteredTypeFive { get; set; }

        [Display(Name = "Metrado Replanteado Tipo I", Prompt = "Metrado Replanteado Tipo I")]
        public string ContractualRestatedTypeOne { get; set; }

        [Display(Name = "Metrado Replanteado Tipo V", Prompt = "Metrado Replanteado Tipo V")]
        public string ContractualRestatedTypeFive { get; set; }
    }
}
