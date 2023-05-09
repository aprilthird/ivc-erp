using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkPosition
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Type { get; set; }
    }
}
