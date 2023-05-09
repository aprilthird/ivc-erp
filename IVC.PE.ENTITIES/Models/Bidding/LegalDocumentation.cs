using System;
using IVC.PE.ENTITIES.Models.Logistics;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class LegalDocumentation
    {
        public Guid Id { get; set; }

        public Guid BusinessId { get; set; }

        public Business Business { get; set; }

        public Guid LegalDocumentationTypeId { get; set; }

        public LegalDocumentationType LegalDocumentationType { get; set; }

        public int NumberOfRenovations { get; set; }
    }
}
