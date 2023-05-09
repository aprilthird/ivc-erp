using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspEquipmentMachinerySoftParts
    {
        public Guid SoftPartId { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectAbbreviation { get; set; }

        public Guid EquipmentMachineryTypeSoftId { get; set; }

        public string TypeSoft { get; set; }
        public Guid EquipmentProviderId { get; set; }
        public string TradeName { get; set; }

        public Guid EquipmentMachinerySoftId { get; set; }
        public string EquipmentSoftPlate { get; set; }
        public string EquipmentSoftModel { get; set; }
        public string EquipmentSoftBrand { get; set; }

        public string UserName { get; set; }
        public Guid ProjectPhaseId { get; set; }
        public string ProjectCode { get; set; }

        public Guid SoftPartFoldingId { get; set; }

        public int Order { get; set; }

        public string PartNumber { get; set; }

        public DateTime Partdate { get; set; }
        public string PardateString => $"{Partdate.ToDateString()}";

        public string InitMileage { get; set; }

        public string EndMileage { get; set; }

        public Guid EquipmentMachineryOperatorId { get; set; }

        public string OperatorName { get; set; }

        public string FromOtherName { get; set; }
    }
}
