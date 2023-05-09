using IVC.PE.ENTITIES.Models.LegalTechnicalLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class ProviderSupplyGroup
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }

        public Guid? SupplyGroupId { get; set; }        
        public SupplyGroup SupplyGroup { get; set; }
    }
}
