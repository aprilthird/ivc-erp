using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public int? CorrelativeCode { get; set; }

        [Display(Name = "Requerimientos", Prompt = "Requerimientos")]
        public List<Guid> RequestIds { get; set; }


        [Display(Name = "Requerimientos", Prompt = "Requerimientos")]
        public string RequestsName { get; set; }

        public string RequestList { get; set; }

        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid ProviderId { get; set; }

        public ProviderViewModel Provider { get; set; }

        [Display(Name = "N° Cotización", Prompt = "N° Cotización")]
        public string QuotationNumber { get; set; }

        [Display(Name = "Fecha (Opcional)", Prompt = "Fecha (Opcional)")]
        public string Date { get; set; }

        [Display(Name = "Moneda", Prompt = "Moneda")]
        public int Currency { get; set; }

        public string CurrencyStr { get; set; }

        [Display(Name = "Forma de Pago", Prompt = "Forma de Pago")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Tiempo de Entrega", Prompt = "Fecha de Entrega")]
        public string DeliveryTime { get; set; }

        [Display(Name = "Lugar de Entrega", Prompt = "Lugar de Entrega")]
        public Guid? WarehouseId { get; set; }
        public WarehouseViewModel Warehouse { get; set; }

        public string ReviewDate { get; set; }

        public string ApproveDate { get; set; }

        [Display(Name = "Facturar a", Prompt = "Facturar a")]
        public string BillTo { get; set; }

        [Display(Name = "Garantía", Prompt = "Garantía")]
        public string Warranty { get; set; }

        [Display(Name = "Observaciones", Prompt = "Observaciones")]
        public string Observations { get; set; }

        [Display(Name = "Condiciones a Cumplir", Prompt = "Condiciones a Cumplir")]
        public string Conditions { get; set; }

        public Uri PriceFileUrl { get; set; }

        [Display(Name = "Cotización", Prompt = "Cotización")]
        public IFormFile PriceFile { get; set; }

        public Uri SupportFileUrl { get; set; }

        [Display(Name = "Sustento", Prompt = "Sustento")]
        public IFormFile SupportFile { get; set; }

        public Uri PdfFileUrl { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public int Status { get; set; }

        [Display(Name = "Atención", Prompt = "Atención")]
        public int AttentionStatus { get; set; }

        public string CostCenter { get; set; }

        public string Abbreviation { get; set; }

        public string BudgetTitle { get; set; }

        public string CorrelativeCodeStr { get; set; }

        [Display(Name = "Certificados de Calidad", Prompt = "Certificados de Calidad")]
        public bool QualityCertificate { get; set; }

        [Display(Name = "Planos", Prompt = "Planos")]
        public bool Blueprint { get; set; }

        [Display(Name = "Hojas de Seguridad (MSDS)", Prompt = "Hojas de Seguridad (MSDS)")]
        public bool SecurityDocument { get; set; }

        [Display(Name = "Certificados de Calibración", Prompt = "Certificados de Calibración")]
        public bool CalibrationCertificate { get; set; }

        [Display(Name = "Catálogos y Criterios de Almacenamiento", Prompt = "Catálogos y Criterios de Almacenamiento")]
        public bool CatalogAndStorageCriteria { get; set; }

        [Display(Name = "Otros", Prompt = "Otros")]
        public bool Other { get; set; }

        public IEnumerable<RequestsInOrderViewModel> Requests { get; set; }

        public int Type { get; set; }
        public IEnumerable<string> Items { get; set; }

        [Display(Name = "Otros (Descripción)", Prompt = "Otros (Descripción)")]
        public string OtherDescription { get; set; }
        public string Parcial { get; set; }
        public string DolarParcial { get; set; }
        [Display(Name = "Tipo de Cambio", Prompt = "Tipo de Cambio")]
        public string ExchangeRate { get; set; }


        [Display(Name = "Sufijo Correlativo (Opcional)", Prompt = "Sufijo Correlativo (Opcional)")]
        public string CorrelativeCodeSuffix { get; set; }

        public bool CorrelativeCodeManual { get; set; }

        public string IssuedUserId { get; set; }

        public bool Liquidation { get; set; }

        [Display(Name = "Lugar de entrega (Ingreso Libre)", Prompt = "Lugar de entrega (Ingreso Libre)")]
        public string ManualWarehouse { get; set; }

        public bool WarehouseCheckbox { get; set; }
    }

    public class SendOrderObsViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Para")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string Obs { get; set; }

    }
}
