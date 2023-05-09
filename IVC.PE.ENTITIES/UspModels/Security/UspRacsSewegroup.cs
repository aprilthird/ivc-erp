using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Security
{
    [NotMapped]
    public class UspRacsSewegroup
    {
        public string Code { get; set; }
        public string WorkFrontHead { get; set; }
        public string Foreman { get; set; }
        public string Username { get; set; }
        public int RacsTotal { get; set; }
        public int RacsInProgress { get; set; }
        public int RacsLifted { get; set; }
        public DateTime ReportDate { get; set; }
        public int RacsCondition { get; set; }
        public int RacsAct { get; set; }
    }
}
