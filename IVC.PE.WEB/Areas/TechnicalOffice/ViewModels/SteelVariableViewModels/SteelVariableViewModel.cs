using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SteelVariableViewModels
{
    public class SteelVariableViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Catálogo de Insumo")]
        public Guid? SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        [Display(Name = "Insumo Venta")]
        public Guid? BudgetInputId { get; set; }

        public BudgetInputViewModel BudgetInput { get; set; }

        [Display(Name = "Pulg.", Prompt = "Pulg.")]
        public string RodDiameterInch { get; set; }

        [Display(Name = "mm", Prompt = "mm")]
        public string RodDiameterMilimeters { get; set; }

        [Display(Name = "Sección (mm^2)", Prompt = "Sección (mm^2)")]
        public int Section { get; set; }

        [Display(Name = "Perimetro (mm)", Prompt = "Perimetro (mm)")]
        public string Perimeter { get; set; }

        [Display(Name = "Peso Nominal (kg/m)", Prompt = "Peso Nominal (kg/m)")]
        public string NominalWeight { get; set; }

        public string PricePerRod { get; set; }
    }
}
