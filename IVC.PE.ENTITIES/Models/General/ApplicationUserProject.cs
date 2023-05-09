using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ApplicationUserProject
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
