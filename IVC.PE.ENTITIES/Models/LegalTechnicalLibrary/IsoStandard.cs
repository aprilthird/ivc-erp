using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.ENTITIES.Models.LegalTechnicalLibrary
{
    public class IsoStandard
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTime? PublicationDate { get; set; }
        public Uri FileUrl { get; set; }
    }
}
