using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachinerySoftPartPlus
    {
        public Guid Id { get; set; }
        
        public Guid EquipmentMachinerySoftPartFoldingId { get; set; }

        public EquipmentMachinerySoftPartFolding EquipmentMachinerySoftPartFolding { get; set; }

        public Guid EquipmentMachineryTypeSoftActivityId { get; set; }
        public EquipmentMachineryTypeSoftActivity EquipmentMachineryTypeSoftActivity { get; set; }


    }
}
