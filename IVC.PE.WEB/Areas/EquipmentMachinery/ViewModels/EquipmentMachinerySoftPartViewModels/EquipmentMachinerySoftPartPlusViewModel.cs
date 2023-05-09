using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftPartViewModels
{
    public class EquipmentMachinerySoftPartPlusViewModel
    {
        public Guid? Id { get; set; }

        public Guid EquipmentMachinerySoftPartFoldingId { get; set; }

        public EquipmentMachinerySoftPartFoldingViewModel EquipmentMachinerySoftPartFolding { get; set; }

        public Guid EquipmentMachineryTypeSoftActivityId { get; set; }

        public EquipmentMachineryTypeSoftActivityViewModel EquipmentMachineryTypeSoftActivity { get; set; }

        public string Specific { get; set; }

    }
}
