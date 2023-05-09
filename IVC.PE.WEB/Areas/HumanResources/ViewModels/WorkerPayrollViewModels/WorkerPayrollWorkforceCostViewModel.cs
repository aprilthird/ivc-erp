using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels
{
    public class WorkerPayrollWorkforceCostViewModel
    {
        public string FileName { get; set; }
        public string WeekStartDate { get; set; }
        public string Document { get; set; }
        public string FullName { get; set; }
        public string CategoryName { get; set; }
        public string Position { get; set; }
        public string Phase { get; set; }
        public string Collaborator { get; set; }
        public string Responsable { get; set; }
        public string SewerGroup { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal EsSalud { get; set; }
        public decimal EarlyRetirementSPP { get; set; }
        public decimal SCTRSalud { get; set; }
        public decimal SCTRPension { get; set; }
        public decimal EsSaludVida { get; set; }
        public decimal TotalContribution { get; set; }
        public decimal TotalCost { get; set; }
    }
}
