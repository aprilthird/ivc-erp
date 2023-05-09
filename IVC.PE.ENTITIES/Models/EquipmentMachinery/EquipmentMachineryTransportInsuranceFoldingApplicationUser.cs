using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachineryTransportInsuranceFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachineryTransportInsuranceFoldingId { get; set; }
        public EquipmentMachineryTransportInsuranceFolding EquipmentMachineryTransportInsuranceFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
