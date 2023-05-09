using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTransport
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public EquipmentProvider EquipmentProvider { get; set; }

        public Guid EquipmentProviderFoldingId { get; set; }
        public EquipmentProviderFolding EquipmentProviderFolding { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string EquipmentYear { get; set; }
        public string EquipmentPlate { get; set; }
        public string EquipmentSerie { get; set; }
        public DateTime? StartDate { get; set; }
        public int Status { get; set; }
        public int ServiceCondition { get; set; }
        public double UnitPrice { get; set; }

        public int InsuranceNumber { get; set; }

        public int SoatNumber { get; set; }

        public int TechincalNumber { get; set; }


    }
}
