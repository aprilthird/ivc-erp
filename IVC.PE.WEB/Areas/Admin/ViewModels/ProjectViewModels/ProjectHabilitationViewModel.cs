using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels
{
    public class ProjectHabilitationViewModel
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name ="Código", Prompt ="Código")]
        public int LocationCode { get; set; }
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }
    }
}
