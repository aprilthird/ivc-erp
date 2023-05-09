using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachinerySoftPart
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid EquipmentMachineryTypeSoftId { get; set; }
        public EquipmentMachineryTypeSoft EquipmentMachineryTypeSoft { get; set; }
        public Guid EquipmentProviderId { get; set; }
        public EquipmentProvider EquipmentProvider { get; set; }
        public Guid EquipmentMachinerySoftId { get; set; }
        public EquipmentMachinerySoft EquipmentMachinerySoft { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}
