using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryFuelTransportPartFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryFuelTransportPartId { get; set; }

        public EquipmentMachineryFuelTransportPart EquipmentMachineryFuelTransportPart { get; set; }

        public Guid FuelProviderFoldingId { get; set; }

        public FuelProviderFolding FuelProviderFolding { get; set; }


        public Guid? FuelProviderPriceFoldingId { get; set; }

        public FuelProviderPriceFolding FuelProviderPriceFolding { get; set; }


        public string PartNumber { get; set; }
        public Guid FuelProviderId { get; set; }

        public FuelProvider FuelProvider { get; set; }

        public DateTime PartDate { get; set; }

        public string PartHour { get; set; }

        public Guid EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperator EquipmentMachineryOperator { get; set; }

        public double Mileage { get; set; } 
        public double Gallon { get; set; }

        public int Order { get; set; }


    }
}
