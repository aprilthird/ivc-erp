using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentProviderFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public EquipmentProvider EquipmentProvider { get; set; }

        public Guid EquipmentMachineryTypeId { get; set; }

        public EquipmentMachineryType EquipmentMachineryType { get; set; }
        public Guid? EquipmentMachineryTypeSoftId { get; set; }

        public EquipmentMachineryTypeSoft EquipmentMachineryTypeSoft { get; set; }

        public Guid? EquipmentMachineryTypeTypeId { get; set; }
        public EquipmentMachineryTypeType EquipmentMachineryTypeType { get; set; }

        public Guid? EquipmentMachineryTypeTransportId { get; set; }
        public EquipmentMachineryTypeTransport EquipmentMachineryTypeTransport { get; set; }
    }
}
