using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    public class UspPayrollReportPhaseWorkersByWeek
    {
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public decimal? TotalWorkersPa { get; set; }
        public decimal? TotalWorkersOp { get; set; }
        public decimal? TotalWorkersOf { get; set; }
    }
}
