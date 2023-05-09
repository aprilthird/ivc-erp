using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels
{
    public class LegalDocumentationRenovationApplicationUserViewModel
    {
        public Guid? Id { get; set; }
        public Guid? LegalDocumentationRenovationId { get; set; }
        public LegalDocumentationRenovationViewModel LegalDocumentationRenovation { get; set; }
        public String UserId { get; set; }
    }
}
