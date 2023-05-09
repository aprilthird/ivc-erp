using IVC.PE.ENTITIES.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryFuelMachPartFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryFuelMachPartId { get; set; }

        public EquipmentMachineryFuelMachPart EquipmentMachineryFuelMachPart { get; set; }

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

        public double Horometer { get; set; }
        public double Gallon { get; set; }

        public int Order { get; set; }

        public Guid? ProjectCalendarWeekId { get; set; }

        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }
    }
}
