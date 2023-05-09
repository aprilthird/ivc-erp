using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.InterestGroupViewModels
{
    public class InterestGroupViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        public ProjectViewModel Project { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        [Display(Name = "Usuario(s)", Prompt = "Usuario(s)")]
        public IEnumerable<String> UserIds { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }
    }
}
