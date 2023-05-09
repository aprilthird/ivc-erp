using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DiverseInputViewModels
{
    public class DiverseInputViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Frente de Trabajo", Prompt = "Frente de Trabajo")]
        public Guid WorkFrontId { get; set; }

        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid ProjectPhaseId { get; set; }

        public ProjectPhaseViewModel ProjectPhase { get; set; }

        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Títulos de Presupuesto", Prompt = "Títulos de Presupuesto")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Catálogo de Insumos", Prompt = "Catálogo de Insumos")]
        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        [Display(Name = "Insumos Venta", Prompt = "Insumos Venta")]
        public Guid? BudgetInputId { get; set; }

        public BudgetInputViewModel BudgetInput { get; set; }

        [Display(Name = "Item", Prompt = "Item")]
        public string ItemNumber { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Unidad", Prompt = "Unidad")]
        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnitViewModel MeasurementUnit { get; set; }


        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Metered { get; set; }

        [Display(Name = "P. U.", Prompt = "P. U.")]
        public string UnitPrice { get; set; }

        [Display(Name = "Parcial", Prompt = "Parcial")]
        public string Parcial { get; set; }
    }
}
