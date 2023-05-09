using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
  public class EquipmentMachSOATFolding
    {
        public Guid Id { get; set; }

        public Guid EquipmentMachId { get; set; }

        public EquipmentMach EquipmentMach { get; set; }

        public DateTime? StartDateSOAT { get; set; }

        public DateTime? EndDateSOAT { get; set; }

        public Uri SOATFileUrl { get; set; }

        public int SoatOrder { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;

    }
}
