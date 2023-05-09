using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachineryPart
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectAbbreviation { get; set; }

        public Guid EquipmentMachineryTypeTypeId { get; set; }

        public string Description { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public string TradeName { get; set; }

        public Guid EquipmentMachId { get; set; }

        public string Plate { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public string MonthDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MONTH[Month];


        public int Status { get; set; }
        public string StatusDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS[Status];

        public int ServiceCondition { get; set; }
        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];

        public Guid? FoldingId { get; set; }

        public Guid TypeFilter { get; set; }

        public int? MonthPartDate { get; set; }

        public int? YearPartDate { get; set; }
    
        public int? WeekDate { get; set; }

        public int? WeekDateFilter { get; set; }


    }
}
