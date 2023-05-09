using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class SupplyFamily
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public int? Category { get; set; }

        public IEnumerable<Supply> Supplies { get; set; }
    }
}
