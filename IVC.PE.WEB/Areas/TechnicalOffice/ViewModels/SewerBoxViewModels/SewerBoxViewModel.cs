using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels
{
    public class SewerBoxViewModel
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Cota Tapa", Prompt = "Cota Tapa")]
        public double CoverLevel { get; set; }
        [Display(Name = "Cota Llegada", Prompt = "Cota Llegada")]
        public double ArrivalLevel { get; set; }
        [Display(Name = "Cota Buzón", Prompt = "Cota Buzón")]
        public double BottomLevel { get; set; }
        [Display(Name = "h BZ", Prompt = "h BZ")]
        public double Height { get; set; }
        [Display(Name = "Tipo de Terreno", Prompt = "Tipo de Terreno")]
        public int TerrainType { get; set; }
        [Display(Name = "Tipo de Buzón", Prompt = "Tipo de Buzón")]
        public int SewerBoxType { get; set; }
        [Display(Name = "Espesor", Prompt = "Espesor")]
        public double Thickness { get; set; }
        [Display(Name = "Diámetro", Prompt = "Diámetro")]
        public double Diameter { get; set; }

        public int ProcessType { get; set; } //0: Projecto, 1:Replanteo, 2:Ejecución

        public Guid? SewerManifoldToPartitionId { get; set; }
    }
}
