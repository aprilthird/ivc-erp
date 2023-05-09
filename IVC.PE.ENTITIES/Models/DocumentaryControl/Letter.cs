using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class Letter
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Subject { get; set; }

        public int Type { get; set; }

        public int ResponseTermDays { get; set; } = ConstantHelpers.Letter.DEFAULT_RESPONSE_TERM_DAYS;

        public Uri FileUrl { get; set; }

        public Guid? IssuerId { get; set; }

        public IssuerTarget Issuer { get; set; }

        public string EmployeeId { get; set; }
        
        public ApplicationUser Employee { get; set; }

        public ICollection<LetterLetter> References { get; set; }

        public ICollection<LetterLetter> ReferencedBy { get; set; }

        public ICollection<LetterStatus> LetterStatus { get; set; }

        public ICollection<LetterInterestGroup> LetterInterestGroups { get; set; }

        public ICollection<LetterIssuerTarget> LetterIssuerTargets { get; set; }
    }
}
