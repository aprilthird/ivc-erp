using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels
{
    public class WorkPositionViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Primer Nombre", Prompt = "Primer Nombre")]
        public string Name { get; set; }

        public int Type { get; set; }
    }
}
