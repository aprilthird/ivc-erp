using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DocumentTypeViewModels
{
    public class DocumentTypeViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public string Type { get; set; }
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }
    }
}
