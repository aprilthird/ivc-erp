using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class EquipmentCertificateType
    {
        public Guid Id { get; set; }
        public string CertificateTypeName { get; set; } 
        public Guid ProjectId { get; set; } 
        public Project Project { get; set; } 
        public int PillColor { get; set; }
    }
}
