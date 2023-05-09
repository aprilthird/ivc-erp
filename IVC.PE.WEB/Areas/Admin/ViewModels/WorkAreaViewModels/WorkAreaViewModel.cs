using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaViewModels
{
    public class WorkAreaViewModel
    {
        public Guid? Id { get; set; }


        [Display(Name = "Nombre del área", Prompt = "Nombre del área")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public int IntValue { get; set; }

        public int Cantidad { get; set; }
    }
}
