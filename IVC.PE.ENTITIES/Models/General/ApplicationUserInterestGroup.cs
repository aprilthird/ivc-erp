using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ApplicationUserInterestGroup
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid InterestGroupId { get; set; }

        public InterestGroup InterestGroup { get; set; }
    }
}
