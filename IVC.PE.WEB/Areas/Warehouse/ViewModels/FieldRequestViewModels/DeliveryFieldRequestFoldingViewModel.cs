using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels
{
    public class DeliveryFieldRequestFoldingViewModel
    {
        public Guid FieldRequestId { get; set; }
        public FieldRequestViewModel FieldRequest { get; set; }

        public Guid GoalBudgetInputId { get; set; }
        public GoalBudgetInputViewModel GoalBudgetInput { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public string DeliveredQuantity { get; set; }

        public string Parcial { get; set; }

    }
}
