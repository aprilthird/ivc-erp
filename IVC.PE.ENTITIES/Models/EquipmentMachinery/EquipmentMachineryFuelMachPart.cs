using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachineryFuelMachPart
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }
        public Guid EquipmentProviderId { get; set; }

        public EquipmentProvider EquipmentProvider { get; set; }

        public Guid EquipmentMachPartId { get; set; }
        public EquipmentMachPart EquipmentMachPart { get; set; }

        public double AcumulatedGallon { get; set; }

        public int FoldingNumber { get; set; }
    }
}
