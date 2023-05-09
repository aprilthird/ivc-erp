using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachTechnicalRevisionFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachId { get; set; }

        public EquipmentMach EquipmentMach { get; set; }

        public DateTime? StartDateTechnicalRevision { get; set; }

        public DateTime? EndDateTechnicalRevision { get; set; }

        public Uri TechnicalRevisionFileUrl { get; set; }

        public int TechnicalOrder { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;
    }
}
