using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerDailyTask
    {
        public Guid Id { get; set; }

        [Required]
        public Guid WorkerId { get; set; }

        public Worker Worker { get; set; }

        public Guid? ProjectPhaseId { get; set; }

        public ProjectPhase ProjectPhase { get; set; }

        [Required]
        public Guid SewerGroupId { get; set; }

        public SewerGroup SewerGroup { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public DateTime Date { get; set; }

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
