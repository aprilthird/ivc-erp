using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.Quality;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IVC.PE.ENTITIES.Models
{
    public static class Extensions
    {
        //public static void Calculate(this SewerBox sewerBox, double? nominalDiameter = null, int? pipelineType = null)
        //{
        //    if(sewerBox != null)
        //    {
        //        sewerBox.Height = sewerBox.Cover - sewerBox.Bottom;
        //        sewerBox.InternalDiameter = sewerBox.Height == 0
        //            ? 0 : sewerBox.Height > 6
        //            ? 1.8 : sewerBox.Height >= 3
        //            ? 1.5 : 1.2;
        //        if(nominalDiameter.HasValue && pipelineType.HasValue)
        //        {
        //            var maxNominalDiameter = sewerBox.InitialSewerLines != null && sewerBox.FinalSewerLines != null
        //                ? Math.Max(nominalDiameter.Value, Math.Max(sewerBox.InitialSewerLines.Max(x => x.NominalDiameter), sewerBox.FinalSewerLines.Max(x => x.NominalDiameter)))
        //                : nominalDiameter.Value;
        //            sewerBox.Type = maxNominalDiameter < 630
        //                ? ConstantHelpers.Sewer.Box.Type.I : maxNominalDiameter > 630
        //                ? ConstantHelpers.Sewer.Box.Type.II : pipelineType.Value == ConstantHelpers.Pipeline.Type.PVC
        //                ? ConstantHelpers.Sewer.Box.Type.I : ConstantHelpers.Sewer.Box.Type.II;
        //            sewerBox.Thickness = sewerBox.Type == ConstantHelpers.Sewer.Box.Type.I
        //                ? 0.2 : sewerBox.Height > 8
        //                ? 0.3 : sewerBox.Height > 5
        //                ? 0.25 : 0.2;
        //        }
        //    }
        //}

        //public static void Calculate(this SewerLine sewerLine, bool calculateSewerBoxes = true)
        //{
        //    if(sewerLine != null)
        //    {
        //        if(calculateSewerBoxes)
        //        {
        //            sewerLine.InitialSewerBox.Calculate(sewerLine.NominalDiameter, sewerLine.PipelineType);
        //            sewerLine.FinalSewerBox.Calculate(sewerLine.NominalDiameter, sewerLine.PipelineType);
        //        }

        //        sewerLine.AverageDepthSewerBox = (
        //            (sewerLine.InitialSewerBox.InputOutput == 0
        //                ? sewerLine.InitialSewerBox.Cover - sewerLine.InitialSewerBox.Bottom
        //                : sewerLine.InitialSewerBox.Cover - sewerLine.InitialSewerBox.InputOutput) +
        //            (sewerLine.FinalSewerBox.InputOutput == 0
        //                ? sewerLine.FinalSewerBox.Cover - sewerLine.FinalSewerBox.Bottom
        //                : sewerLine.FinalSewerBox.Cover - sewerLine.FinalSewerBox.InputOutput)
        //        ) / 2;
        //        sewerLine.AverageDepthSewerLine = sewerLine.AverageDepthSewerBox != 0
        //            ? sewerLine.AverageDepthSewerBox + 0.1f : 0;
        //        sewerLine.TiltedPipelineLengthOnAxis = (float)Math.Sqrt(
        //            Math.Pow(sewerLine.HorizontalDistanceOnAxis, 2) +
        //            Math.Pow(
        //                (sewerLine.InitialSewerBox.InputOutput > 0 ? sewerLine.InitialSewerBox.InputOutput : sewerLine.InitialSewerBox.Bottom) -
        //                (sewerLine.FinalSewerBox.InputOutput > 0 ? sewerLine.FinalSewerBox.InputOutput : sewerLine.FinalSewerBox.Bottom),
        //                2));
        //        sewerLine.InstalledPipelineLength = sewerLine.TiltedPipelineLengthOnAxis -
        //            (sewerLine.InitialSewerBox.InternalDiameter == 1.2
        //                ? 0.6f : sewerLine.InitialSewerBox.InternalDiameter == 1.5
        //                ? 0.75f : 0.9f) -
        //            (sewerLine.FinalSewerBox.InternalDiameter == 1.2
        //                ? 0.6f : sewerLine.FinalSewerBox.InternalDiameter == 1.2
        //                ? 0.75f : 0.9f);
        //        sewerLine.ExcavationLength = sewerLine.InstalledPipelineLength -
        //            (sewerLine.InitialSewerBox.Height > 8
        //                ? 0.3f : sewerLine.InitialSewerBox.Height >= 6
        //                ? 0.25f : 0.2f) -
        //            (sewerLine.FinalSewerBox.Height > 8
        //                ? 0.3f : sewerLine.FinalSewerBox.Height >= 6
        //                ? 0.25f : 0.2f);
        //        sewerLine.Slope = (
        //            (sewerLine.InitialSewerBox.InputOutput > 0 ? sewerLine.InitialSewerBox.InputOutput : sewerLine.InitialSewerBox.Bottom) -
        //            (sewerLine.FinalSewerBox.InputOutput > 0 ? sewerLine.FinalSewerBox.InputOutput : sewerLine.FinalSewerBox.Bottom)
        //            ) * 1000 / sewerLine.HorizontalDistanceOnAxis;
        //        var maxDifference = Math.Max(
        //                sewerLine.InitialSewerBox.InputOutput == 0
        //                    ? sewerLine.InitialSewerBox.Cover - sewerLine.InitialSewerBox.Bottom
        //                    : sewerLine.InitialSewerBox.Cover - sewerLine.InitialSewerBox.InputOutput,
        //                sewerLine.FinalSewerBox.InputOutput == 0
        //                    ? sewerLine.FinalSewerBox.Cover - sewerLine.FinalSewerBox.Bottom
        //                    : sewerLine.FinalSewerBox.Cover - sewerLine.FinalSewerBox.InputOutput
        //                    );
        //        sewerLine.PipelineClass = sewerLine.AverageDepthSewerBox <= 0.1
        //            ? 0 /*-*/ : maxDifference <= 3
        //                ? ConstantHelpers.Pipeline.Class.SN2 : maxDifference <= 5
        //                ? ConstantHelpers.Pipeline.Class.SN4 : maxDifference <= 15
        //                ? ConstantHelpers.Pipeline.Class.SN8 : 0; /*-*/
        //        sewerLine.Piping = false && //si tiene certificado
        //            sewerLine.TerrainType != ConstantHelpers.Terrain.Type.ROCKY
        //            ? sewerLine.AverageDepthSewerBox >= 1.76
        //            ? sewerLine.ExcavationLength : 0 : 0;

        //        if (sewerLine.HasFor47)
        //        {
        //            sewerLine.ExcavationLengthForNormal = sewerLine.ExcavationLength * sewerLine.ExcavationLengthPercentForNormal;
        //            sewerLine.ExcavationLengthForSemirocous = sewerLine.ExcavationLength * sewerLine.ExcavationLengthPercentForSemirocous;
        //            sewerLine.ExcavationLengthForRocky = sewerLine.ExcavationLength * sewerLine.ExcavationLengthPercentForRocky;
        //        }
        //        else
        //        {
        //            switch (sewerLine.TerrainType)
        //            {
        //                case ConstantHelpers.Terrain.Type.NORMAL:
        //                    sewerLine.ExcavationLengthPercentForNormal = 100;
        //                    sewerLine.ExcavationLengthForNormal = sewerLine.ExcavationLength;
        //                    break;
        //                case ConstantHelpers.Terrain.Type.SEMIROCOUS:
        //                    sewerLine.ExcavationLengthPercentForSemirocous = 100;
        //                    sewerLine.ExcavationLengthForSemirocous = sewerLine.ExcavationLength;
        //                    break;
        //                case ConstantHelpers.Terrain.Type.ROCKY:
        //                    sewerLine.ExcavationLengthPercentForRocky = 100;
        //                    sewerLine.ExcavationLengthForRocky = sewerLine.ExcavationLength;
        //                    break;
        //            }
        //        }
        //    }
        //}

        public static CompactionDensityCertificateDetail Calculate(this CompactionDensityCertificateDetail compactionDensityCertificateDetail)
        {
            if(compactionDensityCertificateDetail != null)
            {
                compactionDensityCertificateDetail.DryDensity = compactionDensityCertificateDetail.WetDensity / (1 + compactionDensityCertificateDetail.Moisture);
                if (compactionDensityCertificateDetail.FillingLaboratoryTest != null)
                    compactionDensityCertificateDetail.DensityPercentage = compactionDensityCertificateDetail.DryDensity / compactionDensityCertificateDetail.FillingLaboratoryTest.MaxDensity;
            }
            return compactionDensityCertificateDetail;
        }
    }
}
