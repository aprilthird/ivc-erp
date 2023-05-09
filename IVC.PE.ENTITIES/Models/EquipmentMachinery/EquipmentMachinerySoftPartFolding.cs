using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachinerySoftPartFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachinerySoftPartId { get; set; }

        public EquipmentMachinerySoftPart EquipmentMachinerySoftPart { get; set; }
        public string PartNumber { get; set; }
        public DateTime PartDate { get; set; }
        public Guid EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperator EquipmentMachineryOperator { get; set; }

        public string InitMileage { get; set; }

        public string EndMileage { get; set; }
        public Guid? EquipmentMachineryTypeSoftActivityId { get; set; }

        public EquipmentMachineryTypeSoftActivity EquipmentMachineryTypeSoftActivity { get; set; }

        public string Specific { get; set; }

        
    }


}
