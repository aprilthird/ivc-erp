using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class MeasurementUnit
    {
        public Guid Id { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public IEnumerable<Supply> Supplies { get; set; }
    }
}
