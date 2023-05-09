using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class BehaviourReportSummary
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string BehviourReportCode { get; set; }
        public int BehviourReportCount { get; set; }
        public string VersionCode { get; set; }
        public int VersionNumber { get; set; }
        public DateTime VersionDate { get; set; }
    }
}
