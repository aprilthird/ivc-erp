using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.EntibadoVariableViewModels
{
    public class EntibadoVariableViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Catálogo de Insumo")]
        public Guid? SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        [Display(Name = "Insumo Venta")]
        public Guid? BudgetInputId { get; set; }

        public BudgetInputViewModel BudgetInput { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Dimensiones", Prompt = "Dimensiones")]
        public string Dimensions { get; set; }

        [Display(Name = "Dimensiones L (m)", Prompt = "Dimensiones L (m)")]
        public string LDimension { get; set; }

        [Display(Name = "Dimensiones h (m)", Prompt = "Dimensiones h (m)")]
        public string HDimension { get; set; }

        [Display(Name = "e (espesor, cm)", Prompt = "e (espesor, cm)")]
        public string Thickness { get; set; }

        [Display(Name = "Peso (kg)", Prompt = "Peso (kg)")]
        public string Weight { get; set; }

        [Display(Name = "Prof. Libre max. tope Zanja (m)", Prompt = "Prof. Libre max. tope Zanja (m)")]
        public string FreeDitchTope { get; set; }

        [Display(Name = "Prof. Libre max. fondo Zanja (m)", Prompt = "Prof. Libre max. fondo Zanja (m)")]
        public string FreeDitchFondo { get; set; }

        [Display(Name = "Prof. Max. Zanja (m)", Prompt = "Prof. Max. Zanja (m)")]
        public string MaxDitch { get; set; }

        [Display(Name = "Precio Unitario", Prompt = "Precio Unitario")]
        public string UnitPrice { get; set; }

        [Display(Name = "Factor de Uso", Prompt = "Factor de Uso")]
        public string UseFactor { get; set; }

        [Display(Name = "Cantidad de Juegos", Prompt = "Cantidad de Juegos")]
        public int Quantity { get; set; }

        [Display(Name = "Rendimiento Replanteo (ml x día)", Prompt = "Rendimiento Replanteo (ml x día)")]
        public string RestatedPerformance { get; set; }
    }
}
