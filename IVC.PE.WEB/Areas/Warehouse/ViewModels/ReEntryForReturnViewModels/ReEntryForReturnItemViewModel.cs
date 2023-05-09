using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.ReEntryForReturnViewModels
{
    public class ReEntryForReturnItemViewModel
    {
        public Guid? Id { get; set; }

        public Guid ReEntryForReturnId { get; set; }
        public ReEntryForReturnViewModel ReEntryForReturn { get; set; }

        [Display(Name = "Insumo", Prompt = "Insumo")]
        public Guid GoalBudgetInputId { get; set; }
        public GoalBudgetInputViewModel GoalBudgetInput { get; set; }

        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        [Display(Name = "Cantidad", Prompt = "Cantidad")]
        public string Quantity { get; set; }

        [Display(Name = "Observación", Prompt = "Observación")]
        public string Observations { get; set; }
    }
}
