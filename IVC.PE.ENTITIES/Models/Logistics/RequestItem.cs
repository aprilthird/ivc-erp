using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class RequestItem
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }
        public Request Request { get; set; }

        public Guid? PreRequestItemId { get; set; }
        public PreRequestItem PreRequestItem { get; set; }

        //public Guid? GoalBudgetInputId { get; set; }
        //public GoalBudgetInput GoalBudgetInput { get; set; }

        public Guid SupplyId { get; set; }
        public Supply Supply { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public double Measure { get; set; }

        public double MeasureInAttention { get; set; }

        public string Observations { get; set; }
    }
}
