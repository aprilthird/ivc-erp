using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspFuelTransportFolding
    {
        public Guid Id { get; set; }

        public DateTime PartDate { get; set; }
        public string PartDateString => PartDate.ToDateString();

        public double Gallon { get; set; }

        public int Order { get; set; }

        public double Mileage {get; set;}

        public string PartHour { get; set; }

        public string PartNumber { get; set; }

        public Guid FuelProviderId { get; set; }

        public string TradeName { get; set; }

        public Guid FuelProviderFoldingId { get; set; }

        public string CisternPlate { get; set; }

        public Guid? FuelProviderPriceFoldingId { get; set; }
        public double? Price { get; set; }

        public Guid EquipmentMachineryOperatorId { get; set; }

        public int HiringType { get; set; }

        public string OperatorName { get; set; }

        public string FromOtherName { get; set; }

        public string WorkerName { get; set; }

        public string WorkerMiddleName { get; set; }

        public string WorkerPaternalSurname { get; set; }

        public string WorkerMaternalSurname { get; set; }

        public int MonthPartDate { get; set; }

        public int YearPartDate { get; set; }

        public int WeekPartDate { get; set; }

        public Guid EquipmentMachineryFuelTransportPartId { get; set; }

        public Guid EquipmentMachineryTransportPartId { get; set; }


    }
}
