using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using System;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels
{
    public class CurrentStockViewModel
    {
        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        public double EntriesFloat { get; set; }

        public string Entries { get; set; }

        public double RequestsFloat { get; set; }

        public string Requests { get; set; }

        public double MeasureFloat { get; set; }

        public string Measure { get; set; }

        public string Providers { get; set; }

        public string UnitPrice { get; set; }

        public double Parcial { get; set; }

        public string ParcialString { get; set; }

        public double totalEntry { get; set; }

        public double totalRequest { get; set; }
    }
}
