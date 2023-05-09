using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ProjectBudgetCategory
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Code { get; set; }
    }
}
