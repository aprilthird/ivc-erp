using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.StockInputVoucherViewModels
{
    public class StockInputVoucherDetailViewModel
    {
        public Guid? Id { get; set; }

        public Guid StockVoucherId { get; set; }
        public StockInputVoucherViewModel StockVoucher { get; set; }

        public Guid StockId { get; set; }
        public StockViewModel Stock { get; set; }

        public int? CurrencyType { get; set; }
        public string CurrencyTypeStr => ConstantHelpers.Currency.SIGN_VALUES[CurrencyType.Value];
        public decimal? SalePriceUnit { get; set; }
        public string SalePriceUnitStr => $"{CurrencyTypeStr} {SalePriceUnit.Value:0.##}";
        public int Quantity { get; set; }
    }
}
