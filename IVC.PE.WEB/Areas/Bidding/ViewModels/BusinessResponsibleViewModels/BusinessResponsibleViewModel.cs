using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.BusinessResponsibleViewModels
{
    public class BusinessResponsibleViewModel
    {
        [Display(Name = "Empresas", Prompt = "Empresas")]
        public Guid BusinessId { get; set; }
        public BusinessViewModel Business { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> Responsibles { get; set; }

        [Display(Name = "¿Se le enviará correo?", Prompt = "¿Se le enviará correo?")]
        public bool SendEmail { get; set; }
    }
}
