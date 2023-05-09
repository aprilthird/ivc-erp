using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels
{
    public class SpecialityViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Fórmula", Prompt = "Fórmula")]

        public IEnumerable<Guid> Formulas { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }


    }
}
