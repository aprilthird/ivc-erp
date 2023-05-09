using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollMovementDetail
    {
        public Guid Id { get; set; }

        public Guid PayrollMovementHeaderId { get; set; }

        public PayrollMovementHeader PayrollMovementHeader { get; set; }

        public Guid WorkerId { get; set; }

        public Worker Worker { get; set; }

        public Guid PayrollConceptId { get; set; }

        public PayrollConcept PayrollConcept { get; set; }

        public decimal Value { get; set; }
    }
}
