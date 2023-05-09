using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachineryTransportPart
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid EquipmentMachineryTypeTransportId { get; set; }
        public EquipmentMachineryTypeTransport EquipmentMachineryTypeTransport { get; set; }
        public Guid EquipmentProviderId { get; set; }
        public EquipmentProvider EquipmentProvider { get; set; }
        public Guid EquipmentMachineryTransportId { get; set; }
        public EquipmentMachineryTransport EquipmentMachineryTransport { get; set; }

        public int FoldingNumber { get; set; }
        public double LastInitMileage { get; set; }

        public double LastEndMileage { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

    }
}
