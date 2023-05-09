using IVC.PE.ENTITIES.Models.Accounting;
using IVC.PE.WEB.Areas.Accounting.ViewModels.InvoiceViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.SupplyEntryViewModels
{
    public class SupplyEntryViewModel
    {
        public Guid? Id { get; set; }

        public int Status { get; set; }

        [Display(Name = "N° Documento", Prompt = "N° Documento")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Almacén", Prompt = "Almacén")]
        public Guid WarehouseId { get; set; }
        public WarehouseViewModel Warehouse { get; set; }


        [Display(Name = "Fecha de Entrega", Prompt = "Fecha de Entrega")]
        public string DeliveryDate { get; set; }

        [Display(Name = "Guía de Remisión", Prompt = "Guía de Remisión")]

        public string RemissionGuideName { get; set; }

        [Display(Name = "Guía de Remisión", Prompt = "Guía de Remisión")]
        public IFormFile RemissionGuide { get; set; }

        public Uri RemissionGuideUrl { get; set; }

        [Display(Name = "Orden", Prompt = "Orden")]
        public Guid OrderId { get; set; }
        public OrderViewModel Order { get; set; }

        public string Groups { get; set; }
        public double Parcial { get; set; }
        public double DolarParcial { get; set; }
        public string ParcialString { get; set; }
        public string DolarParcialString { get; set; }

        public IEnumerable<string> Items { get; set; }

        public bool IsValued { get; set; }

        public string ValuedMonth { get; set; }

        public string ValuedYear { get; set; }

        public Guid InvoiceId { get; set; }

        public InvoiceViewModel Invoice { get; set; }
    }
}
