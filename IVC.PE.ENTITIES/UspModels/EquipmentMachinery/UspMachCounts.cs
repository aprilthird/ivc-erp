using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachCounts
    {
        public int Quantity { get; set; }

        public string Transport { get; set; }
    }
}
