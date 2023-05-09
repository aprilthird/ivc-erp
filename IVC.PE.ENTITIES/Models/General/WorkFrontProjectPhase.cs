using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkFrontProjectPhase
    {
        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }
    }
}
