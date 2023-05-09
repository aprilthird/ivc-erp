using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class SkillRenovationApplicationUser
    { 
        public Guid Id { get; set; }

        [Required]
        public Guid SkillRenovationId { get; set; }

        public SkillRenovation SkillRenovation { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
