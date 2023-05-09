using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class ConcreteQualityCertificate
    {
        public Guid Id { get; set; }

        public DateTime SamplingDate { get; set; }

        public DateTime TestDate { get; set; }

        [Required]
        public string CertificateSerialNumber { get; set; }

        [Required]
        public string For07SerialNumber { get; set; }

        public Uri CertificateFileUrl { get; set; }

        public Uri For07FileUrl { get; set; }
        
        public int Age { get; set; } = ConstantHelpers.Certificate.ConcreteQuality.Age.SEVEN;

        [NotMapped]
        public int RealAge => (TestDate - SamplingDate).Days;

        public double FirstResult { get; set; }

        public double SecondResult { get; set; }

        public IEnumerable<ConcreteQualityCertificateDetail> ConcreteQualityCertificateDetails { get; set; }
    }
}
