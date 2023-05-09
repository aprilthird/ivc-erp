using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Production
{
    [NotMapped]
    public class UspWeeklyAdvanceTotalWorker
    {
        public int Totals { get; set; }
        public int Pawns { get; set; }
        public int Officials { get; set; }
        public int Operators { get; set; }
    }
}
