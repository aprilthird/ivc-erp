using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.RenovationTypeViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.PermissionRenovationViewModels
{
    public class PermissionRenovationViewModel
    {
        public Guid? Id { get; set; }

        public int Order { get; set; }

        [Display(Name = "# autorización", Prompt = "# autorización")]
        public string AuthorizationNumber { get; set; }

        public Guid PermissionId { get; set; }
        public PermissionViewModel Permission { get; set; }
        [Display(Name = "Tipo de Renovación", Prompt = "Tipo de Renovación")]
        public Guid RenovationTypeId { get; set; }
        public RenovationTypeViewModel RenovationType { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha Fin", Prompt = "Fecha Fin")]
        public string EndDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();


        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> Responsibles { get; set; }

        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        //Id de la carta fianza a renovar
        public Guid? PermissionRenovationId { get; set; }
        [Display(Name = "¿Ultima Renovación?", Prompt = "¿Ultima Renovación?")]
        public bool IsTheLast { get; set; }
    }
}
