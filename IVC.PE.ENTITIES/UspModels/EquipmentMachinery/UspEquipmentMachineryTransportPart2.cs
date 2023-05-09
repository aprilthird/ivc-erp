using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspEquipmentMachineryTransportPart2
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectAbbreviation { get; set; }

        public Guid EquipmentMachineryTypeTransportId { get; set; }

        public string Description { get; set; }

        public Guid EquipmentProviderId { get; set; }

        public string TradeName { get; set; }

        public Guid EquipmentMachineryTransportId { get; set; }

        public string EquipmentPlate { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }

        public int Status { get; set; }
        public string StatusDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_STATUS[Status];

        public int ServiceCondition { get; set; }
        public string ServiceConditionDesc => ConstantHelpers.EquipmentMachinery.EquipmentMachineryOperator.MACHINERY_SERVICE_CONDITION[ServiceCondition];

        public Guid? FoldingId { get; set; }


        public int? MonthPartDate { get; set; }

        public int? YearPartDate { get; set; }
    }
}
