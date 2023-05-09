using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels
{
    public class BudgetInputViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código S10", Prompt = "Código S10")]
        public string Code { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        public string FullDescription => $"{Code} - {Description}";

        [Display(Name = "Unidad de Medida", Prompt = "Unidad de Medida")]
        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnitViewModel MeasurementUnit { get; set; }

        [Display(Name = "Familia", Prompt = "Familia")]
        public Guid SupplyFamilyId { get; set; }

        public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Display(Name = "Grupo", Prompt = "Grupo")]
        public Guid? SupplyGroupId { get; set; }

        public SupplyGroupViewModel SupplyGroup { get; set; }

        [Display(Name = "P.U. (Venta)", Prompt = "Precio")]
        public string? SaleUnitPrice { get; set; }

        [Display(Name = "P.U. (Meta)", Prompt = "Precio")]
        public string? GoalUnitPrice { get; set; }

        [Display(Name = "Tipo de Presupuesto", Prompt = "Tipo de Presupuesto")]
        public Guid? BudgetTypeId { get; set; }

        [Display(Name = "Agrupación de Presupuesto", Prompt = "Agrupación de Presupuesto")]
        public int Group { get; set; }

        [Display(Name = "Título de Presupuesto", Prompt = "Título de Presupuesto")]
        public Guid? BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Fórmula de Presupuesto", Prompt = "Fórmula de Presupuesto")]
        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Metered { get; set; }

        [Display(Name = "Parcial", Prompt = "Parcial")]
        public string Parcial { get; set; }
    }
}
