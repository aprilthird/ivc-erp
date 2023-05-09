using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
   public class UspFuelTransportVal
    {
        public Guid Id { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public Guid TransportId { get; set; }

        public string TradeName { get; set; }

        public string Description { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }
        public string EquipmentPlate { get; set; }
        public string Username    { get; set; }
        public string ActualName { get; set; }
        public double? Monday  { get; set; }
        public double? Tuesday { get; set; }
        public double? Wednesday { get; set; }
        public double? Thursday { get; set; }
        public double? Friday { get; set; }
        public double? Saturday { get; set; }
        public double? Sunday { get; set; }
        public double TotalGallon { get; set; }
        public double InitMileage { get; set; }
        public double EndMileage { get; set; }
        public Guid FuelId { get; set; }
        public double Dif { get; set; }
        public double KGallon { get; set; }
        public double GKilometer{ get; set; }

        public int ServiceCondition { get; set; }

        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];

    }
}
