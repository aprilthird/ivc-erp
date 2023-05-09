using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachinerySoftFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachinerySoftId { get; set; }

        public EquipmentMachinerySoft EquipmentMachinerySoft { get; set; }
        public string FreeText { get; set; }

        public DateTime FreeDate {get; set;}
    }
}
