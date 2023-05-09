using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class BudgetFormula
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public string FullName => $"{Code} - {Name}";

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public bool IsDirectCost { get; set; } = true;
    }
}
