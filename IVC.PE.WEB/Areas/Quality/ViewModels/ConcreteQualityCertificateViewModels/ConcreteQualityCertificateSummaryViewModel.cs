using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.ConcreteQualityCertificateViewModels
{
    public class ConcreteQualityCertificateSummaryViewModel
    {
        public SewerBoxViewModel SewerBox { get; set; }

        public int? MaxSlabAge { get; set; }

        public int? MaxBodyAge { get; set; }
        
        public int? MaxRoofAge { get; set; }
    }
}
