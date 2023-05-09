using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspEquipmentTransportDetailFolding
    {
        public Guid EquipmentProviderId { get; set; }

        public string TradeName { get; set; }

        public int Order { get; set; }

        public DateTime PartDate { get; set; }

        public string PartDateString => PartDate.ToDateString();

        public string PartNumber { get; set; }

        public double InitMileage { get; set; }

        public double EndMileage { get; set; }

        public double AcumulatedRoute { get; set; }

        public string ActualName { get; set; }

        public string UserName { get; set; }

        public Guid? ProjectPhaseId {get; set;}

        public string Code { get; set; }

        public string Activities { get; set; }

        public Guid FatherId { get; set; }

        public double? Gallon { get; set; }
    }
}
