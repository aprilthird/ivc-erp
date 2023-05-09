using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcessViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcessTypeDocumentViewModels
{
    public class ProcessTypeDocumentViewModel
    {
        public Guid? Id { get; set; }

        public Guid ProcessId { get; set; }
        public ProcessViewModel Process { get; set; }
        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public string DocumentType { get; set; }
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }
    }
}
