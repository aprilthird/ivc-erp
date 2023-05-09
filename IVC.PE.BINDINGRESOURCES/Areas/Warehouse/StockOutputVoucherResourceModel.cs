using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.Warehouse
{
    public class StockOutputVoucherResourceModel
    {
        public string RequestDate { get; set; }
        public Guid SewerGroupId { get; set; }
        public Guid ProjectPhaseId { get; set; }
        public string PickUpResponsible { get; set; }
        public string Observations { get; set; }
        public IEnumerable<StockOutputVoucherDetailResourceModel> VoucherDetails { get; set; }
    }

    public class StockOutputVoucherDetailResourceModel
    {
        public Guid StockId { get; set; }
        public int QuantityRequest { get; set; }
    }
}
