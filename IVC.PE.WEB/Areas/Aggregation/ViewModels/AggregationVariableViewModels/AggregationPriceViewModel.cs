using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Aggregation.ViewModels.AggregationVariableViewModels
{
    public class AggregationPriceViewModel
    {
        public Guid? Id { get; set; }
        public Guid AggregationStockTypeId { get; set; }
        public AggregationStockTypeViewModel AggregationStockType { get; set; }
        public Guid AggregationStockId { get; set; }
        public AggregationStockViewModel AggregationStock { get; set; }
        public Guid AggregationEntryId { get; set; }
        public AggregationEntryViewModel AggregationEntry { get; set; }
        public string Price { get; set; }
    }
}
