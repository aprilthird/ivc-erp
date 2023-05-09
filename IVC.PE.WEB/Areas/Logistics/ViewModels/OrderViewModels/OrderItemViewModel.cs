using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels
{
    public class OrderItemViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Material", Prompt = "Material")]
        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        public Guid OrderId { get; set; }

        public OrderViewModel Order { get; set; }

        public string Glosa { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Measure { get; set; }

        public double MeasureToAttent { get; set; }

        public double MeasureInAttention { get; set; }

        public string UnitPrice { get; set; }

        public string DolarUnitPrice { get; set; }

        public string Parcial { get; set; }

        public string DolarParcial { get; set; }

        public string CorrelativeCode { get; set; }

        public double RequestMeasure { get; set; }

        public double RequestMeasureInAttention { get; set; }

        public string RequestObservation { get; set; }

        public string RequestCode { get; set; }

        public string SupplyEntryCode { get; set; }

        public string MeasureInAttentionString { get; set; }

        public string UnitPriceDiscount { get; set; }

        public OrderItemDiscountViewModel Discount { get; set; }
    }
}
