using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingCurrencyTypeViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkComponentViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessParticipationFoldingViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BusinessViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkViewModels
{
    public class BiddingWorkViewModel
    {
        public Guid? Id { get; set; }
        
        [Display(Name = "Cambio Dolar", Prompt = "Cambio Dolar")]
        public Guid? BiddingCurrencyTypeId { get; set; }

        public BiddingCurrencyTypeViewModel BiddingCurrencyType { get; set; }
        [Display(Name = "Tipo de cambio", Prompt = "Tipo de cambio")]
        public int CurrencyType { get; set; }
        public string CurrencyTypeDesc => ConstantHelpers.Biddings.CURRENCY[CurrencyType];


        [Display(Name = "Componente de Obra", Prompt = "Componente de Obra")]
        public Guid? BiddingWorkComponentId { get; set; }

        public BiddingWorkComponentViewModel BiddingWorkComponent { get; set; }

        [Display(Name = "N°", Prompt = "N°")]
        public string Number { get; set; }
        [Display(Name = "Nombre de Obra", Prompt = "Nombre de Obra")]
        public string Name { get; set; }
        [Display(Name = "Tipo de Obra", Prompt = "Tipo de Obra")]
        public Guid BiddingWorkTypeId { get; set; }

        public BiddingWorkTypeViewModel BiddingWorkType { get; set; }


        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public DateTime StartDateSche { get; set; } 
        [Display(Name = "Fecha de Finalización", Prompt = "Fecha de Finalización")]
        public DateTime EndDateSche { get; set; } 

        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Fecha de Finalización", Prompt = "Fecha de Finalización")]
        public string EndDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        public int DifDate { get; set; }
        [Display(Name = "Fecha de Recepción", Prompt = "Fecha de Recepción")]
        public string ReceivedDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Fecha de Liquidación", Prompt = "Fecha de Liquidación")]
        public string LiquidationDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Empresa", Prompt = "Empresa")]
        public Guid BusinessId { get; set; }
        public BusinessViewModel Business { get; set; }
        [Display(Name = "Participacion de Empresa", Prompt = "Empresa")]
        public Guid? BusinessParticipationFoldingId { get; set; }
        public BusinessParticipationFoldingViewModel BusinessParticipationFolding { get; set; }
        public Uri ContractUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Contrato de Obra", Prompt = "Archivo Contrato")]
        public IFormFile FileContract { get; set; }

        public Uri ReceivedActUrl { get; set; }
        [Display(Name = "Acta de Recepción", Prompt = "Acta de Recepción")]
        public IFormFile FileReceivedAct { get; set; }

        public Uri LiquidationUrl { get; set; }
        [Display(Name = "Liquidación de Obra", Prompt = "Liquidación de Obra")]
        public IFormFile FileLiquidation { get; set; }

        public Uri InVoiceUrl { get; set; }
        [Display(Name = "Facturas", Prompt = "Facturas")]
        public IFormFile FileInVoice { get; set; }

        public Uri ConfirmedWork { get; set; }
        [Display(Name = "Conformidad de Obra", Prompt = "Conformidad de Obra")]
        public IFormFile FileConfirmedWork { get; set; }

        [Display(Name = "Monto de Liquidación (S/)", Prompt = "Monto de Liquidación")]
        public double? LiquidationAmmount { get; set; }
        [Display(Name = "Monto de Contrato (S/)", Prompt = "Monto de Contrato")]
        public double? ContractAmmount { get; set; }

        [Display(Name = "Monto de Participacion (S/)", Prompt = "Monto de Participacion")]
        public double ParticipationAmmount { get; set; }

        [Display(Name = "Monto de Liquidación Dolares ($)", Prompt = "Monto de Liquidación Dolares ( $ )")]
        public double? LiquidationDollarAmmount { get; set; }
        [Display(Name = "Monto de Contrato Dolares ($)", Prompt = "Monto de Contrato Dolares ($)")]
        public double? ContractDollarAmmount { get; set; }
        
        [Display(Name = "Monto de Participacion Dolares ($)", Prompt = "Monto de Participacion Dolares ($)")]
        public double? ParticipationDollarAmmount { get; set; }

        public string LiquidationAmmountFormated => String.Format(new CultureInfo("es-PE"), "{0:C}", LiquidationAmmount);
        public string LiquidationDollarAmmountFormated => String.Format(new CultureInfo("en-US"), "{0:C}", LiquidationDollarAmmount);

        public string ContractAmmountFormated => String.Format(new CultureInfo("es-PE"), "{0:C}", ContractAmmount);

        public string ParticipationAmmountFormated => String.Format(new CultureInfo("es-PE"), "{0:C}", ParticipationAmmount);
        public string ContractDollarAmmountFormated => String.Format(new CultureInfo("en-US"), "{0:C}", ContractDollarAmmount);

        public string ParticipationDollarAmmountFormated => String.Format(new CultureInfo("en-US"), "{0:C}", ParticipationDollarAmmount);

        [Display(Name = "Componentes", Prompt = "Componentes")]
        public IEnumerable<Guid> BiddingWorkComponents { get; set; }

        public string Components { get; set; }
    }
}
