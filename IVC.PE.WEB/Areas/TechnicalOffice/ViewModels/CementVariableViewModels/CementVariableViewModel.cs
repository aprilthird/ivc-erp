using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.CementVariableViewModels
{
    public class CementVariableViewModel
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

        [Display(Name = "Precio Unitario", Prompt = "Precio Unitario")]
        public string UnitPrice { get; set; }
    }
}
