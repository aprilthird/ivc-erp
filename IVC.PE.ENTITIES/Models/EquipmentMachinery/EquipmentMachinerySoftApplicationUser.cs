using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachinerySoftApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachinerySoftId { get; set; }
        public EquipmentMachinerySoft EquipmentMachinerySoft { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
