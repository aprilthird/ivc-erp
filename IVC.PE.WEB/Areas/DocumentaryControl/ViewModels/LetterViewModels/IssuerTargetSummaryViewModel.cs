using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels
{
    public class IssuerTargetSummaryViewModel
    {
        public Guid? IssuerTargetId { get; set; }

        public string Acronym { get; set; }

        public string Name { get; set; }

        public int TotalCount { get; set; }

        public bool Other { get; set; } = false;
    }
}
