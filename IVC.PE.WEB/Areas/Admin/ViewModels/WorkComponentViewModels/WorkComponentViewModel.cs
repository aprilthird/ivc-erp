using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkComponentViewModels
{
    public class WorkComponentViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ProjectId { get; set; }
        public ProjectViewModel ProjectViewModel { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Name { get; set; }

        [Display(Name = "Color", Prompt = "Color")]
        public int DocStyle { get; set; }
        public string DocStyleStr => ConstantHelpers.PillStyle.VALUES[DocStyle];
    }
}
