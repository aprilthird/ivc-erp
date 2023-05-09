using IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor37AViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.FoldingFor37AViewModels
{
    public class FoldingFor37AViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerManifoldFor37AId { get; set; }
        public SewerManifoldFor37AViewModel SewerManifoldFor37A { get; set; }
        [Display(Name = "Tipo de Soldadura", Prompt = "Tipo de Soldadura")]
        public int WeldingType { get; set; }
        [Display(Name = "N° de Junta", Prompt = "N° de Junta")]
        public int MeetingNumber { get; set; }
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }
    }
}
