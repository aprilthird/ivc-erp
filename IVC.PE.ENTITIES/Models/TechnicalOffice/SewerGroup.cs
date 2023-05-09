using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerGroup
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public string Code { get; set; }

        public string Name { get; set; }

        public int Type { get; set; } = ConstantHelpers.Sewer.Group.Type.DRAINAGE; // se va por áreas de trabajo

        public Guid? WorkFrontId { get; set; }

        public WorkFront WorkFront { get; set; }

        public Guid? WorkFrontHeadId { get; set; }

        public WorkFrontHead WorkFrontHead { get; set; }

        public int Destination { get; set; }

        public int WorkComponent { get; set; }

        public int WorkStructure { get; set; }

        public Guid? ProviderId { get; set; }
        public Provider Provider { get; set; }

        public Guid? ProjectCollaboratorId { get; set; }

        public ProjectCollaborator ProjectCollaborator { get; set; }

        public Guid? ForemanWorkerId { get; set; }
        public Worker ForemanWorker { get; set; }

        public Guid? ForemanEmployeeId { get; set; }
        public Employee ForemanEmployee { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<Worker> Workers { get; set; }

        public IEnumerable<BudgetInputAllocationGroup> BudgetInputAllocationGroups { get; set; }
    }
}
