using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollReportPhaseHoursAndCostsByWeekViewModel
    {
        public string FileName { get; set; }
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public string SewerGroupCode { get; set; }
        public string Collaborator { get; set; }
        public string Responsable { get; set; }
        public decimal TotalHours { get; set; }
        public decimal TotalHoursPa { get; set; }
        public decimal TotalHoursOp { get; set; }
        public decimal TotalHoursOf { get; set; }
        public decimal TotalCosts { get; set; }
        public decimal TotalCostsPa { get; set; }
        public decimal TotalCostsOp { get; set; }
        public decimal TotalCostsOf { get; set; }
    }
}
