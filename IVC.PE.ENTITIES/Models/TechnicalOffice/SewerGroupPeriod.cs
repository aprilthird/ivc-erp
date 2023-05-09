using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerGroupPeriod
    {
        public Guid Id { get; set; }
        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }
        //public Guid? WorkFrontId { get; set; }
        //public WorkFront WorkFront { get; set; }
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
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<WorkFrontSewerGroup> WorkFronts { get; set; }
    }
}
