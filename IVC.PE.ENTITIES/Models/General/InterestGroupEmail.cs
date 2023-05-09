using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class InterestGroupEmail
    {
        public Guid Id { get; set; }

        public Guid InterestGroupId { get; set; }

        public InterestGroup InterestGroup { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
