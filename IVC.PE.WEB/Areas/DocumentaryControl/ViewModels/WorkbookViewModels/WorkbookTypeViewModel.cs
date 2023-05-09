using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookViewModels
{
    public class WorkbookTypeViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tipo de Cuaderno de obra", Prompt = "Tipo de Cuaderno de obra")]
        public string Description { get; set; }

        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }
        [Display(Name = "Color", Prompt = "Color")]
        public int Color { get; set; }

        public string ColorStr => ConstantHelpers.PillStyle.VALUES[Color];
    }
}
