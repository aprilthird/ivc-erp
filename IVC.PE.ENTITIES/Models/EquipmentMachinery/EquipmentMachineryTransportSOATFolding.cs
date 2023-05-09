using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTransportSOATFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachineryTransportId { get; set; }

        public EquipmentMachineryTransport EquipmentMachineryTransport { get; set; }

        public DateTime? StartDateSOAT { get; set; }

        public DateTime? EndDateSOAT { get; set; }

        public Uri SOATFileUrl { get; set; }

        public int SoatOrder { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;
    }
}
