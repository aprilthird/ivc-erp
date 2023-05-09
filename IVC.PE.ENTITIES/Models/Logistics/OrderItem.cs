using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid SupplyId { get; set; }

        public Supply Supply { get; set; }

        public string Glosa { get; set; }

        public double Measure { get; set; }

        public double MeasureInAttention { get; set; }

        public double UnitPrice { get; set; }

        public double Parcial { get; set; }

        public string Observations { get; set; }

        public double FinancialDiscount { get; set; }

        public double ItemDiscount { get; set; }

        public double AdditionalDiscount { get; set; }

        public double IGV { get; set; }

        public double ISC { get; set; }

        public double DiscountedUnitPrice { get; set; }
    }
}
