using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels
{
    public class WorkFrontHeadViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Jefe", Prompt = "Jefe")]
        public string UserId { get; set; }

        public UserViewModel User { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid? ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }
    }
}
