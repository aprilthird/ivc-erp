using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Dashboard
{
    [NotMapped]
    public class UspBondsActive
    {
        public string BondNumber { get; set; }
        public double PenAmmount { get; set; }
    }
}
