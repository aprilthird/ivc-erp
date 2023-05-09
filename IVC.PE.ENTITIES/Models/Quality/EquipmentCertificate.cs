using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class EquipmentCertificate
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }

        public Guid EquipmentCertificateOwnerId { get; set; }
        public EquipmentCertificateOwner EquipmentCertificateOwner { get; set; }
        public Guid EquipmentCertificateTypeId { get; set; }
        public EquipmentCertificateType EquipmentCertificateType { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public int NumberOfRenovations { get; set; }

        public string Correlative { get; set; }

        public DateTime? EntryDate { get; set; }
    }
}
