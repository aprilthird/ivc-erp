using System;
using IVC.PE.ENTITIES.Models.Bidding;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class LegalDocumentationRenovation
    {
        public Guid Id { get; set; }

        public int LegalDocumentationOrder { get; set; }

        public Guid LegalDocumentationId { get; set; } 
        public LegalDocumentation LegalDocumentation { get; set; }

        public int DaysLimitTerm { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow;

        public bool Days5 { get; set; } = false;

        public Uri FileUrl { get; set; }

        public bool IsTheLast { get; set; }
    }
}
