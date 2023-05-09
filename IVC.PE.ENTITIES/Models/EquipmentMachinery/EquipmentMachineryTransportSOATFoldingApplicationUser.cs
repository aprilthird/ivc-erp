using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTransportSOATFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachineryTransportSOATFoldingId { get; set; }
        public EquipmentMachineryTransportSOATFolding EquipmentMachineryTransportSOATFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
