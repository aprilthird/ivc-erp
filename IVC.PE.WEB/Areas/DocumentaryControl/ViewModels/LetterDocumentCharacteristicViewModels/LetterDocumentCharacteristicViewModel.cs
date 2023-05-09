using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.CORE.Helpers;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterDocumentCharacteristicViewModels
{
    public class LetterDocumentCharacteristicViewModel
    {
        public Guid? Id { get; set; }

        public Guid? ProjectId { get; set; }
        public ProjectViewModel ProjectViewModel { get; set; }


        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Color", Prompt = "Color")]
        public int DocStyle { get; set; }
        public string DocStyleStr => ConstantHelpers.PillStyle.VALUES[DocStyle];
    }
}
