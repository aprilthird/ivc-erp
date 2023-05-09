using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
    public class UspSewerManifold
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string StartCode { get; set; }
        public double StartCoverLevel { get; set; }
        public string StartCoverLevelStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", StartCoverLevel);
        public double StartArrivalLevel { get; set; }
        public string StartArrivalLevelStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", StartArrivalLevel);
        public double StartBottomLevel { get; set; }
        public string StartBottomLevelStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", StartBottomLevel);
        public double StartHeight { get; set; }
        public string StartHeightStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", StartHeight);
        public int StartTerrainType { get; set; }
        public string StartTerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[StartTerrainType];
        public int StartSewerBoxType { get; set; }
        public string StartSewerBoxTypeStr => ConstantHelpers.Sewer.Box.Type.VALUES[StartSewerBoxType];
        public double StartDiameter { get; set; }
        public string StartDiameterStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", StartDiameter);
        public double StartThickness { get; set; }
        public string StartThicknessStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", StartThickness);
        public string EndCode { get; set; }
        public double EndCoverLevel { get; set; }
        public string EndCoverLevelStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", EndCoverLevel);
        public double EndArrivalLevel { get; set; }
        public string EndArrivalLevelStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", EndArrivalLevel);
        public double EndBottomLevel { get; set; }
        public string EndBottomLevelStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", EndBottomLevel);
        public double EndHeight { get; set; }
        public string EndHeightStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", EndHeight);
        public int EndTerrainType { get; set; }
        public string EndTerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[EndTerrainType];
        public int EndSewerBoxType { get; set; }
        public string EndSewerBoxTypeStr => ConstantHelpers.Sewer.Box.Type.VALUES[EndSewerBoxType];
        public double EndDiameter { get; set; }
        public string EndDiameterStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", EndDiameter);
        public double EndThickness { get; set; }
        public string EndThicknessStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", EndThickness);
        public string Name { get; set; }
        public double DitchHeight { get; set; }
        public string DitchHeightStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", DitchHeight);
        public double DitchLevelPercent { get; set; }
        public string DitchLevelPercentStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", DitchLevelPercent);
        public double PipelineDiameter { get; set; }
        public int PipelineType { get; set; }
        public string PipelineTypeStr => ConstantHelpers.Pipeline.Type.VALUES[PipelineType];
        public int PipelineClass { get; set; }
        public string PipelineClassStr => ConstantHelpers.Pipeline.Class.VALUES[PipelineClass];
        public double LengthBetweenHAxles { get; set; }
        public string LengthBetweenHAxlesStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthBetweenHAxles);
        public double LengthBetweenIAxles { get; set; }
        public string LengthBetweenIAxlesStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthBetweenIAxles);
        public double LengthOfPipelineInstalled { get; set; }
        public string LengthOfPipelineInstalledStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthOfPipelineInstalled);
        public int TerrainType { get; set; }
        public string TerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[TerrainType];
        public double LengthOfDigging { get; set; }
        public string LengthOfDiggingStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", LengthOfDigging);
        public double Pavement2In { get; set; }
        public string Pavement2InStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", Pavement2In);
        public double Pavement3In { get; set; }
        public string Pavement3InStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", Pavement3In);
        public double Pavement3InMixed { get; set; }
        public string Pavement3InMixedStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", Pavement3InMixed);
        public double PavementWidth { get; set; }
        public string PavementWidthStr => string.Format(CultureInfo.InvariantCulture, "{0:,0.00}", PavementWidth);
    }
}
