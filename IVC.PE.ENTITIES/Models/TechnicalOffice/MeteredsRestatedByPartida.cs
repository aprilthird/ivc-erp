using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class MeteredsRestatedByPartida
    {
        public Guid Id { get; set; }

        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHead WorkFrontHead { get; set; }

        public Guid BudgetTitleId { get; set; }
        public BudgetTitle BudgetTitle { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public double Metered { get; set; }

        public double UnitPrice { get; set; }

        public double Parcial { get; set; }

        public double AccumulatedMetered { get; set; }

        public double AccumulatedAmount { get; set; }
    }
}
