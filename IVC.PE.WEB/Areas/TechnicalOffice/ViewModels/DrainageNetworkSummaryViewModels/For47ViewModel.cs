using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DrainageNetworkSummaryViewModels
{
    public class For47ViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Porcentaje Normal", Prompt = "Porcentaje Normal")]
        public double ExcavationLengthPercentForNormal { get; set; }

        [Display(Name = "Porcentaje Rocoso", Prompt = "Porcentaje Rocoso")]
        public double ExcavationLengthPercentForRocky { get; set; }

        [Display(Name = "Porcentaje Semirocoso", Prompt = "Porcentaje Semirocoso")]
        public double ExcavationLengthPercentForSemirocous { get; set; }
    }
}
