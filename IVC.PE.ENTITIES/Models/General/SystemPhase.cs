using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class SystemPhase
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
