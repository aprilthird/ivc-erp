using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class EquipmentMachineryTransportTechnicalRevisionFoldingApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid EquipmentMachineryTransportTechnicalRevisionFoldingId { get; set; }
        public EquipmentMachineryTransportTechnicalRevisionFolding EquipmentMachineryTransportTechnicalRevisionFolding { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
