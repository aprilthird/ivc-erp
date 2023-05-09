using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.ViewModels.TrainingCategoryViewModels
{
    public class TrainingCategoryViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
    }
}
