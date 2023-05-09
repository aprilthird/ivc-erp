using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetFormulaViewModels
{
    public class BudgetFormulaViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Codigo", Prompt = "Codigo")]
        public string Code { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Costo Directo", Prompt = "Costo Directo")]
        public bool IsDirectCost { get; set; }

        public string FullName => $"{Code} - {Name}";
    }
}
