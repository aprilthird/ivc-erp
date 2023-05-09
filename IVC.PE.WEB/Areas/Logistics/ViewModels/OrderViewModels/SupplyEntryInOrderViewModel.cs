using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels
{
    public class SupplyEntryInOrderViewModel
    {
        public Guid Id { get; set; }
        public string DeliveryDate { get; set; }

        public string RemissionGuideName { get; set; }

        public string Parcial { get; set; }

        public string DolarParcial { get; set; }

        public Uri RemissionGuideUrl { get; set; }
    }
}
