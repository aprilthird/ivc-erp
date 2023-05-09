using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PackageViewModels
{
    public class PackageDetailsViewModel
    {
        public Guid ProviderId { get; set; }

        public ProviderViewModel Provider{ get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public string Parcial { get; set; }

        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

    }
}
