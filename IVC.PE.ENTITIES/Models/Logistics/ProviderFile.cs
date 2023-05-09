using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class ProviderFile
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public Uri FileUrl { get; set; }

        public Guid ProviderId { get; set; }

        public Provider Provider { get; set; }
    }
}
