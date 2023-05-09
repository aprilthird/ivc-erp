using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVC.PE.ENTITIES.Models.LegalTechnicalLibrary
{
    public class TechnicalLibraryFile
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Code { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public Uri FileUrl { get; set; }

        [Required]
        public int FileType { get; set; }
        
        public Supplier Supplier { get; set; }

        public Guid? SupplierId { get; set; }
    }
}
