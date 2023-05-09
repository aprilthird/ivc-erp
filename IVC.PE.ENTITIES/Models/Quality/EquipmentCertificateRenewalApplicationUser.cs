using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class EquipmentCertificateRenewalApplicationUser
    {
        public Guid Id { get; set; }
        [Required]
        public Guid EquipmentCertificateRenewalId { get; set; }
        public EquipmentCertificateRenewal EquipmentCertificateRenewal { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
