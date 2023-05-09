using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.BiddingWorkTypeViewModels
{
    public class BiddingWorkTypeViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Obra", Prompt = "Tipo de Obra")]
        public string Name { get; set; }

        [Display(Name = "Color", Prompt = "Color")]
        public int PillColor { get; set; }

        public string PillColorStr => ConstantHelpers.PillStyle.VALUES[PillColor];
    }
}
