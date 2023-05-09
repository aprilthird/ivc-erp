using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.PositionViewModels
{
    public class PositionViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Cargo", Prompt = "Cargo")]
        public string Name { get; set; }
    }
}
