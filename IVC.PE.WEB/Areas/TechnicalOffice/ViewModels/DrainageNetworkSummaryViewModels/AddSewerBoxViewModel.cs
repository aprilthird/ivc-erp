using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DrainageNetworkSummaryViewModels
{
    public class AddSewerBoxViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Área de Drenaje", Prompt = "Área de Drenaje")]
        public string DrainageArea { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código de Buzón", Prompt = "Código de Buzón")]
        public string Code { get; set; }

        [Display(Name = "Tapa", Prompt = "Tapa")]
        public double Cover { get; set; } = 0;

        [Display(Name = "Entrada/Salida", Prompt = "Entrada/Salida")]
        public double InputOutput { get; set; } = 0;

        [Display(Name = "Fondo", Prompt = "Fondo")]
        public double Bottom { get; set; } = 0;

        [Display(Name = "Tipo de Terreno", Prompt = "Tipo de Terreno")]
        public int TerrainType { get; set; }

        [Display(Name = "Tipo de Buzón", Prompt = "Tipo de Buzón")]
        public int SewerBoxType { get; set; }
        
        [Display(Name = "Dist. Horizontal / Ejes", Prompt = "Dist. Horizontal / Ejes")]
        public double HorizontalDistanceOnAxis { get; set; }
    }
}
