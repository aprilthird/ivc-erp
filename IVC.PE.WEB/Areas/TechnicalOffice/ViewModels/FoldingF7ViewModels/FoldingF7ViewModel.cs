using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.F7PdpViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.FoldingF7ViewModels
{
    public class FoldingF7ViewModel
    {
        public Guid? Id { get; set; }
        public Guid ProductionDailyPartId { get; set; }
        public F7PdpViewModel ProductionDailyPart { get; set; }
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }
        [Display(Name = "Long. Excavada", Prompt = "Long. Excavada")]
        public string ExcavatedLength { get; set; }
        [Display(Name = "Long. Instalada", Prompt = "Long. Instalada")]
        public string InstalledLength { get; set; }
        [Display(Name = "Longitud Rellenada", Prompt = "Longitud Rellenada")]
        public string RefilledLength { get; set; }
        [Display(Name = "Long. Base Granular", Prompt = "Long. Base Granular")]
        public string GranularBaseLength { get; set; }
        public DateTime CalendarDate { get; set; }
    }
}
