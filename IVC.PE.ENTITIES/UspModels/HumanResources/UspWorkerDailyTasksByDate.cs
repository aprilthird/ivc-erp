using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerDailyTasksByDate
    {
        public string WorkerDocument { get; set; }
        public string WorkerFullName { get; set; }
        public int WorkerCategory { get; set; }
        public string WorkerCategoryStr => ConstantHelpers.Worker.Category.SHORT_VALUES[WorkerCategory];
        public string ProjectPhaseCode { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public string WorkFrontHeadCode { get; set; }
        public decimal HoursNormal { get; set; }
        public decimal Hours60Percent { get; set; }
        public decimal Hours100Percent { get; set; }
        public bool MedicalLeave { get; set; }
        public bool UnPaidLeave { get; set; }
        public bool LaborSuspension { get; set; }
        public bool NonAttendance { get; set; }
        public decimal HoursPaidLeave { get; set; }
        public decimal HoursHoliday { get; set; }
        public decimal HoursMedicalRest { get; set; }
        public string ForemanFullName { get; set; }
    }
}
