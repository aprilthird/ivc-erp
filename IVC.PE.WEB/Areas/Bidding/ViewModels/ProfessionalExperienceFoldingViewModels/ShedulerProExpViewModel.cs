using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalExperienceFoldingViewModels
{
    public class ShedulerProExpViewModel
    {
        [Display(Name = "Profesional", Prompt = "Profesional")]
        public Guid ProfessionalId { get; set; }

        [Display(Name = "Cargo", Prompt = "Cargo")]
        public Guid PositionId { get; set; }


    }
}
