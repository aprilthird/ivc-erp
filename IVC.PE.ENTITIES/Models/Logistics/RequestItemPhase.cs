using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class RequestItemPhase
    {
        public Guid RequestItemId { get; set; }

        public RequestItem RequestItem { get; set; }

        public Guid ProjectPhaseId { get; set; }

        public ProjectPhase ProjectPhase { get; set; }
    }
}
