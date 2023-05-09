using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class ProviderSupplyFamily
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }

        public Guid SupplyFamilyId { get; set; }
        public SupplyFamily SupplyFamily { get; set; }
    }
}
