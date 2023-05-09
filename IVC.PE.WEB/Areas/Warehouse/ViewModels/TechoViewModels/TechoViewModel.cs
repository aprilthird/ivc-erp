using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.TechoViewModels
{
    public class TechoViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Título de Presupuesto", Prompt = "Título de Presupuesto")]
        public Guid BudgetTitleId { get; set; }
        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Frente de Trabajo", Prompt = "Frente de Trabajo")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        [Display(Name = "Código IVC", Prompt = "Código IVC")]
        public string Code { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Unidad", Prompt = "Unidad")]
        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnitViewModel MeasurementUnit { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Metered { get; set; }

        public string WarehouseCurrentMetered { get; set; }

        public string WarehouseAccumulatedMetered { get; set; }
    }
}
