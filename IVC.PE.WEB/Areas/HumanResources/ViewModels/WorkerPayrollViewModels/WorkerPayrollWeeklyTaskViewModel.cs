using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels
{
    public class WorkerPayrollWeeklyTaskViewModel
    {
        public string FileName { get; set; }
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public string SewerGroupCode { get; set; }
        public string Collaborator { get; set; }
        public string Responsable { get; set; }
        public int TotalWorkers { get; set; }
        public int TotalWorkersPa { get; set; }
        public int TotalWorkersOp { get; set; }
        public int TotalWorkersOf { get; set; }
        public decimal TotalHours { get; set; }
        public decimal TotalHoursPa { get; set; }
        public decimal TotalHoursOp { get; set; }
        public decimal TotalHoursOf { get; set; }
    }
}
