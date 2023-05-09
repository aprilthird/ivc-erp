using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Security.ViewModels.TrainingResultStatusViewModels
{
    public class TrainingResultStatusViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Color", Prompt = "Color")]
        public int Color { get; set; }

        public string ColorStr => ConstantHelpers.Training.ResultStatusColor.VALUES[Color];

        public string ColorClass => ConstantHelpers.Training.ResultStatusColor.CLASSES[Color];
    }
}
