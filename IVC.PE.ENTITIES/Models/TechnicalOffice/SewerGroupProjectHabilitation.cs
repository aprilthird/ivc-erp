using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerGroupProjectHabilitation
    {
        public Guid Id { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public Guid ProjectHabilitationId { get; set; }
        public ProjectHabilitation ProjectHabilitation { get; set; }
    }
}
