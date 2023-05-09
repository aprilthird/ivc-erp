using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
   public class PermissionRenovationApplicationUser
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PermissionRenovationId { get; set; }
        public PermissionRenovation PermissionRenovation { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
