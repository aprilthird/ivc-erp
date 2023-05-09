using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class MachineryPhase
    {
        public Guid Id { get; set; }

        public Guid ProjectPhaseId {get; set;}

        public ProjectPhase ProjectPhase { get; set; }

        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
