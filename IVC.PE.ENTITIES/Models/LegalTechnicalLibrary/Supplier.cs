using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.LegalTechnicalLibrary
{
    public class Supplier
    {
        public Guid Id { get; set; }

        [Required]
        public string RUC { get; set; }

        [Required]
        public string BusinessName { get; set; }

        public int FileCount { get; set; }
    }
}
