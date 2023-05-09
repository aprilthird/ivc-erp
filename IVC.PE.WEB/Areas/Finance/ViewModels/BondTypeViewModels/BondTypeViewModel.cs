using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels
{
    public class BondTypeViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Fianza", Prompt = "Tipo de Fianza")]
        public string Name { get; set; }


    }
}
