using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class BehaviourReportDetail
    {
        public Guid Id { get; set; }
        public Guid BehaviourReportId { get; set; }
        public BehaviourReport BehaviourReport { get; set; }
        public Guid BehaviourReportItemId { get; set; }
        public BehaviourReportItem BehaviourReportItem { get; set; }
        public Guid BehaviourReportCauseId { get; set; }
        public BehaviourReportCause BehaviourReportCause { get; set; }
    }
}
