using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryResponsibleViewModels
{
    public class EquipmentMachineryResponsibleViewModel
    {
        [Display(Name = "Proyectos", Prompt = "Proyectos")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> Responsibles { get; set; }
    }
}
