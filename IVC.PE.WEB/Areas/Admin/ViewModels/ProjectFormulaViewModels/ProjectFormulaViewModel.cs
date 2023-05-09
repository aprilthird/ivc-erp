using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels
{
    public class ProjectFormulaViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Agrupación", Prompt = "Agrupación")]
        public int Group { get; set; }
    }
}
