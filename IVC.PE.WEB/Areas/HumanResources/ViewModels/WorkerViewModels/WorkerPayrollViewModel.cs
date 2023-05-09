using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.UspModels.HumanResources;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementDetailViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollWorkerVariableViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class WorkerPayrollViewModel
    {
        public Guid WorkerId { get; set; }
        public UspWorker Worker { get; set; }
        public string SewerGroup { get; set; } //Borrar más adelante

        // Type E
        public decimal HoursNormal { get; set; }
        public decimal HoursSunday { get; set; }
        public decimal HoursMedicalRest { get; set; }
        public decimal HoursPaternalLeave { get; set; }
        public decimal HoursHoliday { get; set; }
        public decimal HoursExtra60 { get; set; }
        public decimal HoursExtra100 { get; set; }
        public decimal HoursPaidLeave { get; set; }
        public decimal DaysWorked { get; set; }
        public decimal DaysGratification { get; set; }
        public decimal DaysVacations { get; set; }
        public decimal DaysCts { get; set; }
        public decimal DaysAttended { get; set; }
        public decimal DaysMedicalLeave { get; set; }
        public decimal DaysUnattended { get; set; }
        public decimal DaysLaboralSuspension { get; set; }
        public decimal DaysNoPaidLeave { get; set; }

        // Type R
        public decimal Salary { get; set; }
        public decimal Sunday { get; set; }
        public decimal SchoolAssignment { get; set; }
        public decimal Holidays { get; set; }
        public decimal MedicalRest { get; set; }
        public decimal PaternalLeave { get; set; }
        public decimal ExtraHours60 { get; set; }
        public decimal ExtraHours100 { get; set; }
        public decimal Mobility { get; set; }
        public decimal Gratification { get; set; }
        public decimal ExtraordinaryBonification { get; set; }
        public decimal Vacations { get; set; }
        public decimal Cts { get; set; }
        public decimal BUC { get; set; }
        public decimal PaidLeave { get; set; }
        public decimal MedicalLeave { get; set; }
        public decimal CteHe { get; set; }
        public decimal RegularBonification { get; set; }

        // Type D
        public decimal Onp { get; set; }
        public decimal AfpFund { get; set; }
        public decimal AfpFlowCommission { get; set; }
        public decimal AfpMixedCommission { get; set; }
        public decimal AfpDisabilityInsurance { get; set; }
        public decimal Conafovicer { get; set; }
        public decimal FifthCategoryTaxes { get; set; }
        public decimal JudicialRetention { get; set; }
        public decimal UnionFee { get; set; }

        // Type A
        public decimal EsSalud { get; set; }
        public decimal AfpEarlyRetirement { get; set; }
        public decimal SctrHealth { get; set; }
        public decimal SctrPension { get; set; }
        public decimal EsSaludMasVida { get; set; }

        // Type T
        public decimal TotalRem { get; set; }
        public decimal TotalDis { get; set; }
        public decimal TotalCon { get; set; }
        public decimal TotalCos { get; set; }
        public decimal TotalNet { get; set; }

        // Type E
        public decimal TotalEsSaludAffected { get; set; }
        public decimal TotalFifthRentAffected { get; set; }
        public decimal TotalOnpAffected { get; set; }
        public decimal TotalAfpAffected { get; set; }
        public decimal TotalJudicialAffected { get; set; }
    }
}
