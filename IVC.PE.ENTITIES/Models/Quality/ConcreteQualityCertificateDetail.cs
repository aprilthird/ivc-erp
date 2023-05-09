using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class ConcreteQualityCertificateDetail
    {
        public Guid Id { get; set; }

        public Guid ConcreteQualityCertificateId { get; set; }

        public ConcreteQualityCertificate ConcreteQualityCertificate { get; set; }

        public int Segment { get; set; } = ConstantHelpers.Certificate.ConcreteQuality.Segment.SLAB;

        public int SegmentNumber { get; set; } = 1;

        public Guid SewerBoxId { get; set; }

        public SewerBox SewerBox { get; set; }
    }
}
