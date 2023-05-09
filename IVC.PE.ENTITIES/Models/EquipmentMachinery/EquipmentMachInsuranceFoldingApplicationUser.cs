using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachInsuranceFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachInsuranceFoldingId { get; set; }
        public EquipmentMachInsuranceFolding EquipmentMachInsuranceFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
