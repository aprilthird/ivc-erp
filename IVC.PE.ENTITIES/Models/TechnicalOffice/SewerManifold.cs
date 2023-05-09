using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Production;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerManifold
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? ProductionDailyPartId { get; set; }
        public ProductionDailyPart ProductionDailyPart { get; set; }

        public Guid? SewerBoxStartId { get; set; }
        public SewerBox SewerBoxStart { get; set; }

        public Guid? SewerBoxEndId { get; set; }
        public SewerBox SewerBoxEnd { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }
        public double DitchHeight { get; set; }
        public double DitchLevelPercent { get; set; }
        public double PipelineDiameter { get; set; }
        public int PipelineType { get; set; }
        public int PipelineClass { get; set; }
        public double LengthBetweenHAxles { get; set; }
        public double LengthBetweenIAxles { get; set; }
        public double LengthOfPipelineInstalled { get; set; }
        public int TerrainType { get; set; }
        public double LengthOfDigging { get; set; }

        public double Pavement2In { get; set; }
        public double Pavement3In { get; set; }
        public double Pavement3InMixed { get; set; }
        public double PavementWidth { get; set; }

        public int ProcessType { get; set; }

        public bool HasFor01 { get; set; }
        public bool HasFor05 { get; set; }
        public bool HasFor29 { get; set; }
        public bool HasFor37A { get; set; }
        public bool HasFor47 { get; set; }
    }
}
