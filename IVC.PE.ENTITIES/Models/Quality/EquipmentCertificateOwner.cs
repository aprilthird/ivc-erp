using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class EquipmentCertificateOwner
    {
        public Guid Id { get; set; }

        public string OwnerName { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }


    }
}
