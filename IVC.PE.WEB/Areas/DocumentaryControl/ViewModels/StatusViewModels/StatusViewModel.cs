using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.StatusViewModels
{
    public class StatusViewModel
    {
        public Guid? Id {get; set;}

        [Display(Name = "Garantes", Prompt = "Garantes")]
        public string Name { get; set; }
    }
}
