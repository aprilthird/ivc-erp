using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollVariableViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Required]
        public int Type { get; set; }

        public string TypeName => ConstantHelpers.PayrollVariable.Type.VALUES[Type];

        public string Formula { get; set; }
    }
}
