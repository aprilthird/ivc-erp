using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.PdpViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels
{
    public class SewerManifoldViewModel
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        public Guid? ProductionDailyPartId { get; set; }
        public PdpViewModel ProductionDailyPart { get; set; }

        public Guid SewerBoxStartId { get; set; }
        public SewerBoxViewModel SewerBoxStart { get; set; }

        public Guid SewerBoxEndId { get; set; }
        public SewerBoxViewModel SewerBoxEnd { get; set; }

        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Tramo", Prompt = "Tramo")]
        public string Name { get; set; }
        [Display(Name = "h Zanja", Prompt = "h Zanja")]
        public double DitchHeight { get; set; }
        [Display(Name = "% Zanja", Prompt = "% Zanja")]
        public double DitchLevelPercent { get; set; }
        [Display(Name = "DN (mm)", Prompt = "DN (mm)")]
        public double PipeDiameter { get; set; }
        [Display(Name = "Tipo de Tubería", Prompt = "Tipo de Tubería")]
        public int PipeType { get; set; }
        [Display(Name = "Clase", Prompt = "Clase")]
        public string DitchClass { get; set; }
        [Display(Name = "Long. entre Ejes H", Prompt = "Long. entre Ejes H")]
        public double LengthBetweenHAxles { get; set; }
        [Display(Name = "Long. entre Ejes I", Prompt = "Long. entre Ejes I")]
        public double LengthBetweenIAxles { get; set; }
        [Display(Name = "Long. de Excavación Instalada", Prompt = "Long. de Excavación Instalada")]
        public double LengthOfPipeInstalled { get; set; }
        [Display(Name ="Tipo de Terreno", Prompt ="Tipo de Terreno")]
        public int TerrainType { get; set; }
        [Display(Name = "Long. de Excavación", Prompt = "Long. de Excavación")]
        public double LengthOfDigging { get; set; }

        [Display(Name = "Asfalto 2'' (m2)", Prompt = "Asfalto 2'' (m2)")]
        public string Pavement2In { get; set; }
        [Display(Name = "Asfalto 3'' (m2)", Prompt = "Asfalto 3'' (m2)")]
        public string Pavement3In { get; set; }
        [Display(Name = "Asfalto Mixto 3'' (m2)", Prompt = "Asfalto Mixto 3'' (m2)")]
        public string Pavement3InMixed { get; set; }
        [Display(Name = "Ancho", Prompt = "Ancho")]
        public string PavementWidth { get; set; }

        public int ProcessType { get; set; } //0: Projecto, 1:Replanteo, 2:Ejecución
    }

    public class SewerManifoldFor01ViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        public Guid SewerBoxStartId { get; set; }
        public SewerBoxViewModel SewerBoxStart { get; set; }

        public Guid SewerBoxEndId { get; set; }
        public SewerBoxViewModel SewerBoxEnd { get; set; }

        public double LengthBetweenHAxles { get; set; }
        public double LengthBetweenIAxles { get; set; }

        public double LengthOfPipelineInstalled { get; set; }
        public double DitchHeight { get; set; }
        public double DitchLevelPercent { get; set; }

        public int ProcessType { get; set; } //0: Projecto, 1:Replanteo, 2:Ejecución
    }

    public class SewerManifoldSewerGroupScheduleViewModel
    {
        public double LengthOfDigging { get; set; }
        public string LengthOfDiggingStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthOfDigging);
        public int TerrainType { get; set; }
        public string TerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[TerrainType];
        public double DitchHeight { get; set; }
        public string DitchHeightStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", DitchHeight);
        public double LengthOfPipeInstalled { get; set; }
        public string LengthOfPipeInstalledStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthOfPipeInstalled);
        public double NumberOfLayers { get; set; }
        public string NumberOfLayersStr => string.Format(CultureInfo.InvariantCulture, "{0:,0}", NumberOfLayers);
        public double LengthOfFilling => LengthOfDigging * NumberOfLayers;
        public string LengthOfFillingStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthOfFilling);

        public double SewerBoxStartHeight { get; set; }
        public string SewerBoxStartHeightStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", SewerBoxStartHeight);
        public int SewerBoxStartTerrainType { get; set; }
        public string SewerBoxStartTerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[SewerBoxStartTerrainType];

        public double SewerBoxEndHeight { get; set; }
        public string SewerBoxEndHeightStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", SewerBoxEndHeight);
        public int SewerBoxEndTerrainType { get; set; }
        public string SewerBoxEndTerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[SewerBoxEndTerrainType];
    }
}
