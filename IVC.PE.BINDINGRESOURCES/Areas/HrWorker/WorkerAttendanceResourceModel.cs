using IVC.PE.APP.Common.Helpers;
using System;
using System.Collections.Generic;

namespace IVC.PE.BINDINGRESOURCES.Areas.HrWorker
{
    public class WorkerAttendanceResourceModel
    {
        public Guid WorkerId { get; set; }
        public string WorkerFullName { get; set; }
        public string Document { get; set; }
        public int Category { get; set; }
        public string CategoryStr => ConstantHelpers.HrWorker.Category.VALUES[Category];
        public Guid SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public Guid ProjectPhaseId { get; set; }
        public string ProjectPhaseCode { get; set; }
        public bool Attended { get; set; }
        public string AttendedIcon { get; set; }
    }

    public class WorkerAttendanceListResourceModel
    {
        public Guid ProjectId { get; set; }
        public Guid SewerGroupId { get; set; }
        public string TaskDate { get; set; }
        public IEnumerable<WorkerAttendanceResourceModel> WorkerAttendances { get; set; }
    }
}
