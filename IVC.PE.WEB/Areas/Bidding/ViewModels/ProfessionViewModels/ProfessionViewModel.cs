using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionViewModels
{
    public class ProfessionViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Profesión", Prompt = "Profesión")]
        public string Name { get; set; }
    }
}
