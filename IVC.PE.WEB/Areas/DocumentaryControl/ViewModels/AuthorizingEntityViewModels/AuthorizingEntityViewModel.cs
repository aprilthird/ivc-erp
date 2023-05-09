using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AuthorizingEntityViewModels
{
    public class AuthorizingEntityViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }
    }
}
