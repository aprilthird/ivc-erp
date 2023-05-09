using System;
using System.Collections.Generic;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkFront
    {
        public Guid Id { get; set; }

        public SystemPhase SystemPhase { get; set; }

        public Guid? SystemPhaseId { get; set; }

        public string Code { get; set; }

        //public string Class { get; set; }

        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }

        public IEnumerable<WorkFrontProjectPhase> ProjectPhases { get; set; }
        public IEnumerable<WorkFrontSewerGroup> SewerGroupPeriods { get; set; }
    }
}
