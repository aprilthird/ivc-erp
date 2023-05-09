using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Production
{
    [NotMapped]
    public class UspWeeklyAdvanceTotalWorkerCost
    {
        public Guid WorkerId { get; set; }
        public double TotalCost { get; set; }
    }
}
