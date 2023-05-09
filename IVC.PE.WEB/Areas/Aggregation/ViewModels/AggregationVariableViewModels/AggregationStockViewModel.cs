using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Aggregation.ViewModels.AggregationVariableViewModels
{
    public class AggregationStockViewModel
    {
        public Guid? Id { get; set; }
        public Guid AggregationStockTypeId { get; set; }
        public AggregationStockTypeViewModel AggregationStockType { get; set; }
        public string Description { get; set; }
        public Uri QuarryApprovalCertificate { get; set; }
    }
}
