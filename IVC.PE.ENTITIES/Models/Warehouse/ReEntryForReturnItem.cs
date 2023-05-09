using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class ReEntryForReturnItem
    {
        public Guid Id { get; set; }

        public Guid ReEntryForReturnId { get; set; }
        public ReEntryForReturn ReEntryForReturn { get; set; }

        public Guid GoalBudgetInputId { get; set; }
        public GoalBudgetInput GoalBudgetInput { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public double Quantity { get; set; }

        public string Observations { get; set; }
    }
}
