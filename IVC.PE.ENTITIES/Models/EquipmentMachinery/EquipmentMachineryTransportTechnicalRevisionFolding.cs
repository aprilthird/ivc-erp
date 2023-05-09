using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTransportTechnicalRevisionFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryTransportId { get; set; }

        public EquipmentMachineryTransport EquipmentMachineryTransport { get; set; }

        public DateTime? StartDateTechnicalRevision { get; set; }

        public DateTime? EndDateTechnicalRevision { get; set; }

        public Uri TechnicalRevisionFileUrl { get; set; }

        public int TechnicalOrder { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;

    }
}
