using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.InterestGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.IssuerTargetViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.EmployeeViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels
{
    public class LetterViewModel
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Referencia(s)", Prompt = "Referencia(s)")]
        public IEnumerable<Guid> ReferenceIds { get; set; }

        public IEnumerable<LetterViewModel> References { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Asunto", Prompt = "Asunto")]
        public string Subject { get; set; }

        [Display(Name = "Características del Documento", Prompt = "Características del Documento")]
        public IEnumerable<Guid> Status { get; set; }
    
        [Display(Name = "Plazo de Respuesta (Días)", Prompt = "Plazo")]
        public int? ResponseTermDays { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public Uri FileUrl { get; set; }

        [Display(Name = "Emisor", Prompt = "Emisor")]
        public Guid? IssuerId { get; set; }

        public IssuerTargetViewModel Issuer { get; set; }

        [Display(Name = "Encargado", Prompt = "Encargado")]
        public string EmployeeId { get; set; }

        public UserViewModel Employee { get; set; }
        
        [Display(Name = "Grupos de Interés", Prompt = "Grupos de Interés")]

        public IEnumerable<Guid> InterestGroupIds { get; set; }

        public IEnumerable<InterestGroupViewModel> InterestGroups { get; set; }

        public IEnumerable<Guid> IssuerTargetIds { get; set; }

        public IEnumerable<IssuerTargetViewModel> IssuerTargets { get; set; }
    }
}
