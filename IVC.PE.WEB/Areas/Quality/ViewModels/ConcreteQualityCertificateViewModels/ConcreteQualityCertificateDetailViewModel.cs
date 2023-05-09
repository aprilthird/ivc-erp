using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.ConcreteQualityCertificateViewModels
{
    public class ConcreteQualityCertificateDetailViewModel
    {
        public Guid? Id { get; set; }

        public Guid? ConcreteQualityCertificateId { get; set; }

        public ConcreteQualityCertificateViewModel ConcreteQualityCertificate { get; set; }

        [Display(Name = "Estructura", Prompt = "Estructura")]
        public int Segment { get; set; }

        [Display(Name = "N° Estructura", Prompt = "N° Estructura")]
        public int SegmentNumber { get; set; }

        [Display(Name = "Buzón", Prompt = "Buzón")]
        public Guid SewerBoxId { get; set; }
    }
}
