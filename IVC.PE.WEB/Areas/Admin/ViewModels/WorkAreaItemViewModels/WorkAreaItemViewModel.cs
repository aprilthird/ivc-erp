using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkAreaItemViewModels
{
    public class WorkAreaItemViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        [Display(Name = "Área de Trabajo", Prompt = "Área de Trabajo")]
        public Guid WorkAreaId { get; set; }

        public WorkAreaViewModel WorkArea { get; set; }

        [Display(Name = "Es agrupador", Prompt = "Es agrupador")]
        public bool IsItemGroup { get; set; }

        [Display(Name = "Controlador", Prompt = "Controlador")]
        public string Controller { get; set; }

        [Display(Name = "Acción", Prompt = "Acción")]
        public string Action { get; set; }

        [Display(Name = "Elemento Padre", Prompt = "Elemento Padre")]
        public Guid? ParentId { get; set; }

        public WorkAreaItemViewModel Parent { get; set; }

        [Display(Name = "Rol Relacionado", Prompt = "Rol Relacionado")]
        public string RoleId { get; set; }
    }
}
