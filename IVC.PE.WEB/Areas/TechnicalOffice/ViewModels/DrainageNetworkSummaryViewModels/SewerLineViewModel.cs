using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.QualificationZoneViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.CompactionDensityCertificateViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DrainageNetworkSummaryViewModels
{
    public class SewerLineViewModel
    {
        [Display(Name = "Id", Prompt = "Id")]
        public Guid? Id { get; set; }

        [Display(Name = "Etapa", Prompt = "Etapa")]
        public int Stage { get; set; }

        [Display(Name = "Línea", Prompt = "Línea")]
        public Guid? SewerLineId { get; set; }
        
        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }

        public SewerGroupViewModel SewerGroup { get; set; }
        
        [Display(Name = "Habilitación", Prompt = "Habilitación")]
        public Guid QualificationZoneId { get; set; }

        public QualificationZoneViewModel QualificationZone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Área de Drenaje", Prompt = "Área de Drenaje")]
        public string DrainageArea { get; set; }

        [Display(Name = "Área de Drenaje", Prompt = "Área de Drenaje")]
        public string InitialSewerDrainageArea { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Buzón Inicial", Prompt = "Buzón Inicial")]
        public string InitialSewerBoxCode { get; set; }

        [Display(Name = "Media Caña", Prompt = "Media Caña")]
        public bool InitialSewerBoxHalfCane { get; set; }

        [Display(Name = "Alto", Prompt = "Alto")]
        public double InitialSewerBoxHeight { get; set; }
        
        [Display(Name = "Diámetro", Prompt = "Diámetro")]
        public double InitialSewerBoxInternalDiameter { get; set; }

        [Display(Name = "Espesor", Prompt = "Espesor")]
        public double InitialSewerBoxThickness { get; set; }

        [Display(Name = "Tipo de Terreno", Prompt = "Tipo de Terreno")]
        public int InitialSewerBoxTerrainType { get; set; }
        
        [Display(Name = "Tipo de Buzón", Prompt = "Tipo de Búzón")]
        public int InitialSewerBoxType { get; set; }

        [Display(Name = "Área de Drenaje", Prompt = "Área de Drenaje")]
        public string FinalSewerDrainageArea { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Buzón Final", Prompt = "Buzón Final")] 
        public string FinalSewerBoxCode { get; set; }

        [Display(Name = "Media Caña", Prompt = "Media Caña")]
        public bool FinalSewerBoxHalfCane { get; set; }

        [Display(Name = "Alto", Prompt = "Alto")]
        public double FinalSewerBoxHeight { get; set; }

        [Display(Name = "Diámetro", Prompt = "Diámetro")]
        public double FinalSewerBoxInternalDiameter { get; set; }

        [Display(Name = "Espesor", Prompt = "Espesor")]
        public double FinalSewerBoxThickness { get; set; }

        [Display(Name = "Tipo de Terreno", Prompt = "Tipo de Terreno")]
        public int FinalSewerBoxTerrainType { get; set; }

        [Display(Name = "Tipo de Buzón", Prompt = "Tipo de Buzón")]
        public int FinalSewerBoxType { get; set; }

        [Display(Name = "Profundidad Promedio de Buzón", Prompt = "Profundidad Promedio de Buzón")]
        public double AverageDepthSewerBox { get; set; }

        [Display(Name = "Profundidad Promedio de Tramo", Prompt = "Profundidad Promedio de Tramo")]
        public double AverageDepthSewerLine { get; set; }

        [Display(Name = "Tapa", Prompt = "Tapa")]
        public double WaterUpCover { get; set; } = 0;

        [Display(Name = "Salida", Prompt = "Salida")]
        public double WaterUpOutput { get; set; } = 0;

        [Display(Name = "Fondo", Prompt = "Fondo")]
        public double WaterUpBottom { get; set; } = 0;

        [Display(Name = "Tapa", Prompt = "Tapa")]
        public double DownstreamCover { get; set; } = 0;

        [Display(Name = "Entrada", Prompt = "Entrada")]
        public double DownstreamInput { get; set; } = 0;

        [Display(Name = "Fondo", Prompt = "Fondo")]
        public double DownstreamBottom { get; set; } = 0;

        [Display(Name = "Long. Tubería Inclinada / Ejes", Prompt = "Long. Tubería Inclinada / Ejes")]
        public double TiltedPipelineLengthOnAxis { get; set; }

        [Display(Name = "Dist. Horizontal / Ejes", Prompt = "Dist. Horizontal / Ejes")]
        public double HorizontalDistanceOnAxis { get; set; }

        [Display(Name = "Long. Tubería Instalada", Prompt = "Long. Tubería Instalada")]
        public double InstalledPipelineLength { get; set; }

        [Display(Name = "Long. Excavación", Prompt = "Long. Excavación")]
        public double ExcavationLength { get; set; }

        [Display(Name = "Pendiente %o", Prompt = "Pendiente %o")]
        public double Slope { get; set; }

        [Display(Name = "DN (mm)", Prompt = "DN (mm)")]
        public double NominalDiameter { get; set; }

        [Display(Name = "Tipo de Tubería", Prompt = "Tipo de Tubería")]
        public int PipelineType { get; set; }

        [Display(Name = "Clase de Tubería", Prompt = "Clase de Tubería")]
        public int PipelineClass { get; set; }

        [Display(Name = "Entibado (ML)", Prompt = "Entibado (ML)")]
        public double Piping { get; set; }

        [Display(Name = "Tipo de Terreno", Prompt = "Tipo de Terreno")]
        public int TerrainType { get; set; }

        [Display(Name = "For 47", Prompt = "For 47")]
        public bool HasFor47 { get; set; } = false;

        [Display(Name = "Porcentaje Normal", Prompt = "Porcentaje Normal")]
        public double ExcavationLengthPercentForNormal { get; set; }

        [Display(Name = "Porcentaje Rocoso", Prompt = "Porcentaje Rocoso")]
        public double ExcavationLengthPercentForRocky { get; set; }

        [Display(Name = "Porcentaje Semirocoso", Prompt = "Porcentaje Semirocoso")]
        public double ExcavationLengthPercentForSemirocous { get; set; }

        [Display(Name = "Longitud Normal", Prompt = "Longitud Normal")]
        public double ExcavationLengthForNormal { get; set; }

        [Display(Name = "Longitud Rocosa", Prompt = "Longitud Rocosa")]
        public double ExcavationLengthForRocky { get; set; }

        [Display(Name = "Longitud Semirocosa", Prompt = "Longitud Semirocosa")]
        public double ExcavationLengthForSemirocous { get; set; }

        [Display(Name = "Añadido al final", Prompt = "Añadido al final")]
        public bool AddedLately { get; set; }

        public bool IsReviewed { get; set; }

        public int Layers { get; set; }

        public string Name => $"Bz. {InitialSewerBoxCode} al {FinalSewerBoxCode}";

        public CompactionDensityCertificateViewModel CompactionDensityCertificate { get; set; }
    }
}
