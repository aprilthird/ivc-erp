using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessParticipationFoldingViewModels
{
    public class BusinessParticipationFoldingViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Empresa", Prompt = "Empresa")]
        public Guid BusinessId { get; set; }

        public BusinessViewModel Business { get; set; }

        [Display(Name = "% de Participacion de IVC", Prompt = "% de Participacion de IVC")]
        public double IvcParticipation { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        public Uri TestimonyUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo Testimonio", Prompt = "Archivo Testimonio")]
        public IFormFile FileTestimony { get; set; }

        public int Order { get; set; }
    }
}
