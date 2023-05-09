using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
   public class UspEquipmentMachApp
    {
        public Guid EquipmentMachId { get; set; }

        public Guid EquipmentproviderId { get; set; }

        public Guid EquipmentMachineryTypeTypeId { get; set; }
    }
}
