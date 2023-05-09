using IVC.PE.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.SigProcessViewModels
{
    public class SigProcessViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Proceso SIG", Prompt = "Proceso SIG")]
        public string Title { get; set; }

        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

       
        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string PublicationDate { get; set; }
    }
}
