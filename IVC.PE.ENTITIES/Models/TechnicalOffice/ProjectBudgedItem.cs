using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ProjectBudgedItem
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Code { get; set; }

        public string MeasurementUnit { get; set; }

        public float Measure { get; set; }

        public double UnitPrice { get; set; }

        [NotMapped]

        public double TotalPrice => Measure * UnitPrice;

        public Guid? ProjectBudgetCategoryId { get; set; }

        public ProjectBudgetCategory ProjectBudgetCategory { get; set; }

        public Guid? ProjectBudgetItemParent { get; set; }

        public ProjectBudgedItem ProjectBudgedItemParent { get; set; }

        public IEnumerable<ProjectBudgedItem> ProjectBudgetItemChildrens { get; set; }
    }
}
