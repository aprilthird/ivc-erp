using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
   public class EquipmentMachTechnicalRevisionFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachTechnicalRevisionFoldingId { get; set; }
        public EquipmentMachTechnicalRevisionFolding EquipmentMachTechnicalRevisionFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
