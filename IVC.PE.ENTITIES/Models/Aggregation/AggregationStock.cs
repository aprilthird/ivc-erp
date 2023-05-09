using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Aggregation
{
    public class AggregationStock
    {
        public Guid Id { get; set; }
        public Guid AggregationStockTypeId { get; set; }
        public AggregationStockType AggregationStockType { get; set; }
        public string Description { get; set; }
        public Uri QuarryApprovalCertificate { get; set; }
    }
}
