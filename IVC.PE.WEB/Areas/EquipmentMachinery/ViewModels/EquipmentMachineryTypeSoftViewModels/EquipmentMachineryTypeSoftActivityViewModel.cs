using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels
{
    public class EquipmentMachineryTypeSoftActivityViewModel
    {
        public Guid? Id { get; set; }

        public Guid? EquipmentMachineryTypeSoftId { get; set; }
        public EquipmentMachineryTypeSoftViewModel EquipmentMachineryTypeSoftViewModel { get; set; }
        public string Description { get; set; }
    }
}
