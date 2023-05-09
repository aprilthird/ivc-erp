﻿using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspFuelMach
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }
        public int? YearPart { get; set;}
        public int? MonthPart { get; set;}
        public Guid? EquipmentProviderId { get; set; }

        public string TradeName { get; set; }

        public Guid? EquipmentMachPartId { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string Plate { get; set; }

        public string Year { get; set; }

        public int ServiceCondition { get; set; }

        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];


        public string Description { get; set; }

        public double? InitHorometer { get; set; }

        public double? EndHorometer { get; set; }

        public double? AcumulatedHorometer { get; set; }

        public double? RateConsume { get; set; }

        public double AcumulatedGallon { get; set; }

        public int FoldingNumber { get; set; }

        public Guid? FoldingId { get; set; }

        public int? MonthPartDate { get; set; }

        public int? YearPartDate { get; set; }
        public int? WeekPartDate { get; set; }

        public Guid? EquipmentMachineryTypeTypeId { get; set; }

        public int MachPartYear { get; set; }

        public int MachPartMonth { get; set; }
    }
}
