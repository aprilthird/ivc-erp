using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessLegalAgentFoldingViewModels
{
    public class BusinessLegalAgentFoldingViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Empresa", Prompt = "Empresa")]
        public Guid BusinessId { get; set; }

        public BusinessViewModel Business { get; set; }
        [Display(Name = "¿Actual?", Prompt = "¿Actual?")]
        public bool IsActive { get; set; }
        [Display(Name = "Desde", Prompt = "Desde")]
        public string FromDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Hasta", Prompt = "Hasta")]
        public string ToDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int Order { get; set; }
        [Display(Name = "Representante Legal", Prompt = "Representante Legal")]
        public string LegalAgent { get; set; }
    }
}
