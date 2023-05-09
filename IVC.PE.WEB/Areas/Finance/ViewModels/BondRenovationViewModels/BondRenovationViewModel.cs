using IVC.PE.WEB.Areas.Finance.ViewModels.BondAddViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Finance.ViewModels.BondRenovationViewModels
{
    public class BondRenovationViewModel
    {
        public Guid? Id { get; set; }

        public int BondOrder { get; set; }

        [Display(Name = "Número de Carta Fianza", Prompt = "Número de Carta Fianza")]
        public string BondName { get; set; }

        public Guid BondAddId { get; set; }
        public BondAddViewModel BondAdd { get; set; }

        [Display(Name = "Monto", Prompt = "Monto")]
        public double PenAmmount { get; set; }
        public string PenAmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", PenAmmount);

        [Display(Name = "Costo por Emisión", Prompt = "Costo por Emisión")]
        public double IssueCost { get; set; }
        public string IssueCostFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", IssueCost);
        [Display(Name = "Monto US$", Prompt = "Monto US$")]
        public double UsdAmmount { get; set; }

        [Display(Name = "Tipo Moneda", Prompt = "Tipo Moneda")]
        public string currencyType { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public int daysLimitTerm { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string CreateDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha Fin", Prompt = "Fecha Fin")]
        public string EndDate { get; set; }

        [Display(Name = "Contra Garantía", Prompt = "Contra Garantía")]
        public string guaranteeDesc { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> Responsibles { get; set; }

        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Fianza adjunto", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public Uri IssueFileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Costo de Emisión Adjunto", Prompt = "Archivo")]
        public IFormFile IssueFile { get; set; }

        [Display(Name = "¿Es la última renovación?", Prompt = "¿Es la última renovación?")]
        public bool IsTheLast { get; set; }

        //Id de la carta fianza a renovar
        public Guid? BondRenovationId { get; set; }


        /*
        [Display(Name = "Plazo", Prompt = "Plazo")]
        public string daysLimit => (EndDate.Date - CreateDate.Date).TotalDays.ToString();

        [Display(Name = "Dias al vencimiento", Prompt = "Dias al vencimiento")]
        public string daysToEnd => (EndDate.Date - DateTime.UtcNow.Date).TotalDays.ToString();*/
    }
}
