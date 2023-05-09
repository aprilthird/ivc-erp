using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachineryTransportPartPlus
    {

        public Guid Id { get; set; }

        public Guid EquipmentMachineryTransportPartFoldingId { get; set; }

        public EquipmentMachineryTransportPartFolding EquipmentMachineryTransportPartFolding { get; set; }

        public Guid EquipmentMachineryTypeTransportActivityId { get; set; }
        public EquipmentMachineryTypeTransportActivity EquipmentMachineryTypeTransportActivity { get; set; }

    }
}
