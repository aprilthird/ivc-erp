using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SystemPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels
{
    public class WorkFrontViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid? ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }

        [Display(Name = "Fases", Prompt = "Fases")]
        public IEnumerable<Guid> ProjectPhaseIds { get; set; }

        public string FormulaCodes { get; set; }

        [Display(Name = "Formulas", Prompt = "Formulas")]
        public IEnumerable<Guid> FormulaIds { get; set; }
    }
}
