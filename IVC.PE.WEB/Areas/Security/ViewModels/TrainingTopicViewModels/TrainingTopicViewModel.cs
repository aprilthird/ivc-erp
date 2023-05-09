using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Security.ViewModels.TrainingCategoryViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.ViewModels.TrainingTopicViewModels
{
    public class TrainingTopicViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }
        
        public TrainingCategoryViewModel TrainingCategory { get; set; }

        [Display(Name = "Categoría", Prompt = "Categoría")]
        public Guid TrainingCategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
    }
}
