using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels
{
    public class LegalDocumentationTypeViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Licitacion", Prompt = "Tipo de Licitacion")]
        public string Name { get; set; }
    }
}
