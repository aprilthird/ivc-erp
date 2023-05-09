using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.PositionViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalsViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalExperienceFoldingViewModels
{
    public class ProfessionalExperienceFoldingViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Profesional", Prompt = "Profesional")]
        public Guid ProfessionalId { get; set; }

        public ProfessionalViewModel Professional { get; set; }
        [Display(Name = "Numero", Prompt = "Numero")]
        public string Number { get; set; }
        [Display(Name = "Empresa", Prompt = "Empresa")]
        public Guid BusinessId { get; set; }

        public BusinessViewModel Business { get; set; }
        [Display(Name = "Obra", Prompt = "Obra")]
        public Guid BiddingWorkId { get; set; }

        public BiddingWorkViewModel BiddingWork { get; set; }
        [Display(Name = "Cargo Desempeñado", Prompt = "Cargo Desempeñado")]
        public Guid PositionId { get; set; }

        public PositionViewModel Position { get; set; }

        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha de Culminación", Prompt = "Fecha de Culminación")]
        public string EndDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();


        public DateTime StartDateSche { get; set; } 

        public DateTime EndDateSche { get; set; } 

        public int Dif { get; set; }
        [Display(Name = "Observaciones", Prompt = "Observaciones")]
        public string Observations { get; set; }

        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public int Order { get; set; }
    }
}
