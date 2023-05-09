using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ShedulerF7ViewModels
{
    public class ShedulerF7ViewModel
    {
        [Display(Name = "Cuadrillas", Prompt = "Cruadrillas")]
        public IEnumerable<Guid> SewerGroupShedulers { get; set; }

        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public string StartDate { get; set; }

        [Display(Name = "Fecha de Fin", Prompt = "Fecha de Fin")]
        public string EndDate { get; set; }

    }
}
