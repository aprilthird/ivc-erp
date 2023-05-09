using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerWeeklyTask
    {
        public Guid Id { get; set; }

        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }
        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }
        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }
        public decimal HoursNormal { get; set; }
        public decimal Hours60Percent { get; set; }
        public decimal Hours100Percent { get; set; }
        public decimal HoursMedicalRest { get; set; }
        public bool MedicalLeave { get; set; }
        public decimal HoursPaternityLeave { get; set; }
        public decimal HoursHoliday { get; set; }
        public decimal HoursPaidLeave { get; set; }
        public bool UnPaidLeave { get; set; }
        public bool LaborSuspension { get; set; }
        public bool NonAttendance { get; set; }
        public bool IsCeased { get; set; }
        public DateTime? CeasedDate { get; set; }
    }
}
