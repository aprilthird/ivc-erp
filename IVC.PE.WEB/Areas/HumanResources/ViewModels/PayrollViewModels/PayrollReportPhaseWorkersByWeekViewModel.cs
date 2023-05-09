﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollReportPhaseWorkersByWeekViewModel
    {
        public string FileName { get; set; }
        public string PhaseCode { get; set; }
        public string Description { get; set; }
        public string SewerGroupCode { get; set; }
        public string Collaborator { get; set; }
        public string Responsable { get; set; }
        public decimal TotalWorkersPa { get; set; }
        public decimal TotalWorkersOp { get; set; }
        public decimal TotalWorkersOf { get; set; }
        public decimal TotalWorkers => TotalWorkersPa + TotalWorkersOp + TotalWorkersOf;
    }
}
