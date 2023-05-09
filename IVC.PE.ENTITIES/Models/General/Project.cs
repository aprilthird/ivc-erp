using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class Project
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public string CostCenter { get; set; }

        public IEnumerable<ApplicationUserProject> UserProjects { get; set; }

        public Guid? BusinessId { get; set; }

        public Business Business { get; set; }

        public Uri LogoUrl { get; set; }

        public string EstablishmentCode { get; set; }

        public string RucCompany { get; set; }

        public Uri InvoiceSignatureUrl { get; set; }
    }
}
