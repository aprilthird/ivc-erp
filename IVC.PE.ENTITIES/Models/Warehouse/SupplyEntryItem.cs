using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class SupplyEntryItem
    {
        public Guid Id { get; set; }

        public Guid SupplyEntryId { get; set; }
        public SupplyEntry SupplyEntry { get; set; }

        public Guid OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }

        public double Measure { get; set; }

        public double PreviousAttention { get; set; }

        public string Observations { get; set; }

        public bool IsValued { get; set; }
    }
}
