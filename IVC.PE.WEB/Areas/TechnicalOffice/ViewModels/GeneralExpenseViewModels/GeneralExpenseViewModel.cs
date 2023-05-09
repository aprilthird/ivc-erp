using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.GeneralExpenseViewModels
{
    public class GeneralExpenseViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Título de Presupuesto", Prompt = "Título de Presupuesto")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public string Quantity { get; set; }

        public string Unit { get; set; }

        public string Metered { get; set; }

        public string Price { get; set; }

        public string Parcial { get; set; }
    }
}
