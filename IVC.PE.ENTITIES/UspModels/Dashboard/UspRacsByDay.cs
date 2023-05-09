using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Dashboard
{
    [NotMapped]
    public class UspRacsByDay
    {
        public DateTime ReportDate { get; set; }
        public int TotalRacs { get; set; }
    }
}
