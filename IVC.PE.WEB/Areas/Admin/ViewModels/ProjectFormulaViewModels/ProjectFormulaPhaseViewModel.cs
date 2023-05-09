using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels
{
    public class ProjectFormulaPhaseViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Fases", Prompt = "Fases")]
        public IEnumerable<Guid> PhaseIds { get; set; }
    }
}
