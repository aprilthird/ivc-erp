using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class LegalDocumentationLoad
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Business Business { get; set; }
        public Guid LegalDocumentationTypeId { get; set; }
        public LegalDocumentationType LegalDocumentationType { get; set; }

        public Guid LegalDocumentationRenovationId { get; set; }
        public LegalDocumentationRenovation LegalDocumentationRenovation { get; set; }
        public int DaysLimitTerm { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    }
}
