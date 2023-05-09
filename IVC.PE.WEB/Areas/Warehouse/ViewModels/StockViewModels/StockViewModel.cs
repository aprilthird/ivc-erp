using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels
{
    public class StockViewModel
    {
        public Guid? Id { get; set; }
        /*
        public string Code { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int QuantityMinimum { get; set; }
        public int CurrencyType { get; set; }
        public string CurrencyTypeStr => ConstantHelpers.Currency.SIGN_VALUES[CurrencyType];
        public decimal SalePriceUnit { get; set; }
        public string SalePriceUnitStr => $"{CurrencyTypeStr} {SalePriceUnit:0.##}";

        public List<String> Responsibles { get; set; }
        */
        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        public string Measure { get; set; }

        public double MeasureFloat { get; set; }

        [Display(Name = "Mínimo")]
        public string MinimumMeasure { get; set; }

        public string Providers { get; set; }

        public string UnitPrice { get; set; }

        public double Parcial { get; set; }

        public string ParcialString { get; set; }

        public string Income { get; set; }

        public double IncomeFloat { get; set; }

        public string Dispatch { get; set; }

        public double DispatchFloat { get; set; }

        public double totalEntries { get; set; }

        public double totalDispatch { get; set; }
    }
}
