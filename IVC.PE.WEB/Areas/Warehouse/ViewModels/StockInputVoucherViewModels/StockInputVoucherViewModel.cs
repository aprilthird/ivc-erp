using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.StockInputVoucherViewModels
{
    public class StockInputVoucherViewModel
    {
        public Guid? Id { get; set; }

        public int VoucherType { get; set; }

        public string VoucherDate { get; set; }

        public string ReferencePurchaseOrder { get; set; }
        public string Supplier { get; set; }
    }
}
