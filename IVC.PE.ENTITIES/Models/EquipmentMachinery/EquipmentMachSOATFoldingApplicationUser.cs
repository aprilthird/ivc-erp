using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachSOATFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachSOATFoldingId { get; set; }
        public EquipmentMachSOATFolding EquipmentMachSOATFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
