using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ProjectPhase
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid? ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        public IEnumerable<WorkFrontProjectPhase> WorkFronts { get; set; }
    }
}
