using IVC.PE.CORE.Helpers;
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

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GoalBudgetInputViewModels
{
    public class GoalBudgetInputViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]

        [Display(Name = "Catálogo de Insumo")]
        public Guid SupplyId { get; set; }

        public SupplyViewModel Supply { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }


        [Display(Name = "Unidad de Medida", Prompt = "Unidad de Medida")]
        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnitViewModel MeasurementUnit { get; set; }

        [Display(Name = "Familia", Prompt = "Familia")]
        public Guid SupplyFamilyId { get; set; }

        public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Display(Name = "Grupo", Prompt = "Grupo")]
        public Guid? SupplyGroupId { get; set; }

        public SupplyGroupViewModel SupplyGroup { get; set; }

        [Display(Name = "P.U.", Prompt = "Precio")]
        public string UnitPrice { get; set; }

        [Display(Name = "Título de Presupuesto", Prompt = "Título de Presupuesto")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Fórmula de Presupuesto", Prompt = "Fórmula de Presupuesto")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        public Guid WorkFrontId { get; set; }

        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Metered { get; set; }

        public string CurrentMetered { get; set; }

        [Display(Name = "Parcial", Prompt = "Parcial")]
        public string Parcial { get; set; }

        public string WarehouseCurrentMetered { get; set; }

        public string AccumulatedRequestItems { get; set; }

        //se debería manejar otro nombre en la variable,
        //pero para q no existan complicaciones se pondrá como auxiliar
        public double MeteredAux { get; set; }
        public double ParcialAux { get; set; }
    }
}
