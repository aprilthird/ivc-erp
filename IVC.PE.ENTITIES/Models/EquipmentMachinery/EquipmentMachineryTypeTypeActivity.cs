using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTypeTypeActivity
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryTypeTypeId { get; set; }

        public EquipmentMachineryTypeType EquipmentMachineryTypeType { get; set; }

        public string Description { get; set; }
    }
}
