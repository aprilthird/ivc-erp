using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels
{
    public class EquipmentProviderViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid ProviderId { get; set; }

        public ProviderViewModel Provider { get; set; }

        public string Descriptions { get; set; }


    }
}
