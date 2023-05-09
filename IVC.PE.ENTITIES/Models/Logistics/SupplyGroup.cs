using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class SupplyGroup
    {
        public Guid Id { get; set; }

        public Guid? SupplyFamilyId { get; set; }

        public SupplyFamily SupplyFamily { get; set; }

        [Required]
        public string Code { get; set; }

        public string Name { get; set; }

        public int? Category { get; set; }

        public IEnumerable<Supply> Supplies { get; set; }
    }
}
