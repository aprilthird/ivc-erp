using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetFormulaViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTypeViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ExpensesUtilityViewModels
{
    public class ExpensesUtilityViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Agrupación", Prompt = "Agrupación")]
        public int Group { get; set; }

        [Display(Name = "Titulo", Prompt = "Titulo")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Tipo", Prompt = "Tipo")]
        public Guid? BudgetTypeId { get; set; }

        public BudgetTypeViewModel BudgetType { get; set; }

        [Display(Name = "Gasto General Fijo", Prompt = "Gasto General Fijo")]
        public string FixedGeneralExpense { get; set; }

        [Display(Name = "Gasto General Fijo (%)", Prompt = "Gasto General Fijo (%)")]
        public string FixedGeneralPercentage { get; set; }

        [Display(Name = "Gasto General Variable", Prompt = "Gasto General Variable")]
        public string VariableGeneralExpense { get; set; }

        [Display(Name = "Gasto General Variable (%)", Prompt = "Gasto General Variable (%)")]
        public string VariableGeneralPercentage { get; set; }

        public string TotalGeneralExpense { get; set; }

        public string TotalGeneralPercentage { get; set; }

        [Display(Name = "Utilidad", Prompt = "Utilidad")]
        public string Utility { get; set; }

        [Display(Name = "Utilidad (%)", Prompt = "Utilidad (%)")]
        public string UtilityPercentage { get; set; }
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
