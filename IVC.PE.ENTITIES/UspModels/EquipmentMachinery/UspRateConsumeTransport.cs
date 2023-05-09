using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspRateConsumeTransport
    {
        public int Order { get; set; }

        public int WeekNumber { get; set; }

        public double TotalGallon { get; set; }

        public double MileageMax { get; set; }

        public double MileageMin { get; set; }
    }
}
