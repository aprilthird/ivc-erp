using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PensionFundAdministratorViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Fondo", Prompt = "Fondo")]
        public decimal FundRate { get; set; }

        [Display(Name = "Com. Flujo", Prompt = "Com. Flujo")]
        public decimal FlowComissionRate { get; set; }

        [Display(Name = "Com. Mixta", Prompt = "Com. Mixta")]
        public decimal MixedComissionRate { get; set; }

        [Display(Name = "Prima de Seg.", Prompt = "Prima de Seg.")]
        public decimal DisabilityInsuranceRate { get; set; }
    }
}
