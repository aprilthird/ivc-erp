using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels
{
    public class OrderItemDiscountViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Dsct Financiero", Prompt = "Dsct Financiero")]
        public string FinancialDiscount { get; set; }

        [Display(Name = "Dsct Item", Prompt = "Dsct Item")]
        public string ItemDiscount { get; set; }

        [Display(Name = "Dsct Adicional", Prompt = "Dsct Adicional")]
        public string AdditionalDiscount { get; set; }

        [Display(Name = "I.G.V.", Prompt = "I.G.V.")]
        public string IGV { get; set; }

        [Display(Name = "I.S.C.", Prompt = "I.S.C.")]
        public string ISC { get; set; }
    }
}
