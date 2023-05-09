using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachPart
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid EquipmentMachineryTypeTypeId { get; set; }
        public EquipmentMachineryTypeType EquipmentMachineryTypeType { get; set; }
        public Guid EquipmentProviderId { get; set; }
        public EquipmentProvider EquipmentProvider { get; set; }
        public Guid EquipmentMachId { get; set; }
        public EquipmentMach EquipmentMach { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }


        public int FoldingNumber { get; set; }


    }
}
