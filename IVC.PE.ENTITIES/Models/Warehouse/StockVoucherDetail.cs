using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class StockVoucherDetail
    {
        public Guid Id { get; set; }

        public Guid StockVoucherId { get; set; }
        public StockVoucher StockVoucher { get; set; }

        public Guid StockId { get; set; }
        public Stock Stock { get; set; }

        public int? CurrencyType { get; set; }
        public decimal? SalePriceUnit { get; set; }
        public int Quantity { get; set; }
    }
}
