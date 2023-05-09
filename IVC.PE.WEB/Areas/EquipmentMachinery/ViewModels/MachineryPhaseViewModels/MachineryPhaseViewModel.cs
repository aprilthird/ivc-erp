using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.MachineryPhaseViewModels
{
    public class MachineryPhaseViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public Guid? ProjectId { get; set; }
    }
}
