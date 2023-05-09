using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Aggregation
{
    public class AggregationPrice
    {
        public Guid Id { get; set; }
        public Guid AggregationStockTypeId { get; set; }
        public AggregationStockType AggregationStockType { get; set; }
        public Guid AggregationStockId { get; set; }
        public AggregationStock AggregationStock { get; set; }
        public Guid AggregationEntryId { get; set; }
        public AggregationEntry AggregationEntry { get; set; }
        public double Price { get; set; }
    }
}
