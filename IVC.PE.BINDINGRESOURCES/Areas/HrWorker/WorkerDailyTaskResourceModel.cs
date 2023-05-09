using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.HrWorker
{
    public class WorkerDailyTaskResourceModel
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public string WorkerDocument { get; set; }
        public string WorkerFullName { get; set; }
        public string WorkerCategory { get; set; }
        public Guid ProjectPhaseId { get; set; }
        public string Phase { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroup { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursNormal { get; set; }
        public decimal Hours60Percent { get; set; }
        public decimal Hours100Percent { get; set; }
    }
}
