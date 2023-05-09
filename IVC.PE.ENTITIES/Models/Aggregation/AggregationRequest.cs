using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Aggregation
{
    public class AggregationRequest
    {
        public Guid Id { get; set; }
        public string RequestNumber { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }
        public Guid ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }
        public Guid AggregationStockId { get; set; }
        public AggregationStock AggregationStock { get; set; }
        public double Volume { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Turn { get; set; }
        public bool Status { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
