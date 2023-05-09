using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels
{
    public class EquipmentCertificateRenewalApplicationUserViewModel
    {
        public Guid? Id { get; set; }
        public Guid? EquipmentCertificateRenewalId { get; set; }
        public EquipmentCertificateRenewalViewModel EquipmentCertificateRenewal { get; set; }
        public String UserId { get; set; }
    }
}
