using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels
{
    public class LegalDocumentationRenovationViewModel
    {
        public Guid? Id { get; set; }

        public int LegalDocumentationOrder { get; set; }
       
        public Guid LegalDocumentationId { get; set; }
        public LegalDocumentationViewModel LegalDocumentation { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public int daysLimitTerm { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string CreateDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha Fin", Prompt = "Fecha Fin")]
        public string EndDate { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]

        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        [Display(Name = "¿Es la última renovación?", Prompt = "¿Es la última renovación?")]
        public bool IsTheLast { get; set; }
        //Id a Renovar
        public Guid? LegalDocumentationRenovationId { get; set; }
    }
}
