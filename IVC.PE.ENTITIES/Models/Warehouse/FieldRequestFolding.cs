using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class FieldRequestFolding
    {
        public Guid Id { get; set; }

        public Guid FieldRequestId {get; set; }
        public FieldRequest FieldRequest { get; set; }

        public Guid GoalBudgetInputId { get; set; }
        public GoalBudgetInput GoalBudgetInput { get; set; }
   
        public Guid ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public double Quantity { get; set; }

        public double ValidatedQuantity { get; set; }

        public double DeliveredQuantity { get; set; }

    }
}
