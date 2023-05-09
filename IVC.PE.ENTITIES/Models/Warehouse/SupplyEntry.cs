using IVC.PE.ENTITIES.Models.Accounting;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class SupplyEntry
    {
        public Guid Id { get; set; }

        public int DocumentNumber { get; set; }

        public int Status { get; set; }

        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public Guid? InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string RemissionGuide { get; set; }
        public Uri RemissionGuideUrl { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public double Parcial { get; set; }

        public bool IsValued { get; set; }
        
        public int ValuedMonth { get; set; }

        public int ValuedYear { get; set; }

        public IEnumerable<SupplyEntryItem> SupplyEntryItems { get; set; }

    }
}
