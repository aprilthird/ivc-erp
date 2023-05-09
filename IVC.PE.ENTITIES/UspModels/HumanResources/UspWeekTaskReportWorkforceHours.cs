using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWeekTaskReportWorkforceHours
    {
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public int TotalWorkers { get; set; }
        public int TotalWorkersPa { get; set; }
        public int TotalWorkersOp { get; set; }
        public int TotalWorkersOf { get; set; }
        public decimal TotalHours { get; set; }
        public decimal? TotalHoursPa { get; set; }
        public decimal? TotalHoursOp { get; set; }
        public decimal? TotalHoursOf { get; set; }
    }
}
