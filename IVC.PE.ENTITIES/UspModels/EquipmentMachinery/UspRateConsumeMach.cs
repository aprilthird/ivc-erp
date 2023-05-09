using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    public class UspRateConsumeMach
    {
        public int Order { get; set; }

        public int WeekNumber { get; set; }

        public double TotalGallon { get; set; }

        public double HorometerMax { get; set; }

        public double HorometerMin { get; set; }
    }
}
