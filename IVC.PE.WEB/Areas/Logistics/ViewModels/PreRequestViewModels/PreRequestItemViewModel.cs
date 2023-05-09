using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PreRequestViewModels
{
    public class PreRequestItemViewModel
    {
        public Guid? Id { get; set; }

        public Guid PreRequestId { get; set; }
        public PreRequestViewModel PreRequest { get; set; }

        [Display(Name = "Insumos", Prompt = "Insumos")]
        public Guid SupplyId { get; set; }
        public SupplyViewModel Supply { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Unidad", Prompt = "Unidad")]
        public string MeasurementUnitName { get; set; }

        [Display(Name = "Grupo", Prompt = "Grupo")]
        public string SupplyGroupStr { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public double Measure { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public double MeasureInAttention { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public double MeasureToAttent { get; set; }

        [Display(Name = "Para ser usado en:", Prompt = "Para ser usado en:")]
        public string UsedFor { get; set; }

        [Display(Name = "Observaciones", Prompt = "Observaciones")]
        public string Observations { get; set; }

        [Display(Name = "Ingreo Insumo Manual", Prompt = "Ingreo Insumo Manual")]
        public string SupplyName { get; set; }

        public bool SupplyManual { get; set; }

        public string Validator { get; set; }
    }
}
