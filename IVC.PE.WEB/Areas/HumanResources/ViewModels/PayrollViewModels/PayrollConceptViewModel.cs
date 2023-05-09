using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollConceptViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name ="Descripción", Prompt ="Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Descripción Corta", Prompt = "Descripción Corta")]
        public string ShortDescription { get; set; }

        [Required]
        [Display(Name = "Categoría", Prompt = "Categoría")]
        public int CategoryId { get; set; }

        public string CategoryName => ConstantHelpers.PayrollConcept.Category.VALUES[CategoryId];

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Cod.Plame", Prompt = "Cod.Plame")]
        public string SunatCode { get; set; }

        public IEnumerable<PayrollConceptFormulaViewModel> PayrollConceptFormulas { get; set; }
    }
}
