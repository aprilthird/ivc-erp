using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachinerySoftInsuranceFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachinerySoftInsuranceFoldingId { get; set; }
        public EquipmentMachinerySoftInsuranceFolding EquipmentMachinerySoftInsuranceFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
