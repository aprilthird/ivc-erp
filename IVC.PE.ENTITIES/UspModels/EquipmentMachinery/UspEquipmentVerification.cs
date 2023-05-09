using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
   public class UspEquipmentVerification
    {
        public string Equipment { get; set; }

        public string Model { get; set; }

        public string Year { get; set; } 

        public string Provider { get; set; }
    }
}
