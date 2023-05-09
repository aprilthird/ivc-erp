using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingBusinessViewModels
{
    public class BiddingBusinessViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Empresa", Prompt = "Empresa")]
        public Guid? BusinessId { get; set; }

        public BusinessViewModel Business { get; set; }
        [Display(Name = "Nombre de Empresa", Prompt = "Nombre de Empresa")]
        public string BiddingBusinessName { get; set; }
        [Display(Name = "RUC", Prompt = "RUC")]

        public string BiddingBusinessRuc { get; set; }
        [Display(Name = "Fecha de creación", Prompt = "Fecha de creación")]
        public string BidddingBusinessCreationDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Representante Legal", Prompt = "Representante Legal")]
        public string LegalRepresentant { get; set; }


        public Uri RucUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Ruc Adjunto", Prompt = "Ruc Adjunto")]
        public IFormFile FileRuc { get; set; }
        public Uri TestimonyUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Testimonio Adjunto", Prompt = "Testimonio Adjunto")]
        public IFormFile FileTestimony { get; set; }

    }
}
