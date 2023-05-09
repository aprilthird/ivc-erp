using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkRole
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<WorkRoleItem> WorkRoleItems { get; set; }
    }
}
