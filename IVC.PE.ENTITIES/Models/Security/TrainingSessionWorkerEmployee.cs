using IVC.PE.ENTITIES.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Security
{
    public class TrainingSessionWorkerEmployee
    {
        public Guid Id { get; set; }

        public TrainingSession TrainingSession { get; set; }

        public Guid TrainingSessionId { get; set; }

        public TrainingResultStatus TrainingResultStatus { get; set; }
        
        public Guid TrainingResultStatusId { get; set; }

        public Worker Worker { get; set; }

        public Guid? WorkerId { get; set; }

        public Employee Employee { get; set; }

        public Guid? EmployeeId { get; set; }

        public string Observation { get; set; }
    }
}
