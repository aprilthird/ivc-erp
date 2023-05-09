using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels
{
    public class DeliveryFieldRequestViewModel
    {
        public Guid? Id { get; set; }


        public Guid GoalBudgetInputId { get; set; }
        public GoalBudgetInputViewModel GoalBudgetInput { get; set; }

        public string DeliveredQuantity { get; set; }

        public string MeasureToRequest { get; set; }

        public string Techo { get; set; }

        public double Cost { get; set; }

        public string CostString { get; set; }
    }
}
