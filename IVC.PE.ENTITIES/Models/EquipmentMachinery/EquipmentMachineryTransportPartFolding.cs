using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachineryTransportPartFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryTransportPartId { get; set; }

        public EquipmentMachineryTransportPart EquipmentMachineryTransportPart { get; set; }
        public string PartNumber { get; set; }
        public DateTime PartDate { get; set; }
        public Guid? EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperator EquipmentMachineryOperator { get; set; }

        public int Order { get; set; }
        public double InitMileage { get; set; }

        public double EndMileage { get; set; }
        public Guid? EquipmentMachineryTypeTransportActivityId { get; set; }

        public EquipmentMachineryTypeTransportActivity EquipmentMachineryTypeTransportActivity { get; set; }

        public string Specific { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        
        public int WorkArea { get; set; }


        public Guid? TransportPhaseId { get; set; }
    
        public TransportPhase TransportPhase { get; set; }
    }
}
