using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.SupplyEntryViewModels
{
    public class SupplyEntryItemViewModel
    {
        public Guid? Id { get; set; }

        public Guid SupplyEntryId { get; set; }
        public SupplyEntryViewModel SupplyEntry { get; set; }

        public Guid OrderItemId { get; set; }
        public OrderItemViewModel OrderItem { get; set; }

        public string Measure { get; set; }

        public string Observations { get; set; }

        public double PreviousAttention { get; set; }

        public bool IsValued { get; set; }

        public bool isEditable { get; set; } = false;
    }
}
