using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.LegalTechnicalLibrary.ViewModels
{
    public class SupplierViewModel
    {

        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "RUC", Prompt = "RUC")]
        public string RUC { get; set; }

        [Display(Name = "Razón Social", Prompt = "Razón Social")]
        public string BusinessName{ get; set; }

        public int FileCount { get; set; }
    }
}
