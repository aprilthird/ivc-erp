using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class WorkerFixedConcept
    {
        public Guid Id { get; set; }
        public Guid WorkerId { get; set; }
        public Worker Worker { get; set; }
        public Guid PayrollConceptId { get; set; }
        public PayrollConcept PayrollConcept { get; set; }
        public double FixedValue { get; set; }
    }
}
