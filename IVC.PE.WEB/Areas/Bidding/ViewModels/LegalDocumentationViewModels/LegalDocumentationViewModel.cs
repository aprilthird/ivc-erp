using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationViewModels
{
    public class LegalDocumentationViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Entidad", Prompt = "Entidad")]
        public Guid BusinessId { get; set; }

        public BusinessViewModel Business { get; set; }
        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public Guid LegalDocumentationTypeId { get; set; }
        public LegalDocumentationTypeViewModel LegalDocumentationType { get; set; }

        public int NumberOfRenovations { get; set; }

        public LegalDocumentationRenovationViewModel LegalDocumentationRenovation { get; set; }
        public IEnumerable<LegalDocumentationRenovationViewModel> LegalDocumentationRenovations { get; set; }

    }
}
