using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels
{
    public class WorkerPayrollPhaseSewerGroupCostViewModel
    {
        public string FileName { get; set; }
        public string WeekStartDate { get; set; }
        public string PhaseCode { get; set; }
        public string PhaseDescription { get; set; }
        public string Collaborator { get; set; }
        public string Responsable { get; set; }
        public string SewerGroup { get; set; }
        public decimal TotalCost { get; set; }
    }
}
