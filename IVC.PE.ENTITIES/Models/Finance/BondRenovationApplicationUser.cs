using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Finance
{
    public class BondRenovationApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid BondRenovationId { get; set; }
        public BondRenovation BondRenovation { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
