using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class Stock
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        /*
        public string Code { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public int QuantityMinimum { get; set; }
        public int CurrencyType { get; set; }
        public decimal SalePriceUnit { get; set; }
        */

        public Guid SupplyId { get; set; }

        public Supply Supply { get; set; }

        public double Measure { get; set; }

        public double MinimumMeasure { get; set; }

        public double UnitPrice { get; set; }

        public double Parcial { get; set; }
    }
}
