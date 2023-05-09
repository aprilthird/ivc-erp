using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels
{
    public class FieldRequestFoldingViewModel
    {
        public Guid? Id { get; set; }

        public Guid FieldRequestId { get; set; }
        public FieldRequestViewModel FieldRequest { get; set; }

        [Display(Name = "Insumo", Prompt = "Insumo")]
        public Guid GoalBudgetInputId { get; set; }
        public GoalBudgetInputViewModel GoalBudgetInput { get; set; }

        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        [Display(Name = "Cantidad", Prompt = "Cantidad")]

        public string Quantity { get; set; }

        public string ValidatedQuantity { get; set; }

        public string DeliveredQuantity { get; set; }

        public string Stock { get; set; }

        public string Techo { get; set; }

        public string Acumulado { get; set; }
    }
}
