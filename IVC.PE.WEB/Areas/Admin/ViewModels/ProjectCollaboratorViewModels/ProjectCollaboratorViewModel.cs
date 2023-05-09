using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectCollaboratorViewModels
{
    public class ProjectCollaboratorViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Apellido Materno", Prompt = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Apellido Paterno", Prompt = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        public string FullName => $"{PaternalSurname} {MaternalSurname}, {Name}";

        public ProjectViewModel Project { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }
        
        public ProviderViewModel Provider { get; set; }

        [Display(Name = "Proveedor", Prompt = "Proveedor")] 
        public Guid? ProviderId { get; set; }
    }
}
