using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels
{
    public class SewerManifoldLetterViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }

        public Guid LetterId { get; set; }
        public LetterViewModel Letter { get; set; }
    }

    public class SewerManifoldLetterListViewModel
    {
        [Display(Name = "Tramo", Prompt = "Tramo")]
        public Guid? SewerManifoldId { get; set; }
        [Display(Name = "Tramos", Prompt = "Tramos")]
        public IEnumerable<Guid> SewerManifoldIds { get; set; }
        [Display(Name ="De Solicitud", Prompt = "De Solicitud")]
        public IEnumerable<Guid> RequestLetterIds { get; set; }
        [Display(Name = "De Aprobación", Prompt = "De Aprobación")]
        public IEnumerable<Guid> ApprovalLetterIds { get; set; }
    }
}
