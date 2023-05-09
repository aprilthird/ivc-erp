using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentLoadViewModel
{
    public class LegalDocumentationLoadViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Entidad", Prompt = "Entidad")]
        public Guid BusinessId { get; set; }

        public BusinessViewModel Business { get; set; }
        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public Guid LegalDocumentationTypeId { get; set; }
        public LegalDocumentationTypeViewModel LegalDocumentationType { get; set; }

        [Display(Name = "Renovación", Prompt = "Renovación")]
        public Guid LegalDocumentationRenovationId { get; set; }
        public LegalDocumentationRenovationViewModel LegalDocumentationRenovation { get; set; }
        
        [Display(Name = "Plazo", Prompt = "Plazo")]
        public int DaysLimitTerm { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;


    }
}
