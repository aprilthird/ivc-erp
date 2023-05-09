using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Quality;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerLine
    {
        public Guid Id { get; set; }

        [Required]
        public string Address { get; set; }

        public Guid SewerGroupId { get; set; }

        public SewerGroup SewerGroup { get; set; }

        public Guid QualificationZoneId { get; set; }

        public QualificationZone QualificationZone { get; set; }

        public Guid InitialSewerBoxId { get; set; }

        public SewerBox InitialSewerBox { get; set; }

        public Guid FinalSewerBoxId { get; set; }

        public SewerBox FinalSewerBox { get; set; }

        public int Stage { get; set; } = ConstantHelpers.Stage.CONTRACTUAL;

        public string DrainageArea { get; set; }

        public double AverageDepthSewerBox { get; set; }

        public double AverageDepthSewerLine { get; set; }

        public double TiltedPipelineLengthOnAxis { get; set; }

        public double HorizontalDistanceOnAxis { get; set; }

        public double InstalledPipelineLength { get; set; }

        public double ExcavationLength { get; set; }

        public double Slope { get; set; }

        public double NominalDiameter { get; set; }

        public int PipelineType { get; set; }

        public int PipelineClass { get; set; }

        public double Piping { get; set; }

        public int TerrainType { get; set; } = ConstantHelpers.Terrain.Type.NORMAL;

        public bool HasFor47 { get; set; } = false;

        public double ExcavationLengthPercentForNormal { get; set; }

        public double ExcavationLengthPercentForRocky { get; set; }

        public double ExcavationLengthPercentForSemirocous { get; set; }

        public double ExcavationLengthForNormal { get; set; }

        public double ExcavationLengthForRocky { get; set; }

        public double ExcavationLengthForSemirocous { get; set; }

        public bool AddedLately { get; set; }

        public bool IsReviewed { get; set; }

        [NotMapped]
        public int Layers => (int)Math.Ceiling(((AverageDepthSewerLine * 100) - (NominalDiameter / 10) - 40) / 30);

        [NotMapped]
        public string Name => $"Bz. {InitialSewerBox?.Code} al {FinalSewerBox?.Code}";

        public CompactionDensityCertificate CompactionDensityCertificate { get; set; }
    }
}
