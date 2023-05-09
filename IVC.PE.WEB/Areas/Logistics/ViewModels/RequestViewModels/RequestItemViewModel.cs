using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.PreRequestViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetInputViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels
{
    public class RequestItemViewModel
    {
        public Guid? Id { get; set; }

        public Guid RequestId { get; set; }
        public RequestInRequestItemsViewModel Request { get; set; }

        [Display(Name = "Insumos", Prompt = "Insumos")]
        public Guid SupplyId { get; set; }
        public SupplyViewModel Supply { get; set; }
        //public Guid GoalBudgetInputId { get; set; }
        //public GoalBudgetInputViewModel GoalBudgetInput { get; set; }

        public Guid? PreRequestItemId { get; set; }
        public PreRequestItemViewModel PreRequestItem { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Unidad", Prompt = "Unidad")]
        public string MeasureUnit { get; set; }

        [Display(Name = "Grupo", Prompt = "Grupo")]
        public string SupplyGroupStr { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Measure { get; set; }

        [Display(Name = "Metrado En Atención", Prompt = "Metrado")]
        public double MeasureInAttention { get; set; }

        public double MeasureToAttent { get; set; }

        [Display(Name = "Para ser usado en:", Prompt = "Para ser usado en:")]
        public string UsedFor { get; set; }

        [Display(Name = "Observaciones", Prompt = "Observaciones")]
        public string Observations { get; set; }
    }
}
