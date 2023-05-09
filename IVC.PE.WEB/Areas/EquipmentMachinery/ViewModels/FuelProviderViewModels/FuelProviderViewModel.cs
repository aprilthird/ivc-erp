using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.FuelProviderViewModels
{
    public class FuelProviderViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid ProviderId { get; set; }

        public ProviderViewModel Provider { get; set; }

        public double LastPrice { get; set; }

    }
}
