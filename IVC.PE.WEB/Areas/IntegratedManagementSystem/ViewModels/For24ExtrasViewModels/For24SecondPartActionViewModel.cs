using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels
{
    public class For24SecondPartActionViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPartViewModel SewerManifoldFor24SecondPart { get; set; }
        [Display(Name = "Acción Tomada", Prompt = "Acción Tomada")]
        public string ActionName { get; set; }
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }
    }
}
