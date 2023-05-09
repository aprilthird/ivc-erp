using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkFrontSewerGroup
    {
        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }
        public Guid SewerGroupPeriodId { get; set; }
        public SewerGroupPeriod SewerGroupPeriod { get; set; }
    }
}
