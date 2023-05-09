using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class EquipmentCertifyingEntity
    {
        public Guid Id { get; set; }
        public string CertifyingEntityName { get; set; }
        public Guid ProjectId { get; set; } 
        
        public Project Project { get; set; }


    }
}
