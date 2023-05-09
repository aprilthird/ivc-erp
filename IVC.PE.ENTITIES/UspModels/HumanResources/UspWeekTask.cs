using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWeekTask
    {
        public Guid WorkerId { get; set; }
        //public string SewerGroupCode { get; set; }
        public int CategoryId { get; set; }
        public int NumberOfChildren { get; set; }
        public bool HasUnionFee { get; set; }
        public bool HasSctr { get; set; }
        public int SctrHealthType { get; set; }
        public int SctrPensionType { get; set; }
        public decimal JudicialRetentionFixedAmmount { get; set; }
        public decimal JudicialRetentionPercentRate { get; set; }
        public bool HasWeeklysettlement { get; set; }
        public int LaborRegimen { get; set; }
        public bool HasEPS { get; set; }
        public bool HasEsSaludPlusVida { get; set; }
        public decimal DayWage { get; set; }
        public decimal BUCRate { get; set; }
        public string PensionFundCode { get; set; }
        public decimal FundRate { get; set; }
        public decimal FlowComissionRate { get; set; }
        public decimal MixedComissionRate { get; set; }
        public decimal DisabilityInsuranceRate { get; set; }
        public decimal HoursNormal { get; set; }
        public decimal Hours60Percent { get; set; }
        public decimal Hours100Percent { get; set; }
        public decimal HoursMedicalRest { get; set; }
        public decimal HoursPaternityLeave { get; set; }
        public decimal HoursHoliday { get; set; }
        public decimal HoursPaidLeave { get; set; }
        public int Attendance { get; set; }
        public int LaborSuspension { get; set; }
        public int NonAttendance { get; set; }
        public int UnPaidLeave { get; set; }
        public int MedicalLeave { get; set; }
    }
}
