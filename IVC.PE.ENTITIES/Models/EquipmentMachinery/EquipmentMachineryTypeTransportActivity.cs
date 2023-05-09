using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
  public class EquipmentMachineryTypeTransportActivity
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryTypeTransportId { get; set; }

        public EquipmentMachineryTypeTransport EquipmentMachineryTypeTransport { get; set; }
        public string Description { get; set; }
    }
}
