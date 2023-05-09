using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingCurrencyTypeViewModels
{
    public class BiddingCurrencyTypeViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Cambio en Soles (S/.)", Prompt = "Cambio en Soles (S/.)")]
        public double Currency { get; set; }
        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string PublicationDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
    }
}
