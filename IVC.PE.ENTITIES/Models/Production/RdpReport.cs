using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class RdpReport
    {
        public Guid Id { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
