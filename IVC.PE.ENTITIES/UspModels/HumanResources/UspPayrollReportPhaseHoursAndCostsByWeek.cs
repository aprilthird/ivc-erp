using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    public class UspPayrollReportPhaseHoursAndCostsByWeek
    {
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public decimal TotalHours { get; set; }
        public decimal TotalHoursPa { get; set; }
        public decimal TotalHoursOp { get; set; }
        public decimal TotalHoursOf { get; set; }
        public decimal TotalCostsPa { get; set; }
        public decimal TotalCostsOp { get; set; }
        public decimal TotalCostsOf { get; set; }
        public decimal TotalCosts => TotalCostsPa + TotalCostsOf + TotalCostsOp;
    }
}
