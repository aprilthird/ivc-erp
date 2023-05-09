using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTypeViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetViewModels
{
    public class BudgetViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Presupuesto", Prompt = "Tipo de Presupuesto")]
        public Guid? BudgetTypeId { get; set; }
        public BudgetTypeViewModel BudgetType { get; set; }
        public int Type { get; set; }

        [Display(Name = "Agrupación de Presupuesto", Prompt = "Agrupación de Presupuesto")]
        public int Group { get; set; }

        [Display(Name = "Título de Presupuesto", Prompt = "Título de Presupuesto")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Fórmula de Presupuesto", Prompt = "Fórmula de Presupuesto")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }


        [Display(Name = "Item", Prompt = "Item")]
        public string NumberItem { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Unidad")]
        public string Unit { get; set; }

        [Display(Name = "Metrado", Prompt = "Metrado")]
        public string Metered { get; set; }

        [Display(Name = "Precio S/", Prompt = "Precio S/")]
        public string UnitPrice { get; set; }

        //public IFormFile File { get; set; }
        [Display(Name = "Parcial S/", Prompt = "Parcial S/")]
        public string TotalPrice { get; set; }

        [Display(Name = "MO", Prompt = "MO")]
        public string ContractualMO { get; set; }

        [Display(Name = "EQ", Prompt = "EQ")]
        public string ContractualEQ { get; set; }

        [Display(Name = "Subcontrato", Prompt = "SubContrato")]
        public string ContractualSubcontract { get; set; }

        [Display(Name = "Materiales", Prompt = "Materiales")]
        public string ContractualMaterials { get; set; }

        [Display(Name = "MO", Prompt = "MO")]
        public string CollaboratorMO { get; set; }

        [Display(Name = "EQ", Prompt = "EQ")]
        public string CollaboratorEQ { get; set; }
    }
}
