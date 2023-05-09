using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryOperator
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string FromOtherName { get; set; }

        public string FromOtherPhone { get; set; }

        public string FromOtherDNI { get; set; }

        public string OperatorName { get; set; }

        public string PhoneOperator { get; set; }

        public string DNIOperator { get; set; }

        public int HiringType { get; set; }

        public Guid EquipmentMachineryTypeId { get; set; }

        public EquipmentMachineryType EquipmentMachineryType { get; set; }



        public Guid? WorkerId { get; set; }

        public Worker Worker { get; set; }

        public Guid? EquipmentMachineryTypeSoftId { get; set; }

        public EquipmentMachineryTypeSoft EquipmentMachineryTypeSoft { get; set; }

        public Guid? EquipmentMachineryTypeTypeId { get; set; }

        public EquipmentMachineryTypeType EquipmentMachineryTypeType { get; set; }

        public Guid? EquipmentMachineryTypeTransportId { get; set; }

        public EquipmentMachineryTypeTransport EquipmentMachineryTypeTransport { get; set; }

        public DateTime? StartDate { get; set; } = DateTime.UtcNow;

        public Uri FileUrl { get; set; }

        public string ActualName { get; set; }

        public string ActualDni { get; set; }
    }
}
