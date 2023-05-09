using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollReportPhaseCost
    {
        public string PhaseCode { get; set; }
        public string PhaseDescription { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Sunday { get; set; }
        public decimal Scholarship { get; set; }
        public decimal Holiday { get; set; }
        public decimal MedicalRest { get; set; }
        public decimal PaternityLeave { get; set; }
        public decimal He60 { get; set; }
        public decimal He100 { get; set; }
        public decimal Mobility { get; set; }
        public decimal Gratification { get; set; }
        public decimal ExtraordinaryBonification { get; set; }
        public decimal Vacations { get; set; }
        public decimal Cts { get; set; }
        public decimal Buc { get; set; }
        public decimal PaidLeave { get; set; }
        public decimal CtsHe { get; set; }
        public decimal EsSaludVida { get; set; }
        public string Afp { get; set; }
        public decimal SctrRate { get; set; }
        public string EpsRate { get; set; }
        public string EsSaludRate { get; set; }
    }
}
