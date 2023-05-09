using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTypeSoftActivity
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryTypeSoftId { get; set; }

        public EquipmentMachineryTypeSoft EquipmentMachineryTypeSoft { get; set; }
        public string Description { get; set; }
    }
}
