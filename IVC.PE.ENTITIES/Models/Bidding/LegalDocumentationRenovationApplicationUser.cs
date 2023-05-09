using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class LegalDocumentationRenovationApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid LegalDocumentationRenovationId { get; set; }

        public LegalDocumentationRenovation LegalDocumentationRenovation { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
