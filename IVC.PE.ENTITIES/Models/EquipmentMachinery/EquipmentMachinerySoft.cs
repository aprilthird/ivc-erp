using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachinerySoft
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

        public string Potency { get; set; }

        public string Year { get; set; }

        public string SerieNumber { get; set; }

        public string EquipmentPlate { get; set; }

        public DateTime? StartDate { get; set; }


        public int Status { get; set; }

        public double UnitPrice { get; set; }

        public int InsuranceNumber { get; set; }

        public int FoldingNumber { get; set; }

        public string FreeText { get; set; }
    }
}
