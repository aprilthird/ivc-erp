using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.AreaModuleViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ErpManualViewModels
{
    public class ErpManualViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Módulo", Prompt = "Módulo")]
        public Guid AreaModuleId { get; set; }
        public AreaModuleViewModel AreaModule { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
