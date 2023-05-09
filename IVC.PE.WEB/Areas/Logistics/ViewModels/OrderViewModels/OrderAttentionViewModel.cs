using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels
{
    public class OrderAttentionViewModel
    {
        public Guid? Id { get; set; }

        public string Abbreviation { get; set; }

        public string CorrelativeCodeStr { get; set; }

        public int Status { get; set; }

        public Guid ProviderId { get; set; }

        public ProviderViewModel Provider { get; set; }

        public string Date { get; set; }

        public string ParcialInAttention { get; set; }

        public string DolarParcialInAttention { get; set; }

        public string Parcial { get; set; }

        public string DolarParcial { get; set; }

        [Display(Name = "Tipo de Cambio", Prompt = "Tipo de Cambio")]
        public string ExchangeRate { get; set; }
        
        public string PercentageAttention { get; set; }

        public string Project { get; set; }

        public string ClosureReason { get; set; }
    }
}
